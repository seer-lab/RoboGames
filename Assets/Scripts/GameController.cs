using System.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Xml; 
using System.IO; 
using UnityEngine;
using System.Text;
using UnityEngine.Networking;
using System.Runtime.InteropServices;

/// <summary>
/// Oversees the Game Logic such as winning, losing, picking which level to Load. 
/// </summary>
public class GameController : MonoBehaviour, ITimeUser
{
    LevelFactory factory; 
    LevelGenerator lg;
    Output output;
    SidebarController sidebar;
    SelectedTool selectedTool; 
    BackgroundController background; 
    BackButton backButton; 
    EnergyController EnergyController; 
    bool firstUpdate = true; 

    public Logger logger; 
    bool winning = false;
    string webdata;

    #if UNITY_WEBGL && !UNITY_EDITOR
        [DllImport("__Internal")]
        private static extern string GetData(string url);
    #endif

    IEnumerator GetXMLFromServer(string url){
        UnityWebRequest www = UnityWebRequest.Get(url);
        www.SendWebRequest();
        System.Threading.Thread.Sleep(stringLib.DOWNLOAD_TIME);        
        if(www.isNetworkError || www.isHttpError){
            Debug.Log("Error at WEB: " + www.error);
        }else{
            Debug.Log(www.downloadHandler.text);
            webdata = www.downloadHandler.text;
        }
        yield return new WaitForSeconds(0.5f);
    }

    IEnumerator Put(string url, string bodyJsonString)
    {
        //Debug.Log(url);
        byte[] myData = System.Text.Encoding.UTF8.GetBytes(bodyJsonString);
        UnityWebRequest request = UnityWebRequest.Put(url, myData);
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();
    }

    /// <summary>
    /// Checks if the game meets the win condition. 
    /// </summary>
    private void CheckWin()
    {
        if (GlobalState.level != null)
        {
            winning = true;
            //Check if all the tasks have been completed. 
            for (int i = 0; i < 5; i++)
            {
                if (GlobalState.level.Tasks[i] != GlobalState.level.CompletedTasks[i])
                {
                    winning = false;
                }
            }
            if (GlobalState.level.Tasks[0] != 0 && GlobalState.level.Tasks[0] == GlobalState.level.CompletedTasks[0] && GlobalState.GameMode == stringLib.GAME_MODE_BUG)
                winning = true; 
            if (winning)
            {
                StartCoroutine(Win()); 
            }
        }
    }
    /// <summary>
    /// Checks if the Game meets the Lose Condition
    /// </summary>
    private void CheckLose()
    {
        if ((EnergyController.currentEnergy <= 0 && GlobalState.GameMode == stringLib.GAME_MODE_ON)
            || (EnergyController.currentEnergy <= 0|| selectedTool.CheckAllToolsUsed()))
        {
            StartCoroutine(Lose());
        }
    }
    /// <summary>
    /// ITimeUser Callback function
    /// </summary>
    public void OnTimeFinish()
    {
        GameOver(); 
    }

    /// <summary>
    /// Initialize the Timer with the Level's time. 
    /// </summary>
    /// <param name="time"></param>
    private void LoadTimer(float time)
    {
        TimeDisplayController controller = sidebar.timer.GetComponent<TimeDisplayController>();
        controller.EndTime = time;
        controller.Callback = this;
    }

    /// <summary>
    /// Switch to the GAME_END state and load the credits scene. 
    /// </summary>
    public void Victory()
    {
        //logger.onGameEnd(); 
        GlobalState.IsPlaying = false;
        GlobalState.CurrentONLevel = "level5";
        GlobalState.GameState = stateLib.GAMESTATE_GAME_END;
        SceneManager.LoadScene("Credits");
    }

    /// <summary>
    /// Switches to the Level Lose and Loads the Cinematic Scene. 
    /// </summary>
    public void GameOver()
    {
        GlobalState.IsPlaying = false;
        GlobalState.GameState = stateLib.GAMESTATE_LEVEL_LOSE;
        GlobalState.level.NextLevel = GlobalState.level.Failure_Level;
        logger.onGameEnd();
        #if UNITY_WEBGL
            string json = logger.jsonObj;
            StartCoroutine(Put(stringLib.DB_URL + GlobalState.GameMode.ToUpper() + "/" + GlobalState.sessionID.ToString(), json));
        #endif
        SceneManager.LoadScene("Cinematic"); 
    }
    /// <summary>
    /// Delays the Lose Operation to ensure the player isn't about to win 
    /// and then ends the game.
    /// </summary>
    /// <returns></returns>
    IEnumerator Lose()
    {
        CheckWin(); 
        do {
            yield return new WaitForSecondsRealtime(2.7f); 
        }while(GlobalState.GameState != stateLib.GAMESTATE_IN_GAME); 
        GameObject.Find("Fade").GetComponent<Fade>().onFadeOut(); 
        if (!winning)
        {
            if (winning){
                GlobalState.GameState = stateLib.GAMESTATE_LEVEL_WIN;
            logger.onGameEnd();
            #if UNITY_WEBGL
                string json = logger.jsonObj;
                StartCoroutine(Put(stringLib.DB_URL + GlobalState.GameMode.ToUpper() + "/" + GlobalState.sessionID.ToString(), json));
            #endif
            SceneManager.LoadScene("Cinematic", LoadSceneMode.Single); 
            }
            else GameOver();  
        }
    }
    /// <summary>
    /// Delays the Win Operation to ensure the player actually won, then 
    /// completes win game operations. 
    /// </summary>
    /// <returns></returns>
    IEnumerator Win()
    {
        do {
        yield return new WaitForSecondsRealtime(2.2f);
        }while(GlobalState.GameState != stateLib.GAMESTATE_IN_GAME); 
        //if the end of the level string is empty then there is no anticipated next level. 
        Debug.Log(GlobalState.level.NextLevel);
        if (GlobalState.level.NextLevel != Path.Combine(Application.streamingAssetsPath, GlobalState.GameMode + "leveldata"))
        {
            GlobalState.GameState = stateLib.GAMESTATE_LEVEL_WIN;
            logger.onGameEnd();
            #if UNITY_WEBGL
                string json = logger.jsonObj;
                StartCoroutine(Put(stringLib.DB_URL +GlobalState.GameMode.ToUpper()+"/" + GlobalState.sessionID.ToString(), json));
            #endif
            SceneManager.LoadScene("Cinematic", LoadSceneMode.Single); 
        }
        else
        {
            Victory(); 
        }
    }
    /// <summary>
    /// Use this if the level needs to be loaded because of the player warping. 
    /// </summary>
    /// <param name="file">file of the warped level</param>
    /// <param name="line">line number to take the player</param>
    public void WarpLevel(string file, string line)
    {
        #if (UNITY_EDITOR || UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN) && !UNITY_WEBGL
            Debug.Log("GameController: WarpLevel() WINDOWS");
        #endif

        //Want to check if the player is WebGL, and if it is, grab the xml as a string and put it in levelfactory
        #if UNITY_WEBGL && !UNITY_EDITOR
            webdata =GetData(stringLib.SERVER_URL + file);
            Debug.Log("GameController: Warp() WEBGL");
            file = webdata;
        #elif UNITY_WEBGL
            StartCoroutine(GetXMLFromServer(stringLib.SERVER_URL + file));
            Debug.Log("GameController: Warp() WEBGL AND WINDOWS");
            file = webdata;
        #endif

        factory = new LevelFactory(file, true);
        GlobalState.level = factory.GetLevel();
        lg.BuildLevel(true);
        lg.WarpPlayer(line); 
        lg.manager.ResizeObjects(); 
    }
    void Awake(){
        GameObject hero = Instantiate(Resources.Load<GameObject>("Prefabs/Hero"+GlobalState.Character)); 
        hero.name = "Hero"; 
    }
    // Start is called before the first frame update
    void Start()
    {
        GameObject.Find("Fade").GetComponent<Fade>().onFadeIn(); 
        lg = GameObject.Find("CodeScreen").GetComponent<LevelGenerator>();

        string filepath ="";
        #if (UNITY_EDITOR || UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN) && !UNITY_WEBGL
            filepath = Path.Combine(Application.streamingAssetsPath, GlobalState.GameMode + "leveldata");
            if (GlobalState.Language == "python") filepath = Path.Combine(filepath, "python");
            filepath = Path.Combine(filepath, GlobalState.CurrentONLevel);
            Debug.Log("GameController: Start() WINDOWS");
        #endif

        //Want to check if the player is WebGL, and if it is, grab the xml as a string and put it in levelfactory
        #if UNITY_WEBGL && !UNITY_EDITOR
            filepath = "StreamingAssets" + "/" + GlobalState.GameMode + "leveldata/";
            if (GlobalState.Language == "python") filepath += "python/";
            filepath+=GlobalState.CurrentONLevel;
            webdata =GetData(stringLib.SERVER_URL + filepath);
            Debug.Log("GameController: Start() WEBGL");
            filepath = webdata;
        #elif UNITY_WEBGL
            filepath = "StreamingAssets" + "/" + GlobalState.GameMode + "leveldata/";
            if (GlobalState.Language == "python") filepath += "python/";
            filepath+=GlobalState.CurrentONLevel;
            StartCoroutine(GetXMLFromServer(stringLib.SERVER_URL + filepath));
            //Debug.Log("GameController: Start() WEBGL AND WINDOWS");
            filepath = webdata;
        #endif
        factory = new LevelFactory(filepath);
        GlobalState.level = factory.GetLevel();
        backButton = GameObject.Find("BackButton").GetComponent<BackButton>();
        output = GameObject.Find("OutputCanvas").transform.GetChild(0).gameObject.GetComponent<Output>();
        sidebar = GameObject.Find("Sidebar").GetComponent<SidebarController>();
        background = GameObject.Find("BackgroundCanvas").GetComponent<BackgroundController>();
        selectedTool = sidebar.transform.Find("Sidebar Tool").GetComponent<SelectedTool>(); 
        logger = new Logger(); 
        EnergyController = GameObject.Find("Energy").GetComponent<EnergyController>(); 
    }
    /// <summary>
    /// Handles operations regaserding the UI of the game. 
    /// </summary>
    private void HandleInterface()
    {
        if (Input.GetKeyDown(KeyCode.C) && !Output.IsAnswering)
        {
            sidebar.ToggleSidebar(); 
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && !Output.IsAnswering)
        {
            //SaveGameState();
            GlobalState.IsResume = true; 
            firstUpdate = true; 
            GlobalState.GameState = stateLib.GAMESTATE_MENU;
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Additive);
        }
    }
    /// <summary>
    /// Triggers all elements with the Toggle theme capability to 
    /// update their theme. 
    /// </summary>
    private void SetTheme()
    {
        if (!GlobalState.IsDark)
        {
            lg.ToggleLight();
            sidebar.ToggleLight();
            output.ToggleLight();
            background.ToggleLight(); 
            backButton.ToggleColor();
            GlobalState.level.ToggleLight(); 
            lg.DrawInnerXmlLinesToScreen(); 
        } 
        else {
            lg.ToggleDark();
            sidebar.ToggleDark();
            output.ToggleDark();
            background.ToggleDark(); 
            backButton.ToggleColor();
            GlobalState.level.ToggleDark(); 
            lg.DrawInnerXmlLinesToScreen(); 
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (firstUpdate && !GlobalState.IsResume){
            firstUpdate = false; 
            SetTheme(); 
            lg.TransformTextSize(); 
        }
        if (GlobalState.GameState == stateLib.GAMESTATE_IN_GAME)
        {
            CheckWin();
            CheckLose(); 
            HandleInterface(); 
        }
    }
}

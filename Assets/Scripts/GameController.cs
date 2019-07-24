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
    bool calledDead = false;
    GameObject hero;
    EnergyController EnergyController;
    DateTime startDate, endDate;
    bool firstUpdate = true;

    public Logger logger;
    bool winning = false;
    bool finalized = false;
    float leftCodescreen;

    bool hasCalled = false; 

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
                finalized = true; 
                StopCoroutine(Lose());
                StartCoroutine(Win());
            }
        }
    }
    /// <summary>
    /// Checks if the Game meets the Lose Condition
    /// </summary>
    private void CheckLose()
    {
        if (EnergyController.currentEnergy <= 0)
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
        SceneManager.LoadScene("IntroScene");
    }

    /// <summary>
    /// Switches to the Level Lose and Loads the Cinematic Scene. 
    /// </summary>
    public void GameOver()
    {
        GlobalState.IsPlaying = false;
        GlobalState.GameState = stateLib.GAMESTATE_LEVEL_LOSE;
        GlobalState.level.NextLevel = GlobalState.level.Failure_Level;
        //logger.onGameEnd(startDate, false);
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

        do
        {
            yield return new WaitForSecondsRealtime(2.7f);
        } while (GlobalState.GameState != stateLib.GAMESTATE_IN_GAME);
        if (!winning && !finalized)
        {
            //Glitch all of the code wen the player loses by messig up with the 
            // Bug Fixer. (This Solution is based on UNITY bugging out when switching fonts quickly fyi)
            if (GlobalState.GameMode == stringLib.GAME_MODE_BUG){
                TextMesh text = GameObject.Find("Code").GetComponent<TextMesh>(); 
                text.font = Resources.Load<Font>("Fonts/HACKED"); 
                yield return new WaitForSeconds(0.12f); 
                text.font = Resources.Load<Font>("Fonts/CFGlitchCity-Regular"); 
                yield return new WaitForSeconds(0.12f); 
                text.font = Resources.Load<Font>("Fonts/HACKED"); 
                yield return new WaitForSeconds(0.1f); 
                text.text = ""; 
                
            }
            if (!calledDead)
            {
                hero.GetComponent<Animator>().SetTrigger("Dead");
                calledDead = true;
            }
            yield return new WaitForSecondsRealtime(1.5f);
            GameObject.Find("Fade").GetComponent<Fade>().onFadeOut();
            GameOver();
        }
    }
    int CalculateTimeBonus(){
        int time = DateTime.Now.Subtract(startDate).Seconds;
        int value = GlobalState.level.Code.Length*stateLib.POINTS_TIME; 

        value = value - time*stateLib.TIME_DEDUCTION; 

        if (value < 0) value = 0; 
        return value; 
    }
    /// <summary>
    /// Delays the Win Operation to ensure the player actually won, then 
    /// completes win game operations. 
    /// </summary>
    /// <returns></returns>
    IEnumerator Win()
    {
        logger.onGameEnd(startDate, true, EnergyController.currentEnergy);
        GlobalState.timeBonus = logger.CalculateTimeBonus();
        GlobalState.timeBonus = CalculateTimeBonus();
        GlobalState.currentLevelTimeBonus = GlobalState.timeBonus;
        GlobalState.CurrentLevelEnergy = (int)EnergyController.currentEnergy; 
        if (GlobalState.GameMode == stringLib.GAME_MODE_BUG){
            GlobalState.CurrentLevelPoints = stateLib.DEFAULT_BUG_POINTS; 
        }
        GlobalState.RunningScore+= GlobalState.CurrentLevelPoints; 
        //logger.sendPoints();
        do
        {
            yield return new WaitForSecondsRealtime(2.2f);
        } while (GlobalState.GameState != stateLib.GAMESTATE_IN_GAME);
        while(output.Text.text != "" && !GlobalState.level.IsDemo) yield return new WaitForSecondsRealtime(0.5f); 
        //if the end of the level string is empty then there is no anticipated next level. 
        Debug.Log(GlobalState.level.NextLevel);
        if (GlobalState.level.NextLevel != "")
        {
            GlobalState.GameState = stateLib.GAMESTATE_LEVEL_WIN;
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

#if UNITY_WEBGL
            WebHelper.i.url = stringLib.SERVER_URL + file;
            WebHelper.i.GetWebDataFromWeb();
            file = WebHelper.i.webData;
#endif

        factory = new LevelFactory(file, true);
        GlobalState.level = factory.GetLevel();
        lg.BuildLevel(true);
        lg.WarpPlayer(line);
        lg.manager.ResizeObjects();
    }
    void Awake()
    {
        if (GlobalState.level.IsDemo)
        {
            GlobalState.level.IsDemo = true;
            hero = Instantiate(Resources.Load<GameObject>("Prefabs/DemoRobot"));
        }
        else{
            GlobalState.level.IsDemo = false; 
            hero = Instantiate(Resources.Load<GameObject>("Prefabs/Hero"+GlobalState.Character)); 
        }
        hero.name = "Hero"; 
    }
    // Start is called before the first frame update
    void Start()
    {
        GameObject.Find("Fade").GetComponent<Fade>().onFadeIn();
        lg = GameObject.Find("CodeScreen").GetComponent<LevelGenerator>();

        startDate = DateTime.Now;
        GlobalState.CurrentLevelPoints = 0; 

        //GlobalState.level = factory.GetLevel();
        backButton = GameObject.Find("BackButton").GetComponent<BackButton>();
        output = GameObject.Find("OutputCanvas").transform.GetChild(0).gameObject.GetComponent<Output>();
        sidebar = GameObject.Find("Sidebar").GetComponent<SidebarController>();
        background = GameObject.Find("BackgroundCanvas").GetComponent<BackgroundController>();
        selectedTool = sidebar.transform.GetChild(2).transform.Find("Sidebar Tool").GetComponent<SelectedTool>();
        EnergyController = GameObject.Find("Energy").GetComponent<EnergyController>();
        leftCodescreen = GlobalState.StringLib.LEFT_CODESCREEN_X_COORDINATE;
        logger = new Logger();
    }
    /// <summary>
    /// Brings the player back to the main menu. 
    /// The character moves 
    /// </summary>
    public void Escape()
    {
        if (!GlobalState.level.IsDemo)
        {
            //SaveGameState();
            GlobalState.IsResume = true;
            firstUpdate = true;
            GlobalState.GameState = stateLib.GAMESTATE_MENU;
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Additive);
            CodeProperties properties = new CodeProperties();
            hero.GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);
            hero.transform.position = new Vector3(GlobalState.StringLib.LEFT_CODESCREEN_X_COORDINATE + 0.5f, properties.initialLineY, hero.transform.position.z);
            hero.GetComponent<Rigidbody2D>().gravityScale = 0;
        }
        else
        {
            GlobalState.GameState = stateLib.GAMESTATE_LEVEL_WIN;
            logger.onGameEnd(startDate, true, EnergyController.currentEnergy);
            SceneManager.LoadScene("Cinematic", LoadSceneMode.Single);
        }
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
            Escape();
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
            EnergyController.ToggleLight();
            GlobalState.level.ToggleLight();
            lg.DrawInnerXmlLinesToScreen();
        }
        else
        {
            lg.ToggleDark();
            sidebar.ToggleDark();
            output.ToggleDark();
            background.ToggleDark();
            backButton.ToggleColor();
            GlobalState.level.ToggleDark();
            EnergyController.ToggleDark();
            lg.DrawInnerXmlLinesToScreen();
        }
    }
    // Update is called once per frame
    void Update()
    {

        if (GlobalState.DebugMode && Input.GetKeyDown(KeyCode.G)){
            GlobalState.Stats.GrantPower(); 
            Debug.Log("All Powers Maxed Out!"); 
             Debug.Log("XP Boost: " + GlobalState.Stats.XPBoost.ToString() 
            +"\n Speed: " + GlobalState.Stats.Speed.ToString()
            + "\n DamageLevel: " + GlobalState.Stats.DamageLevel.ToString()
             + "\n Energy: " + GlobalState.Stats.Energy.ToString() 
             +"\n Points: " + GlobalState.Stats.Points.ToString()); 

        }
        if (firstUpdate && !GlobalState.IsResume)
        {
            firstUpdate = false;
            SetTheme();
            lg.TransformTextSize();
        }
        if (GlobalState.GameState == stateLib.GAMESTATE_IN_GAME)
        {
            if (!finalized){
                CheckWin();
                CheckLose();
            }
            HandleInterface();
        }
        if (leftCodescreen != GlobalState.StringLib.LEFT_CODESCREEN_X_COORDINATE && GlobalState.GameState == stateLib.GAMESTATE_IN_GAME){
            leftCodescreen = GlobalState.StringLib.LEFT_CODESCREEN_X_COORDINATE; 
            lg.DrawInnerXmlLinesToScreen(); 
        }
    }
}

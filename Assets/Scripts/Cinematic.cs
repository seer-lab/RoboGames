
using System.Linq;
using System;
using System.Transactions;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using UnityEngine.Networking;

public partial class Cinematic : MonoBehaviour
{
    Logger logger;
    LevelFactory factory;
    // This is the text that is displayed at the start of the level (during the "loading screen") prior to playing the level.
    public string introtext = "Level Start Placeholder!";
    // This text basically says "Press Enter to Continue" and is displayed at the bottom of the "Loading Screen" prior to playing the level.
    public string continuetext = "Continue Text Placeholder";
    // This is the text that is displayed at the end of the level (in the "Victory Screen") after playing the level.
    public string endtext = "Winner!\nLevel End Placeholder!";
    public GameObject prompt1, prompt2;
    public GameObject[] stars = new GameObject[5];
    float originalEnergy, totalEnergy;
    private bool cinerun = false;
    private float delaytime = 0f;
    private float delay = 0.1f;
    bool shownResults = false;
    int score;
    bool updatedLevel = false;
    bool showingTime = false;
    bool shownCharacter = false;
    bool hasTimeBonus = true;
    bool entered = false; 
    bool enabledButtons = false;
    int GlobalPoints; //this is purely for UI purposes
    string webdata;
    int maxScore;
    int option = 0; 
    Button[] options; 
    //.................................>8.......................................
    // Use this for initialization
    void Start()
    {
        logger = new Logger(true);
        //Determine the score/points the player should recieve here. 
        //continuetext = stringLib.CONTINUE_TEXT;
        
        continuetext = ""; 
        score = -1;
        if (GlobalState.timeBonus < 0) GlobalState.timeBonus = 0;

        if (GlobalState.level != null && !GlobalState.level.IsDemo)
        {
            score = GlobalState.CurrentLevelPoints;
            originalEnergy = 0;
            if(GlobalState.passed != null){
                if (GlobalState.passed.Contains(GlobalState.level.FileName)){
                    GlobalState.timeBonus /= 10; 
                    score /= 10; 
                }
            
            }else {
                GlobalState.passed = new List<string>();
                GlobalState.passed.Add(GlobalState.level.FileName); 
            }
            for (int i = 0; i < GlobalState.passed.Count; i++){
                Debug.Log(GlobalState.passed[i]); 
            }
            if (GlobalState.GameState == stateLib.GAMESTATE_LEVEL_WIN) GlobalState.passed.Add(GlobalState.level.FileName); 
            totalEnergy = score + GlobalState.timeBonus;
            GlobalPoints = GlobalState.totalPoints + (int)((score + GlobalState.timeBonus));
            // Debug.Log("tE: " + totalEnergy);
            // Debug.Log("gP: " + GlobalPoints);
            // Debug.Log("tB: " + GlobalState.timeBonus);
            // Debug.Log("cP: " + GlobalState.CurrentLevelPoints);
            GlobalState.totalPoints +=(int)((score + GlobalState.timeBonus) * (1 + ((float)GlobalState.CurrentLevelEnergy / (float)GlobalState.Stats.Energy) * GlobalState.Stats.XPBoost));
            GlobalState.Stats.Points += (int)((score + GlobalState.timeBonus) * (1 + ((float)GlobalState.CurrentLevelEnergy / (float)GlobalState.Stats.Energy) * GlobalState.Stats.XPBoost));
            maxScore = 0;
            int[] pointArr;
            if (GlobalState.GameMode == stringLib.GAME_MODE_ON)
            {
                pointArr = new int[] { stateLib.POINTS_BEACON, stateLib.POINTS_QUESTION, stateLib.POINTS_RENAMER, stateLib.POINTS_COMMENT, stateLib.POINTS_UNCOMMENT };
            }
            else
            {
                pointArr = new int[] { stateLib.POINTS_CATCHER, stateLib.POINTS_CHECKER, stateLib.POINTS_WARPER, stateLib.POINTS_COMMENT, stateLib.POINTS_BREAKPOINT };
            }
            for (int i = 0; i < pointArr.Length; i++)
            {
                maxScore += GlobalState.level.Tasks[i] * pointArr[i];
            }
            
        }
        saveScore();
        ShowButtons();
        UpdateLevel();
        //Load the text for the cinematic scene, and load the next scene's data. 
        UpdateText();
        //Fade the scene in. 
        GameObject.Find("Fade").GetComponent<Fade>().onFadeIn();

        if (!GlobalState.IsDark)
        {
            GameObject.Find("BackgroundCanvas").transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/circuit_board_light");
            transform.Find("PressEnter").GetComponent<Text>().color = Color.black;
            transform.Find("Title").GetComponent<Text>().color = Color.black;
        }

        foreach (GameObject star in stars)
        {
            star.GetComponent<Image>().enabled = false;
            star.GetComponent<Animator>().enabled = false;
        }
    }
   
    public void ToggleLight()
    {
        prompt1.GetComponent<Text>().color = Color.black;
        prompt2.GetComponent<Text>().color = Color.black;
    }
    public void ToggleDark()
    {
        prompt1.GetComponent<Text>().color = Color.white;
        prompt2.GetComponent<Text>().color = Color.white;
    }

    /// <summary>
    /// Gets the text for the cinematic.
    /// If this is the first game this function will also get the 
    /// level data.
    /// </summary>
    private void UpdateText()
    {
        if (GlobalState.level == null || GlobalState.level.FileName == GlobalState.CurrentONLevel || GlobalState.GameState == stateLib.GAMESTATE_LEVEL_START)
        {
            UpdateLevel();
        }
        introtext = GlobalState.level.IntroText;
        endtext = GlobalState.level.ExitText;
    }
    /// <summary>
    /// Gets the data for the next level and replaces GlobalState
    /// params with the updated level. All information needed pretaining 
    /// to the level should be obtained before this is called.
    /// </summary>
    private void UpdateLevel()
    {

        string filepath = "";
#if (UNITY_EDITOR || UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN) && !UNITY_WEBGL
        filepath = Path.Combine(Application.streamingAssetsPath, GlobalState.GameMode + "leveldata");
        if (GlobalState.Language == "python") filepath = Path.Combine(filepath, "python");
        filepath = Path.Combine(filepath, GlobalState.CurrentONLevel);
#endif

        //Want to check if the player is WebGL, and if it is, grab the xml as a string and put it in levelfactory

#if UNITY_WEBGL
            filepath = "StreamingAssets" + "/" + GlobalState.GameMode + "leveldata/";
            if (GlobalState.Language == "python") filepath += "python/";
            filepath+=GlobalState.CurrentONLevel;

            WebHelper.i.url = stringLib.SERVER_URL + filepath;
            WebHelper.i.GetWebDataFromWeb();
            filepath = WebHelper.i.webData;
#endif

        updatedLevel = true;
        factory = new LevelFactory(filepath);
        GlobalState.level = factory.GetLevel();
    }
    /// <summary>
    /// Gets the data for the next level and replaces GlobalState
    /// params with the updated level. All information needed pretaining 
    /// to the level should be obtained before this is called.
    /// </summary>
    /// <param name="file">Gets the passed in file and will override the default.</param>
    private void UpdateLevel(string file)
    {
        string[] temp = file.Split('\\');
        GlobalState.CurrentONLevel = temp[temp.Length - 1];

        string filepath = "";
#if (UNITY_EDITOR || UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN) && !UNITY_WEBGL
        filepath = Path.Combine(Application.streamingAssetsPath, GlobalState.GameMode + "leveldata");
        if (GlobalState.Language == "python") filepath = Path.Combine(filepath, "python");
        filepath = Path.Combine(filepath, file);
#endif


#if UNITY_WEBGL
            filepath = "StreamingAssets" + "/" + GlobalState.GameMode + "leveldata/";
            if (GlobalState.Language == "python") filepath += "python/";
            filepath+=GlobalState.CurrentONLevel;

            WebHelper.i.url = stringLib.SERVER_URL + filepath;
            WebHelper.i.GetWebDataFromWeb(true);
            filepath = WebHelper.i.webData;
#endif

        updatedLevel = true;
        factory = new LevelFactory(filepath);
        GlobalState.level = factory.GetLevel();
    }
    /// <summary>
    /// Simulate pressing Enter
    /// </summary>
    public void OnContinue()
    {
        entered = true; 
    }
    /// <summary>
    /// Load Progression Scene
    /// </summary>
    public void OnUpgrade(){
        SceneManager.LoadScene("Progression");
    }
    /// <summary>
    /// Show all buttons but keep upgrade/progression disabled.
    /// </summary>
    void ShowButtons(){
        GameObject upgrade = transform.Find("upgrade").gameObject; 
        GameObject cont = transform.Find("continue").gameObject; 

        upgrade.GetComponent<Image>().enabled = true; 
        upgrade.transform.GetChild(1).GetComponent<Text>().enabled = true; 

        cont.GetComponent<Image>().enabled = false; 
        cont.transform.GetChild(1).GetComponent<Text>().enabled = true; 
        cont.transform.GetChild(0).GetComponent<Image>().enabled = true;
        upgrade.GetComponent<Button>().interactable = false; 
        options = new Button[]{cont.GetComponent<Button>(),upgrade.GetComponent<Button>()};
    }
    /// <summary>
    /// Swaps between the two buttons
    /// </summary>
    void SwapOption(){
        if (option > 0) option = 0; 
        else option = 1; 
    }
    /// <summary>
    /// Enable the progression button to be unlocked
    /// </summary>
    void AllowUpgrade(){
        transform.Find("upgrade").GetComponent<Button>().interactable = true;
        enabledButtons = true;  
    }
    void Update()
    {
        // Exit to the Main Menu 
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GlobalState.GameState = stateLib.GAMESTATE_MENU;
            GlobalState.IsResume = false;
            if (!updatedLevel) UpdateLevel(GlobalState.level.NextLevel);
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
        }
        //Handles when the upcoming level has been loaded. 
        if (GlobalState.GameState == stateLib.GAMESTATE_LEVEL_START)
        {
            if (!cinerun)
            {
                cinerun = true;
                if (GlobalState.Stats.Points > 0)
                    AllowUpgrade();
            }

            if (!shownCharacter)
            {
                shownCharacter = true;
                StartCoroutine(ShowCharacter());
            }

            prompt1.GetComponent<Text>().text = introtext;
            if ((entered|| Input.GetKeyDown(KeyCode.KeypadEnter) ) && delaytime < Time.time)
            {
                entered = false; 
                if (GlobalState.level == null){
                    UpdateLevel(GlobalState.CurrentONLevel); 
                }
                GlobalState.GameState = stateLib.GAMESTATE_IN_GAME;
                cinerun = false;
                StartCoroutine(LoadGame());
            }
            
        }
        //Handles when the player successfully completes a level. 
        else if (GlobalState.GameState == stateLib.GAMESTATE_LEVEL_WIN)
        {
            if (!cinerun)
            {
                
                cinerun = true;
            }
            if (score > 0 && !shownResults)
            {
                shownResults = true;
                StartCoroutine(FadeInResults());
            }
            prompt1.GetComponent<Text>().text = endtext;

            if ((Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter) || entered) && delaytime < Time.time)
            {
                entered = false; 
                GlobalState.GameState = stateLib.GAMESTATE_LEVEL_START;
                delaytime = Time.time + delay; 
                UpdateLevel(GlobalState.level.NextLevel);
                UpdateText();
                if (score > 0)
                {
                    StartCoroutine(FadeOutResults());
                    StartCoroutine(AnimateStars());
    
                }
                cinerun = false;
                logger.sendPoints();
            }
        }
        //Handles when the player fails a level. 
        else if (GlobalState.GameState == stateLib.GAMESTATE_LEVEL_LOSE)
        {
            score = 0;
            StartCoroutine(AnimateStars());
            if (!cinerun)
            {
                cinerun = true;
            }
            prompt1.GetComponent<Text>().text = stringLib.LOSE_TEXT;
            //prompt2.GetComponent<Text>().text = stringLib.RETRY_TEXT;
            if (Input.GetKeyDown(KeyCode.Escape) && delaytime < Time.time)
            {
                prompt2.GetComponent<Text>().text = "";

                cinerun = false;
                GlobalState.GameState = stateLib.GAMESTATE_MENU;
            }
            if ((Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter) ||entered) && delaytime < Time.time)
            {
                entered = false; 
                prompt2.GetComponent<Text>().text = "";
                cinerun = false;
                // One is called Bugleveldata and another OnLevel data.
                // Levels.txt, coding in menu.cs

                string filepath = GlobalState.level.Failure_Level;
                Debug.Log("FailureLevel: " + filepath);
                if (filepath == "Null")
                    UpdateLevel();
                else
                    UpdateLevel(filepath);
                GlobalState.GameState = stateLib.GAMESTATE_LEVEL_START;
                //Debug.Log("LoadingScreen");
            }
        }
        else
        {
            delaytime = Time.time + delay;
        }
        if ((Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow)) && enabledButtons){
            options[option].transform.GetChild(0).GetComponent<Image>().enabled = false; 
            options[option].GetComponent<Image>().enabled = true; 
            SwapOption();
            options[option].GetComponent<Image>().enabled = false; 
            options[option].transform.GetChild(0).GetComponent<Image>().enabled = true; 
        }else if (Input.GetKeyDown(KeyCode.Return) && delaytime < Time.time){
            if (option == 1) OnUpgrade(); 
            else OnContinue();
        }
    }

    //.................................>8.......................................

    public void saveScore(){

        if(GlobalState.Stats == null){
            GlobalState.Stats = new CharacterStats();
        }
        PlayerPrefs.SetInt("totalPoints", GlobalState.totalPoints);
        PlayerPrefs.SetInt("currentPoint", GlobalState.Stats.Points);
        PlayerPrefs.SetFloat("damageUpgrade", GlobalState.Stats.DamageLevel);
        PlayerPrefs.SetFloat("energyUpgrade", GlobalState.Stats.Energy);
        PlayerPrefs.SetFloat("pointUpgrade", GlobalState.Stats.XPBoost);
        PlayerPrefs.SetFloat("speedUpgrade", GlobalState.Stats.Speed);
    }
}

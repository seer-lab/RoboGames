using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        } while (GlobalState.GameState != stateLib.GAMESTATE_IN_GAME || output.Text.text != "");
        if (!winning && !finalized)
        {
            //Glitch all of the code wen the player loses by messig up with the 
            // Bug Fixer. (This Solution is based on UNITY bugging out when switching fonts quickly fyi)
            if (GlobalState.GameMode == stringLib.GAME_MODE_BUG)
            {
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

                GlobalState.failures++; //ADAPTIVE CODE
                int time = DateTime.UtcNow.Subtract(startDate).Seconds;
                GlobalState.elapsedTime += time;

                logger.sendFailure();
            }
            yield return new WaitForSecondsRealtime(1.5f);
            GameObject.Find("Fade").GetComponent<Fade>().onFadeOut();
            GameOver();
        }
    }
    int CalculateTimeBonus()
    {
        int time = DateTime.UtcNow.Subtract(startDate).Seconds;
        int value = GlobalState.level.Code.Length * stateLib.POINTS_TIME;

        value = value - time * stateLib.TIME_DEDUCTION;

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
        Debug.Log("Win function");

        int time = DateTime.UtcNow.Subtract(startDate).Seconds;
        GlobalState.elapsedTime += time;

        GlobalState.previousFilename = GlobalState.CurrentONLevel;
        GlobalState.timeBonus = logger.CalculateTimeBonus();
        GlobalState.timeBonus = CalculateTimeBonus();
        GlobalState.currentLevelTimeBonus = GlobalState.timeBonus;
        GlobalState.CurrentLevelEnergy = (int)EnergyController.currentEnergy;
        if (GlobalState.GameMode == stringLib.GAME_MODE_BUG)
        {
            GlobalState.CurrentLevelPoints = stateLib.DEFAULT_BUG_POINTS;
        }
        GlobalState.RunningScore += GlobalState.CurrentLevelPoints;
        GlobalState.isPassed = true;
        //logger.sendPoints();
        do
        {
            yield return new WaitForSecondsRealtime(2.2f);
        } while (GlobalState.GameState != stateLib.GAMESTATE_IN_GAME);
        while (output.Text.text != "" && !GlobalState.level.IsDemo) yield return new WaitForSecondsRealtime(0.5f);
        //if the end of the level string is empty then there is no anticipated next level. 
        Debug.Log("Next Level: " + GlobalState.level.NextLevel);


        if (GlobalState.level.NextLevel != "")
        {
            if (GlobalState.AdaptiveOffON == 1)
            {


                GlobalState.GameState = stateLib.GAMESTATE_LEVEL_WIN;


                //ADAPTIVE CODE:


                if (GlobalState.levelsDone == 0)
                {
                    string json = GrabMLDataFromDB(stringLib.DB_URL + GlobalState.GameMode.ToUpper() + "/ml/" + GlobalState.courseCode.ToString() + "/" + GlobalState.sessionID.ToString());

                    json = "{\"Items\":" + json + "}";
                    Root rt = JsonConvert.DeserializeObject<Root>(json);
                    GlobalState.dataNames = new List<string>();

                    //Start at 1 because we add in this run later
                    GlobalState.dataSize = 1;

                    for (int i = 0; i < rt.Items.Count; i++)
                    {
                        GlobalState.dataNames.Add(rt.Items[i].students.levels.name);
                        GlobalState.dataSize++;
                    }

                    GlobalState.rawData = new double[GlobalState.dataSize][];
                    GlobalState.rawSize = 0;

                    for (int i = 0; i < rt.Items.Count; i++)
                    {
                        DateTime ts = Convert.ToDateTime(rt.Items[i].students.levels.timeStarted);
                        DateTime te = Convert.ToDateTime(rt.Items[i].students.levels.timeEnded);

                        string et = te.Subtract(ts).TotalSeconds.ToString();

                        GlobalState.rawData[GlobalState.rawSize] = new double[] { Convert.ToDouble(et), Convert.ToDouble(rt.Items[i].students.levels.failedToolUse) };
                        GlobalState.rawSize++;

                    }

                    GlobalState.dataNames.Add(GlobalState.level.FileName);
                }

                GlobalState.dataNames[GlobalState.rawSize] = GlobalState.level.FileName;
                GlobalState.rawData[GlobalState.rawSize] = new double[] { GlobalState.elapsedTime, GlobalState.failedTool };

                Debug.Log(GlobalState.dataNames[GlobalState.dataSize - 1]);
                Debug.Log(GlobalState.rawData[GlobalState.dataSize - 1]);

                Debug.Log("This Level = " + GlobalState.level.FileName);
                string[] intermediateNames = GlobalState.level.FileName.Split('\\');
                int lenOfInter = intermediateNames.Length;
                string levelname = intermediateNames[lenOfInter - 1];
                if (GlobalState.HintMode == 0 && GlobalState.AdaptiveMode > 0)
                {
                    levelname = levelname.Substring(1);
                }
                Debug.Log("Levelname = " + levelname);
                Debug.Log("Old Adaptive Mode = " + GlobalState.AdaptiveMode.ToString());
                if (!(levelname.Contains("tut")))
                {
                    int[] clustering = ClusteringKMeans.KMeansDemo.Cluster(GlobalState.rawData, 3);

                    int count0 = 0;
                    int count1 = 0;
                    int count2 = 0;
                    double fail0 = 0;
                    double fail1 = 0;
                    double fail2 = 0;
                    double time0 = 0;
                    double time1 = 0;
                    double time2 = 0;

                    for (int i = 0; i < GlobalState.dataSize; i++)
                    {
                        if (levelname == GlobalState.dataNames[i])
                        {
                            if (clustering[i] == 0)
                            {
                                count0++;
                                time0 += GlobalState.rawData[i][0];
                                fail0 += GlobalState.rawData[i][1];
                            }
                            else if (clustering[i] == 1)
                            {
                                count1++;
                                time1 += GlobalState.rawData[i][0];
                                fail1 += GlobalState.rawData[i][1];
                            }
                            else
                            {
                                count2++;
                                time2 += GlobalState.rawData[i][0];
                                fail2 += GlobalState.rawData[i][1];
                            }
                        }
                    }

                    double avgT0 = time0 / count0;
                    double avgT1 = time1 / count1;
                    double avgT2 = time2 / count2;

                    double avgF0 = fail0 / count0;
                    double avgF1 = fail1 / count1;
                    double avgF2 = fail2 / count2;

                    Debug.Log("Adaptive Results:");

                    Debug.Log("Group Counts: " + count0.ToString() + " " + count1.ToString() + " " + count2.ToString());
                    Debug.Log("Fail Counts: " + fail0.ToString() + " " + fail1.ToString() + " " + fail2.ToString());
                    Debug.Log("Fail Averages: " + avgF0.ToString() + " " + avgF1.ToString() + " " + avgF2.ToString());
                    Debug.Log("This Player's Group: " + clustering[GlobalState.dataSize-1].ToString());
                    Debug.Log("This Player's Elapsed Time: " + GlobalState.rawData[GlobalState.dataSize-1][0].ToString());
                    Debug.Log("This Player's Tool Failures: " + GlobalState.rawData[GlobalState.dataSize-1][1].ToString());
                    Debug.Log("This Player's Elapsed Time: " + GlobalState.elapsedTime);
                    Debug.Log("This Player's Tool Failures: " + GlobalState.failedTool);

                    int max = 0;
                    int mid = 0;
                    int low = 0;
                    if (avgF0 < avgF1)
                    {
                        if (avgF0 < avgF2)
                        {
                            max = 0;
                            if (avgF1 > avgF2)
                            {
                                mid = 1;
                                low = 2;
                            }
                            else
                            {
                                mid = 2;
                                low = 1;
                            }
                        }
                        else
                        {
                            max = 2;
                            mid = 0;
                            low = 1;
                        }
                    }
                    else if (avgF1 < avgF2)
                    {
                        max = 1;
                        if (avgF0 < avgF2)
                        {
                            mid = 0;
                            low = 2;
                        }
                        else
                        {
                            mid = 2;
                            low = 0;
                        }
                    }
                    else
                    {
                        max = 2;
                        mid = 1;
                        low = 0;
                    }

                    //Logging Done Before Reassigning the Adaptive Mode 
                    logger.onGameEnd(startDate, "Passed", EnergyController.currentEnergy);
                    if (clustering[GlobalState.dataSize-1] == max)
                    {
                        GlobalState.AdaptiveMode = 0;
                    }
                    else if (clustering[GlobalState.dataSize-1] == mid)
                    {
                        GlobalState.AdaptiveMode = 1;
                    }
                    else
                    {
                        GlobalState.AdaptiveMode = 2;
                    }

                    Debug.Log("New Adaptive Mode = " + GlobalState.AdaptiveMode.ToString());
                }
                else
                {
                    Debug.Log("Tutorial level; no ML done");
                }
                
                GlobalState.elapsedTime = 0;
                GlobalState.failures = 0;
                GlobalState.hitByEnemy = 0;
                GlobalState.failedTool = 0;
                SceneManager.LoadScene("Cinematic", LoadSceneMode.Single);
            }
            else
            {
                logger.onGameEnd(startDate, "Passed", EnergyController.currentEnergy);
                GlobalState.GameState = stateLib.GAMESTATE_LEVEL_WIN;
                Debug.Log("ML has been turned off");
                GlobalState.elapsedTime = 0;
                GlobalState.failures = 0;
                GlobalState.hitByEnemy = 0;
                GlobalState.failedTool = 0;
                SceneManager.LoadScene("Cinematic", LoadSceneMode.Single);
            }
        }
        else
        {
            logger.onGameEnd(startDate, "Passed", EnergyController.currentEnergy);
            Victory();
        }
    }

    [Serializable]
    public class Levels
    {
        public string name;
        public string timeStarted;
        public string timeEnded;
        public string failedToolUse;
    }

    [Serializable]
    public class Students
    {
        public Levels levels;
    }

    [Serializable]
    public class Item
    {
        public string _id;
        public string courseCode;
        public Students students;
    }

    [Serializable]
    public class Root
    {
        public List<Item> Items;
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
        Debug.Log(WebHelper.i.url);
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
        else
        {
            GlobalState.level.IsDemo = false;
            hero = Instantiate(Resources.Load<GameObject>("Prefabs/Hero" + GlobalState.Character));
        }
        hero.name = "Hero";
    }
    // Start is called before the first frame update
    void Start()
    {
        GlobalState.foundBug = false;
        GlobalState.isPassed = false;
        GameObject.Find("Fade").GetComponent<Fade>().onFadeIn();
        lg = GameObject.Find("CodeScreen").GetComponent<LevelGenerator>();

        startDate = DateTime.UtcNow;
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
        Debug.Log("Escpae");
        if (!GlobalState.level.IsDemo)
        {
            //SaveGameState();
            Debug.Log("Escpae1");
            GlobalState.IsResume = true;
            firstUpdate = true;
            GlobalState.GameState = stateLib.GAMESTATE_MENU;
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Additive);
            CodeProperties properties = new CodeProperties();
            hero.GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);
            hero.transform.position = new Vector3(GlobalState.StringLib.LEFT_CODESCREEN_X_COORDINATE + 0.5f, properties.initialLineY, hero.transform.position.z);
            hero.GetComponent<Rigidbody2D>().gravityScale = 0;
            logger.onGameEnd(startDate, "Ongoing", EnergyController.currentEnergy);
        }
        else
        {
            Debug.Log("Escpae2");
            GlobalState.GameState = stateLib.GAMESTATE_LEVEL_WIN; //Needs to be this way so that lose animation does not play
            logger.onGameEnd(startDate, "Ongoing", EnergyController.currentEnergy);
            GlobalState.previousFilename = GlobalState.CurrentONLevel;
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
        /*
        if (GlobalState.DebugMode && Input.GetKeyDown(KeyCode.G)){
            GlobalState.Stats.GrantPower(); 
            // Debug.Log("All Powers Maxed Out!"); 
            //  Debug.Log("XP Boost: " + GlobalState.Stats.XPBoost.ToString() 
            // +"\n Speed: " + GlobalState.Stats.Speed.ToString()
            // + "\n DamageLevel: " + GlobalState.Stats.DamageLevel.ToString()
            //  + "\n Energy: " + GlobalState.Stats.Energy.ToString() 
            //  +"\n Points: " + GlobalState.Stats.Points.ToString()); 

        }
		if (GlobalState.DebugMode && Input.GetKeyDown(KeyCode.Alpha0)){
			GlobalState.AdaptiveMode = 0;
			Debug.Log("Adaptive Mode = 0");
		}
		if (GlobalState.DebugMode && Input.GetKeyDown(KeyCode.Alpha1)){
			GlobalState.AdaptiveMode = 1;
			Debug.Log("Adaptive Mode = 1");
		}
		if (GlobalState.DebugMode && Input.GetKeyDown(KeyCode.Alpha2)){
			GlobalState.AdaptiveMode = 2;
			Debug.Log("Adaptive Mode = 2");
		}
		if (GlobalState.DebugMode && Input.GetKeyDown(KeyCode.Alpha3) && GlobalState.HintMode == 0){
			GlobalState.HintMode = 1;
			Debug.Log("Hint Mode ON");
		}
		if (GlobalState.DebugMode && Input.GetKeyDown(KeyCode.Alpha3) && GlobalState.HintMode == 1){
			GlobalState.HintMode = 0;
			Debug.Log("Hint Mode OFF");
		}*/
        if (firstUpdate && !GlobalState.IsResume)
        {
            firstUpdate = false;
            SetTheme();
            lg.TransformTextSize();
        }
        if (GlobalState.GameState == stateLib.GAMESTATE_IN_GAME)
        {
            if (!finalized)
            {
                CheckWin();
                CheckLose();
            }
            HandleInterface();
        }
        if (leftCodescreen != GlobalState.StringLib.LEFT_CODESCREEN_X_COORDINATE && GlobalState.GameState == stateLib.GAMESTATE_IN_GAME)
        {
            leftCodescreen = GlobalState.StringLib.LEFT_CODESCREEN_X_COORDINATE;
            lg.DrawInnerXmlLinesToScreen();
        }
    }

    public string GrabMLDataFromDB(string url)
    {
        WebHelper.i.url = url;
        Debug.Log("ML Data url " + WebHelper.i.url);
        WebHelper.i.GetWebDataFromWeb();
        Debug.Log(WebHelper.i.webData);
        return WebHelper.i.webData;
    }
}


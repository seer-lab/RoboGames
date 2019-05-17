
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class Cinematic : MonoBehaviour
{
    LevelFactory factory;
    // This is the text that is displayed at the start of the level (during the "loading screen") prior to playing the level.
    public string introtext = "Level Start Placeholder!";
    // This text basically says "Press Enter to Continue" and is displayed at the bottom of the "Loading Screen" prior to playing the level.
    public string continuetext = "Continue Text Placeholder";
    // This is the text that is displayed at the end of the level (in the "Victory Screen") after playing the level.
    public string endtext = "Winner!\nLevel End Placeholder!";
    public GameObject prompt1, prompt2;
    public GameObject[] cinebugs = new GameObject[6];

    private bool cinerun = false;
    private float delaytime = 0f;
    private float delay = 0.1f;
    private List<GameObject> objs;

    //.................................>8.......................................
    // Use this for initialization
    void Start()
    {
        objs = new List<GameObject>();
        continuetext = stringLib.CONTINUE_TEXT;
        UpdateText();
        GameObject.Find("Fade").GetComponent<Fade>().onFadeIn(); 
        //Debug.Log(SceneManager.sceneCount);
    }
    IEnumerator LoadGame(){
        GameObject.Find("Fade").GetComponent<Fade>().onFadeOut(); 
        yield return new WaitForSecondsRealtime(1f); 
        SceneManager.LoadScene("newgame");

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
    private void UpdateText()
    {
        if (GlobalState.level == null)
        {
            UpdateLevel();
        }
        introtext = GlobalState.level.IntroText;
        endtext = GlobalState.level.ExitText;
    }
    private void UpdateLevel()
    {
        string filepath = Application.streamingAssetsPath +"/"+ GlobalState.GameMode + "leveldata/" + GlobalState.CurrentONLevel;
        //filepath = Path.Combine(filepath,  GlobalState.CurrentONLevel);
        Debug.Log("Cinematics.cs UpdateLevel() path: " + filepath);
        factory = new LevelFactory(filepath);
        GlobalState.level = factory.GetLevel();
    }
    private void UpdateLevel(string file)
    {
        string[] temp = file.Split('\\');
        for (int i = 0; i < temp.Length; i++)
        {
            Debug.Log(temp[i]);
        }
        GlobalState.CurrentONLevel = temp[temp.Length - 1];
        factory = new LevelFactory(file);
        GlobalState.level = factory.GetLevel();
    }
    //.................................>8.......................................
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)){
            GlobalState.GameState = stateLib.GAMESTATE_MENU;
            SceneManager.LoadScene("MainMenu");
        }
        if (GlobalState.GameState == stateLib.GAMESTATE_LEVEL_START)
        {
            if (!cinerun)
            {
                cinerun = true;
                if (GlobalState.GameMode != stringLib.GAME_MODE_ON)
                {
                    GameObject bug = (GameObject)Instantiate(cinebugs[2]);
                    objs.Add(bug);
                }
                else
                {
                    GameObject rob = (GameObject)Instantiate(cinebugs[3]);
                    objs.Add(rob);
                }
            }
            prompt1.GetComponent<Text>().text = introtext;
            if ((Input.GetMouseButtonDown(0)||Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) && delaytime < Time.time)
            {
                GlobalState.GameState = stateLib.GAMESTATE_IN_GAME;
                Destroy(objs[0]);
                cinerun = false;
                objs = new List<GameObject>();
                StartCoroutine(LoadGame()); 
            }
        }
        else if (GlobalState.GameState == stateLib.GAMESTATE_LEVEL_WIN)
        {
            if (!cinerun)
            {
                cinerun = true;
                GameObject bug = (GameObject)Instantiate(cinebugs[3]);
                objs.Add(bug);
                bug = (GameObject)Instantiate(cinebugs[0]);
                objs.Add(bug);
            }

            prompt1.GetComponent<Text>().text = endtext;

            if ((Input.GetMouseButtonDown(0)||Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) && delaytime < Time.time)
            {
                // RobotON 2, don't always want tutorials to run comics.
                // Read in the levels.txt and grab the top one.
                if (GlobalState.CurrentONLevel.StartsWith("tut") && GlobalState.GameMode == stringLib.GAME_MODE_BUG)
                {
                    // GlobalState.GameState = stateLib.GAMESTATE_STAGE_COMIC;
                }
                else
                {
                    GlobalState.GameState = stateLib.GAMESTATE_LEVEL_START;
                }
                GlobalState.GameState = stateLib.GAMESTATE_LEVEL_START;
                UpdateLevel(GlobalState.level.NextLevel);
                UpdateText();
                //GameObject.Find("Main Camera").GetComponent<GameController>().SetLevel(GlobalState.level.NextLevel);
                Destroy(objs[1]);
                Destroy(objs[0]);
                cinerun = false;
                objs = new List<GameObject>();

            }
        }
        else if (GlobalState.GameState == stateLib.GAMESTATE_LEVEL_LOSE)
        {
            if (!cinerun)
            {
                cinerun = true;
                GameObject bug = (GameObject)Instantiate(cinebugs[4]);
                objs.Add(bug);
            }
            prompt1.GetComponent<Text>().text = stringLib.LOSE_TEXT;
            prompt2.GetComponent<Text>().text = stringLib.RETRY_TEXT;
            if (Input.GetKeyDown(KeyCode.Escape) && delaytime < Time.time)
            {
                Destroy(objs[0]);
                prompt2.GetComponent<Text>().text = stringLib.CONTINUE_TEXT;

                cinerun = false;
                objs = new List<GameObject>();
                GlobalState.GameState = stateLib.GAMESTATE_MENU;
            }
            if ((Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) && delaytime < Time.time)
            {
                Destroy(objs[0]);
                prompt2.GetComponent<Text>().text = stringLib.CONTINUE_TEXT;

                cinerun = false;
                objs = new List<GameObject>();
                // One is called Bugleveldata and another OnLevel data.
                // Levels.txt, coding in menu.cs
                UpdateLevel(Application.streamingAssetsPath +"/"+ GlobalState.GameMode + "leveldata" + GlobalState.FilePath + GlobalState.CurrentONLevel);
                GlobalState.GameState = stateLib.GAMESTATE_LEVEL_START;
                //Debug.Log("LoadingScreen");
                StartCoroutine(LoadGame()); 
            }
        }
        else
        {
            delaytime = Time.time + delay;
        }
    }

    //.................................>8.......................................
}

//**************************************************//
// Class Name: Cinematic
// Class Description: This class controls the transition scenes between levels.
// Methods:
// 		void Start()
//		void Update()
// Author: Michael Miljanovic
// Date Last Modified: 6/1/2016
//**************************************************//

using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI; 

public class CinematicNew : MonoBehaviour
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
        introtext = GlobalState.level.IntroText;
        endtext = GlobalState.level.ExitText; 
    }

    private void UpdateLevel(string file)
    {
        string[] temp = file.Split('\\');
        for (int i = 0; i < temp.Length; i++)
        {
            Debug.Log(temp[i]); 
        }
        GlobalState.CurrentONLevel = temp[temp.Length - 1];
        Debug.Log("CurrentONLevel: " + GlobalState.CurrentONLevel); 
        factory = new LevelFactory(file);
        GlobalState.level = factory.GetLevel();
    }
    //.................................>8.......................................
    // Update is called once per frame
    void Update()
    {
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
            if ((Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) && delaytime < Time.time)
            {
                GlobalState.GameState = stateLib.GAMESTATE_IN_GAME;
                Destroy(objs[0]);
                cinerun = false;
                objs = new List<GameObject>();
                SceneManager.LoadScene("newgame");
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

            if ((Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) && delaytime < Time.time)
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
                UpdateLevel(GlobalState.GameMode + "leveldata" + GlobalState.FilePath + GlobalState.CurrentONLevel); 
                //GameObject.Find("Main Camera").GetComponent<GameController>().SetLevel(GlobalState.GameMode + "leveldata" + GlobalState.FilePath + GlobalState.CurrentONLevel);
                //lg.BuildLevel(GlobalState.GameMode + "leveldata" + GlobalState.FilePath + GlobalState.CurrentONLevel, false);
                GlobalState.GameState = stateLib.GAMESTATE_LEVEL_START;
                SceneManager.LoadScene("newgame"); 
            }
        }
        else
        {
            delaytime = Time.time + delay;
        }
    }

    //.................................>8.......................................
}

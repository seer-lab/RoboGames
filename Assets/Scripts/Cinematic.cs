//**************************************************//
// Class Name: Cinematic
// Class Description:
// Methods:
// 		void Start()
//		void Update()
// Author: Michael Miljanovic
// Date Last Modified: 6/1/2016
//**************************************************//

using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Cinematic : MonoBehaviour
{

	public string introtext = "Level Start Placeholder!";
	public string endtext = "Winner!\nLevel End Placeholder!";
	public string continuetext = stringLib.CONTINUE_TEXT;
	public GameObject codescreen;
	public GameObject prompt2;
	public GameObject menu;
	public GameObject[] cinebugs = new GameObject[6];

	private bool cinerun = false;
	private float delaytime = 0f;
	private float delay = 0.1f;
	private List<GameObject> objs;
	private LevelGenerator lg;

	//.................................>8.......................................
	// Use this for initialization
	void Start() {
		lg = codescreen.GetComponent<LevelGenerator>();
		objs = new List<GameObject>();
	}

	//.................................>8.......................................
	// Update is called once per frame
	void Update()
	{
		if (lg.gamestate == stateLib.GAMESTATE_LEVEL_START) {
			if (!cinerun) {
				cinerun = true;
				if (lg.gamemode != stringLib.GAME_MODE_ON) {
					GameObject bug =(GameObject)Instantiate(cinebugs[2]);
					objs.Add(bug);
				}
				else {
					GameObject rob =(GameObject)Instantiate(cinebugs[3]);
					objs.Add(rob);
				}
			}
			GetComponent<TextMesh>().text = introtext;
			if (Input.GetKeyDown(KeyCode.Return) && delaytime < Time.time) {
				lg.gamestate = stateLib.GAMESTATE_IN_GAME;
				Destroy(objs[0]);
				cinerun = false;
				objs = new List<GameObject>();
				lg.GUISwitch(true);
			}
		}
		else if (lg.gamestate == stateLib.GAMESTATE_LEVEL_WIN) {
			if (!cinerun) {
				cinerun = true;
				GameObject bug =(GameObject)Instantiate(cinebugs[3]);
				objs.Add(bug);
				bug =(GameObject)Instantiate(cinebugs[0]);
				objs.Add(bug);
			}

			GetComponent<TextMesh>().text = endtext;

			if (Input.GetKeyDown(KeyCode.Return) && delaytime < Time.time) {
				//@TODO: Figure out what this is saying, it's hardcode
				// RobotON 2, don't always want tutorials to run comics.
				// Read in the levels.txt and grab the top one.
				if (lg.currentlevel.StartsWith("tut") && lg.gamemode == stringLib.GAME_MODE_BUG) {
					lg.gamestate = stateLib.GAMESTATE_STAGE_COMIC;
				}
				else {
					lg.gamestate = stateLib.GAMESTATE_LEVEL_START;
				}

				lg.BuildLevel(lg.nextlevel, false, "");
				Destroy(objs[1]);
				Destroy(objs[0]);
				cinerun = false;
				objs = new List<GameObject>();
			}
		}
		else if (lg.gamestate == stateLib.GAMESTATE_LEVEL_LOSE) {
			if (!cinerun) {
				cinerun = true;
				GameObject bug =(GameObject)Instantiate(cinebugs[4]);
				objs.Add(bug);
			}
			GetComponent<TextMesh>().text = stringLib.LOSE_TEXT;
			prompt2.GetComponent<TextMesh>().text = stringLib.RETRY_TEXT;
			if (Input.GetKeyDown(KeyCode.Escape) && delaytime < Time.time) {
				Destroy(objs[0]);
				prompt2.GetComponent<TextMesh>().text = stringLib.CONTINUE_TEXT;

				cinerun = false;
				objs = new List<GameObject>();
				lg.gamestate = stateLib.GAMESTATE_MENU;
			}
			if (Input.GetKeyDown(KeyCode.Return) && delaytime < Time.time) {
				Destroy(objs[0]);
				prompt2.GetComponent<TextMesh>().text = stringLib.CONTINUE_TEXT;

				cinerun = false;
				objs = new List<GameObject>();
				menu.GetComponent<Menu>().gameon = true;
				//@TODO: More hardcode to check
				// One is called Bugleveldata and another OnLevel data.
				// Levels.txt, coding in menu.cs
				lg.BuildLevel(lg.gamemode + @"leveldata\" + lg.currentlevel, false);
				lg.gamestate = stateLib.GAMESTATE_LEVEL_START;
			}
		}
		else {
			delaytime = Time.time + delay;
		}
	}

	//.................................>8.......................................
}

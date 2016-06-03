//**************************************************//
// Class Name: Menu
// Class Description: Menu logic used in the game
// Methods:
// 		void Start()
//		void Update()
//		public void saveGame(string currentlevel)
//		void m2switch(bool on)
// Author: Michael Miljanovic
// Date Last Modified: 6/1/2016
//**************************************************//

// @TODO: Bug: New Game > RoboBUG > Menu > Sound. Back and Sound buttons both green.
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class Menu : MonoBehaviour
{
	public bool gameon = false;
	public List<string> levels;
	public List<string> passed;
	public GameObject codescreen;
	public GameObject cinematic;
	public GameObject[] buttons = new GameObject[5];
	public GameObject[] buttontext = new GameObject[5];
	public GameObject[] m2buttons = new GameObject[2];
	public GameObject[] m2buttontext = new GameObject[2];
	public GameObject[] m2arrows = new GameObject[2];
	public GameObject menu2;
	public Sprite bluebutton;
	public Sprite greenbutton;

	private bool soundon = true;
	private float delaytime = 0f;
	private float delay = 0.1f;
	private int option = 0;
	private int levoption = 0;
	private string lfile;
	private LevelGenerator lg;
	private StreamReader sr;

	//.................................>8.......................................
	// Use this for initialization
	void Start() {
		buttontext[stateLib.GAMEMENU_NEW_GAME].GetComponent<TextMesh>().text = "New Game";
		buttontext[stateLib.GAMEMENU_LOAD_GAME].GetComponent<TextMesh>().text = "Load Game";
		buttontext[stateLib.GAMEMENU_SOUND_OPTIONS].GetComponent<TextMesh>().text = "Sound Options";
		buttontext[stateLib.GAMEMENU_EXIT_GAME].GetComponent<TextMesh>().text = "Exit Game";
		//@TODO: Resume should not show at main menu.
		//@TODO: Additional game states declared here incl. submenu selections for "option" that have not been indexed.
		buttontext[stateLib.GAMEMENU_RESUME_GAME].GetComponent<TextMesh>().text = "Resume Game";
		buttons[stateLib.GAMEMENU_RESUME_GAME].GetComponent<SpriteRenderer>().color = Color.grey;
		lg = codescreen.GetComponent<LevelGenerator>();
		m2switch(false);
	}

	//.................................>8.......................................
	// Update is called once per frame
	void Update() {
		if (!gameon) {
			buttons[stateLib.GAMEMENU_RESUME_GAME].GetComponent<SpriteRenderer>().color = Color.grey;
		}
		else {
			buttons[stateLib.GAMEMENU_RESUME_GAME].GetComponent<SpriteRenderer>().color = Color.white;
		}

		if (lg.gamestate == stateLib.GAMESTATE_MENU) {
			if (Input.GetKeyDown(KeyCode.UpArrow)) {
				buttons[option].GetComponent<SpriteRenderer>().sprite = bluebutton;
				option = (option == stateLib.GAMEMENU_NEW_GAME) ? stateLib.GAMEMENU_NEW_GAME : option - 1;
			}
			if (Input.GetKeyDown(KeyCode.DownArrow)) {
				buttons[option].GetComponent<SpriteRenderer>().sprite = bluebutton;
				if (gameon) {
					option = (option == stateLib.GAMEMENU_RESUME_GAME) ? stateLib.GAMEMENU_RESUME_GAME : option + 1;
				}
				else {
					option = (option == stateLib.GAMEMENU_EXIT_GAME) ? stateLib.GAMEMENU_EXIT_GAME : option + 1;
				}
			}

			buttons[option].GetComponent<SpriteRenderer>().sprite = greenbutton;

			if (Input.GetKeyDown(KeyCode.Return) && delaytime < Time.time) {
				switch(option) {
					case stateLib.GAMEMENU_NEW_GAME:
						//@TODO: Why Figure out what states -1 through -4 are.
						lg.gamestate = -3;
						buttons[option].GetComponent<SpriteRenderer>().sprite = bluebutton;
						option = 0;
						m2switch(true);
						m2buttontext[0].GetComponent<TextMesh>().text = stringLib.GAME_ROBOT_ON;
						m2buttontext[1].GetComponent<TextMesh>().text = stringLib.GAME_ROBOT_BUG;
						break;
					case stateLib.GAMEMENU_LOAD_GAME:
						lg.gamestate = -4;
						buttons[option].GetComponent<SpriteRenderer>().sprite = bluebutton;
						option = 0;
						m2switch(true);
						m2buttontext[0].GetComponent<TextMesh>().text = stringLib.GAME_ROBOT_ON;
						m2buttontext[1].GetComponent<TextMesh>().text = stringLib.GAME_ROBOT_BUG;
						break;
					case stateLib.GAMEMENU_SOUND_OPTIONS:
						lg.gamestate = -2;
						buttons[option].GetComponent<SpriteRenderer>().sprite = bluebutton;
						option = 0;
						m2switch(true);
						m2buttontext[0].GetComponent<TextMesh>().text = "Sound: " +(soundon ? stringLib.MENU_SOUND_ON_COLOR_TAG + "ON" + stringLib.CLOSE_COLOR_TAG : stringLib.MENU_SOUND_OFF_COLOR_TAG + "OFF" + stringLib.CLOSE_COLOR_TAG);
						m2buttontext[1].GetComponent<TextMesh>().text = "Back";
						break;
					case stateLib.GAMEMENU_EXIT_GAME:
						Application.Quit();
						break;
					case stateLib.GAMEMENU_RESUME_GAME:
						lg.gamestate = stateLib.GAMESTATE_IN_GAME;
						lg.GUISwitch(true);
						break;
					default:
						break;
				}
			}
		}
		else if (lg.gamestate == -1) {
			if (levoption < levels.Count - 1 && passed[levoption] == "1") {
				m2arrows[1].GetComponent<SpriteRenderer>().enabled = true;
			}
			else {
				m2arrows[1].GetComponent<SpriteRenderer>().enabled = false;
			}
			if (levoption != 0) {
				m2arrows[0].GetComponent<SpriteRenderer>().enabled = true;
			}
			else {
				m2arrows[0].GetComponent<SpriteRenderer>().enabled = false;
			}
			m2buttons[option].GetComponent<SpriteRenderer>().sprite = greenbutton;
			if (Input.GetKeyDown(KeyCode.UpArrow)) {
				m2buttons[1].GetComponent<SpriteRenderer>().sprite = bluebutton;
				option = 0;
			}
			if (Input.GetKeyDown(KeyCode.DownArrow)) {
				m2buttons[0].GetComponent<SpriteRenderer>().sprite = bluebutton;
				option = 1;
			}
			if (Input.GetKeyDown(KeyCode.RightArrow)) {
				if (levoption < levels.Count - 1 && passed[levoption] == "1") {
					levoption++;
				}
				m2buttontext[0].GetComponent<TextMesh>().text = levels[levoption];
			}
			if (Input.GetKeyDown(KeyCode.LeftArrow)) {
				levoption = 0 == levoption ? 0 : levoption - 1;
				m2buttontext[0].GetComponent<TextMesh>().text = levels[levoption];
			}
			if (Input.GetKeyDown(KeyCode.Return)) {
				switch(option) {
					case 0:
						lg.BuildLevel(lg.gamemode + @"leveldata\" + levels[levoption], false);
						lg.gamestate = stateLib.GAMESTATE_LEVEL_START;
						levoption = 0;
						gameon = true;
						buttons[4].GetComponent<SpriteRenderer>().color = Color.white;
						m2switch(false);
						break;
					case 1:
						lg.gamestate = stateLib.GAMESTATE_MENU;
						m2buttons[1].GetComponent<SpriteRenderer>().sprite = bluebutton;

						m2switch(false);
						break;
					default:
						break;
				}
			}

		}
		//
		else if (lg.gamestate == -2) {
			m2buttons[option].GetComponent<SpriteRenderer>().sprite = greenbutton;
			if (Input.GetKeyDown(KeyCode.UpArrow)) {
				m2buttons[1].GetComponent<SpriteRenderer>().sprite = bluebutton;
				option = 0;
			}
			if (Input.GetKeyDown(KeyCode.DownArrow)) {
				m2buttons[0].GetComponent<SpriteRenderer>().sprite = bluebutton;
				option = 1;
			}
			if (Input.GetKeyDown(KeyCode.Return)) {
				switch(option) {
					case 0:
						soundon = !soundon;
						m2buttontext[0].GetComponent<TextMesh>().text = "Sound: " + (soundon ? stringLib.MENU_SOUND_ON_COLOR_TAG + "ON" + stringLib.CLOSE_COLOR_TAG : stringLib.MENU_SOUND_OFF_COLOR_TAG + "OFF" + stringLib.CLOSE_COLOR_TAG);
						AudioListener.volume = (soundon) ? 1 : 0;
						break;
					case 1:
						lg.gamestate = stateLib.GAMESTATE_MENU;
						m2buttons[1].GetComponent<SpriteRenderer>().sprite = bluebutton;
						m2switch(false);
						option = 2;
						break;
				}
			}
		}
		else if (lg.gamestate == -3) {
			m2buttons[option].GetComponent<SpriteRenderer>().sprite = greenbutton;
			if (Input.GetKeyDown(KeyCode.UpArrow)) {
				m2buttons[1].GetComponent<SpriteRenderer>().sprite = bluebutton;
				option = 0;
			}
			if (Input.GetKeyDown(KeyCode.DownArrow)) {
				m2buttons[0].GetComponent<SpriteRenderer>().sprite = bluebutton;
				option = 1;
			}
			if (Input.GetKeyDown(KeyCode.Return)) {
				switch(option) {
					case 0:
						lg.gamemode = stringLib.GAME_MODE_ON;
						lg.BuildLevel(@"onleveldata\demo.xml", false);
						lg.gamestate = stateLib.GAMESTATE_LEVEL_START;
						break;
					case 1:
						lg.gamemode = stringLib.GAME_MODE_BUG;
						lg.BuildLevel(@"bugleveldata\tut1.xml", false);
						lg.gamestate = stateLib.GAMESTATE_INITIAL_COMIC;
						break;
				}
				m2switch(false);
				gameon = true;
				buttons[4].GetComponent<SpriteRenderer>().color = Color.white;

			}
		}
		else if (lg.gamestate == -4) {
			m2buttons[option].GetComponent<SpriteRenderer>().sprite = greenbutton;
			if (Input.GetKeyDown(KeyCode.UpArrow)) {
				m2buttons[1].GetComponent<SpriteRenderer>().sprite = bluebutton;
				option = 0;
			}
			if (Input.GetKeyDown(KeyCode.DownArrow)) {
				m2buttons[0].GetComponent<SpriteRenderer>().sprite = bluebutton;
				option = 1;
			}
			if (Input.GetKeyDown(KeyCode.Return)) {
				switch(option) {
					//@TODO: Switches between RobotON and RoboBUG. Should not be magic number!
					case 0:
						lg.gamemode = stringLib.GAME_MODE_ON;
						break;
					case 1:
						lg.gamemode = stringLib.GAME_MODE_BUG;
						break;
				}

				levels.Clear();
				passed.Clear();
				lfile = lg.gamemode + @"leveldata\levels.txt";
				sr = File.OpenText(lfile);
				string line;
				while((line = sr.ReadLine()) != null) {
					string[] data = line.Split(' ');
					levels.Add(data[0]);
					passed.Add(data[1]);
				}
				sr.Close();

				lg.gamestate = -1;
				option = 0;
				m2buttons[1].GetComponent<SpriteRenderer>().sprite = bluebutton;
				m2buttontext[0].GetComponent<TextMesh>().text = levels[levoption];
				m2buttontext[1].GetComponent<TextMesh>().text = "Back";
			}
		}
		else {
			delaytime = Time.time + delay;
		}
	}

	//.................................>8.......................................
	public void saveGame(string currentlevel) {
		levels.Clear();
		passed.Clear();
		lfile = lg.gamemode + @"leveldata\levels.txt";
		sr = File.OpenText(lfile);
		string line;
		while((line = sr.ReadLine()) != null) {
			string[] data = line.Split(' ');
			levels.Add(data[0]);
			passed.Add(data[1]);
		}
		sr.Close();

		//@TODO: Remove hardcode here
		passed[levels.IndexOf(currentlevel)] = "1";
		StreamWriter sw = File.CreateText(lg.gamemode + @"leveldata\levels.txt");
		for (int i = 0; i < levels.Count; i++) {
			sw.WriteLine(levels[i] + " " + passed[i]);
		}
		sw.Close();
	}

	//.................................>8.......................................
	void m2switch(bool on) {
		if (on) {
			menu2.GetComponent<SpriteRenderer>().enabled = true;
			foreach(GameObject button in m2buttons) {
				button.GetComponent<SpriteRenderer>().enabled = true;
			}
		}
		else {
			menu2.GetComponent<SpriteRenderer>().enabled = false;
			foreach(GameObject button in m2buttons) {
				button.GetComponent<SpriteRenderer>().enabled = false;
			}
			foreach(GameObject btext in m2buttontext) {
				btext.GetComponent<TextMesh>().text = "";
			}
			foreach(GameObject arrow in m2arrows) {
				arrow.GetComponent<SpriteRenderer>().enabled = false;
			}
		}
	}
}

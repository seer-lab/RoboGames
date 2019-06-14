//**************************************************//
// Class Name: Menu
// Class Description: This is the class for the menu, used for both games. This class gets called first.
// Methods:
// 		void Start()
//		void Update()
//		public void saveGame(string currentlevel)
//		void m2switch(bool on)
// Author: Michael Miljanovic
// Date Last Modified: 6/1/2016
//**************************************************//

using UnityEngine;
using System.Collections;
using UnityEngine.UI; 
using System.Collections.Generic;
using System.IO;

public class Menu : MonoBehaviour
{
	public bool gameon = false;
    public string filepath;
	public List<string> levels;
	public List<string> passed;
	public GameObject codescreen;
	public GameObject cinematic;
	private GameObject[] buttons = new GameObject[5];
    private MenuButton[] menuButtons = new MenuButton[5]; 
	private Text[] buttontext = new Text[5];
	public GameObject[] m2buttons = new GameObject[2];
	public GameObject[] m2buttontext = new GameObject[2];
	public GameObject[] m2arrows = new GameObject[2];
	public GameObject menu2;

    private Submenu submenu; 

	private bool soundon = true;
	private float delaytime = 0f;
	private float delay = 0.1f;
	private int option = 0;
	private int levoption = 0;
	private string lfile;
	private StreamReader sr;
    private string windowsFilepath = @"\";
    private string unixFilepath = @"/";

	//.................................>8.......................................
	// Use this for initialization
	void Start() {
        LoadButtons();
        menuButtons[stateLib.GAMEMENU_NEW_GAME].LoadText("New Game");
        menuButtons[stateLib.GAMEMENU_LOAD_GAME].LoadText("Load Game");
        menuButtons[stateLib.GAMEMENU_SOUND_OPTIONS].LoadText("Sound Options");
        menuButtons[stateLib.GAMEMENU_EXIT_GAME].LoadText("Exit Game");
        menuButtons[stateLib.GAMEMENU_RESUME_GAME].LoadText("Resume Game");
        menuButtons[stateLib.GAMEMENU_RESUME_GAME].ToggleInactive(); 
        filepath = (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor) ? windowsFilepath : unixFilepath;
	}
    public void HandleClick(int index)
    {

    }
    private void LoadButtons()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i] = this.transform.GetChild(i + 1).gameObject;
            buttontext[i] = buttons[i].transform.GetChild(0).GetComponent<Text>();
            menuButtons[i] = buttons[i].GetComponent<MenuButton>(); 
        }
        submenu = this.transform.GetChild(buttons.Length + 1).GetComponent<Submenu>(); 
    }
    private void HandleResumeGame()
    {
        // Handle "Resume Game" button behavior. If we have a game session we can click it, otherwise grey it out. --[
        if (!gameon)
        {
            buttons[stateLib.GAMEMENU_RESUME_GAME].GetComponent<Image>().color = Color.grey;
        }
        else
        {
            buttons[stateLib.GAMEMENU_RESUME_GAME].GetComponent<Image>().color = Color.white;
        }
    }
    private void HandleArrowInput()
    {
        // If we are in the menu, handle up and down arrows --[
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            // The previous button should be made blue (change from green to blue).
            // If we are on the first option (New Game), don't allow the up arrow to wrap-around.
            menuButtons[option].ToggleBlue();
            option = (option == stateLib.GAMEMENU_NEW_GAME) ? stateLib.GAMEMENU_NEW_GAME : option - 1;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            // The previous button should be made blue (change from green to blue).
            // The last option will be either Resume Game or Exit game. In either case, don't allow the down arrow to wrap-around.
            menuButtons[option].ToggleBlue();
            if (gameon)
            {
                option = (option == stateLib.GAMEMENU_RESUME_GAME) ? stateLib.GAMEMENU_RESUME_GAME : option + 1;
            }
            else
            {
                option = (option == stateLib.GAMEMENU_EXIT_GAME) ? stateLib.GAMEMENU_EXIT_GAME : option + 1;
            }
        }
    }
	//.................................>8.......................................
	// Update is called once per frame
	void Update() {
        HandleResumeGame();
        HandleArrowInput(); 

		// Make the current button appear green. The previous buttons change to blue
		// because there is an Up or Down arrow event. This will fire outside of the event.
		menuButtons[option].ToggleGreen();

		// When we press Return (Enter Key), take us to the sub-menus
		if ((Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) && delaytime < Time.time) {
            switch (option) {
                case stateLib.GAMEMENU_NEW_GAME:
                    // Select between RobotON or RoboBUG.
                    menuButtons[option].ToggleBlue();
                    option = 0;            
                    string[] values = { stringLib.GAME_ROBOT_ON, stringLib.GAME_ROBOT_BUG };
                    submenu.ParentPosition = buttons[0].transform.position; 
                    submenu.LoadButtons(values);
                    break;
                case stateLib.GAMEMENU_LOAD_GAME:
                    // Load a level from RobotON or RoboBUG.
                    menuButtons[option].ToggleBlue();
                    option = 0;
                    string[] arr = { stringLib.GAME_ROBOT_ON, stringLib.GAME_ROBOT_BUG };
                    submenu.ParentPosition = buttons[0].transform.position; 
                    submenu.LoadButtons(arr);
                    break;
                case stateLib.GAMEMENU_SOUND_OPTIONS:
                    menuButtons[option].ToggleBlue();
                    option = 0;
                    string[] v = { "Sound: " + (soundon ? "ON" + stringLib.CLOSE_COLOR_TAG : "OFF" + stringLib.CLOSE_COLOR_TAG) };
                    submenu.LoadButtons(v); 
				    break;
				case stateLib.GAMEMENU_EXIT_GAME:
				    Application.Quit();
				    break;
				case stateLib.GAMEMENU_RESUME_GAME:
				    menuButtons[option].ToggleBlue();
				    break;
				    default:
				    break;
				}
			}
		else if (Input.GetKeyDown(KeyCode.Escape)) {
			m2switch(false);
			flushButtonColor();
		}
		else if (false) {
			if (levoption < levels.Count - 1 && passed[levoption] == "1") {
				m2arrows[1].GetComponent<Image>().enabled = true;
			}
			else {
				m2arrows[1].GetComponent<Image>().enabled = false;
			}
			if (levoption != 0) {
				m2arrows[0].GetComponent<Image>().enabled = true;
			}
			else {
				m2arrows[0].GetComponent<Image>().enabled = false;
			}
			//m2buttons[option].GetComponent<Image>().sprite = greenbutton;
			if (Input.GetKeyDown(KeyCode.UpArrow)) {
				//m2buttons[1].GetComponent<Image>().sprite = bluebutton;
				option = 0;
			}
			if (Input.GetKeyDown(KeyCode.DownArrow)) {
				//m2buttons[0].GetComponent<Image>().sprite = bluebutton;
				option = 1;
			}
			if (Input.GetKeyDown(KeyCode.RightArrow)) {
				if (levoption < levels.Count - 1 && passed[levoption] == "1") {
					levoption++;
				}
				m2buttontext[0].GetComponent<Text>().text = levels[levoption];
			}
			if (Input.GetKeyDown(KeyCode.LeftArrow)) {
				levoption = (levoption == 0) ? 0 : levoption - 1;
				m2buttontext[0].GetComponent<Text>().text = levels[levoption];
			}
			if ((Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))) {
				switch(option) {
					case 0:
					levoption = 0;
					gameon = true;
					buttons[4].GetComponent<Image>().color = Color.white;
					m2switch(false);
					break;
					case 1:
					//m2buttons[1].GetComponent<Image>().sprite = bluebutton;

					m2switch(false);
					break;
					default:
					break;
				}
			}

		}
		//
		else if (false) {
			//m2buttons[option].GetComponent<Image>().sprite = greenbutton;
			if (Input.GetKeyDown(KeyCode.UpArrow)) {
				//m2buttons[1].GetComponent<Image>().sprite = bluebutton;
				option = 0;
			}
			if (Input.GetKeyDown(KeyCode.DownArrow)) {
				//m2buttons[0].GetComponent<Image>().sprite = bluebutton;
				option = 1;
			}
			if ((Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))) {
				switch(option) {
					case 0:
					soundon = !soundon;
					//m2buttontext[0].GetComponent<Text>().text = "Sound: " + ((soundon) ? lg.stringLibrary.menu_sound_on_color_tag + "ON" + stringLib.CLOSE_COLOR_TAG : lg.stringLibrary.menu_sound_off_color_tag + "OFF" + stringLib.CLOSE_COLOR_TAG);
					AudioListener.volume = (soundon) ? 1 : 0;
					break;
					case 1:
					//m2buttons[1].GetComponent<Image>().sprite = bluebutton;
					m2switch(false);
					option = 2;
					break;
				}
			}
		}
		else if (false) {
			//m2buttons[option].GetComponent<Image>().sprite = greenbutton;
			if (Input.GetKeyDown(KeyCode.UpArrow)) {
				//m2buttons[1].GetComponent<SpriteRenderer>().sprite = bluebutton;
				option = 0;
			}
			if (Input.GetKeyDown(KeyCode.DownArrow)) {
				//m2buttons[0].GetComponent<SpriteRenderer>().sprite = bluebutton;
				option = 1;
			}
			if ((Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))) {
				switch(option) {
					case 0:

					break;
					case 1:

					/*
					lg.gamemode = stringLib.GAME_MODE_BUG;
					lg.BuildLevel("bugleveldata" + filepath + "tut1.xml");
					lg.gamestate = stateLib.GAMESTATE_INITIAL_COMIC;
					*/
					break;
				}
				m2switch(false);
				gameon = true;
				buttons[4].GetComponent<Image>().color = Color.white;

			}
		}
		else if (true) {
			//m2buttons[option].GetComponent<Image>().sprite = greenbutton;
			if (Input.GetKeyDown(KeyCode.UpArrow)) {
				//m2buttons[1].GetComponent<Image>().sprite = bluebutton;
				option = 0;
			}
			if (Input.GetKeyDown(KeyCode.DownArrow)) {
				//m2buttons[0].GetComponent<Image>().sprite = bluebutton;
				option = 1;
			}
			if ((Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))) {
				switch(option) {
					case 0:
					break;
					case 1:
					break;
				}

				levels.Clear();
				passed.Clear();
				lfile = "leveldata" + filepath + "levels.txt";
				sr = File.OpenText(lfile);
				string line;
				while((line = sr.ReadLine()) != null) {
					string[] data = line.Split(' ');
					levels.Add(data[0]);
					passed.Add(data[1]);
				}
				sr.Close();
				option = 0;
				//m2buttons[1].GetComponent<Image>().sprite = bluebutton;
				m2buttontext[0].GetComponent<Text>().text = levels[levoption];
				m2buttontext[1].GetComponent<Text>().text = "Back";
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
		//lfile = lg.gamemode + "leveldata" + filepath + "levels.txt";

		sr = File.OpenText(lfile);
		string line;
		while((line = sr.ReadLine()) != null) {
			string[] data = line.Split(' ');
			levels.Add(data[0]);
			passed.Add(data[1]);
		}
		sr.Close();
		passed[levels.IndexOf(currentlevel)] = "1";
		StreamWriter sw = File.CreateText("leveldata" + filepath + "levels.txt");
		for (int i = 0; i < levels.Count; i++) {
			sw.WriteLine(levels[i] + " " + passed[i]);
		}
		sw.Close();
	}

	//.................................>8.......................................
	//************************************************************************//
	// Method: private void m2switch(bool on)
	// Description: Sub-menu switch (menu 2 switch). If bool is TRUE, show the Sub-menu
	// and all the buttons for that sub-menu. If the bool is FALSE, hide the
	// sub-menu, the buttons, text, arrows, etc.
	//************************************************************************//
	private void m2switch(bool on) {
		if (on) {
			menu2.GetComponent<Image>().enabled = true;
			foreach(GameObject button in m2buttons) {
				button.GetComponent<Image>().enabled = true;
			}
		}
		else {
			menu2.GetComponent<Image>().enabled = false;
			foreach(GameObject button in m2buttons) {
				button.GetComponent<Image>().enabled = false;
			}
			foreach(GameObject btext in m2buttontext) {
				btext.GetComponent<Text>().text = "";
			}
			foreach(GameObject arrow in m2arrows) {
				arrow.GetComponent<Image>().enabled = false;
			}
		}
	}

	//.................................>8.......................................
	//************************************************************************//
	// Method: public void flushButtonColor()
	// Description: Change all menu and sub-menu buttons to blue. Set the cursor
	// to the New Game option. Basically, reset the menu.
	//************************************************************************//
	public void flushButtonColor() {
        /*
		m2buttons[0].GetComponent<Image>().sprite = bluebutton;
		m2buttons[1].GetComponent<Image>().sprite = bluebutton;
		option = 0;
		buttons[option].GetComponent<Image>().sprite = greenbutton;
        */
	}
}

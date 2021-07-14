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

using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OldMenu : MonoBehaviour
{
    public bool gameon = false;
    public string filepath;
    public List<string> levels;
    public List<string> passed;
    public GameObject background;
    public GameObject cinematic;
    public GameObject[] buttons = new GameObject[5];
    public GameObject[] buttontext = new GameObject[5];
    public GameObject[] m2buttons = new GameObject[3];
    public GameObject[] m2buttontext = new GameObject[3];
    public GameObject[] m2arrows = new GameObject[2];
    public GameObject menu2;
    public Sprite bluebutton;
    public Sprite greenbutton;

    int optionPage = 0;
    private bool soundon = true;
    private float delaytime = 0f;
    private float delay = 0.1f;
    private int option = 0;
    private int levoption = 0;
    private string lfile;
    private StreamReader sr;
    private string windowsFilepath = @"\";
    private string unixFilepath = @"/";
    bool firstOption = true;
    int selectedIndex = -1;
    int textOption = GlobalState.TextSize;
    string[] textsizes;
    int[] fontSizes;
    bool entered = false;

    //.................................>8.......................................
    // Use this for initialization
    void Start()
    {
        Screen.orientation = ScreenOrientation.Landscape;
        if (SceneManager.sceneCount > 1)
        {
            GetComponent<AudioSource>().Stop();
            GetComponent<AudioListener>().enabled = false;
        }
        if (!GlobalState.IsResume)
        {
            InitializeGlobals();
            GameObject.Find("Fade").GetComponent<Fade>().onFadeIn();
        }
        buttontext[stateLib.GAMEMENU_NEW_GAME].GetComponent<TextMesh>().text = "New Game";
        buttontext[stateLib.GAMEMENU_LOAD_GAME].GetComponent<TextMesh>().text = "Load Game";
        buttontext[stateLib.GAMEMENU_SOUND_OPTIONS].GetComponent<TextMesh>().text = "Options";
        buttontext[stateLib.GAMEMENU_EXIT_GAME].GetComponent<TextMesh>().text = "Exit Game";
        buttontext[stateLib.GAMEMENU_RESUME_GAME].GetComponent<TextMesh>().text = "Resume Game";
        buttons[stateLib.GAMEMENU_RESUME_GAME].GetComponent<SpriteRenderer>().color = Color.grey;
        textsizes = new string[] { "Small", "Text: Normal", "Large", "Large++" };
        fontSizes = new int[] { stateLib.TEXT_SIZE_SMALL, stateLib.TEXT_SIZE_NORMAL, stateLib.TEXT_SIZE_LARGE, stateLib.TEXT_SIZE_VERY_LARGE };
        m2switch(false);
        GlobalState.IsDark = !GlobalState.IsDark;
        ToggleTheme();
        filepath = (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor) ? windowsFilepath : unixFilepath;


        //Checks for users previous settings such as sessionID, or any menu preferences
        String sessionID = PlayerPrefs.GetString("sessionID");
        String courseCode = PlayerPrefs.GetString("courseCode");

        Debug.Log("Resume OM: " + GlobalState.IsResume);
        Debug.Log("Level OM: " + GlobalState.CurrentBUGLevel);


        //Checks for sessionID, if there is, grab that and the menu preferences
        if (sessionID == "" || sessionID == null)
        {
            //Create a sessionID and store it
            if (GlobalState.sessionID == 0)
            {
                GlobalState.sessionID = AnalyticsSessionInfo.sessionId;
                Debug.Log("Making Session ID: " + GlobalState.sessionID);
                PlayerPrefs.SetString("sessionID", GlobalState.sessionID.ToString());
                SetUserPrefs();

            }
            else
            {
                Debug.Log("Found Session ID: " + GlobalState.sessionID);
            }

            /*Hard Coded course code for now, Removed for new one
            sendInitialDataDB(GlobalState.sessionID.ToString(), DateTime.Now.ToString(),
                               stringLib.DB_URL + GlobalState.GameMode.ToUpper(), GlobalState.sessionID.ToString());
            */
            Debug.Log(GlobalState.courseCode);
            sendInitialDataDB(GlobalState.sessionID.ToString(), DateTime.UtcNow.ToString(), stringLib.DB_URL + GlobalState.GameMode.ToUpper() + "/" + GlobalState.courseCode);

        }
        else
        {
            if (GlobalState.sessionID == 0)
            {
                //Debug.Log("STRING SESSIONID: " +sessionID);
                GlobalState.sessionID = Convert.ToInt64(sessionID);
            }

            Debug.Log("Found Session ID: " + GlobalState.sessionID);

            //Check if it does exits in the DB
            WebHelper.i.url = stringLib.DB_URL + GlobalState.GameMode.ToUpper() + "/" + GlobalState.sessionID.ToString();
            Debug.Log(WebHelper.i.url + "Getting Data from Database");
            WebHelper.i.GetWebDataFromWeb(false);
            if (WebHelper.i.webData == "null" || WebHelper.i.webData == null)
            {
                Debug.Log("Does Not exits in " + GlobalState.GameMode.ToUpper());
                Debug.Log(GlobalState.courseCode);
                sendInitialDataDB(GlobalState.sessionID.ToString(), DateTime.UtcNow.ToString(), stringLib.DB_URL + GlobalState.GameMode.ToUpper() + "/" + GlobalState.courseCode);
            }
        }

        if (GlobalState.GameMode == stringLib.GAME_MODE_BUG)
        {
            transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("MenuPrefabs/LogoBugDark");
        }
        GrabUserPrefs();
        SetUserPrefs();
        readFromFiles();
    }
    public void onClick(int index)
    {
        if (!GlobalState.IsResume && index == 4)
            return;
        if (index < -1)
        {
            if (index == -2)
            {
                if (levoption < levels.Count - 1 && passed[levoption] == "1")
                {
                    levoption++;
                }
                m2buttontext[0].GetComponent<TextMesh>().text = levels[levoption];
            }
            else if (index == -3)
            {
                levoption = (levoption == 0) ? 0 : levoption - 1;
                m2buttontext[0].GetComponent<TextMesh>().text = levels[levoption];
            }
            return;
        }
        selectedIndex = index;
        buttons[option].GetComponent<SpriteRenderer>().sprite = bluebutton;
        option = selectedIndex;
        entered = true;
    }
    private void InitializeGlobals()
    {
        GlobalState.IsPlaying = false;
        if (GlobalState.CurrentONLevel == null)
        {
            GlobalState.CurrentONLevel = "tutorial0.xml";
        }
        else GlobalState.IsPlaying = true;
        GlobalState.CurrentBUGLevel = GlobalState.CurrentONLevel;
        GlobalState.GameState = stateLib.GAMEMENU_NEW_GAME;
        GlobalState.level = null;
        GlobalState.Character = "Robot";
        GlobalState.StringLib = new stringLib();
        if (GlobalState.Stats == null) GlobalState.Stats = new CharacterStats(true);
        textOption = GlobalState.TextSize;
        soundon = GlobalState.soundon;

    }
    private void ToggleTheme()
    {
        GlobalState.IsDark = !GlobalState.IsDark;

        if (GlobalState.IsDark)
        {
            this.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/panel-2");
            menu2.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/panel-4");
            background.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/circuit_board_dark");
            if (GlobalState.GameMode == stringLib.GAME_MODE_ON)
                transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("MenuPrefabs/LogoDark");
            else
                transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("MenuPrefabs/LogoBugDark");
        }
        else
        {
            this.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/panel-9");
            menu2.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/panel-8");
            background.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/circuit_board_light");
            if (GlobalState.GameMode == stringLib.GAME_MODE_ON)
                transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("MenuPrefabs/LogoLight");
            else
                transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("MenuPrefabs/LogoBugLight");
        }

    }
    //.................................>8.......................................
    // Update is called once per frame
    void Update()
    {

        AudioListener.volume = (soundon) ? 1 : 0;
        if (GlobalState.DebugMode && Input.GetKeyDown(KeyCode.G))
        {
            GlobalState.Stats.GrantPower();
            Debug.Log("All Powers Maxed Out!");
            Debug.Log("XP Boost: " + GlobalState.Stats.XPBoost.ToString()
           + "\n Speed: " + GlobalState.Stats.Speed.ToString()
           + "\n DamageLevel: " + GlobalState.Stats.DamageLevel.ToString()
            + "\n Energy: " + GlobalState.Stats.Energy.ToString()
            + "\n Points: " + GlobalState.Stats.Points.ToString());
        }
        if (GlobalState.GameState != stateLib.GAMESTATE_MENU)
        {
            foreach (GameObject button in buttons)
            {
                button.GetComponent<SpriteRenderer>().color = Color.grey;
            }
        }
        else
        {
            foreach (GameObject button in buttons)
            {
                button.GetComponent<SpriteRenderer>().color = Color.white;
            }
        }
        if ((GlobalState.passed == null || GlobalState.passed.Count < 1))
        {
            buttons[stateLib.GAMEMENU_LOAD_GAME].GetComponent<SpriteRenderer>().color = Color.grey;
        }
        else if (!menu2.GetComponent<SpriteRenderer>().enabled)
        {
            buttons[stateLib.GAMEMENU_LOAD_GAME].GetComponent<SpriteRenderer>().color = Color.white;
        }
        // Handle "Resume Game" button behavior. If we have a game session we can click it, otherwise grey it out. --[
        if (!GlobalState.IsResume)
        {
            buttons[stateLib.GAMEMENU_RESUME_GAME].GetComponent<SpriteRenderer>().color = Color.grey;
        }
        else if (!menu2.GetComponent<SpriteRenderer>().enabled)
        {
            buttons[stateLib.GAMEMENU_RESUME_GAME].GetComponent<SpriteRenderer>().color = Color.white;
        }
        // ]-- End of "Resume Game" button behavior.
        // If we are in the menu, handle up and down arrows --[
        if (GlobalState.GameState == stateLib.GAMESTATE_MENU)
        {

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                // The previous button should be made blue (change from green to blue).
                // If we are on the first option (New Game), don't allow the up arrow to wrap-around.
                buttons[option].GetComponent<SpriteRenderer>().sprite = bluebutton;
                option = (option == stateLib.GAMEMENU_NEW_GAME) ? stateLib.GAMEMENU_NEW_GAME : option - 1;

                if (option == stateLib.GAMEMENU_LOAD_GAME && buttons[stateLib.GAMEMENU_LOAD_GAME].GetComponent<SpriteRenderer>().color == Color.grey)
                {
                    option -= 1;
                }
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                // The previous button should be made blue (change from green to blue).
                // The last option will be either Resume Game or Exit game. In either case, don't allow the down arrow to wrap-around.
                buttons[option].GetComponent<SpriteRenderer>().sprite = bluebutton;
                if (GlobalState.IsResume)
                {
                    option = (option == stateLib.GAMEMENU_RESUME_GAME) ? stateLib.GAMEMENU_RESUME_GAME : option + 1;
                }
                else
                {
                    option = (option == stateLib.GAMEMENU_EXIT_GAME) ? stateLib.GAMEMENU_EXIT_GAME : option + 1;
                }

                if (option == stateLib.GAMEMENU_LOAD_GAME && buttons[stateLib.GAMEMENU_LOAD_GAME].GetComponent<SpriteRenderer>().color == Color.grey)
                {
                    option += 1;
                }
            }

            // ]-- End of Arrow controller.

            // Make the current button appear green. The previous buttons change to blue
            // because there is an Up or Down arrow event. This will fire outside of the event.
            buttons[option].GetComponent<SpriteRenderer>().sprite = greenbutton;

            // When we press Return (Enter Key), take us to the sub-menus
            if ((entered || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) && delaytime < Time.time)
            {
                entered = false;
                switch (option)
                {
                    case stateLib.GAMEMENU_NEW_GAME:
                        // Select between RobotON or RoboBUG.
                        GlobalState.GameState = -3;
                        buttons[option].GetComponent<SpriteRenderer>().sprite = bluebutton;
                        option = 0;
                        GlobalState.GameState = stateLib.GAMESTATE_LEVEL_START;

                        if (SceneManager.sceneCount > 1)
                        {
                            SceneManager.UnloadSceneAsync("newgame");
                            GlobalState.CurrentONLevel = "tutorial0.xml";
                        }
                        Debug.Log("OldMenu.cs 300 - FilePath = " + GlobalState.FilePath);
                        Debug.Log("OldMenu.cs 300 - CurrentBUGLevel = " + GlobalState.CurrentBUGLevel);
                        SceneManager.LoadScene("CharacterSelect");
                        break;
                    case stateLib.GAMEMENU_LOAD_GAME:
                        // Load a level from RobotON or RoboBUG.
                        if (!(GlobalState.passed == null || GlobalState.passed.Count < 1))
                        {
                            GlobalState.GameState = -4;
                            buttons[option].GetComponent<SpriteRenderer>().sprite = bluebutton;
                            option = 0;
                            // levels.Clear();
                            // passed.Clear();
                            // //lfile = Application.streamingAssetsPath +"/" + GlobalState.GameMode + "leveldata" + filepath + "levels.txt";
                            // readFromFiles();
                            GlobalState.GameState = -1;
                            option = 0;
                            m2buttons[1].GetComponent<SpriteRenderer>().sprite = bluebutton;
                            m2buttontext[0].GetComponent<TextMesh>().text = levels[levoption];
                            m2buttontext[1].GetComponent<TextMesh>().text = "Back";
                            GlobalState.GameState = stateLib.GAMESTATE_MENU_LOADGAME_SUBMENU;
                            m2switch(true);
                        }
                        break;
                    case stateLib.GAMEMENU_SOUND_OPTIONS:
                        GlobalState.GameState = -2;
                        buttons[option].GetComponent<SpriteRenderer>().sprite = bluebutton;
                        option = 0;
                        m2switch(true);
                        if (optionPage == 0)
                        {
                            m2buttontext[0].GetComponent<TextMesh>().text = "Sound: " + (soundon ? GlobalState.StringLib.menu_sound_on_color_tag + "ON" + stringLib.CLOSE_COLOR_TAG : GlobalState.StringLib.menu_sound_off_color_tag + "OFF" + stringLib.CLOSE_COLOR_TAG);
                            m2buttontext[1].GetComponent<TextMesh>().text = (!GlobalState.IsDark) ? "Light Mode" : "Dark Mode";
                            m2buttontext[2].GetComponent<TextMesh>().text = "Next";
                            m2buttontext[3].GetComponent<TextMesh>().text = "Back";
                        }
                        else if (optionPage == 1)
                        {
                            m2buttontext[0].GetComponent<TextMesh>().text = textsizes[textOption];
                            m2buttontext[0].GetComponent<TextMesh>().fontSize = fontSizes[textOption];
                            m2buttontext[1].GetComponent<TextMesh>().text = (GlobalState.Language == "c++") ? "C++" : "Python";
                            m2buttontext[2].GetComponent<TextMesh>().text = "Previous";
                            m2buttontext[3].GetComponent<TextMesh>().text = "Back";
                        }
                        break;
                    case stateLib.GAMEMENU_EXIT_GAME:
                        GlobalState.CurrentONLevel = null;
                        GlobalState.IsResume = false;
                        GlobalState.Stats = null;
                        SceneManager.LoadScene("TitleScene");
                        break;
                    case stateLib.GAMEMENU_RESUME_GAME:
                        GlobalState.GameState = stateLib.GAMESTATE_IN_GAME;
                        buttons[option].GetComponent<SpriteRenderer>().sprite = bluebutton;
                        GlobalState.IsResume = false;

                        Debug.Log("Resume Button Pressed");
                        Debug.Log("Resume OM: " + GlobalState.IsResume);
                        Debug.Log("Level OM: " + GlobalState.CurrentBUGLevel);
                        Debug.Log("Fail: " + GlobalState.failures);

                        if (SceneManager.sceneCount > 1)
                            SceneManager.UnloadSceneAsync("MainMenu");
                        else
                        {
                            SceneManager.LoadScene("newgame");
                        }
                        break;
                    default:
                        break;
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && GlobalState.GameState < 0)
        {
            m2switch(false);
            flushButtonColor();
            GlobalState.GameState = stateLib.GAMESTATE_MENU;
        }
        else if (GlobalState.GameState == stateLib.GAMESTATE_MENU_LOADGAME_SUBMENU)
        {
            if (levoption < levels.Count - 1 && passed[levoption] == "1")
            {
                m2arrows[1].GetComponent<SpriteRenderer>().enabled = true;
            }
            else
            {
                m2arrows[1].GetComponent<SpriteRenderer>().enabled = false;
            }
            if (levoption != 0)
            {
                m2arrows[0].GetComponent<SpriteRenderer>().enabled = true;
            }
            else
            {
                m2arrows[0].GetComponent<SpriteRenderer>().enabled = false;
            }
            m2buttons[option].GetComponent<SpriteRenderer>().sprite = greenbutton;
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                m2buttons[1].GetComponent<SpriteRenderer>().sprite = bluebutton;
                option = 0;
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                m2buttons[0].GetComponent<SpriteRenderer>().sprite = bluebutton;
                option = 1;
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (levoption < levels.Count - 1 && passed[levoption] == "1")
                {
                    levoption++;
                }
                m2buttontext[0].GetComponent<TextMesh>().text = levels[levoption];
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                levoption = (levoption == 0) ? 0 : levoption - 1;
                m2buttontext[0].GetComponent<TextMesh>().text = levels[levoption];
            }
            if ((entered || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)))
            {
                entered = false;
                switch (option)
                {
                    case 0:
                        GlobalState.GameState = stateLib.GAMESTATE_LEVEL_START;
                        GlobalState.IsResume = false;

                        if (SceneManager.sceneCount > 1)
                            SceneManager.UnloadSceneAsync("newgame");
                        GlobalState.level = null;
                        GlobalState.CurrentONLevel = levels[levoption];
                        Debug.Log("OldMenu.cs 425 - FilePath = " + GlobalState.FilePath);
                        Debug.Log("OldMenu.cs 425 - CurrentBUGLevel = " + GlobalState.CurrentBUGLevel);
                        SceneManager.LoadScene("CharacterSelect");

                        buttons[4].GetComponent<SpriteRenderer>().color = Color.white;
                        m2switch(false);
                        break;
                    case 1:
                        m2switch(false);
                        flushButtonColor();
                        GlobalState.GameState = stateLib.GAMESTATE_MENU;
                        break;
                    default:
                        break;
                }
            }

        }

        else if (GlobalState.GameState == stateLib.GAMESTATE_MENU_SOUNDOPTIONS)
        {
            m2buttons[option].GetComponent<SpriteRenderer>().sprite = greenbutton;
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                m2buttons[option].GetComponent<SpriteRenderer>().sprite = bluebutton;
                option = (option - 1 < 0) ? option = 3 : option - 1;
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                m2buttons[option].GetComponent<SpriteRenderer>().sprite = bluebutton;
                option = (option + 1) % 4;
            }
            if ((entered || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)))
            {

                switch (option)
                {
                    case 0:
                        foreach (GameObject btn in m2buttons)
                        {
                            btn.GetComponent<SpriteRenderer>().sprite = bluebutton;
                        }
                        m2buttons[0].GetComponent<SpriteRenderer>().sprite = greenbutton;
                        option = 0;
                        if (optionPage > 0)
                        {
                            textOption = (textOption + 1) % textsizes.Length;
                            m2buttontext[0].GetComponent<TextMesh>().text = textsizes[textOption];
                            m2buttontext[0].GetComponent<TextMesh>().fontSize = fontSizes[textOption];
                            GlobalState.TextSize = textOption;
                            break;
                        }
                        soundon = !soundon;
                        m2buttontext[0].GetComponent<TextMesh>().text = "Sound: " + ((soundon) ? GlobalState.StringLib.menu_sound_on_color_tag + "ON" + stringLib.CLOSE_COLOR_TAG : GlobalState.StringLib.menu_sound_off_color_tag + "OFF" + stringLib.CLOSE_COLOR_TAG);
                        AudioListener.volume = (soundon) ? 1 : 0;
                        break;
                    case 1:
                        foreach (GameObject btn in m2buttons)
                        {
                            btn.GetComponent<SpriteRenderer>().sprite = bluebutton;
                        }
                        m2buttons[1].GetComponent<SpriteRenderer>().sprite = greenbutton;
                        option = 1;
                        if (optionPage > 0)
                        {
                            if (!GlobalState.RestrictGameMode)
                            {
                                m2buttontext[1].GetComponent<TextMesh>().text = (GlobalState.Language == "c++") ? "Python" : "C++";
                                GlobalState.Language = (GlobalState.Language == "c++") ? "python" : "c++";
                            }
                            break;
                        }
                        ToggleTheme();
                        m2buttontext[1].GetComponent<TextMesh>().text = (!GlobalState.IsDark) ? "Light Mode" : "Dark Mode";
                        break;
                    case 2:
                        if (optionPage == 1)
                        {
                            GlobalState.HideToolTips = !GlobalState.HideToolTips;
                            m2buttontext[2].GetComponent<TextMesh>().text = ("HUD: " + ((GlobalState.HideToolTips) ? GlobalState.StringLib.menu_sound_off_color_tag + "OFF" + stringLib.CLOSE_COLOR_TAG : GlobalState.StringLib.menu_sound_on_color_tag + "ON" + stringLib.CLOSE_COLOR_TAG));
                        }
                        else
                        {
                            optionPage = 1;
                            m2buttontext[0].GetComponent<TextMesh>().text = textsizes[textOption];
                            m2buttontext[0].GetComponent<TextMesh>().fontSize = fontSizes[textOption];
                            m2buttontext[1].GetComponent<TextMesh>().text = (GlobalState.Language == "c++") ? "C++" : "Python";
                            m2buttontext[2].GetComponent<TextMesh>().text = ("HUD: " + ((GlobalState.HideToolTips) ? GlobalState.StringLib.menu_sound_off_color_tag + "OFF" + stringLib.CLOSE_COLOR_TAG : GlobalState.StringLib.menu_sound_on_color_tag + "ON" + stringLib.CLOSE_COLOR_TAG));
                            m2buttontext[3].GetComponent<TextMesh>().text = "Back";
                        }
                        foreach (GameObject btn in m2buttons)
                        {
                            btn.GetComponent<SpriteRenderer>().sprite = bluebutton;
                        }
                        m2buttons[2].GetComponent<SpriteRenderer>().sprite = greenbutton;
                        option = 2;
                        break;
                    case 3:
                        if (optionPage == 0)
                        {
                            GlobalState.GameState = stateLib.GAMESTATE_MENU;
                            m2buttons[3].GetComponent<SpriteRenderer>().sprite = bluebutton;
                            m2switch(false);
                            option = 2;
                            break;
                        }
                        else
                        {
                            optionPage = 0;
                            m2buttontext[0].GetComponent<TextMesh>().fontSize = m2buttontext[1].GetComponent<TextMesh>().fontSize;
                            m2buttontext[0].GetComponent<TextMesh>().text = "Sound: " + (soundon ? GlobalState.StringLib.menu_sound_on_color_tag + "ON" + stringLib.CLOSE_COLOR_TAG : GlobalState.StringLib.menu_sound_off_color_tag + "OFF" + stringLib.CLOSE_COLOR_TAG);
                            m2buttontext[1].GetComponent<TextMesh>().text = (!GlobalState.IsDark) ? "Light Mode" : "Dark Mode";
                            m2buttontext[2].GetComponent<TextMesh>().text = "Next";
                            m2buttontext[3].GetComponent<TextMesh>().text = "Back";
                            foreach (GameObject btn in m2buttons)
                            {
                                btn.GetComponent<SpriteRenderer>().sprite = bluebutton;
                            }
                            m2buttons[2].GetComponent<SpriteRenderer>().sprite = greenbutton;
                            option = 2;
                            break;
                        }
                }
                GlobalState.soundon = soundon;
                SetUserPrefs();
                entered = false;
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (option == 2)
                {
                    textOption = (textOption + 1) % textsizes.Length;
                    m2buttontext[2].GetComponent<TextMesh>().text = textsizes[textOption];
                    GlobalState.TextSize = textOption;
                }
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (option == 2)
                {
                    textOption = (textOption - 1 < 0) ? textOption = textsizes.Length - 1 : textOption - 1;
                    m2buttontext[2].GetComponent<TextMesh>().text = textsizes[textOption];
                    GlobalState.TextSize = textOption;
                }
            }
        }
        else if (GlobalState.GameState == stateLib.GAMESTATE_MENU_NEWGAME)
        {

            m2buttons[option].GetComponent<SpriteRenderer>().sprite = greenbutton;
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                m2buttons[1].GetComponent<SpriteRenderer>().sprite = bluebutton;
                option = 0;
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                m2buttons[0].GetComponent<SpriteRenderer>().sprite = bluebutton;
                option = 1;
            }
            if ((entered || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)))
            {
                GlobalState.GameState = stateLib.GAMESTATE_LEVEL_START;
                GlobalState.IsResume = false;

                SceneManager.LoadScene("CharacterSelect");


            }
        }
        else if (GlobalState.GameState == stateLib.GAMESTATE_MENU_LOADGAME)
        {
            m2buttons[option].GetComponent<SpriteRenderer>().sprite = greenbutton;
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                m2buttons[1].GetComponent<SpriteRenderer>().sprite = bluebutton;
                option = 0;
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                m2buttons[0].GetComponent<SpriteRenderer>().sprite = bluebutton;
                option = 1;
            }
            if ((entered || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)))
            {
                entered = false;



            }
        }
        else
        {
            delaytime = Time.time + delay;
        }
    }

    private void ResetM2()
    {
        m2buttontext[0].GetComponent<TextMesh>().fontSize = m2buttontext[1].GetComponent<TextMesh>().fontSize;
        Transform trans = menu2.GetComponent<Transform>();
        trans.position = new Vector3(trans.position.x, 0, trans.position.z);
        trans.localScale = new Vector3(trans.localScale.x, 0.8f, trans.localScale.z);
        Transform button = m2buttons[0].GetComponent<Transform>();
        button.position = new Vector3(button.position.x, 0.82f, button.position.z);
        button = m2buttons[1].GetComponent<Transform>();
        button.position = new Vector3(button.position.x, -0.945f, button.position.z);
    }
    //.................................>8.......................................
    //************************************************************************//
    // Method: private void m2switch(bool on)
    // Description: Sub-menu switch (menu 2 switch). If bool is TRUE, show the Sub-menu
    // and all the buttons for that sub-menu. If the bool is FALSE, hide the
    // sub-menu, the buttons, text, arrows, etc.
    //************************************************************************//
    private void m2switch(bool on)
    {
        if (on)
        {
            menu2.GetComponent<SpriteRenderer>().enabled = true;
            for (int i = 0; i < m2buttons.Length; i++)
            {
                if (i > 1 && GlobalState.GameState != stateLib.GAMESTATE_MENU_SOUNDOPTIONS)
                    break;
                m2buttons[i].GetComponent<SpriteRenderer>().enabled = true;
            }
            if (GlobalState.GameState == stateLib.GAMESTATE_MENU_SOUNDOPTIONS)
            {

                Transform trans = menu2.GetComponent<Transform>();
                trans.position = new Vector3(trans.position.x, -0.9f, trans.position.z);
                trans.localScale = new Vector3(trans.localScale.x, 1.2f, trans.localScale.z);
                Transform button = m2buttons[0].GetComponent<Transform>();
                button.position = new Vector3(button.position.x, 0.42f, button.position.z);
                for (int i = 0; i < 2; i++)
                {
                    Transform thisButton = m2buttons[i].GetComponent<Transform>();
                    thisButton.position = new Vector3(thisButton.position.x, thisButton.position.y + 0.7f, thisButton.position.z);
                }
                if (firstOption)
                {
                    firstOption = false;
                    for (int i = 2; i < 4; i++)
                    {
                        Transform thisButton = m2buttons[i].GetComponent<Transform>();
                        thisButton.position = new Vector3(thisButton.position.x, thisButton.position.y + 0.7f, thisButton.position.z);
                    }
                }
            }
            else
            {
                ResetM2();
            }
        }
        else
        {
            ResetM2();
            menu2.GetComponent<SpriteRenderer>().enabled = false;
            foreach (GameObject button in m2buttons)
            {
                button.GetComponent<SpriteRenderer>().enabled = false;
            }
            foreach (GameObject btext in m2buttontext)
            {
                btext.GetComponent<TextMesh>().text = "";
            }
            foreach (GameObject arrow in m2arrows)
            {
                arrow.GetComponent<SpriteRenderer>().enabled = false;
            }
        }
    }

    //.................................>8.......................................
    //************************************************************************//
    // Method: public void flushButtonColor()
    // Description: Change all menu and sub-menu buttons to blue. Set the cursor
    // to the New Game option. Basically, reset the menu.
    //************************************************************************//
    public void flushButtonColor()
    {
        m2buttons[0].GetComponent<SpriteRenderer>().sprite = bluebutton;
        m2buttontext[0].GetComponent<TextMesh>().fontSize = m2buttontext[1].GetComponent<TextMesh>().fontSize;
        m2buttons[1].GetComponent<SpriteRenderer>().sprite = bluebutton;
        option = 0;
        buttons[option].GetComponent<SpriteRenderer>().sprite = greenbutton;
    }
    public void readFromFiles()
    {
        levels.Clear();
        passed.Clear();
        if (GlobalState.passed == null) GlobalState.passed = new List<string>();
        string filepath = "";
#if (UNITY_EDITOR || UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN) && !UNITY_WEBGL
            filepath = Path.Combine(Application.streamingAssetsPath, GlobalState.GameMode + "leveldata");
            filepath = Path.Combine(filepath, "levels.txt");

            sr = File.OpenText(filepath);
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                string[] data = line.Split(' ');
                levels.Add(data[0]);
                passed.Add(data[1]);
                if (GlobalState.DebugMode) GlobalState.passed.Add(data[0]); 
            }
            sr.Close();
#endif

#if UNITY_WEBGL
        if (GlobalState.DebugMode)
        {
            filepath = stringLib.SERVER_URL + "StreamingAssets" + "/" + GlobalState.GameMode + "leveldata" + "/levels.txt";
        }
        else
        {
            filepath = stringLib.DB_URL + GlobalState.GameMode.ToUpper() + "/completedlevels/" + GlobalState.sessionID.ToString();
        }
        Debug.Log(filepath);
        WebHelper.i.url = filepath;
        Debug.Log(WebHelper.i.url);
        WebHelper.i.GetWebDataFromWeb();
        filepath = WebHelper.i.webData;
        Debug.Log(filepath);



        if (!filepath.Equals("\"\"") && filepath != null)
        {
            int webHolder = 0;

            string[] leveldata;
            if (GlobalState.DebugMode)
            {
                leveldata = filepath.Split('\n');
            }
            else
            {
                filepath = filepath.Substring(1, filepath.Length - 2);
                leveldata = filepath.Split(',');
                webHolder = 1;
            }

            Regex rgxCheck = new Regex("([0-9][a-z])");

            for (int i = 0; i < leveldata.Length + webHolder - 1; i++)
            {
                string[] tmp = leveldata[i].Split(' ');
                string[] tmpTwo = tmp[1].Split('\r');

                if (tmpTwo[0] == "1") GlobalState.passed.Add(tmp[0]);

                if (rgxCheck.IsMatch(tmp[0]) && GlobalState.DebugMode == false)
                {
                    continue;
                }
                levels.Add(tmp[0]);
                passed.Add(tmpTwo[0]);
            }
        }
#endif

    }

    public void SetUserPrefs()
    {
        if (GlobalState.LoggingMode == false)
        {
            return;
        }
        PlayerPrefs.SetString("language", GlobalState.Language);
        PlayerPrefs.SetInt("textsize", GlobalState.TextSize);
        int sounds = soundon ? 1 : 0;
        PlayerPrefs.SetInt("soundon", sounds);
        int themes = GlobalState.IsDark ? 1 : 0;
        PlayerPrefs.SetInt("themes", themes);
        int toolsTips = GlobalState.HideToolTips ? 1 : 0;
        PlayerPrefs.SetInt("tooltips", toolsTips);
        string url = stringLib.DB_URL + GlobalState.GameMode.ToUpper() + "/points/" + GlobalState.sessionID;
        string json = "{ \"totalPoints\":\"" + GlobalState.totalPoints.ToString() + "\"}";
        string jsonTwo = "{ \"currentPoints\":\"" + GlobalState.Stats.Points.ToString() + "\"}";

        SendPointsToDB(url + "/totalPoints", json);
        SendPointsToDB(url + "/currentPoints", jsonTwo);
    }

    public void GrabUserPrefs()
    {
        if (GlobalState.LoggingMode == false)
        {
            return;
        }

        //Grab the Menu Preference
        //First Check if it exist
        if (PlayerPrefs.HasKey("language"))
        {
            GlobalState.Language = PlayerPrefs.GetString("language", "c++");
        }
        if (PlayerPrefs.HasKey("textsize"))
        {
            GlobalState.TextSize = PlayerPrefs.GetInt("textsize", 1);
        }
        //if(PlayerPrefs.HasKey("soundon")){
        GlobalState.soundon = Convert.ToBoolean(PlayerPrefs.GetInt("soundon", 1));
        soundon = GlobalState.soundon;
        //}
        if (PlayerPrefs.HasKey("themes"))
        {
            GlobalState.IsDark = Convert.ToBoolean(PlayerPrefs.GetInt("themes", 1));
        }
        if (PlayerPrefs.HasKey("tooltips"))
        {
            GlobalState.HideToolTips = Convert.ToBoolean(PlayerPrefs.GetInt("tooltips", 1));
        }

        if (GlobalState.Stats == null)
        {
            GlobalState.Stats = new CharacterStats();
        }
        LoggerPoints lg = new LoggerPoints();
        string json = GrabPointsFromDB(stringLib.DB_URL + GlobalState.GameMode.ToUpper() + "/points/" + GlobalState.sessionID.ToString() + "/totalPoints");
        if (json != "{}" && json != null)
        {
            lg = LoggerPoints.CreateFromJson(json);
            try
            {
                GlobalState.totalPoints = Convert.ToInt32(lg.totalPoints);
            }
            catch (Exception e)
            {
                Debug.Log("Error on getting points, will try again later!");
            }
        }
        else
        {
            GlobalState.totalPoints = 0;
        }

        json = GrabPointsFromDB(stringLib.DB_URL + GlobalState.GameMode.ToUpper() + "/points/" + GlobalState.sessionID.ToString() + "/currentPoints");
        if (json != "{}" && json != null)
        {
            lg = LoggerPoints.CreateFromJson(json);
            try
            {
                GlobalState.Stats.Points = Convert.ToInt32(lg.currentPoints);
            }
            catch (Exception e)
            {
                Debug.Log("Error on gettting upgrades, will try again later!");
            }
        }
        else
        {
            GlobalState.Stats.Points = 0;
        }

        json = GrabPointsFromDB(stringLib.DB_URL + GlobalState.GameMode.ToUpper() + "/points/" + GlobalState.sessionID.ToString() + "/speedUpgrades");
        if (json != "{}" && json != null)
        {
            lg = LoggerPoints.CreateFromJson(json);
            try
            {
                GlobalState.Stats.Speed = Convert.ToInt32(lg.speedUpgrades);
            }
            catch (Exception e)
            {
                Debug.Log("Error on gettting upgrades, will try again later!");
            }
        }

        json = GrabPointsFromDB(stringLib.DB_URL + GlobalState.GameMode.ToUpper() + "/points/" + GlobalState.sessionID.ToString() + "/resistanceUpgrade");
        if (json != "{}" && json != null)
        {
            lg = LoggerPoints.CreateFromJson(json);
            try
            {
                GlobalState.Stats.DamageLevel = Convert.ToInt32(lg.resistanceUpgrade);
            }
            catch (Exception e)
            {
                Debug.Log("Error on gettting upgrades, will try again later!");
            }
        }

        json = GrabPointsFromDB(stringLib.DB_URL + GlobalState.GameMode.ToUpper() + "/points/" + GlobalState.sessionID.ToString() + "/energyUpgrades");
        if (json != "{}" && json != null)
        {
            lg = LoggerPoints.CreateFromJson(json);
            try
            {
                GlobalState.Stats.Energy = Convert.ToInt32(lg.energyUpgrades);
            }
            catch (Exception e)
            {
                Debug.Log("Error on gettting upgrades, will try again later!");
            }
        }

        json = GrabPointsFromDB(stringLib.DB_URL + GlobalState.GameMode.ToUpper() + "/points/" + GlobalState.sessionID.ToString() + "/xpUpgrades");
        if (json != "{}" && json != null)
        {
            lg = LoggerPoints.CreateFromJson(json);
            try
            {
                GlobalState.Stats.XPBoost = Convert.ToInt32(lg.xpUpgrades);
            }
            catch (Exception e)
            {
                Debug.Log("Error on gettting upgrades, will try again later!");
            }
        }
    }
    public void sendInitialDataDB(string name, string time, string url)
    {
        if (GlobalState.LoggingMode == false)
        {
            return;
        }
        /*
        LoggerDataStart start = new LoggerDataStart();
        start.name = GlobalState.sessionID.ToString();
        start.username = GlobalState.username;
        start.timeStarted = DateTime.Now.ToString();
        */

        LoggerCourseCodeStart start = new LoggerCourseCodeStart();
        start.courseCode = GlobalState.courseCode;

        LoggerDataStart sub = new LoggerDataStart();
        sub.name = name;
        sub.username = GlobalState.username;
        sub.timeStarted = time;

        start.studentStart = sub;

        String json = JsonUtility.ToJson(start);
        DatabaseHelperV2.i.url = url;
        DatabaseHelperV2.i.jsonData = json;
        DatabaseHelperV2.i.PostToDataBase();

    }

    public string GrabPointsFromDB(string url)
    {
        WebHelper.i.url = url;
        Debug.Log(WebHelper.i.url);
        WebHelper.i.GetWebDataFromWeb();
        return WebHelper.i.webData;
    }

    public void SendPointsToDB(string url, string json)
    {
        if (GlobalState.LoggingMode == false)
        {
            return;
        }
        DatabaseHelperV2.i.url = url;
        DatabaseHelperV2.i.jsonData = json;
        DatabaseHelperV2.i.PutToDataBase();
    }
}


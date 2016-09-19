//****************************************************************************//
// @TODO: Ensure this is up to date when finished.
// Class Name: LevelGenerator
// Class Description: Master class for most of the game's functionality, this runs when a new level
//                    is to be generatd. This class behaves like a factory for game objects, displays the lines
//                    of code, controls light/dark scheme, text size, and level win/loss.
// Methods:
// 		private void Start()
//		private void Update()
//		public void GUISwitch(bool gui_on)
//		public void BuildLevel(string filename, bool warp, string linenum = "")
//		private void StoreInnerXml(XmlNode levelnode)
//		public string NodeToColorString(XmlNode thisNode, bool newline = true)
//		private void PlaceObjects(XmlNode levelnode)
//		public void ProvisionToolsFromXml(XmlNode levelnode)
//		public void ResetLevel(bool warp)
//		public void GameOver()
//		public void Victory()
//		private void OnTriggerEnter2D(Collider2D collidingObj)
// Author: Michael Miljanovic
// Date Last Modified: 6/1/2016
//****************************************************************************//

using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Text;
using System.Xml;
using System.Text.RegularExpressions;
using System;

public class LevelGenerator : MonoBehaviour {
	public stringLib stringLibrary = new stringLib();
	// A state-transition variable. When this becomes true, when Update() is called it will trigger a Game Over state.
	public bool isLosing;
	public bool isAnswering = false;
	public bool backgroundLightDark = false;
	public bool sidebarToggle = true;
	// The amount of time the player has to complete this level. Read from XML file.
	public float endTime = 0f;
	// The amount of time remaining, in seconds, until the level is considered lost.
	public float remainingTime = 0f;
	// The number of Bugs remaining in this level. Originally read from XML file, it will decrease as players squash bugs.
	public int numberOfBugsRemaining = 0;
	// Number of in-flight wrenches/shurikens.
	public int toolsAirborne = 0;
	// Refer to stateLib for distinct gamestates
	public int gamestate;
	// Contains the remaining tasks the player must complete before winning the level.
	public int[] tasklist = new int[5];
	// Contains the completed tasks of the player.
	public int[] taskscompleted = new int[5];
	// The number of lines in the XML file. Computed by counting the number of newline characters the XML contains.
	public int linecount = 0;
	public int renamegroupidCounter = 0;
	// Lines of code stored in an array. innerXmlLines is the colorized text from NodeToColorString(), outerXmlLnes is the line with the tags.
	public string[] innerXmlLines;
	public string[] outerXmlLines;
	public string[] lineNumbers;
	// The filename of the next XML file to read from.
	public string nextlevel = "";
	// The current level, contains the filename of the XML loaded.
	public string currentlevel = "level0.xml";
	// Game Mode is either "on" or "bug", for RobotON or RoboBUG respectively. This is defined in stringLib.
	public string gamemode;
	public string language;
	// Stores the audio clips used in the game.
	public AudioClip[] sounds = new AudioClip[10];
	// Stores the icons for each tool.
	public GameObject[] toolIcons = new GameObject[stateLib.NUMBER_OF_TOOLS];
    public GameObject[] toolLabels = new GameObject[stateLib.NUMBER_OF_TOOLS];
	// Stores the tasks for each line.
	public int[,] taskOnLines;
	// Stores the time remaining on the sidebar.
	public GameObject sidebartimer;
	// Stores the level text, the lines of code the player sees.
	public GameObject leveltext;
	// Stores the level's (Displayed at the top of the level when playing).
	public GameObject destext;
	public GameObject bugobject;
	public GameObject beaconobject;
	public GameObject lineobject;
	public GameObject printobject;
	public GameObject commentobject;
	public GameObject questionobject;
	public GameObject prizeobject;
	public GameObject renameobject;
	public GameObject variablecolorobject;
	public GameObject warpobject;
	public GameObject breakpointobject;
	public GameObject hero;
	public GameObject sidebaroutput;
	public GameObject backgroundImage;
	public GameObject sidebarChecklist;
	public GameObject sidebarLabel;
    public GameObject sidebarDescription;
	public Texture2D lightBackground;
	public Texture2D darkBackground;
	public Sprite whiteCodescreen;
	public Sprite blackCodescreen;
	public Sprite panel2;
	public Texture2D panel3;
	public Sprite panel4;
	public Texture2D panel6;
	public Texture2D panel5;
	public Texture2D panel7;
	public Sprite panel8;
	public Sprite panel9;
	// Reference to SelectedTool object. When ProvisionToolsFromXml() is called, tools are provisioned and then passed to SelectedTool object.
	public GameObject selectedtool;
	public GameObject sidebarpanel;
	public GameObject outputpanel;
	public GameObject outputEnter;
	public GameObject cinematic;
	public GameObject cinematicEnter;
	public GameObject cinematicVoidMain;
	public GameObject menu;
	public GameObject menuSubmenu;
	public GameObject menuTitle;
	public GameObject credits;
	public GameObject toolprompt;

	public Vector3 defaultPosition = new Vector3(0,0,0);
	public Vector3 defaultLocalScale = new Vector3(0,0,0);
	// Player has been notified of less than 30 seconds remaining on the clock.
	private bool isTimerAlarmTriggered;
	private bool winning;
	private bool storedDefaultPlayArea = false;
    private bool initialresize = false;
	// This 3f is not a typo, it's different from the initialLineY in stateLib
	private float initialLineY 							= 3f;
	private float initialLineX 							= -4.47f;
	// Spacing between lines.
	public float linespacing 							= 0.825f;
	// Offset for line spacing.
	private float lineOffset							= -0.3f;
	// Used for Bugs, prizes, and resizing the play area
	private float levelLineRatio 						= 0.55f;
	private float bugXshift 							= -9.5f;
	private float fontwidth 							= 0.15f;
	private float bugsize 								= 1f;
	private float bugscale 								= 1.5f;
	private float textscale 							= 1.75f;
	private float losstime;
	private float lossdelay 							= 3f;
	private float leveldelay 							= 2f;
	private float startNextLevelTimeDelay 				= 0f;
	private float startTime 							= 0f;
	private int   totalNumberOfTools 				    = stateLib.NUMBER_OF_TOOLS;
	private string codetext;
	private GameObject levelbug;

	public List<GameObject> robotONrenamers;
	public List<GameObject> robotONvariablecolors;
	public List<GameObject> lines;
	public List<GameObject> prints;
	public List<GameObject> roboBUGwarps;
	public List<GameObject> bugs;
	public List<GameObject> roboBUGcomments;

	public List<GameObject> robotONcorrectComments;
	public List<GameObject> robotONincorrectComments;
	public List<GameObject> robotONcorrectUncomments;
	public List<GameObject> robotONincorrectUncomments;
	public List<GameObject> robotONquestions;
	public List<GameObject> roboBUGbreakpoints;
	public List<GameObject> roboBUGprizes;
	// Stores the robotONbeacons used in this level.
	public List<GameObject> robotONbeacons;

	//.................................>8.......................................
	// Use this for initialization
	private void Start() {
		gamemode 					 = stringLib.GAME_MODE_BUG;
		losstime 					 = 0;
		lines 						 = new List<GameObject>();
		prints 					 	 = new List<GameObject>();
		bugs 						 = new List<GameObject>();
		roboBUGwarps 				 = new List<GameObject>();
		roboBUGcomments 			 = new List<GameObject>();
		roboBUGbreakpoints	 		 = new List<GameObject>();
		roboBUGprizes 				 = new List<GameObject>();
		robotONbeacons 				 = new List<GameObject>();
		robotONincorrectComments 	 = new List<GameObject>();
		robotONcorrectUncomments 	 = new List<GameObject>();
		robotONincorrectUncomments 	 = new List<GameObject>();
		robotONrenamers 			 = new List<GameObject>();
		robotONvariablecolors		 = new List<GameObject>();
		robotONcorrectComments 		 = new List<GameObject>();
		robotONquestions 			 = new List<GameObject>();
		isLosing 					 = false;
		gamestate 					 = stateLib.GAMESTATE_MENU;
		for (int i = 0; i < 5; i++) {
			tasklist[i] = 0;
			taskscompleted[i] = 0;
		}
		GUISwitch(false);
		isTimerAlarmTriggered = false;
		winning = false;
	}

	//.................................>8.......................................
	//************************************************************************//
	// Method: private void Update()
	// Description: Update the remaining time, check for remaining tasks, handle win and loss transitions, handle menu toggle event
	//************************************************************************//
	// Update is called once per frame
	private void Update() {
		if (gamestate == stateLib.GAMESTATE_IN_GAME)
		{
			// Running out of time. --[
			if (endTime - startTime >= 9000) {
				sidebartimer.GetComponent<GUIText>().text = "Time Remaining: --:--:--";
			}
			else if (endTime - Time.time < 30) {
				sidebartimer.GetComponent<GUIText>().text = "Time Remaining: <size=50><color=red>" + ((int)(endTime - Time.time)).ToString() + "</color></size> seconds";
				if (!isTimerAlarmTriggered) {
					isTimerAlarmTriggered = true;
					sidebartimer.GetComponent<AudioSource>().Play();
				}
			}
			// ]-- End of Out of Time
			// Not running out of time, so handle the time here --[
			else {
				// Time Controller --[
				int nNumberOfSeconds = (int)(endTime - Time.time);
				if (nNumberOfSeconds > 3600) {
					int nNumberOfHours = nNumberOfSeconds / 3600;
					nNumberOfSeconds -= nNumberOfHours * 3600;
					int nNumberOfMinutes = (nNumberOfSeconds) / 60;
					nNumberOfSeconds -= nNumberOfMinutes * 60;
					sidebartimer.GetComponent<GUIText>().text = "Time Remaining: ";
					if (nNumberOfHours < 10) sidebartimer.GetComponent<GUIText>().text += "0";
					sidebartimer.GetComponent<GUIText>().text += nNumberOfHours.ToString() + ":";
					if (nNumberOfMinutes < 10) sidebartimer.GetComponent<GUIText>().text += "0";
					sidebartimer.GetComponent<GUIText>().text += nNumberOfMinutes.ToString() + ":";
					if (nNumberOfSeconds < 10) sidebartimer.GetComponent<GUIText>().text += "0";
					sidebartimer.GetComponent<GUIText>().text += nNumberOfSeconds.ToString();
				}
				else if (nNumberOfSeconds > 60) {
					int nNumberOfMinutes = nNumberOfSeconds / 60;
					nNumberOfSeconds -= nNumberOfMinutes * 60;
					sidebartimer.GetComponent<GUIText>().text = "Time Remaining: 00:";
					if (nNumberOfMinutes < 10) sidebartimer.GetComponent<GUIText>().text += "0";
					sidebartimer.GetComponent<GUIText>().text += nNumberOfMinutes.ToString() + ":";
					if (nNumberOfSeconds < 10) sidebartimer.GetComponent<GUIText>().text += "0";
					sidebartimer.GetComponent<GUIText>().text += nNumberOfSeconds.ToString() + ":";
				}
				else {
					sidebartimer.GetComponent<GUIText>().text = "Time Remaining: 00:00:";
					if (nNumberOfSeconds < 10) sidebartimer.GetComponent<GUIText>().text += "0";
					 sidebartimer.GetComponent<GUIText>().text += nNumberOfSeconds.ToString();
				}
				isTimerAlarmTriggered = false;
				// ]-- End of Time Controller
			}
			// ]-- End of Out of Time else block.

			// Win condition check for RobotON, this is necessary because the win condition is a checklist
			// and the checklist can be completed in any order --[
			if (gamemode == stringLib.GAME_MODE_ON) {
				winning = true;
				for (int i = 0; i < 5; i++) {
					if (tasklist[i] != taskscompleted[i]) {
						winning = false;
					}
				}
			}
			// ]--
			// For either RobotON or RoboBUG, if we're losing, play the audio clip and show the game over screen. --[
			if (isLosing) {
				if (losstime == 0) {
					GetComponent<AudioSource>().clip = sounds[0];
					GetComponent<AudioSource>().Play();
					losstime = Time.time + lossdelay;
				}
				else if (losstime < Time.time) {
					losstime = 0;
					isLosing = false;
					GameOver();
				}
			}
			// ]-- End of Losing blocktext
			// Handle win conditions --[
			if (numberOfBugsRemaining <= 0 && bugs.Count > 0 || winning) {
				if (startNextLevelTimeDelay == 0f) {
					startNextLevelTimeDelay = Time.time + leveldelay;
				}
				else if (Time.time > startNextLevelTimeDelay) {
					winning = false;
					startNextLevelTimeDelay = 0f;
					if (nextlevel != gamemode + "leveldata" + menu.GetComponent<Menu>().filepath) {
						// Destroy the bugs in this level and go to win screen.
						foreach (GameObject bug in bugs) {
							Destroy(bug);
						}
						GUISwitch(false);
						print("Savegame (currentlevel): " + currentlevel);
						menu.GetComponent<Menu>().saveGame(currentlevel);
						gamestate = stateLib.GAMESTATE_LEVEL_WIN;
					}
					else {
						// Credits
						Victory();
					}
				}

			}
			// ]-- End of Win Conditions
			// Handle out of time --[
			if (endTime < Time.time && (numberOfBugsRemaining > 0 || bugs.Count == 0) && endTime - startTime < 9000) {
				GameOver();
			}
			// ]--
			// Handle menu toggle (Escape key pressed) --[
			if (Input.GetKeyDown(KeyCode.Escape) && !isAnswering) {
				gamestate = stateLib.GAMESTATE_MENU;
				menu.GetComponent<Menu>().flushButtonColor();
				GUISwitch(false);
			}
			// ]--
			else if (Input.GetKeyDown(KeyCode.X) && !isAnswering) {
				TransformTextSize(leveltext.GetComponent<TextMesh>().fontSize);
			}
			else if (Input.GetKeyDown(KeyCode.Z) && !isAnswering) {
				ToggleLightDark();
			}
			else if (Input.GetKeyDown(KeyCode.C) && !isAnswering) {
				sidebarToggle = (sidebarToggle) ? false : true;
				sidebartimer.GetComponent<GUIText>().enabled = sidebarToggle;
				sidebarChecklist.GetComponent<GUIText>().enabled = sidebarToggle;
				sidebarLabel.GetComponent<GUIText>().enabled = sidebarToggle;
				sidebarpanel.GetComponent<GUITexture>().enabled = sidebarToggle;
                sidebarDescription.GetComponent<GUIText>().enabled = sidebarToggle;
                // transform the bottoms to the right. Store their original position first.
                for (int i = 0 ; i < stateLib.NUMBER_OF_TOOLS ; i++) {
                    if (sidebarToggle) {
                        toolIcons[i].transform.position -= new Vector3(0.18f, 0, 0);
                        toolLabels[i].transform.position -= new Vector3(0.18f, 0, 0);
                    }
                    else {
                        toolIcons[i].transform.position += new Vector3(0.18f, 0, 0);
                        toolLabels[i].transform.position += new Vector3(0.18f, 0, 0);
                    }
                }

			}
		}
	}

	//.................................>8.......................................
	//************************************************************************//
	// Method: public void GUISwitch(bool gui_on)
	// Description: Toggle the GUI on or off
	//************************************************************************//
	public void GUISwitch(bool gui_on) {
        switch(gui_on) {
            case true:
                sidebarpanel.GetComponent<GUITexture>().enabled = (sidebarToggle) ? true : false;
                outputpanel.GetComponent<GUITexture>().enabled = true;
                endTime = remainingTime + Time.time;
            break;

            case false:
                sidebarpanel.GetComponent<GUITexture>().enabled = false;
                outputpanel.GetComponent<GUITexture>().enabled = false;
                sidebartimer.GetComponent<GUIText>().text = "";
                remainingTime = endTime - Time.time;
            break;

            default: break;
        }
	}

	//.................................>8.......................................
	//************************************************************************//
	// Method: public void BuildLevel(string filename, bool warp, string linenum = "")
	// Description: Driver for level creation. If we're warping to this level, behavior is different.
	// linenum is the line number the player should appear on, if warping.
	//************************************************************************//
	public void BuildLevel(string filename, bool warp, string linenum = "")	{
		ResetLevel(warp);
		XmlDocument doc = new XmlDocument();
		doc.Load(filename);

		XmlNode levelnode = doc.FirstChild;
		StoreOuterXml(levelnode);
		StoreInnerXml(levelnode);
		taskOnLines = new int[linecount,stateLib.NUMBER_OF_TOOLS];
		PlaceObjects(levelnode);

		if (warp) {
            hero.transform.position = (linenum != "")  ? new Vector3(-7, initialLineY -(int.Parse(linenum) - 1) * linespacing, 1) : hero.transform.position;
			GetComponent<AudioSource>().clip = sounds[1];
			GetComponent<AudioSource>().Play();
		}
		else {
			ProvisionToolsFromXml(levelnode);
            selectedtool.GetComponent<SelectedTool>().NextTool();
			currentlevel = filename.Substring(filename.IndexOf(menu.GetComponent<Menu>().filepath) + 1);
			startTime = Time.time;
			foreach (XmlNode node in levelnode.ChildNodes) {
                switch (node.Name) {
                    case stringLib.NODE_NAME_TIME:
                        node.InnerText = (node.InnerText.ToLower() == "unlimited") ? "9001" : node.InnerText;
                        endTime = (float)int.Parse(node.InnerText) + startTime;
                        remainingTime = (float)int.Parse(node.InnerText);
                    break;
                    case stringLib.NODE_NAME_NEXT_LEVEL: nextlevel = gamemode + "leveldata" + menu.GetComponent<Menu>().filepath + node.InnerText; break;
                    case stringLib.NODE_NAME_INTRO_TEXT: cinematic.GetComponent<Cinematic>().introtext = node.InnerText; break;
                    case stringLib.NODE_NAME_END_TEXT: cinematic.GetComponent<Cinematic>().endtext = node.InnerText; break;
                    default: break;
                }
			}
		}
		// Resize the boundaries of the level to correspond with how many lines we have
		if (leveltext.GetComponent<TextMesh>().fontSize == stateLib.TEXT_SIZE_VERY_LARGE) {
			this.transform.position -= new Vector3(0, linecount * linespacing / 2, 0);
			this.transform.position += new Vector3(2.2f, 0, 0);
			this.transform.localScale += new Vector3(2, levelLineRatio * linecount, 0);
		}
		else {
			this.transform.position -= new Vector3(0, linecount * linespacing / 2, 0);
			this.transform.localScale += new Vector3(0.1f, levelLineRatio * linecount, 0);
		}
		DrawInnerXmlLinesToScreen();
        if (!initialresize) {
            // Make the text large in size for first run.
            initialresize = true;
            TransformTextSize(leveltext.GetComponent<TextMesh>().fontSize);
        }

	}

	//.................................>8.......................................
	//************************************************************************//
	// Method: private void StoreOuterXml(XmlNode levelnode)
	// Description: Read through levelnode XML and store the outerXml lines. These
	// will need to be parsed later to create the level text.
	//************************************************************************//
	private void StoreOuterXml(XmlNode levelnode) {
		string outerXml = "";
		int outerXmlLineCount = 0;

		foreach (XmlNode codenode in levelnode.ChildNodes) {
			if (codenode.Name == stringLib.NODE_NAME_CODE) outerXml += codenode.OuterXml;
		}

		// Insert newlines between tags. Don't do this for <bug> tags though
		Regex rgxTags = new Regex("><(?!bug)");
		outerXml = rgxTags.Replace(outerXml, ">\n<");

		foreach (char c in outerXml) {
			if (c == '\n') outerXmlLineCount++;
		}

		outerXmlLines = new string[outerXmlLineCount+1];
		Regex rgxNewlineSplit = new Regex("\n");
		string[] substrings = rgxNewlineSplit.Split(outerXml);
		int iterator = 0;
		//@TODO
		// VERY IMPORTANT: Rename and Variable Color scripts will BREAK if the XML file has a newline right after <code language="lang">
		// The first line of the code must start on the SAME LINE as the <code> tag. I need to check for this case and correct it if the level
		// designer puts a newline right after. If they do, the indexing of outerXmlLines[] will be offset by +1, and it will cause the rename and variable color references
		// to be wrong.
		foreach(string s in substrings) {
			outerXmlLines[iterator] = s;
			iterator++;
		}
	}

	//.................................>8.......................................
	//************************************************************************//
	// Method: private void StoreInnerXml(XmlNode levelnode)
	// Description: Read through levelnode XML and store the InnerXML
	//************************************************************************//
	private void StoreInnerXml(XmlNode levelnode) {
		destext.GetComponent<TextMesh>().text = "";
		codetext = "";
		foreach (XmlNode codenode in levelnode.ChildNodes) {
            switch(codenode.Name) {
                case stringLib.NODE_NAME_CODE:
                    foreach (XmlNode printnode in codenode.ChildNodes) {
                        if (printnode.Name == stringLib.NODE_NAME_QUESTION || printnode.Name == stringLib.NODE_NAME_NEWLINE) {
                            printnode.InnerText = NodeToColorString(codenode, printnode, false);
                            printnode.InnerText += "\n";
                        }
                        else {
                            printnode.InnerText = NodeToColorString(codenode, printnode, true);
                        }
                        codetext += printnode.InnerText;
                    }
                break;

                case stringLib.NODE_NAME_DESCRIPTION: destext.GetComponent<TextMesh>().text = codenode.InnerText; break;
                default: break;
            }
		}

		foreach (char c in codetext) {
			if (c == '\n') linecount++;
		}

		// Create the grey line objects for each line.
        lineNumbers = new string[linecount];
		for (int i = 0; i < linecount; i++) {
			float fTransform = initialLineY - i * linespacing + lineOffset;
			GameObject newline = (GameObject)Instantiate(lineobject, new Vector3(initialLineX, fTransform, 1.1f), transform.rotation);
			newline.transform.localScale += new Vector3(0.35f, 0, 0);
			if (leveltext.GetComponent<TextMesh>().fontSize == stateLib.TEXT_SIZE_VERY_LARGE) {
				newline.transform.position += new Vector3(2.4f, 0, 0);
				newline.transform.localScale += new Vector3(0.8f, 0, 0);
			}
			lines.Add(newline);
		}

		// Store the entire codetext (The code displayed in the game's level)
		innerXmlLines = new string[linecount+1];
		Regex rgxNewlineSplit = new Regex("\n");
		string[] substrings = rgxNewlineSplit.Split(codetext);
		int iterator = 0;
		foreach(string s in substrings) {
			innerXmlLines[iterator] = s;
			iterator++;
		}
	}

	//.................................>8.......................................
	//************************************************************************//
	// Method: public string NodeToColorString(XmlNode thisNode);
	// Description: Read an XML node and return a colorized string
	//************************************************************************//
    public string NodeToColorString(XmlNode codenode, XmlNode thisNode, bool newline = true) {
        string sReturn = "";
        switch (thisNode.Name) {
            case stringLib.NODE_NAME_NEWLINE:
                sReturn = "";
                break;
            case stringLib.NODE_NAME_PRINT:
                sReturn = stringLibrary.node_color_print + thisNode.InnerText + stringLib.CLOSE_COLOR_TAG;
                break;
            case stringLib.NODE_NAME_WARP:
                sReturn = stringLibrary.node_color_warp + thisNode.InnerText + stringLib.CLOSE_COLOR_TAG;
                break;
            case stringLib.NODE_NAME_RENAME:
                sReturn = stringLibrary.node_color_rename + thisNode.InnerText + stringLib.CLOSE_COLOR_TAG;
                break;
            case stringLib.NODE_NAME_QUESTION:
                sReturn = stringLibrary.node_color_question + thisNode.InnerText + stringLib.CLOSE_COLOR_TAG;
                break;
            case stringLib.NODE_NAME_VARIABLE_COLOR:
                sReturn = stringLibrary.node_color_rename + thisNode.InnerText + stringLib.CLOSE_COLOR_TAG;
                break;
            case stringLib.NODE_NAME_COMMENT: {
				string commentStyle;
				string commentLanguage;
				string multilineCommentOpenSymbolPython = @"'''";
				string multilineCommentCloseSymbolPython = @"'''";
				string multilineCommentOpenSymbolCpp = @"/* ";
				string multilineCommentCloseSymbolCpp = @" */";
				string singlelineCommentOpenSymbolPython = @"# ";
				string singlelineCommentOpenSymbolCpp = @"// ";
				string commentOpenSymbol = multilineCommentOpenSymbolPython;
				string commentCloseSymbol = multilineCommentCloseSymbolPython;
				try {
					commentStyle = thisNode.Attributes[stringLib.XML_ATTRIBUTE_COMMENT_STYLE].Value;
					commentLanguage = codenode.Attributes[stringLib.XML_ATTRIBUTE_LANGUAGE].Value;
				}
				catch {
					commentStyle = "single";
					commentLanguage = "default";
				}
				switch(commentLanguage) {
					case "python": {
						commentOpenSymbol = (commentStyle == "multi") ? multilineCommentOpenSymbolPython : singlelineCommentOpenSymbolPython;
						commentCloseSymbol = (commentStyle == "multi") ? multilineCommentCloseSymbolPython : "";
						break;
					}
					case "c":
					case "c++":
					case "c#": {
						commentOpenSymbol = (commentStyle == "multi") ? multilineCommentOpenSymbolCpp : singlelineCommentOpenSymbolCpp;
						commentCloseSymbol = (commentStyle == "multi") ? multilineCommentCloseSymbolCpp : "";
						break;
					}
					default: {
						commentOpenSymbol = (commentStyle == "multi") ? multilineCommentOpenSymbolPython : singlelineCommentOpenSymbolPython;
						commentCloseSymbol = (commentStyle == "multi") ? multilineCommentCloseSymbolPython : "";
						break;
					}
				}
                switch(thisNode.Attributes[stringLib.XML_ATTRIBUTE_TYPE].Value) {
                    case "description":
                        switch(thisNode.Attributes[stringLib.XML_ATTRIBUTE_CORRECT].Value) {
                            case "true":
                                sReturn = thisNode.InnerText;
                                break;
                            case "false":
                                sReturn = thisNode.InnerText;
                                break;
                            default:
                                break;
                        }
                        break;
                    case "code":
						string[] sNewParts = thisNode.InnerText.Split('\n');
                        switch(thisNode.Attributes[stringLib.XML_ATTRIBUTE_CORRECT].Value) {
                            case "true":
								if (sNewParts.Length != 1 && commentStyle == "single") {
									// multiple lines using single-line commenting style
									sReturn = "";
									for (int i = 0 ; i < sNewParts.Length ; i++) {
										sReturn += stringLibrary.node_color_uncomment + commentOpenSymbol + sNewParts[i] + commentCloseSymbol + stringLib.CLOSE_COLOR_TAG;
										sReturn += "\n";
									}
								}
								else {
									sReturn = stringLibrary.node_color_uncomment + commentOpenSymbol + thisNode.InnerText + commentCloseSymbol + stringLib.CLOSE_COLOR_TAG;
								}
                                break;
                            case "false":
								if (sNewParts.Length != 1 && commentStyle == "single") {
									// multiple lines using single-line commenting style.
									sReturn = "";
									for (int i = 0 ; i < sNewParts.Length ; i++) {
										sReturn += stringLibrary.node_color_incorrect_uncomment + commentOpenSymbol + sNewParts[i] + commentCloseSymbol + stringLib.CLOSE_COLOR_TAG;
										sReturn += "\n";
									}
								}
								else {
									sReturn = stringLibrary.node_color_incorrect_uncomment + commentOpenSymbol + thisNode.InnerText + commentCloseSymbol + stringLib.CLOSE_COLOR_TAG;
								}
                                break;
                            default:
                                break;
                        }
                        break;
                    case "robobug":
                        sReturn = thisNode.InnerText + stringLibrary.node_color_comment + commentOpenSymbol + commentCloseSymbol + stringLib.CLOSE_COLOR_TAG;
                        break;
                    default:
                        break;
                }
                if (newline) sReturn += "\n";
                break;
            }
            default:
				string thislanguage;
				try {
					thislanguage = codenode.Attributes[stringLib.XML_ATTRIBUTE_LANGUAGE].Value;
				}
				catch {
					thislanguage = "python";
				}
                sReturn = ColorizeText(thisNode.InnerText, thislanguage, true);
                break;
        }
        return sReturn;
    }

	//.................................>8.......................................
	//************************************************************************//
	// Method: public string OuterToInnerXml(string outerXml);
	// Description: Convert an outerXml string to innerXml
	//************************************************************************//
	public string OuterToInnerXml(string outerXml, string language) {
		string sReturn 		= "";
		string sHead		= "";
		string sTail		= "";
		string sPretagText	= "";
		XmlDocument doc = new XmlDocument();
		Regex rgxTail = new Regex("(<.*>)(.*)");
		Match m = rgxTail.Match(outerXml);
		sHead = m.Groups[1].Value;
		sTail = m.Groups[2].Value;
		sPretagText = outerXml.Substring(0, outerXml.Length - sHead.Length - sTail.Length);

		// Not a tag, just colorize it
		if (sHead == "" && sTail == "") {
			sReturn = ColorizeText(outerXml, language, false);
		}
		// Code node
		else if (sHead.IndexOf("<" + stringLib.NODE_NAME_CODE + " ") == -1 && sHead.IndexOf("</" + stringLib.NODE_NAME_CODE + ">") == -1) {
			doc.LoadXml("<" + stringLib.NODE_NAME_CODE + ">" + sHead + "</" + stringLib.NODE_NAME_CODE + ">");
			XmlNode node = doc.FirstChild;
			foreach(XmlNode codenode in node.ChildNodes) {
				sReturn += (codenode.Name == stringLib.NODE_NAME_NEWLINE) ? sReturn += "" : NodeToColorString(codenode, codenode, false);
			}
			if (sPretagText.Length > 0) {
				sReturn = ColorizeText(sPretagText, language, false) + sReturn;
			}
			if (sHead.IndexOf("<" + stringLib.NODE_NAME_NEWLINE + " />") == -1 && sHead.IndexOf("<" + stringLib.NODE_NAME_NEWLINE + "/>") == -1 && sTail != "") {
				sReturn += ColorizeText(sTail, language, false);
			}
		}
		else if (sHead.IndexOf("</" + stringLib.NODE_NAME_CODE + ">") != -1) {
			sReturn = "";
		}
		else {
			sReturn += ColorizeText(sTail, language, false);
		}
		// Remove &gt; and &lt;
		sReturn = sReturn.Replace("&gt;", ">").Replace("&lt;", "<");
		return sReturn;
	}

	//.................................>8.......................................
	//************************************************************************//
	// Method: public void DrawInnerXmlLinesToScreen();
	// Description: Updates the code the player sees on the screen. Also adds the line
	// numbers to the code
	//************************************************************************//
	public void DrawInnerXmlLinesToScreen(bool bRedrawLineNumbers = true) {
		string drawCode = "";
		for (int x = 1 ; x < innerXmlLines.GetLength(0); x++) {
			if (bRedrawLineNumbers) {
				string lineNum = ColorTaskLine(innerXmlLines[x-1], x-1);
				lineNum += (x).ToString();
				lineNum += (lineNum.IndexOf("color") != -1) ? stringLib.CLOSE_COLOR_TAG : "";
				lineNumbers[x-1] = lineNum;
			}
			drawCode += lineNumbers[x-1] + "\t" + innerXmlLines[x-1];
			drawCode += "\n";
		}
		leveltext.GetComponent<TextMesh>().text = drawCode;
	}

	//.................................>8.......................................
	//************************************************************************//
	// Method: 	public string ColorTaskLine(string sLine)
	// Description: returns the colorized number for the line. This function
	// selects the color to use based on a task hierarchy.
	//************************************************************************//
    public string ColorTaskLine(string sLine, int nLine)
    {
        for (int toolCheck = 0; toolCheck < stateLib.NUMBER_OF_TOOLS; toolCheck++) {
            if (taskOnLines[nLine,toolCheck] != 1) {
				continue;
			}
            switch(toolCheck) {
                case stateLib.TOOL_CATCHER_OR_ACTIVATOR:
                    break;

                case stateLib.TOOL_PRINTER_OR_QUESTION:
					if (sLine.IndexOf(stringLibrary.node_color_print) != -1) return stringLibrary.node_color_print;
                    else if (sLine.IndexOf(stringLibrary.node_color_question) != -1) return stringLibrary.node_color_question;
                    break;

                case stateLib.TOOL_WARPER_OR_RENAMER:
                    if (sLine.IndexOf(stringLibrary.node_color_warp) != -1) return stringLibrary.node_color_warp;
                    else if (sLine.IndexOf(stringLibrary.node_color_rename) != -1) return stringLibrary.node_color_rename;
                    break;

                case stateLib.TOOL_COMMENTER:
                    if (sLine.IndexOf(stringLibrary.node_color_correct_comment) == -1 && sLine != "") return stringLibrary.node_color_correct_comment;
                    else if (sLine.IndexOf(stringLibrary.node_color_incorrect_comment) == -1 && sLine != "") return stringLibrary.node_color_incorrect_comment;
                    else if (sLine.IndexOf(stringLibrary.node_color_comment) == -1 && sLine != "") return stringLibrary.node_color_comment;
                    break;

                case stateLib.TOOL_CONTROL_FLOW:
                    if (sLine.IndexOf(stringLibrary.node_color_uncomment) != -1) return stringLibrary.node_color_uncomment;
                    else if (sLine.IndexOf(stringLibrary.node_color_incorrect_uncomment) != -1) return stringLibrary.node_color_incorrect_uncomment;
                    break;
                case stateLib.TOOL_HELPER:
                    break;
                default:
                    break;
            }
        }
        return "";
    }

	//.................................>8.......................................
	//************************************************************************//
	// Method: private void PlaceObjects(XmlNode levelnode)
	// Description: Read through levelnode XML and create the interactable game objects.
	//************************************************************************//
	private void PlaceObjects(XmlNode levelnode) {

		foreach (XmlNode codenode in levelnode.ChildNodes) {
			XmlNamespaceManager nsmgr = new XmlNamespaceManager(new NameTable());
			XmlParserContext context = new XmlParserContext(null, nsmgr, null, XmlSpace.None);
			XmlValidatingReader xmlReader = new XmlValidatingReader(codenode.InnerXml, XmlNodeType.Element, context);
			IXmlLineInfo lineInfo = (IXmlLineInfo)xmlReader;
			// Parse the XML code tags
			if (codenode.Name == stringLib.NODE_NAME_CODE) {
				// Parse XML --[
				do {
					CreateLevelObject(codenode, xmlReader, lineInfo);
				}
				while(xmlReader.Read());
				// ]-- End of XML Parsing

				// These are counters to update the blocktext of each object
				int numberOfroboBUGcomments = 0;
				int numberOfrobotONcorrectComments = 0;
				int numberOfrobotONincorrectComments = 0;
				int numberOfrobotONcorrectUncomments = 0;
				int numberOfrobotONincorrectUncomments = 0;

				// Cycle through all comment objects and assign their blocktext values here
				for (int i = 0; i < codenode.ChildNodes.Count; i++) {
					if (codenode.ChildNodes[i].Name == stringLib.NODE_NAME_COMMENT) {
						//////////////////////
                        List<GameObject> allComments = new List<GameObject>();
                        allComments.AddRange(robotONcorrectComments);
                        allComments.AddRange(robotONincorrectComments);
                        allComments.AddRange(robotONcorrectUncomments);
                        allComments.AddRange(robotONincorrectUncomments);
                        foreach(GameObject comment in allComments) {
                            comment.transform.position = new Vector3(-7, initialLineY -(comment.GetComponent<comment>().index + 0.9f *(comment.GetComponent<comment>().size - 1)) * linespacing, 0f);
                        }
						foreach(GameObject question in robotONquestions) {
							question.transform.position = new Vector3(-7, initialLineY -question.GetComponent<question>().index * linespacing, 1);
						}
						foreach(GameObject rename in robotONrenamers) {
							rename.transform.position = new Vector3(-7, initialLineY - rename.GetComponent<rename>().index * linespacing, 1);
						}
						/////////////////
                        GameObject thisObject;

                        switch(codenode.ChildNodes[i].Attributes[stringLib.XML_ATTRIBUTE_TYPE].Value) {
                            case "robobug":
                                // RoboBUG comment
    							thisObject = roboBUGcomments[numberOfroboBUGcomments];
    							thisObject.GetComponent<comment>().size = thisObject.GetComponent<comment>().blocktext.Split('\n').Length;
    							// Colorize all multi-comment line numbers green
    							for (int j = 1 ; j < thisObject.GetComponent<comment>().size ; j++) {
    								taskOnLines[thisObject.GetComponent<comment>().index + j, stateLib.TOOL_COMMENTER]++;
    							}
    							// Resize the hitbox for this comment to cover all lines (if multi-line comment)
    							thisObject.transform.position = new Vector3(-7, initialLineY - (thisObject.GetComponent<comment>().index + 0.93f *(thisObject.GetComponent<comment>().size - 1)) * linespacing, 0f);
    							thisObject.transform.localScale = new Vector3(thisObject.transform.localScale.x, textscale * (thisObject.GetComponent<comment>().size - 1), thisObject.transform.localScale.z);
    							numberOfroboBUGcomments++;
                                break;
                            case "description":
                                if (codenode.ChildNodes[i].Attributes[stringLib.XML_ATTRIBUTE_CORRECT].Value == "true") {
                                    // Correct Comment
                                    thisObject = robotONcorrectComments[numberOfrobotONcorrectComments];
                                    thisObject.GetComponent<comment>().blocktext = codenode.ChildNodes[i].InnerText.Trim();
                                    thisObject.GetComponent<comment>().size = thisObject.GetComponent<comment>().blocktext.Split('\n').Length;
                                    // Colorize all multi-comment line numbers green
                                    for (int j = 1 ; j < thisObject.GetComponent<comment>().size ; j++) {
                                        taskOnLines[thisObject.GetComponent<comment>().index + j, stateLib.TOOL_COMMENTER]++;
                                    }
                                    // Resize the hitbox for this comment to cover all lines (if multi-line comment)
                                    thisObject.transform.position = new Vector3(-7, initialLineY -(thisObject.GetComponent<comment>().index + 0.93f *(thisObject.GetComponent<comment>().size - 1)) * linespacing, 0f);

									float yPos = (textscale * (thisObject.GetComponent<comment>().size - 1) > 0) ? textscale * (thisObject.GetComponent<comment>().size - 1) : 1.0f;
                                    thisObject.transform.localScale = new Vector3(thisObject.transform.localScale.x, yPos, thisObject.transform.localScale.z);
                                    numberOfrobotONcorrectComments++;
                                }
                                else if (codenode.ChildNodes[i].Attributes[stringLib.XML_ATTRIBUTE_CORRECT].Value == "false") {
                                    // Incorrect comment
                                    thisObject = robotONincorrectComments[numberOfrobotONincorrectComments];
                                    thisObject.GetComponent<comment>().blocktext = codenode.ChildNodes[i].InnerText.Trim();
                                    thisObject.GetComponent<comment>().size = thisObject.GetComponent<comment>().blocktext.Split('\n').Length;
                                    // Colorize all multi-comment line numbers green
                                    for (int j = 1 ; j < thisObject.GetComponent<comment>().size ; j++) {
                                        taskOnLines[thisObject.GetComponent<comment>().index + j, stateLib.TOOL_COMMENTER]++;
                                    }
                                    // Resize the hitbox for this comment to cover all lines (if multi-line comment)
									float yPos = (textscale * (thisObject.GetComponent<comment>().size - 1) > 0) ? textscale * (thisObject.GetComponent<comment>().size - 1) : 1.0f;
                                    thisObject.transform.position = new Vector3(-7, initialLineY -(thisObject.GetComponent<comment>().index + 0.93f *(thisObject.GetComponent<comment>().size - 1)) * linespacing, 0f);
                                    thisObject.transform.localScale = new Vector3(thisObject.transform.localScale.x, yPos, thisObject.transform.localScale.z);
                                    numberOfrobotONincorrectComments++;
                                }
                                break;
                            case "code":
                                if (codenode.ChildNodes[i].Attributes[stringLib.XML_ATTRIBUTE_CORRECT].Value == "true") {
                                    // Correct Uncomment
                                    thisObject = robotONcorrectUncomments[numberOfrobotONcorrectUncomments];
                                    thisObject.GetComponent<comment>().blocktext = codenode.ChildNodes[i].InnerText.Trim();
                                    thisObject.GetComponent<comment>().size = thisObject.GetComponent<comment>().blocktext.Split('\n').Length;
                                    // Colorize all multi-comment line numbers red
                                    for (int j = 1 ; j < thisObject.GetComponent<comment>().size ; j++) {
                                        taskOnLines[thisObject.GetComponent<comment>().index + j, stateLib.TOOL_CONTROL_FLOW]++;
                                    }
                                    // Resize the hitbox for this comment to cover all lines (if multi-line comment)
									float yPos = (textscale * (thisObject.GetComponent<comment>().size - 1) > 0) ? textscale * (thisObject.GetComponent<comment>().size - 1) : 1.0f;
                                    thisObject.transform.position = new Vector3(-7, initialLineY -(thisObject.GetComponent<comment>().index + 0.93f *(thisObject.GetComponent<comment>().size - 1)) * linespacing, 0f);
                                    thisObject.transform.localScale = new Vector3(thisObject.transform.localScale.x, yPos, thisObject.transform.localScale.z);
                                    numberOfrobotONcorrectUncomments++;
                                }
                                else if (codenode.ChildNodes[i].Attributes[stringLib.XML_ATTRIBUTE_CORRECT].Value == "false") {
                                    // Incorrect Uncomment
                                    thisObject = robotONincorrectUncomments[numberOfrobotONincorrectUncomments];
                                    thisObject.GetComponent<comment>().blocktext = codenode.ChildNodes[i].InnerText.Trim();
                                    thisObject.GetComponent<comment>().size = thisObject.GetComponent<comment>().blocktext.Split('\n').Length;
                                    // Colorize all multi-comment line numbers red
                                    for (int j = 1 ; j < thisObject.GetComponent<comment>().size ; j++) {
                                        taskOnLines[thisObject.GetComponent<comment>().index + j, stateLib.TOOL_CONTROL_FLOW]++;
                                    }
                                    // Resize the hitbox for this comment to cover all lines (if multi-line comment)
									float yPos = (textscale * (thisObject.GetComponent<comment>().size - 1) > 0) ? textscale * (thisObject.GetComponent<comment>().size - 1) : 1.0f;
                                    thisObject.transform.position = new Vector3(-7, initialLineY -(thisObject.GetComponent<comment>().index + 0.93f *(thisObject.GetComponent<comment>().size - 1)) * linespacing, 0f);
                                    thisObject.transform.localScale = new Vector3(thisObject.transform.localScale.x, yPos, thisObject.transform.localScale.z);
                                    numberOfrobotONincorrectUncomments++;
                                }
                                break;
                            default:
                                break;
                        }
					}
				}
				// ]-- End of blocktext setting
                // Pair Incorrect Comments to their corresponding correct comments --[
				foreach (GameObject incorrectComment in robotONincorrectComments) {
					foreach (GameObject correctComment in robotONcorrectComments) {
						if (incorrectComment.GetComponent<comment>().groupid == correctComment.GetComponent<comment>().groupid) {
							incorrectComment.GetComponent<comment>().CorrectCommentObject = correctComment;
							break;
						}
					}
				}


				foreach (GameObject incorrectUncomment in robotONincorrectUncomments) {
					foreach (GameObject correctUncomment in robotONcorrectUncomments) {
						if (incorrectUncomment.GetComponent<comment>().groupid == correctUncomment.GetComponent<comment>().groupid) {
							incorrectUncomment.GetComponent<comment>().CorrectCommentObject = correctUncomment;
							break;
						}
					}
				}

				foreach (GameObject variablecolor in robotONvariablecolors) {
					foreach (GameObject rename in robotONrenamers) {
						if (variablecolor.GetComponent<VariableColor>().groupid == rename.GetComponent<rename>().groupid) {
							variablecolor.GetComponent<VariableColor>().CorrectRenameObject = rename;
							variablecolor.GetComponent<VariableColor>().correct = rename.GetComponent<rename>().correct;
							break;
						}
					}
				}
                // ]--

			}
		}
	}

	//.................................>8.......................................
	//************************************************************************//
	// Method: int void CreateLevelObjects(XmlNode parentNode)
	// Description: Handle XML parsing. Right now it just returns 1 if a tag was found.
	//              Returns 0 if an XML tag was not found.
	//************************************************************************//
	GameObject CreateLevelObject(XmlNode codenode, XmlValidatingReader xmlReader, IXmlLineInfo lineInfo) {

        if (xmlReader.NodeType != XmlNodeType.Element) {
            return null;
        }
        switch(xmlReader.Name) {
            case stringLib.NODE_NAME_PRINT: {
                GameObject newoutput = (GameObject)Instantiate(printobject, new Vector3(-7, initialLineY -(lineInfo.LineNumber - 1) * linespacing, 1), transform.rotation);
                taskOnLines[lineInfo.LineNumber - 1, stateLib.TOOL_PRINTER_OR_QUESTION]++;
                prints.Add(newoutput);
                printer propertyHandler = newoutput.GetComponent<printer>();
                propertyHandler.CodescreenObject = this.gameObject;
                propertyHandler.displaytext = xmlReader.GetAttribute(stringLib.XML_ATTRIBUTE_TEXT);
                propertyHandler.SidebarObject = sidebaroutput;
                propertyHandler.ToolSelectorObject = selectedtool;
                propertyHandler.index = lineInfo.LineNumber-1;
				propertyHandler.language = codenode.Attributes[stringLib.XML_ATTRIBUTE_LANGUAGE].Value;
                if (xmlReader.GetAttribute(stringLib.XML_ATTRIBUTE_TOOL) != null) {
                    string toolatt = xmlReader.GetAttribute(stringLib.XML_ATTRIBUTE_TOOL);
                    string[] toolcounts = toolatt.Split(',');
                    for (int i = 0; i < stateLib.NUMBER_OF_TOOLS; i++) {
                        propertyHandler.tools[i] = int.Parse(toolcounts[i]);
                    }
                }
                return newoutput;
            }
            case stringLib.NODE_NAME_WARP: {
                GameObject newwarp = (GameObject)Instantiate(warpobject, new Vector3(-7, initialLineY - (lineInfo.LineNumber - 1) * linespacing, 1), transform.rotation);
                taskOnLines[lineInfo.LineNumber - 1, stateLib.TOOL_WARPER_OR_RENAMER]++;
                roboBUGwarps.Add(newwarp);
                warper propertyHandler = newwarp.GetComponent<warper>();
                propertyHandler.CodescreenObject = this.gameObject;
                propertyHandler.filename = xmlReader.GetAttribute(stringLib.XML_ATTRIBUTE_FILE);
                propertyHandler.ToolSelectorObject = selectedtool;
                propertyHandler.Menu = menu;
                propertyHandler.index = lineInfo.LineNumber-1;
				propertyHandler.language = codenode.Attributes[stringLib.XML_ATTRIBUTE_LANGUAGE].Value;
                if (xmlReader.GetAttribute(stringLib.XML_ATTRIBUTE_TOOL) != null) {
                    string toolatt = xmlReader.GetAttribute(stringLib.XML_ATTRIBUTE_TOOL);
                    string[] toolcounts = toolatt.Split(',');
                    for (int i = 0; i < stateLib.NUMBER_OF_TOOLS; i++) {
                        propertyHandler.tools[i] = int.Parse(toolcounts[i]);
                    }
                }
                if (xmlReader.GetAttribute(stringLib.XML_ATTRIBUTE_LINE) != null) {
                    propertyHandler.linenum = xmlReader.GetAttribute(stringLib.XML_ATTRIBUTE_LINE);
                }
                return newwarp;
            }
            case stringLib.NODE_NAME_BUG: {
                bugsize = int.Parse(xmlReader.GetAttribute(stringLib.XML_ATTRIBUTE_SIZE));
                int row = 0;
                if (xmlReader.GetAttribute(stringLib.XML_ATTRIBUTE_ROW) != null) {
                    row = int.Parse(xmlReader.GetAttribute(stringLib.XML_ATTRIBUTE_ROW));
                }
                int col = 0;
                if (xmlReader.GetAttribute(stringLib.XML_ATTRIBUTE_COL) != null) {
                    col = int.Parse(xmlReader.GetAttribute(stringLib.XML_ATTRIBUTE_COL));
                }
                levelbug =(GameObject)Instantiate(bugobject, new Vector3(bugXshift + col * fontwidth +(bugsize - 1) * levelLineRatio, initialLineY -(lineInfo.LineNumber + row - 1 + 0.5f *(bugsize - 1)) * linespacing + 0.4f, 0f), transform.rotation);
                levelbug.transform.localScale += new Vector3(bugscale *(bugsize - 1), bugscale *(bugsize - 1), 0);
                GenericBug propertyHandler = levelbug.GetComponent<GenericBug>();
                propertyHandler.CodescreenObject = this.gameObject;
                propertyHandler.CodescreenObject = selectedtool;
				propertyHandler.language = codenode.Attributes[stringLib.XML_ATTRIBUTE_LANGUAGE].Value;
                taskOnLines[lineInfo.LineNumber - 1, stateLib.TOOL_CATCHER_OR_ACTIVATOR]++;
                bugs.Add(levelbug);
                numberOfBugsRemaining++;
                return levelbug;
            }
            case stringLib.NODE_NAME_COMMENT: {
                GameObject newcomment = (GameObject)Instantiate(commentobject, new Vector3(-7, initialLineY -(lineInfo.LineNumber - 1) * linespacing, 0f), transform.rotation);
                comment propertyHandler = newcomment.GetComponent<comment>();
                propertyHandler.groupid = int.Parse(xmlReader.GetAttribute(stringLib.XML_ATTRIBUTE_GROUPID));
                propertyHandler.index = lineInfo.LineNumber - 1;
                propertyHandler.CodescreenObject = this.gameObject;
                propertyHandler.ToolSelectorObject = selectedtool;
				propertyHandler.language = codenode.Attributes[stringLib.XML_ATTRIBUTE_LANGUAGE].Value;
				try {
					propertyHandler.commentStyle = xmlReader.GetAttribute(stringLib.XML_ATTRIBUTE_COMMENT_STYLE);
				}
				catch {
					propertyHandler.commentStyle = "single";
					print("no commentStyle found");
				}
                switch(xmlReader.GetAttribute(stringLib.XML_ATTRIBUTE_TYPE)) {
                    case "robobug":
                        roboBUGcomments.Add(newcomment);
                        propertyHandler.entityType = stateLib.ENTITY_TYPE_ROBOBUG_COMMENT;
                        propertyHandler.errmsg = xmlReader.GetAttribute(stringLib.XML_ATTRIBUTE_TEXT);
                        propertyHandler.SidebarObject = sidebaroutput;
                        if (xmlReader.GetAttribute(stringLib.XML_ATTRIBUTE_TOOL) != null) {
                            string toolatt = xmlReader.GetAttribute(stringLib.XML_ATTRIBUTE_TOOL);
                            string[] toolcounts = toolatt.Split(',');
                            for (int i = 0; i < stateLib.NUMBER_OF_TOOLS; i++) {
                                propertyHandler.tools[i] = int.Parse(toolcounts[i]);
                            }
                        }
                        break;
                    case "description":
                        taskOnLines[lineInfo.LineNumber - 1, stateLib.TOOL_COMMENTER]++;
                        if (xmlReader.GetAttribute(stringLib.XML_ATTRIBUTE_CORRECT) == "true") {
                            robotONcorrectComments.Add(newcomment);
                            propertyHandler.entityType = stateLib.ENTITY_TYPE_CORRECT_COMMENT;
                            tasklist[3]++;
                        }
                        else if(xmlReader.GetAttribute(stringLib.XML_ATTRIBUTE_CORRECT) == "false") {
                            robotONincorrectComments.Add(newcomment);
                            propertyHandler.entityType = stateLib.ENTITY_TYPE_INCORRECT_COMMENT;
                        }
                        else {
                            print("Error: Description comment is neither true or false");
                        }
                        break;
                    case "code":
                        taskOnLines[lineInfo.LineNumber - 1, stateLib.TOOL_CONTROL_FLOW]++;
                        if (xmlReader.GetAttribute(stringLib.XML_ATTRIBUTE_CORRECT) == "true") {
                            robotONcorrectUncomments.Add(newcomment);
                            propertyHandler.entityType = stateLib.ENTITY_TYPE_CORRECT_UNCOMMENT;
                            tasklist[4]++;
                        }
                        else if (xmlReader.GetAttribute(stringLib.XML_ATTRIBUTE_CORRECT) == "false") {
                            robotONincorrectUncomments.Add(newcomment);
                            propertyHandler.entityType = stateLib.ENTITY_TYPE_INCORRECT_UNCOMMENT;
                        }
                        else {
                            print("Error: Code comment is neither true or false");
                        }
                        break;
                    default: break;
                }
                return newcomment;
            }
            case stringLib.NODE_NAME_QUESTION: {
                GameObject newquestion = (GameObject)Instantiate(questionobject, new Vector3(-7, initialLineY -(lineInfo.LineNumber - 1) * linespacing, 1), transform.rotation);
                taskOnLines[lineInfo.LineNumber - 1, stateLib.TOOL_PRINTER_OR_QUESTION]++;
                robotONquestions.Add(newquestion);
                question propertyHandler = newquestion.GetComponent<question>();
                propertyHandler.displaytext = xmlReader.GetAttribute(stringLib.XML_ATTRIBUTE_TEXT) + "\n";
                propertyHandler.expected = xmlReader.GetAttribute(stringLib.XML_ATTRIBUTE_ANSWER);
                propertyHandler.CodescreenObject = this.gameObject;
                propertyHandler.SidebarObject = sidebaroutput;
                propertyHandler.ToolSelectorObject = selectedtool;
                propertyHandler.index = lineInfo.LineNumber-1;
				propertyHandler.language = codenode.Attributes[stringLib.XML_ATTRIBUTE_LANGUAGE].Value;
                tasklist[1]++;
                // propertyHandler.innertext = xmlReader.ReadInnerXml(); //Danger will robinson
                Regex rgx = new Regex("(.*)("+stringLibrary.node_color_question+")(.*)(</color>)(.*)");
                string thisQuestionInnerText = rgx.Replace(innerXmlLines[propertyHandler.index], "$2$3$4");
                propertyHandler.innertext = thisQuestionInnerText;
                return newquestion;
            }
            case stringLib.NODE_NAME_RENAME: {
                GameObject newrename = (GameObject)Instantiate(renameobject, new Vector3(-7, initialLineY -(lineInfo.LineNumber - 1) * linespacing, 1), transform.rotation);
                taskOnLines[lineInfo.LineNumber - 1, stateLib.TOOL_WARPER_OR_RENAMER]++;
                robotONrenamers.Add(newrename);
                rename propertyHandler = newrename.GetComponent<rename>();
                propertyHandler.displaytext = xmlReader.GetAttribute(stringLib.XML_ATTRIBUTE_TEXT) + "\n";
                propertyHandler.correct = xmlReader.GetAttribute(stringLib.XML_ATTRIBUTE_CORRECT);
                propertyHandler.groupid = int.Parse(xmlReader.GetAttribute(stringLib.XML_ATTRIBUTE_GROUPID));
                propertyHandler.CodescreenObject = this.gameObject;
                propertyHandler.SidebarObject = sidebaroutput;
                propertyHandler.ToolSelectorObject = selectedtool;
                propertyHandler.index = lineInfo.LineNumber-1;
				propertyHandler.language = codenode.Attributes[stringLib.XML_ATTRIBUTE_LANGUAGE].Value;
                string options = xmlReader.GetAttribute(stringLib.XML_ATTRIBUTE_OPTIONS);
                string[] optionsArray = options.Split(',');
                for (int i = 0; i < optionsArray.Length; i++) {
                    propertyHandler.options.Add(optionsArray[i]);
                }
                tasklist[2]++;
                Regex rgx = new Regex("(.*)("+stringLibrary.node_color_rename+")(.*)(</color>)(.*)");
                string thisRenameInnerText = rgx.Replace(innerXmlLines[propertyHandler.index], "$2$3$4");
                propertyHandler.innertext = thisRenameInnerText;
                return newrename;
            }
            case stringLib.NODE_NAME_BREAKPOINT: {
                GameObject newbreakpoint =(GameObject)Instantiate(breakpointobject, new Vector3(-10, initialLineY -(lineInfo.LineNumber - 1) * linespacing + 0.4f, 1), transform.rotation);
                taskOnLines[lineInfo.LineNumber - 1, stateLib.TOOL_CONTROL_FLOW]++;
                roboBUGbreakpoints.Add(newbreakpoint);
                Breakpoint propertyHandler = newbreakpoint.GetComponent<Breakpoint>();
                propertyHandler.SidebarObject = sidebaroutput;
                propertyHandler.values = xmlReader.GetAttribute(stringLib.XML_ATTRIBUTE_TEXT);
                propertyHandler.ToolSelectorObject = selectedtool;
                propertyHandler.index = lineInfo.LineNumber-1;
				propertyHandler.language = codenode.Attributes[stringLib.XML_ATTRIBUTE_LANGUAGE].Value;
                if (xmlReader.GetAttribute(stringLib.XML_ATTRIBUTE_TOOL) != null) {
                    string toolatt = xmlReader.GetAttribute(stringLib.XML_ATTRIBUTE_TOOL);
                    string[] toolcounts = toolatt.Split(',');
                    for (int i = 0; i < stateLib.NUMBER_OF_TOOLS; i++) {
                        propertyHandler.tools[i] = int.Parse(toolcounts[i]);
                    }
                }
                return newbreakpoint;
            }
            case stringLib.NODE_NAME_PRIZE: {
                bugsize = int.Parse(xmlReader.GetAttribute(stringLib.XML_ATTRIBUTE_SIZE));
                GameObject prizebug =(GameObject)Instantiate(prizeobject, new Vector3(-9f +(bugsize - 1) * levelLineRatio, initialLineY -(lineInfo.LineNumber - 1 + 0.5f *(bugsize - 1)) * linespacing + 0.4f, 0f), transform.rotation);
                prizebug.transform.localScale += new Vector3(bugscale *(bugsize - 1), bugscale *(bugsize - 1), 0);
                PrizeBug propertyHandler = prizebug.GetComponent<PrizeBug>();
                propertyHandler.CodescreenObject = this.gameObject;
                propertyHandler.ToolSelectorObject = selectedtool;
				propertyHandler.language = codenode.Attributes[stringLib.XML_ATTRIBUTE_LANGUAGE].Value;
                string[] bonuses = xmlReader.GetAttribute(stringLib.XML_ATTRIBUTE_BONUSES).Split(',');
                for (int i = 0; i < stateLib.NUMBER_OF_TOOLS; i++) {
                    propertyHandler.bonus[i] += int.Parse(bonuses[i]);
                }
                roboBUGprizes.Add(prizebug);
                return prizebug;
            }
            case stringLib.NODE_NAME_BEACON: {
                GameObject newbeacon = (GameObject)Instantiate(beaconobject, new Vector3(-9.95f, initialLineY -(lineInfo.LineNumber - 1) * linespacing + lineOffset + 0.4f, 1), transform.rotation);
                taskOnLines[lineInfo.LineNumber - 1, stateLib.TOOL_CATCHER_OR_ACTIVATOR]++;
                beacon propertyHandler = newbeacon.GetComponent<beacon>();
                propertyHandler.CodescreenObject = this.gameObject;
                propertyHandler.index = lineInfo.LineNumber - 1;
                propertyHandler.ToolSelectorObject = selectedtool;
				propertyHandler.language = codenode.Attributes[stringLib.XML_ATTRIBUTE_LANGUAGE].Value;
                if (xmlReader.GetAttribute(stringLib.XML_ATTRIBUTE_FLOWORDER) != "") {
                    string[] flowOrder = xmlReader.GetAttribute(stringLib.XML_ATTRIBUTE_FLOWORDER).Split(',');
                    for (int i = 0; i < flowOrder.Length; i++) {
                        propertyHandler.flowOrder.Add(int.Parse(flowOrder[i]));
                        tasklist[0]++;
                    }
                }
                robotONbeacons.Add(newbeacon);
                return newbeacon;
            }
            case stringLib.NODE_NAME_VARIABLE_COLOR: {
                GameObject newvariablecolor =(GameObject)Instantiate(variablecolorobject, new Vector3(-7, initialLineY -(lineInfo.LineNumber - 1) * linespacing, 1), transform.rotation);
                newvariablecolor.GetComponent<VariableColor>().CodescreenObject = this.gameObject;
                robotONvariablecolors.Add(newvariablecolor);
                VariableColor propertyHandler = newvariablecolor.GetComponent<VariableColor>();
                propertyHandler.groupid = int.Parse(xmlReader.GetAttribute(stringLib.XML_ATTRIBUTE_GROUPID));
                propertyHandler.index = lineInfo.LineNumber-1;
				propertyHandler.language = codenode.Attributes[stringLib.XML_ATTRIBUTE_LANGUAGE].Value;
                Regex rgx = new Regex("(.*)("+stringLibrary.node_color_rename+")(\\w)(</color>)(.*)");
                string thisVarnamenInnerText = rgx.Replace(innerXmlLines[propertyHandler.index], "$2$3$4");
                propertyHandler.innertext = thisVarnamenInnerText;
                return newvariablecolor;
            }
        }
		return null;
	}

	//.................................>8.......................................
	//************************************************************************//
	// Method: public void ProvisionToolsFromXml(XmlNode levelnode)
	// Description: Read through levelnode XML and provision the tools for this level
	// levelnode is typically the parent XML node in the XML document.
	//************************************************************************//
	public void ProvisionToolsFromXml(XmlNode levelnode) {
		// Grey out all tools
		for (int i = 0; i < totalNumberOfTools; i++) {
			toolIcons[i].GetComponent<GUITexture>().enabled = false;
		}
		// For each XML node in levelnode's children
		foreach (XmlNode codenode in levelnode.ChildNodes) {
			// For each child node whose XML node name is "tools". eg <tools>
			if (codenode.Name == stringLib.NODE_NAME_TOOLS) {
				// For each XML node in <tools> nodes (really there should only be 1 <tools> tag), eg <tool attr="val">
				foreach (XmlNode toolnode in codenode.ChildNodes) {
					// Set the tool count for each tool node --[
					int toolnum = 0;
					switch(toolnode.Attributes[stringLib.XML_ATTRIBUTE_NAME].Value) {
						case "catcher":
						case "activator":
							toolnum = stateLib.TOOL_CATCHER_OR_ACTIVATOR;
							break;
						case "printer":
						case "checker":
						case "answer":
							toolnum = stateLib.TOOL_PRINTER_OR_QUESTION;
							break;
						case "warper":
						case "namer":
							toolnum = stateLib.TOOL_WARPER_OR_RENAMER;
							break;
						case "commenter": toolnum = stateLib.TOOL_COMMENTER;
							break;
						case "controlflow": toolnum = stateLib.TOOL_CONTROL_FLOW;
							break;
						default:
							break;
					}
					toolIcons[toolnum].GetComponent<GUITexture>().enabled = bool.Parse(toolnode.Attributes[stringLib.XML_ATTRIBUTE_ENABLED].Value);
					selectedtool.GetComponent<SelectedTool>().toolCounts[toolnum] = (toolnode.Attributes[stringLib.XML_ATTRIBUTE_COUNT].Value == "unlimited") ? 999 : int.Parse(toolnode.Attributes[stringLib.XML_ATTRIBUTE_COUNT].Value);
					// ]-- End of tool count for each tool node
				}
			}
		}
	}

	//.................................>8.......................................
	//************************************************************************//
	// Method: public void ResetLevel(bool warp)
	// Description: Completely resets the level. Will destroy all game objects,
	// 				and reset task list and bug values to their default state.
	//				If isWarping is TRUE, then retain current tools. If FALSE, reset
	//				the tool count.
	//************************************************************************//
	public void ResetLevel(bool warping) {
		// Destroy objects
		foreach (GameObject line in lines) {
			Destroy(line);
		}
		foreach (GameObject robotONbeacon in robotONbeacons) {
			Destroy(robotONbeacon);
		}
		foreach (GameObject correctComment in robotONcorrectComments) {
			Destroy(correctComment);
		}
		foreach (GameObject question in robotONquestions) {
			Destroy(question);
		}
		foreach (GameObject variablerenames in robotONrenamers) {
			Destroy(variablerenames);
		}
		foreach (GameObject variablecolor in robotONvariablecolors) {
			Destroy(variablecolor);
		}
		foreach (GameObject print in prints) {
			Destroy(print);
		}
		foreach (GameObject incorrectComment in robotONincorrectComments) {
			Destroy(incorrectComment);
		}
		foreach (GameObject warp in roboBUGwarps) {
			Destroy(warp);
		}
		foreach (GameObject roboBUGComment in roboBUGcomments) {
			Destroy(roboBUGComment);
		}
		foreach (GameObject correctUncomment in robotONcorrectUncomments) {
			Destroy(correctUncomment);
		}
		foreach (GameObject incorrectUncomment in robotONincorrectUncomments) {
			Destroy(incorrectUncomment);
		}
		foreach (GameObject breakpoint in roboBUGbreakpoints) {
			Destroy(breakpoint);
		}
		foreach (GameObject roboBUGprize in roboBUGprizes) {
			Destroy(roboBUGprize);
		}
		if (levelbug) {
			Destroy(levelbug);
		}

		// Reset task list
		for (int i = 0; i < 5; i++) {
			taskscompleted[i] = 0;
			tasklist[i] = 0;
		}

		// Reset local variables
		sidebaroutput.GetComponent<GUIText>().text = "";
		lines 									  	= new List<GameObject>();
		prints 								  	  	= new List<GameObject>();
		roboBUGwarps 							  	= new List<GameObject>();
		robotONcorrectComments 					  	= new List<GameObject>();
		robotONrenamers 						  	= new List<GameObject>();
		robotONvariablecolors					  	= new List<GameObject>();
		robotONincorrectComments 				  	= new List<GameObject>();
		robotONquestions 						  	= new List<GameObject>();
		robotONcorrectUncomments 				  	= new List<GameObject>();
		robotONincorrectUncomments 				  	= new List<GameObject>();
		robotONbeacons							  	= new List<GameObject>();
		bugs 									  	= new List<GameObject>();
		roboBUGcomments 						  	= new List<GameObject>();
		roboBUGbreakpoints 						  	= new List<GameObject>();
		roboBUGprizes 							 	= new List<GameObject>();

		// Reset tool counts if not warping to this level
		if (!warping) {
			for (int i = 0; i < totalNumberOfTools; i++) {
				toolIcons[i].GetComponent<GUITexture>().enabled 		 = false;
				toolIcons[i].GetComponent<GUITexture>().color 			 = new Color(0.3f, 0.3f, 0.3f);
				selectedtool.GetComponent<SelectedTool>().toolCounts[i]  = 0;
				selectedtool.GetComponent<SelectedTool>().bonusTools[i]  = 0;
				selectedtool.GetComponent<SelectedTool>().projectilecode = 0;
			}
		}

		// Reset bug count
		numberOfBugsRemaining = 0;

		// Reset counter for Renames / Variable Color
		renamegroupidCounter = 0;

		// Reset play area size
		if (!storedDefaultPlayArea) {
			storedDefaultPlayArea = true;
			defaultPosition = this.transform.position;
			defaultLocalScale = this.transform.localScale;
		}
		this.transform.position = defaultPosition;
		this.transform.localScale = defaultLocalScale;

		// Reset line count
		linecount = 0;

		// Move player to default position
		hero.transform.position = leveltext.transform.position;
	}

	//.................................>8.......................................
	//************************************************************************//
	// Method: public void GameOver()
	// Description: Switch to the LEVEL_LOSE state. When Update() is called, appropriate
	// action is taken there.
	//************************************************************************//
	public void GameOver() {
		GUISwitch(false);
		menu.GetComponent<Menu>().gameon = false;
		gamestate = stateLib.GAMESTATE_LEVEL_LOSE;
	}

	//.................................>8.......................................
	//************************************************************************//
	// Method: public void Victory()
	// Description: Switch to the GAME_END state. When Update() is called, appropriate
	// action is taken there.
	//@TODO: level5 seems to be a magic level or something to signal the end of the game?
	//************************************************************************//
	public void Victory() {
		GUISwitch(false);
		menu.GetComponent<Menu>().gameon = false;
		currentlevel = "level5";
		gamestate = stateLib.GAMESTATE_GAME_END;
	}

	//.................................>8.......................................
	//************************************************************************//
	// Method: private void OnTriggerEnter2D(Collider2D c)
	// Description: Projectile has hit the game's boundry box.
	//************************************************************************//
	private void OnTriggerEnter2D(Collider2D collidingObj)	{
		// Nothing happens when a player/projectile hits the game's boundary box/game area.
		if (collidingObj.name != "Hero") toolsAirborne--;
	}

	//.................................>8.......................................
	//************************************************************************//
	// Method: public string ColorizeText(string sBlockText, bool overrideNewline = false)
	// Description: Colorizes sBlockText. If overrideNewline is true, it won't put a newline character
	// at the end of sBlockText. Right now, it colorizes comments and keywords only.
	// The tool highlighting is handled elsewhere, in StoreInnerXml(). The color tags are saved in
	// stringLib.cs, if you want to use a different color.
	//************************************************************************//
	public string ColorizeText(string sBlockText, string language, bool newline = true) {
	// sBlockText is empty. Save us the hassle of doing the regex operations and just return.
	if (sBlockText == "") return sBlockText;

	// Turn all comments and their following text green. Remove all color tags from following text.
	Regex rgxNewlineSplit = new Regex("\n");
	Regex rgxStringLiteral = new Regex("(\"|\')(.*)(\"|\')");
	string patternCommentPython = "(//|\\s#|\n#|#)(.*)";
	string patternCommentCpp = "(//|\\*/)(.*)";
	string patternKeywordPython = "(\\W|^)(else if|class|print|not|or|and|def|include|bool|auto|double|int|struct|break|else|long|switch|case|enum|register|typedef|char|extern|return|union|continue|for|signed|void|do|if|static|while|default|goto|sizeof|volatile|const|float|short|unsigned)(\\W|$)";
	string patternKeywordCpp = "(\\W|^)(else if|class|print|not|or|and|def|include|bool|auto|double|int|struct|break|else|long|switch|case|enum|register|typedef|char|extern|return|union|continue|for|signed|void|do|if|static|while|default|goto|sizeof|volatile|const|float|short|unsigned)(\\W|$)";
	string patternIncludeGeneric = "(#include\\s)(.*)";
	string patternComment = patternCommentPython;
	string patternKeyword = patternKeywordPython;
	string patternInclude = patternIncludeGeneric;
	switch(language) {
		case "python": {
			patternComment = patternCommentPython;
			patternKeyword = patternKeywordPython;
			break;
		}
		case "c++":
		case "c":
		case "c#": {
			patternComment = patternCommentCpp;
			patternKeyword = patternKeywordCpp;
			break;
		}
		default: {
			patternComment = patternCommentPython;
			patternKeyword = patternKeywordPython;
			break;
		}
	}
	Regex rgxComment = new Regex(patternComment);
	Regex rgxKeyword = new Regex(patternKeyword);
	Regex rgxInclude = new Regex(patternInclude);
	Match m0, m1, m2, m3;
	string[] saStringBuilder = rgxNewlineSplit.Split(sBlockText);
	string sReturnString = "";
	foreach (string substring in saStringBuilder)
	{
		m0 = rgxStringLiteral.Match(substring);
		m1 = rgxComment.Match(substring);
		m2 = rgxKeyword.Match(substring);
		m3 = rgxInclude.Match(substring);
		string sPiece = substring;
		if (m3.Success) {
			sPiece = rgxInclude.Replace(sPiece, stringLibrary.syntax_color_include + "$1$2" + stringLib.CLOSE_COLOR_TAG);
		}
		else if (m0.Success && !m1.Success) {
			sPiece = rgxStringLiteral.Replace(sPiece, stringLibrary.syntax_color_string + "$1$2$3" + stringLib.CLOSE_COLOR_TAG);
			if (m2.Success) {
				// Found a keyword as well as a string, the fun begins.
				Regex rgxOutsideKeyword = new Regex(patternKeyword + "(?=(?:[^\"]|\"[^\"]*\")*$)");
				sPiece = rgxOutsideKeyword.Replace(sPiece, "$1" + stringLibrary.syntax_color_keyword + "$2" + stringLib.CLOSE_COLOR_TAG + "$3");
			}
		}
		// Comment found in this substring
		else if (m1.Success) {
			sPiece = rgxComment.Replace(sPiece, stringLibrary.syntax_color_comment + "$1$2" + stringLib.CLOSE_COLOR_TAG);
		}
		// Keyword found in this substring
		else if (m2.Success) {
			sPiece = rgxKeyword.Replace(sPiece, "$1" + stringLibrary.syntax_color_keyword + "$2" + stringLib.CLOSE_COLOR_TAG + "$3");
		}
		// Handle new lines
		if (substring != "" && substring.Trim().Length > 0 && substring.Substring(substring.Length-1, 1) != " " && newline) {
			sPiece += "\n";
		}
		sReturnString += sPiece;
	}
	return sReturnString;
}

	//.................................>8.......................................
	//************************************************************************//
	// Method: public string DecolorizeText(string sBlockText)
	// Description: Removes color tags surrounding sBlockText and returns the new substring.
	//************************************************************************//
	public string DecolorizeText(string sBlockText) {
		Regex rgx = new Regex("(?s)(.*)(<color=#.{8}>)(.*)(</color>)(.*)");
		sBlockText = rgx.Replace(sBlockText, "$1$3$5");
		return sBlockText;
	}

	//.................................>8.......................................
	//************************************************************************//
	// Method: public void TransformTextSize(int nTextSizeConst)
	// Description: Transform the play area to correspond to a new text size.
	//************************************************************************//
	public void TransformTextSize(int nTextSizeConst) {
		hero.transform.position = new Vector3(-9.5f, initialLineY, hero.transform.position.z);
		switch (nTextSizeConst) {
			case stateLib.TEXT_SIZE_SMALL:
				leveltext.GetComponent<TextMesh>().fontSize = stateLib.TEXT_SIZE_NORMAL;
				levelLineRatio = 0.55f;
				linespacing = 0.825f;
				lineOffset	= -0.3f;
                textscale = 1.75f;
				break;
			case stateLib.TEXT_SIZE_NORMAL:
				leveltext.GetComponent<TextMesh>().fontSize = stateLib.TEXT_SIZE_LARGE;
				levelLineRatio = 0.55f * (float)stateLib.TEXT_SIZE_LARGE / (float)stateLib.TEXT_SIZE_NORMAL;
				linespacing = 0.825f * (float)stateLib.TEXT_SIZE_LARGE / (float)stateLib.TEXT_SIZE_NORMAL;
				lineOffset	= -0.3f * (float)stateLib.TEXT_SIZE_LARGE / (float)stateLib.TEXT_SIZE_NORMAL;
                textscale = 1.75f * (float)stateLib.TEXT_SIZE_LARGE / (float)stateLib.TEXT_SIZE_NORMAL;
				break;
			case stateLib.TEXT_SIZE_LARGE:
				leveltext.GetComponent<TextMesh>().fontSize = stateLib.TEXT_SIZE_VERY_LARGE;
				levelLineRatio = 0.55f * (float)stateLib.TEXT_SIZE_VERY_LARGE / (float)stateLib.TEXT_SIZE_LARGE * (float)stateLib.TEXT_SIZE_LARGE / (float)stateLib.TEXT_SIZE_NORMAL;
				linespacing = 0.825f * (float)stateLib.TEXT_SIZE_VERY_LARGE / (float)stateLib.TEXT_SIZE_LARGE * (float)stateLib.TEXT_SIZE_LARGE / (float)stateLib.TEXT_SIZE_NORMAL;
				lineOffset	= -0.3f * (float)stateLib.TEXT_SIZE_VERY_LARGE / (float)stateLib.TEXT_SIZE_LARGE * (float)stateLib.TEXT_SIZE_LARGE / (float)stateLib.TEXT_SIZE_NORMAL;
                textscale = 1.75f * (float)stateLib.TEXT_SIZE_VERY_LARGE / (float)stateLib.TEXT_SIZE_LARGE * (float)stateLib.TEXT_SIZE_LARGE / (float)stateLib.TEXT_SIZE_NORMAL;
				break;
			case stateLib.TEXT_SIZE_VERY_LARGE:
				leveltext.GetComponent<TextMesh>().fontSize = stateLib.TEXT_SIZE_SMALL;
				levelLineRatio = 0.55f * (float)stateLib.TEXT_SIZE_SMALL / (float)stateLib.TEXT_SIZE_VERY_LARGE * (float)stateLib.TEXT_SIZE_VERY_LARGE / (float)stateLib.TEXT_SIZE_LARGE * (float)stateLib.TEXT_SIZE_LARGE / (float)stateLib.TEXT_SIZE_NORMAL;
				linespacing = 0.825f * (float)stateLib.TEXT_SIZE_SMALL / (float)stateLib.TEXT_SIZE_VERY_LARGE * (float)stateLib.TEXT_SIZE_VERY_LARGE / (float)stateLib.TEXT_SIZE_LARGE * (float)stateLib.TEXT_SIZE_LARGE / (float)stateLib.TEXT_SIZE_NORMAL;
				lineOffset	= -0.3f * (float)stateLib.TEXT_SIZE_SMALL / (float)stateLib.TEXT_SIZE_VERY_LARGE * (float)stateLib.TEXT_SIZE_VERY_LARGE / (float)stateLib.TEXT_SIZE_LARGE * (float)stateLib.TEXT_SIZE_LARGE / (float)stateLib.TEXT_SIZE_NORMAL;
                textscale = 1.75f * (float)stateLib.TEXT_SIZE_SMALL / (float)stateLib.TEXT_SIZE_VERY_LARGE * (float)stateLib.TEXT_SIZE_VERY_LARGE / (float)stateLib.TEXT_SIZE_LARGE * (float)stateLib.TEXT_SIZE_LARGE / (float)stateLib.TEXT_SIZE_NORMAL;
                break;
			default:
				break;
		}
		this.transform.position = defaultPosition;
		this.transform.localScale = defaultLocalScale;
		this.transform.position -= new Vector3(0, linecount * linespacing / 2, 0);
		this.transform.localScale += new Vector3(0, levelLineRatio * linecount, 0);
		if (nTextSizeConst == stateLib.TEXT_SIZE_LARGE) {
			this.transform.position += new Vector3(2.2f, 0, 0);
			this.transform.localScale += new Vector3(2, 0, 0);
		}
		// Redraw lines --[
		foreach (GameObject line in lines) {
			Destroy(line);
		}
		lines.Clear();
		for (int i = 0; i < linecount; i++) {
			float fTransform = initialLineY - i * linespacing + lineOffset;
			GameObject newline =(GameObject)Instantiate(lineobject, new Vector3(initialLineX, fTransform, 1.1f), transform.rotation);
			newline.transform.localScale += new Vector3(0.35f, 0, 0);
			if (nTextSizeConst == stateLib.TEXT_SIZE_LARGE) {
				// Transition from Large to Very Large. In Very Large, the lines need to be longer.
				newline.transform.position += new Vector3(2.4f, 0, 0);
				newline.transform.localScale += new Vector3(0.8f, 0, 0);
			}
			lines.Add(newline);
		}
		// ]-- Redraw lines

		// Resize game objects
		// Each game object needs to know its line number I think. Alternatively, the line number can be derived based on its position.
		foreach(GameObject printer in prints) {
			printer.transform.position = new Vector3(-7, initialLineY - printer.GetComponent<printer>().index * linespacing, 1);
		}
		foreach(GameObject warp in roboBUGwarps) {
			warp.transform.position = new Vector3(-7, initialLineY - warp.GetComponent<warper>().index * linespacing, 1);

		}
		/*
		foreach(GameObject bug in bugs) {
			bug.transform.position = new Vector3(bugXshift + col * fontwidth +(bugsize - 1) * levelLineRatio, initialLineY -(lineInfo.LineNumber + row - 1 + 0.5f *(bugsize - 1)) * linespacing + 0.4f, 0f);

		}
		foreach(GameObject comment in roboBUGcomments) {
			comment.transform.position = new Vector3(-7, initialLineY -(lineInfo.LineNumber - 1 + 0.9f *(commentsize - 1)) * linespacing, 0f);
		}
		*/
        List<GameObject> allComments = new List<GameObject>();
        allComments.AddRange(robotONcorrectComments);
        allComments.AddRange(robotONincorrectComments);
        allComments.AddRange(robotONcorrectUncomments);
        allComments.AddRange(robotONincorrectUncomments);
        foreach(GameObject comment in allComments) {
            comment thisComment = comment.GetComponent<comment>();
            comment.transform.position = new Vector3(-7, initialLineY -(thisComment.index + 0.93f * (thisComment.size - 1)) * linespacing, 0f);
			float yPos = (textscale * (thisComment.size - 1) > 0) ? textscale * (thisComment.size - 1) : 1.0f;
            comment.transform.localScale = new Vector3(comment.transform.localScale.x, yPos, comment.transform.localScale.z);
        }
		foreach(GameObject question in robotONquestions) {
			question.transform.position = new Vector3(-7, initialLineY -question.GetComponent<question>().index * linespacing, 1);
		}
		foreach(GameObject rename in robotONrenamers) {
			rename.transform.position = new Vector3(-7, initialLineY - rename.GetComponent<rename>().index * linespacing, 1);
		}
		/*
		foreach(GameObject breakpoint in roboBUGbreakpoints) {
			breakpoint.transform.position = new Vector3(-10, initialLineY -(lineInfo.LineNumber - 1) * linespacing + 0.4f, 1);

		}
		foreach(GameObject prize in roboBUGprizes) {
			prize.transform.position = new Vector3(-9f +(bugsize - 1) * levelLineRatio, initialLineY -(lineInfo.LineNumber - 1 + 0.5f *(bugsize - 1)) * linespacing + 0.4f, 0f);
		}
		*/
		foreach(GameObject beacon in robotONbeacons) {
			beacon.transform.position = new Vector3(-9.95f, initialLineY - beacon.GetComponent<beacon>().index * linespacing + lineOffset + 0.4f, 1);
		}
		foreach(GameObject varcolor in robotONvariablecolors) {
			varcolor.transform.position = new Vector3(-7, initialLineY - varcolor.GetComponent<VariableColor>().index * linespacing, 1);
		}

	}

	//.................................>8.......................................
	//************************************************************************//
	// Method: public void ToggleLightDark()
	// Description: Transform the play area to correspond to a new text size.
	//************************************************************************//
	public void ToggleLightDark() {
		if (backgroundLightDark == false) {
			backgroundLightDark = true;
			backgroundImage.GetComponent<GUITexture>().texture 		= lightBackground;
			this.GetComponent<SpriteRenderer>().sprite 				= whiteCodescreen;
			this.GetComponent<SpriteRenderer>().color 				= new Color(0.94f, 0.97f, 0.99f, 0.8f);
			sidebartimer.GetComponent<GUIText>().color 				= Color.black;
			sidebarpanel.GetComponent<GUITexture>().texture 		= panel6;
			destext.GetComponent<TextMesh>().color 					= Color.black;
			leveltext.GetComponent<TextMesh>().color 				= Color.black;
			sidebaroutput.GetComponent<GUIText>().color 			= Color.black;
			sidebarChecklist.GetComponent<GUIText>().color 			= Color.black;
			selectedtool.GetComponent<GUIText>().color 				= Color.black;
			sidebarLabel.GetComponent<GUIText>().color 				= Color.black;
			// Labels are updated on each frame in SelectedTool.cs
			sidebarDescription.GetComponent<GUIText>().color		= Color.black;
			outputEnter.GetComponent<GUIText>().color				= Color.black;
			menuTitle.GetComponent<TextMesh>().color				= Color.black;
			cinematicEnter.GetComponent<TextMesh>().color			= Color.black;
			cinematic.GetComponent<TextMesh>().color				= Color.black;
			credits.GetComponent<TextMesh>().color					= Color.black;
			toolprompt.GetComponent<TextMesh>().color				= Color.black;
			outputpanel.GetComponent<GUITexture>().texture			= panel7;
            menuSubmenu.GetComponent<SpriteRenderer>().sprite		= panel8;
			menu.GetComponent<SpriteRenderer>().sprite				= panel9;
			foreach (GameObject line in lines) {
				line.GetComponent<SpriteRenderer>().color 	= new Color(0.95f, 0.95f, 0.95f, 1);
			}
			// Switch the text colors to correspond with the Light Color palette (darker colors)
			for (int i = 0 ; i < innerXmlLines.GetLength(0) ; i++) {
				innerXmlLines[i] = innerXmlLines[i].Replace(stringLibrary.node_color_print, stringLibrary.node_color_print_dark);
				innerXmlLines[i] = innerXmlLines[i].Replace(stringLibrary.node_color_warp, stringLibrary.node_color_warp_dark);
				innerXmlLines[i] = innerXmlLines[i].Replace(stringLibrary.node_color_rename, stringLibrary.node_color_rename_dark);
				innerXmlLines[i] = innerXmlLines[i].Replace(stringLibrary.node_color_question, stringLibrary.node_color_question_dark);
				innerXmlLines[i] = innerXmlLines[i].Replace(stringLibrary.node_color_uncomment, stringLibrary.node_color_uncomment_dark);
				innerXmlLines[i] = innerXmlLines[i].Replace(stringLibrary.node_color_incorrect_uncomment, stringLibrary.node_color_incorrect_uncomment_dark);
				innerXmlLines[i] = innerXmlLines[i].Replace(stringLibrary.node_color_correct_comment, stringLibrary.node_color_correct_comment_dark);
				innerXmlLines[i] = innerXmlLines[i].Replace(stringLibrary.node_color_incorrect_comment, stringLibrary.node_color_incorrect_comment_dark);
				innerXmlLines[i] = innerXmlLines[i].Replace(stringLibrary.node_color_comment, stringLibrary.node_color_comment_dark);

				innerXmlLines[i] = innerXmlLines[i].Replace(stringLibrary.syntax_color_comment, stringLibrary.syntax_color_comment_dark);
				innerXmlLines[i] = innerXmlLines[i].Replace(stringLibrary.syntax_color_keyword, stringLibrary.syntax_color_keyword_dark);
				innerXmlLines[i] = innerXmlLines[i].Replace(stringLibrary.syntax_color_badcomment, stringLibrary.syntax_color_badcomment_dark);
				innerXmlLines[i] = innerXmlLines[i].Replace(stringLibrary.syntax_color_string, stringLibrary.syntax_color_string_dark);
			}
			foreach (GameObject renameObj in robotONrenamers) {
				rename propertyHandler = renameObj.GetComponent<rename>();
				propertyHandler.innertext = propertyHandler.innertext.Replace(stringLibrary.node_color_rename, stringLibrary.node_color_rename_dark);
			}
			foreach (GameObject varcolorObj in robotONvariablecolors) {
				VariableColor propertyHandler = varcolorObj.GetComponent<VariableColor>();
				propertyHandler.innertext = propertyHandler.innertext.Replace(stringLibrary.node_color_rename, stringLibrary.node_color_rename_dark);
			}
			foreach (GameObject questionObj in robotONquestions) {
				question propertyHandler = questionObj.GetComponent<question>();
				propertyHandler.innertext = propertyHandler.innertext.Replace(stringLibrary.node_color_question, stringLibrary.node_color_question_dark);
			}
			stringLibrary.node_color_print 					= stringLibrary.node_color_print_dark;
			stringLibrary.node_color_warp 					= stringLibrary.node_color_warp_dark;
			stringLibrary.node_color_rename 				= stringLibrary.node_color_rename_dark;
			stringLibrary.node_color_question 				= stringLibrary.node_color_question_dark;
			stringLibrary.node_color_uncomment				= stringLibrary.node_color_uncomment_dark;
			stringLibrary.node_color_incorrect_uncomment 	= stringLibrary.node_color_incorrect_uncomment_dark;
			stringLibrary.node_color_correct_comment 		= stringLibrary.node_color_correct_comment_dark;
			stringLibrary.node_color_incorrect_comment 		= stringLibrary.node_color_incorrect_comment_dark;
			stringLibrary.node_color_comment 				= stringLibrary.node_color_comment_dark;

			stringLibrary.syntax_color_comment 				= stringLibrary.syntax_color_comment_dark;
			stringLibrary.syntax_color_keyword 				= stringLibrary.syntax_color_keyword_dark;
			stringLibrary.syntax_color_badcomment 			= stringLibrary.syntax_color_badcomment_dark;
			stringLibrary.syntax_color_string 				= stringLibrary.syntax_color_string_dark;

			stringLibrary.checklist_complete_color_tag    	      		= stringLibrary.checklist_complete_color_tag_dark;
			stringLibrary.checklist_incomplete_activate_color_tag  		= stringLibrary.checklist_incomplete_activate_color_tag_dark;
			stringLibrary.checklist_incomplete_question_color_tag   	= stringLibrary.checklist_incomplete_question_color_tag_dark;
			stringLibrary.checklist_incomplete_name_color_tag       	= stringLibrary.checklist_incomplete_name_color_tag_dark;
			stringLibrary.checklist_incomplete_comment_color_tag    	= stringLibrary.checklist_incomplete_comment_color_tag_dark;
			stringLibrary.checklist_incomplete_uncomment_color_tag  	= stringLibrary.checklist_incomplete_uncomment_color_tag_dark;

			sidebarChecklist.GetComponent<GUIText>().text = sidebarChecklist.GetComponent<GUIText>().text.Replace(stringLibrary.checklist_complete_color_tag, stringLibrary.checklist_complete_color_tag_dark);
			sidebarChecklist.GetComponent<GUIText>().text = sidebarChecklist.GetComponent<GUIText>().text.Replace(stringLibrary.checklist_incomplete_activate_color_tag, stringLibrary.checklist_incomplete_activate_color_tag_dark);
			sidebarChecklist.GetComponent<GUIText>().text = sidebarChecklist.GetComponent<GUIText>().text.Replace(stringLibrary.checklist_incomplete_question_color_tag, stringLibrary.checklist_incomplete_question_color_tag_dark);
			sidebarChecklist.GetComponent<GUIText>().text = sidebarChecklist.GetComponent<GUIText>().text.Replace(stringLibrary.checklist_incomplete_name_color_tag, stringLibrary.checklist_incomplete_name_color_tag_dark);
			sidebarChecklist.GetComponent<GUIText>().text = sidebarChecklist.GetComponent<GUIText>().text.Replace(stringLibrary.checklist_incomplete_comment_color_tag, stringLibrary.checklist_incomplete_comment_color_tag_dark);
			sidebarChecklist.GetComponent<GUIText>().text = sidebarChecklist.GetComponent<GUIText>().text.Replace(stringLibrary.checklist_incomplete_uncomment_color_tag, stringLibrary.checklist_incomplete_uncomment_color_tag_dark);

			for (int i = 0 ; i < stateLib.NUMBER_OF_TOOLS - 1; i++) {
                toolLabels[i].GetComponent<GUIText>().color = (tasklist[i] == taskscompleted[i]) ? new Color(0, 0.6f, 0.2f, 1) : Color.white;
			}

		}
		else {
			backgroundLightDark = false;
			backgroundImage.GetComponent<GUITexture>().texture 		= darkBackground;
			this.GetComponent<SpriteRenderer>().sprite 				= blackCodescreen;
			this.GetComponent<SpriteRenderer>().color 				= Color.black;
			sidebartimer.GetComponent<GUIText>().color 				= Color.white;
			sidebarpanel.GetComponent<GUITexture>().texture 		= panel3;
			destext.GetComponent<TextMesh>().color 					= Color.white;
			leveltext.GetComponent<TextMesh>().color 				= Color.white;
			sidebaroutput.GetComponent<GUIText>().color 			= Color.white;
			sidebarChecklist.GetComponent<GUIText>().color 			= Color.white;
			selectedtool.GetComponent<GUIText>().color 				= Color.white;
			sidebarLabel.GetComponent<GUIText>().color 				= Color.white;
			// Labels are updated on each frame in SelectedTool.cs
			sidebarDescription.GetComponent<GUIText>().color		= Color.white;
			outputEnter.GetComponent<GUIText>().color				= Color.white;
			menuTitle.GetComponent<TextMesh>().color				= Color.white;
			cinematicEnter.GetComponent<TextMesh>().color			= Color.white;
			cinematic.GetComponent<TextMesh>().color				= Color.white;
			credits.GetComponent<TextMesh>().color					= Color.white;
			toolprompt.GetComponent<TextMesh>().color				= Color.white;
			outputpanel.GetComponent<GUITexture>().texture			= panel5;
			menu.GetComponent<SpriteRenderer>().sprite				= panel2;
			menuSubmenu.GetComponent<SpriteRenderer>().sprite		= panel4;
			foreach (GameObject line in lines) {
				line.GetComponent<SpriteRenderer>().color 			= Color.white;
			}
			// Switch the text colors to correspond with the Dark Color palette (lighter colors)
			for (int i = 0 ; i < innerXmlLines.GetLength(0) ; i++) {
				innerXmlLines[i] = innerXmlLines[i].Replace(stringLibrary.node_color_print, stringLibrary.node_color_print_light);
				innerXmlLines[i] = innerXmlLines[i].Replace(stringLibrary.node_color_warp, stringLibrary.node_color_warp_light);
				innerXmlLines[i] = innerXmlLines[i].Replace(stringLibrary.node_color_rename, stringLibrary.node_color_rename_light);
				innerXmlLines[i] = innerXmlLines[i].Replace(stringLibrary.node_color_question, stringLibrary.node_color_question_light);
				innerXmlLines[i] = innerXmlLines[i].Replace(stringLibrary.node_color_uncomment, stringLibrary.node_color_uncomment_light);
				innerXmlLines[i] = innerXmlLines[i].Replace(stringLibrary.node_color_incorrect_uncomment, stringLibrary.node_color_incorrect_uncomment_light);
				innerXmlLines[i] = innerXmlLines[i].Replace(stringLibrary.node_color_correct_comment, stringLibrary.node_color_correct_comment_light);
				innerXmlLines[i] = innerXmlLines[i].Replace(stringLibrary.node_color_incorrect_comment, stringLibrary.node_color_incorrect_comment_light);
				innerXmlLines[i] = innerXmlLines[i].Replace(stringLibrary.node_color_comment, stringLibrary.node_color_comment_light);

				innerXmlLines[i] = innerXmlLines[i].Replace(stringLibrary.syntax_color_comment, stringLibrary.syntax_color_comment_light);
				innerXmlLines[i] = innerXmlLines[i].Replace(stringLibrary.syntax_color_keyword, stringLibrary.syntax_color_keyword_light);
				innerXmlLines[i] = innerXmlLines[i].Replace(stringLibrary.syntax_color_badcomment, stringLibrary.syntax_color_badcomment_light);
				innerXmlLines[i] = innerXmlLines[i].Replace(stringLibrary.syntax_color_string, stringLibrary.syntax_color_string_light);
			}
			foreach (GameObject renameObj in robotONrenamers) {
				rename propertyHandler = renameObj.GetComponent<rename>();
				propertyHandler.innertext = propertyHandler.innertext.Replace(stringLibrary.node_color_rename, stringLibrary.node_color_rename_light);
			}
			foreach (GameObject varcolorObj in robotONvariablecolors) {
				VariableColor propertyHandler = varcolorObj.GetComponent<VariableColor>();
				propertyHandler.innertext = propertyHandler.innertext.Replace(stringLibrary.node_color_rename, stringLibrary.node_color_rename_light);
			}
			foreach (GameObject questionObj in robotONquestions) {
				question propertyHandler = questionObj.GetComponent<question>();
				propertyHandler.innertext = propertyHandler.innertext.Replace(stringLibrary.node_color_question, stringLibrary.node_color_question_light);
			}
			stringLibrary.node_color_print 					= stringLibrary.node_color_print_light;
			stringLibrary.node_color_warp 					= stringLibrary.node_color_warp_light;
			stringLibrary.node_color_rename 				= stringLibrary.node_color_rename_light;
			stringLibrary.node_color_question 				= stringLibrary.node_color_question_light;
			stringLibrary.node_color_uncomment 				= stringLibrary.node_color_uncomment_light;
			stringLibrary.node_color_incorrect_uncomment 	= stringLibrary.node_color_incorrect_uncomment_light;
			stringLibrary.node_color_correct_comment 		= stringLibrary.node_color_correct_comment_light;
			stringLibrary.node_color_incorrect_comment 		= stringLibrary.node_color_incorrect_comment_light;
			stringLibrary.node_color_comment 				= stringLibrary.node_color_comment_light;

			stringLibrary.syntax_color_comment 				= stringLibrary.syntax_color_comment_light;
			stringLibrary.syntax_color_keyword 				= stringLibrary.syntax_color_keyword_light;
			stringLibrary.syntax_color_badcomment 			= stringLibrary.syntax_color_badcomment_light;
			stringLibrary.syntax_color_string 				= stringLibrary.syntax_color_string_light;

			stringLibrary.checklist_complete_color_tag    	      		= stringLibrary.checklist_complete_color_tag_light;
			stringLibrary.checklist_incomplete_activate_color_tag  		= stringLibrary.checklist_incomplete_activate_color_tag_light;
			stringLibrary.checklist_incomplete_question_color_tag   	= stringLibrary.checklist_incomplete_question_color_tag_light;
			stringLibrary.checklist_incomplete_name_color_tag       	= stringLibrary.checklist_incomplete_name_color_tag_light;
			stringLibrary.checklist_incomplete_comment_color_tag    	= stringLibrary.checklist_incomplete_comment_color_tag_light;
			stringLibrary.checklist_incomplete_uncomment_color_tag  	= stringLibrary.checklist_incomplete_uncomment_color_tag_light;

			sidebarChecklist.GetComponent<GUIText>().text = sidebarChecklist.GetComponent<GUIText>().text.Replace(stringLibrary.checklist_complete_color_tag, stringLibrary.checklist_complete_color_tag_light);
			sidebarChecklist.GetComponent<GUIText>().text = sidebarChecklist.GetComponent<GUIText>().text.Replace(stringLibrary.checklist_incomplete_activate_color_tag, stringLibrary.checklist_incomplete_activate_color_tag_light);
			sidebarChecklist.GetComponent<GUIText>().text = sidebarChecklist.GetComponent<GUIText>().text.Replace(stringLibrary.checklist_incomplete_question_color_tag, stringLibrary.checklist_incomplete_question_color_tag_light);
			sidebarChecklist.GetComponent<GUIText>().text = sidebarChecklist.GetComponent<GUIText>().text.Replace(stringLibrary.checklist_incomplete_name_color_tag, stringLibrary.checklist_incomplete_name_color_tag_light);
			sidebarChecklist.GetComponent<GUIText>().text = sidebarChecklist.GetComponent<GUIText>().text.Replace(stringLibrary.checklist_incomplete_comment_color_tag, stringLibrary.checklist_incomplete_comment_color_tag_light);
			sidebarChecklist.GetComponent<GUIText>().text = sidebarChecklist.GetComponent<GUIText>().text.Replace(stringLibrary.checklist_incomplete_uncomment_color_tag, stringLibrary.checklist_incomplete_uncomment_color_tag_light);

			for (int i = 0 ; i < stateLib.NUMBER_OF_TOOLS - 1 ; i++) {
                toolLabels[i].GetComponent<GUIText>().color = (tasklist[i] == taskscompleted[i]) ? Color.green : Color.white;
			}

		}
		DrawInnerXmlLinesToScreen();
	}

	//.................................>8.......................................
    public void floatingTextOnPlayer(string sMessage) {
        toolprompt.GetComponent<TextMesh>().text = sMessage;
        Animator anim = toolprompt.GetComponent<Animator>();
        anim.Play("hide");
    }

    //.................................>8.......................................
}

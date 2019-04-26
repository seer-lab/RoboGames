using UnityEngine;
using UnityEngine.UI; 
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Text;
using System.Xml;
using UnityEngine.SceneManagement; 
using System.Text.RegularExpressions;
using System;

public partial class LevelGenerator : MonoBehaviour {
	public stringLib stringLibrary = new stringLib();
    public CodeFormater codeFormatter; 
	// A state-transition variable. When this becomes true, when Update() is called it will trigger a Game Over state.
	public bool isLosing;
	public bool isAnswering = false;
	public bool backgroundLightDark = false;
	public bool sidebarToggle = true;
	// The amount of time the player has to complete this level. Read from XML file.

	// The number of Bugs remaining in this level. Originally read from XML file, it will decrease as players squash bugs.
	public int numberOfBugsRemaining = 0;
	// Number of in-flight wrenches/shurikens.
	public int toolsAirborne = 0;
	// The number of lines in the XML file. Computed by counting the number of newline characters the XML contains.
	public int renamegroupidCounter = 0;
	// Lines of code stored in an array. innerXmlLines is the colorized text from NodeToColorString(), outerXmlLnes is the line with the tags.
	public string[] lineNumbers;
	// Stores the audio clips used in the game.
	public AudioClip[] sounds = new AudioClip[10];
	// Stores the icons for each tool.
	public GameObject[] toolIcons = new GameObject[stateLib.NUMBER_OF_TOOLS];

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

	public Sprite whiteCodescreen;
	public Sprite blackCodescreen;

	// Reference to SelectedTool object. When ProvisionToolsFromXml() is called, tools are provisioned and then passed to SelectedTool object.
	public GameObject selectedtool;

	public GameObject menuSubmenu;
	public GameObject menuTitle;
	public GameObject credits;
	public GameObject toolprompt;

    private Output output; 
    private SidebarController sidebar;
    private BackgroundController background; 

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
    LevelManager manager; 
	//.................................>8.......................................
	// Use this for initialization
	private void Start() { 
		GlobalState.GameMode 					 = stringLib.GAME_MODE_ON;
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
        codeFormatter = new CodeFormater(); 
		GlobalState.GameState 					 = stateLib.GAMESTATE_IN_GAME;
        GlobalState.level.Tasks = new int[5];
        GlobalState.level.CompletedTasks = new int[5]; 
		for (int i = 0; i < 5; i++) {
            GlobalState.level.Tasks[i] = 0;
            GlobalState.level.CompletedTasks[i] = 0;
		}
		isTimerAlarmTriggered = false;
		winning = false;
        manager = new LevelManager();
        output = GameObject.Find("OutputCanvas").transform.GetChild(0).gameObject.GetComponent<Output>();
        sidebar = GameObject.Find("Sidebar").GetComponent<SidebarController>();
        background = GameObject.Find("BackgroundCanvas").GetComponent<BackgroundController>();
        BuildLevel();
	}
 
    // This is called every draw call in game.
    private void Update() {

	}

    public void BuildLevel()
    {
        ResetLevel(false);
        CreateLevelLines(GlobalState.level.LineCount);
        PlaceObjects(GlobalState.level.LevelNode);
        ProvisionToolsFromXml(GlobalState.level.NodeList);

        // Resize the boundaries of the level to correspond with how many lines we have
        if (leveltext.GetComponent<TextMesh>().fontSize == stateLib.TEXT_SIZE_VERY_LARGE)
        {
            this.transform.position -= new Vector3(0, GlobalState.level.LineCount * linespacing / 2, 0);
            this.transform.position += new Vector3(2.2f, 0, 0);
            this.transform.localScale += new Vector3(2, levelLineRatio * GlobalState.level.LineCount, 0);
        }
        else
        {
            this.transform.position -= new Vector3(0, GlobalState.level.LineCount * linespacing / 2, 0);
            this.transform.localScale += new Vector3(0.1f, levelLineRatio * GlobalState.level.LineCount, 0);
        }
        DrawInnerXmlLinesToScreen();
        if (!initialresize)
        {
            // Make the text large in size for first run.
            initialresize = true;
            TransformTextSize(leveltext.GetComponent<TextMesh>().fontSize);
        }

    }
    public void WarpPlayer(string warpToLine)
    {
        hero.transform.position = (warpToLine != "") ? new Vector3(-8, initialLineY - (int.Parse(warpToLine) - 1) * linespacing, 1) : hero.transform.position;
        GetComponent<AudioSource>().clip = sounds[1];
        GetComponent<AudioSource>().Play();
    }
	//.................................>8.......................................
	//************************************************************************//
	// Method: public void CreateLevelLines();
	// Description: Create the grey level line objects between each line of code
	//************************************************************************//
	public void CreateLevelLines(int linecount) {
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
	}



	//.................................>8.......................................
	//************************************************************************//
	// Method: public void DrawInnerXmlLinesToScreen();
	// Description: Updates the code the player sees on the screen. Also adds the line
	// numbers to the code
	//************************************************************************//
	public void DrawInnerXmlLinesToScreen(bool bRedrawLineNumbers = true) {
		string drawCode = "";
		for (int x = 1 ; x < GlobalState.level.Code.GetLength(0) + 1; x++) {
			//draw the line number next to the text
			if (bRedrawLineNumbers) {
				// Color the number next to the line depending on the tasks on the line.
				
				string lineNumber = (x).ToString();

				lineNumbers[x-1] = lineNumber;
			}
			drawCode += lineNumbers[x-1] + "\t" + GlobalState.level.Code[x-1];
			drawCode += "\n";
		}
		print("Drawcode is: " + drawCode);
		leveltext.GetComponent<TextMesh>().text = drawCode;
	}


	//.................................>8.......................................
	//************************************************************************//
	// Method: private void PlaceObjects(XmlNode levelnode)
	// Description: Read through levelnode XML and create the interactable game objects.
	//************************************************************************//
	private void PlaceObjects(XmlNode levelnode) {
		foreach (XmlNode codenode in levelnode.ChildNodes) {
			if (codenode.Name != stringLib.NODE_NAME_CODE) {
				continue;
			}
			int indexOf = 0;
			foreach(XmlNode childNode in codenode.ChildNodes)
			{
				CreateLevelObject(codenode.Attributes[stringLib.XML_ATTRIBUTE_LANGUAGE].Value, childNode, indexOf);
				foreach(char c in childNode.OuterXml)
				{
					if (c == '\n') indexOf++;
				}
			}

			// These are counters to update the blocktext of each object
			int numberOfroboBUGcomments = 0;
			int numberOfrobotONcorrectComments = 0;
			int numberOfrobotONincorrectComments = 0;
			int numberOfrobotONcorrectUncomments = 0;
			int numberOfrobotONincorrectUncomments = 0;

			// Cycle through all comment objects and assign their blocktext values here
			for (int i = 0; i < codenode.ChildNodes.Count; i++) {
				if (codenode.ChildNodes[i].Name == stringLib.NODE_NAME_COMMENT) {

					List<GameObject> allComments = new List<GameObject>();
					allComments.AddRange(robotONcorrectComments);
					allComments.AddRange(robotONincorrectComments);
					allComments.AddRange(robotONcorrectUncomments);
					allComments.AddRange(robotONincorrectUncomments);
					foreach(GameObject comment in allComments) {
						comment.transform.position = new Vector3(stateLib.LEFT_CODESCREEN_X_COORDINATE, initialLineY + stateLib.TOOLBOX_Y_OFFSET -(comment.GetComponent<comment>().index) * linespacing, 0f);
					}
					foreach(GameObject question in robotONquestions) {
						question.transform.position = new Vector3(stateLib.LEFT_CODESCREEN_X_COORDINATE, initialLineY + stateLib.TOOLBOX_Y_OFFSET -question.GetComponent<question>().Index * linespacing, 1);
					}
					foreach(GameObject rename in robotONrenamers) {
						rename.transform.position = new Vector3(stateLib.LEFT_CODESCREEN_X_COORDINATE, initialLineY + stateLib.TOOLBOX_Y_OFFSET - rename.GetComponent<rename>().Index * linespacing, 1);
					}

					GameObject thisObject;

					switch(codenode.ChildNodes[i].Attributes[stringLib.XML_ATTRIBUTE_TYPE].Value) {
						case "robobug":
						// RoboBUG comment
						thisObject = roboBUGcomments[numberOfroboBUGcomments];
						thisObject.GetComponent<comment>().size = thisObject.GetComponent<comment>().blocktext.Split('\n').Length;
						// Colorize all multi-comment line numbers green
						for (int j = 1 ; j < thisObject.GetComponent<comment>().size ; j++) {
                                GlobalState.level.TaskOnLine[thisObject.GetComponent<comment>().index + j, stateLib.TOOL_COMMENTER]++;
						}
						// Resize the hitbox for this comment to cover all lines (if multi-line comment)
						thisObject.transform.position = new Vector3(stateLib.LEFT_CODESCREEN_X_COORDINATE, initialLineY + stateLib.TOOLBOX_Y_OFFSET - (thisObject.GetComponent<comment>().index + 0.93f *(thisObject.GetComponent<comment>().size - 1)) * linespacing, 0f);
						
						//Removed; using sprites instead:
						numberOfroboBUGcomments++;
						break;
						case "description":
						if (codenode.ChildNodes[i].Attributes[stringLib.XML_ATTRIBUTE_CORRECT].Value == "true") {
							// Correct Comment
							thisObject = robotONcorrectComments[numberOfrobotONcorrectComments];			
							thisObject.GetComponent<comment>().blocktext = codenode.ChildNodes[i].InnerText;
							thisObject.GetComponent<comment>().size = thisObject.GetComponent<comment>().blocktext.Split('\n').Length;
							// Colorize all multi-comment line numbers green
							for (int j = 1 ; j < thisObject.GetComponent<comment>().size ; j++) {
                                    GlobalState.level.TaskOnLine[thisObject.GetComponent<comment>().index + j, stateLib.TOOL_COMMENTER]++;
							}
							// Resize the hitbox for this comment to cover all lines (if multi-line comment)
							thisObject.transform.position = new Vector3(stateLib.LEFT_CODESCREEN_X_COORDINATE, initialLineY + stateLib.TOOLBOX_Y_OFFSET -(thisObject.GetComponent<comment>().index) * linespacing, 0f);

							float yPos = (textscale * (thisObject.GetComponent<comment>().size - 1) > 0) ? textscale * (thisObject.GetComponent<comment>().size - 1) : 1.0f;
							//Removed; using sprites instead:						
							numberOfrobotONcorrectComments++;
						}
						else if (codenode.ChildNodes[i].Attributes[stringLib.XML_ATTRIBUTE_CORRECT].Value == "false") {
							// Incorrect comment
							thisObject = robotONincorrectComments[numberOfrobotONincorrectComments];
							thisObject.GetComponent<comment>().blocktext = codenode.ChildNodes[i].InnerText;
							thisObject.GetComponent<comment>().size = thisObject.GetComponent<comment>().blocktext.Split('\n').Length;
							// Colorize all multi-comment line numbers green
							for (int j = 1 ; j < thisObject.GetComponent<comment>().size ; j++) {
                                    GlobalState.level.TaskOnLine[thisObject.GetComponent<comment>().index + j, stateLib.TOOL_COMMENTER]++;
							}
							// Resize the hitbox for this comment to cover all lines (if multi-line comment)
							float yPos = (textscale * (thisObject.GetComponent<comment>().size - 1) > 0) ? textscale * (thisObject.GetComponent<comment>().size - 1) : 1.0f;
							thisObject.transform.position = new Vector3(stateLib.LEFT_CODESCREEN_X_COORDINATE, initialLineY + stateLib.TOOLBOX_Y_OFFSET -(thisObject.GetComponent<comment>().index) * linespacing, 0f);
							
							numberOfrobotONincorrectComments++;
						}
						break;
						case "code":
						if (codenode.ChildNodes[i].Attributes[stringLib.XML_ATTRIBUTE_CORRECT].Value == "true") {
							// Correct Uncomment
							thisObject = robotONcorrectUncomments[numberOfrobotONcorrectUncomments];
							thisObject.GetComponent<comment>().blocktext = codenode.ChildNodes[i].InnerText;
							thisObject.GetComponent<comment>().size = thisObject.GetComponent<comment>().blocktext.Split('\n').Length;
							// Colorize all multi-comment line numbers red
							for (int j = 1 ; j < thisObject.GetComponent<comment>().size ; j++) {
                                    GlobalState.level.TaskOnLine[thisObject.GetComponent<comment>().index + j, stateLib.TOOL_CONTROL_FLOW]++;
							}
							// Resize the hitbox for this comment to cover all lines (if multi-line comment)
							float yPos = (textscale * (thisObject.GetComponent<comment>().size - 1) > 0) ? textscale * (thisObject.GetComponent<comment>().size - 1) : 1.0f;
							thisObject.transform.position = new Vector3(stateLib.LEFT_CODESCREEN_X_COORDINATE, initialLineY + stateLib.TOOLBOX_Y_OFFSET -(thisObject.GetComponent<comment>().index) * linespacing, 0f);
							
							//Removed; using sprites instead:
							//thisObject.transform.localScale = new Vector3(thisObject.transform.localScale.x, yPos, thisObject.transform.localScale.z);
							numberOfrobotONcorrectUncomments++;
						}
						else if (codenode.ChildNodes[i].Attributes[stringLib.XML_ATTRIBUTE_CORRECT].Value == "false") {
							// Incorrect Uncomment
							thisObject = robotONincorrectUncomments[numberOfrobotONincorrectUncomments];
							thisObject.GetComponent<comment>().blocktext = codenode.ChildNodes[i].InnerText;
							thisObject.GetComponent<comment>().size = thisObject.GetComponent<comment>().blocktext.Split('\n').Length;
							// Colorize all multi-comment line numbers red
							for (int j = 1 ; j < thisObject.GetComponent<comment>().size ; j++) {
                                    GlobalState.level.TaskOnLine[thisObject.GetComponent<comment>().index + j, stateLib.TOOL_CONTROL_FLOW]++;
							}
							// Resize the hitbox for this comment to cover all lines (if multi-line comment)
							float yPos = (textscale * (thisObject.GetComponent<comment>().size - 1) > 0) ? textscale * (thisObject.GetComponent<comment>().size - 1) : 1.0f;
							thisObject.transform.position = new Vector3(stateLib.LEFT_CODESCREEN_X_COORDINATE, initialLineY + stateLib.TOOLBOX_Y_OFFSET -(thisObject.GetComponent<comment>().index) * linespacing, 0f);

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

	//.................................>8.......................................
	//************************************************************************//
	// Method: int void CreateLevelObjects(XmlNode parentNode)
	// Description: Handle XML parsing. Right now it just returns 1 if a tag was found.
	//              Returns 0 if an XML tag was not found.
	//************************************************************************//
	GameObject CreateLevelObject(String language, XmlNode childnode, int lineNumber) {
		if (childnode.NodeType != XmlNodeType.Element) {
			//print("CreateLevelObject: This is just text, no game object to create.");
			return null;
		}
        ToolFactory toolFactory; 
		switch(childnode.Name) {
			case stringLib.NODE_NAME_PRINT: {
                    toolFactory = new PrinterFactory(childnode, lineNumber);
                    GameObject newoutput = toolFactory.GetGameObject(); 
				    newoutput.transform.position = new Vector3(stateLib.LEFT_CODESCREEN_X_COORDINATE, initialLineY - lineNumber * linespacing, 1);
				    return newoutput;
			}
			case stringLib.NODE_NAME_WARP: {
                    toolFactory = new WarperFactory(childnode, lineNumber);
                    GameObject newWarper = toolFactory.GetGameObject();
                    newWarper.transform.position =  new Vector3(stateLib.LEFT_CODESCREEN_X_COORDINATE, initialLineY - (lineNumber) * linespacing, 1);       
				    return newWarper;
			}
			case stringLib.NODE_NAME_BUG: {
				print("CreateLevelObject: Creating bug node with index of " + lineNumber);
				bugsize = int.Parse(childnode.Attributes[stringLib.XML_ATTRIBUTE_SIZE].Value);
				int row = 0;
				if (childnode.Attributes[stringLib.XML_ATTRIBUTE_ROW].Value != null) {
					row = int.Parse(childnode.Attributes[stringLib.XML_ATTRIBUTE_ROW].Value);
				}
				int col = 0;
				if (childnode.Attributes[stringLib.XML_ATTRIBUTE_COL].Value != null) {
					col = int.Parse(childnode.Attributes[stringLib.XML_ATTRIBUTE_COL].Value);
				}
				levelbug =(GameObject)Instantiate(bugobject, new Vector3(bugXshift + col * fontwidth +(bugsize - 1) * levelLineRatio, initialLineY -(lineNumber + row + 0.5f *(bugsize - 1)) * linespacing + 0.4f, 0f), transform.rotation);
				levelbug.transform.localScale += new Vector3(bugscale *(bugsize - 1), bugscale *(bugsize - 1), 0);
				GenericBug propertyHandler = levelbug.GetComponent<GenericBug>();
                GlobalState.level.TaskOnLine[lineNumber, stateLib.TOOL_CATCHER_OR_ACTIVATOR]++;
				bugs.Add(levelbug);
				numberOfBugsRemaining++;
				return levelbug;
			}
			case stringLib.NODE_NAME_COMMENT: {
				print("CreateLevelObject: Creating comment node with index of " + lineNumber);
				GameObject newcomment = (GameObject)Instantiate(commentobject, new Vector3(stateLib.LEFT_CODESCREEN_X_COORDINATE, initialLineY - lineNumber * linespacing, 0f), transform.rotation);
				comment propertyHandler = newcomment.GetComponent<comment>();
				propertyHandler.groupid = int.Parse(childnode.Attributes[stringLib.XML_ATTRIBUTE_GROUPID].Value);
				propertyHandler.index = lineNumber;
				propertyHandler.CodescreenObject = this.gameObject;
				propertyHandler.ToolSelectorObject = selectedtool;
				propertyHandler.language = language;
				try {
					propertyHandler.commentStyle = childnode.Attributes[stringLib.XML_ATTRIBUTE_COMMENT_STYLE].Value;
				}
				catch {
					propertyHandler.commentStyle = "single";
				}
				switch(childnode.Attributes[stringLib.XML_ATTRIBUTE_TYPE].Value) {
					case "robobug":
					roboBUGcomments.Add(newcomment);
					propertyHandler.entityType = stateLib.ENTITY_TYPE_ROBOBUG_COMMENT;
					propertyHandler.errmsg = childnode.Attributes[stringLib.XML_ATTRIBUTE_TEXT].Value;
					propertyHandler.SidebarObject = output.text;
					if (childnode.Attributes[stringLib.XML_ATTRIBUTE_TOOL].Value != null) {
						string toolatt = childnode.Attributes[stringLib.XML_ATTRIBUTE_TOOL].Value;
						string[] toolcounts = toolatt.Split(',');
						for (int i = 0; i < stateLib.NUMBER_OF_TOOLS; i++) {
							propertyHandler.tools[i] = int.Parse(toolcounts[i]);
						}
					}
					break;
					case "description":
                            GlobalState.level.TaskOnLine[lineNumber, stateLib.TOOL_COMMENTER]++;
					if (childnode.Attributes[stringLib.XML_ATTRIBUTE_CORRECT].Value == "true") {
						robotONcorrectComments.Add(newcomment);
						propertyHandler.entityType = stateLib.ENTITY_TYPE_CORRECT_COMMENT;
                        GlobalState.level.Tasks[3]++;
					}
					else if(childnode.Attributes[stringLib.XML_ATTRIBUTE_CORRECT].Value == "false") {
						robotONincorrectComments.Add(newcomment);
						propertyHandler.entityType = stateLib.ENTITY_TYPE_INCORRECT_COMMENT;
					}
					else {
						print("Error: Description comment is neither true or false");
					}
					break;
					case "code":
                            GlobalState.level.TaskOnLine[lineNumber, stateLib.TOOL_CONTROL_FLOW]++;
					if (childnode.Attributes[stringLib.XML_ATTRIBUTE_CORRECT].Value == "true") {
						robotONcorrectUncomments.Add(newcomment);
						propertyHandler.entityType = stateLib.ENTITY_TYPE_CORRECT_UNCOMMENT;
                        GlobalState.level.Tasks[4]++;
					}
					else if (childnode.Attributes[stringLib.XML_ATTRIBUTE_CORRECT].Value == "false") {
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
				    print("CreateLevelObject: Creating question node with index of " + lineNumber);
                    toolFactory = new QuestionFactory(childnode, lineNumber);
                    GameObject newquestion = toolFactory.GetGameObject();
                    newquestion.transform.position = new Vector3(stateLib.LEFT_CODESCREEN_X_COORDINATE, initialLineY - lineNumber * linespacing, 1); 
                    robotONquestions.Add(newquestion);
				    return newquestion;
			}
			case stringLib.NODE_NAME_RENAME: {
				print("CreateLevelObject: Creating rename node with index of " + lineNumber);
                    toolFactory = new RenamerFactory(childnode, lineNumber);
                    GameObject newrename = toolFactory.GetGameObject(); 
                    newrename.transform.position = new Vector3(stateLib.LEFT_CODESCREEN_X_COORDINATE, initialLineY + stateLib.TOOLBOX_Y_OFFSET - lineNumber * linespacing, 1);               
			        robotONrenamers.Add(newrename);								
				    return newrename;
			}
			case stringLib.NODE_NAME_BREAKPOINT: {
				    print("CreateLevelObject: Creating breakpoint node with index of " + lineNumber);
                    toolFactory = new BreakpointFactory(childnode, lineNumber);
                    GameObject newbreakpoint = toolFactory.GetGameObject();
                    newbreakpoint.transform.position = new Vector3(-10, initialLineY - lineNumber * linespacing + 0.4f, 1);             
				    roboBUGbreakpoints.Add(newbreakpoint);			
				    return newbreakpoint;
			}
			case stringLib.NODE_NAME_PRIZE: {
				print("CreateLevelObject: Creating prize node with index of " + lineNumber);
				bugsize = int.Parse(childnode.Attributes[stringLib.XML_ATTRIBUTE_SIZE].Value);
				GameObject prizebug =(GameObject)Instantiate(prizeobject, new Vector3(-9f +(bugsize - 1) * levelLineRatio, initialLineY -(lineNumber + 0.5f *(bugsize - 1)) * linespacing + 0.4f, 0f), transform.rotation);
				prizebug.transform.localScale += new Vector3(bugscale *(bugsize - 1), bugscale *(bugsize - 1), 0);
				PrizeBug propertyHandler = prizebug.GetComponent<PrizeBug>();
				string[] bonuses = childnode.Attributes[stringLib.XML_ATTRIBUTE_BONUSES].Value.Split(',');
				for (int i = 0; i < stateLib.NUMBER_OF_TOOLS; i++) {
					propertyHandler.bonus[i] += int.Parse(bonuses[i]);
				}
				roboBUGprizes.Add(prizebug);
				return prizebug;
			}
			case stringLib.NODE_NAME_BEACON: {
				    print("CreateLevelObject: Creating beacon node with index of " + lineNumber);
                    toolFactory = new BeaconFactory(childnode, lineNumber);
                    GameObject newbeacon = toolFactory.GetGameObject();
                    newbeacon.transform.position = new Vector3(-9.95f, initialLineY - lineNumber * linespacing + lineOffset + 0.4f, 1);
				    robotONbeacons.Add(newbeacon);
				    return newbeacon;
			}
			case stringLib.NODE_NAME_VARIABLE_COLOR: {
				    print("CreateLevelObject: Creating print node with index of " + lineNumber);
                    toolFactory = new VariableColorFactory(childnode, lineNumber);
                    GameObject newvariablecolor = toolFactory.GetGameObject();
                    newvariablecolor.transform.position = new Vector3(stateLib.LEFT_CODESCREEN_X_COORDINATE, initialLineY - lineNumber * linespacing, 1);
				    robotONvariablecolors.Add(newvariablecolor);
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

    public void ProvisionToolsFromXml(IList<XmlNode> nodelist)
    {
        foreach (XmlNode tool in nodelist)
        {
            // Set the tool count for each tool node --[
            int toolnum = 0;
            Debug.Log("Working with node: " + tool.OuterXml);
            switch (tool.Attributes[stringLib.XML_ATTRIBUTE_NAME].Value)
            {
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
                case "commenter":
                    toolnum = stateLib.TOOL_COMMENTER;
                    break;
                case "controlflow":
                    toolnum = stateLib.TOOL_CONTROL_FLOW;
                    break;
                default:
                    break;
            }
            toolIcons[toolnum].GetComponent<Image>().enabled = bool.Parse(tool.Attributes[stringLib.XML_ATTRIBUTE_ENABLED].Value);
            selectedtool.GetComponent<SelectedTool>().toolCounts[toolnum] = (tool.Attributes[stringLib.XML_ATTRIBUTE_COUNT].Value == "unlimited") ? 999 : int.Parse(tool.Attributes[stringLib.XML_ATTRIBUTE_COUNT].Value);
            // ]-- End of tool count for each tool node
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

        // Reset local variables
        if (output != null)output.text.GetComponent<Text>().text = "";
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
				toolIcons[i].GetComponent<Image>().enabled 		 = false;
				toolIcons[i].GetComponent<Image>().color 			 = new Color(0.3f, 0.3f, 0.3f);
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


		// Move player to default position
		hero.transform.position = leveltext.transform.position;
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
		this.transform.position -= new Vector3(0, GlobalState.level.LineCount * linespacing / 2, 0);
		this.transform.localScale += new Vector3(0, levelLineRatio * GlobalState.level.LineCount, 0);
		if (nTextSizeConst == stateLib.TEXT_SIZE_LARGE) {
			this.transform.position += new Vector3(2.2f, 0, 0);
			this.transform.localScale += new Vector3(2, 0, 0);
		}
		// Redraw lines --[
		foreach (GameObject line in lines) {
			Destroy(line);
		}
		lines.Clear();
		for (int i = 0; i < GlobalState.level.LineCount; i++) {
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
			printer.transform.position = new Vector3(stateLib.LEFT_CODESCREEN_X_COORDINATE, initialLineY - printer.GetComponent<printer>().Index * linespacing, 1);
		}
		foreach(GameObject warp in roboBUGwarps) {
			warp.transform.position = new Vector3(stateLib.LEFT_CODESCREEN_X_COORDINATE, initialLineY - warp.GetComponent<warper>().Index * linespacing, 1);

		}


        List<GameObject> allComments = new List<GameObject>();
        allComments.AddRange(robotONcorrectComments);
        allComments.AddRange(robotONincorrectComments);
        allComments.AddRange(robotONcorrectUncomments);
        allComments.AddRange(robotONincorrectUncomments);
        foreach(GameObject comment in allComments) {
	        comment thisComment = comment.GetComponent<comment>();
	        //comment.transform.position = new Vector3(stateLib.LEFT_CODESCREEN_X_COORDINATE, initialLineY + stateLib.TOOLBOX_Y_OFFSET -(thisComment.index + 1 * (thisComment.size/2)) * linespacing, 0f);
	        comment.transform.position = new Vector3(stateLib.LEFT_CODESCREEN_X_COORDINATE, initialLineY + stateLib.TOOLBOX_Y_OFFSET -(thisComment.index) * linespacing, 0f);
	        float yPos = (textscale * (thisComment.size - 1) > 0) ? textscale * (thisComment.size - 1) : 1.0f;
	
	        //Removed; using sprites instead:
	        //comment.transform.localScale = new Vector3(comment.transform.localScale.x, yPos, comment.transform.localScale.z);
        }
        foreach(GameObject question in robotONquestions) {
	        question.transform.position = new Vector3(stateLib.LEFT_CODESCREEN_X_COORDINATE, initialLineY + stateLib.TOOLBOX_Y_OFFSET -question.GetComponent<question>().Index * linespacing, 1);
        }
        foreach(GameObject rename in robotONrenamers) {
	        rename.transform.position = new Vector3(stateLib.LEFT_CODESCREEN_X_COORDINATE, initialLineY + stateLib.TOOLBOX_Y_OFFSET - rename.GetComponent<rename>().Index * linespacing, 1);
        }

        foreach(GameObject beacon in robotONbeacons) {
	        beacon.transform.position = new Vector3(-9.95f, initialLineY - beacon.GetComponent<beacon>().Index * linespacing + lineOffset + 0.4f, 1);
        }
        foreach(GameObject varcolor in robotONvariablecolors) {
	        varcolor.transform.position = new Vector3(stateLib.LEFT_CODESCREEN_X_COORDINATE, initialLineY + stateLib.TOOLBOX_Y_OFFSET - varcolor.GetComponent<VariableColor>().Index * linespacing, 1);
        }

    }

    public void ToggleLight()
    {
        if (backgroundLightDark == false)
        {
            backgroundLightDark = true;
            this.GetComponent<SpriteRenderer>().sprite = whiteCodescreen;
            this.GetComponent<SpriteRenderer>().color = new Color(0.94f, 0.97f, 0.99f, 0.8f);
            destext.GetComponent<TextMesh>().color = Color.black;
            leveltext.GetComponent<TextMesh>().color = Color.black;
            foreach (GameObject line in lines)
            {
                line.GetComponent<SpriteRenderer>().color = new Color(0.95f, 0.95f, 0.95f, 1);
            }
            // Switch the text colors to correspond with the Light Color palette (darker colors)
            for (int i = 0; i < GlobalState.level.Code.GetLength(0); i++)
            {
                GlobalState.level.Code[i] = GlobalState.level.Code[i].Replace(stringLibrary.node_color_print, stringLibrary.node_color_print_dark);
                GlobalState.level.Code[i] = GlobalState.level.Code[i].Replace(stringLibrary.node_color_warp, stringLibrary.node_color_warp_dark);
                GlobalState.level.Code[i] = GlobalState.level.Code[i].Replace(stringLibrary.node_color_rename, stringLibrary.node_color_rename_dark);
                GlobalState.level.Code[i] = GlobalState.level.Code[i].Replace(stringLibrary.node_color_question, stringLibrary.node_color_question_dark);
                GlobalState.level.Code[i] = GlobalState.level.Code[i].Replace(stringLibrary.node_color_uncomment, stringLibrary.node_color_uncomment_dark);
                GlobalState.level.Code[i] = GlobalState.level.Code[i].Replace(stringLibrary.node_color_incorrect_uncomment, stringLibrary.node_color_incorrect_uncomment_dark);
                GlobalState.level.Code[i] = GlobalState.level.Code[i].Replace(stringLibrary.node_color_correct_comment, stringLibrary.node_color_correct_comment_dark);
                GlobalState.level.Code[i] = GlobalState.level.Code[i].Replace(stringLibrary.node_color_incorrect_comment, stringLibrary.node_color_incorrect_comment_dark);
                GlobalState.level.Code[i] = GlobalState.level.Code[i].Replace(stringLibrary.node_color_comment, stringLibrary.node_color_comment_dark);

                GlobalState.level.Code[i] = GlobalState.level.Code[i].Replace(stringLibrary.syntax_color_comment, stringLibrary.syntax_color_comment_dark);
                GlobalState.level.Code[i] = GlobalState.level.Code[i].Replace(stringLibrary.syntax_color_keyword, stringLibrary.syntax_color_keyword_dark);
                GlobalState.level.Code[i] = GlobalState.level.Code[i].Replace(stringLibrary.syntax_color_badcomment, stringLibrary.syntax_color_badcomment_dark);
                GlobalState.level.Code[i] = GlobalState.level.Code[i].Replace(stringLibrary.syntax_color_string, stringLibrary.syntax_color_string_dark);
            }
            foreach (GameObject renameObj in robotONrenamers)
            {
                rename propertyHandler = renameObj.GetComponent<rename>();
                propertyHandler.innertext = propertyHandler.innertext.Replace(stringLibrary.node_color_rename, stringLibrary.node_color_rename_dark);
            }
            foreach (GameObject varcolorObj in robotONvariablecolors)
            {
                VariableColor propertyHandler = varcolorObj.GetComponent<VariableColor>();
                propertyHandler.innertext = propertyHandler.innertext.Replace(stringLibrary.node_color_rename, stringLibrary.node_color_rename_dark);
            }
            foreach (GameObject questionObj in robotONquestions)
            {
                question propertyHandler = questionObj.GetComponent<question>();
                propertyHandler.innertext = propertyHandler.innertext.Replace(stringLibrary.node_color_question, stringLibrary.node_color_question_dark);
            }

        }
    }
//.................................>8.......................................
//************************************************************************//
// Method: public void ToggleLightDark()
// Description: Toggle between light and dark color schemes for the game.
//************************************************************************//
public void ToggleDark() {
	 
		backgroundLightDark = false;
		this.GetComponent<SpriteRenderer>().sprite 				= blackCodescreen;
		this.GetComponent<SpriteRenderer>().color 				= Color.black;
		destext.GetComponent<TextMesh>().color 					= Color.white;
		leveltext.GetComponent<TextMesh>().color 				= Color.white;;
		toolprompt.GetComponent<TextMesh>().color				= Color.white;
		foreach (GameObject line in lines) {
			line.GetComponent<SpriteRenderer>().color 			= Color.white;
		}
            // Switch the text colors to correspond with the Dark Color palette (lighter colors)
            for (int i = 0; i < GlobalState.level.Code.GetLength(0); i++)
            {
                GlobalState.level.Code[i] = GlobalState.level.Code[i].Replace(stringLibrary.node_color_print, stringLibrary.node_color_print_light);
                GlobalState.level.Code[i] = GlobalState.level.Code[i].Replace(stringLibrary.node_color_warp, stringLibrary.node_color_warp_light);
                GlobalState.level.Code[i] = GlobalState.level.Code[i].Replace(stringLibrary.node_color_rename, stringLibrary.node_color_rename_light);
                GlobalState.level.Code[i] = GlobalState.level.Code[i].Replace(stringLibrary.node_color_question, stringLibrary.node_color_question_light);
                GlobalState.level.Code[i] = GlobalState.level.Code[i].Replace(stringLibrary.node_color_uncomment, stringLibrary.node_color_uncomment_light);
                GlobalState.level.Code[i] = GlobalState.level.Code[i].Replace(stringLibrary.node_color_incorrect_uncomment, stringLibrary.node_color_incorrect_uncomment_light);
                GlobalState.level.Code[i] = GlobalState.level.Code[i].Replace(stringLibrary.node_color_correct_comment, stringLibrary.node_color_correct_comment_light);
                GlobalState.level.Code[i] = GlobalState.level.Code[i].Replace(stringLibrary.node_color_incorrect_comment, stringLibrary.node_color_incorrect_comment_light);
                GlobalState.level.Code[i] = GlobalState.level.Code[i].Replace(stringLibrary.node_color_comment, stringLibrary.node_color_comment_light);
                GlobalState.level.Code[i] = GlobalState.level.Code[i].Replace(stringLibrary.syntax_color_comment, stringLibrary.syntax_color_comment_light);
                GlobalState.level.Code[i] = GlobalState.level.Code[i].Replace(stringLibrary.syntax_color_keyword, stringLibrary.syntax_color_keyword_light);
                GlobalState.level.Code[i] = GlobalState.level.Code[i].Replace(stringLibrary.syntax_color_badcomment, stringLibrary.syntax_color_badcomment_light);
                GlobalState.level.Code[i] = GlobalState.level.Code[i].Replace(stringLibrary.syntax_color_string, stringLibrary.syntax_color_string_light);
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

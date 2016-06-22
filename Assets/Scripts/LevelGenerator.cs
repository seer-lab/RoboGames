//****************************************************************************//
// @TODO: Ensure this is up to date when finished.
// Class Name: LevelGenerator
// Class Description:
// Methods:
// 		private void Start()
//		private void Update()
//		public void GUISwitch(bool gui_on)
//		public void BuildLevel(string filename, bool warp, string linenum = "")
//		private void PrintCode(XmlNode levelnode)
//		private void PlaceObjects(XmlNode levelnode)
//		public void SetTools(XmlNode levelnode)
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

public class LevelGenerator : MonoBehaviour
{
	// A state-transition variable. When this becomes true, when Update() is called it will trigger a Game Over state.
	public bool isLosing;
	// The amount of time the player has to complete this level. Read from XML file.
	public float endTime = 0f;
	// The amount of time remaining, in seconds, until the level is considered lost.
	public float remainingTime = 0f;
	// The number of Bugs remaining in this level. Originally read from XML file, it will decrease as players squash bugs.
	public int numberOfBugsRemaining = 0;
	// Refer to stateLib for distinct gamestates
	public int gamestate;
	// Contains the remaining tasks the player must complete before winning the level.
	public int[] tasklist = new int[5];
	// Contains the completed tasks of the player.
	public int[] taskscompleted = new int[5];
	// The number of lines in the XML file. Computed by counting the number of newline characters the XML contains.
	public int linecount = 0;
	// The filename of the next XML file to load in.
	public string nextlevel = "";
	// The current level, contains the filename of the XML loaded.
	public string currentlevel = "level0.xml";
	// Game Mode is either "on" or "bug", for RobotON or RoboBUG respectively. This is defined in stringLib.
	public string gamemode;
	// Stores the beacons used in this level.
	public List<GameObject> beacons;
	// Stores the audio clips used in the game.
	public AudioClip[] sounds = new AudioClip[10];
	// Stores the icons for each tool.
	public GameObject[] toolIcons = new GameObject[stateLib.NUMBER_OF_TOOLS];
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
	public GameObject uncomobject;
	public GameObject baduncomobject;
	public GameObject oncheckobject;
	public GameObject prizeobject;
	public GameObject renameobject;
	public GameObject warpobject;
	public GameObject badcommentobject;
	public GameObject commentobject;
	public GameObject oncommentobject;
	public GameObject breakpointobject;
	public GameObject hero;
	public GameObject sidebaroutput;
	// Reference to SelectedTool object. When SetTools() is called, tools are provisioned and then passed to SelectedTool object.
	public GameObject selectedtool;
	public GameObject sidebarpanel;
	public GameObject outputpanel;
	public GameObject cinematic;
	public GameObject menu;

	// Player has been notified of less than 30 seconds remaining on the clock.
	private bool isTimerAlarmTriggered;
	private bool winning;
	// This 3f is not a typo, it's different from the initialLineY in stateLib
	private float initialLineY = 3f;
	private float initialLineX = -4.5f;
	private float linespacing = 0.825f;
	private float levelLineRatio = 0.55f;
	private float bugXshift = -9.5f;
	private float fontwidth = 0.15f;
	private float bugsize = 1f;
	private float bugscale = 1.5f;
	private float textscale = 1.75f;
	private float losstime;
	private float lossdelay = 3f;
	private float leveldelay = 2f;
	private float startNextLevelTimeDelay = 0f;
	private float startTime = 0f;
	private int totalNumberOfTools = stateLib.NUMBER_OF_TOOLS;
	private string codetext;
	private GameObject levelbug;
	private List<GameObject> lines;
	private List<GameObject> outputs;
	private List<GameObject> warps;
	private List<GameObject> bugs;
	private List<GameObject> comments;
	private List<GameObject> renamers;
	private List<GameObject> oncomments;
	private List<GameObject> badcomments;
	private List<GameObject> uncoms;
	private List<GameObject> baduncoms;
	private List<GameObject> onchecks;
	private List<GameObject> breakpoints;
	private List<GameObject> prizes;


	//.................................>8.......................................
	// Use this for initialization
	private void Start() {
		gamemode 	= stringLib.GAME_MODE_BUG;
		losstime 	= 0;
		lines 		= new List<GameObject>();
		outputs 	= new List<GameObject>();
		warps 		= new List<GameObject>();
		bugs 		= new List<GameObject>();
		comments 	= new List<GameObject>();
		breakpoints = new List<GameObject>();
		beacons 	= new List<GameObject>();
		badcomments = new List<GameObject>();
		uncoms 		= new List<GameObject>();
		baduncoms 	= new List<GameObject>();
		renamers 	= new List<GameObject>();
		oncomments 	= new List<GameObject>();
		onchecks 	= new List<GameObject>();
		prizes 		= new List<GameObject>();
		isLosing 	= false;
		gamestate 	= stateLib.GAMESTATE_MENU;
		for (int i = 0; i < 5; i++) {
			tasklist[i] = 0;
			taskscompleted[i] = 0;
		}
		GUISwitch(false);
		isTimerAlarmTriggered = false;
		winning = false;
	}

	//.................................>8.......................................
	// Update is called once per frame
	private void Update() {
		if (gamestate == stateLib.GAMESTATE_IN_GAME) {
			// Running out of time. --[
			if (endTime - startTime >= 9000)
			{
				// It's over 9000!
				sidebartimer.GetComponent<GUIText>().text = "Time remaining: Unlimited";
			}
			else if (endTime - Time.time < 30) {
				sidebartimer.GetComponent<GUIText>().text = "Time remaining: <size=50><color=red>" +((int)(endTime - Time.time)).ToString() + "</color></size> seconds";
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
				if (nNumberOfSeconds > 3600)
				{
					int nNumberOfHours = nNumberOfSeconds / 3600;
					nNumberOfSeconds -= nNumberOfHours * 3600;
					int nNumberOfMinutes = (nNumberOfSeconds) / 60;
					nNumberOfSeconds -= nNumberOfMinutes * 60;
					sidebartimer.GetComponent<GUIText>().text = "Time remaining: " +
					nNumberOfHours.ToString() + "h " +
					nNumberOfMinutes.ToString() + "m " +
					nNumberOfSeconds.ToString() + "s";
				}
				else if (nNumberOfSeconds > 60) {
					int nNumberOfMinutes = nNumberOfSeconds / 60;
					nNumberOfSeconds -= nNumberOfMinutes * 60;
					sidebartimer.GetComponent<GUIText>().text = "Time remaining: " +
					nNumberOfMinutes.ToString() + "m " +
					nNumberOfSeconds.ToString() + "s";
				}
				else {
					sidebartimer.GetComponent<GUIText>().text = "Time remaining: " + nNumberOfSeconds.ToString() + " s";
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
			// For either RobotON or RoboBUG,
			if (numberOfBugsRemaining <= 0 && bugs.Count > 0 || winning) {
				if (startNextLevelTimeDelay == 0f) {
					startNextLevelTimeDelay = Time.time + leveldelay;
				}
				else if (Time.time > startNextLevelTimeDelay) {
					winning = false;
					startNextLevelTimeDelay = 0f;
					if (nextlevel != gamemode + @"leveldata\") {
						foreach (GameObject bug in bugs) {
							Destroy(bug);
						}
						GUISwitch(false);
						menu.GetComponent<Menu>().saveGame(currentlevel);
						gamestate = stateLib.GAMESTATE_LEVEL_WIN;
					}
					else {
						Victory();
					}
				}

			}
			// Handle out of time --[
			if (endTime < Time.time && (numberOfBugsRemaining > 0 || bugs.Count == 0) && endTime - startTime < 9000) {
				GameOver();
			}
			// ]--
			// Handle menu toggle (Escape key pressed) --[
			if (Input.GetKeyDown(KeyCode.Escape)) {
				gamestate = stateLib.GAMESTATE_MENU;
				menu.GetComponent<Menu>().flushButtonColor();
				GUISwitch(false);
			}
			// ]--
		}
	}

	//.................................>8.......................................
	//************************************************************************//
	// Method: public void GUISwitch(bool gui_on)
	// Description: Toggle the GUI on or off
	//************************************************************************//
	public void GUISwitch(bool gui_on)	{
		if (gui_on) {
			sidebarpanel.GetComponent<GUITexture>().enabled = true;
			outputpanel.GetComponent<GUITexture>().enabled = true;
			endTime = remainingTime + Time.time;
		}
		else {
			sidebarpanel.GetComponent<GUITexture>().enabled = false;
			outputpanel.GetComponent<GUITexture>().enabled = false;
			sidebartimer.GetComponent<GUIText>().text = "";
			remainingTime = endTime - Time.time;
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
		PrintCode(levelnode);
		PlaceObjects(levelnode);
		// Warping to this level.
		if(warp) {
			if (linenum != "") {
				hero.transform.position = new Vector3(-7, initialLineY -(int.Parse(linenum) - 1) * linespacing, 1);
			}
			GetComponent<AudioSource>().clip = sounds[1];
			GetComponent<AudioSource>().Play();
		}
		else {
			// Provision tools.
			SetTools(levelnode);
			// Store this filename as current level
			currentlevel = filename.Substring(filename.IndexOf("\\") + 1);
			// Level's starting time is set to Now.
			startTime = Time.time;
			foreach (XmlNode node in levelnode.ChildNodes) {
				// Time
				if (node.Name == stringLib.NODE_NAME_TIME) {
					if (node.InnerText == "unlimited") {
						node.InnerText = "9001";
					}
					endTime = (float)int.Parse(node.InnerText) + startTime;
					remainingTime =(float)int.Parse(node.InnerText);
				}
				// Next level
				else if (node.Name == stringLib.NODE_NAME_NEXT_LEVEL) {
					nextlevel = gamemode + @"leveldata\" + node.InnerText;
				}
				// Intro Text
				else if (node.Name == stringLib.NODE_NAME_INTRO_TEXT) {
					cinematic.GetComponent<Cinematic>().introtext = node.InnerText;
				}
				// End Text
				else if (node.Name == stringLib.NODE_NAME_END_TEXT) {
					cinematic.GetComponent<Cinematic>().endtext = node.InnerText;
				}
			}
			selectedtool.GetComponent<SelectedTool>().NextTool();
		}
		this.transform.position -= new Vector3(0,(linecount / 2) * linespacing, 0);
		this.transform.localScale += new Vector3(0, levelLineRatio * linecount, 0);
	}

	//.................................>8.......................................
	//************************************************************************//
	// Method: private void PrintCode(XmlNode levelnode)
	// Description: Read through levelnode XML and print the lines
	//************************************************************************//
	private void PrintCode(XmlNode levelnode) {
		destext.GetComponent<TextMesh>().text = "";
		foreach (XmlNode codenode in levelnode.ChildNodes) {
			// Create lines of code for the level --[
			if (codenode.Name == stringLib.CODENODE_NAME_CODE) {
				foreach (XmlNode printnode in codenode.ChildNodes) {
					if (printnode.Name == stringLib.NODE_NAME_PRINT) {
						printnode.InnerText = stringLib.NODE_COLOR_PRINT +
						printnode.InnerText +
						stringLib.CLOSE_COLOR_TAG;
					}
					else if (printnode.Name == stringLib.NODE_NAME_WARP) {
						printnode.InnerText = stringLib.NODE_COLOR_WARP +
						printnode.InnerText +
						stringLib.CLOSE_COLOR_TAG;
					}
					else if (printnode.Name == stringLib.NODE_NAME_RENAME) {
						printnode.InnerText = stringLib.NODE_COLOR_RENAME +
						printnode.InnerText +
						stringLib.CLOSE_COLOR_TAG;
					}
					else if (printnode.Name == stringLib.NODE_NAME_ON_CHECK) {
						printnode.InnerText = stringLib.NODE_COLOR_ON_CHECK +
						printnode.InnerText +
						stringLib.CLOSE_COLOR_TAG;
						printnode.InnerText += "\n";
					}
					else if (printnode.Name == stringLib.NODE_NAME_UNCOMMENT) {
						printnode.InnerText = stringLib.NODE_COLOR_UNCOMMENT +
						stringLib.COMMENT_CLOSE_COLOR_TAG +
						"     " +
						stringLib.NODE_COLOR_UNCOMMENT +
						printnode.InnerText +
						stringLib.COMMENT_CLOSE_COLOR_TAG;
						printnode.InnerText += "\n";
					}
					else if (printnode.Name == stringLib.NODE_NAME_BAD_UNCOMMENT) {
						printnode.InnerText = stringLib.NODE_COLOR_BAD_UNCOMMENT +
						stringLib.COMMENT_CLOSE_COLOR_TAG +
						"     " +
						stringLib.NODE_COLOR_BAD_UNCOMMENT +
						printnode.InnerText +
						stringLib.COMMENT_CLOSE_COLOR_TAG;
						printnode.InnerText += "\n";
					}
					else if (printnode.Name == stringLib.NODE_NAME_ON_COMMENT) {
						printnode.InnerText = stringLib.NODE_COLOR_ON_COMMENT +
						stringLib.COMMENT_CLOSE_COLOR_TAG +
						"     " +
						printnode.InnerText;
						printnode.InnerText += "\n";
					}
					else if (printnode.Name == stringLib.NODE_NAME_BAD_COMMENT) {
						printnode.InnerText = stringLib.NODE_COLOR_BAD_COMMENT +
						stringLib.COMMENT_CLOSE_COLOR_TAG +
						"     " +
						printnode.InnerText;
						printnode.InnerText += "\n";
					}
					else if (printnode.Name == stringLib.NODE_NAME_COMMENT) {
						printnode.InnerText = stringLib.NODE_COLOR_COMMENT +
						stringLib.COMMENT_CLOSE_COLOR_TAG +
						printnode.InnerText +
						stringLib.NODE_COLOR_COMMENT +
						stringLib.COMMENT_CLOSE_COLOR_TAG;
					}
					else
					{
						printnode.InnerText = ColorizeKeywords(printnode.InnerText);
					}
				}
				// ]-- end of print lines

				// Count the number of lines in this level, store it in linecount --[
				codetext = codenode.InnerText;
				foreach (char c in codetext) {
					if (c == '\n')
					linecount++;
				}
				// ]-- End of count
			}
			// Create the level description
			else if (codenode.Name == stringLib.CODENODE_NAME_DESCRIPTION) {
				destext.GetComponent<TextMesh>().text = codenode.InnerText;
			}
		}
		// Create the grey line objects for each line.
		for (int i = 0; i < linecount; i++) {
			GameObject newline =(GameObject)Instantiate(lineobject, new Vector3(initialLineX, initialLineY - i * linespacing, 1), transform.rotation);
			lines.Add(newline);
		}
		leveltext.GetComponent<TextMesh>().text = codetext;
	}

	//.................................>8.......................................
	//************************************************************************//
	// Method: private void PlaceObjects(XmlNode levelnode)
	// Description: Read through levelnode XML and create the necessary game objects
	//************************************************************************//
	private void PlaceObjects(XmlNode levelnode) {
		foreach (XmlNode codenode in levelnode.ChildNodes) {
			if (codenode.Name == stringLib.CODENODE_NAME_CODE) {
				// Create the XmlNamespaceManager.
				XmlNamespaceManager nsmgr = new XmlNamespaceManager(new NameTable());

				// Create the XmlParserContext.
				XmlParserContext context = new XmlParserContext(null, nsmgr, null, XmlSpace.None);

				// Create the reader.
				XmlValidatingReader xmlReader = new XmlValidatingReader(codenode.InnerXml, XmlNodeType.Element, context);

				IXmlLineInfo lineInfo = ((IXmlLineInfo)xmlReader);
				while(xmlReader.Read()) {
					if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name == stringLib.NODE_NAME_PRINT) {
						GameObject newoutput =(GameObject)Instantiate(printobject, new Vector3(-7, initialLineY -(lineInfo.LineNumber - 1) * linespacing, 1), transform.rotation);
						outputs.Add(newoutput);
						printer printcode = newoutput.GetComponent<printer>();
						printcode.displaytext = xmlReader.GetAttribute("text");
						printcode.sidebar = sidebaroutput;
						printcode.selectTools = selectedtool;
						if (xmlReader.GetAttribute("tool") != null) {
							string toolatt = xmlReader.GetAttribute("tool");
							string[] toolcounts = toolatt.Split(',');
							for (int i = 0; i < stateLib.NUMBER_OF_TOOLS; i++) {
								printcode.tools[i] = int.Parse(toolcounts[i]);
							}
						}
					}
					else if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name == stringLib.NODE_NAME_WARP) {
						GameObject newwarp = (GameObject)Instantiate(warpobject, new Vector3(-7, initialLineY -(lineInfo.LineNumber - 1) * linespacing, 1), transform.rotation);
						warps.Add(newwarp);
						warper warpcode = newwarp.GetComponent<warper>();
						warpcode.CodescreenObject = this.gameObject;
						warpcode.filename = xmlReader.GetAttribute("file");
						warpcode.SelectToolsObject = selectedtool;
						if (xmlReader.GetAttribute("tool") != null) {
							string toolatt = xmlReader.GetAttribute("tool");
							string[] toolcounts = toolatt.Split(',');
							for (int i = 0; i < stateLib.NUMBER_OF_TOOLS; i++) {
								warpcode.tools[i] = int.Parse(toolcounts[i]);
							}
						}
						if (xmlReader.GetAttribute("line") != null) {
							warpcode.linenum = xmlReader.GetAttribute("line");
						}
					}
					else if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name == stringLib.NODE_NAME_BUG) {
						bugsize = int.Parse(xmlReader.GetAttribute("size"));
						int row = 0;
						if (xmlReader.GetAttribute("row") != null) {
							row = int.Parse(xmlReader.GetAttribute("row"));
						}
						int col = 0;
						if (xmlReader.GetAttribute("col") != null) {
							col = int.Parse(xmlReader.GetAttribute("col"));
						}
						levelbug =(GameObject)Instantiate(bugobject, new Vector3(bugXshift + col * fontwidth +(bugsize - 1) * levelLineRatio, initialLineY -(lineInfo.LineNumber + row - 1 + 0.5f *(bugsize - 1)) * linespacing + 0.4f, 0f), transform.rotation);
						levelbug.transform.localScale += new Vector3(bugscale *(bugsize - 1), bugscale *(bugsize - 1), 0);
						levelbug.GetComponent<GenericBug>().CodescreenObject = this.gameObject;
						bugs.Add(levelbug);
						numberOfBugsRemaining++;

					}
					else if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name == stringLib.NODE_NAME_COMMENT) {
						int commentsize = int.Parse(xmlReader.GetAttribute("size"));
						GameObject newcomment =(GameObject)Instantiate(commentobject, new Vector3(-7, initialLineY -(lineInfo.LineNumber - 1 + 0.9f *(commentsize - 1)) * linespacing, 0f), transform.rotation);
						comments.Add(newcomment);
						commentBlock commentcode = newcomment.GetComponent<commentBlock>();
						commentcode.code = leveltext;
						commentcode.errmsg = xmlReader.GetAttribute("text");
						commentcode.sideoutput = sidebaroutput;
						commentcode.oldtext = codetext;
						newcomment.transform.localScale += new Vector3(0, textscale *(commentsize - 1), 0);
						commentcode.selectTools = selectedtool;
						if (xmlReader.GetAttribute("tool") != null) {
							string toolatt = xmlReader.GetAttribute("tool");
							string[] toolcounts = toolatt.Split(',');
							for (int i = 0; i < stateLib.NUMBER_OF_TOOLS; i++) {
								commentcode.tools[i] = int.Parse(toolcounts[i]);
							}
						}
					}
					else if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name == stringLib.NODE_NAME_ON_CHECK) {
						GameObject newcheck =(GameObject)Instantiate(oncheckobject, new Vector3(-7, initialLineY -(lineInfo.LineNumber - 1) * linespacing, 1), transform.rotation);
						oncomments.Add(newcheck);
						checker oncheckcode = newcheck.GetComponent<checker>();
						oncheckcode.displaytext = xmlReader.GetAttribute("text");
						oncheckcode.expected = xmlReader.GetAttribute("answer");
						oncheckcode.CodescreenObject = this.gameObject;
						oncheckcode.SidebarObject = sidebaroutput;
						oncheckcode.CodeObject = leveltext;
						oncheckcode.innertext = xmlReader.ReadInnerXml(); //danger will robinson
						tasklist[1]++;
					}
					else if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name == stringLib.NODE_NAME_RENAME) {
						GameObject newrename =(GameObject)Instantiate(renameobject, new Vector3(-7, initialLineY -(lineInfo.LineNumber - 1) * linespacing, 1), transform.rotation);
						renamers.Add(newrename);
						rename renamecode = newrename.GetComponent<rename>();
						renamecode.displaytext = xmlReader.GetAttribute("text");
						renamecode.correct = xmlReader.GetAttribute("correct");
						renamecode.CodescreenObject = this.gameObject;
						renamecode.SidebarObject = sidebaroutput;
						renamecode.CodeObject = leveltext;

						string options = xmlReader.GetAttribute("options");
						string[] optionsArray = options.Split(',');
						for (int i = 0; i < optionsArray.Length; i++) {
							renamecode.options.Add(optionsArray[i]);
						}
						renamecode.innertext = xmlReader.ReadInnerXml();

						tasklist[2]++;
					}
					else if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name == stringLib.NODE_NAME_ON_COMMENT) {
						int commentsize = int.Parse(xmlReader.GetAttribute("size"));
						GameObject newcomment =(GameObject)Instantiate(oncommentobject, new Vector3(-7, initialLineY -(lineInfo.LineNumber - 1 + 0.9f *(commentsize - 1)) * linespacing, 0f), transform.rotation);
						oncomments.Add(newcomment);
						oncomment oncommentcode = newcomment.GetComponent<oncomment>();
						oncommentcode.CodeObject = leveltext;
						oncommentcode.oldtext = codetext;
						oncommentcode.CodescreenObject = this.gameObject;
						newcomment.transform.localScale += new Vector3(0, textscale *(commentsize - 1), 0);
						tasklist[3]++;
					}
					else if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name == stringLib.NODE_NAME_BAD_COMMENT) {
						int commentsize = int.Parse(xmlReader.GetAttribute("size"));
						GameObject newbadcom =(GameObject)Instantiate(badcommentobject, new Vector3(-7, initialLineY -(lineInfo.LineNumber - 1 + 0.9f *(commentsize - 1)) * linespacing, 0f), transform.rotation);
						badcomments.Add(newbadcom);
						badcomment badcomcode = newbadcom.GetComponent<badcomment>();
						badcomcode.CodeObject = leveltext;
						badcomcode.righttext = xmlReader.GetAttribute("righttext");
						badcomcode.oldtext = codetext;
						badcomcode.CodescreenObject = this.gameObject;
						newbadcom.transform.localScale += new Vector3(0, textscale *(commentsize - 1), 0);
					}
					else if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name == stringLib.NODE_NAME_UNCOMMENT) {
						int commentsize = int.Parse(xmlReader.GetAttribute("size"));
						GameObject newuncom =(GameObject)Instantiate(uncomobject, new Vector3(-7, initialLineY -(lineInfo.LineNumber - 1 + 0.9f *(commentsize - 1)) * linespacing, 0f), transform.rotation);
						uncoms.Add(newuncom);
						uncom uncomcode = newuncom.GetComponent<uncom>();
						uncomcode.CodeObject = leveltext;
						uncomcode.oldtext = codetext;
						uncomcode.CodescreenObject = this.gameObject;
						newuncom.transform.localScale += new Vector3(0, textscale *(commentsize - 1), 0);
						tasklist[4]++;
					}
					else if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name == stringLib.NODE_NAME_BAD_UNCOMMENT) {
						int commentsize = int.Parse(xmlReader.GetAttribute("size"));
						GameObject newbaduncom =(GameObject)Instantiate(baduncomobject, new Vector3(-7, initialLineY -(lineInfo.LineNumber - 1 + 0.9f *(commentsize - 1)) * linespacing, 0f), transform.rotation);
						baduncoms.Add(newbaduncom);
						baduncom baduncomcode = newbaduncom.GetComponent<baduncom>();
						baduncomcode.CodeObject = leveltext;
						baduncomcode.righttext = xmlReader.GetAttribute("righttext");
						baduncomcode.oldtext = codetext;
						baduncomcode.CodescreenObject = this.gameObject;
						newbaduncom.transform.localScale += new Vector3(0, textscale *(commentsize - 1), 0);
					}
					else if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name == stringLib.NODE_NAME_BREAKPOINT) {
						GameObject newbreakpoint =(GameObject)Instantiate(breakpointobject, new Vector3(-10, initialLineY -(lineInfo.LineNumber - 1) * linespacing + 0.4f, 1), transform.rotation);
						breakpoints.Add(newbreakpoint);
						Breakpoint breakcode = newbreakpoint.GetComponent<Breakpoint>();
						breakcode.sidebaroutput = sidebaroutput;
						breakcode.values = xmlReader.GetAttribute("text");
						breakcode.selectTools = selectedtool;
						if (xmlReader.GetAttribute("tool") != null) {
							string toolatt = xmlReader.GetAttribute("tool");
							string[] toolcounts = toolatt.Split(',');
							for (int i = 0; i < stateLib.NUMBER_OF_TOOLS; i++) {
								breakcode.tools[i] = int.Parse(toolcounts[i]);
							}
						}
					}
					else if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name == stringLib.NODE_NAME_PRIZE) {
						bugsize = int.Parse(xmlReader.GetAttribute("size"));
						GameObject prizebug =(GameObject)Instantiate(prizeobject, new Vector3(-9f +(bugsize - 1) * levelLineRatio, initialLineY -(lineInfo.LineNumber - 1 + 0.5f *(bugsize - 1)) * linespacing + 0.4f, 0f), transform.rotation);
						prizebug.transform.localScale += new Vector3(bugscale *(bugsize - 1), bugscale *(bugsize - 1), 0);
						prizebug.GetComponent<PrizeBug>().tools = selectedtool;
						string[] bonuses = xmlReader.GetAttribute("bonuses").Split(',');
						for (int i = 0; i < stateLib.NUMBER_OF_TOOLS; i++) {
							prizebug.GetComponent<PrizeBug>().bonus[i] += int.Parse(bonuses[i]);
						}
						prizes.Add(prizebug);
					}
					else if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name == stringLib.NODE_NAME_BEACON) {
						GameObject newbeacon =(GameObject)Instantiate(beaconobject, new Vector3(-10, initialLineY -(lineInfo.LineNumber - 1) * linespacing + 0.4f, 1), transform.rotation);
						newbeacon.GetComponent<beacon>().CodescreenObject = this.gameObject;
						if (xmlReader.GetAttribute("flow-order") != "") {
							// @TODO: Maybe move attributes to the string library for simplication as XML_ATTR_ constants
							string[] flowOrder = xmlReader.GetAttribute("flow-order").Split(',');

							for (int i = 0; i < flowOrder.Length; i++) {
								newbeacon.GetComponent<beacon>().flowOrder.Add(int.Parse(flowOrder[i]));
								tasklist[0]++;
							}
						}
						beacons.Add(newbeacon);
					}
				}
				xmlReader.Close();

				int numberOfComments = 0;
				int numberOfOnComments = 0;
				int numberOfBadComments = 0;
				int numberOfUncomments = 0;
				int numberOfBadUncomments = 0;
				for (int i = 0; i < codenode.ChildNodes.Count; i++) {
					if (codenode.ChildNodes[i].Name == stringLib.NODE_NAME_COMMENT) {
						comments[numberOfComments].GetComponent<commentBlock>().blocktext = codenode.ChildNodes[i].InnerText.Trim();
						numberOfComments++;
					}
					// @TODO: Handle substring inside of the .cs file respectively.
					if (codenode.ChildNodes[i].Name == stringLib.NODE_NAME_ON_COMMENT) {
						// Substring <color=#cccccccc>/* (19 characters) + */</color> 10 = 29.
						oncomments[numberOfOnComments].GetComponent<oncomment>().blocktext = codenode.ChildNodes[i].InnerText.Substring(29).Trim();
						numberOfOnComments++;
					}
					if (codenode.ChildNodes[i].Name == stringLib.NODE_NAME_BAD_COMMENT) {
						// Substring <color=#cccccccc>/* (19 characters) + */</color> 10 = 29.
						badcomments[numberOfBadComments].GetComponent<badcomment>().blocktext = codenode.ChildNodes[i].InnerText.Substring(29).Trim();
						numberOfBadComments++;
					}
					if (codenode.ChildNodes[i].Name == stringLib.NODE_NAME_UNCOMMENT) {
						// Substring <color=#cccccccc>/* (19 characters) + */</color> 10 = 29.
						uncoms[numberOfUncomments].GetComponent<uncom>().blocktext = codenode.ChildNodes[i].InnerText.Substring(29).Trim();
						numberOfUncomments++;
					}
					if (codenode.ChildNodes[i].Name == stringLib.NODE_NAME_BAD_UNCOMMENT) {
						// Substring <color=#cccccccc>/* (19 characters) + */</color> 10 = 29.
						baduncoms[numberOfBadUncomments].GetComponent<baduncom>().blocktext = codenode.ChildNodes[i].InnerText.Substring(29).Trim();
						numberOfBadUncomments++;
					}
				}
				foreach (GameObject badcom in badcomments) {
					foreach (GameObject oncom in oncomments) {
						if (badcom.GetComponent<badcomment>().righttext == oncom.GetComponent<oncomment>().blocktext) {
							badcom.GetComponent<badcomment>().CorrectCommentObject = oncom;
							break;
						}
					}
				}

				foreach (GameObject baduncom in baduncoms) {
					foreach (GameObject uncom in uncoms) {
						if (uncom.GetComponent<uncom>().blocktext.Contains(baduncom.GetComponent<baduncom>().righttext)) {
							baduncom.GetComponent<baduncom>().CorrectCommentObject = uncom;
							break;
						}
					}
				}
			}
		}
	}

	//.................................>8.......................................
	//************************************************************************//
	// Method: public void SetTools(XmlNode levelnode)
	// Description: Read through levelnode XML and provision the tools for this level
	//************************************************************************//
	public void SetTools(XmlNode levelnode)	{

		for (int i = 0; i < totalNumberOfTools; i++) {
			toolIcons[i].GetComponent<GUITexture>().enabled = false;
		}
		foreach (XmlNode codenode in levelnode.ChildNodes) {
			// For each <tools> tag
			if (codenode.Name == stringLib.CODENODE_NAME_TOOLS) {
				// For each XML node contained within <tools>
				foreach (XmlNode toolnode in codenode.ChildNodes) {
					int toolnum = 0;
					switch(toolnode.Attributes["name"].Value)
					{
						case "catcher":
						case "activator":
							toolnum = stringLib.TOOL_CATCHER_OR_ACTIVATOR; break;
						case "printer":
						case "checker":
							toolnum = stringLib.TOOL_PRINTER_OR_CHECKER; break;
						case "warper":
						case "namer":
							toolnum = stringLib.TOOL_WARPER_OR_RENAMER; break;
						case "commenter": toolnum = stringLib.TOOL_COMMENTER; break;
						case "controlflow": toolnum = stringLib.TOOL_CONTROL_FLOW; break;
						default: break;
					}
					toolIcons[toolnum].GetComponent<GUITexture>().enabled = bool.Parse(toolnode.Attributes["enabled"].Value);
					if (toolnode.Attributes["count"].Value == "unlimited") {
						selectedtool.GetComponent<SelectedTool>().toolCounts[toolnum] = 999;
					}
					else {
						selectedtool.GetComponent<SelectedTool>().toolCounts[toolnum] = int.Parse(toolnode.Attributes["count"].Value);
					}
				}
			}
		}
	}

	//.................................>8.......................................
	//************************************************************************//
	// Method: public void ResetLevel(bool warp)
	// Description: Completely resets the level. Will destroy all game objects,
	// 				and reset task list and bug values to their default state.
	//				If warp is TRUE, then retain current tools. If FALSE, reset
	//				the tool count.
	//************************************************************************//
	public void ResetLevel(bool warp) {

		foreach (GameObject ln in lines) {
			Destroy(ln);
		}
		foreach (GameObject bc in beacons) {
			Destroy(bc);
		}
		foreach (GameObject oc in oncomments) {
			Destroy(oc);
		}
		foreach (GameObject oc in onchecks) {
			Destroy(oc);
		}
		foreach (GameObject oc in renamers) {
			Destroy(oc);
		}
		foreach (GameObject op in outputs) {
			Destroy(op);
		}
		foreach (GameObject op in badcomments) {
			Destroy(op);
		}
		foreach (GameObject wp in warps) {
			Destroy(wp);
		}
		foreach (GameObject cm in comments) {
			Destroy(cm);
		}
		foreach (GameObject wp in uncoms) {
			Destroy(wp);
		}
		foreach (GameObject cm in baduncoms) {
			Destroy(cm);
		}
		foreach (GameObject bp in breakpoints) {
			Destroy(bp);
		}
		foreach (GameObject pb in prizes) {
			Destroy(pb);
		}
		if (levelbug) {
			Destroy(levelbug);
		}
		for (int i = 0; i < 5; i++) {
			taskscompleted[i] = 0;
			tasklist[i] = 0;
		}
		sidebaroutput.GetComponent<GUIText>().text = "";
		lines = new List<GameObject>();
		outputs = new List<GameObject>();
		warps = new List<GameObject>();
		oncomments = new List<GameObject>();
		renamers = new List<GameObject>();
		badcomments = new List<GameObject>();
		onchecks = new List<GameObject>();
		uncoms = new List<GameObject>();
		baduncoms = new List<GameObject>();
		bugs = new List<GameObject>();
		comments = new List<GameObject>();
		breakpoints = new List<GameObject>();
		prizes = new List<GameObject>();

		if (!warp) {
			for (int i = 0; i < totalNumberOfTools; i++) {
				toolIcons[i].GetComponent<GUITexture>().enabled = false;
				toolIcons[i].GetComponent<GUITexture>().color = new Color(0.3f, 0.3f, 0.3f);
				selectedtool.GetComponent<SelectedTool>().toolCounts[i] = 0;
				selectedtool.GetComponent<SelectedTool>().projectilecode = 0;
			}
		}

		numberOfBugsRemaining = 0;
		this.transform.position += new Vector3(0,(linecount / 2) * linespacing, 0);
		this.transform.localScale -= new Vector3(0, levelLineRatio * linecount, 0);
		linecount = 0;
		hero.transform.position = leveltext.transform.position;
	}

	//.................................>8.......................................
	//************************************************************************//
	// Method: public void GameOver()
	// Description: Switch to the LEVEL_LOSE state.
	//************************************************************************//
	public void GameOver() {
		GUISwitch(false);
		menu.GetComponent<Menu>().gameon = false;
		gamestate = stateLib.GAMESTATE_LEVEL_LOSE;
	}

	//.................................>8.......................................
	//************************************************************************//
	// Method: public void Victory()
	// Description: Switch to the GAME_END state.
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

		// if (collidingObj.name.StartsWith("projectile")) {
		// RoboBUG will trigger a game loss (or hint)
		// if (gamemode == stringLib.GAME_MODE_BUG) {
		//	isLosing = true;
		//}
		// Otherwise, it's RobotON, so keep playing.
		// }
	}

	//.................................>8.......................................
	public string ColorizeKeywords(string sBlockText, bool overrideNewline = false) {

		/* Left this in - This will allow for the modification of found groups. Not needed right now but useful.
		Regex rgxsub;
		Match m = rgx.Match(sBlockText);
		while (m.Success)
		{
		rgxsub = new Regex("(\\W|^)" + "(" + m.Groups[1].Value + ")" + "(" + m.Groups[2].Value + ")" + "(\\W|$)");
		sBlockText = rgxsub.Replace(sBlockText, "$1" + stringLib.SYNTAX_COLOR_COMMENT + "$2" + stringLib.CLOSE_COLOR_TAG + stringLib.SYNTAX_COLOR_COMMENT + m.Groups[2].Value + stringLib.CLOSE_COLOR_TAG + "$4");
		m = m.NextMatch();
	}
	*/

	if (sBlockText == "") return sBlockText;

	// Turn all comments and their following text green. Remove all color tags from following text.
	Regex rgxNewlineSplit = new Regex("\n");
	Regex rgxComment = new Regex("(//|\\s#|\n#|#)(.*)");
	Regex rgxKeyword = new Regex("(\\W|^)(else if|class|print|not|or|and|def|include|bool|auto|double|int|struct|break|else|long|switch|case|enum|register|typedef|char|extern|return|union|continue|for|signed|void|do|if|static|while|default|goto|sizeof|volatile|const|float|short|unsigned)(\\W|$)");
	Match m1, m2;
	string[] saStringBuilder = rgxNewlineSplit.Split(sBlockText);
	string sReturnString = "";
	int count = 0;
	foreach (string substring in saStringBuilder)
	{
		// print("substring" + count + ": " + substring);
		m1 = rgxComment.Match(substring);
		m2 = rgxKeyword.Match(substring);
		if (m1.Success) {
			sReturnString += rgxComment.Replace(substring, stringLib.SYNTAX_COLOR_COMMENT + "$1$2" + stringLib.CLOSE_COLOR_TAG);
		}
		else if (m2.Success) {
			sReturnString += rgxKeyword.Replace(substring, "$1" + stringLib.SYNTAX_COLOR_KEYWORD + "$2" + stringLib.CLOSE_COLOR_TAG + "$3");
		}
		else {
			sReturnString += substring;

		}
		if (substring != "" && substring.Trim().Length > 0 && substring.Substring(substring.Length-1, 1) != " " && !overrideNewline) {
			sReturnString += "\n";
		}
	}
	return sReturnString;
}

//.................................>8.......................................
public string DecolorizeKeywords(string sBlockText) {
	Regex rgx = new Regex("(.*)(<color=#.{8}>)(.*)(</color>)(.*)");
	sBlockText = rgx.Replace(sBlockText, "$1$3$5");
	return sBlockText;
}

//.................................>8.......................................
}

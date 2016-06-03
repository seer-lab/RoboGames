//****************************************************************************//
// Class Name: LevelGenerator
// Class Description:
// Methods:
// 		private void Start()
//		private void Update()
//		public void GUISwitch(bool gui_on)
//		public void BuildLevel(string filename, bool warp, string linenum = "")
//		private void WriteCode(XmlNode levelnode)
//		private void PlaceObjects(XmlNode levelnode)
//		public void SetTools(XmlNode levelnode)
//		public void ResetLevel(bool warp)
//		public void GameOver()
//		public void Victory()
//		private void OnTriggerEnter2D(Collider2D c)
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
	public bool losing;
	public float endTime = 0f;
	public float remainingtime = 0f;
	public int num_of_bugs = 0;
	public int gamestate;
	public int[] tasklist = new int[5];
	public int[] taskscompleted = new int[5];
	public int linecount = 0;
	public string nextlevel = "";
	public string currentlevel = "level0.xml";
	public string gamemode;
	public List<GameObject> beacons;
	public GameObject leveltext;
	public AudioClip[] sounds = new AudioClip[10];
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
	public GameObject selectedtool;
	public GameObject sidebarpanel;
	public GameObject outputpanel;
	public GameObject cinematic;
	public GameObject menu;
	// was 6, now number of tools
	public GameObject[] toolIcons = new GameObject[stateLib.NUMBER_OF_TOOLS];
	public GameObject sidebartimer;

	private bool alarmed;
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
	private float startNextLevel = 0f;
	private float startTime = 0f;
	private int numOfTools = stateLib.NUMBER_OF_TOOLS;
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
		losing 		= false;
		gamestate 	= stateLib.GAMESTATE_MENU;
		for (int i = 0; i < 5; i++) {
			tasklist[i] = 0;
			taskscompleted[i] = 0;
		}
		GUISwitch(false);
		alarmed = false;
		winning = false;
	}


	//.................................>8.......................................
	// Update is called once per frame
	private void Update() {
		if (gamestate == stateLib.GAMESTATE_IN_GAME) {
			if (endTime - Time.time < 30) {
				sidebartimer.GetComponent<GUIText>().text = "Time remaining: <size=50><color=red>" +((int)(endTime - Time.time)).ToString() + "</color></size> seconds";
				if (!alarmed) {
					alarmed = true;
					sidebartimer.GetComponent<AudioSource>().Play();
				}
			}
			else {
				// @TODO: Convert to m:s
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
				alarmed = false;
			}
			if (gamemode == stringLib.GAME_MODE_ON) {
				winning = true;
				for (int i = 0; i < 5; i++) {
					if (tasklist[i] != taskscompleted[i]) {
						winning = false;
					}
				}
			}
			if (losing) {
				if (losstime == 0) {
					GetComponent<AudioSource>().clip = sounds[0];
					GetComponent<AudioSource>().Play();
					losstime = Time.time + lossdelay;
				}
				else if (losstime < Time.time) {
					losstime = 0;
					losing = false;
					GameOver();
				}
			}
			if (num_of_bugs <= 0 && bugs.Count > 0 || winning) {
				if (startNextLevel == 0f) {
					startNextLevel = Time.time + leveldelay;
				}
				else if (Time.time > startNextLevel) {
					winning = false;
					startNextLevel = 0f;
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
			if (endTime < Time.time &&(num_of_bugs > 0 || bugs.Count == 0)) {
				GameOver();
			}
			if (Input.GetKeyDown(KeyCode.Escape)) {
				gamestate = stateLib.GAMESTATE_MENU;
				GUISwitch(false);
			}
		}
		else {
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
			endTime = remainingtime + Time.time;
		}
		else {
			sidebarpanel.GetComponent<GUITexture>().enabled = false;
			outputpanel.GetComponent<GUITexture>().enabled = false;
			sidebartimer.GetComponent<GUIText>().text = "";
			remainingtime = endTime - Time.time;
		}
	}

	//.................................>8.......................................
	//************************************************************************//
	// Method: public void BuildLevel(string filename, bool warp, string linenum = "")
	// Description: Driver for level creation
	//************************************************************************//
	public void BuildLevel(string filename, bool warp, string linenum = "")	{
		ResetLevel(warp);
		XmlDocument doc = new XmlDocument();
		doc.Load(filename);
		XmlNode levelnode = doc.FirstChild;
		WriteCode(levelnode);
		PlaceObjects(levelnode);
		if (!warp) {
			SetTools(levelnode);
			currentlevel = filename.Substring(filename.IndexOf("\\") + 1);
			startTime = Time.time;
			foreach (XmlNode node in levelnode.ChildNodes) {
				if (node.Name == stringLib.NODE_NAME_TIME) {
					endTime =(float)int.Parse(node.InnerText) + startTime;
					remainingtime =(float)int.Parse(node.InnerText);
				}
				else if (node.Name == stringLib.NODE_NAME_NEXT_LEVEL) {
					nextlevel = gamemode + @"leveldata\" + node.InnerText;
				}
				else if (node.Name == stringLib.NODE_NAME_INTRO_TEXT) {
					cinematic.GetComponent<Cinematic>().introtext = node.InnerText;
				}
				else if (node.Name == stringLib.NAME_NAME_END_TEXT) {
					cinematic.GetComponent<Cinematic>().endtext = node.InnerText;
				}
			}
			selectedtool.GetComponent<SelectedTool>().NextTool();

		}
		else if (linenum != "") {
			hero.transform.position = new Vector3(-7, initialLineY -(int.Parse(linenum) - 1) * linespacing, 1);
		}
		this.transform.position -= new Vector3(0,(linecount / 2) * linespacing, 0);
		this.transform.localScale += new Vector3(0, levelLineRatio * linecount, 0);
		if (warp) {
			GetComponent<AudioSource>().clip = sounds[1];
			GetComponent<AudioSource>().Play();
		}
	}

	//.................................>8.......................................
	//************************************************************************//
	// Method: private void WriteCode(XmlNode levelnode)
	// Description: Read through levelnode XML and write the lines
	//************************************************************************//
	private void WriteCode(XmlNode levelnode) {
		destext.GetComponent<TextMesh>().text = "";
		foreach (XmlNode codenode in levelnode.ChildNodes) {
			// Create lines of code for the level
			if (codenode.Name == stringLib.CODENODE_NAME_CODE) {
				foreach (XmlNode printnode in codenode.ChildNodes) {
					if (printnode.Name == stringLib.NODE_NAME_PRINT) {
						printnode.InnerText = stringLib.NODE_COLOR_PRINT +
											  printnode.InnerText +
											  stringLib.CLOSE_COLOR_TAG;
					}
					if (printnode.Name == stringLib.NODE_NAME_WARP) {
						printnode.InnerText = stringLib.NODE_COLOR_WARP +
											  printnode.InnerText +
											  stringLib.CLOSE_COLOR_TAG;
					}
					if (printnode.Name == stringLib.NODE_NAME_RENAME) {
						printnode.InnerText = stringLib.NODE_COLOR_RENAME +
											  printnode.InnerText +
											  stringLib.CLOSE_COLOR_TAG;
					}
					if (printnode.Name == stringLib.NODE_NAME_ON_CHECK) {
						printnode.InnerText = stringLib.NODE_COLOR_ON_CHECK +
											  printnode.InnerText +
											  stringLib.CLOSE_COLOR_TAG;
					}
					if (printnode.Name == stringLib.NODE_NAME_UNCOMMENT) {
						printnode.InnerText = stringLib.NODE_COLOR_UNCOMMENT +
											  printnode.InnerText +
											  stringLib.CLOSE_COLOR_TAG;
					}
					if (printnode.Name == stringLib.NODE_NAME_BAD_UNCOMMENT) {
						printnode.InnerText = stringLib.NODE_COLOR_BAD_UNCOMMENT +
											  printnode.InnerText +
											  stringLib.CLOSE_COLOR_TAG;
					}
					if (printnode.Name == stringLib.NODE_NAME_ON_COMMENT) {
						printnode.InnerText = stringLib.NODE_COLOR_ON_COMMENT +
											  stringLib.COMMENT_CLOSE_COLOR_TAG +
											  printnode.InnerText;
					}
					if (printnode.Name == stringLib.NODE_NAME_BAD_COMMENT) {
						printnode.InnerText = stringLib.NODE_COLOR_BAD_COMMENT +
										      stringLib.COMMENT_CLOSE_COLOR_TAG +
											  	printnode.InnerText;
					}
					if (printnode.Name == stringLib.NODE_NAME_COMMENT) {
						printnode.InnerText = stringLib.NODE_COLOR_COMMENT +
											  stringLib.COMMENT_CLOSE_COLOR_TAG +
											  printnode.InnerText +
											  stringLib.NODE_COLOR_COMMENT +
											  stringLib.COMMENT_CLOSE_COLOR_TAG;
					}
				}

				// Count the number of lines in this level, store it in linecount
				codetext = codenode.InnerText;
				foreach (char c in codetext) {
					if (c == '\n')
					linecount++;
				}

				// Syntax highlighting
				Regex rgx = new Regex("(//|\\s#|\n#)(.*)");

				codetext = rgx.Replace(codetext, stringLib.SYNTAX_COLOR + "$1$2" + stringLib.CLOSE_COLOR_TAG);

				rgx = new Regex("(\\W|^)(else if|class|print|not|or|and|def|include|bool|auto|double|int|struct|break|else|long|switch|case|enum|register|typedef|char|extern|return|union|continue|for|signed|void|do|if|static|while|default|goto|sizeof|volatile|const|float|short|unsigned)(\\W|$)");
				codetext = rgx.Replace(codetext, "$1<color=#00ffffff>$2</color>$3");
				rgx = new Regex("(//)(.*)(<color=#00ffffff>)(.*)(</color>)(.*)(</color>)");
				codetext = rgx.Replace(codetext, "$1$2$4$6$7");
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
				XmlValidatingReader reader = new XmlValidatingReader(codenode.InnerXml, XmlNodeType.Element, context);

				IXmlLineInfo lineInfo =((IXmlLineInfo)reader);
				while(reader.Read()) {
					if (reader.NodeType == XmlNodeType.Element && reader.Name == stringLib.NODE_NAME_PRINT) {
						GameObject newoutput =(GameObject)Instantiate(printobject, new Vector3(-7, initialLineY -(lineInfo.LineNumber - 1) * linespacing, 1), transform.rotation);
						outputs.Add(newoutput);
						printer printcode = newoutput.GetComponent<printer>();
						printcode.displaytext = reader.GetAttribute("text");
						printcode.sidebar = sidebaroutput;
						printcode.selectTools = selectedtool;
						if (reader.GetAttribute("tool") != null) {
							string toolatt = reader.GetAttribute("tool");
							string[] toolcounts = toolatt.Split(',');
							for (int i = 0; i < stateLib.NUMBER_OF_TOOLS; i++) {
								printcode.tools[i] = int.Parse(toolcounts[i]);
							}
						}
					}
					else if (reader.NodeType == XmlNodeType.Element && reader.Name == stringLib.NODE_NAME_WARP) {
						GameObject newwarp =(GameObject)Instantiate(warpobject, new Vector3(-7, initialLineY -(lineInfo.LineNumber - 1) * linespacing, 1), transform.rotation);
						warps.Add(newwarp);
						warper warpcode = newwarp.GetComponent<warper>();
						warpcode.CodeScreen = this.gameObject;
						warpcode.filename = reader.GetAttribute("file");
						warpcode.selectTools = selectedtool;
						if (reader.GetAttribute("tool") != null) {
							string toolatt = reader.GetAttribute("tool");
							string[] toolcounts = toolatt.Split(',');
							for (int i = 0; i < stateLib.NUMBER_OF_TOOLS; i++) {
								warpcode.tools[i] = int.Parse(toolcounts[i]);
							}
						}
						if (reader.GetAttribute("line") != null) {
							warpcode.linenum = reader.GetAttribute("line");
						}
					}
					else if (reader.NodeType == XmlNodeType.Element && reader.Name == stringLib.NODE_NAME_BUG) {
						bugsize = int.Parse(reader.GetAttribute("size"));
						int row = 0;
						if (reader.GetAttribute("row") != null) {
							row = int.Parse(reader.GetAttribute("row"));
						}
						int col = 0;
						if (reader.GetAttribute("col") != null) {
							col = int.Parse(reader.GetAttribute("col"));
						}
						levelbug =(GameObject)Instantiate(bugobject, new Vector3(bugXshift + col * fontwidth +(bugsize - 1) * levelLineRatio, initialLineY -(lineInfo.LineNumber + row - 1 + 0.5f *(bugsize - 1)) * linespacing + 0.4f, 0f), transform.rotation);
						levelbug.transform.localScale += new Vector3(bugscale *(bugsize - 1), bugscale *(bugsize - 1), 0);
						levelbug.GetComponent<GenericBug>().codescreen = this.gameObject;
						bugs.Add(levelbug);
						num_of_bugs++;

					}
					else if (reader.NodeType == XmlNodeType.Element && reader.Name == stringLib.NODE_NAME_COMMENT) {
						int commentsize = int.Parse(reader.GetAttribute("size"));
						GameObject newcomment =(GameObject)Instantiate(commentobject, new Vector3(-7, initialLineY -(lineInfo.LineNumber - 1 + 0.9f *(commentsize - 1)) * linespacing, 0f), transform.rotation);
						comments.Add(newcomment);
						commentBlock commentcode = newcomment.GetComponent<commentBlock>();
						commentcode.code = leveltext;
						commentcode.errmsg = reader.GetAttribute("text");
						commentcode.sideoutput = sidebaroutput;
						commentcode.oldtext = codetext;
						newcomment.transform.localScale += new Vector3(0, textscale *(commentsize - 1), 0);
						commentcode.selectTools = selectedtool;
						if (reader.GetAttribute("tool") != null) {
							string toolatt = reader.GetAttribute("tool");
							string[] toolcounts = toolatt.Split(',');
							for (int i = 0; i<6; i++) {
								commentcode.tools[i] = int.Parse(toolcounts[i]);
							}
						}
					}
					else if (reader.NodeType == XmlNodeType.Element && reader.Name == stringLib.NODE_NAME_ON_CHECK) {
						GameObject newcheck =(GameObject)Instantiate(oncheckobject, new Vector3(-7, initialLineY -(lineInfo.LineNumber - 1) * linespacing, 1), transform.rotation);
						oncomments.Add(newcheck);
						checker oncheckcode = newcheck.GetComponent<checker>();
						oncheckcode.displaytext = reader.GetAttribute("text");
						oncheckcode.expected = reader.GetAttribute("answer");
						oncheckcode.codescreen = this.gameObject;
						oncheckcode.sidebar = sidebaroutput;
						oncheckcode.code = leveltext;
						oncheckcode.innertext = reader.ReadInnerXml(); //danger will robinson
						tasklist[1]++;
					}
					else if (reader.NodeType == XmlNodeType.Element && reader.Name == stringLib.NODE_NAME_RENAME) {
						GameObject newrename =(GameObject)Instantiate(renameobject, new Vector3(-7, initialLineY -(lineInfo.LineNumber - 1) * linespacing, 1), transform.rotation);
						renamers.Add(newrename);
						rename renamecode = newrename.GetComponent<rename>();
						renamecode.displaytext = reader.GetAttribute("text");
						renamecode.correct = int.Parse(reader.GetAttribute("correct"));
						renamecode.codescreen = this.gameObject;
						renamecode.sidebar = sidebaroutput;
						renamecode.code = leveltext;

						string names = reader.GetAttribute("names");
						string[] namelist = names.Split(',');
						for (int i = 0; i<namelist.Length; i++) {
							renamecode.names.Add(namelist[i]);
						}
						renamecode.innertext = reader.ReadInnerXml();

						tasklist[2]++;
					}
					else if (reader.NodeType == XmlNodeType.Element && reader.Name == stringLib.NODE_NAME_ON_COMMENT) {
						int commentsize = int.Parse(reader.GetAttribute("size"));
						GameObject newcomment =(GameObject)Instantiate(oncommentobject, new Vector3(-7, initialLineY -(lineInfo.LineNumber - 1 + 0.9f *(commentsize - 1)) * linespacing, 0f), transform.rotation);
						oncomments.Add(newcomment);
						oncomment oncommentcode = newcomment.GetComponent<oncomment>();
						oncommentcode.code = leveltext;
						oncommentcode.oldtext = codetext;
						oncommentcode.codescreen = this.gameObject;
						newcomment.transform.localScale += new Vector3(0, textscale *(commentsize - 1), 0);
						tasklist[3]++;
					}
					else if (reader.NodeType == XmlNodeType.Element && reader.Name == stringLib.NODE_NAME_BAD_COMMENT) {
						int commentsize = int.Parse(reader.GetAttribute("size"));
						GameObject newbadcom =(GameObject)Instantiate(badcommentobject, new Vector3(-7, initialLineY -(lineInfo.LineNumber - 1 + 0.9f *(commentsize - 1)) * linespacing, 0f), transform.rotation);
						badcomments.Add(newbadcom);
						badcomment badcomcode = newbadcom.GetComponent<badcomment>();
						badcomcode.code = leveltext;
						badcomcode.righttext = reader.GetAttribute("righttext");
						badcomcode.oldtext = codetext;
						badcomcode.codescreen = this.gameObject;
						newbadcom.transform.localScale += new Vector3(0, textscale *(commentsize - 1), 0);
					}
					else if (reader.NodeType == XmlNodeType.Element && reader.Name == stringLib.NODE_NAME_UNCOMMENT) {
						int commentsize = int.Parse(reader.GetAttribute("size"));
						GameObject newuncom =(GameObject)Instantiate(uncomobject, new Vector3(-7, initialLineY -(lineInfo.LineNumber - 1 + 0.9f *(commentsize - 1)) * linespacing, 0f), transform.rotation);
						uncoms.Add(newuncom);
						uncom uncomcode = newuncom.GetComponent<uncom>();
						uncomcode.code = leveltext;
						uncomcode.oldtext = codetext;
						uncomcode.codescreen = this.gameObject;
						newuncom.transform.localScale += new Vector3(0, textscale *(commentsize - 1), 0);
						tasklist[4]++;
					}
					else if (reader.NodeType == XmlNodeType.Element && reader.Name == stringLib.NODE_NAME_BAD_UNCOMMENT) {
						int commentsize = int.Parse(reader.GetAttribute("size"));
						GameObject newbaduncom =(GameObject)Instantiate(baduncomobject, new Vector3(-7, initialLineY -(lineInfo.LineNumber - 1 + 0.9f *(commentsize - 1)) * linespacing, 0f), transform.rotation);
						baduncoms.Add(newbaduncom);
						baduncom baduncomcode = newbaduncom.GetComponent<baduncom>();
						baduncomcode.code = leveltext;
						baduncomcode.righttext = reader.GetAttribute("righttext");
						baduncomcode.oldtext = codetext;
						baduncomcode.codescreen = this.gameObject;
						newbaduncom.transform.localScale += new Vector3(0, textscale *(commentsize - 1), 0);
					}
					else if (reader.NodeType == XmlNodeType.Element && reader.Name == stringLib.NODE_NAME_BREAKPOINT) {
						GameObject newbreakpoint =(GameObject)Instantiate(breakpointobject, new Vector3(-10, initialLineY -(lineInfo.LineNumber - 1) * linespacing + 0.4f, 1), transform.rotation);
						breakpoints.Add(newbreakpoint);
						Breakpoint breakcode = newbreakpoint.GetComponent<Breakpoint>();
						breakcode.sidebaroutput = sidebaroutput;
						breakcode.values = reader.GetAttribute("text");
						breakcode.selectTools = selectedtool;
						if (reader.GetAttribute("tool") != null) {
							string toolatt = reader.GetAttribute("tool");
							string[] toolcounts = toolatt.Split(',');
							for (int i = 0; i<6; i++) {
								breakcode.tools[i] = int.Parse(toolcounts[i]);
							}
						}
					}
					else if (reader.NodeType == XmlNodeType.Element && reader.Name == stringLib.NODE_NAME_PRIZE) {
						bugsize = int.Parse(reader.GetAttribute("size"));
						GameObject prizebug =(GameObject)Instantiate(prizeobject, new Vector3(-9f +(bugsize - 1) * levelLineRatio, initialLineY -(lineInfo.LineNumber - 1 + 0.5f *(bugsize - 1)) * linespacing + 0.4f, 0f), transform.rotation);
						prizebug.transform.localScale += new Vector3(bugscale *(bugsize - 1), bugscale *(bugsize - 1), 0);
						prizebug.GetComponent<PrizeBug>().tools = selectedtool;
						string[] bonuses = reader.GetAttribute("bonuses").Split(',');
						for (int i = 0; i<6; i++) {
							prizebug.GetComponent<PrizeBug>().bonus[i] += int.Parse(bonuses[i]);
						}
						prizes.Add(prizebug);
					}
					else if (reader.NodeType == XmlNodeType.Element && reader.Name == stringLib.NODE_NAME_BEACON) {
						GameObject newbeacon =(GameObject)Instantiate(beaconobject, new Vector3(-10, initialLineY -(lineInfo.LineNumber - 1) * linespacing + 0.4f, 1), transform.rotation);
						newbeacon.GetComponent<beacon>().codescreen = this.gameObject;
						if (reader.GetAttribute("actnums") != "") {
							string[] actnums = reader.GetAttribute("actnums").Split(',');

							for (int i = 0; i<actnums.Length; i++) {
								newbeacon.GetComponent<beacon>().actnumbers.Add(int.Parse(actnums[i]));
								tasklist[0]++;
							}
						}
						beacons.Add(newbeacon);
					}
				}
				reader.Close();
				int j = 0;
				int k = 0;
				int l = 0;
				int m = 0;
				int n = 0;
				for (int i=0; i<codenode.ChildNodes.Count; i++) {
					if (codenode.ChildNodes[i].Name == stringLib.NODE_NAME_COMMENT) {
						comments[j].GetComponent<commentBlock>().blocktext = codenode.ChildNodes[i].InnerText.Trim();
						j++;
					}
					if (codenode.ChildNodes[i].Name == stringLib.NODE_NAME_ON_COMMENT) {
						oncomments[k].GetComponent<oncomment>().blocktext = codenode.ChildNodes[i].InnerText.Substring(29).Trim();
						k++;
					}
					if (codenode.ChildNodes[i].Name == stringLib.NODE_NAME_BAD_COMMENT) {
						badcomments[l].GetComponent<badcomment>().blocktext = codenode.ChildNodes[i].InnerText.Substring(29).Trim();
						l++;
					}
					if (codenode.ChildNodes[i].Name == stringLib.NODE_NAME_UNCOMMENT) {
						uncoms[m].GetComponent<uncom>().blocktext = codenode.ChildNodes[i].InnerText;
						m++;
					}
					if (codenode.ChildNodes[i].Name == stringLib.NODE_NAME_BAD_UNCOMMENT) {
						baduncoms[n].GetComponent<baduncom>().blocktext = codenode.ChildNodes[i].InnerText;
						n++;
					}
				}
				foreach (GameObject badcom in badcomments) {
					foreach (GameObject oncom in oncomments) {
						if (badcom.GetComponent<badcomment>().righttext == oncom.GetComponent<oncomment>().blocktext) {
							badcom.GetComponent<badcomment>().rightcomment = oncom;
							break;
						}
					}
				}

				foreach (GameObject baduncom in baduncoms) {
					foreach (GameObject uncom in uncoms) {
						if (uncom.GetComponent<uncom>().blocktext.Contains(baduncom.GetComponent<baduncom>().righttext)) {
							baduncom.GetComponent<baduncom>().rightcomment = uncom;
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

		for (int i = 0; i < numOfTools; i++) {
			toolIcons[i].GetComponent<GUITexture>().enabled = false;
		}
		foreach (XmlNode codenode in levelnode.ChildNodes) {
			if (codenode.Name == stringLib.CODENODE_NAME_TOOLS) {
				foreach (XmlNode toolnode in codenode.ChildNodes) {
					int toolnum = int.Parse(toolnode.InnerText);
					toolIcons[toolnum].GetComponent<GUITexture>().enabled = true;
					selectedtool.GetComponent<SelectedTool>().toolCounts[toolnum] = int.Parse(toolnode.Attributes["count"].Value);
				}
			}
		}
	}

	//.................................>8.......................................
	//************************************************************************//
	// Method: public void ResetLevel(bool warp)
	// Description: Completely resets the level. Will destroy all game objects,
	// 				and reset appropriate values to their default state.
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
			for (int i = 0; i < numOfTools; i++) {
				toolIcons[i].GetComponent<GUITexture>().enabled = false;
				toolIcons[i].GetComponent<GUITexture>().color = new Color(0.3f, 0.3f, 0.3f);
				selectedtool.GetComponent<SelectedTool>().toolCounts[i] = 0;
				selectedtool.GetComponent<SelectedTool>().projectilecode = 0;
			}
		}

		num_of_bugs = 0;
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
	// Description: Switch to the LEVEL_LOSE state.
	//************************************************************************//
	private void OnTriggerEnter2D(Collider2D c)	{

		if (c.name.StartsWith("projectile")) {
			// RoboBUG will trigger a game loss (or hint)
			if (gamemode == stringLib.GAME_MODE_BUG) {
				losing = true;
			}
			// Otherwise, it's RobotON, so keep playing.
		}
	}

	//.................................>8.......................................
}

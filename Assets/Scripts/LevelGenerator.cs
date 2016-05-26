using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Text;
using System.Xml;
using System.Text.RegularExpressions;

public class LevelGenerator : MonoBehaviour
{

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
	public GameObject[] toolIcons = new GameObject[6];
	string codetext;
	public GameObject sidebartimer;
	public int linecount = 0;
	float initialLineY = 3f;
	float initialLineX = -4.5f;
	float linespacing = 0.825f;
	float levelLineRatio = 0.55f;
	float bugXshift = -9.5f;
	float fontwidth = 0.15f;
	float bugsize = 1f;
	float bugscale = 1.5f;
	float textscale = 1.75f;
	GameObject levelbug;
	float leveldelay = 2f;
	float startNextLevel = 0f;
	int numOfTools = 6;
	float startTime = 0f;
	public float endTime = 0f;
	public float remainingtime = 0f;
	public int num_of_bugs = 0;
	public int[] tasklist = new int[5];
	public int[] taskscompleted = new int[5];
	public string nextlevel = "";
	public string currentlevel = "level0.xml";
	List<GameObject> lines;
	List<GameObject> outputs;
	List<GameObject> warps;
	List<GameObject> bugs;
	public List<GameObject> beacons;
	List<GameObject> comments;
	List<GameObject> renamers;
	List<GameObject> oncomments;
	List<GameObject> badcomments;
	List<GameObject> uncoms;
	List<GameObject> baduncoms;
	List<GameObject> onchecks;
	List<GameObject> breakpoints;
	List<GameObject> prizes;
	public int gamestate;
	public string gamemode;
	bool alarmed;
	bool winning;
	public bool losing;
	float losstime;
	float lossdelay = 3f;
	/*gamestates:
	 * <0-SubMenus
		0-Menu
		1-Game
		2-LevelStart
		3-LevelFinish
		4-LevelLose
		5-Unused
		6-Unused
		7-Unused
		8-Unused
		9-Unused
		10-InitialComic
		11-StageComic
		12-GameEnd
	*/

	// Use this for initialization
	void Start ()
	{
		gamemode = "bug";
		losstime = 0;
		lines = new List<GameObject> ();
		outputs = new List<GameObject> ();
		warps = new List<GameObject> ();
		bugs = new List<GameObject> ();
		comments = new List<GameObject> ();
		breakpoints = new List<GameObject> ();
		beacons = new List<GameObject> ();
		badcomments = new List<GameObject> ();
		uncoms = new List<GameObject> ();
		baduncoms = new List<GameObject> ();
		renamers = new List<GameObject> ();
		oncomments = new List<GameObject> ();
		onchecks = new List<GameObject> ();
		prizes = new List<GameObject> ();
		losing = false;
		gamestate = 0;
		for (int i = 0; i<5; i++) {
			tasklist [i] = 0;
			taskscompleted [i] = 0;
		}
		GUISwitch (false);
		alarmed = false;
		winning = false;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (gamestate == 1) {
			if (endTime - Time.time < 30) {
				sidebartimer.GetComponent<GUIText> ().text = "Time remaining: <size=50><color=red>" + ((int)(endTime - Time.time)).ToString () + "</color></size> seconds";
				if (!alarmed) {
					alarmed = true;
					sidebartimer.GetComponent<AudioSource> ().Play ();
				}
			} else {
				sidebartimer.GetComponent<GUIText> ().text = "Time remaining: " + ((int)(endTime - Time.time)).ToString () + " seconds";
				alarmed = false;
			}
			if (gamemode == "on") {
				winning = true;
				for (int i = 0; i<5; i++) {
					if (tasklist [i] != taskscompleted [i]) {
						winning = false;
					}
				}
			}
			if (losing){
				if (losstime == 0){
					GetComponent<AudioSource>().clip = sounds[0];
					GetComponent<AudioSource> ().Play ();
					losstime = Time.time + lossdelay;
				} else if (losstime < Time.time){
					losstime = 0;
					losing = false;
					GameOver();
				}
			}
			if (num_of_bugs <= 0 && bugs.Count > 0 || winning) {						
				if (startNextLevel == 0f) {
					startNextLevel = Time.time + leveldelay;
				} else if (Time.time > startNextLevel) {
					winning = false;
					startNextLevel = 0f;
					if (nextlevel != gamemode + @"leveldata\") {
						foreach (GameObject bug in bugs) {
							Destroy (bug);
						}
						GUISwitch (false);
						menu.GetComponent<Menu> ().saveGame (currentlevel);
						gamestate = 3;
					} else {
						Victory ();
					}
				}
						
			}
			if (endTime < Time.time && (num_of_bugs > 0 || bugs.Count == 0)) {
				GameOver ();
			}
			if (Input.GetKeyDown (KeyCode.Escape)) {
				gamestate = 0;
				GUISwitch (false);
			}
		} else {
		}
	}

	public void GUISwitch (bool gui_on)
	{
		if (gui_on) {
			sidebarpanel.GetComponent<GUITexture> ().enabled = true;
			outputpanel.GetComponent<GUITexture> ().enabled = true;
			endTime = remainingtime + Time.time;
		} else {
			sidebarpanel.GetComponent<GUITexture> ().enabled = false;
			outputpanel.GetComponent<GUITexture> ().enabled = false;
			sidebartimer.GetComponent<GUIText> ().text = "";
			remainingtime = endTime - Time.time;
		}
	}

	public void BuildLevel (string filename, bool warp, string linenum="")
	{
		ResetLevel (warp);
		XmlDocument doc = new XmlDocument ();
		doc.Load (filename);
		XmlNode levelnode = doc.FirstChild;
		WriteCode (levelnode);
		PlaceObjects (levelnode);
		if (!warp) {
			SetTools (levelnode);
			currentlevel = filename.Substring (filename.IndexOf ("\\") + 1);
			startTime = Time.time;
			foreach (XmlNode node in levelnode.ChildNodes) {
				if (node.Name == "time") {
					endTime = (float)int.Parse (node.InnerText) + startTime;
					remainingtime = (float)int.Parse (node.InnerText);
				} else if (node.Name == "nextlevel") {
					nextlevel = gamemode + @"leveldata\" + node.InnerText;
				} else if (node.Name == "introtext") {
					cinematic.GetComponent<Cinematic> ().introtext = node.InnerText;
				} else if (node.Name == "endtext") {
					cinematic.GetComponent<Cinematic> ().endtext = node.InnerText;
				}
			}
			selectedtool.GetComponent<SelectedTool> ().NextTool ();

		} else if (linenum != "") {
			hero.transform.position = new Vector3 (-7, initialLineY - (int.Parse (linenum) - 1) * linespacing, 1);
		}
		this.transform.position -= new Vector3 (0, (linecount / 2) * linespacing, 0);
		this.transform.localScale += new Vector3 (0, levelLineRatio * linecount, 0);
		if (warp) {
			GetComponent<AudioSource>().clip = sounds[1];
			GetComponent<AudioSource> ().Play ();
		}
	}

	void WriteCode (XmlNode levelnode)
	{
		destext.GetComponent<TextMesh> ().text = "";
		foreach (XmlNode codenode in levelnode.ChildNodes) {
			if (codenode.Name == "code") {
				foreach (XmlNode printnode in codenode.ChildNodes) {
					if (printnode.Name == "print") {
						printnode.InnerText = "<color=#ffff00ff>" + printnode.InnerText + "</color>";
					}
					if (printnode.Name == "warp") {
						printnode.InnerText = "<color=#ff00ffff>" + printnode.InnerText + "</color>";
					}
					if (printnode.Name == "rename") {
						printnode.InnerText = "<color=#ff00ffff>" + printnode.InnerText + "</color>";
					}
					if (printnode.Name == "oncheck") {
						printnode.InnerText = "<color=#ffff00ff>" + printnode.InnerText + "</color>";
					}
					if (printnode.Name == "uncom") {
						printnode.InnerText = "<color=#ff0000ff>/*" + printnode.InnerText + "*/</color>";
					}
					if (printnode.Name == "baduncom") {
						printnode.InnerText = "<color=#ff0000ff>/*" + printnode.InnerText + "*/</color>";
					}
					if (printnode.Name == "oncomment") {
						printnode.InnerText = "<color=#00ff00ff>/**/</color>" + printnode.InnerText;
					}
					if (printnode.Name == "badcomment") {
						printnode.InnerText = "<color=#00ff00ff>/**/</color>" + printnode.InnerText;
					}
					if (printnode.Name == "comment") {
						printnode.InnerXml = "&lt;color=#00ff00ff&gt;/**/&lt;/color&gt;" + printnode.InnerXml + "&lt;color=#00ff00ff&gt;/**/&lt;/color&gt;\n\n";
					}
				}
				codetext = codenode.InnerText;
				foreach (char c in codetext) {
					if (c == '\n')
						linecount++;
				}
				Regex rgx = new Regex ("(//|\\s#|\n#)(.*)");

				codetext = rgx.Replace (codetext, "<color=#00ff00ff>$1$2</color>");

				rgx = new Regex ("(\\W|^)(else if|class|print|not|or|and|def|include|bool|auto|double|int|struct|break|else|long|switch|case|enum|register|typedef|char|extern|return|union|continue|for|signed|void|do|if|static|while|default|goto|sizeof|volatile|const|float|short|unsigned)(\\W|$)");
				codetext = rgx.Replace (codetext, "$1<color=#00ffffff>$2</color>$3");
				rgx = new Regex ("(//)(.*)(<color=#00ffffff>)(.*)(</color>)(.*)(</color>)");
				codetext = rgx.Replace (codetext, "$1$2$4$6$7");
			} else if (codenode.Name == "description") {
				destext.GetComponent<TextMesh> ().text = codenode.InnerText;
			}
		}
		for (int i = 0; i<linecount; i++) {
			GameObject newline = (GameObject)Instantiate (lineobject, new Vector3 (initialLineX, initialLineY - i * linespacing, 1), transform.rotation);
			lines.Add (newline);
		}
		leveltext.GetComponent<TextMesh> ().text = codetext;
	}

	void PlaceObjects (XmlNode levelnode)
	{
		foreach (XmlNode codenode in levelnode.ChildNodes) {
			if (codenode.Name == "code") {
				// Create the XmlNamespaceManager.
				XmlNamespaceManager nsmgr = new XmlNamespaceManager (new NameTable ());
				
				// Create the XmlParserContext.
				XmlParserContext context = new XmlParserContext (null, nsmgr, null, XmlSpace.None);
				
				// Create the reader.
				XmlValidatingReader reader = new XmlValidatingReader (codenode.InnerXml, XmlNodeType.Element, context);
				
				IXmlLineInfo lineInfo = ((IXmlLineInfo)reader);
				while (reader.Read()) {

					if (reader.NodeType == XmlNodeType.Element && reader.Name == "print") {
						GameObject newoutput = (GameObject)Instantiate (printobject, new Vector3 (-7, initialLineY - (lineInfo.LineNumber - 1) * linespacing, 1), transform.rotation);
						outputs.Add (newoutput);
						printer printcode = newoutput.GetComponent<printer> ();
						printcode.displaytext = reader.GetAttribute ("text");
						printcode.sidebar = sidebaroutput;
						printcode.selectTools = selectedtool;
						if (reader.GetAttribute ("tool") != null) {
							string toolatt = reader.GetAttribute ("tool");
							string[] toolcounts = toolatt.Split (',');
							for (int i = 0; i<6; i++) {
								printcode.tools [i] = int.Parse (toolcounts [i]);
							}
						}
					} else if (reader.NodeType == XmlNodeType.Element && reader.Name == "warp") {
						GameObject newwarp = (GameObject)Instantiate (warpobject, new Vector3 (-7, initialLineY - (lineInfo.LineNumber - 1) * linespacing, 1), transform.rotation);
						warps.Add (newwarp);
						warper warpcode = newwarp.GetComponent<warper> ();
						warpcode.CodeScreen = this.gameObject;
						warpcode.filename = reader.GetAttribute ("file");
						warpcode.selectTools = selectedtool;
						if (reader.GetAttribute ("tool") != null) {
							string toolatt = reader.GetAttribute ("tool");
							string[] toolcounts = toolatt.Split (',');
							for (int i = 0; i<6; i++) {
								warpcode.tools [i] = int.Parse (toolcounts [i]);
							}
						}
						if (reader.GetAttribute ("line") != null) {
							warpcode.linenum = reader.GetAttribute ("line");
						}
					} else if (reader.NodeType == XmlNodeType.Element && reader.Name == "bug") {
						bugsize = int.Parse (reader.GetAttribute ("size"));
						int row = 0;
						if (reader.GetAttribute ("row") != null) {
							row = int.Parse (reader.GetAttribute ("row"));
						}
						int col = 0;
						if (reader.GetAttribute ("col") != null) {
							col = int.Parse (reader.GetAttribute ("col"));
						}
						levelbug = (GameObject)Instantiate (bugobject, new Vector3 (bugXshift + col * fontwidth + (bugsize - 1) * levelLineRatio, initialLineY - (lineInfo.LineNumber + row - 1 + 0.5f * (bugsize - 1)) * linespacing + 0.4f, 0f), transform.rotation);
						levelbug.transform.localScale += new Vector3 (bugscale * (bugsize - 1), bugscale * (bugsize - 1), 0);
						levelbug.GetComponent<GenericBug> ().codescreen = this.gameObject;
						bugs.Add (levelbug);
						num_of_bugs++;

					} else if (reader.NodeType == XmlNodeType.Element && reader.Name == "comment") {
						int commentsize = int.Parse (reader.GetAttribute ("size"));
						GameObject newcomment = (GameObject)Instantiate (commentobject, new Vector3 (-7, initialLineY - (lineInfo.LineNumber - 1 + 0.9f * (commentsize - 1)) * linespacing, 0f), transform.rotation);
						comments.Add (newcomment);
						commentBlock commentcode = newcomment.GetComponent<commentBlock> ();
						commentcode.code = leveltext;
						commentcode.errmsg = reader.GetAttribute ("text");
						commentcode.sideoutput = sidebaroutput;
						commentcode.oldtext = codetext;
						newcomment.transform.localScale += new Vector3 (0, textscale * (commentsize - 1), 0);
						commentcode.selectTools = selectedtool;
						if (reader.GetAttribute ("tool") != null) {
							string toolatt = reader.GetAttribute ("tool");
							string[] toolcounts = toolatt.Split (',');
							for (int i = 0; i<6; i++) {
								commentcode.tools [i] = int.Parse (toolcounts [i]);
							}
						}
					} else if (reader.NodeType == XmlNodeType.Element && reader.Name == "oncheck") {
						GameObject newcheck = (GameObject)Instantiate (oncheckobject, new Vector3 (-7, initialLineY - (lineInfo.LineNumber - 1) * linespacing, 1), transform.rotation);
						oncomments.Add (newcheck);
						checker oncheckcode = newcheck.GetComponent<checker> ();
						oncheckcode.displaytext = reader.GetAttribute ("text");
						oncheckcode.expected = reader.GetAttribute ("answer");
						oncheckcode.codescreen = this.gameObject;
						oncheckcode.sidebar = sidebaroutput;
						oncheckcode.code = leveltext;
						oncheckcode.innertext = reader.ReadInnerXml (); //danger will robinson
						tasklist [1]++;
					} else if (reader.NodeType == XmlNodeType.Element && reader.Name == "rename") {
						GameObject newrename = (GameObject)Instantiate (renameobject, new Vector3 (-7, initialLineY - (lineInfo.LineNumber - 1) * linespacing, 1), transform.rotation);
						renamers.Add (newrename);
						rename renamecode = newrename.GetComponent<rename> ();
						renamecode.displaytext = reader.GetAttribute ("text");
						renamecode.correct = int.Parse (reader.GetAttribute ("correct"));
						renamecode.codescreen = this.gameObject;
						renamecode.sidebar = sidebaroutput;
						renamecode.code = leveltext;

						string names = reader.GetAttribute ("names");
						string[] namelist = names.Split (',');
						for (int i = 0; i<namelist.Length; i++) {
							renamecode.names.Add (namelist [i]);
						}
						renamecode.innertext = reader.ReadInnerXml ();

						tasklist [2]++;
					} else if (reader.NodeType == XmlNodeType.Element && reader.Name == "oncomment") {
						int commentsize = int.Parse (reader.GetAttribute ("size"));
						GameObject newcomment = (GameObject)Instantiate (oncommentobject, new Vector3 (-7, initialLineY - (lineInfo.LineNumber - 1 + 0.9f * (commentsize - 1)) * linespacing, 0f), transform.rotation);
						oncomments.Add (newcomment);
						oncomment oncommentcode = newcomment.GetComponent<oncomment> ();
						oncommentcode.code = leveltext;
						oncommentcode.oldtext = codetext;
						oncommentcode.codescreen = this.gameObject;
						newcomment.transform.localScale += new Vector3 (0, textscale * (commentsize - 1), 0);
						tasklist [3]++;
					} else if (reader.NodeType == XmlNodeType.Element && reader.Name == "badcomment") {
						int commentsize = int.Parse (reader.GetAttribute ("size"));
						GameObject newbadcom = (GameObject)Instantiate (badcommentobject, new Vector3 (-7, initialLineY - (lineInfo.LineNumber - 1 + 0.9f * (commentsize - 1)) * linespacing, 0f), transform.rotation);
						badcomments.Add (newbadcom);
						badcomment badcomcode = newbadcom.GetComponent<badcomment> ();
						badcomcode.code = leveltext;
						badcomcode.righttext = reader.GetAttribute ("righttext");
						badcomcode.oldtext = codetext;
						badcomcode.codescreen = this.gameObject;
						newbadcom.transform.localScale += new Vector3 (0, textscale * (commentsize - 1), 0);
					} else if (reader.NodeType == XmlNodeType.Element && reader.Name == "uncom") {
						int commentsize = int.Parse (reader.GetAttribute ("size"));
						GameObject newuncom = (GameObject)Instantiate (uncomobject, new Vector3 (-7, initialLineY - (lineInfo.LineNumber - 1 + 0.9f * (commentsize - 1)) * linespacing, 0f), transform.rotation);
						uncoms.Add (newuncom);
						uncom uncomcode = newuncom.GetComponent<uncom> ();
						uncomcode.code = leveltext;
						uncomcode.oldtext = codetext;
						uncomcode.codescreen = this.gameObject;
						newuncom.transform.localScale += new Vector3 (0, textscale * (commentsize - 1), 0);
						tasklist [4]++;
					} else if (reader.NodeType == XmlNodeType.Element && reader.Name == "baduncom") {
						int commentsize = int.Parse (reader.GetAttribute ("size"));
						GameObject newbaduncom = (GameObject)Instantiate (baduncomobject, new Vector3 (-7, initialLineY - (lineInfo.LineNumber - 1 + 0.9f * (commentsize - 1)) * linespacing, 0f), transform.rotation);
						baduncoms.Add (newbaduncom);
						baduncom baduncomcode = newbaduncom.GetComponent<baduncom> ();
						baduncomcode.code = leveltext;
						baduncomcode.righttext = reader.GetAttribute ("righttext");
						baduncomcode.oldtext = codetext;
						baduncomcode.codescreen = this.gameObject;
						newbaduncom.transform.localScale += new Vector3 (0, textscale * (commentsize - 1), 0);
					} else if (reader.NodeType == XmlNodeType.Element && reader.Name == "breakpoint") {
						GameObject newbreakpoint = (GameObject)Instantiate (breakpointobject, new Vector3 (-10, initialLineY - (lineInfo.LineNumber - 1) * linespacing + 0.4f, 1), transform.rotation);
						breakpoints.Add (newbreakpoint);
						Breakpoint breakcode = newbreakpoint.GetComponent<Breakpoint> ();
						breakcode.sidebaroutput = sidebaroutput;
						breakcode.values = reader.GetAttribute ("text");
						breakcode.selectTools = selectedtool;
						if (reader.GetAttribute ("tool") != null) {
							string toolatt = reader.GetAttribute ("tool");
							string[] toolcounts = toolatt.Split (',');
							for (int i = 0; i<6; i++) {
								breakcode.tools [i] = int.Parse (toolcounts [i]);
							}
						}
					} else if (reader.NodeType == XmlNodeType.Element && reader.Name == "prize") {
						bugsize = int.Parse (reader.GetAttribute ("size"));
						GameObject prizebug = (GameObject)Instantiate (prizeobject, new Vector3 (-9f + (bugsize - 1) * levelLineRatio, initialLineY - (lineInfo.LineNumber - 1 + 0.5f * (bugsize - 1)) * linespacing + 0.4f, 0f), transform.rotation);
						prizebug.transform.localScale += new Vector3 (bugscale * (bugsize - 1), bugscale * (bugsize - 1), 0);
						prizebug.GetComponent<PrizeBug> ().tools = selectedtool;
						string[] bonuses = reader.GetAttribute ("bonuses").Split (',');
						for (int i = 0; i<6; i++) {
							prizebug.GetComponent<PrizeBug> ().bonus [i] += int.Parse (bonuses [i]);
						}
						prizes.Add (prizebug);
					} else if (reader.NodeType == XmlNodeType.Element && reader.Name == "beacon") {
						GameObject newbeacon = (GameObject)Instantiate (beaconobject, new Vector3 (-10, initialLineY - (lineInfo.LineNumber - 1) * linespacing + 0.4f, 1), transform.rotation);
						newbeacon.GetComponent<beacon> ().codescreen = this.gameObject;
						if (reader.GetAttribute ("actnums") != "") {
							string[] actnums = reader.GetAttribute ("actnums").Split (',');

							for (int i = 0; i<actnums.Length; i++) {
								newbeacon.GetComponent<beacon> ().actnumbers.Add (int.Parse (actnums [i]));
								tasklist [0]++;
							}
						}
						beacons.Add (newbeacon);
					}
				}
				reader.Close ();
				int j = 0;
				int k = 0;
				int l = 0;
				int m = 0;
				int n = 0;
				for (int i=0; i<codenode.ChildNodes.Count; i++) {
					if (codenode.ChildNodes [i].Name == "comment") {
						comments [j].GetComponent<commentBlock> ().blocktext = codenode.ChildNodes [i].InnerText.Trim ();
						j++;
					}
					if (codenode.ChildNodes [i].Name == "oncomment") {
						oncomments [k].GetComponent<oncomment> ().blocktext = codenode.ChildNodes [i].InnerText.Substring(29).Trim ();
						k++;
					}
					if (codenode.ChildNodes [i].Name == "badcomment") {
						badcomments [l].GetComponent<badcomment> ().blocktext = codenode.ChildNodes [i].InnerText.Substring(29).Trim ();
						l++;
					}
					if (codenode.ChildNodes [i].Name == "uncom") {
						uncoms [m].GetComponent<uncom> ().blocktext = codenode.ChildNodes [i].InnerText;
						m++;


						
					}
					if (codenode.ChildNodes [i].Name == "baduncom") {
						baduncoms [n].GetComponent<baduncom> ().blocktext = codenode.ChildNodes [i].InnerText;
						n++;
					}
				}
				foreach (GameObject badcom in badcomments) {
					foreach (GameObject oncom in oncomments) {
						if (badcom.GetComponent<badcomment> ().righttext == oncom.GetComponent<oncomment> ().blocktext) {
							badcom.GetComponent<badcomment> ().rightcomment = oncom;
							break;
						}
					}
				}

				foreach (GameObject baduncom in baduncoms) {
					foreach (GameObject uncom in uncoms) {
						if (uncom.GetComponent<uncom> ().blocktext.Contains(baduncom.GetComponent<baduncom> ().righttext)) {
							baduncom.GetComponent<baduncom> ().rightcomment = uncom;
							break;
						}
					}
				}
			}
		}
	}

	public void SetTools (XmlNode levelnode)
	{
		for (int i = 0; i<numOfTools; i++) {
			toolIcons [i].GetComponent<GUITexture> ().enabled = false;
		}
		foreach (XmlNode codenode in levelnode.ChildNodes) {
			if (codenode.Name == "tools") {
				foreach (XmlNode toolnode in codenode.ChildNodes) {
					int toolnum = int.Parse (toolnode.InnerText);
					toolIcons [toolnum].GetComponent<GUITexture> ().enabled = true;
					selectedtool.GetComponent<SelectedTool> ().toolCounts [toolnum] = int.Parse (toolnode.Attributes ["count"].Value);
				}
			}
		}
	}

	public void ResetLevel (bool warp)
	{
		foreach (GameObject ln in lines) {
			Destroy (ln);
		}
		foreach (GameObject bc in beacons) {
			Destroy (bc);
		}
		foreach (GameObject oc in oncomments) {
			Destroy (oc);
		}
		foreach (GameObject oc in onchecks) {
			Destroy (oc);
		}
		foreach (GameObject oc in renamers) {
			Destroy (oc);
		}
		foreach (GameObject op in outputs) {
			Destroy (op);
		}
		foreach (GameObject op in badcomments) {
			Destroy (op);
		}
		foreach (GameObject wp in warps) {
			Destroy (wp);
		}
		foreach (GameObject cm in comments) {
			Destroy (cm);
		}
		foreach (GameObject wp in uncoms) {
			Destroy (wp);
		}
		foreach (GameObject cm in baduncoms) {
			Destroy (cm);
		}
		foreach (GameObject bp in breakpoints) {
			Destroy (bp);
		}
		foreach (GameObject pb in prizes) {
			Destroy (pb);
		}
		if (levelbug) {
			Destroy (levelbug);
		}
		for (int i = 0; i<5; i++) {
			taskscompleted [i] = 0;
			tasklist [i] = 0;
		}
		sidebaroutput.GetComponent<GUIText> ().text = "";
		lines = new List<GameObject> ();
		outputs = new List<GameObject> ();
		warps = new List<GameObject> ();
		oncomments = new List<GameObject> ();
		renamers = new List<GameObject> ();
		badcomments = new List<GameObject> ();
		onchecks = new List<GameObject> ();
		uncoms = new List<GameObject> ();
		baduncoms = new List<GameObject> ();
		bugs = new List<GameObject> ();
		comments = new List<GameObject> ();
		breakpoints = new List<GameObject> ();
		prizes = new List<GameObject> ();
		if (!warp) {
			for (int i = 0; i<numOfTools; i++) {
				toolIcons [i].GetComponent<GUITexture> ().enabled = false;
				toolIcons [i].GetComponent<GUITexture> ().color = new Color (0.3f, 0.3f, 0.3f);
				selectedtool.GetComponent<SelectedTool> ().toolCounts [i] = 0;
				selectedtool.GetComponent<SelectedTool> ().projectilecode = 0;
			}
		}
				
		num_of_bugs = 0;
		this.transform.position += new Vector3 (0, (linecount / 2) * linespacing, 0);
		this.transform.localScale -= new Vector3 (0, levelLineRatio * linecount, 0);
		linecount = 0;
		hero.transform.position = leveltext.transform.position;
	}

	public void GameOver ()
	{

		GUISwitch (false);
		menu.GetComponent<Menu> ().gameon = false;
		gamestate = 4;
	}

	public void Victory ()
	{
		GUISwitch (false);
		
		menu.GetComponent<Menu> ().gameon = false;
		currentlevel = "level5";
		gamestate = 12;
	}

	void OnTriggerEnter2D(Collider2D c){
		if (c.name.StartsWith("projectile")) {
			losing = true;
		}
	}
}

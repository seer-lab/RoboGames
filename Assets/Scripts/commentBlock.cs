//**************************************************//
// Class Name: commentBlock
// Class Description:
// Methods:
// 		void Start()
//		void Update()
//		void OnTriggerEnter2D(Collider2D c)
//		void printLogFile(string sMessage)
// Author: Michael Miljanovic
// Date Last Modified: 6/1/2016
//**************************************************//

using UnityEngine;
using System.Collections;
using System.IO;

public class commentBlock : MonoBehaviour {

	public int[] tools = new int[stateLib.NUMBER_OF_TOOLS];
	public string oldtext   = "";
	public string blocktext = "";
	public string errmsg    = "";
	public GameObject code;
	public GameObject sideoutput;
	public GameObject selectTools;

	private bool resetting  = false;
	private bool toolgiven = false;
	private float resetTime = 0f;
	private float timeDelay = 30f;

	//.................................>8.......................................
	// Use this for initialization
	void Start() {
	}

	//.................................>8.......................................
	// Update is called once per frame
	void Update() {
		if (resetting) {
			if (code.GetComponent<TextMesh>().text != oldtext.Replace(blocktext, stringLib.COMMENT_BLOCK_COLOR_TAG +
																				 blocktext.Replace("/**/","") +
																				 stringLib.COMMENT_CLOSE_COLOR_TAG)) {
				resetting = false;
			}
			else if (Time.time > resetTime || Input.GetKeyDown(KeyCode.Return)) {
				resetting = false;
				sideoutput.GetComponent<GUIText>().text = "";
				code.GetComponent<TextMesh>().text = oldtext;
			}
		}
	}

	//.................................>8.......................................
	void OnTriggerEnter2D(Collider2D c) {
		if (c.name == stringLib.PROJECTILE_COMMENT) {
			printLogFile(stringLib.LOG_COMMENT_ON);
			Destroy(c.gameObject);
			GetComponent<AudioSource>().Play();
			code.GetComponent<TextMesh>().text = oldtext.Replace(blocktext, stringLib.COMMENT_BLOCK_COLOR_TAG +
																			blocktext.Replace("/**/","") +
																			stringLib.COMMENT_CLOSE_COLOR_TAG);
			sideoutput.GetComponent<GUIText>().text = errmsg;
			resetTime = Time.time + timeDelay;
			resetting = true;

			if (!toolgiven) {
				toolgiven = true;
				for (int i = 0; i < stateLib.NUMBER_OF_TOOLS; i++) {
					if (tools[i] > 0) {
						selectTools.GetComponent<SelectedTool>().toolget = true;
					}
					selectTools.GetComponent<SelectedTool>().toolCounts[i] += tools[i];
				}
			}
		}
	}

	//.................................>8.......................................
	void printLogFile(string sMessage)
	{
		int position = (int)((stateLib.GAMESETTING_INITIAL_LINE_Y - this.transform.position.y) / stateLib.GAMESETTING_LINE_SPACING);
		StreamWriter sw = new StreamWriter(stringLib.TOOL_LOGFILE, true);
		sMessage = sMessage + position.ToString() + ", " + Time.time.ToString();
		sw.WriteLine(sMessage);
		sw.Close();
	}

	//.................................>8.......................................

}

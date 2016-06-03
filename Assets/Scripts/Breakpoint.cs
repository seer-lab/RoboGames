//**************************************************//
// Class Name: Breakpoint
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

public class Breakpoint : MonoBehaviour {

	public int[] tools = new int[stateLib.NUMBER_OF_TOOLS];
	public string values;
	public GameObject sidebaroutput;
	public AudioClip[] sound = new AudioClip[2];
	public GameObject selectTools;

	private bool activated = false;
	private bool toolgiven = false;

	//.................................>8.......................................
	// Use this for initialization
	void Start() {
		this.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 0.5f);
	}

	//.................................>8.......................................
	// Update is called once per frame
	void Update() {
	}

	//.................................>8.......................................
	void OnTriggerEnter2D(Collider2D c) {
		if (c.name == stringLib.PROJECTILE_DEBUG) {
			if (!activated) {
				GetComponent<AudioSource>().clip = sound[0];
				GetComponent<AudioSource>().Play();
				printLogFile(stringLib.LOG_BREAKPOINT_ON);
			}
			activated = true;
			this.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 1);
		}
		else if (activated && c.name == stringLib.PROJECTILE_ACTIVATOR) {
			printLogFile(stringLib.LOG_BREAKPOINT_ACTIVATED);
			GetComponent<AudioSource>().clip = sound[1];
			GetComponent<AudioSource>().Play();
			sidebaroutput.GetComponent<GUIText>().text = values;
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

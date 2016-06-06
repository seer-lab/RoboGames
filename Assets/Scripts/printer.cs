//**************************************************//
// Class Name: printer
// Class Description:
// Methods:
// 		void Start()
//		void Update()
//		void OnTriggerEnter2D(Collider2D collidingObj)
//		void printLogFile(string sMessage)
// Author: Michael Miljanovic
// Date Last Modified: 6/1/2016
//**************************************************//

using UnityEngine;
using System.Collections;
using System.IO;

public class printer : MonoBehaviour {

	public string displaytext = "";
	public GameObject sidebar;

	public GameObject selectTools;
	public int[] tools = new int[stateLib.NUMBER_OF_TOOLS];
	bool toolgiven = false;

	//.................................>8.......................................
	// Use this for initialization
	void Start() {

	}

	//.................................>8.......................................
	// Update is called once per frame
	void Update() {

	}

	//.................................>8.......................................
	void OnTriggerEnter2D(Collider2D collidingObj) {
		if (collidingObj.name == stringLib.PROJECTILE_ACTIVATOR) {
			// "Printed,"
			printLogFile(stringLib.LOG_PRINTED);
			Destroy(collidingObj.gameObject);
			sidebar.GetComponent<GUIText>().text = displaytext;
			GetComponent<AudioSource>().Play();
			if (!toolgiven) {
				toolgiven = true;
				for (int i = 0; i < stateLib.NUMBER_OF_TOOLS; i++) {
					if (tools[i] > 0) {
						selectTools.GetComponent<SelectedTool>().notifyToolAcquisition();
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

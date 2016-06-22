//**************************************************//
// Class Name: warper
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

public class warper : MonoBehaviour
{
	public int[] tools = new int[stateLib.NUMBER_OF_TOOLS];
	public string filename = "";
	public string linenum = "";
	public GameObject CodescreenObject;
	public GameObject SelectToolsObject;

	private bool toolgiven = false;

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
		if (collidingObj.name == stringLib.PROJECTILE_WARP) {
			string sMessage = stringLib.LOG_WARPED + filename;
			printLogFile(sMessage);
			Destroy(collidingObj.gameObject);
			LevelGenerator lg = CodescreenObject.GetComponent<LevelGenerator>();
			if (!toolgiven) {
				toolgiven = true;
				for (int i = 0; i < stateLib.NUMBER_OF_TOOLS; i++) {
					if (tools[i] > 0) {
						SelectToolsObject.GetComponent<SelectedTool>().notifyToolAcquisition();
					}
					SelectToolsObject.GetComponent<SelectedTool>().toolCounts[i] += tools[i];
					if (SelectToolsObject.GetComponent<SelectedTool>().toolCounts[i] == 0 && SelectToolsObject.GetComponent<SelectedTool>().bonusTools[i] == 0) {
						SelectToolsObject.GetComponent<SelectedTool>().toolIcons[i].GetComponent<GUITexture>().enabled = false;
						if (SelectToolsObject.GetComponent<SelectedTool>().projectilecode == i) {
							SelectToolsObject.GetComponent<SelectedTool>().NextTool();
							}
						}
					}
				}

		lg.BuildLevel(lg.gamemode + @"leveldata\" + filename, true, linenum);
		}
	}

	//.................................>8.......................................
	void printLogFile(string sMessage)
	{
		int position = (int)((stateLib.GAMESETTING_INITIAL_LINE_Y - this.transform.position.y) / stateLib.GAMESETTING_LINE_SPACING);
		StreamWriter sw = new StreamWriter(stringLib.TOOL_LOGFILE, true);
		sw.WriteLine(sMessage + position.ToString() + ", " + Time.time.ToString());
		sw.Close();
	}

	//.................................>8.......................................
}

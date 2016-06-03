//**************************************************//
// Class Name: badcomment
// Class Description:
// Methods:
// 		void Start()
//		void Update()
// Author: Michael Miljanovic
// Date Last Modified: 6/1/2016
//**************************************************//

using UnityEngine;
using System.Collections;

public class Checklist : MonoBehaviour {

	public GameObject codescreen;

	private LevelGenerator lg;

	//.................................>8.......................................
	// Use this for initialization
	void Start() {
		GetComponent<GUIText>().text = "";
		lg = codescreen.GetComponent<LevelGenerator>();
	}

	//.................................>8.......................................
	// Update is called once per frame
	void Update() {
		if (lg.gamestate == stateLib.GAMESTATE_IN_GAME && lg.gamemode == stringLib.GAME_MODE_ON) {
			GetComponent<GUIText>().text = "Tasks:";
			// Activate
			if (lg.tasklist[0] > 0) {
				if (lg.tasklist[0] == lg.taskscompleted[0]) {
					GetComponent<GUIText>().text += "\n" +
					 								stringLib.CHECKLIST_COMPLETE_COLOR_TAG +
													"ACTIVATE the beacons in the right order.✓" +
													stringLib.CLOSE_COLOR_TAG;
				}
				else {
					GetComponent<GUIText>().text += "\n" +
													stringLib.CHECKLIST_INCOMPLETE_ACTIVATE_COLOR_TAG +
													"ACTIVATE" +
													stringLib.CLOSE_COLOR_TAG +
													" the beacons in the right order.";
				}
			}
			// Check
			if (lg.tasklist[1] > 0) {
				if (lg.tasklist[1]==lg.taskscompleted[1]) {
					GetComponent<GUIText>().text += "\n" +
													stringLib.CHECKLIST_COMPLETE_COLOR_TAG +
													"CHECK the values of the variables.✓" +
													stringLib.CLOSE_COLOR_TAG;
				}
				else {
					GetComponent<GUIText>().text += "\n" +
													stringLib.CHECKLIST_INCOMPLETE_CHECK_COLOR_TAG +
													"CHECK" +
													stringLib.CLOSE_COLOR_TAG +
													" the values of the variables.";
				}
			}
			// Name
			if (lg.tasklist[2] > 0) {
				if (lg.tasklist[2]==lg.taskscompleted[2]) {
					GetComponent<GUIText>().text += "\n" +
													stringLib.CHECKLIST_COMPLETE_COLOR_TAG +
													"NAME the variables with appropriate names.✓" +
													stringLib.CLOSE_COLOR_TAG;
				}
				else {
					GetComponent<GUIText>().text += "\n" +
													stringLib.CHECKLIST_INCOMPLETE_NAME_COLOR_TAG +
													"NAME" +
													stringLib.CLOSE_COLOR_TAG +
													" the variables with appropriate names.";
				}
			}
			// Comment
			if (lg.tasklist[3] > 0) {
				if (lg.tasklist[3]==lg.taskscompleted[3]) {
					GetComponent<GUIText>().text += "\n" +
													stringLib.CHECKLIST_COMPLETE_COLOR_TAG +
													"COMMENT the lines that describe the code.✓" +
													stringLib.CLOSE_COLOR_TAG;
				}
				else {
					GetComponent<GUIText>().text += "\n" +
													stringLib.CHECKLIST_INCOMPLETE_COMMENT_COLOR_TAG +
													"COMMENT" +
													stringLib.CLOSE_COLOR_TAG +
													" the lines that describe the code.";
				}
			}
			// Un-comment
			if (lg.tasklist[4] > 0) {
				if (lg.tasklist[4]==lg.taskscompleted[4]) {
					GetComponent<GUIText>().text += "\n" +
													stringLib.CHECKLIST_COMPLETE_COLOR_TAG +
													"UN-COMMENT the code that is correct.✓" +
													stringLib.CLOSE_COLOR_TAG;
				}
				else {
					GetComponent<GUIText>().text += "\n" +
													stringLib.CHECKLIST_INCOMPLETE_UNCOMMENT_COLOR_TAG +
													"UN-COMMENT" +
													stringLib.CLOSE_COLOR_TAG +
													" the code that is correct.";
				}
			}
		}
		// No remaining tasks
		else {
			GetComponent<GUIText>().text = "";
		}
	}

	//.................................>8.......................................
}

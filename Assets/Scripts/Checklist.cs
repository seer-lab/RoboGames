//**************************************************//
// Class Name: Checklist.cs
// Class Description: This is the controller for the Tooltip on the sidebar.
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
	public GameObject selectedTool;

	private LevelGenerator lg;
	private int projectilecode;
	//.................................>8.......................................
	// Use this for initialization
	void Start() {
		lg = codescreen.GetComponent<LevelGenerator>();
		GetComponent<GUIText>().text = "";
		projectilecode = selectedTool.GetComponent<SelectedTool>().projectilecode;
	}

	//.................................>8.......................................
	// Update is called once per frame
	void Update() {

		if (lg.gamestate == stateLib.GAMESTATE_IN_GAME && lg.gamemode == stringLib.GAME_MODE_ON) {
			GetComponent<GUIText>().text = "Tooltip:";
			projectilecode = selectedTool.GetComponent<SelectedTool>().projectilecode;
			// Activate
			if (lg.tasklist[0] > 0 && projectilecode == 0) {
				if (lg.tasklist[0] == lg.taskscompleted[0]) {
					GetComponent<GUIText>().text += "\n" +
					 								lg.stringLibrary.checklist_complete_color_tag +
													"ACTIVATE the beacons in the \nright order✓" +
													stringLib.CLOSE_COLOR_TAG;
				}
				else {
					GetComponent<GUIText>().text += "\n" +
													lg.stringLibrary.checklist_incomplete_activate_color_tag +
													"ACTIVATE" +
													stringLib.CLOSE_COLOR_TAG +
													" the beacons in the \nright order";
				}
			}
			// Check
			if (lg.tasklist[1] > 0 && projectilecode == 1) {
				if (lg.tasklist[1]==lg.taskscompleted[1]) {
					GetComponent<GUIText>().text += "\n" +
													lg.stringLibrary.checklist_complete_color_tag +
													"CHECK the values of the \nvariables✓" +
													stringLib.CLOSE_COLOR_TAG;
				}
				else {
					GetComponent<GUIText>().text += "\n" +
													lg.stringLibrary.checklist_incomplete_question_color_tag +
													"CHECK" +
													stringLib.CLOSE_COLOR_TAG +
													" the values of the \nvariables";
				}
			}
			// Name
			if (lg.tasklist[2] > 0 && projectilecode == 2) {
				if (lg.tasklist[2]==lg.taskscompleted[2]) {
					GetComponent<GUIText>().text += "\n" +
													lg.stringLibrary.checklist_complete_color_tag +
													"RENAME the obscure variables✓" +
													stringLib.CLOSE_COLOR_TAG;
				}
				else {
					GetComponent<GUIText>().text += "\n" +
													lg.stringLibrary.checklist_incomplete_name_color_tag +
													"RENAME" +
													stringLib.CLOSE_COLOR_TAG +
													" the obscure variables";
				}
			}
			// Comment
			if (lg.tasklist[3] > 0 && projectilecode == 3) {
				if (lg.tasklist[3]==lg.taskscompleted[3]) {
					GetComponent<GUIText>().text += "\n" +
													lg.stringLibrary.checklist_complete_color_tag +
													"COMMENT the lines that \ndescribe the code✓" +
													stringLib.CLOSE_COLOR_TAG;
				}
				else {
					GetComponent<GUIText>().text += "\n" +
													lg.stringLibrary.checklist_incomplete_comment_color_tag +
													"COMMENT" +
													stringLib.CLOSE_COLOR_TAG +
													" the lines that \ndescribe the code";
				}
			}
			// Un-comment
			if (lg.tasklist[4] > 0 && projectilecode == 4) {
				if (lg.tasklist[4]==lg.taskscompleted[4]) {
					GetComponent<GUIText>().text += "\n" +
													lg.stringLibrary.checklist_complete_color_tag +
													"UN-COMMENT the code that is \ncorrect✓" +
													stringLib.CLOSE_COLOR_TAG;
				}
				else {
					GetComponent<GUIText>().text += "\n" +
													lg.stringLibrary.checklist_incomplete_uncomment_color_tag +
													"UN-COMMENT" +
													stringLib.CLOSE_COLOR_TAG +
													" the code that is \ncorrect";
				}
			}
		}
		// Not in game
		else {
			GetComponent<GUIText>().text = "";
		}
	}

	//.................................>8.......................................
}

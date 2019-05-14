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
using UnityEngine.UI; 
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
		GetComponent<Text>().text = "";
		projectilecode = selectedTool.GetComponent<SelectedTool>().projectilecode;
	}

	//.................................>8.......................................
	// Update is called once per frame
	void Update() {

		if (GlobalState.GameState == stateLib.GAMESTATE_IN_GAME && GlobalState.GameMode == stringLib.GAME_MODE_ON) {
			GetComponent<Text>().text = "Tooltip:";
			projectilecode = selectedTool.GetComponent<SelectedTool>().projectilecode;
			// Activate
			if (GlobalState.level.Tasks[0] > 0 && projectilecode == 0) {
                if (GlobalState.level.Tasks[0] == GlobalState.level.CompletedTasks[0]) {
					GetComponent<Text>().text += "\n" +
                                                     GlobalState.StringLib.checklist_complete_color_tag +
													"ACTIVATE the beacons in the \nright order✓" +
													stringLib.CLOSE_COLOR_TAG;
				}
				else {
					GetComponent<Text>().text += "\n" +
                                                    GlobalState.StringLib.checklist_incomplete_activate_color_tag +
													"ACTIVATE" +
													stringLib.CLOSE_COLOR_TAG +
													" the beacons in the \nright order";
				}
			}
			// Check
			if (GlobalState.level.Tasks[1] > 0 && projectilecode == 1) {
				if (GlobalState.level.Tasks[1]==GlobalState.level.CompletedTasks[1]) {
					GetComponent<Text>().text += "\n" +
                                                    GlobalState.StringLib.checklist_complete_color_tag +
													"CHECK the values of the \nvariables✓" +
													stringLib.CLOSE_COLOR_TAG;
				}
				else {
					GetComponent<Text>().text += "\n" +
                                                    GlobalState.StringLib.checklist_incomplete_question_color_tag +
													"CHECK" +
													stringLib.CLOSE_COLOR_TAG +
													" the values of the \nvariables";
				}
			}
			// Name
			if (GlobalState.level.Tasks[2] > 0 && projectilecode == 2) {
				if (GlobalState.level.Tasks[2]== GlobalState.level.CompletedTasks[2]) {
					GetComponent<Text>().text += "\n" +
                                                    GlobalState.StringLib.checklist_complete_color_tag +
													"RENAME the obscure variables✓" +
													stringLib.CLOSE_COLOR_TAG;
				}
				else {
					GetComponent<Text>().text += "\n" +
                                                    GlobalState.StringLib.checklist_incomplete_name_color_tag +
													"RENAME" +
													stringLib.CLOSE_COLOR_TAG +
													" the obscure variables";
				}
			}
			// Comment
			if (GlobalState.level.Tasks[3] > 0 && projectilecode == 3) {
				if (GlobalState.level.Tasks[3]== GlobalState.level.CompletedTasks[3]) {
					GetComponent<Text>().text += "\n" +
                                                    GlobalState.StringLib.checklist_complete_color_tag +
													"COMMENT the lines that \ndescribe the code✓" +
													stringLib.CLOSE_COLOR_TAG;
				}
				else {
					GetComponent<Text>().text += "\n" +
													GlobalState.StringLib.checklist_incomplete_comment_color_tag +
													"COMMENT" +
													stringLib.CLOSE_COLOR_TAG +
													" the lines that \ndescribe the code";
				}
			}
			// Un-comment
			if (GlobalState.level.Tasks[4] > 0 && projectilecode == 4) {
				if (GlobalState.level.Tasks[4]== GlobalState.level.CompletedTasks[4]) {
					GetComponent<Text>().text += "\n" +
                                                    GlobalState.StringLib.checklist_complete_color_tag +
													"UN-COMMENT the code that is \ncorrect✓" +
													stringLib.CLOSE_COLOR_TAG;
				}
				else {
					GetComponent<Text>().text += "\n" +
                                                    GlobalState.StringLib.checklist_incomplete_uncomment_color_tag +
													"UN-COMMENT" +
													stringLib.CLOSE_COLOR_TAG +
													" the code that is \ncorrect";
				}
			}
			//Hint Tool
			if(projectilecode == stateLib.TOOL_HINTER){
				GetComponent<Text>().text +=GlobalState.StringLib.checklist_complete_color_tag + 
											"\nProvids Hints when thrown at code" + 
											stringLib.CLOSE_COLOR_TAG;
			}
		}
		// Not in game
		else {
			GetComponent<Text>().text = "";
		}
	}

	//.................................>8.......................................
}

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

/// <summary>
/// Handles the tool tips at the bottom of the sidebar. 
/// </summary>
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

		if (GlobalState.GameState == stateLib.GAMESTATE_IN_GAME ) {
			GetComponent<Text>().text = "Tooltip:";
			projectilecode = selectedTool.GetComponent<SelectedTool>().projectilecode;
			// Activate
			if (GlobalState.level.Tasks[0] > 0 && projectilecode == 0) {
                if (GlobalState.level.Tasks[0] == GlobalState.level.CompletedTasks[0]) {
					if (GlobalState.GameMode == stringLib.GAME_MODE_ON)
						GetComponent<Text>().text += "\n" +
														GlobalState.StringLib.checklist_complete_color_tag +
														"ACTIVATE the beacons in the right order✓" +
														stringLib.CLOSE_COLOR_TAG;
					else 
						GetComponent<Text>().text += "\n" +
														GlobalState.StringLib.checklist_complete_color_tag +
														"FIX the bug in the code!✓" +
														stringLib.CLOSE_COLOR_TAG;
				}
				else {
					if (GlobalState.GameMode == stringLib.GAME_MODE_ON)
						GetComponent<Text>().text += "\n" +
														GlobalState.StringLib.checklist_incomplete_activate_color_tag +
														"ACTIVATE" +
														stringLib.CLOSE_COLOR_TAG +
														" the beacons in the right order";
					else 
						GetComponent<Text>().text += "\n" +
														GlobalState.StringLib.checklist_incomplete_activate_color_tag +
														"FIX" +
														stringLib.CLOSE_COLOR_TAG +
														" the bug in the code!";
				}
			}
			// Check
			if (GlobalState.level.Tasks[1] > 0 && projectilecode == 1) {
				if (GlobalState.level.Tasks[1]==GlobalState.level.CompletedTasks[1]) {
					if (GlobalState.GameMode == stringLib.GAME_MODE_ON)
						GetComponent<Text>().text += "\n" +
														GlobalState.StringLib.checklist_complete_color_tag +
														"CHECK the values of the variables✓" +
														stringLib.CLOSE_COLOR_TAG;
					else 
						GetComponent<Text>().text += "\n" +
														GlobalState.StringLib.checklist_complete_color_tag +
														"PRINT the values of the variables✓" +
														stringLib.CLOSE_COLOR_TAG;
				}
				else {
					if (GlobalState.GameMode == stringLib.GAME_MODE_ON)
						GetComponent<Text>().text += "\n" +
														GlobalState.StringLib.checklist_incomplete_question_color_tag +
														"CHECK" +
														stringLib.CLOSE_COLOR_TAG +
														" the values of the variables";
					else 
						GetComponent<Text>().text += "\n" +
														GlobalState.StringLib.checklist_incomplete_question_color_tag +
														"PRINT" +
														stringLib.CLOSE_COLOR_TAG +
														" the values of the variables";
				}
			}
			// Name
			if (GlobalState.level.Tasks[2] > 0 && projectilecode == 2) {
				if (GlobalState.level.Tasks[2]== GlobalState.level.CompletedTasks[2]) {
					if (GlobalState.GameMode == stringLib.GAME_MODE_ON)
						GetComponent<Text>().text += "\n" +
														GlobalState.StringLib.checklist_complete_color_tag +
														"RENAME the obscure variables✓" +
														stringLib.CLOSE_COLOR_TAG;
					else 
					GetComponent<Text>().text += "\n" +
                                                    GlobalState.StringLib.checklist_complete_color_tag +
													"WARP to other blocks of Code✓" +
													stringLib.CLOSE_COLOR_TAG;
				}
				else {
					if (GlobalState.GameMode == stringLib.GAME_MODE_ON)
						GetComponent<Text>().text += "\n" +
														GlobalState.StringLib.checklist_incomplete_name_color_tag +
														"RENAME" +
														stringLib.CLOSE_COLOR_TAG +
														" the obscure variables";
					else 
						GetComponent<Text>().text += "\n" +
															GlobalState.StringLib.checklist_incomplete_name_color_tag +
															"WARP" +
															stringLib.CLOSE_COLOR_TAG +
															" to other blocks of Code";
				}
			}
			// Comment
			if (GlobalState.level.Tasks[3] > 0 && projectilecode == 3) {
				if (GlobalState.level.Tasks[3]== GlobalState.level.CompletedTasks[3]) {
					if (GlobalState.GameMode == stringLib.GAME_MODE_ON)
						GetComponent<Text>().text += "\n" +
														GlobalState.StringLib.checklist_complete_color_tag +
														"COMMENT the lines that describe the code✓" +
														stringLib.CLOSE_COLOR_TAG;
					else 
						GetComponent<Text>().text += "\n" +
                                                    GlobalState.StringLib.checklist_complete_color_tag +
													"COMMENT the lines to test the code✓" +
													stringLib.CLOSE_COLOR_TAG;
				}
				else {
					if (GlobalState.GameMode == stringLib.GAME_MODE_ON)
						GetComponent<Text>().text += "\n" +
														GlobalState.StringLib.checklist_incomplete_comment_color_tag +
														"COMMENT" +
														stringLib.CLOSE_COLOR_TAG +
														" the lines that describe the code";
					else 
						GetComponent<Text>().text += "\n" +
													GlobalState.StringLib.checklist_incomplete_comment_color_tag +
													"COMMENT" +
													stringLib.CLOSE_COLOR_TAG +
													" the lines to test the code";
				}
			}
			// Un-comment
			if (GlobalState.level.Tasks[4] > 0 && projectilecode == 4) {
				if (GlobalState.level.Tasks[4]== GlobalState.level.CompletedTasks[4]) {
					if (GlobalState.GameMode == stringLib.GAME_MODE_ON)
						GetComponent<Text>().text += "\n" +
														stringLib.BLUE_COLOR_TAG +
														"REVIEW the code that is correct✓" +
														stringLib.CLOSE_COLOR_TAG;
					else 
						GetComponent<Text>().text += "\n" +
                                                    stringLib.BLUE_COLOR_TAG +
													"Add a BREAKPOINT to monitor the code✓" +
													stringLib.CLOSE_COLOR_TAG;
				}
				else {
					if (GlobalState.GameMode == stringLib.GAME_MODE_ON)
						GetComponent<Text>().text += "\n" +
														stringLib.BLUE_COLOR_TAG +
														"REVIEW" +
														stringLib.CLOSE_COLOR_TAG +
														" the code that is correct";
					else 
						GetComponent<Text>().text += "\n Add a " +
                                                    stringLib.BLUE_COLOR_TAG +
													"BREAKPOINT" +
													stringLib.CLOSE_COLOR_TAG +
													" to monitor the code.";
					
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

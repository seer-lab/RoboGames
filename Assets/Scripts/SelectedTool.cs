//**************************************************//
// Class Name: SelectedTool
// Class Description: Controller for the sidebar tools
// Methods:
// 		void Start()
//		void Update()
//		public void NextTool()
//		private string ReplaceTextInfinite(int nToolCount)
// Author: Michael Miljanovic
// Date Last Modified: 6/1/2016
//**************************************************//

using UnityEngine;
using System.Collections;

public class SelectedTool : MonoBehaviour
{
	public int projectilecode = 0;
	public int[] toolCounts = new int[stateLib.NUMBER_OF_TOOLS];
	public int[] bonusTools = new int[stateLib.NUMBER_OF_TOOLS];
	public GameObject[] toolLabels = new GameObject[stateLib.NUMBER_OF_TOOLS];
	public GameObject codescreen;
	public GameObject hero;
	public GameObject toolprompt;
	public GameObject toolAvailableTools;
	public GameObject levelDescription;
  public GameObject outputtext;
	public GameObject[] toolIcons = new GameObject[stateLib.NUMBER_OF_TOOLS];

	// Determine if the player has lost the game.
	private bool isLosing = false;
	// Determine the player has any remaining activavator tools (RoboBUG)
	private bool noRemainingActivators = false;
	private float lossDelay = 4f;
	private float losstime;
	private Color toolOnColor = new Color(.7f, .7f, .7f);
	private Color toolOffColor = new Color(.3f, .3f, .3f);
	private LevelGenerator lg;
	private bool[] taskComplete = new bool[stateLib.NUMBER_OF_TOOLS];

	private string displayString = "";

	//.................................>8.......................................
	// Use this for initialization
	void Start() {
		lg = codescreen.GetComponent<LevelGenerator>();
		this.GetComponent<GUIText>().text = "";
	}

	//.................................>8.......................................
	// Update is called once per frame
	void Update() {
		// Start of the game, so we are not losing the game and have not failed.
		if (lg.gamestate >= stateLib.GAMESTATE_LEVEL_START) {
			isLosing = false;
			noRemainingActivators = false;
		}
		// In game
		if (lg.gamestate == stateLib.GAMESTATE_IN_GAME) {
			toolAvailableTools.GetComponent<GUIText>().text = "Available Tools:";
			levelDescription.GetComponent<GUIText>().text = lg.destext.GetComponent<TextMesh>().text;
			// If we are losing or failed, trigger the losing sequence on LevelGenerator
			if (isLosing || noRemainingActivators) {
				if (Time.time > losstime) {
					noRemainingActivators = false;
					isLosing = false;
					lg.isLosing = true;
				}
			}
			// Tools are enabled if we have a count greater than 0 for each tool.
			for (int i = 0; i < stateLib.NUMBER_OF_TOOLS; i++) {
				if (toolCounts[i] + bonusTools[i] > 0) {
					if (lg.tasklist[i] != lg.taskscompleted[i]) {
						toolIcons[i].GetComponent<GUITexture>().enabled = true;
					}
					isLosing = false;
					if (projectilecode == stateLib.PROJECTILE_CODE_NO_TOOLS) {
						projectilecode = i;
					}
				}
			}
			hero.GetComponent<hero2Controller>().projectilecode = projectilecode;
			// Pressing Tab cycles to the next tool
			if (Input.GetKeyDown("tab") && projectilecode >= 0) {
				NextTool();
			}

			// A projectile has been thrown by the player in hero2Controller
			if (hero.GetComponent<hero2Controller>().throwing) {
				lg.toolsAirborne++;
				hero.GetComponent<hero2Controller>().throwing = false;
				// Decrease the remaining number of tools if tools are not infinite (999)
				if (toolCounts[projectilecode] < 999) {
					if (bonusTools[projectilecode] > 0) {
						bonusTools[projectilecode] -= 1;
					}
					else {
						toolCounts[projectilecode] -= 1;
					}
				}
				// RoboBUG: If we are out of activators, we've failed the game.
				if (projectilecode == 0 && toolCounts[stateLib.TOOL_CATCHER_OR_ACTIVATOR] == 0 && lg.gamemode == stringLib.GAME_MODE_BUG) {
					noRemainingActivators = true;
					losstime = Time.time + lossDelay;
				}
			}
			switch(projectilecode) {
				case stateLib.TOOL_CATCHER_OR_ACTIVATOR:
					refreshToolList();
					toolIcons[stateLib.TOOL_CATCHER_OR_ACTIVATOR].GetComponent<GUITexture>().color = toolOnColor;
					break;
				case stateLib.TOOL_PRINTER_OR_QUESTION:
					refreshToolList();
					toolIcons[stateLib.TOOL_PRINTER_OR_QUESTION].GetComponent<GUITexture>().color = toolOnColor;
					break;
				case stateLib.TOOL_WARPER_OR_RENAMER:
					refreshToolList();
					toolIcons[stateLib.TOOL_WARPER_OR_RENAMER].GetComponent<GUITexture>().color = toolOnColor;
					break;
				case stateLib.TOOL_COMMENTER:
					refreshToolList();
					toolIcons[stateLib.TOOL_COMMENTER].GetComponent<GUITexture>().color = toolOnColor;
					break;
				case stateLib.TOOL_CONTROL_FLOW:
					refreshToolList();
					toolIcons[stateLib.TOOL_CONTROL_FLOW].GetComponent<GUITexture>().color = toolOnColor;
					break;
				case stateLib.TOOL_HELPER:
					refreshToolList();
					toolIcons[stateLib.TOOL_HELPER].GetComponent<GUITexture>().color = toolOnColor;
					break;
				default:
					break;
			}
			if (projectilecode == stateLib.PROJECTILE_CODE_NO_TOOLS) {
				isLosing = true;
				losstime = Time.time + lossDelay;
				return;
			}
			if (toolCounts[projectilecode] <= 0 && bonusTools[projectilecode] <= 0 && !lg.isAnswering && lg.toolsAirborne <= 0) {
				NextTool();
			}
		}
		else {
            // Menu is up
			for (int i = 0; i < stateLib.NUMBER_OF_TOOLS; i++) {
			    toolIcons[i].GetComponent<GUITexture>().enabled = false;
			}
			isLosing = false;
			toolAvailableTools.GetComponent<GUIText>().text = "";
			toolLabels[stateLib.TOOL_CATCHER_OR_ACTIVATOR].GetComponent<GUIText>().text = "";
			toolLabels[stateLib.TOOL_PRINTER_OR_QUESTION].GetComponent<GUIText>().text = "";
			toolLabels[stateLib.TOOL_WARPER_OR_RENAMER].GetComponent<GUIText>().text = "";
			toolLabels[stateLib.TOOL_COMMENTER].GetComponent<GUIText>().text = "";
			toolLabels[stateLib.TOOL_CONTROL_FLOW].GetComponent<GUIText>().text = "";
			toolLabels[stateLib.TOOL_HELPER].GetComponent<GUIText>().text = "";
			toolLabels[stateLib.TOOL_CATCHER_OR_ACTIVATOR].GetComponent<GUIText>().enabled = true;
			toolLabels[stateLib.TOOL_PRINTER_OR_QUESTION].GetComponent<GUIText>().enabled = true;
			toolLabels[stateLib.TOOL_WARPER_OR_RENAMER].GetComponent<GUIText>().enabled = true;
			toolLabels[stateLib.TOOL_COMMENTER].GetComponent<GUIText>().enabled = true;
			toolLabels[stateLib.TOOL_CONTROL_FLOW].GetComponent<GUIText>().enabled = true;
			toolLabels[stateLib.TOOL_HELPER].GetComponent<GUIText>().enabled = true;
			levelDescription.GetComponent<GUIText>().text = "";
			taskComplete = new bool[stateLib.NUMBER_OF_TOOLS];
		}
	}

	//.................................>8.......................................
	public void NextTool() {
		int notoolcount = 0;
		// Turn this tool's color to the toolOff color.
		toolIcons[projectilecode].GetComponent<GUITexture>().color = toolOffColor;
		// If the checklist entry was is completed, then disable this current tool before switching to the next
		if (lg.tasklist[projectilecode] == lg.taskscompleted[projectilecode]) {
			taskComplete[projectilecode] = true;
			toolIcons[projectilecode].GetComponent<GUITexture>().enabled = false;
			toolLabels[projectilecode].GetComponent<GUIText>().enabled = false;
			toolCounts[projectilecode] = 0;
			bonusTools[projectilecode] = 0;
		}
		else if (toolCounts[projectilecode] + bonusTools[projectilecode] <= 0) {
			toolLabels[projectilecode].GetComponent<GUIText>().color = Color.red;
			toolLabels[projectilecode].GetComponent<GUIText>().text = "Out of Tools!";
			lg.isLosing = true;
		}
		// Cycle to the next tool.
		projectilecode = (projectilecode + 1) % stateLib.NUMBER_OF_TOOLS;
		// Count the number of empty tools from the set of tools.
		while(toolCounts[projectilecode] <= 0) {
			notoolcount++;
			projectilecode = (projectilecode + 1) % stateLib.NUMBER_OF_TOOLS;
			// Player has no useable tools
			if (notoolcount > stateLib.NUMBER_OF_TOOLS + 1) {
				projectilecode = stateLib.PROJECTILE_CODE_NO_TOOLS;
				break;
			}
		}
		// If we have no remaining tools, lose the game.
		if (projectilecode == stateLib.PROJECTILE_CODE_NO_TOOLS) {
			isLosing = true;
			losstime = Time.time + lossDelay;
		}
	}

	//.................................>8.......................................
	public void refreshToolList() {
	  if (toolCounts[stateLib.TOOL_CATCHER_OR_ACTIVATOR] + bonusTools[stateLib.TOOL_CATCHER_OR_ACTIVATOR] > 0) {
	    CheckTaskComplete(stateLib.TOOL_CATCHER_OR_ACTIVATOR);
	    displayString = (lg.gamemode == stringLib.GAME_MODE_BUG) ? stringLib.INTERFACE_TOOL_NAME_0_ROBOBUG : stringLib.INTERFACE_TOOL_NAME_0_ROBOTON;
	    toolLabels[stateLib.TOOL_CATCHER_OR_ACTIVATOR].GetComponent<GUIText>().text = (lg.sidebarToggle) ? displayString + " [" : "[";
	    toolLabels[stateLib.TOOL_CATCHER_OR_ACTIVATOR].GetComponent<GUIText>().text += ReplaceTextInfinite(toolCounts[stateLib.TOOL_CATCHER_OR_ACTIVATOR]) + ReplaceBonusText(stateLib.TOOL_CATCHER_OR_ACTIVATOR) + "]";
	  }
	  if (toolCounts[stateLib.TOOL_PRINTER_OR_QUESTION] + bonusTools[stateLib.TOOL_PRINTER_OR_QUESTION] > 0) {
	    CheckTaskComplete(stateLib.TOOL_PRINTER_OR_QUESTION);
	    displayString = (lg.gamemode == stringLib.GAME_MODE_BUG) ? stringLib.INTERFACE_TOOL_NAME_1_ROBOBUG : stringLib.INTERFACE_TOOL_NAME_1_ROBOTON;
	    toolLabels[stateLib.TOOL_PRINTER_OR_QUESTION].GetComponent<GUIText>().text = (lg.sidebarToggle) ? displayString + " [" : "[";
	    toolLabels[stateLib.TOOL_PRINTER_OR_QUESTION].GetComponent<GUIText>().text += ReplaceTextInfinite(toolCounts[stateLib.TOOL_PRINTER_OR_QUESTION]) + ReplaceBonusText(stateLib.TOOL_PRINTER_OR_QUESTION) + "]";
	  }
	  if (toolCounts[stateLib.TOOL_WARPER_OR_RENAMER] + bonusTools[stateLib.TOOL_WARPER_OR_RENAMER] > 0) {
	    CheckTaskComplete(stateLib.TOOL_WARPER_OR_RENAMER);
	    displayString = (lg.gamemode == stringLib.GAME_MODE_BUG) ? stringLib.INTERFACE_TOOL_NAME_2_ROBOBUG : stringLib.INTERFACE_TOOL_NAME_2_ROBOTON;
	    toolLabels[stateLib.TOOL_WARPER_OR_RENAMER].GetComponent<GUIText>().text = (lg.sidebarToggle) ? displayString + " [" : "[";
	    toolLabels[stateLib.TOOL_WARPER_OR_RENAMER].GetComponent<GUIText>().text += ReplaceTextInfinite(toolCounts[stateLib.TOOL_WARPER_OR_RENAMER]) + ReplaceBonusText(stateLib.TOOL_WARPER_OR_RENAMER) + "]";
	  }
	  if (toolCounts[stateLib.TOOL_COMMENTER] + bonusTools[stateLib.TOOL_COMMENTER] > 0) {
	    CheckTaskComplete(stateLib.TOOL_COMMENTER);
	    toolLabels[stateLib.TOOL_COMMENTER].GetComponent<GUIText>().text = (lg.sidebarToggle) ? stringLib.INTERFACE_TOOL_NAME_3 + " [" : "[";
	    toolLabels[stateLib.TOOL_COMMENTER].GetComponent<GUIText>().text += ReplaceTextInfinite(toolCounts[stateLib.TOOL_COMMENTER]) + ReplaceBonusText(stateLib.TOOL_COMMENTER) + "]";

	  }
	  if (toolCounts[stateLib.TOOL_CONTROL_FLOW] + bonusTools[stateLib.TOOL_CONTROL_FLOW] > 0) {
	    CheckTaskComplete(stateLib.TOOL_CONTROL_FLOW);
	    displayString = (lg.gamemode == stringLib.GAME_MODE_BUG) ? stringLib.INTERFACE_TOOL_NAME_4_ROBOBUG : stringLib.INTERFACE_TOOL_NAME_4_ROBOTON;
	    toolLabels[stateLib.TOOL_CONTROL_FLOW].GetComponent<GUIText>().text = (lg.sidebarToggle) ? displayString + " [" : "[";
	    toolLabels[stateLib.TOOL_CONTROL_FLOW].GetComponent<GUIText>().text += ReplaceTextInfinite(toolCounts[stateLib.TOOL_CONTROL_FLOW]) + ReplaceBonusText(stateLib.TOOL_CONTROL_FLOW) + "]";
	  }
	  if (toolCounts[stateLib.TOOL_HELPER] + bonusTools[stateLib.TOOL_HELPER] > 0) {
	    CheckTaskComplete(stateLib.TOOL_HELPER);
	    toolLabels[stateLib.TOOL_HELPER].GetComponent<GUIText>().text = (lg.sidebarToggle) ? stringLib.INTERFACE_TOOL_NAME_5 + " [" : "[";
	    toolLabels[stateLib.TOOL_HELPER].GetComponent<GUIText>().text += ReplaceTextInfinite(toolCounts[stateLib.TOOL_HELPER]) + ReplaceBonusText(stateLib.TOOL_HELPER) + "]";
	  }
	}
	//.................................>8.......................................
	private void CheckTaskComplete(int nToolCode) {
		if (lg.tasklist[nToolCode] == lg.taskscompleted[nToolCode] && !taskComplete[nToolCode] && lg.tasklist[nToolCode] == 0) {
			taskComplete[nToolCode] = true;
			for (int i = 0 ; i < 5 ; i++) {
				NextTool();
			}
			toolIcons[nToolCode].GetComponent<GUITexture>().enabled = false;
			toolLabels[nToolCode].GetComponent<GUIText>().enabled = false;
		}
		if (lg.tasklist[nToolCode] == lg.taskscompleted[nToolCode] && !taskComplete[nToolCode]) {
            taskComplete[nToolCode] = true;
            toolLabels[nToolCode].GetComponent<GUIText>().color = lg.backgroundLightDark == true ? new Color(0, 0.6f, 0.2f, 1) : Color.green;
            NextTool();
            outputtext.GetComponent<GUIText>().text = "Task Complete!";
            outputtext.GetComponent<AudioSource>().Play();
		}
		else if (lg.tasklist[nToolCode] != lg.taskscompleted[nToolCode]) {
            toolLabels[nToolCode].GetComponent<GUIText>().color = lg.backgroundLightDark == false ? Color.white : Color.black;
		}
	}

	//.................................>8.......................................
	private string ReplaceTextInfinite(int nToolCount) {
        return nToolCount >= 999 ? "--" : nToolCount.ToString();
	}

	//.................................>8.......................................
	private string ReplaceBonusText(int nToolIndex) {
		return bonusTools[nToolIndex] > 0 ? lg.stringLibrary.checklist_complete_color_tag + " +" + ReplaceTextInfinite(bonusTools[nToolIndex]) + stringLib.CLOSE_COLOR_TAG : "";
	}

	//.................................>8.......................................

}

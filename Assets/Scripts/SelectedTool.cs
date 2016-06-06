//**************************************************//
// Class Name: SelectedTool
// Class Description: Tool bar on right side of interface.
// Methods:
// 		void Start()
//		void Update()
//		public void NextTool()
// 		public void notifyToolAcquisition()
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
	public GameObject codescreen;
	public GameObject hero;
	public GameObject toolprompt;
	public GameObject toolLabel;
	public GameObject[] toolIcons = new GameObject[stateLib.NUMBER_OF_TOOLS];

	// Determine if the player has lost the game.
	private bool isLosing = false;
	// Determine the player has any remaining activavator tools (RoboBUG)
	private bool noRemainingActivators = false;
	// This is controlled in the projectile scripts, if a player should receive a new tool, notify them.
	private bool notifyNewToolsAcquired = false;
	private float lossDelay = 4f;
	private float losstime;
	private Color toolOnColor = new Color(.7f, .7f, .7f);
	private Color toolOffColor = new Color(.3f, .3f, .3f);
	private GUIText tm;
	private LevelGenerator lg;

	//.................................>8.......................................
	// Use this for initialization
	void Start() {
		tm = this.GetComponent<GUIText>();
		tm.text = "Bug Catcher";
		lg = codescreen.GetComponent<LevelGenerator>();
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
			toolLabel.GetComponent<GUIText>().text = "Available Tools:";

			if (notifyNewToolsAcquired) {
				Animator anim = toolprompt.GetComponent<Animator>();
				anim.Play("hide");
				notifyNewToolsAcquired = false;
			}
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
					toolIcons[i].GetComponent<GUITexture>().enabled = true;
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
			if (hero.GetComponent<hero2Controller>().throwing) {
				hero.GetComponent<hero2Controller>().throwing = false;
				// Decrease the remaining number of tools if tools are not infinite (999)
				if (toolCounts[projectilecode] < 999) {
					if (toolCounts[projectilecode] == 0) {
						bonusTools[projectilecode] -= 1;
					}
					else {
						toolCounts[projectilecode] -= 1;
					}
				}
				// RoboBUG: If we are out of activators, we've failed the game.
				if (projectilecode == 0 && toolCounts[0] == 0 && lg.gamemode == stringLib.GAME_MODE_BUG) {
					noRemainingActivators = true;
					losstime = Time.time + lossDelay;
				}
				// Out of tools, switch to the next one
				if (toolCounts[projectilecode] == 0 && bonusTools[projectilecode] == 0) {
					toolIcons[projectilecode].GetComponent<GUITexture>().enabled = false;
					NextTool();
				}
			}
			switch(projectilecode) {
				case 0:
					tm.color = Color.white;
					if (lg.gamemode == stringLib.GAME_MODE_BUG) {
						tm.text = "Bug Catcher: " + ReplaceTextInfinite(toolCounts[0]);
					}
					else {
						tm.text = "Activator: " + ReplaceTextInfinite(toolCounts[0]);
					}
					toolIcons[0].GetComponent<GUITexture>().color = toolOnColor;
					break;
				case 1:
					tm.color = Color.white;
					if (lg.gamemode == stringLib.GAME_MODE_BUG) {
						tm.text = "Printer: " + ReplaceTextInfinite(toolCounts[1]);
					}
					else {
						tm.text = "Checker: " + ReplaceTextInfinite(toolCounts[1]);
					}
					toolIcons[1].GetComponent<GUITexture>().color = toolOnColor;
					break;
				case 2:
					tm.color = Color.white;
					if (lg.gamemode == stringLib.GAME_MODE_BUG) {
						tm.text = "Warper: " + ReplaceTextInfinite(toolCounts[2]);
					}
					else {
						tm.text = "Namer: " + ReplaceTextInfinite(toolCounts[2]);
					}
					toolIcons[2].GetComponent<GUITexture>().color = toolOnColor;
					break;
				case 3:
					tm.color = Color.white;
					tm.text = "Commenter: " + ReplaceTextInfinite(toolCounts[3]);
					toolIcons[3].GetComponent<GUITexture>().color = toolOnColor;
					break;
				case 4:
					tm.color = Color.white;
					if (lg.gamemode == stringLib.GAME_MODE_BUG) {
						tm.text = "Breakpointer: " + ReplaceTextInfinite(toolCounts[4]);
					}
					else {
						tm.text = "Un-Commenter: " + ReplaceTextInfinite(toolCounts[4]);
					}
					toolIcons[4].GetComponent<GUITexture>().color = toolOnColor;
					break;
				case 5:
					tm.color = Color.white;
					tm.text = "Helper: " + ReplaceTextInfinite(toolCounts[5]);
					toolIcons[5].GetComponent<GUITexture>().color = toolOnColor;
					break;
				case -1:
					tm.color = Color.red;
					tm.text = "Out of Tools!!";
					break;
			}
			if (projectilecode >= 0 && bonusTools[projectilecode] > 0) {
				tm.text += " <color=#ff8800ff>+ " + bonusTools[projectilecode].ToString() + "</color>";
			}
		}
		else {
			for (int i = 0; i < stateLib.NUMBER_OF_TOOLS; i++) {
				toolIcons[i].GetComponent<GUITexture>().enabled = false;
			}
			tm.text = "";
			isLosing = false;
			toolLabel.GetComponent<GUIText>().text = "";
		}
	}

	//.................................>8.......................................
	public void NextTool() {
		int notoolcount = 0;
		// Turn this tool's color to the toolOff color.
		toolIcons[projectilecode].GetComponent<GUITexture>().color = toolOffColor;
		// Cycle to the next tool.
		projectilecode = (projectilecode + 1) % stateLib.NUMBER_OF_TOOLS;
		// Count the number of empty tools from the set of tools.
		while(!toolIcons[projectilecode].GetComponent<GUITexture>().enabled) {
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
	public void notifyToolAcquisition() {
		notifyNewToolsAcquired = true;
	}

	//.................................>8.......................................
	private string ReplaceTextInfinite(int nToolCount) {
		if (nToolCount == 999) {
			return "Infinite";
		}
		else {
			return nToolCount.ToString();
		}
	}

	//.................................>8.......................................
}

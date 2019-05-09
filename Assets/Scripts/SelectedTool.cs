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
using UnityEngine.UI; 
using System.Collections;

public class SelectedTool : MonoBehaviour
{
	public int projectilecode = 0;
	public int[] toolCounts = new int[stateLib.NUMBER_OF_TOOLS];
	public int[] bonusTools = new int[stateLib.NUMBER_OF_TOOLS];
	public GameObject[] toolLabels = new GameObject[stateLib.NUMBER_OF_TOOLS];
	public GameObject codescreen;
	GameObject hero;
	public GameObject toolprompt;
	public GameObject toolAvailableTools;
	public GameObject levelDescription;
  public GameObject outputtext;
	public GameObject[] toolIcons = new GameObject[stateLib.NUMBER_OF_TOOLS];

	// Determine if the player has lost the game.
	public bool isLosing = false;
	// Determine the player has any remaining activavator tools (RoboBUG)
	public bool noRemainingActivators = false;
	private float lossDelay = 4f;
	private float losstime;
	private Color toolOnColor = new Color(1,1,1);
	private Color toolOffColor = new Color(.3f, .3f, .3f);
	private LevelGenerator lg;
	private bool[] taskComplete = new bool[stateLib.NUMBER_OF_TOOLS];

    SidebarController sidebar; 
	private string displayString = "";

	//.................................>8.......................................
	// Use this for initialization
	void Start() {
        hero = GameObject.Find("Hero");
		lg = codescreen.GetComponent<LevelGenerator>();
		this.GetComponent<Text>().text = "";
        InitializeToolLabels();
        sidebar = GameObject.Find("Sidebar").GetComponent<SidebarController>(); 
	}
    private void Initialize()
    {
        isLosing = false;
        noRemainingActivators = false;
    }
    private void SetDisplayText()
    {
        toolAvailableTools.GetComponent<Text>().text = stringLib.INTERFACE_SIDEBAR_AVAILABLE_TOOLS;
    }
    private void CheckLosing()
    {
        if (isLosing || noRemainingActivators)
        {
            if (Time.time > losstime)
            {
                noRemainingActivators = false;
                isLosing = false;
            }
        }
    }
    private void CheckAvailableTools()
    {
        for (int i = 0; i < stateLib.NUMBER_OF_TOOLS; i++)
        {
            if (toolCounts[i] + bonusTools[i] > 0)
            {
                if (GlobalState.level.Tasks[i] != GlobalState.level.CompletedTasks[i])
                {
                    toolIcons[i].GetComponent<Image>().enabled = true;
                    toolLabels[i].GetComponent<Text>().enabled = true; 
                    toolLabels[i].GetComponent<Text>().color = (GlobalState.IsDark ? Color.white: Color.black); 
                    //Debug.Log("Updating Icons");
                }
                isLosing = false;
                if (projectilecode == stateLib.PROJECTILE_CODE_NO_TOOLS)
                {
                    projectilecode = i;
                }
            }
        }
    }

    private void HandleThrows()
    {
        // A projectile has been thrown by the player in hero2Controller
        if (hero.GetComponent<hero2Controller>().throwing)
        {
            hero.GetComponent<hero2Controller>().throwing = false;
            // Decrease the remaining number of tools if tools are not infinite (999)
            if (toolCounts[projectilecode] < 999)
            {
                if (bonusTools[projectilecode] > 0)
                {
                    bonusTools[projectilecode] -= 1;
                }
                else
                {
                    toolCounts[projectilecode] -= 1;
                }
            }
            // RoboBUG: If we are out of activators, we've failed the game.
            if (projectilecode == 0 && toolCounts[stateLib.TOOL_CATCHER_OR_ACTIVATOR] == 0 && GlobalState.GameMode == stringLib.GAME_MODE_BUG)
            {
                noRemainingActivators = true;
                toolIcons[stateLib.TOOL_CATCHER_OR_ACTIVATOR].GetComponent<Image>().enabled = false; 
                toolLabels[stateLib.TOOL_CATCHER_OR_ACTIVATOR].GetComponent<Text>().enabled = false; 
                losstime = Time.time + lossDelay;
            }
        }
        switch (projectilecode)
        {
            case stateLib.TOOL_CATCHER_OR_ACTIVATOR:
                refreshToolList();
                toolIcons[stateLib.TOOL_CATCHER_OR_ACTIVATOR].GetComponent<Image>().color = toolOnColor;
                break;
            case stateLib.TOOL_PRINTER_OR_QUESTION:
                refreshToolList();
                toolIcons[stateLib.TOOL_PRINTER_OR_QUESTION].GetComponent<Image>().color = toolOnColor;
                break;
            case stateLib.TOOL_WARPER_OR_RENAMER:
                refreshToolList();
                toolIcons[stateLib.TOOL_WARPER_OR_RENAMER].GetComponent<Image>().color = toolOnColor;
                break;
            case stateLib.TOOL_COMMENTER:
                refreshToolList();
                toolIcons[stateLib.TOOL_COMMENTER].GetComponent<Image>().color = toolOnColor;
                break;
            case stateLib.TOOL_CONTROL_FLOW:
                refreshToolList();
                toolIcons[stateLib.TOOL_CONTROL_FLOW].GetComponent<Image>().color = toolOnColor;
                break;
            case stateLib.TOOL_HELPER:
                refreshToolList();
                toolIcons[stateLib.TOOL_HELPER].GetComponent<Image>().color = toolOnColor;
                break;
            default:
                break;
        }
        if (projectilecode == stateLib.PROJECTILE_CODE_NO_TOOLS)
        {
            isLosing = true;
            losstime = Time.time + lossDelay;
            return;
        }
        if (toolCounts[projectilecode] <= 0 && bonusTools[projectilecode] <= 0 && !Output.IsAnswering && GameObject.FindGameObjectsWithTag("Projectile").Length == 0)
        {
            NextTool();
        }
    }
    private void InitializeToolLabels()
    {
        for (int i = 0; i < toolIcons.Length; i++)
        {
            toolLabels[i] = toolIcons[i].transform.GetChild(0).gameObject;
        }
    }
    private void MenuIsUp()
    {
        // Menu is up
        for (int i = 0; i < stateLib.NUMBER_OF_TOOLS; i++)
        {
            toolIcons[i].GetComponent<Image>().enabled = false;
        }
        isLosing = false;
        toolAvailableTools.GetComponent<Text>().text = "";
        /* 
        toolLabels[stateLib.TOOL_CATCHER_OR_ACTIVATOR].GetComponent<Text>().text = "";
        toolLabels[stateLib.TOOL_PRINTER_OR_QUESTION].GetComponent<Text>().text = "";
        toolLabels[stateLib.TOOL_WARPER_OR_RENAMER].GetComponent<Text>().text = "";
        toolLabels[stateLib.TOOL_COMMENTER].GetComponent<Text>().text = "";
        toolLabels[stateLib.TOOL_CONTROL_FLOW].GetComponent<Text>().text = "";
        toolLabels[stateLib.TOOL_HELPER].GetComponent<Text>().text = "";
        */
        toolLabels[stateLib.TOOL_CATCHER_OR_ACTIVATOR].GetComponent<Text>().enabled = false;
        toolLabels[stateLib.TOOL_PRINTER_OR_QUESTION].GetComponent<Text>().enabled = false;
        toolLabels[stateLib.TOOL_WARPER_OR_RENAMER].GetComponent<Text>().enabled = false;
        toolLabels[stateLib.TOOL_COMMENTER].GetComponent<Text>().enabled = false;
        toolLabels[stateLib.TOOL_CONTROL_FLOW].GetComponent<Text>().enabled = false;
        toolLabels[stateLib.TOOL_HELPER].GetComponent<Text>().enabled = false;
        levelDescription.GetComponent<Text>().text = "";
        taskComplete = new bool[stateLib.NUMBER_OF_TOOLS];
    }

    //.................................>8.......................................
    // Update is called once per frame
    void Update() {
        if (GlobalState.GameState >= stateLib.GAMESTATE_LEVEL_START)
        {
            Initialize();
        }
        if (GlobalState.GameState == stateLib.GAMESTATE_IN_GAME)
        {
            SetDisplayText();
            CheckLosing();
            CheckAvailableTools();
            hero.GetComponent<hero2Controller>().projectilecode = projectilecode;
            // Pressing Tab cycles to the next tool
            if (Input.GetKeyDown("tab") && projectilecode >= 0)
            {
                NextTool();
            }
            HandleThrows();
        }
        else
        {
            MenuIsUp();
        }
    }

	//.................................>8.......................................
	public void NextTool() {
        int notoolcount = 0;
        // Turn this tool's color to the toolOff color.
        print("projectilecode is " + projectilecode.ToString());
        toolIcons[projectilecode].GetComponent<Image>().color = toolOffColor;
        // If the checklist entry was is completed, then disable this current tool before switching to the next
        if (GlobalState.level.Tasks[projectilecode] == GlobalState.level.CompletedTasks[projectilecode] && GlobalState.GameMode == stringLib.GAME_MODE_ON)
        {
            taskComplete[projectilecode] = true;
            toolIcons[projectilecode].GetComponent<Image>().enabled = false;
            toolLabels[projectilecode].GetComponent<Text>().enabled = false;
            
            toolCounts[projectilecode] = 0;
            bonusTools[projectilecode] = 0;
        }
        else if (toolCounts[projectilecode] + bonusTools[projectilecode] <= 0)
        {
            Debug.Log(toolCounts[projectilecode]);
            toolLabels[projectilecode].GetComponent<Text>().color = Color.red;
            toolLabels[projectilecode].GetComponent<Text>().text = stringLib.INTERFACE_SIDEBAR_OUT_OF_TOOLS;
            isLosing = true;
        }
        // Cycle to the next tool.
        projectilecode = (projectilecode + 1) % stateLib.NUMBER_OF_TOOLS;
        // Count the number of empty tools from the set of tools.
        while (toolCounts[projectilecode] <= 0)
        {
            notoolcount++;
            projectilecode = (projectilecode + 1) % stateLib.NUMBER_OF_TOOLS;
            // Player has no useable tools
            if (notoolcount > stateLib.NUMBER_OF_TOOLS + 1)
            {
                projectilecode = stateLib.PROJECTILE_CODE_NO_TOOLS;
                break;
            }
        }
        // If we have no remaining tools, lose the game.
        if (projectilecode == stateLib.PROJECTILE_CODE_NO_TOOLS)
        {
            isLosing = true;
            losstime = Time.time + lossDelay;
        }
    }

	//.................................>8.......................................
	public void refreshToolList() {
	  if (toolCounts[stateLib.TOOL_CATCHER_OR_ACTIVATOR] + bonusTools[stateLib.TOOL_CATCHER_OR_ACTIVATOR] > 0) {
	    CheckTaskComplete(stateLib.TOOL_CATCHER_OR_ACTIVATOR);
	    displayString = (GlobalState.GameMode == stringLib.GAME_MODE_BUG) ? stringLib.INTERFACE_TOOL_NAME_0_ROBOBUG : stringLib.INTERFACE_TOOL_NAME_0_ROBOTON;
	    toolLabels[stateLib.TOOL_CATCHER_OR_ACTIVATOR].GetComponent<Text>().text = (sidebar.isActive) ? displayString + " [" : "[";
	    toolLabels[stateLib.TOOL_CATCHER_OR_ACTIVATOR].GetComponent<Text>().text += ReplaceTextInfinite(toolCounts[stateLib.TOOL_CATCHER_OR_ACTIVATOR]) + ReplaceBonusText(stateLib.TOOL_CATCHER_OR_ACTIVATOR) + "]";
	  }
	  if (toolCounts[stateLib.TOOL_PRINTER_OR_QUESTION] + bonusTools[stateLib.TOOL_PRINTER_OR_QUESTION] > 0) {
	    CheckTaskComplete(stateLib.TOOL_PRINTER_OR_QUESTION);
	    displayString = (GlobalState.GameMode == stringLib.GAME_MODE_BUG) ? stringLib.INTERFACE_TOOL_NAME_1_ROBOBUG : stringLib.INTERFACE_TOOL_NAME_1_ROBOTON;
	    toolLabels[stateLib.TOOL_PRINTER_OR_QUESTION].GetComponent<Text>().text = (sidebar.isActive) ? displayString + " [" : "[";
	    toolLabels[stateLib.TOOL_PRINTER_OR_QUESTION].GetComponent<Text>().text += ReplaceTextInfinite(toolCounts[stateLib.TOOL_PRINTER_OR_QUESTION]) + ReplaceBonusText(stateLib.TOOL_PRINTER_OR_QUESTION) + "]";
	  }
	  if (toolCounts[stateLib.TOOL_WARPER_OR_RENAMER] + bonusTools[stateLib.TOOL_WARPER_OR_RENAMER] > 0) {
	    CheckTaskComplete(stateLib.TOOL_WARPER_OR_RENAMER);
	    displayString = (GlobalState.GameMode == stringLib.GAME_MODE_BUG) ? stringLib.INTERFACE_TOOL_NAME_2_ROBOBUG : stringLib.INTERFACE_TOOL_NAME_2_ROBOTON;
	    toolLabels[stateLib.TOOL_WARPER_OR_RENAMER].GetComponent<Text>().text = (sidebar.isActive) ? displayString + " [" : "[";
	    toolLabels[stateLib.TOOL_WARPER_OR_RENAMER].GetComponent<Text>().text += ReplaceTextInfinite(toolCounts[stateLib.TOOL_WARPER_OR_RENAMER]) + ReplaceBonusText(stateLib.TOOL_WARPER_OR_RENAMER) + "]";
	  }
	  if (toolCounts[stateLib.TOOL_COMMENTER] + bonusTools[stateLib.TOOL_COMMENTER] > 0) {
	    CheckTaskComplete(stateLib.TOOL_COMMENTER);
	    toolLabels[stateLib.TOOL_COMMENTER].GetComponent<Text>().text = (sidebar.isActive) ? stringLib.INTERFACE_TOOL_NAME_3 + " [" : "[";
	    toolLabels[stateLib.TOOL_COMMENTER].GetComponent<Text>().text += ReplaceTextInfinite(toolCounts[stateLib.TOOL_COMMENTER]) + ReplaceBonusText(stateLib.TOOL_COMMENTER) + "]";

	  }
	  if (toolCounts[stateLib.TOOL_CONTROL_FLOW] + bonusTools[stateLib.TOOL_CONTROL_FLOW] > 0) {
	    CheckTaskComplete(stateLib.TOOL_CONTROL_FLOW);
	    displayString = (GlobalState.GameMode == stringLib.GAME_MODE_BUG) ? stringLib.INTERFACE_TOOL_NAME_4_ROBOBUG : stringLib.INTERFACE_TOOL_NAME_4_ROBOTON;
	    toolLabels[stateLib.TOOL_CONTROL_FLOW].GetComponent<Text>().text = (sidebar.isActive) ? displayString + " [" : "[";
	    toolLabels[stateLib.TOOL_CONTROL_FLOW].GetComponent<Text>().text += ReplaceTextInfinite(toolCounts[stateLib.TOOL_CONTROL_FLOW]) + ReplaceBonusText(stateLib.TOOL_CONTROL_FLOW) + "]";
	  }
	  if (toolCounts[stateLib.TOOL_HELPER] + bonusTools[stateLib.TOOL_HELPER] > 0) {
	    CheckTaskComplete(stateLib.TOOL_HELPER);
	    toolLabels[stateLib.TOOL_HELPER].GetComponent<Text>().text = (sidebar.isActive) ? stringLib.INTERFACE_TOOL_NAME_5 + " [" : "[";
	    toolLabels[stateLib.TOOL_HELPER].GetComponent<Text>().text += ReplaceTextInfinite(toolCounts[stateLib.TOOL_HELPER]) + ReplaceBonusText(stateLib.TOOL_HELPER) + "]";
	  }
	}
	//.................................>8.......................................
	private void CheckTaskComplete(int nToolCode) {
        if (GlobalState.GameMode == stringLib.GAME_MODE_BUG)
            return; 
		if (GlobalState.level.Tasks[nToolCode] == GlobalState.level.CompletedTasks[nToolCode] && !taskComplete[nToolCode] && GlobalState.level.Tasks[nToolCode] == 0) {
			taskComplete[nToolCode] = true;
			for (int i = 0 ; i < 5 ; i++) {
				NextTool();
			}
			toolIcons[nToolCode].GetComponent<Image>().enabled = false;
			toolLabels[nToolCode].GetComponent<Text>().enabled = false;
		}
		if (GlobalState.level.Tasks[nToolCode] == GlobalState.level.CompletedTasks[nToolCode] && !taskComplete[nToolCode]) {
            taskComplete[nToolCode] = true;
            toolLabels[nToolCode].GetComponent<Text>().color = GlobalState.IsDark == true ? new Color(0, 0.6f, 0.2f, 1) : Color.green;
            NextTool();
            outputtext.GetComponent<Text>().text = stringLib.INTERFACE_TASK_COMPLETE;
            outputtext.GetComponent<AudioSource>().Play();
		}
		else if (GlobalState.level.Tasks[nToolCode] != GlobalState.level.Tasks[nToolCode]) {
            toolLabels[nToolCode].GetComponent<Text>().color = GlobalState.IsDark == false ? Color.white : Color.black;
		}
	}

	//.................................>8.......................................
	private string ReplaceTextInfinite(int nToolCount) {
        return nToolCount >= 999 ? "--" : nToolCount.ToString();
	}

	//.................................>8.......................................
	private string ReplaceBonusText(int nToolIndex) {
		return bonusTools[nToolIndex] > 0 ? GlobalState.StringLib.checklist_complete_color_tag + " +" + ReplaceTextInfinite(bonusTools[nToolIndex]) + stringLib.CLOSE_COLOR_TAG : "";
	}

	//.................................>8.......................................

}

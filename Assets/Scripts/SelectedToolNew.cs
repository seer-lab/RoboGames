using UnityEngine;
using UnityEngine.UI; 
using System.Collections;

public partial class SelectedTool : MonoBehaviour
{
    private void Initialize()
    {
        isLosing = false;
        noRemainingActivators = false; 
    }
    private void SetDisplayText()
    {
        toolAvailableTools.GetComponent<Text>().text = stringLib.INTERFACE_SIDEBAR_AVAILABLE_TOOLS;
        levelDescription.GetComponent<Text>().text = lg.destext.GetComponent<TextMesh>().text;
    }
    private void CheckLosing()
    {
        if (isLosing || noRemainingActivators)
        {
            if (Time.time > losstime)
            {
                noRemainingActivators = false;
                isLosing = false;
                lg.isLosing = true;
            }
        }
    }
    private void CheckAvailableTools()
    {
        for (int i = 0; i < stateLib.NUMBER_OF_TOOLS; i++)
        {
            if (toolCounts[i] + bonusTools[i] > 0)
            {
                if (lg.tasklist[i] != lg.taskscompleted[i])
                {
                    toolIcons[i].GetComponent<Image>().enabled = true;
                }
                isLosing = false;
                if (projectilecode == stateLib.PROJECTILE_CODE_NO_TOOLS)
                {
                    projectilecode = i;
                }
            }
        }
    }
    public void NextToolNew()
    {
        int notoolcount = 0;
        // Turn this tool's color to the toolOff color.
        print("projectilecode is " + projectilecode.ToString());
        toolIcons[projectilecode].GetComponent<Image>().color = toolOffColor;
        // If the checklist entry was is completed, then disable this current tool before switching to the next
        if (lg.tasklist[projectilecode] == lg.taskscompleted[projectilecode])
        {
            taskComplete[projectilecode] = true;
            toolIcons[projectilecode].GetComponent<Image>().enabled = false;
            toolLabels[projectilecode].GetComponent<Text>().enabled = false;
            toolCounts[projectilecode] = 0;
            bonusTools[projectilecode] = 0;
        }
        else if (toolCounts[projectilecode] + bonusTools[projectilecode] <= 0)
        {
            toolLabels[projectilecode].GetComponent<Text>().color = Color.red;
            toolLabels[projectilecode].GetComponent<Text>().text = stringLib.INTERFACE_SIDEBAR_OUT_OF_TOOLS;
            lg.isLosing = true;
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
    private void HandleThrows()
    {
        // A projectile has been thrown by the player in hero2Controller
        if (hero.GetComponent<hero2Controller>().throwing)
        {
            lg.toolsAirborne++;
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
            if (projectilecode == 0 && toolCounts[stateLib.TOOL_CATCHER_OR_ACTIVATOR] == 0 && lg.gamemode == stringLib.GAME_MODE_BUG)
            {
                noRemainingActivators = true;
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
        if (toolCounts[projectilecode] <= 0 && bonusTools[projectilecode] <= 0 && !lg.isAnswering && lg.toolsAirborne <= 0)
        {
            NextTool();
        }
    }
    private void InitializeToolLabels()
    {
        for(int i = 0; i < toolIcons.Length; i++)
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
        toolLabels[stateLib.TOOL_CATCHER_OR_ACTIVATOR].GetComponent<Text>().text = "";
        toolLabels[stateLib.TOOL_PRINTER_OR_QUESTION].GetComponent<Text>().text = "";
        toolLabels[stateLib.TOOL_WARPER_OR_RENAMER].GetComponent<Text>().text = "";
        toolLabels[stateLib.TOOL_COMMENTER].GetComponent<Text>().text = "";
        toolLabels[stateLib.TOOL_CONTROL_FLOW].GetComponent<Text>().text = "";
        toolLabels[stateLib.TOOL_HELPER].GetComponent<Text>().text = "";
        toolLabels[stateLib.TOOL_CATCHER_OR_ACTIVATOR].GetComponent<Text>().enabled = true;
        toolLabels[stateLib.TOOL_PRINTER_OR_QUESTION].GetComponent<Text>().enabled = true;
        toolLabels[stateLib.TOOL_WARPER_OR_RENAMER].GetComponent<Text>().enabled = true;
        toolLabels[stateLib.TOOL_COMMENTER].GetComponent<Text>().enabled = true;
        toolLabels[stateLib.TOOL_CONTROL_FLOW].GetComponent<Text>().enabled = true;
        toolLabels[stateLib.TOOL_HELPER].GetComponent<Text>().enabled = true;
        levelDescription.GetComponent<Text>().text = "";
        taskComplete = new bool[stateLib.NUMBER_OF_TOOLS];
    }
    private void UpdateProtocol()
    {
        if (lg.gamestate >= stateLib.GAMESTATE_LEVEL_START)
        {
            Initialize();
        }
        if (lg.gamestate == stateLib.GAMESTATE_IN_GAME)
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
}



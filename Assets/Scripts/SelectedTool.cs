
using System.Linq;
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
    private bool isBugActive = false;
    SidebarController sidebar; 
	private string displayString = "";
    EnergyController energy; 
    Vector3[] positions; 
    bool firstUpdate = true; 

	//.................................>8.......................................
	// Use this for initialization
	void Start() {
        isLosing = false;
        noRemainingActivators = false;
        toolCounts =new int[stateLib.NUMBER_OF_TOOLS];
        bonusTools = new int[stateLib.NUMBER_OF_TOOLS];
        hero = GameObject.Find("Hero");
		lg = codescreen.GetComponent<LevelGenerator>();
		this.GetComponent<Text>().text = "";
        InitializeToolLabels();
        sidebar = GameObject.Find("Sidebar").GetComponent<SidebarController>(); 
        energy = GameObject.Find("Energy").GetComponent<EnergyController>();
	}
    private void MoveTools(){
        int counter = 0; 
        //Debug.Log("--------------------------------------------------------");
        for (int i = 0; i < positions.Length; i++){
            if (toolIcons[i].GetComponent<Image>().enabled){
                //Debug.Log(toolIcons[i].name + " " + positions[counter].ToString()) ;
                toolIcons[i].GetComponent<RectTransform>().localPosition = positions[counter]; 
                counter++; 
            }//else Debug.Log( positions[counter].ToString()); 
        }
    }
    public bool CheckAllToolsUsed(){
        for (int i = 0; i < toolCounts.Length; i++){
            if (toolCounts[i] > 0) return false; 
        }
        return true; 
    }
    private void SetDisplayText()
    {
        toolAvailableTools.GetComponent<Text>().text = stringLib.INTERFACE_SIDEBAR_AVAILABLE_TOOLS;
    }
    public void onClick(int index){
        if (!GlobalState.level.IsDemo)
            NextTool(index); 
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
        for (int i = 0; i < toolCounts.Length; i++)
        {
            if (toolCounts[i] + bonusTools[i] > 0)
            {
                if(i!= stateLib.TOOL_HINTER){
             
                    if (GlobalState.level.Tasks[i] != GlobalState.level.CompletedTasks[i] || (i == 0 && GlobalState.GameMode == "bug" && toolCounts[i] != 0))
                    {
                        if (energy.currentEnergy < energy.throwEnergy[i] && !(i == 0 && GlobalState.GameMode == "bug")){
                            toolIcons[i].GetComponent<Image>().enabled = false;
                            toolLabels[i].GetComponent<Text>().enabled = false; 
                        }
                        else{
                        toolIcons[i].GetComponent<Image>().enabled = true;
                        toolLabels[i].GetComponent<Text>().enabled = true; 
                        toolLabels[i].GetComponent<Text>().color = (GlobalState.IsDark ? Color.white: Color.black); 
                        }
                    }
                    isLosing = false;
                    noRemainingActivators = false; 
                    if (projectilecode == stateLib.PROJECTILE_CODE_NO_TOOLS)
                    {
                        projectilecode = i;
                    }
                }
            }
            
        }
        MoveTools(); 
    }

    private void HandleThrows()
    {
        // A projectile has been thrown by the player in hero2Controller
        if (hero.GetComponent<hero2Controller>().throwing)
        {
            hero.GetComponent<hero2Controller>().throwing = false;
            // Decrease the remaining number of tools if tools are not infinite (999)
            // RoboBUG: If we are out of activators, we've failed the game.
            if (projectilecode == 0 && toolCounts[stateLib.TOOL_CATCHER_OR_CONTROL_FLOW] == 0 && GlobalState.GameMode == stringLib.GAME_MODE_BUG)
            {
                noRemainingActivators = true;
                toolIcons[stateLib.TOOL_CATCHER_OR_CONTROL_FLOW].GetComponent<Image>().enabled = false; 
                toolLabels[stateLib.TOOL_CATCHER_OR_CONTROL_FLOW].GetComponent<Text>().enabled = false; 
                losstime = Time.time + lossDelay;
            }
        }
        switch (projectilecode)
        {
            case stateLib.TOOL_CATCHER_OR_CONTROL_FLOW:
                refreshToolList();
                toolIcons[stateLib.TOOL_CATCHER_OR_CONTROL_FLOW].GetComponent<Animator>().SetBool("enabled", true); 
                break;
            case stateLib.TOOL_PRINTER_OR_QUESTION:
                refreshToolList();
                toolIcons[stateLib.TOOL_PRINTER_OR_QUESTION].GetComponent<Animator>().SetBool("enabled", true); 
                break;
            case stateLib.TOOL_WARPER_OR_RENAMER:
                refreshToolList();
                toolIcons[stateLib.TOOL_WARPER_OR_RENAMER].GetComponent<Animator>().SetBool("enabled", true); 
                break;
            case stateLib.TOOL_COMMENTER:
                refreshToolList();
                toolIcons[stateLib.TOOL_COMMENTER].GetComponent<Animator>().SetBool("enabled", true); 
                break;
            case stateLib.TOOL_UNCOMMENTER:
                refreshToolList();
                toolIcons[stateLib.TOOL_UNCOMMENTER].GetComponent<Animator>().SetBool("enabled", true); 
                break;
            case stateLib.TOOL_HELPER:
                refreshToolList();
                toolIcons[stateLib.TOOL_HELPER].GetComponent<Animator>().SetBool("enabled", true); 
                break;
            case stateLib.TOOL_HINTER:
                refreshToolList();
                toolIcons[stateLib.TOOL_HINTER].GetComponent<Animator>().SetBool("enabled", true); 
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
            if (GlobalState.GameMode == stringLib.GAME_MODE_ON && i < GlobalState.StringLib.onIcons.Length){
                toolIcons[i].GetComponent<Image>().overrideSprite = Resources.Load<Sprite>("Sprites/icons/" + GlobalState.StringLib.onIcons[i]); 

            }
            else if (GlobalState.GameMode == stringLib.GAME_MODE_BUG && i < GlobalState.StringLib.bugIcons.Length){
                toolIcons[i].GetComponent<Image>().overrideSprite = Resources.Load<Sprite>("Sprites/icons/" + GlobalState.StringLib.bugIcons[i]); 
            }
        }
    }

    //.................................>8.......................................
    // Update is called once per frame
    void Update() {
        if (GlobalState.GameState == stateLib.GAMESTATE_IN_GAME)
        {
            if (firstUpdate){
                positions = new Vector3[toolIcons.Length]; 
        for (int i = 0; i < positions.Length; i++){
            positions[i] = toolIcons[i].GetComponent<RectTransform>().localPosition;  
        }
         positions = positions.ToList().OrderBy(e => e.y).Reverse().ToArray(); 
        firstUpdate = false; 
            }
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
    }

	//.................................>8.......................................
	public void NextTool(int index = -1) {
        int notoolcount = 0;
        // Turn this tool's color to the toolOff color.
        toolIcons[projectilecode].GetComponent<Animator>().SetBool("enabled", false);

        //Check if its the hinter tool and if it is ignore the next statement
        if(projectilecode != stateLib.TOOL_HINTER){
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
                if (GlobalState.GameMode == "on")
                isLosing = true;
            }

        }else{
            if(toolCounts[stateLib.TOOL_HINTER] + bonusTools[stateLib.TOOL_HINTER] <=0){
                Debug.Log(toolCounts[projectilecode]);
                toolLabels[projectilecode].GetComponent<Text>().color = Color.red;
                toolLabels[projectilecode].GetComponent<Text>().text = stringLib.INTERFACE_SIDEBAR_OUT_OF_TOOLS;

            }
        }
        if (index == -1)
            projectilecode = (projectilecode + 1) % stateLib.NUMBER_OF_TOOLS;
        else 
            projectilecode = index;
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
	  if (toolCounts[stateLib.TOOL_CATCHER_OR_CONTROL_FLOW] + bonusTools[stateLib.TOOL_CATCHER_OR_CONTROL_FLOW] > 0) {
	    CheckTaskComplete(stateLib.TOOL_CATCHER_OR_CONTROL_FLOW);
	    displayString = (GlobalState.GameMode == stringLib.GAME_MODE_BUG) ? stringLib.INTERFACE_TOOL_NAME_0_ROBOBUG : stringLib.INTERFACE_TOOL_NAME_0_ROBOTON;
	    toolLabels[stateLib.TOOL_CATCHER_OR_CONTROL_FLOW].GetComponent<Text>().text =displayString;
	  }
	  if (toolCounts[stateLib.TOOL_PRINTER_OR_QUESTION] + bonusTools[stateLib.TOOL_PRINTER_OR_QUESTION] > 0) {
	    CheckTaskComplete(stateLib.TOOL_PRINTER_OR_QUESTION);
	    displayString = (GlobalState.GameMode == stringLib.GAME_MODE_BUG) ? stringLib.INTERFACE_TOOL_NAME_1_ROBOBUG : stringLib.INTERFACE_TOOL_NAME_1_ROBOTON;
	    toolLabels[stateLib.TOOL_PRINTER_OR_QUESTION].GetComponent<Text>().text =  displayString ;
	  }
	  if (toolCounts[stateLib.TOOL_WARPER_OR_RENAMER] + bonusTools[stateLib.TOOL_WARPER_OR_RENAMER] > 0) {
	    CheckTaskComplete(stateLib.TOOL_WARPER_OR_RENAMER);
	    displayString = (GlobalState.GameMode == stringLib.GAME_MODE_BUG) ? stringLib.INTERFACE_TOOL_NAME_2_ROBOBUG : stringLib.INTERFACE_TOOL_NAME_2_ROBOTON;
	    toolLabels[stateLib.TOOL_WARPER_OR_RENAMER].GetComponent<Text>().text = displayString;
	  }
	  if (toolCounts[stateLib.TOOL_COMMENTER] + bonusTools[stateLib.TOOL_COMMENTER] > 0) {
	    CheckTaskComplete(stateLib.TOOL_COMMENTER);
	    toolLabels[stateLib.TOOL_COMMENTER].GetComponent<Text>().text =(GlobalState.GameMode == stringLib.GAME_MODE_BUG)? stringLib.INTERFACE_TOOL_NAME_3_ROBOBUG : stringLib.INTERFACE_TOOL_NAME_3_ROBOTON ;
	  }
	  if (toolCounts[stateLib.TOOL_UNCOMMENTER] + bonusTools[stateLib.TOOL_UNCOMMENTER] > 0) {
	    CheckTaskComplete(stateLib.TOOL_UNCOMMENTER);
	    displayString = (GlobalState.GameMode == stringLib.GAME_MODE_BUG) ? stringLib.INTERFACE_TOOL_NAME_4_ROBOBUG : stringLib.INTERFACE_TOOL_NAME_4_ROBOTON;
	    toolLabels[stateLib.TOOL_UNCOMMENTER].GetComponent<Text>().text =  displayString;
	  }
	  if (toolCounts[stateLib.TOOL_HELPER] + bonusTools[stateLib.TOOL_HELPER] > 0) {
	    CheckTaskComplete(stateLib.TOOL_HELPER);
	    toolLabels[stateLib.TOOL_HELPER].GetComponent<Text>().text =stringLib.INTERFACE_TOOL_NAME_5; 
	  }
      if(toolCounts[stateLib.TOOL_HINTER] + bonusTools[stateLib.TOOL_HINTER] > 0){
          toolLabels[stateLib.TOOL_HINTER].GetComponent<Text>().text = stringLib.INTERFACE_TOOL_NAME_6 ;
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
		else if (GlobalState.level.Tasks[nToolCode] != GlobalState.level.CompletedTasks[nToolCode]) {
            toolLabels[nToolCode].GetComponent<Text>().color = (GlobalState.IsDark) ? Color.white: Color.black;
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

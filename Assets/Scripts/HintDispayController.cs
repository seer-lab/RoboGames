using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HintDispayController : MonoBehaviour
{
    public int projectilecode = 0;
    public int usedprojectile = 0;
    public string hints;
    public int[] toolCounts = new int[stateLib.NUMBER_OF_TOOLS];
    public int[] bonusTools = new int[stateLib.NUMBER_OF_TOOLS];
    protected Output output;
    public GameObject toolAvailableTools;
    public GameObject hint;

    private LevelGenerator lg;
    SidebarController sidebar;

    private bool isButtonPressed = false;

    // Use this for initialization
    void Start() {
        this.GetComponent<Text>().text = "";
        sidebar = GameObject.Find("Sidebar").GetComponent<SidebarController>();
        output = GameObject.Find("OutputCanvas").transform.GetChild(0).GetComponent<Output>();
        Debug.Log(message: GlobalState.level.Hint);
        hints = GlobalState.level.Hint;
        
    }

    // Update is called once per frame
    void Update()
    {
        //Idea, if the user has used one of its tool, display a hint
        //Testing
        if(Input.GetKeyDown("h") && isButtonPressed == false){
            output.Text.text = hints;
            isButtonPressed = true;
        }else if(Input.GetKeyDown("enter") || Input.GetKeyDown("h")){
            output.Text.text = ""; 
            isButtonPressed = false;
        }
        checkCurrentTools();

    }
    // private void CheckAvailableTools() {
    //     for (int i = 0; i < stateLib.NUMBER_OF_TOOLS; i++) {
    //         if (toolCounts[i] + bonusTools[i] > 0) {
    //             if (projectilecode == stateLib.PROJECTILE_CODE_NO_TOOLS) {
    //                 projectilecode = i;
    //             }
    //         }
    //     }
    // }

    private void checkCurrentTools(){
        for(int i = 0; i < stateLib.NUMBER_OF_TOOLS; i++){
            if(toolCounts[i] + bonusTools[i] > 0){
                if(projectilecode == stateLib.PROJECTILE_CODE_NO_TOOLS){
                    projectilecode = i;
                }
            }

        }
    }
}

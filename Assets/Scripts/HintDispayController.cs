using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HintDispayController : MonoBehaviour
{
    public int projectilecode = 0;
    public int usedprojectile = 0;
    public int[] toolCounts = new int[stateLib.NUMBER_OF_TOOLS];
    public int[] bonusTools = new int[stateLib.NUMBER_OF_TOOLS];
    public GameObject[] toolLabels = new GameObject[stateLib.NUMBER_OF_TOOLS];
    public GameObject codescreen;
    public GameObject toolAvailableTools;

    private LevelGenerator lg;

    SidebarController sidebar;

    // Use this for initialization
    void Start() {
        lg = codescreen.GetComponent<LevelGenerator>();
        this.GetComponent<Text>().text = "";
        sidebar = GameObject.Find("Sidebar").GetComponent<SidebarController>();

        CheckAvailableTools();
        if(projectilecode == stateLib.PROJECTILE_CODE_NO_TOOLS) {

        }
    }

    // Update is called once per frame
    void Update()
    {
        //Idea, if the user has used one of its tool, display a hint

        CheckAvailableTools();

        if(GlobalState.GameState == stateLib.GAMESTATE_IN_GAME && (usedprojectile < projectilecode)) {
            Debug.Log("Show Hint here");
        }
    }
    private void CheckAvailableTools() {
        for (int i = 0; i < stateLib.NUMBER_OF_TOOLS; i++) {
            if (toolCounts[i] + bonusTools[i] > 0) {
                if (projectilecode == stateLib.PROJECTILE_CODE_NO_TOOLS) {
                    projectilecode = i;
                }
            }
        }
    }
}

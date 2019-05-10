using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HintDispayController : MonoBehaviour
{
    public string hints;
    protected Output output;

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
        if(hints.Equals("")){
            hints = "fake hints";
            output.Text.text = hints;
        }
        
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

    }

}

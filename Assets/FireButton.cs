using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class FireButton : MonoBehaviour
{
    Color[] colours; 
    string[] icons; 
    hero2Controller hero; 
    public GameObject toolObject; 
    private SelectedTool tool; 
    int code = -1; 
    void Start(){
        hero = GameObject.Find("Hero").GetComponent<hero2Controller>(); 
        tool = toolObject.GetComponent<SelectedTool>(); 
        colours = new Color[]{Color.white, Color.yellow, Color.magenta, Color.green,
        Color.red, Color.blue, Color.cyan};
        icons = new string[]{"bugcatcher", "activator",  "warp","comment", "breakpoint","help", "help"};
        code = tool.projectilecode; 
        UpdateLook();
    }
    void UpdateLook(){
        GetComponent<Image>().color = colours[code];
        GameObject.Find("Image").GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/" + icons[code]);
    }
    void Update(){
        if (code != tool.projectilecode){
            code = tool.projectilecode; 
            UpdateLook();
        }
    }
    public void onClick(){
        hero.ThrowTool(); 
    }
}

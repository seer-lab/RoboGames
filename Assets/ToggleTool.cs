using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using  UnityEngine.UI; 

public class ToggleTool : MonoBehaviour
{
    string[] icons; 
    int currentIndex; 
    SelectedTool tool; 
    // Start is called before the first frame update
    void Start()
    {
        icons = new string[]{"bugcatcher", "activator","warp","comment", "breakpoint","help", "help"};
        currentIndex = 0; 
        tool = transform.parent.Find("Sidebar Tool").GetComponent<SelectedTool>(); 
    }

    // Update is called once per frame
    void Update()
    {
        if (currentIndex != tool.projectilecode){
            currentIndex = tool.projectilecode; 
            GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/" + icons[currentIndex]); 
        }
    }
}

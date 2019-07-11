using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using  UnityEngine.UI; 

public class ToggleTool : MonoBehaviour
{
    int currentIndex; 
    SelectedTool tool; 
    // Start is called before the first frame update
    void Start()
    {
        currentIndex = 0; 
        tool = transform.parent.Find("Sidebar Tool").GetComponent<SelectedTool>(); 
    }

    // Update is called once per frame
    void Update()
    {
        if (currentIndex != tool.projectilecode){
            currentIndex = tool.projectilecode; 
            if (currentIndex>= 0 && currentIndex < GlobalState.StringLib.onIcons.Length)
                GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/icons/" + ((GlobalState.GameMode == stringLib.GAME_MODE_ON) ? GlobalState.StringLib.onIcons[currentIndex] : GlobalState.StringLib.bugIcons[currentIndex]));
        }
    }
}

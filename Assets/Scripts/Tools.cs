using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tools : MonoBehaviour
{
    protected int index = -1;
    protected string language = "";
    protected string displaytext = "";
    protected SidebarController sidebar;
    protected SelectedTool selectedTool;
    protected Output output;
    protected LevelGenerator lg;
    public int[] tools = new int[stateLib.NUMBER_OF_TOOLS];
    protected bool toolgiven = false; 
    public int Index
    {
        get
        {
            return index; 
        }
        set
        {
            index = value;
        }
    }
    public string DisplayText
    {
        set
        {
            displaytext = value; 
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        lg = GameObject.Find("CodeScreen").GetComponent<LevelGenerator>();
        output = GameObject.Find("OutputCanvas").transform.GetChild(0).GetComponent<Output>();
        sidebar = GameObject.Find("Sidebar").GetComponent<SidebarController>();
        selectedTool = sidebar.gameObject.transform.Find("Sidebar Tool").gameObject.GetComponent<SelectedTool>();
        Initialize(); 
    }
    protected virtual void Initialize() { }
}

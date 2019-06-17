using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI; 
using UnityEngine;

/// <summary>
/// Control the elements in the sidebar and buffer them from other 
/// classes trying to access them. 
/// </summary>
public class SidebarController : MonoBehaviour
{
    public GameObject panel, levelDescriptor, availableTools;
    public GameObject[] tools = new GameObject[6]; 
    public GameObject checklist, tool, timer, hint;
    private Sprite[] panels = new Sprite[8];
    private stringLib stringLibrary;
    bool active = true; 
    public bool isActive
    {
        get
        {
            return active; 
        }
    }
    public void NextTool()
    {
        tool.GetComponent<SelectedTool>().NextTool(); 
    }
    private void LoadPanels()
    {
        string path = "Sprites/panel-";
        for (int i = 0; i < panels.Length; i++)
        {
            panels[i] = Resources.Load<Sprite>(path + (i + 2).ToString());
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        LoadPanels();
        stringLibrary = new stringLib(); 
        levelDescriptor.GetComponent<Text>().text = GlobalState.level.Description; 
    }
    public void ToggleSidebar()
    {
        active = !active;
        this.GetComponent<Canvas>().enabled = active; 
    }
    public void ToggleLight()
    {
        panel.GetComponent<Image>().sprite = panels[4];
        timer.GetComponent<Text>().color = Color.black; 
        checklist.GetComponent<Text>().color = Color.black;
        tool.GetComponent<Text>().color = Color.black;
        levelDescriptor.GetComponent<Text>().color = Color.black; 
        availableTools.GetComponent<Text>().color = Color.black;
        tool.GetComponent<Text>().color = Color.black;
        //hint.GetComponent<Text>().color = Color.black;

        checklist.GetComponent<Text>().text = checklist.GetComponent<Text>().text.Replace(stringLibrary.checklist_complete_color_tag, stringLibrary.checklist_complete_color_tag_dark);
        checklist.GetComponent<Text>().text = checklist.GetComponent<Text>().text.Replace(stringLibrary.checklist_incomplete_activate_color_tag, stringLibrary.checklist_incomplete_activate_color_tag_dark);
        checklist.GetComponent<Text>().text = checklist.GetComponent<Text>().text.Replace(stringLibrary.checklist_incomplete_question_color_tag, stringLibrary.checklist_incomplete_question_color_tag_dark);
        checklist.GetComponent<Text>().text = checklist.GetComponent<Text>().text.Replace(stringLibrary.checklist_incomplete_name_color_tag, stringLibrary.checklist_incomplete_name_color_tag_dark);
        checklist.GetComponent<Text>().text = checklist.GetComponent<Text>().text.Replace(stringLibrary.checklist_incomplete_comment_color_tag, stringLibrary.checklist_incomplete_comment_color_tag_dark);
        checklist.GetComponent<Text>().text = checklist.GetComponent<Text>().text.Replace(stringLibrary.checklist_incomplete_uncomment_color_tag, stringLibrary.checklist_incomplete_uncomment_color_tag_dark);
        transform.Find("Indicate Hide").GetComponent<Text>().color = Color.black; 
        for (int i = 0; i < stateLib.NUMBER_OF_TOOLS - 1; i++)
        {
            // @TODO: figure out if theyre done or not and put it in the expression
            tools[i].transform.GetChild(0).GetComponent<Text>().color = (false) ? new Color(0, 0.6f, 0.2f, 1) : (GlobalState.IsDark ? Color.white : Color.black); 
        }
    }
    public void ToggleDark()
    {
        timer.GetComponent<Text>().color = Color.white;
        panel.GetComponent<Image>().sprite = panels[1];
        levelDescriptor.GetComponent<Text>().color = Color.white;
        checklist.GetComponent<Text>().color = Color.white; 
        availableTools.GetComponent<Text>().color = Color.white;
        tool.GetComponent<Text>().color = Color.white;
        levelDescriptor.GetComponent<Text>().color = Color.white;

        checklist.GetComponent<Text>().text = checklist.GetComponent<Text>().text.Replace(stringLibrary.checklist_complete_color_tag, stringLibrary.checklist_complete_color_tag_light);
        checklist.GetComponent<Text>().text = checklist.GetComponent<Text>().text.Replace(stringLibrary.checklist_incomplete_activate_color_tag, stringLibrary.checklist_incomplete_activate_color_tag_light);
        checklist.GetComponent<Text>().text = checklist.GetComponent<Text>().text.Replace(stringLibrary.checklist_incomplete_question_color_tag, stringLibrary.checklist_incomplete_question_color_tag_light);
        checklist.GetComponent<Text>().text = checklist.GetComponent<Text>().text.Replace(stringLibrary.checklist_incomplete_name_color_tag, stringLibrary.checklist_incomplete_name_color_tag_light);
        checklist.GetComponent<Text>().text = checklist.GetComponent<Text>().text.Replace(stringLibrary.checklist_incomplete_comment_color_tag, stringLibrary.checklist_incomplete_comment_color_tag_light);
        checklist.GetComponent<Text>().text = checklist.GetComponent<Text>().text.Replace(stringLibrary.checklist_incomplete_uncomment_color_tag, stringLibrary.checklist_incomplete_uncomment_color_tag_light);
        transform.Find("Indicate Hide").GetComponent<Text>().color = Color.white; 
        
        for (int i = 0; i < stateLib.NUMBER_OF_TOOLS - 1; i++)
        {
            // @TODO: figure out if theyre done or not and put it in the expression
            tools[i].transform.GetChild(0).GetComponent<Text>().color = (false) ? Color.green : (GlobalState.IsDark ? Color.white : Color.black); 
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (GlobalState.GameState == stateLib.GAMESTATE_IN_GAME && this.GetComponent<Canvas>().enabled != active){
            this.GetComponent<Canvas>().enabled = active;
        }
        else if (GlobalState.GameState != stateLib.GAMESTATE_IN_GAME) GetComponent<Canvas>().enabled = false; 
    }
}

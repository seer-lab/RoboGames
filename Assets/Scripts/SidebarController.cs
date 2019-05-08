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
        hint.GetComponent<Text>().color = Color.black;

        stringLibrary.node_color_print = stringLibrary.node_color_print_dark;
        stringLibrary.node_color_warp = stringLibrary.node_color_warp_dark;
        stringLibrary.node_color_rename = stringLibrary.node_color_rename_dark;
        stringLibrary.node_color_question = stringLibrary.node_color_question_dark;
        stringLibrary.node_color_uncomment = stringLibrary.node_color_uncomment_dark;
        stringLibrary.node_color_incorrect_uncomment = stringLibrary.node_color_incorrect_uncomment_dark;
        stringLibrary.node_color_correct_comment = stringLibrary.node_color_correct_comment_dark;
        stringLibrary.node_color_incorrect_comment = stringLibrary.node_color_incorrect_comment_dark;
        stringLibrary.node_color_comment = stringLibrary.node_color_comment_dark;

        stringLibrary.syntax_color_comment = stringLibrary.syntax_color_comment_dark;
        stringLibrary.syntax_color_keyword = stringLibrary.syntax_color_keyword_dark;
        stringLibrary.syntax_color_badcomment = stringLibrary.syntax_color_badcomment_dark;
        stringLibrary.syntax_color_string = stringLibrary.syntax_color_string_dark;

        stringLibrary.checklist_complete_color_tag = stringLibrary.checklist_complete_color_tag_dark;
        stringLibrary.checklist_incomplete_activate_color_tag = stringLibrary.checklist_incomplete_activate_color_tag_dark;
        stringLibrary.checklist_incomplete_question_color_tag = stringLibrary.checklist_incomplete_question_color_tag_dark;
        stringLibrary.checklist_incomplete_name_color_tag = stringLibrary.checklist_incomplete_name_color_tag_dark;
        stringLibrary.checklist_incomplete_comment_color_tag = stringLibrary.checklist_incomplete_comment_color_tag_dark;
        stringLibrary.checklist_incomplete_uncomment_color_tag = stringLibrary.checklist_incomplete_uncomment_color_tag_dark;

        checklist.GetComponent<Text>().text = checklist.GetComponent<Text>().text.Replace(stringLibrary.checklist_complete_color_tag, stringLibrary.checklist_complete_color_tag_dark);
        checklist.GetComponent<Text>().text = checklist.GetComponent<Text>().text.Replace(stringLibrary.checklist_incomplete_activate_color_tag, stringLibrary.checklist_incomplete_activate_color_tag_dark);
        checklist.GetComponent<Text>().text = checklist.GetComponent<Text>().text.Replace(stringLibrary.checklist_incomplete_question_color_tag, stringLibrary.checklist_incomplete_question_color_tag_dark);
        checklist.GetComponent<Text>().text = checklist.GetComponent<Text>().text.Replace(stringLibrary.checklist_incomplete_name_color_tag, stringLibrary.checklist_incomplete_name_color_tag_dark);
        checklist.GetComponent<Text>().text = checklist.GetComponent<Text>().text.Replace(stringLibrary.checklist_incomplete_comment_color_tag, stringLibrary.checklist_incomplete_comment_color_tag_dark);
        checklist.GetComponent<Text>().text = checklist.GetComponent<Text>().text.Replace(stringLibrary.checklist_incomplete_uncomment_color_tag, stringLibrary.checklist_incomplete_uncomment_color_tag_dark);

        for (int i = 0; i < stateLib.NUMBER_OF_TOOLS - 1; i++)
        {
            // @TODO: figure out if theyre done or not and put it in the expression
            tools[i].transform.GetChild(0).GetComponent<Text>().color = (false) ? new Color(0, 0.6f, 0.2f, 1) : Color.white;
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

        stringLibrary.node_color_print = stringLibrary.node_color_print_light;
        stringLibrary.node_color_warp = stringLibrary.node_color_warp_light;
        stringLibrary.node_color_rename = stringLibrary.node_color_rename_light;
        stringLibrary.node_color_question = stringLibrary.node_color_question_light;
        stringLibrary.node_color_uncomment = stringLibrary.node_color_uncomment_light;
        stringLibrary.node_color_incorrect_uncomment = stringLibrary.node_color_incorrect_uncomment_light;
        stringLibrary.node_color_correct_comment = stringLibrary.node_color_correct_comment_light;
        stringLibrary.node_color_incorrect_comment = stringLibrary.node_color_incorrect_comment_light;
        stringLibrary.node_color_comment = stringLibrary.node_color_comment_light;

        stringLibrary.syntax_color_comment = stringLibrary.syntax_color_comment_light;
        stringLibrary.syntax_color_keyword = stringLibrary.syntax_color_keyword_light;
        stringLibrary.syntax_color_badcomment = stringLibrary.syntax_color_badcomment_light;
        stringLibrary.syntax_color_string = stringLibrary.syntax_color_string_light;

        stringLibrary.checklist_complete_color_tag = stringLibrary.checklist_complete_color_tag_light;
        stringLibrary.checklist_incomplete_activate_color_tag = stringLibrary.checklist_incomplete_activate_color_tag_light;
        stringLibrary.checklist_incomplete_question_color_tag = stringLibrary.checklist_incomplete_question_color_tag_light;
        stringLibrary.checklist_incomplete_name_color_tag = stringLibrary.checklist_incomplete_name_color_tag_light;
        stringLibrary.checklist_incomplete_comment_color_tag = stringLibrary.checklist_incomplete_comment_color_tag_light;
        stringLibrary.checklist_incomplete_uncomment_color_tag = stringLibrary.checklist_incomplete_uncomment_color_tag_light;

        checklist.GetComponent<Text>().text = checklist.GetComponent<Text>().text.Replace(stringLibrary.checklist_complete_color_tag, stringLibrary.checklist_complete_color_tag_light);
        checklist.GetComponent<Text>().text = checklist.GetComponent<Text>().text.Replace(stringLibrary.checklist_incomplete_activate_color_tag, stringLibrary.checklist_incomplete_activate_color_tag_light);
        checklist.GetComponent<Text>().text = checklist.GetComponent<Text>().text.Replace(stringLibrary.checklist_incomplete_question_color_tag, stringLibrary.checklist_incomplete_question_color_tag_light);
        checklist.GetComponent<Text>().text = checklist.GetComponent<Text>().text.Replace(stringLibrary.checklist_incomplete_name_color_tag, stringLibrary.checklist_incomplete_name_color_tag_light);
        checklist.GetComponent<Text>().text = checklist.GetComponent<Text>().text.Replace(stringLibrary.checklist_incomplete_comment_color_tag, stringLibrary.checklist_incomplete_comment_color_tag_light);
        checklist.GetComponent<Text>().text = checklist.GetComponent<Text>().text.Replace(stringLibrary.checklist_incomplete_uncomment_color_tag, stringLibrary.checklist_incomplete_uncomment_color_tag_light);

        for (int i = 0; i < stateLib.NUMBER_OF_TOOLS - 1; i++)
        {
            // @TODO: figure out if theyre done or not and put it in the expression
            tools[i].transform.GetChild(0).GetComponent<Text>().color = (false) ? Color.green : Color.white;
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
}

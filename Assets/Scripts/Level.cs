using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

/// <summary>
/// Store all Data regarding the Level. 
/// </summary>
public class Level
{
    public string Language { get; set; }
    public string NextLevel { get; set; }
    public string FileName { get; set; }
    public bool Warp { get; set; }
    public bool IsDemo {get;set;}
    public string[] Code { get; set; }
    public string Description {get; set;}
    public string[] Tags { get; set; }
    public float Time { get; set; }
    public int[] Tasks;
    public int[] CompletedTasks;
    public int[,] TaskOnLine { get; set; }
    public string IntroText { get; set; }
    public string ExitText { get; set; }
    public int LineCount { get; set; }
    public string Hint { get; set; }
    public string Failure_Level { get; set; }

    public XmlNodeList CodeNodes { get; set; }
    public IList<XmlNode> NodeList { get; set; }
    public XmlNode LevelNode { get; set; }

    //The code uses tags to simulate text highlighting. The following functions are
    //used to replace the tags with ones that are friendly to the colour theme. 
    public void ToggleDark()
    {
        // Switch the text colors to correspond with the Dark Color palette (lighter colors)
        for (int i = 0; i < GlobalState.level.Code.GetLength(0); i++)
        {
            GlobalState.level.Code[i] = GlobalState.level.Code[i].Replace(GlobalState.StringLib.node_color_print, GlobalState.StringLib.node_color_print_light);
            GlobalState.level.Code[i] = GlobalState.level.Code[i].Replace(GlobalState.StringLib.node_color_warp, GlobalState.StringLib.node_color_warp_light);
            GlobalState.level.Code[i] = GlobalState.level.Code[i].Replace(GlobalState.StringLib.node_color_rename, GlobalState.StringLib.node_color_rename_light);
            GlobalState.level.Code[i] = GlobalState.level.Code[i].Replace(GlobalState.StringLib.node_color_question, GlobalState.StringLib.node_color_question_light);
            GlobalState.level.Code[i] = GlobalState.level.Code[i].Replace(GlobalState.StringLib.node_color_uncomment, GlobalState.StringLib.node_color_uncomment_light);
            GlobalState.level.Code[i] = GlobalState.level.Code[i].Replace(GlobalState.StringLib.node_color_incorrect_uncomment, GlobalState.StringLib.node_color_incorrect_uncomment_light);
            GlobalState.level.Code[i] = GlobalState.level.Code[i].Replace(GlobalState.StringLib.node_color_correct_comment, GlobalState.StringLib.node_color_correct_comment_light);
            GlobalState.level.Code[i] = GlobalState.level.Code[i].Replace(GlobalState.StringLib.node_color_incorrect_comment, GlobalState.StringLib.node_color_incorrect_comment_light);
            GlobalState.level.Code[i] = GlobalState.level.Code[i].Replace(GlobalState.StringLib.node_color_comment, GlobalState.StringLib.node_color_comment_light);
            GlobalState.level.Code[i] = GlobalState.level.Code[i].Replace(GlobalState.StringLib.syntax_color_comment, GlobalState.StringLib.syntax_color_comment_light);
            GlobalState.level.Code[i] = GlobalState.level.Code[i].Replace(GlobalState.StringLib.syntax_color_keyword, GlobalState.StringLib.syntax_color_keyword_light);
            GlobalState.level.Code[i] = GlobalState.level.Code[i].Replace(GlobalState.StringLib.syntax_color_badcomment, GlobalState.StringLib.syntax_color_badcomment_light);
            GlobalState.level.Code[i] = GlobalState.level.Code[i].Replace(GlobalState.StringLib.syntax_color_string, GlobalState.StringLib.syntax_color_string_light);


        }
        GlobalState.StringLib.node_color_print = GlobalState.StringLib.node_color_print_light;
        GlobalState.StringLib.node_color_warp = GlobalState.StringLib.node_color_warp_light;
        GlobalState.StringLib.node_color_rename = GlobalState.StringLib.node_color_rename_light;
        GlobalState.StringLib.node_color_question = GlobalState.StringLib.node_color_question_light;
        GlobalState.StringLib.node_color_uncomment = GlobalState.StringLib.node_color_uncomment_light;
        GlobalState.StringLib.node_color_incorrect_uncomment = GlobalState.StringLib.node_color_incorrect_uncomment_light;
        GlobalState.StringLib.node_color_correct_comment = GlobalState.StringLib.node_color_correct_comment_light;
        GlobalState.StringLib.node_color_incorrect_comment = GlobalState.StringLib.node_color_incorrect_comment_light;
        GlobalState.StringLib.node_color_comment = GlobalState.StringLib.node_color_comment_light;

        GlobalState.StringLib.syntax_color_comment = GlobalState.StringLib.syntax_color_comment_light;
        GlobalState.StringLib.syntax_color_keyword = GlobalState.StringLib.syntax_color_keyword_light;
        GlobalState.StringLib.syntax_color_badcomment = GlobalState.StringLib.syntax_color_badcomment_light;
        GlobalState.StringLib.syntax_color_string = GlobalState.StringLib.syntax_color_string_light;

        GlobalState.StringLib.checklist_complete_color_tag = GlobalState.StringLib.checklist_complete_color_tag_light;
        GlobalState.StringLib.checklist_incomplete_activate_color_tag = GlobalState.StringLib.checklist_incomplete_activate_color_tag_light;
        GlobalState.StringLib.checklist_incomplete_question_color_tag = GlobalState.StringLib.checklist_incomplete_question_color_tag_light;
        GlobalState.StringLib.checklist_incomplete_name_color_tag = GlobalState.StringLib.checklist_incomplete_name_color_tag_light;
        GlobalState.StringLib.checklist_incomplete_comment_color_tag = GlobalState.StringLib.checklist_incomplete_comment_color_tag_light;
        GlobalState.StringLib.checklist_incomplete_uncomment_color_tag = GlobalState.StringLib.checklist_incomplete_uncomment_color_tag_light;
    }
    public void ToggleLight()
    {
        // Switch the text colors to correspond with the Light Color palette (darker colors)
        for (int i = 0; i < GlobalState.level.Code.GetLength(0); i++)
        {
            GlobalState.level.Code[i] = GlobalState.level.Code[i].Replace(GlobalState.StringLib.node_color_print, GlobalState.StringLib.node_color_print_dark);
            GlobalState.level.Code[i] = GlobalState.level.Code[i].Replace(GlobalState.StringLib.node_color_warp, GlobalState.StringLib.node_color_warp_dark);
            GlobalState.level.Code[i] = GlobalState.level.Code[i].Replace(GlobalState.StringLib.node_color_rename, GlobalState.StringLib.node_color_rename_dark);
            GlobalState.level.Code[i] = GlobalState.level.Code[i].Replace(GlobalState.StringLib.node_color_question, GlobalState.StringLib.node_color_question_dark);
            GlobalState.level.Code[i] = GlobalState.level.Code[i].Replace(GlobalState.StringLib.node_color_uncomment, GlobalState.StringLib.node_color_uncomment_dark);
            GlobalState.level.Code[i] = GlobalState.level.Code[i].Replace(GlobalState.StringLib.node_color_incorrect_uncomment, GlobalState.StringLib.node_color_incorrect_uncomment_dark);
            GlobalState.level.Code[i] = GlobalState.level.Code[i].Replace(GlobalState.StringLib.node_color_correct_comment, GlobalState.StringLib.node_color_correct_comment_dark);
            GlobalState.level.Code[i] = GlobalState.level.Code[i].Replace(GlobalState.StringLib.node_color_incorrect_comment, GlobalState.StringLib.node_color_incorrect_comment_dark);
            GlobalState.level.Code[i] = GlobalState.level.Code[i].Replace(GlobalState.StringLib.node_color_comment, GlobalState.StringLib.node_color_comment_dark);

            GlobalState.level.Code[i] = GlobalState.level.Code[i].Replace(GlobalState.StringLib.syntax_color_comment, GlobalState.StringLib.syntax_color_comment_dark);
            GlobalState.level.Code[i] = GlobalState.level.Code[i].Replace(GlobalState.StringLib.syntax_color_keyword, GlobalState.StringLib.syntax_color_keyword_dark);
            GlobalState.level.Code[i] = GlobalState.level.Code[i].Replace(GlobalState.StringLib.syntax_color_badcomment, GlobalState.StringLib.syntax_color_badcomment_dark);
            GlobalState.level.Code[i] = GlobalState.level.Code[i].Replace(GlobalState.StringLib.syntax_color_string, GlobalState.StringLib.syntax_color_string_dark);
        }


        GlobalState.StringLib.node_color_print = GlobalState.StringLib.node_color_print_dark;
        GlobalState.StringLib.node_color_warp = GlobalState.StringLib.node_color_warp_dark;
        GlobalState.StringLib.node_color_rename = GlobalState.StringLib.node_color_rename_dark;
        GlobalState.StringLib.node_color_question = GlobalState.StringLib.node_color_question_dark;
        GlobalState.StringLib.node_color_uncomment = GlobalState.StringLib.node_color_uncomment_dark;
        GlobalState.StringLib.node_color_incorrect_uncomment = GlobalState.StringLib.node_color_incorrect_uncomment_dark;
        GlobalState.StringLib.node_color_correct_comment = GlobalState.StringLib.node_color_correct_comment_dark;
        GlobalState.StringLib.node_color_incorrect_comment = GlobalState.StringLib.node_color_incorrect_comment_dark;
        GlobalState.StringLib.node_color_comment = GlobalState.StringLib.node_color_comment_dark;

        GlobalState.StringLib.syntax_color_comment = GlobalState.StringLib.syntax_color_comment_dark;
        GlobalState.StringLib.syntax_color_keyword = GlobalState.StringLib.syntax_color_keyword_dark;
        GlobalState.StringLib.syntax_color_badcomment = GlobalState.StringLib.syntax_color_badcomment_dark;
        GlobalState.StringLib.syntax_color_string = GlobalState.StringLib.syntax_color_string_dark;

        GlobalState.StringLib.checklist_complete_color_tag = GlobalState.StringLib.checklist_complete_color_tag_dark;
        GlobalState.StringLib.checklist_incomplete_activate_color_tag = GlobalState.StringLib.checklist_incomplete_activate_color_tag_dark;
        GlobalState.StringLib.checklist_incomplete_question_color_tag = GlobalState.StringLib.checklist_incomplete_question_color_tag_dark;
        GlobalState.StringLib.checklist_incomplete_name_color_tag = GlobalState.StringLib.checklist_incomplete_name_color_tag_dark;
        GlobalState.StringLib.checklist_incomplete_comment_color_tag = GlobalState.StringLib.checklist_incomplete_comment_color_tag_dark;
        GlobalState.StringLib.checklist_incomplete_uncomment_color_tag = GlobalState.StringLib.checklist_incomplete_uncomment_color_tag_dark;

    }
}

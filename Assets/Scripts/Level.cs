using System.Collections;
using System.Collections.Generic;
using System.Xml; 
using UnityEngine;

public class Level 
{
    public string Language { get; set; }
    public string NextLevel { get; set; }
    public string FileName { get; set; }
    public bool Warp { get; set; }
    public string[] Code { get; set; }
    public string[] Tags { get; set; }
    public float Time { get; set; }
    public int[] Tasks;
    public int[] CompletedTasks; 
    public int[,] TaskOnLine { get; set; }
    public string IntroText { get; set; }
    public string ExitText { get; set; }
    public int LineCount { get; set; }

    public XmlNodeList CodeNodes { get; set; }
    public IList<XmlNode> NodeList { get; set; }
    public XmlNode LevelNode { get; set; }
    stringLib stringLibrary = new stringLib(); 
    public void ToggleDark()
    {
        // Switch the text colors to correspond with the Dark Color palette (lighter colors)
        for (int i = 0; i < GlobalState.level.Code.GetLength(0); i++)
        {
            GlobalState.level.Code[i] = GlobalState.level.Code[i].Replace(stringLibrary.node_color_print, stringLibrary.node_color_print_light);
            GlobalState.level.Code[i] = GlobalState.level.Code[i].Replace(stringLibrary.node_color_warp, stringLibrary.node_color_warp_light);
            GlobalState.level.Code[i] = GlobalState.level.Code[i].Replace(stringLibrary.node_color_rename, stringLibrary.node_color_rename_light);
            GlobalState.level.Code[i] = GlobalState.level.Code[i].Replace(stringLibrary.node_color_question, stringLibrary.node_color_question_light);
            GlobalState.level.Code[i] = GlobalState.level.Code[i].Replace(stringLibrary.node_color_uncomment, stringLibrary.node_color_uncomment_light);
            GlobalState.level.Code[i] = GlobalState.level.Code[i].Replace(stringLibrary.node_color_incorrect_uncomment, stringLibrary.node_color_incorrect_uncomment_light);
            GlobalState.level.Code[i] = GlobalState.level.Code[i].Replace(stringLibrary.node_color_correct_comment, stringLibrary.node_color_correct_comment_light);
            GlobalState.level.Code[i] = GlobalState.level.Code[i].Replace(stringLibrary.node_color_incorrect_comment, stringLibrary.node_color_incorrect_comment_light);
            GlobalState.level.Code[i] = GlobalState.level.Code[i].Replace(stringLibrary.node_color_comment, stringLibrary.node_color_comment_light);
            GlobalState.level.Code[i] = GlobalState.level.Code[i].Replace(stringLibrary.syntax_color_comment, stringLibrary.syntax_color_comment_light);
            GlobalState.level.Code[i] = GlobalState.level.Code[i].Replace(stringLibrary.syntax_color_keyword, stringLibrary.syntax_color_keyword_light);
            GlobalState.level.Code[i] = GlobalState.level.Code[i].Replace(stringLibrary.syntax_color_badcomment, stringLibrary.syntax_color_badcomment_light);
            GlobalState.level.Code[i] = GlobalState.level.Code[i].Replace(stringLibrary.syntax_color_string, stringLibrary.syntax_color_string_light);
        }
    }
    public void ToggleLight()
    {
        // Switch the text colors to correspond with the Light Color palette (darker colors)
        for (int i = 0; i < GlobalState.level.Code.GetLength(0); i++)
        {
            GlobalState.level.Code[i] = GlobalState.level.Code[i].Replace(stringLibrary.node_color_print, stringLibrary.node_color_print_dark);
            GlobalState.level.Code[i] = GlobalState.level.Code[i].Replace(stringLibrary.node_color_warp, stringLibrary.node_color_warp_dark);
            GlobalState.level.Code[i] = GlobalState.level.Code[i].Replace(stringLibrary.node_color_rename, stringLibrary.node_color_rename_dark);
            GlobalState.level.Code[i] = GlobalState.level.Code[i].Replace(stringLibrary.node_color_question, stringLibrary.node_color_question_dark);
            GlobalState.level.Code[i] = GlobalState.level.Code[i].Replace(stringLibrary.node_color_uncomment, stringLibrary.node_color_uncomment_dark);
            GlobalState.level.Code[i] = GlobalState.level.Code[i].Replace(stringLibrary.node_color_incorrect_uncomment, stringLibrary.node_color_incorrect_uncomment_dark);
            GlobalState.level.Code[i] = GlobalState.level.Code[i].Replace(stringLibrary.node_color_correct_comment, stringLibrary.node_color_correct_comment_dark);
            GlobalState.level.Code[i] = GlobalState.level.Code[i].Replace(stringLibrary.node_color_incorrect_comment, stringLibrary.node_color_incorrect_comment_dark);
            GlobalState.level.Code[i] = GlobalState.level.Code[i].Replace(stringLibrary.node_color_comment, stringLibrary.node_color_comment_dark);

            GlobalState.level.Code[i] = GlobalState.level.Code[i].Replace(stringLibrary.syntax_color_comment, stringLibrary.syntax_color_comment_dark);
            GlobalState.level.Code[i] = GlobalState.level.Code[i].Replace(stringLibrary.syntax_color_keyword, stringLibrary.syntax_color_keyword_dark);
            GlobalState.level.Code[i] = GlobalState.level.Code[i].Replace(stringLibrary.syntax_color_badcomment, stringLibrary.syntax_color_badcomment_dark);
            GlobalState.level.Code[i] = GlobalState.level.Code[i].Replace(stringLibrary.syntax_color_string, stringLibrary.syntax_color_string_dark);
        }
    }
}

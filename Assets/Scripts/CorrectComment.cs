using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorrectComment : comment
{
    protected override void OnTriggerProtocol(Collider2D collidingObj)
    {
        if (collidingObj.name == stringLib.PROJECTILE_COMMENT && !isCommented)
        {
            //GetComponent<SpriteRenderer>().sprite = descSpriteOn;
            isCommented = true;
            audioSource.PlayOneShot(correct); 
            anim.SetTrigger("Complete");
            Destroy(collidingObj.gameObject);
            GetComponent<AudioSource>().Play();
            selectedTool.bonusTools[stateLib.TOOL_COMMENTER]++;
            string sNewText = blocktext;
            string[] sNewParts = sNewText.Split('\n');
            string multilineCommentOpenSymbolPython = @"'''";
            string multilineCommentCloseSymbolPython = @"'''";
            string multilineCommentOpenSymbolCpp = @"/* ";
            string multilineCommentCloseSymbolCpp = @" */";
            string singlelineCommentOpenSymbolPython = @"# ";
            string singlelineCommentOpenSymbolCpp = @"// ";
            string commentOpenSymbol = multilineCommentOpenSymbolPython;
            string commentCloseSymbol = multilineCommentCloseSymbolPython;
            switch (GlobalState.level.Language)
            {
                case "python":
                    {
                        commentOpenSymbol = (commentStyle == "multi") ? multilineCommentOpenSymbolPython : singlelineCommentOpenSymbolPython;
                        commentCloseSymbol = (commentStyle == "multi") ? multilineCommentCloseSymbolPython : "";
                        break;
                    }
                case "c++":
                case "c":
                case "c#":
                    {
                        commentOpenSymbol = (commentStyle == "multi") ? multilineCommentOpenSymbolCpp : singlelineCommentOpenSymbolCpp;
                        commentCloseSymbol = (commentStyle == "multi") ? multilineCommentCloseSymbolCpp : "";
                        break;
                    }
                default:
                    {
                        commentOpenSymbol = (commentStyle == "multi") ? multilineCommentOpenSymbolPython : singlelineCommentOpenSymbolPython;
                        commentCloseSymbol = (commentStyle == "multi") ? multilineCommentCloseSymbolPython : "";
                        break;
                    }
            }
            if (sNewParts.Length == 1)
            {
                GlobalState.level.Code[index] = GlobalState.StringLib.node_color_correct_comment + commentOpenSymbol + blocktext + commentCloseSymbol + stringLib.CLOSE_COLOR_TAG;
            }
            else
            {
                // Multi line
                sNewParts[0] = GlobalState.StringLib.node_color_correct_comment + commentOpenSymbol + sNewParts[0] + stringLib.CLOSE_COLOR_TAG;
                for (int i = 1; i < sNewParts.Length - 1; i++)
                {
                    sNewParts[i] = (commentStyle == "multi") ? GlobalState.StringLib.node_color_correct_comment + sNewParts[i] + stringLib.CLOSE_COLOR_TAG :
                                                               GlobalState.StringLib.node_color_correct_comment + commentOpenSymbol + sNewParts[i] + commentCloseSymbol + stringLib.CLOSE_COLOR_TAG;
                }
                sNewParts[sNewParts.Length - 1] = (commentStyle == "multi") ? GlobalState.StringLib.node_color_correct_comment + sNewParts[sNewParts.Length - 1] + commentCloseSymbol + stringLib.CLOSE_COLOR_TAG :
                                                                            GlobalState.StringLib.node_color_correct_comment + commentOpenSymbol + sNewParts[sNewParts.Length - 1] + stringLib.CLOSE_COLOR_TAG;

                for (int i = 0; i < sNewParts.Length; i++)
                {
                    GlobalState.level.Code[index + i] = sNewParts[i];
                }
            }

            lg.DrawInnerXmlLinesToScreen();
            GlobalState.level.CompletedTasks[3]++;
            GlobalState.CurrentLevelPoints+= stateLib.POINTS_COMMENT; 
        }
        else if (collidingObj.name.Contains("projectile") && collidingObj.name != stringLib.PROJECTILE_COMMENT){
			hero.onFail();
            audioSource.PlayOneShot(wrong); 
		}
    }
}

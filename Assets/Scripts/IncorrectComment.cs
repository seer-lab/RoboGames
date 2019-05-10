using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class IncorrectComment : comment
{
    protected override void OnTriggerProtocol(Collider2D collidingObj)
    {
        if (collidingObj.name == stringLib.PROJECTILE_COMMENT && !doneUpdating)
        {
            Destroy(collidingObj.gameObject);
            selectedTool.outputtext.GetComponent<Text>().text = "This comment does not correctly describe \nthe code; a nearby comment better explains \nwhat is taking place.";
        }
    }
    public override void UpdateProtocol(){
        base.UpdateProtocol();
         if (CorrectCommentObject)
        {
            if (CorrectCommentObject.GetComponent<comment>().isCommented && !doneUpdating)
            {
                doneUpdating = true;
                if (entityType == stateLib.ENTITY_TYPE_INCORRECT_COMMENT)
                {
                    GetComponent<SpriteRenderer>().sprite = descSpriteOn;
                }
                else
                {
                    GetComponent<SpriteRenderer>().sprite = codeSpriteOn;
                }
                string sNewText = blocktext;
                string[] sNewParts = sNewText.Split('\n');
                if (sNewParts.Length == 1 && commentStyle == "single")
                {
                    // Single line

                    //verify comment color is removed

                    GlobalState.level.Code[index] = textColoration.DecolorizeText(GlobalState.level.Code[index]);

                    GlobalState.level.Code[index] = "";
                }
                else
                {

                    // Multi line
                    for (int i = 0; i < sNewParts.Length; i++)
                    {
                        //GlobalState.level.Code[index+i] = textColoration.DecolorizeText( GlobalState.level.Code[index + i]);
                        GlobalState.level.Code[index + i] = "";
                    }
                }
                lg.DrawInnerXmlLinesToScreen();
            }
        }
    }
}

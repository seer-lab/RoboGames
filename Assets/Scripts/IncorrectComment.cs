using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class IncorrectComment : comment
{
    bool failed = false; 
    protected override void OnRightArrowClick()
    {
        onComplete();
        HandleClick();
    }
    protected override void OnLeftArrowClick()
    {
        HandleClick();
        failed = true; 
        selectedTool.outputtext.GetComponent<Text>().text = "This comment does not correctly describe \nthe code; a nearby comment better explains \nwhat is taking place.";
        hero.onFail();
        audioSource.PlayOneShot(wrong);
    }
    protected override void OnTriggerProtocol(Collider2D collidingObj)
    {
        if (collidingObj.name == stringLib.PROJECTILE_COMMENT && !doneUpdating)
        {
            Destroy(collidingObj.gameObject);
            isAnswering = true;
            Output.IsAnswering = true;
            if (GlobalState.level.IsDemo){
                StartCoroutine(DemoPlay()); 
                hero.GetComponent<DemoBotControl>().InsertOptionAction(stateLib.TOOL_COMMENTER,0); 
            }
            string text = blocktext.Replace("\n",""); 
            text = blocktext.Replace("\t"," "); 
            if (text.Length > 75){
                output.Text.text = text.Substring(0, 72) + "...";
            }
            else output.Text.text = text;
        }
    }
    /// <summary>
    /// automatically handle the output box in demos
    /// </summary>
    /// <returns></returns>
    IEnumerator DemoPlay(){
        yield return new WaitForSecondsRealtime(1.5f); 
        onComplete(); 
        HandleClick(); 
    }
    /// <summary>
    /// Handle animations and text upong successful completion
    /// </summary>
    public void onComplete()
    {
        anim.SetTrigger("Complete");
        doneUpdating = true;
        //update the image
        if (entityType == stateLib.ENTITY_TYPE_INCORRECT_COMMENT)
        {
            GetComponent<SpriteRenderer>().sprite = descSpriteOn;
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = codeSpriteOn;
        }
        //replace the text to empty
        string sNewText = blocktext;
        string[] sNewParts = sNewText.Split('\n');
        if (sNewParts.Length == 1 && commentStyle == "single")
        {
            // Single line

            //verify comment color is removed

            GlobalState.level.Code[index] = TextColoration.DecolorizeText(GlobalState.level.Code[index]);

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
        if (failed) GlobalState.CurrentLevelPoints += stateLib.POINTS_COMMENT/2; 
        else GlobalState.CurrentLevelPoints += stateLib.POINTS_COMMENT; 
        GlobalState.level.CompletedTasks[3]++;
        if (CorrectCommentObject != null && !CorrectCommentObject.GetComponent<CorrectComment>().isCommented){
            CorrectCommentObject.GetComponent<CorrectComment>().onComment(); 
        }
    }
}


using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class CorrectUncomment : comment
{
    public bool failed = false; 
    public override void Initialize(){
        base.Initialize();
        anim.SetBool("IsUncomment", true);
    }
    protected override void OnTriggerProtocol(Collider2D collidingObj)
    {
        //TODO NOTE: This section is a bit of a mess and needs cleaning ^_^
        if (collidingObj.name == stringLib.PROJECTILE_DEBUG && !isCommented)
        {
            audioSource.PlayOneShot(correct); 
            anim.SetTrigger("Complete");
            GetComponent<SpriteRenderer>().sprite = codeSpriteOn;
            Destroy(collidingObj.gameObject);
            GetComponent<AudioSource>().Play();
            GlobalState.level.CompletedTasks[4]++;
            selectedTool.bonusTools[stateLib.TOOL_UNCOMMENTER]++;
            string sNewText = textColoration.DecolorizeText(GlobalState.level.Code[index]);
            //Debug.Log(blocktext);
            string tempDecolText = sNewText;
            //string[] sNewParts = GlobalState.level.Code[index].Split('\n');

            //this hack is ment to get around the block text, which doesnt contain any special variable 
            int counter = 0;
            bool header = false;
            bool tail = false;
            string tmpS = "";

            while(!tail){
                if((GlobalState.level.Code[index + counter].Contains(@"/*") || GlobalState.level.Code[index + counter].Contains("'''"))
                && header == false){
                    tmpS += GlobalState.level.Code[index + counter] + "\n";
                    header = true;
                    counter++;
                    continue;
                }
                if((GlobalState.level.Code[index + counter].Contains(@"*/") || GlobalState.level.Code[index + counter].Contains("'''"))
                    && header){
                    tmpS += GlobalState.level.Code[index + counter];
                    tail = true;
                }else if(header){
                    tmpS += GlobalState.level.Code[index + counter] + "\n";
                }else{
                    break;
                }
                counter++;
            }

            tmpS = textColoration.DecolorizeText(tmpS);

            string[] sNewParts = tmpS.Split('\n');
            if (sNewParts.Length == 1)
            {
                // Single line
                // Look for /* something */
                string multilinePatternCommentCpp = @"(\/\*)(.*)(\*\/)";
                // Look for ''' something '''
                string multilinePatternCommentPython = @"(\'\'\')(.*)(\'\'\')";
                // Look for //something
                string singlelinePatternCommentCpp = (@"(\/\/)(.*)");
                // Look for #something
                string singlelinePatternCommentPython = @"(#)(.*)";

                string patternComment = singlelinePatternCommentPython;
                switch (GlobalState.level.Language)
                {
                    case "python":
                        {
                            patternComment = (commentStyle == "multi") ? multilinePatternCommentPython : singlelinePatternCommentPython;
                            break;
                        }
                    case "c++":
                    case "c":
                    case "c#":
                        {
                            patternComment = (commentStyle == "multi") ? multilinePatternCommentCpp : singlelinePatternCommentCpp;
                            break;
                        }
                    default:
                        {
                            patternComment = (commentStyle == "multi") ? multilinePatternCommentPython : singlelinePatternCommentPython;
                            break;
                        }
                }
                Regex rgx = new Regex(patternComment);
                sNewText = rgx.Replace(sNewText, "$2");
                //todo: refactor this quick hack
                rgx = new Regex(@"(\/\*)(.*)(\*\/)");
                sNewText = rgx.Replace(sNewText, "$2");
                rgx = new Regex(@"(\/\/)(.*?)");
                sNewText = rgx.Replace(sNewText, "$2");


                //This is for single lime comment.
                //This will check if the word has a /v(word)/v and will color it 
                tempDecolText = textColoration.DecolorizeText(sNewText);
                sNewText = textColoration.ColorizeText(tempDecolText, GlobalState.level.Language);
                Regex tmp = new Regex(@"\v(.+?)\v");
                if(tmp.IsMatch(sNewText)){
                    Debug.Log(tmp.Match(sNewText));
                    sNewText = tmp.Replace(sNewText,"<color=#ff00ffff>" + tmp.Match(sNewText) + "</color>" );
                    //Debug.Log(sNewText);
                }
                GlobalState.level.Code[index] = sNewText;
                
            }
            else
            {
                string commentOpenSymbol;
                string commentCloseSymbol;
                if(GlobalState.Language == "python"){
                    commentOpenSymbol = "'''";
                    commentCloseSymbol = "'''";

                }else{
                    commentOpenSymbol = "/*";
                    commentCloseSymbol = "*/";
                }

                //Replace the comment symbol with nothing
                sNewParts[0] = sNewParts[0].Replace(GlobalState.StringLib.node_color_correct_comment, "");
                sNewParts[0] = sNewParts[0].Replace(commentOpenSymbol, "");
                sNewParts[sNewParts.Length - 1] = sNewParts[sNewParts.Length - 1].Replace(commentCloseSymbol, "");
                sNewParts[sNewParts.Length - 1] = sNewParts[sNewParts.Length - 1].Replace(stringLib.CLOSE_COLOR_TAG, "");

                //Colour the text
                GlobalState.level.Code[index] = textColoration.ColorizeText(sNewParts[0], language);
                GlobalState.level.Code[index + sNewParts.Length - 1] = textColoration.ColorizeText(sNewParts[sNewParts.Length - 1], language);

                //Sorry for the naming, I suck at it
                //Variable tmp will match if the word has a vertical tab in them
                //Variable tmpTwo will match if the word has /v(word)/v in it
                Regex tmp = new Regex("\v(.*?)\v");
                Regex tmpTwo = new Regex("\\v(.*?)\\v");
                Debug.Log(tmp.IsMatch(sNewParts[0]));
                
                bool checkIfExist = false;
                bool checkIfExistTwo = false;

                //check if the string matches the regex
                for(int i = 0; i < sNewParts.Length; i++){
                    if(tmp.IsMatch(GlobalState.level.Code[index + 1])){
                        checkIfExist = true;
                        break;
                    }else if(tmpTwo.IsMatch(sNewParts[i])){
                        checkIfExistTwo = true;
                        break;
                    }
                }

                //Color the text
                if(checkIfExist){
                    //GlobalState.level.Code[index] = tmp.Replace(GlobalState.level.Code[index],"<color=#ff00ffff>" + tmp.Match(GlobalState.level.Code[index]) + "</color>" );
                    for(int i = 0; i < sNewParts.Length; i++){
                        sNewParts[i] = tmp.Replace(GlobalState.level.Code[index + i], "<color=#ff00ffff>" + tmp.Match(GlobalState.level.Code[index + i]) + "</color>" );
                        GlobalState.level.Code[index + i] = sNewParts[i];
                    }
                //Somtime the regex wont work
                }else if(checkIfExistTwo){
                    for(int i = 0; i < sNewParts.Length; i++){
                        sNewParts[i] = tmpTwo.Replace(GlobalState.level.Code[index + i], "<color=#ff00ffff>" + tmpTwo.Match(GlobalState.level.Code[index + i]) + "</color>" );
                        GlobalState.level.Code[index + i] = sNewParts[i];
                    }
                }

            }
            if (failed) GlobalState.CurrentLevelPoints += stateLib.POINTS_UNCOMMENT/2; 
            else GlobalState.CurrentLevelPoints+= stateLib.POINTS_UNCOMMENT; 
            //Redraw the level
            lg.DrawInnerXmlLinesToScreen();
            isCommented = true;
        }
        else if (collidingObj.name.Contains("projectile") && collidingObj.name != stringLib.PROJECTILE_DEBUG){
	
            audioSource.PlayOneShot(wrong); 
		}
    
    }
    public override void UpdateProtocol()
    {
        base.UpdateProtocol();
    }
}

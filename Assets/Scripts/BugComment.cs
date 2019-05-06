using System.Xml.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI; 
using UnityEngine;

public class BugComment : comment
{
    bool isAnswered = false; 
    protected override void OnTriggerProtocol(Collider2D collidingObj)
    {
        if (collidingObj.name == stringLib.PROJECTILE_COMMENT)
        {
            isAnswered = true; 
            Logger.printLogFile(stringLib.LOG_COMMENT_ON, this.transform.position);
            Destroy(collidingObj.gameObject);
            GetComponent<AudioSource>().Play();
            blocktext = blocktext.Substring("<color=#00ff00ff>/**/</color>".Length, blocktext.Length- "<color=#00ff00ff>/**/</color>".Length);
            blocktext = "<color=#00ff00ff>/*" + blocktext + "*/</color>";
            
            string[] text = blocktext.Split('\n');
            for (int i = 0; i < text.Length; i++)
            {
                GlobalState.level.Code[index + i] = text[i];
            }
            
            lg.DrawInnerXmlLinesToScreen();

            // CodeObject.GetComponent<TextMesh>().text = oldtext.Replace(blocktext, stringLib.comment_block_color_tag + "\*" +
            // 																	  blocktext.Replace("/**/","") +
            // 																	  " */" + stringLib.CLOSE_COLOR_TAG);
            output.Text.text = errmsg;
            resetTime = Time.time + timeDelay;
            resetting = true;

            // Award bonus tools if applicable
            if (!toolgiven)
            {
                toolgiven = true;
                for (int i = 0; i < stateLib.NUMBER_OF_TOOLS; i++)
                {
                    if (tools[i] > 0) lg.floatingTextOnPlayer("New Tools!");
                    selectedTool.toolCounts[i] += tools[i];
                }
            }

        }
    }
    public override void UpdateProtocol(){
        if (output.Text.text == "" && isAnswered){
            blocktext = blocktext.Substring("<color=#00ff00ff>/*".Length, blocktext.Length-"<color=#00ff00ff>/**/</color>".Length);
            blocktext = "<color=#00ff00ff>/**/</color>" + blocktext; 
            string[] text = blocktext.Split('\n');
            for (int i = 0; i < text.Length; i++)
            {
                GlobalState.level.Code[index + i] = text[i];
            }
            
            lg.DrawInnerXmlLinesToScreen();
            isAnswered = false; 
        }
    }
}

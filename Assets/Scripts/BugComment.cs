using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI; 
using UnityEngine;

public class BugComment : comment
{
    
    protected override void OnTriggerProtocol(Collider2D collidingObj)
    {
        if (collidingObj.name == stringLib.PROJECTILE_COMMENT)
        {
            Logger.printLogFile(stringLib.LOG_COMMENT_ON, this.transform.position);
            Destroy(collidingObj.gameObject);
            GetComponent<AudioSource>().Play();
            // Substring is startingPos, length. We want to start after the first color tag, and the length is the whole string - length of color tag - length of close color tag.
            //blocktext = blocktext.Substring(("<color=#00ff00ff>/**/").Length, (blocktext.Length) - (GlobalState.StringLib.node_color_question.Length) - (stringLib.CLOSE_COLOR_TAG.Length));
            //blocktext = "<color=#00ff00ff>/*" + blocktext.Replace("<color=#00ff00ff>/**/", "").Replace("/**/</color>", "") + "*/</color>");
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
}

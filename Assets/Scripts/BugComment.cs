using System.Xml.Linq;
using System.Text.RegularExpressions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class BugComment : comment
{
    bool isAnswered = false;
    string resultingOutput = "";

    protected override void OnTriggerProtocol(Collider2D collidingObj)
    {
        CleanBlocktext();
        if (collidingObj.name == stringLib.PROJECTILE_COMMENT && !isAnswered)
        {
            isAnswered = true;
            audioSource.PlayOneShot(correct);
            anim.SetTrigger("Complete");
            Destroy(collidingObj.gameObject);
            GetComponent<AudioSource>().Play();
            
            //Update the text in the code to comment out the code along with any additioanl lines 
            //associated with the block text. 
            string value = "<color=#00ff00ff>/*" + blocktext + "*/</color>";

            string[] text = value.Split('\n');
            for (int i = 0; i < text.Length; i++)
            {
                GlobalState.level.Code[index + i] = text[i];
            }

            lg.DrawInnerXmlLinesToScreen();

            //If the error is found in this block text, provide an error. 
            if (errmsg.Contains("$err$"))
            {
                output.Text.text = "<color=#B30730FF>ERROR: </color>" + errmsg.Replace("$err$", ""); 
            }
            else output.Text.text = errmsg;
            resultingOutput = output.Text.text; 
            resetTime = Time.time + timeDelay;
            resetting = true;

            // Award bonus tools if applicable
            if (!toolgiven)
            {
                toolgiven = true;
                for (int i = 0; i < stateLib.NUMBER_OF_TOOLS; i++)
                {
                    if (tools[i] > 0) lg.floatingTextOnPlayer(GlobalState.StringLib.COLORS[i]);
                    selectedTool.toolCounts[i] += tools[i];
                }
            }

        }
        else if (collidingObj.name.Contains("projectile") && collidingObj.name != stringLib.PROJECTILE_COMMENT)
        {

            audioSource.PlayOneShot(wrong);
        }
    }

    /// <summary>
    /// produces the original text before commenting and updates the
    /// code.
    /// </summary>
    public void Uncomment()
    {
        if (isAnswered)
        {
            anim.SetTrigger("isBug");
            output.Text.text = "";
            string value = textColoration.ColorizeText(blocktext, GlobalState.level.Language);
            value = "<color=#00ff00ff>/**/</color>" + value;
            string[] text = value.Split('\n');
            for (int i = 0; i < text.Length; i++)
            {
                GlobalState.level.Code[index + i] = text[i];
            }

            lg.DrawInnerXmlLinesToScreen();
            isAnswered = false;
        }
    }
    public override void UpdateProtocol()
    {
        if (output.Text.text == "" && isAnswered)
        {
            anim.SetTrigger("isBug");
            string value = textColoration.ColorizeText(blocktext, GlobalState.level.Language);
            value = "<color=#00ff00ff>/**/</color>" + value;
            string[] text = value.Split('\n');
            for (int i = 0; i < text.Length; i++)
            {
                GlobalState.level.Code[index + i] = text[i];
            }

            lg.DrawInnerXmlLinesToScreen();
            isAnswered = false;
        }
        else if (resultingOutput != output.Text.text && isAnswered){
            Uncomment(); 
        }
    }
}

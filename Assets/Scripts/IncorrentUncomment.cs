using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI; 
using UnityEngine;

public class IncorrentUncomment : comment
{
    protected override void OnTriggerProtocol(Collider2D collidingObj)
    {
        if (collidingObj.name == stringLib.PROJECTILE_DEBUG && !doneUpdating) {
			Destroy(collidingObj.gameObject);
			lg.toolsAirborne--;
			selectedTool.outputtext.GetComponent<Text>().text = "There are errors with the selected code; \nfigure out what the mistake is, then \nuncomment the correct solution.";
		}
    }
}

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
}

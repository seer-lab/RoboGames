//**************************************************//
// Class Name: VariableColor
// Class Description: Instantiable object for the Robot ON! game. This object is a child to the "rename" object.
//                    When a rename task is completed, VariableColor objects will update.
//                    This class is responsible for the flashing magenta text for the rename task.
// Methods:
// 		void Start()
//		void Update()
//		void OnTriggerEnter2D(Collider2D collidingObj)
// Author: Scott McLean
// Date Last Modified: 29/06/2016
//**************************************************//

using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;


public class VariableColor : MonoBehaviour {

    public GameObject parent;
	public int index = -1;
    public int groupid = -1;
    public string innertext = "";
    public string correct = "";
    public string language;
	private LevelGenerator lg;
    public GameObject CodescreenObject;
    public GameObject CorrectRenameObject;

    private bool doneUpdating;
    private bool doneFlashing;
    private int  flashCounter;
    private bool decolorOnce = false;

	//.................................>8.......................................
	// Use this for initialization
	void Start() {
		lg = CodescreenObject.GetComponent<LevelGenerator>();
	}

	//.................................>8.......................................
	// Update is called once per frame
	void Update() {
        if (CorrectRenameObject) {
            if (CorrectRenameObject.GetComponent<rename>().answered && !doneUpdating) {
                doneUpdating = true;
                lg.innerXmlLines[index] = lg.innerXmlLines[index].Replace(innertext, correct);
                lg.DrawInnerXmlLinesToScreen();
                flashCounter = 200;
            }
            else if (CorrectRenameObject.GetComponent<rename>().answered && doneUpdating && !doneFlashing) {
                if (flashCounter % 100 == 0) {
                    // lg.innerXmlLines[index] = lg.innerXmlLines[index].Replace(correct, lg.stringLibrary.NODE_COLOR_RENAME + correct + lg.stringLibrary.CLOSE_COLOR_TAG);
                    lg.innerXmlLines[index] = lg.innerXmlLines[index].Replace(correct, innertext);
                    lg.DrawInnerXmlLinesToScreen();
                }
                else if (flashCounter % 50 == 0) {
                    // lg.innerXmlLines[index] = lg.innerXmlLines[index].Replace(lg.stringLibrary.NODE_COLOR_RENAME + correct + lg.stringLibrary.CLOSE_COLOR_TAG, correct);
                    lg.innerXmlLines[index] = lg.innerXmlLines[index].Replace(innertext, correct);
                    lg.DrawInnerXmlLinesToScreen(false);
                }
                flashCounter--;
                if (flashCounter == 0) {
                    doneFlashing = true;
                    // Change the next groupid objects to the new colors
                    foreach(GameObject variablecolor in lg.robotONvariablecolors) {
                        if (variablecolor.GetComponent<VariableColor>().groupid == (groupid+1)) {
                            int lineNum = variablecolor.GetComponent<VariableColor>().index;
                            string sReplace = lg.outerXmlLines[lineNum];
                            sReplace = lg.OuterToInnerXml(sReplace, language);
                            lg.innerXmlLines[lineNum] = sReplace;
                            lg.DrawInnerXmlLinesToScreen();
                        }
					}
                }
            }
            else if (lg.renamegroupidCounter != groupid && decolorOnce != true) {
    			// Change the next groupid objects to the new colors
    			decolorOnce = true;
    			lg.innerXmlLines[index] = lg.innerXmlLines[index].Replace(innertext, lg.DecolorizeText(innertext));
    			lg.DrawInnerXmlLinesToScreen();
    		}
        }
	}

	//.................................>8.......................................
	void OnTriggerEnter2D(Collider2D collidingObj) {
	}
	//.................................>8.......................................
}

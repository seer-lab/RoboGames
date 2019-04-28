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


public class VariableColor : Tools {

    public GameObject parent;
    public int groupid = -1;
    public string innertext = "";
	public string oldname;
    public string correct = "";
    public GameObject CorrectRenameObject;

    private bool doneUpdating;
    private bool doneFlashing;
    private int  flashCounter;
    private bool decolorOnce = false;
    TextColoration textColoration;

    public override void Initialize()
    {
        textColoration = new TextColoration(); 
    }

    //.................................>8.......................................
    // Update is called once per frame
    void Update() {
        if (CorrectRenameObject) {
            if (CorrectRenameObject.GetComponent<rename>().answered && !doneUpdating) {
                doneUpdating = true;

				Regex rgx = new Regex("(?s)(.*)(<color=#ff00ffff>)(.*)(</color>)(.*)");
                GlobalState.level.Code[index] = rgx.Replace(GlobalState.level.Code[index], "$1$3$5");
				rgx = new Regex(@"(^| |\t|\>)("+oldname+")(;| )");
                GlobalState.level.Code[index] = rgx.Replace(GlobalState.level.Code[index],"$1"+correct+"$3");
				
				lg.DrawInnerXmlLinesToScreen();
                flashCounter = 200;
            }
            else if (CorrectRenameObject.GetComponent<rename>().answered && doneUpdating && !doneFlashing) {
                if (flashCounter % 100 == 0) {

					Regex rgx = new Regex(@"(^| |\t|\>)("+correct+")(;| )");
                    GlobalState.level.Code[index] = rgx.Replace(GlobalState.level.Code[index],"$1"+oldname+"$3");
					
                    lg.DrawInnerXmlLinesToScreen();
                }
                else if (flashCounter % 50 == 0) {

					Regex rgx = new Regex(@"(^| |\t|\>)("+oldname+")(;| )");
                    GlobalState.level.Code[index] = rgx.Replace(GlobalState.level.Code[index],"$1"+correct+"$3");
					
                    lg.DrawInnerXmlLinesToScreen(false);
                }
                flashCounter--;
                if (flashCounter == 0) {
                    doneFlashing = true;
                    // Change the next groupid objects to the new colors
                    foreach(GameObject variablecolor in lg.robotONvariablecolors) {
                        if (variablecolor.GetComponent<VariableColor>().groupid == (groupid+1)) {
                            int lineNum = variablecolor.GetComponent<VariableColor>().index;
                            string sReplace = GlobalState.level.Tags[lineNum];
                            //sReplace = lg.OuterToInnerXml(sReplace, language);
                            GlobalState.level.Code[lineNum] = sReplace;
                            lg.DrawInnerXmlLinesToScreen();
                        }
					}
                }
            }
            else if (lg.renamegroupidCounter != groupid && decolorOnce != true) {
    			// Change the next groupid objects to the new colors
    			decolorOnce = true;
                GlobalState.level.Code[index] = GlobalState.level.Code[index].Replace(innertext, textColoration.DecolorizeText(innertext));
    			lg.DrawInnerXmlLinesToScreen();
    		}
        }
	}

	//.................................>8.......................................
	void OnTriggerEnter2D(Collider2D collidingObj) {
	}
	//.................................>8.......................................
}

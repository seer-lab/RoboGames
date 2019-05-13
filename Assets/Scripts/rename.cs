//**************************************************//
// Class Name: rename
// Class Description: Instantiable object for the Robot ON! game. This is used with the Renamer tool.
// Methods:
// 		void Start()
//		void Update()
//		void OnTriggerEnter2D(Collider2D collidingObj)
// Author: Michael Miljanovic
// Date Last Modified: 6/1/2016
//**************************************************//

using UnityEngine;
using UnityEngine.UI; 
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;


public class rename : Tools {

	public int groupid = -1;
	public string correct;
	public string innertext;
	public string oldname = "";
	public List<string> options;

	public AudioSource audioPrompt;
	public AudioSource audioCorrect;
	public bool answered = false;

	private bool answering = false;
	private bool decolorOnce = false;
	private bool colorOnce = false;
	
	public Sprite renSpriteOff;
	public Sprite renSpriteOn;

	private int selection = 0;

    public override void Initialize()
    {
		if (answered)GetComponent<SpriteRenderer>().sprite = renSpriteOn;
        else GetComponent<SpriteRenderer>().sprite = renSpriteOff;
    }

	//.................................>8.......................................
	// Update is called once per frame
	void Update() {
		if (answering) {
			// Handle left and right arrows --[
			if (selection == 0) {
				output.Text.text = displaytext + "   " + options[selection] + " →";
			}
			else if (selection == options.Count-1) {
                output.Text.text = displaytext + "← " + options[selection];
			}
			else {
                output.Text.text = displaytext + "← " + options[selection] + " →";
			}
			// ]-- End of handling arrows

			// Handle input --[
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				// Increment number of tool uses
				if (selectedTool.toolCounts[stateLib.TOOL_WARPER_OR_RENAMER] != 999)
				{
					selectedTool.toolCounts[stateLib.TOOL_WARPER_OR_RENAMER]++;
				}
				answered = false;
				answering = false;
                Output.IsAnswering = false;
				output.Text.text = "";
			}
			if ((Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))) {
				answered = true;
				answering = false;
                Output.IsAnswering = false;
				if (selection != options.IndexOf(correct)) {
					answered = false;
					selectedTool.outputtext.GetComponent<Text>().text = "The name you chose isn't the best option for\nthis variable's purpose.\nWhat is this variable used for?";
				}
				else {
					// Award 1 extra use of the tool.
					selectedTool.bonusTools[stateLib.TOOL_WARPER_OR_RENAMER]++;
					audioCorrect.Play();
					// Change this object to the correct text
					GetComponent<SpriteRenderer>().sprite = renSpriteOn;

					int iter = 0;
					Regex rgx = new Regex(@"(?s)(.*)(<color=#ff00ffff>)(.*?)(</color>)(.*)");
					GlobalState.level.Code[index] = rgx.Replace(GlobalState.level.Code[index], "$1$3$5");
					foreach(string s in GlobalState.level.Code) {
						rgx = new Regex(@"([^a-zA-Z0-9])("+oldname+@")([^a-zA-Z0-9])");
						GlobalState.level.Code[iter] = rgx.Replace(GlobalState.level.Code[iter],"$1"+correct+"$3");
						iter += 1;
					}

					
					output.Text.text= "";
					lg.renamegroupidCounter++;
                    GlobalState.level.CompletedTasks[2]++;
                    lg.DrawInnerXmlLinesToScreen();

				}
			}
			else if (Input.GetKeyDown(KeyCode.RightArrow)) {
				selection = (selection + 1 <= options.Count - 1) ? selection + 1 : options.Count - 1;
			}
			else if (Input.GetKeyDown(KeyCode.LeftArrow)) {
				selection = (selection - 1 >= 0) ? selection - 1 : 0;
			}
			// ]-- End of input handling
		}
		else if (lg.renamegroupidCounter != groupid && decolorOnce != true) {
			// Change the next groupid objects to the new colors
			decolorOnce = true;
			lg.DrawInnerXmlLinesToScreen();
		}
		
	}

	//.................................>8.......................................
	void OnTriggerEnter2D(Collider2D collidingObj) {
		if (collidingObj.name == stringLib.PROJECTILE_WARP && !answered) {
			Destroy(collidingObj.gameObject);
			output.Text.text = displaytext;
			audioPrompt.Play();
			answering = true;
            Output.IsAnswering = true;
		}
	}

	//.................................>8.......................................
}

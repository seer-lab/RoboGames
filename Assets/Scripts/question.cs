//**************************************************//
// Class Name: question
// Class Description: Instantiable object for Robot ON! game. This corresponds to the Checker Tool
// Methods:
// 		void Start()
//		void Update()
//		void OnTriggerEnter2D(Collider2D collidingObj)
// Author: Michael Miljanovic
// Date Last Modified: 6/1/2016
//**************************************************//

using UnityEngine;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;
using System;


public class question : MonoBehaviour {

	public int index = -1;
	public string innertext;
	public string displaytext = "";
	public string expected;
	public string language;
	// Expected could be a CSV of acceptable answers or just one accepted string. If it's a CSV we need to break that up into an array called expectedArray
	public string[] expectedArray;
	public GameObject SidebarObject;
	public GameObject CodescreenObject;
	public GameObject ToolSelectorObject;
    public AudioSource audioPrompt;
    public AudioSource audioCorrect;

	private bool answering = false;
	private bool answered = false;
	private string input = "";
	private LevelGenerator lg;

	//.................................>8.......................................
	// Use this for initialization
	void Start() {
		lg = CodescreenObject.GetComponent<LevelGenerator>();
		expectedArray = expected.Split(new String[] {", ", ","}, StringSplitOptions.RemoveEmptyEntries);
	}

	//.................................>8.......................................
	// Update is called once per frame
	void Update() {
		if (answering) {
			if ((Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))) {
				answered = true;
				answering = false;
				lg.isAnswering = false;
				if (input != expected && Array.IndexOf(expectedArray, input) == -1) {
					answered = false;
					string lastInput = input;
					input = "";
					// Check to see if the expected answer could be a decimal
					decimal expectedValue = -999;
					decimal inputValue = -999;
					bool parsedExpected = decimal.TryParse(expected, out expectedValue);
					bool parsedInput = decimal.TryParse(lastInput, out inputValue);
					bool expectedBool = false;
					bool parsedBool = bool.TryParse(expected, out expectedBool);
					int inputInteger = -999;
					int expectedInteger = -999;
					bool parsedInputInt = Int32.TryParse(lastInput, out inputInteger);
					bool parsedExpectedInt = Int32.TryParse(expected, out expectedInteger);
					if (parsedBool) {
						ToolSelectorObject.GetComponent<SelectedTool>().outputtext.GetComponent<GUIText>().text = "You should double check to make \nsure you have the right result; \nit is either 'true' or 'false', \nnothing else is possible.";
					}
					else if (parsedExpected && parsedInput) {
						// Working with numbers
						if (expectedValue < inputValue) {
							ToolSelectorObject.GetComponent<SelectedTool>().outputtext.GetComponent<GUIText>().text = "Looks like your answer is too high; \ndid you forget to subtract a value?";
						}
						else if (expectedValue > inputValue) {
							ToolSelectorObject.GetComponent<SelectedTool>().outputtext.GetComponent<GUIText>().text = "Your answer is too low; \nperhaps you missed an addition somewhere?";
						}
						else if (parsedExpectedInt && !parsedInputInt) {
							ToolSelectorObject.GetComponent<SelectedTool>().outputtext.GetComponent<GUIText>().text = "Remember that integer variables do \nnot have decimal points; \nthey are whole numbers.";
						}
						else if (parsedInputInt && ! parsedExpectedInt) {
							ToolSelectorObject.GetComponent<SelectedTool>().outputtext.GetComponent<GUIText>().text = "Remember that double variables have \ndecimal points; the number 5 would \nbe written as 5.0";
						}
					}
					else if (parsedExpected) {
						ToolSelectorObject.GetComponent<SelectedTool>().outputtext.GetComponent<GUIText>().text = "The answer should be a number value. \nTry again.";
					}
					else {
						ToolSelectorObject.GetComponent<SelectedTool>().outputtext.GetComponent<GUIText>().text = "Try again. Make sure to check for spelling \nerrors and read the directions carefully.";
					}
				}
				else {
					lg.taskscompleted[1]++;
					ToolSelectorObject.GetComponent<SelectedTool>().bonusTools[stateLib.TOOL_PRINTER_OR_QUESTION]++;
				    audioCorrect.Play();
					// Substring is startingPos, length. We want to start after the first color tag, and the length is the whole string - length of color tag - length of close color tag.
					string newtext = innertext.Substring(lg.stringLibrary.node_color_question.Length,(innertext.Length)-(lg.stringLibrary.node_color_question.Length)-(stringLib.CLOSE_COLOR_TAG.Length));
					string sOpenCommentSymbol = "# ";
					string sCloseCommentSymbol = "";
					switch(language){
						case "python": {
							sOpenCommentSymbol = "# ";
							sCloseCommentSymbol = "";
							break;
						}
						case "c++":
						case "c#":
						case "c": {
							sOpenCommentSymbol = "/* ";
							sCloseCommentSymbol = " */";
							break;
						}

					}
					lg.innerXmlLines[index] = lg.innerXmlLines[index].Replace(innertext, newtext + "\t\t" + lg.stringLibrary.node_color_comment + sOpenCommentSymbol + input + sCloseCommentSymbol + stringLib.CLOSE_COLOR_TAG);
					lg.DrawInnerXmlLinesToScreen();
				}
			}
			else if (Input.GetKeyDown(KeyCode.Backspace) && input.Length-1 >= 0) {
				input = input.Substring(0,input.Length-1);
				SidebarObject.GetComponent<GUIText>().text = displaytext + input;
			}
			else {
				string inputString = Input.inputString;
				input += inputString;
				SidebarObject.GetComponent<GUIText>().text = displaytext + input;
			}
		}
	}

	//.................................>8.......................................
	void OnTriggerEnter2D(Collider2D collidingObj) {
		if (collidingObj.name == stringLib.PROJECTILE_ACTIVATOR && !answered) {
			Destroy(collidingObj.gameObject);
			SidebarObject.GetComponent<GUIText>().text = displaytext;
			audioPrompt.Play();
			answering = true;
			lg.isAnswering = true;
			lg.toolsAirborne--;
		}
	}

	//.................................>8.......................................
}

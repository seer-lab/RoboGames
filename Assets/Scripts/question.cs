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
  
  	public Sprite qSpriteOff;
	public Sprite qSpriteOn;

	private bool answering = false;
	private bool answered = false;
	private string input = "";
	private LevelGenerator lg;

	//.................................>8.......................................
	// Use this for initialization
	void Start() {
		lg = CodescreenObject.GetComponent<LevelGenerator>();
		GetComponent<SpriteRenderer>().sprite = qSpriteOff;
		expectedArray = expected.Split(new String[] {", ", ","}, StringSplitOptions.RemoveEmptyEntries);
	}

	//.................................>8.......................................
	// Update is called once per frame
	void Update() {
		if (answering) {

			// Added escape option to return to game
			if (Input.GetKeyDown(KeyCode.Escape)) {
				// Increment number of tool uses
				if (ToolSelectorObject.GetComponent<SelectedTool>().toolCounts[stateLib.TOOL_PRINTER_OR_QUESTION] != 999)
				{
					ToolSelectorObject.GetComponent<SelectedTool>().toolCounts[stateLib.TOOL_PRINTER_OR_QUESTION]++;
				}
				answered = false;
				answering = false;
				lg.isAnswering = false;
				input = "";
				// Hide the pop-up window (Output.cs)
				SidebarObject.GetComponent<GUIText>().text = "";
			}
			else if ((Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))) {
				answered = true;
				answering = false;
				lg.isAnswering = false;
				
				
				/* This Fix is nonfunctional; to be removed
				
				// There's an odd case where if a user enters "3." instead of "3.0" for an expected of "3.0", it will be marked wrong
				// So we try casting the input as a decimal.
				decimal inputCastDecimal;
				string inputDecimalAsString;
				bool inputIsDecimal = decimal.TryParse(input, out inputCastDecimal);
				inputDecimalAsString = inputCastDecimal.ToString();
				if (inputIsDecimal && !inputDecimalAsString.Contains(".")) {
					input = inputDecimalAsString + ".0";
				}*/

				if (input != expected && Array.IndexOf(expectedArray, input) == -1) {
					// Incorrect Answer
					answered = false;
					string lastInput = input;
					input = "";
					// Check to see if the expected answer could be a decimal
					decimal expectedValue = -999;
					decimal inputValue = -999;
					// "out expectedValue" writes the result of the TryParse function to expectedValue.
					bool correctAnswerIsDecimal = decimal.TryParse(expected, out expectedValue);
					bool lastAnswerIsDecimal = decimal.TryParse(lastInput, out inputValue);
					bool expectedBool = false;
					bool correctAnswerIsBoolean = bool.TryParse(expected, out expectedBool);
					int inputInteger = -999;
					int expectedInteger = -999;
					bool lastAnswerIsIntegerValue = Int32.TryParse(lastInput, out inputInteger);
					bool correctAnswerIsIntegerValue = Int32.TryParse(expected, out expectedInteger);

					if (correctAnswerIsBoolean) {
						ToolSelectorObject.GetComponent<SelectedTool>().outputtext.GetComponent<GUIText>().text = "You should double check to make \nsure you have the right result; \nit is either 'true' or 'false', \nnothing else is possible.";
					}
					else if (correctAnswerIsDecimal && lastAnswerIsDecimal) {
						// Working with numbers
						if (expectedValue < inputValue) {
							ToolSelectorObject.GetComponent<SelectedTool>().outputtext.GetComponent<GUIText>().text = "Looks like your answer is too high; \ndid you forget to subtract a value?";
						}
						else if (expectedValue > inputValue) {
							ToolSelectorObject.GetComponent<SelectedTool>().outputtext.GetComponent<GUIText>().text = "Your answer is too low; \nperhaps you missed an addition somewhere?";
						}
						else if (!lastAnswerIsIntegerValue && correctAnswerIsIntegerValue) {
							ToolSelectorObject.GetComponent<SelectedTool>().outputtext.GetComponent<GUIText>().text = "Remember that integer variables do \nnot have decimal points; \nthey are whole numbers.";
						}
						else if (lastAnswerIsIntegerValue && !correctAnswerIsIntegerValue) {
							ToolSelectorObject.GetComponent<SelectedTool>().outputtext.GetComponent<GUIText>().text = "Remember that double variables have \ndecimal points; the number 5 would \nbe written as 5.0";
						}
					}
					else if (correctAnswerIsDecimal) {
						ToolSelectorObject.GetComponent<SelectedTool>().outputtext.GetComponent<GUIText>().text = "The answer should be a number value. \nTry again.";
					}
					else {
						ToolSelectorObject.GetComponent<SelectedTool>().outputtext.GetComponent<GUIText>().text = "Try again. Make sure to check for spelling \nerrors and read the directions carefully.";
					}
				}
				else {
					// Correct Answer
					GetComponent<SpriteRenderer>().sprite = qSpriteOn;
					lg.taskscompleted[1]++;
					ToolSelectorObject.GetComponent<SelectedTool>().bonusTools[stateLib.TOOL_PRINTER_OR_QUESTION]++;
				  audioCorrect.Play();
					// Substring is startingPos, length. We want to start after the first color tag, and the length is the whole string - length of color tag - length of close color tag.
					//TODO: the following line of code causes errors at times, double check what's going on
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
					//lg.innerXmlLines[index] = lg.innerXmlLines[index].Replace(innertext, newtext + "\t\t" + lg.stringLibrary.node_color_comment + sOpenCommentSymbol + input + sCloseCommentSymbol + stringLib.CLOSE_COLOR_TAG);
					//lg.DrawInnerXmlLinesToScreen();
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

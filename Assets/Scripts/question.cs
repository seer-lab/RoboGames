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
using UnityEngine.UI; 
using System;


public class question : Tools {

	public string innertext;
	public string expected;
	// Expected could be a CSV of acceptable answers or just one accepted string. If it's a CSV we need to break that up into an array called expectedArray
	public string[] expectedArray;

     public AudioSource audioPrompt;
     public AudioSource audioCorrect;
	
  	public Sprite qSpriteOff;
	public Sprite qSpriteOn;

	private bool answering = false;
	private bool answered = false;
	private string input = "";
	bool demoCompleteAnswer = false; 
	public Animator anim; 
	//TouchScreenKeyboard keyboard;
	public bool IsAnswerd {
		get{
			return answered;
		}
		set{
			answered =value;
		}
	}

    public override void Initialize()
    {
		if (IsAnswerd)GetComponent<SpriteRenderer>().sprite = qSpriteOn;
        else GetComponent<SpriteRenderer>().sprite = qSpriteOff;
        expectedArray = expected.Split(new String[] { ", ", "," }, StringSplitOptions.RemoveEmptyEntries);
		anim = GetComponent<Animator>(); 
    }
    //.................................>8.......................................
    // Update is called once per frame
    void Update() {
		if (answering) {
			/*
			if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer){
				keyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default, false, false, false, true);
			}
			*/

			// Added escape option to return to game
			 if (/*(keyboard != null && keyboard.status == TouchScreenKeyboard.Status.LostFocus) ||*/ Input.GetKeyDown(KeyCode.Escape)) {
				// Increment number of tool uses
				if (selectedTool.toolCounts[stateLib.TOOL_PRINTER_OR_QUESTION] != 999)
				{
					selectedTool.toolCounts[stateLib.TOOL_PRINTER_OR_QUESTION]++;
				}
				answered = false;
				answering = false;
				Output.IsAnswering = false;
				input = "";
                // Hide the pop-up window (Output.cs)
                output.Text.text = "";
			}
			else if ((/*(keyboard != null && keyboard.status == TouchScreenKeyboard.Status.Done)|| */ demoCompleteAnswer|| Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.KeypadEnter))) {
				answered = true;
				answering = false;
                Output.IsAnswering = false;
				/*
				if (keyboard != null ){
					input = keyboard.text;
				}
				*/
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
					    selectedTool.outputtext.GetComponent<Text>().text = "You should double check to make \nsure you have the right result; \nit is either 'true' or 'false', \nnothing else is possible.";
					}
					else if (correctAnswerIsDecimal && lastAnswerIsDecimal) {
						// Working with numbers
						if (expectedValue < inputValue) {
							selectedTool.outputtext.GetComponent<Text>().text = "Looks like your answer is too high; \ndid you forget to subtract a value?";
						}
						else if (expectedValue > inputValue) {
                            selectedTool.outputtext.GetComponent<Text>().text = "Your answer is too low; \nperhaps you missed an addition somewhere?";
						}
						else if (!lastAnswerIsIntegerValue && correctAnswerIsIntegerValue) {
                            selectedTool.outputtext.GetComponent<Text>().text = "Remember that integer variables do \nnot have decimal points; \nthey are whole numbers.";
						}
						else if (lastAnswerIsIntegerValue && !correctAnswerIsIntegerValue) {
                            selectedTool.outputtext.GetComponent<Text>().text = "Remember that double variables have \ndecimal points; the number 5 would \nbe written as 5.0";
						}
					}
					else if (correctAnswerIsDecimal) {
                        selectedTool.outputtext.GetComponent<Text>().text = "The answer should be a number value. \nTry again.";
					}
					else {
                        selectedTool.outputtext.GetComponent<Text>().text = "Try again. Make sure to check for spelling \nerrors and read the directions carefully.";
					}
					hero.onFail();
				}
				else {
					anim.SetTrigger("Complete");
					// Correct Answer
					GetComponent<SpriteRenderer>().sprite = qSpriteOn;
                    GlobalState.level.CompletedTasks[1]++;
                    selectedTool.bonusTools[stateLib.TOOL_PRINTER_OR_QUESTION]++;
				  audioCorrect.Play();
					// Substring is startingPos, length. We want to start after the first color tag, and the length is the whole string - length of color tag - length of close color tag.
					//TODO: the following line of code causes errors at times, double check what's going on
					string newtext = innertext.Substring(GlobalState.StringLib.node_color_question.Length,(innertext.Length)-(GlobalState.StringLib.node_color_question.Length)-(stringLib.CLOSE_COLOR_TAG.Length));
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
                selectedTool.outputtext.GetComponent<Text>().text = displaytext + input;
			}
			else if (GlobalState.level.IsDemo){
				input = expected; 
			}
			else {
				string inputString = Input.inputString;
				input += inputString;
                selectedTool.outputtext.GetComponent<Text>().text = displaytext + input;
			}
		}
	}

	//.................................>8.......................................
	void OnTriggerEnter2D(Collider2D collidingObj) {
		if (collidingObj.name == stringLib.PROJECTILE_ACTIVATOR && !answered) {
			Destroy(collidingObj.gameObject);
			if (GlobalState.level.IsDemo){
				selectedTool.outputtext.GetComponent<Text>().text = displaytext + expected;				
			}
            else selectedTool.outputtext.GetComponent<Text>().text = displaytext;
			audioSource.PlayOneShot(correct); 
			answering = true;
            Output.IsAnswering = true;

		}
		else if (collidingObj.name.Contains("projectile") && collidingObj.name != stringLib.PROJECTILE_ACTIVATOR){
			hero.onFail();
			audioSource.PlayOneShot(wrong); 
		}
	}

	//.................................>8.......................................
}

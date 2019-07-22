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
	public string[] options; 
	// Expected could be a CSV of acceptable answers or just one accepted string. If it's a CSV we need to break that up into an array called expectedArray
	public string[] expectedArray;

     public AudioSource audioPrompt;
     public AudioSource audioCorrect;

	 private GameObject leftArrow, rightArrow; 
	
  	public Sprite qSpriteOff;
	public Sprite qSpriteOn;

	private bool answering = false;
	private bool answered = false;
	private string input = "";
	private int optionsIndex = 0; 
	bool demoCompleteAnswer = false; 
	bool arrowShown = false; 

	public Animator anim; 
	bool failed = false; 
	int selectionCode = -1; 
	//TouchScreenKeyboard keyboard;
	public bool IsAnswerd {
		get{
			return answered;
		}
		set{
			answered =value;
		}
	}
	public void OnRightArrowClick(){
		selectionCode = stateLib.OUTPUT_RIGHT; 
	}
	public void OnLeftArrowClick(){
		selectionCode = stateLib.OUTPUT_LEFT; 
	}
	public void OnEnterClick(){
		selectionCode = stateLib.OUTPUT_ENTER; 
	}
    public override void Initialize()
    {
		if (IsAnswerd)GetComponent<SpriteRenderer>().sprite = qSpriteOn;
        else GetComponent<SpriteRenderer>().sprite = qSpriteOff;
        expectedArray = expected.Split(new String[] { ", ", "," }, StringSplitOptions.RemoveEmptyEntries);
		anim = GetComponent<Animator>(); 
		rightArrow = GameObject.Find("OutputCanvas").transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).gameObject; 
		leftArrow = GameObject.Find("OutputCanvas").transform.GetChild(0).transform.GetChild(0).transform.GetChild(1).gameObject; 
    }
	public void Cleaninnertext()
    {
        if (innertext.Contains("$bug"))
        {
            Regex ansRgx = new Regex(stringLib.BUG_REGEX);
            string answer = ansRgx.Match(innertext).Value;
            innertext = innertext.Replace("$bug" + answer + "$", "");
        }
        if (innertext.Contains("@"))
        {
            Regex paramRgx = new Regex(stringLib.DIALOG_REGEX);
            Match match = paramRgx.Match(innertext);
            while (match.Success)
            {
                string value = match.Value;
                innertext = innertext.Replace("@" + value + "@", "");
                match = match.NextMatch();
            }
        }
        if (innertext.Contains("!!!"))
        {
            innertext = innertext.Replace("!!!", "");
        }
        if (innertext.Contains("???"))
        {
            innertext = innertext.Replace("???", "");
        }
        string[] text = innertext.Split('\n');
        for (int i = 0; i < text.Length; i++)
        {
            GlobalState.level.Code[index + i] = text[i];
        }
    }
    //.................................>8.......................................
    // Update is called once per frame
    void Update() {
		if (hero.projectilecode == stateLib.TOOL_PRINTER_OR_QUESTION){
            EmphasizeTool(); 
        }else DeEmphasizeTool(); 
		if (answering) {

			// Show the arrows when the output is being used for this object.
			if (!arrowShown){
				rightArrow.GetComponent<Image>().enabled = true; 
				leftArrow.GetComponent<Image>().enabled = true; 
				arrowShown = true; 
				if (!GlobalState.level.IsDemo){
					rightArrow.GetComponent<Button>().onClick.AddListener(OnRightArrowClick);
					leftArrow.GetComponent<Button>().onClick.AddListener(OnLeftArrowClick); 
					output.enter.GetComponent<Button>().onClick.AddListener(OnEnterClick); 
				}
			}

			// Added escape option to return to game
			 if ( Input.GetKeyDown(KeyCode.Escape)) {
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
			//player has chosen to select their answer
			else if (( demoCompleteAnswer|| (Input.GetMouseButtonDown(0) && GlobalState.level.IsDemo) || Input.GetKeyDown(KeyCode.Return) || selectionCode == stateLib.OUTPUT_ENTER || Input.GetKeyDown(KeyCode.KeypadEnter))) {
				answered = true;
				answering = false;
				selectionCode = -1; 
                Output.IsAnswering = false;

				//Hide the arrows for the next output user.
				if (arrowShown){
					rightArrow.GetComponent<Image>().enabled = false; 
					leftArrow.GetComponent<Image>().enabled = false; 
					arrowShown = false; 
					if (!GlobalState.level.IsDemo){
						rightArrow.GetComponent<Button>().onClick.RemoveListener(OnRightArrowClick);
						leftArrow.GetComponent<Button>().onClick.RemoveListener(OnLeftArrowClick); 
						output.enter.GetComponent<Button>().onClick.RemoveListener(OnEnterClick);  
					}
				}
				//When the input is not the correct answer
				if (input != expected && Array.IndexOf(expectedArray, input) == -1 && !GlobalState.level.IsDemo) {
					// Incorrect Answer
					answered = false;
					failed = true; 
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
					
					//provide tips based on their input.	
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
				//when the input is the correct answer
				else {
					input = expected; 
					if (failed) GlobalState.CurrentLevelPoints += stateLib.POINTS_QUESTION/2; 
					else GlobalState.CurrentLevelPoints+= stateLib.POINTS_QUESTION; 
					selectedTool.outputtext.GetComponent<Text>().text = displaytext + input;
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
				}
			}
			//handle right arrow click/input
			else if (Input.GetKeyDown(KeyCode.RightArrow) || selectionCode == stateLib.OUTPUT_RIGHT){
				selectionCode = -1; 
				optionsIndex = (optionsIndex +1 < options.Length) ? optionsIndex+1 : optionsIndex; 
				input = options[optionsIndex]; 
				selectedTool.outputtext.GetComponent<Text>().text = displaytext + input; 
			}
			//handle left arrow click /input
			else if (Input.GetKeyDown(KeyCode.LeftArrow) || selectionCode == stateLib.OUTPUT_LEFT){
				selectionCode = -1; 
				optionsIndex = (optionsIndex -1 >= 0) ? optionsIndex-1 : 0; 
				input = options[optionsIndex]; 
				selectedTool.outputtext.GetComponent<Text>().text = displaytext + input; 
			}

		}
		
	}
	/// <summary>
	/// Auto play interaction with output 
	/// for the demo
	/// </summary>
	/// <returns></returns>
	IEnumerator DemoPlay(){
		//cycle until the correct answer is reached
		for (int i = 0; i < options.Length; i++){
			yield return new WaitForSecondsRealtime(0.3f);
			if (options[i] == expected) {
				optionsIndex = i; 
				break; 
			}
			input = options[i]; 
			selectedTool.outputtext.GetComponent<Text>().text = displaytext + input; 
		} 
		input = options[optionsIndex]; 
		selectedTool.outputtext.GetComponent<Text>().text = displaytext + input; 
		//wait until the player is ready to contine.
		while(!Input.GetKeyDown(KeyCode.Return) || !Input.GetMouseButtonDown(0)){
			yield return null; 
		}
		output.Text.text = ""; 
		demoCompleteAnswer = true; 
	}
	//.................................>8.......................................
	void OnTriggerEnter2D(Collider2D collidingObj) {
		if (collidingObj.name == stringLib.PROJECTILE_ACTIVATOR && !answered) {
			Destroy(collidingObj.gameObject);
			Debug.Log(optionsIndex); 
			input = options[optionsIndex];
			if (GlobalState.level.IsDemo){
				hero.GetComponent<DemoBotControl>().InsertOptionAction(stateLib.TOOL_PRINTER_OR_QUESTION); 
				selectedTool.outputtext.GetComponent<Text>().text = displaytext + input;
				StartCoroutine(DemoPlay()); 				
			}
            else selectedTool.outputtext.GetComponent<Text>().text = displaytext + input;
			audioSource.PlayOneShot(correct); 
			answering = true;
            Output.IsAnswering = true;

		}
		else if (collidingObj.name.Contains("projectile") && collidingObj.name != stringLib.PROJECTILE_ACTIVATOR){
			audioSource.PlayOneShot(wrong); 
		}
	}

	//.................................>8.......................................
}

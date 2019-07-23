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
	bool failed = false; 
	private bool answering = false;
	private bool decolorOnce = false;
	private bool colorOnce = false;
	private GameObject leftArrow, rightArrow; 
	public Sprite renSpriteOff;
	public Sprite renSpriteOn;
	bool entered = false; 
	private int selection = 0;
	private bool hasHappened = false;
	private bool isChecked = false;
	bool arrowShown = false; 
	int selectionCode = -1; 

/// <summary>
/// A method that initializes the game object
/// </summary>
/// <remarks> 
///Since VariableRenamer.cs is not in use
/// We must use the object to rename the string
/// </remarks>
    public override void Initialize()
    {
		if (answered)GetComponent<SpriteRenderer>().sprite = renSpriteOn;
        else GetComponent<SpriteRenderer>().sprite = renSpriteOff;
		int i = 0;
		bool startMultiComments= false;
		bool endMultiComments= false;
		rightArrow = GameObject.Find("OutputCanvas").transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).gameObject; 
		leftArrow = GameObject.Find("OutputCanvas").transform.GetChild(0).transform.GetChild(0).transform.GetChild(1).gameObject;

		//find elements in the code that have the same name as it 

		foreach(string s in GlobalState.level.Code){
			//Sorry for the naming, I suck at it. (LITERALLY)
			//Variable rgxO will match the oldname with spaces before and after the word
			//Variable rgxT will match if there is an existing color tag
			//Variable rgxTh does the same as rgxT
			//Variable rgxF matches the string that is between the quotation mark
			//Variable rgxFiv matches the string with only one space after the word
			Regex rgxO = new Regex(@"\b" + oldname + @"\b");
			Regex rgxT = new Regex("(?s)(.*)(<color=#ff00ffff>)(.*)(</color>)(.*)");
			Regex rgxTh = new Regex(@"<color=#00ff00ff>[\s\S]*?<\/color>");
			//https://stackoverflow.com/questions/171480/regex-grabbing-values-between-quotation-marks
			Regex rgxF = new Regex("\"(.*?)\"");
			Regex rgxFiv = new Regex(oldname+@"\b");

			//Check if its python or noth
			if(GlobalState.Language.ToLower() == "python"){
				if(rgxF.IsMatch(s) && !(rgxO.IsMatch(s) || rgxFiv.IsMatch(s))){
					if(rgxFiv.IsMatch(s)){
						GlobalState.level.Code[i] = rgxFiv.Replace(GlobalState.level.Code[i], "\v" + oldname + "\v");
					}
					i++;
					continue;
				}else if((rgxO.IsMatch(s) || rgxFiv.IsMatch(s)) && !rgxT.IsMatch(s) && !rgxTh.IsMatch(s)
					&& !s.Contains("<color=#00ff00ff>") && !s.Contains(stringLib.CLOSE_COLOR_TAG) && !startMultiComments && !endMultiComments){
					GlobalState.level.Code[i] = rgxO.Replace(GlobalState.level.Code[i], "<color=#ff00ffff>" + oldname +"</color>");
				}else if(rgxTh.IsMatch(s)){
					GlobalState.level.Code[i] = rgxO.Replace(GlobalState.level.Code[i], "\v" + oldname + "\v");
				}else if(rgxO.IsMatch(s) && !s.Contains("'''")){
					GlobalState.level.Code[i] = rgxO.Replace(GlobalState.level.Code[i], "<color=#ff00ffff>" + oldname +"</color>");
				}else if(rgxO.IsMatch(s) || rgxFiv.IsMatch(s)){
					GlobalState.level.Code[i] = rgxO.Replace(GlobalState.level.Code[i], "\v" + oldname + "\v");
				}
				i++;

			}else{
				//check if its a multiline comment
				if(s.Contains("/*") || s.Contains("/**")){
					startMultiComments = true;
				}else if(s.Contains("*/") || s.Contains("**/")){
					startMultiComments = false;
				}

				//If the string is between the quotation mark, we want to check if its outside of the quotation mark
				//Eg with moon being the word cout << "The fox jumped over the " + moon
				//since the first check will see if there is a quotation mark, we want to check if its outside of it
				if(rgxF.IsMatch(s)){
					if(rgxFiv.IsMatch(s)){
						GlobalState.level.Code[i] = rgxFiv.Replace(GlobalState.level.Code[i], "\v" + oldname + "\v");
					}
					i++;
					continue;
				//check if its not in a multilime comment and doesnt have the tag attached to it
				}else if(rgxO.IsMatch(s) && !rgxT.IsMatch(s) && !rgxTh.IsMatch(s)
					&& !s.Contains("<color=#00ff00ff>") && !s.Contains(stringLib.CLOSE_COLOR_TAG) && !startMultiComments && !endMultiComments){
					GlobalState.level.Code[i] = rgxO.Replace(GlobalState.level.Code[i], "<color=#ff00ffff>" + oldname +"</color>");
				//check if it is between the comment, and will place a placeholder for the CorrectComment to see and color it apporpriatly
				}else if(rgxTh.IsMatch(s)){
					GlobalState.level.Code[i] = rgxO.Replace(GlobalState.level.Code[i], "\v" + oldname + "\v");
				//check if it is between the comment, and will place a placeholder for the CorrectComment to see and color it apporpriatly
				}else if(startMultiComments){
					GlobalState.level.Code[i] = rgxO.Replace(GlobalState.level.Code[i], "\v" + oldname + "\v");
				//ANother *hacky check to see if it can color it*
				}else if(rgxO.IsMatch(s) && !s.Contains("*/") && !s.Contains("**/")){
					GlobalState.level.Code[i] = rgxO.Replace(GlobalState.level.Code[i], "<color=#ff00ffff>" + oldname +"</color>");
				//ANother *hacky check to see if it can place a placeholder*
				}else if(rgxO.IsMatch(s) || rgxFiv.IsMatch(s)){
					GlobalState.level.Code[i] = rgxO.Replace(GlobalState.level.Code[i], "\v" + oldname + "\v");
				}
				i++;
			}
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
	//.................................>8.......................................
	// Update is called once per frame
	void Update() {
		if (hero.projectilecode == stateLib.TOOL_WARPER_OR_RENAMER){
            EmphasizeTool(); 
        }else DeEmphasizeTool(); 
		if (answering) {
			// Handle left and right arrows --[
			if (selection == 0) {
				output.Text.text = displaytext + "   " + options[selection];
			}
			else if (selection == options.Count-1) {
                output.Text.text = displaytext + "  " + options[selection];
			}
			else {
                output.Text.text = displaytext + "  " + options[selection];
			}
			//show arrow keys for answering this question
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
			if ((Input.GetKeyDown(KeyCode.Return) ||  (Input.GetMouseButtonDown(0) && GlobalState.level.IsDemo)||Input.GetKeyDown(KeyCode.KeypadEnter) || entered|| selectionCode == stateLib.OUTPUT_ENTER)) {
				answered = true;
				answering = false;
				
                Output.IsAnswering = false;

				//hide arrow keys after answering the question.
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
				if (GlobalState.level.IsDemo) selection = options.IndexOf(correct); 
				// completed the task incorrectly.
				if (selection != options.IndexOf(correct)) {
					answered = false;
					failed = true; 
					hero.onFail(); 
					selectedTool.outputtext.GetComponent<Text>().text = "The name you chose isn't the best option for\nthis variable's purpose.\nWhat is this variable used for?";
					selectionCode = -1; 
				}
				//completed the task correctly.
				else {
					if (failed) GlobalState.CurrentLevelPoints+= stateLib.POINTS_RENAMER/2; 
					else GlobalState.CurrentLevelPoints+= stateLib.POINTS_RENAMER; 
					// Award 1 extra use of the tool.
					selectedTool.bonusTools[stateLib.TOOL_WARPER_OR_RENAMER]++;
					audioCorrect.Play();
					// Change this object to the correct text
					GetComponent<SpriteRenderer>().sprite = renSpriteOn;

					int iter = 0;
					Regex rgx = new Regex(@"(?s)(.*)(<color=#ff00ffff>)(.*?)(</color>)(.*)");
					Regex rgxT = new Regex(@"(?s)(.*)(<color=#ff00ff00>)(.*?)(</color>)(.*)");
					Regex tmp = new Regex(@"\v"+oldname+"\v");
					// /GlobalState.level.Code[index] = rgx.Replace(GlobalState.level.Code[index], "$1$3$5");
					foreach(string s in GlobalState.level.Code) {
						rgx = new Regex(@"\b"+oldname+@"\b");
						if(rgx.IsMatch(s) && !rgxT.IsMatch(s)){
							//GlobalState.level.Code[iter] = GlobalState.level.Code[iter].Replace(oldname,correct);
							GlobalState.level.Code[iter] = rgx.Replace(GlobalState.level.Code[iter],correct);
						}else if(tmp.IsMatch(s)){
							GlobalState.level.Code[iter] = tmp.Replace(GlobalState.level.Code[iter], "<color=#ff00ffff>"+correct + "</color>");
						}else{
							GlobalState.level.Code[iter] = rgx.Replace(GlobalState.level.Code[iter],"\v"+correct + "\v");
						}
						iter += 1;
					}

					
					output.Text.text= "";
					lg.renamegroupidCounter++;
                    GlobalState.level.CompletedTasks[2]++;
                    lg.DrawInnerXmlLinesToScreen();
					selectionCode = -1; 

				}
			}
			else if (Input.GetKeyDown(KeyCode.RightArrow) || selectionCode == stateLib.OUTPUT_RIGHT) {
				selection = (selection + 1 <= options.Count - 1) ? selection + 1 : options.Count - 1;
				selectionCode = -1; 
			}
			else if (Input.GetKeyDown(KeyCode.LeftArrow)|| selectionCode == stateLib.OUTPUT_LEFT) {
				selection = (selection - 1 >= 0) ? selection - 1 : 0;
				selectionCode = -1; 
			}
			// ]-- End of input handling
		}
		else if (lg.renamegroupidCounter != groupid && decolorOnce != true) {
			// Change the next groupid objects to the new colors
			decolorOnce = true;
			lg.DrawInnerXmlLinesToScreen();
		}
		
	}
	/// <summary>
	/// Auto play interaction with output 
	/// for the demo
	/// </summary>
	/// <returns></returns>
	IEnumerator DemoPlay(){
		for (int i = 0; i < options.Count; i++){
			yield return new WaitForSecondsRealtime(0.4f);
			selection = i; 
			if (options[i] == correct) {
				break; 
			}
		} 
		while(!Input.GetKeyDown(KeyCode.Return) || !Input.GetMouseButtonDown(0)){
			yield return null; 
		}
		output.Text.text = ""; 
		entered = true; 
	}

	//.................................>8.......................................
	void OnTriggerEnter2D(Collider2D collidingObj) {
		if (collidingObj.name == stringLib.PROJECTILE_WARP && !answered) {
			Destroy(collidingObj.gameObject);
			output.Text.text = displaytext;
			audioPrompt.Play();
			answering = true;
            Output.IsAnswering = true;
			audioSource.PlayOneShot(base.correct);
			if (GlobalState.level.IsDemo){
				hero.GetComponent<DemoBotControl>().InsertOptionAction(stateLib.TOOL_WARPER_OR_RENAMER); 
				StartCoroutine(DemoPlay());
			}
			  
		}
		else if (collidingObj.name.Contains("projectile")){

			audioSource.PlayOneShot(wrong);
		}
	}

	//.................................>8.......................................
}

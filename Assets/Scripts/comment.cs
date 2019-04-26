//**************************************************//
// Class Name: comment
// Class Description: Instantiable object in the Robot ON! game. This class is the controller for
//                    the comment tasks, and is paired with the Commenter and Un-commenter tool.
// Methods:
// 		void Start()
//		void Update()
//		void OnTriggerEnter2D(Collider2D collidingObj)
// Author: Scott McLean
// Date Last Modified: 6/24/2016
//**************************************************//

using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI; 
using System.Text.RegularExpressions;

public class comment : MonoBehaviour {

	public int index = -1;
	public bool isCommented;
	public string commentStyle;
	public int entityType = -1;
	public int groupid    = -1;
	public int size = -1;
	public int[] tools = new int[stateLib.NUMBER_OF_TOOLS];
	public string oldtext   = "";
	public string blocktext = "";
	public string errmsg    = "";
	public string language;
	public GameObject CodeObject;
	public GameObject CodescreenObject;
	public GameObject CorrectCommentObject;
	public GameObject SidebarObject;
	public GameObject ToolSelectorObject;
	
	public Sprite descSpriteOff;
	public Sprite descSpriteOn;
	public Sprite codeSpriteOff;
	public Sprite codeSpriteOn;

	private LevelGenerator lg;
	private bool doneUpdating = false;

	private bool resetting  = false;
	private bool toolgiven = false;
	private float resetTime = 0f;
	private float timeDelay = 30f;

    TextColoration textColoration; 
	//.................................>8.......................................
	// Use this for initialization
	void Start() {
		lg = CodescreenObject.GetComponent<LevelGenerator>();
		if (entityType == stateLib.ENTITY_TYPE_CORRECT_COMMENT || entityType == stateLib.ENTITY_TYPE_INCORRECT_COMMENT){
			GetComponent<SpriteRenderer>().sprite = descSpriteOff;
		}
		else {
			GetComponent<SpriteRenderer>().sprite = codeSpriteOff;
		}
        textColoration = new TextColoration(); 
	}

	//.................................>8.......................................
	// Update is called once per frame
	void Update() {
		switch(entityType) {
			case stateLib.ENTITY_TYPE_INCORRECT_COMMENT:
			case stateLib.ENTITY_TYPE_INCORRECT_UNCOMMENT: UpdateIncorrect(); break;
			case stateLib.ENTITY_TYPE_ROBOBUG_COMMENT:  break;
			default: break;
		}
	}

	//.................................>8.......................................
	void OnTriggerEnter2D(Collider2D collidingObj) {
		switch(entityType) {
			case stateLib.ENTITY_TYPE_CORRECT_COMMENT: TriggerCorrectComment(collidingObj); break;
			case stateLib.ENTITY_TYPE_CORRECT_UNCOMMENT: TriggerCorrectUncomment(collidingObj); break;
			case stateLib.ENTITY_TYPE_INCORRECT_COMMENT: TriggerIncorrectComment(collidingObj); break;
			case stateLib.ENTITY_TYPE_INCORRECT_UNCOMMENT: TriggerIncorrectUncomment(collidingObj); break;
			case stateLib.ENTITY_TYPE_ROBOBUG_COMMENT: TriggerRoboBUGComment(collidingObj); break;
			default: break;
		}
	}

	//.................................>8.......................................
	void TriggerCorrectComment(Collider2D collidingObj) {
		if (collidingObj.name == stringLib.PROJECTILE_COMMENT && !isCommented) {
			GetComponent<SpriteRenderer>().sprite = descSpriteOn;
			isCommented = true;
			Destroy(collidingObj.gameObject);
			GetComponent<AudioSource>().Play();
			ToolSelectorObject.GetComponent<SelectedTool>().bonusTools[stateLib.TOOL_COMMENTER]++;
			string sNewText = blocktext;
			string[] sNewParts = sNewText.Split('\n');
			string multilineCommentOpenSymbolPython = @"'''";
			string multilineCommentCloseSymbolPython = @"'''";
			string multilineCommentOpenSymbolCpp = @"/* ";
			string multilineCommentCloseSymbolCpp = @" */";
			string singlelineCommentOpenSymbolPython = @"# ";
			string singlelineCommentOpenSymbolCpp = @"// ";
			string commentOpenSymbol = multilineCommentOpenSymbolPython;
			string commentCloseSymbol = multilineCommentCloseSymbolPython;
			switch(language) {
				case "python": {
					commentOpenSymbol = (commentStyle == "multi") ? multilineCommentOpenSymbolPython : singlelineCommentOpenSymbolPython;
					commentCloseSymbol = (commentStyle == "multi") ? multilineCommentCloseSymbolPython : "";
					break;
				}
				case "c++":
				case "c":
				case "c#": {
					commentOpenSymbol = (commentStyle == "multi") ? multilineCommentOpenSymbolCpp : singlelineCommentOpenSymbolCpp;
					commentCloseSymbol = (commentStyle == "multi") ? multilineCommentCloseSymbolCpp : "";
					break;
				}
				default: {
					commentOpenSymbol = (commentStyle == "multi") ? multilineCommentOpenSymbolPython : singlelineCommentOpenSymbolPython;
					commentCloseSymbol = (commentStyle == "multi") ? multilineCommentCloseSymbolPython : "";
					break;
				}
			}
			if (sNewParts.Length == 1) {
                // Single line
                // Add comment style around the text
                //lg.innerXmlLines[index] = lg.innerXmlLines[index].Replace(blocktext, lg.stringLibrary.node_color_correct_comment + commentOpenSymbol + blocktext + commentCloseSymbol + stringLib.CLOSE_COLOR_TAG);
                GlobalState.level.Code[index] = lg.stringLibrary.node_color_correct_comment + commentOpenSymbol + blocktext + commentCloseSymbol + stringLib.CLOSE_COLOR_TAG;
			}
			else {
				// Multi line
				sNewParts[0] = lg.stringLibrary.node_color_correct_comment + commentOpenSymbol + sNewParts[0] + stringLib.CLOSE_COLOR_TAG;
				for (int i = 1 ; i < sNewParts.Length - 1 ; i++) {
					sNewParts[i] = (commentStyle == "multi") ? lg.stringLibrary.node_color_correct_comment + sNewParts[i] + stringLib.CLOSE_COLOR_TAG :
															   lg.stringLibrary.node_color_correct_comment + commentOpenSymbol + sNewParts[i] + commentCloseSymbol + stringLib.CLOSE_COLOR_TAG;
				}
				sNewParts[sNewParts.Length-1] = (commentStyle == "multi") ? lg.stringLibrary.node_color_correct_comment + sNewParts[sNewParts.Length-1] + commentCloseSymbol + stringLib.CLOSE_COLOR_TAG :
																			lg.stringLibrary.node_color_correct_comment + commentOpenSymbol + sNewParts[sNewParts.Length-1] + stringLib.CLOSE_COLOR_TAG;

				for (int i = 0 ; i < sNewParts.Length ; i++) {
                    GlobalState.level.Code[index+i] = sNewParts[i];
				}
			}

			lg.DrawInnerXmlLinesToScreen();
			lg.toolsAirborne--;
            GlobalState.level.CompletedTasks[3]++;

		}
	}

	//.................................>8.......................................
	void TriggerCorrectUncomment(Collider2D collidingObj) {
		
		//TODO NOTE: This section is a bit of a mess and needs cleaning ^_^
		if (collidingObj.name == stringLib.PROJECTILE_DEBUG && !isCommented) {
			GetComponent<SpriteRenderer>().sprite = codeSpriteOn;
			Destroy(collidingObj.gameObject);
			GetComponent<AudioSource>().Play();
            GlobalState.level.CompletedTasks[4]++;
			ToolSelectorObject.GetComponent<SelectedTool>().bonusTools[stateLib.TOOL_CONTROL_FLOW]++;
			string sNewText = textColoration.DecolorizeText(blocktext);
			string tempDecolText = sNewText;
			string[] sNewParts = sNewText.Split('\n');
			if (sNewParts.Length == 1) {
				// Single line
				// Look for /* something */
				string multilinePatternCommentCpp = @"(\/\*)(.*)(\*\/)";
				// Look for ''' something '''
				string multilinePatternCommentPython = @"(\'\'\')(.*)(\'\'\')";
				// Look for //something
				string singlelinePatternCommentCpp = (@"(\/\/)(.*)");
				// Look for #something
				string singlelinePatternCommentPython = @"(#)(.*)";

				string patternComment = singlelinePatternCommentPython;
				switch(language) {
					case "python": {
						patternComment = (commentStyle == "multi") ? multilinePatternCommentPython : singlelinePatternCommentPython;
						break;
					}
					case "c++":
					case "c":
					case "c#": {
						patternComment = (commentStyle == "multi") ? multilinePatternCommentCpp : singlelinePatternCommentCpp;
						break;
					}
					default: {
						patternComment = (commentStyle == "multi") ? multilinePatternCommentPython : singlelinePatternCommentPython;
						break;
					}
				}
				Regex rgx = new Regex(patternComment);
				sNewText = rgx.Replace(sNewText, "$2");
				//todo: refactor this quick hack
				rgx = new Regex(@"(\/\*)(.*)(\*\/)");
				sNewText = rgx.Replace(sNewText, "$2");	
				rgx = new Regex(@"(\/\/)(.*?)");
				sNewText = rgx.Replace(sNewText, "$2");	
				
				
				//verify comment color is removed
				tempDecolText = textColoration.DecolorizeText(sNewText);
				
				sNewText = textColoration.ColorizeText(tempDecolText, language);
                GlobalState.level.Code[index] = sNewText;
			}
			else {
				string commentOpenSymbol = "/*";
				string commentCloseSymbol = "*/"; //TODO: Modularize
			
				sNewParts[0] = sNewParts[0].Replace(lg.stringLibrary.node_color_correct_comment, "");
				sNewParts[0] = sNewParts[0].Replace(commentOpenSymbol, "");
				sNewParts[sNewParts.Length-1] = sNewParts[sNewParts.Length-1].Replace(commentCloseSymbol, "");
				sNewParts[sNewParts.Length-1] = sNewParts[sNewParts.Length-1].Replace(stringLib.CLOSE_COLOR_TAG, "");

                GlobalState.level.Code[index] = textColoration.ColorizeText(sNewParts[0], language);
                GlobalState.level.Code[index+sNewParts.Length-1] = textColoration.ColorizeText(sNewParts[sNewParts.Length-1], language);

				

			}

			lg.DrawInnerXmlLinesToScreen();
			isCommented = true;
			lg.toolsAirborne--;
		}
	}

	//.................................>8.......................................
	void TriggerIncorrectComment(Collider2D collidingObj) {
		if (collidingObj.name == stringLib.PROJECTILE_COMMENT && !doneUpdating) {
			Destroy(collidingObj.gameObject);
			lg.toolsAirborne--;
			ToolSelectorObject.GetComponent<SelectedTool>().outputtext.GetComponent<Text>().text = "This comment does not correctly describe \nthe code; a nearby comment better explains \nwhat is taking place.";
		}
	}

	//.................................>8.......................................
	void TriggerIncorrectUncomment(Collider2D collidingObj) {
		if (collidingObj.name == stringLib.PROJECTILE_DEBUG && !doneUpdating) {
			Destroy(collidingObj.gameObject);
			lg.toolsAirborne--;
			ToolSelectorObject.GetComponent<SelectedTool>().outputtext.GetComponent<Text>().text = "There are errors with the selected code; \nfigure out what the mistake is, then \nuncomment the correct solution.";
		}
	}

	//.................................>8.......................................
	void TriggerRoboBUGComment(Collider2D collidingObj) {
		if (collidingObj.name == stringLib.PROJECTILE_COMMENT) {
			Logger.printLogFile(stringLib.LOG_COMMENT_ON, this.transform.position);
			Destroy(collidingObj.gameObject);
			GetComponent<AudioSource>().Play();
			// Substring is startingPos, length. We want to start after the first color tag, and the length is the whole string - length of color tag - length of close color tag.
			blocktext = blocktext.Substring(lg.stringLibrary.node_color_question.Length,(blocktext.Length)-(lg.stringLibrary.node_color_question.Length)-(stringLib.CLOSE_COLOR_TAG.Length));
			lg.DrawInnerXmlLinesToScreen();

			// CodeObject.GetComponent<TextMesh>().text = oldtext.Replace(blocktext, stringLib.comment_block_color_tag + "\*" +
			// 																	  blocktext.Replace("/**/","") +
			// 																	  " */" + stringLib.CLOSE_COLOR_TAG);
			SidebarObject.GetComponent<Text>().text = errmsg;
			resetTime = Time.time + timeDelay;
			resetting = true;

			// Award bonus tools if applicable
			if (!toolgiven) {
				toolgiven = true;
				for (int i = 0; i < stateLib.NUMBER_OF_TOOLS; i++) {
					if (tools[i] > 0) lg.floatingTextOnPlayer("New Tools!");
					ToolSelectorObject.GetComponent<SelectedTool>().toolCounts[i] += tools[i];
				}
			}
			lg.toolsAirborne--;
		}
	}

	//.................................>8.......................................
	void UpdateIncorrect() {
		if (CorrectCommentObject) {
			if (CorrectCommentObject.GetComponent<comment>().isCommented && !doneUpdating) {
				doneUpdating = true;
				if (entityType == stateLib.ENTITY_TYPE_INCORRECT_COMMENT){
					GetComponent<SpriteRenderer>().sprite = descSpriteOn;
				}
				else{
					GetComponent<SpriteRenderer>().sprite = codeSpriteOn;
				}
				string sNewText = blocktext;
				string[] sNewParts = sNewText.Split('\n');
				if (sNewParts.Length == 1) {
					// Single line
					
				    //verify comment color is removed
				    GlobalState.level.Code[index] = textColoration.DecolorizeText(GlobalState.level.Code[index]);

                    GlobalState.level.Code[index] = "";
				}
				else {
					// Multi line
					for (int i = 0 ; i < sNewParts.Length ; i++) {
                        GlobalState.level.Code[index+i] = "";
					}
				}
				lg.DrawInnerXmlLinesToScreen();
			}
		}
	}

}

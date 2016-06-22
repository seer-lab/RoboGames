//**************************************************//
// Class Name: checker
// Class Description:
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


public class checker : MonoBehaviour {

	public string innertext;
	public string displaytext = "";
	public string expected;
	public GameObject SidebarObject;
	public GameObject CodeObject;
	public GameObject CodescreenObject;

	private bool answering = false;
	private bool answered = false;
	private string input = "";
	private LevelGenerator lg;

	//.................................>8.......................................
	// Use this for initialization
	void Start() {
		lg = CodescreenObject.GetComponent<LevelGenerator>();
	}

	//.................................>8.......................................
	// Update is called once per frame
	void Update() {
		if (answering) {
			if (Input.GetKeyDown(KeyCode.Return)) {
				answered = true;
				answering = false;
				if (expected != input) {
					lg.isLosing = true;
				}
				else {
					lg.taskscompleted[1]++;
					GetComponent<AudioSource>().Play();
					innertext = innertext.Substring(23,innertext.Length-38);
					CodeObject.GetComponent<TextMesh>().text = CodeObject.GetComponent<TextMesh>()
															   .text
															   .Replace(stringLib.NODE_COLOR_ON_CHECK + innertext + stringLib.CLOSE_COLOR_TAG, innertext +
															 					   "     " +
																				   stringLib.NODE_COLOR_COMMENT +
																				   " " +
																				   expected +
																				   " " +
																				   stringLib.COMMENT_CLOSE_COLOR_TAG);
				//Regex rgx = new Regex("(" + stringLib.CHECKER_TEXT_COLOR_TAG + ")" + "(" + innertext + ")" + "(" + stringLib.CLOSE_COLOR_TAG + ")");
				//lg.codetext = rgx.Replace(innertext, "$2" + stringLib.NODE_COLOR_COMMENT + "   // " + expected + stringLib.COMMENT_CLOSE_COLOR_TAG);
				}
			}
			else if (Input.GetKeyDown(KeyCode.Backspace)) {
				input = input.Substring(0,input.Length-1);
				SidebarObject.GetComponent<GUIText>().text = displaytext + input;
			}
			else {
				string inputString = Input.inputString;
				//	Regex rgx = new Regex("[A-Za-z0-9]");
				//if (rgx.Equals(inputString)) {
				input += inputString;
				SidebarObject.GetComponent<GUIText>().text = displaytext + input;
				//}
			}
		}
	}

	//.................................>8.......................................
	void OnTriggerEnter2D(Collider2D collidingObj) {
		if (collidingObj.name == stringLib.PROJECTILE_ACTIVATOR && !answered) {
			Destroy(collidingObj.gameObject);
			SidebarObject.GetComponent<GUIText>().text = displaytext;
			GetComponent<AudioSource>().Play();
			answering = true;
		}
	}

	//.................................>8.......................................
}

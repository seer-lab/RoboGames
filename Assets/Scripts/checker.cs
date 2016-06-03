//**************************************************//
// Class Name: checker
// Class Description:
// Methods:
// 		void Start()
//		void Update()
//		void OnTriggerEnter2D(Collider2D c)
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
	public GameObject sidebar;
	public GameObject code;
	public GameObject codescreen;

	private bool answering = false;
	private bool answered = false;
	private string input = "";
	private LevelGenerator lg;

	//.................................>8.......................................
	// Use this for initialization
	void Start() {
		lg = codescreen.GetComponent<LevelGenerator>();
	}

	//.................................>8.......................................
	// Update is called once per frame
	void Update() {
		if (answering) {
			if (Input.GetKeyDown(KeyCode.Return)) {
				answered = true;
				answering = false;
				if (expected != input) {
					lg.losing = true;
				}
				else {
					lg.taskscompleted[1]++;
					GetComponent<AudioSource>().Play();
					//@TODO: Need this substring explained
					// Chomping color tag
					innertext = innertext.Substring(23,innertext.Length-37);
					code.GetComponent<TextMesh>().text = code.GetComponent<TextMesh>()
															 .text
															 .Replace(stringLib.CHECKER_TEXT_COLOR_TAG + innertext +stringLib.CLOSE_COLOR_TAG, innertext);
				}
			}
			else if (Input.GetKeyDown(KeyCode.Backspace)) {
				input = input.Substring(0,input.Length-1);
				sidebar.GetComponent<GUIText>().text = displaytext + input;
			}
			else {
				string inputString = Input.inputString;
				//	Regex rgx = new Regex("[A-Za-z0-9]");
				//if (rgx.Equals(inputString)) {
				input += inputString;
				sidebar.GetComponent<GUIText>().text = displaytext + input;
				//}
			}
		}
	}

	//.................................>8.......................................
	void OnTriggerEnter2D(Collider2D c) {
		if (c.name == stringLib.PROJECTILE_ACTIVATOR && !answered) {
			Destroy(c.gameObject);
			sidebar.GetComponent<GUIText>().text = displaytext;
			GetComponent<AudioSource>().Play();
			answering = true;
		}
	}

	//.................................>8.......................................
}

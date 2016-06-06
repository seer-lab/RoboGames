//**************************************************//
// Class Name: rename
// Class Description:
// Methods:
// 		void Start()
//		void Update()
//		void OnTriggerEnter2D(Collider2D collidingObj)
// Author: Michael Miljanovic
// Date Last Modified: 6/1/2016
//**************************************************//

using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;


public class rename : MonoBehaviour {

	public int correct;
	public string displaytext = "";
	public string innertext;
	public List<string> names;
	public GameObject sidebar;
	public GameObject code;
	public GameObject codescreen;

	private bool answering = false;
	private bool answered = false;
	private int selection = 0;
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
			if (selection == 0) {
				sidebar.GetComponent<GUIText>().text = displaytext + "   " + names[selection]+" →";
			}
			else if (selection == names.Count-1) {
				sidebar.GetComponent<GUIText>().text = displaytext + "← " + names[selection];
			}
			else{
				sidebar.GetComponent<GUIText>().text = displaytext + "← " + names[selection]+" →";
			}
			if (Input.GetKeyDown(KeyCode.Return)) {
				answered = true;
				answering = false;
				if (selection != correct) {
					lg.GameOver();
				}
				else{
					lg.taskscompleted[2]++;
					GetComponent<AudioSource>().Play();

					innertext = innertext.Substring(23,innertext.Length-37);
					code.GetComponent<TextMesh>().text = code.GetComponent<TextMesh>().text.Replace(stringLib.RENAME_COLOR_TAG + innertext + stringLib.CLOSE_COLOR_TAG, innertext);
					sidebar.GetComponent<GUIText>().text= "";
				}
			}
			else if (Input.GetKeyDown(KeyCode.RightArrow)) {
				selection = (selection + 1 <= names.Count - 1) ? selection + 1 : names.Count - 1;
			}
			else if (Input.GetKeyDown(KeyCode.LeftArrow)) {
				selection = (selection-1>=0) ? selection - 1 : 0;
			}
		}
	}

	//.................................>8.......................................
	void OnTriggerEnter2D(Collider2D collidingObj) {
		if (collidingObj.name == stringLib.PROJECTILE_WARP && !answered) {
			Destroy(collidingObj.gameObject);
			sidebar.GetComponent<GUIText>().text = displaytext;
			GetComponent<AudioSource>().Play();
			answering = true;
		}
	}

	//.................................>8.......................................
}

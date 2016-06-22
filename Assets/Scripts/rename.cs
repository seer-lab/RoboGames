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

	public string correct;
	public string displaytext = "";
	public string innertext;
	public List<string> options;
	public GameObject SidebarObject;
	public GameObject CodeObject;
	public GameObject CodescreenObject;


	private bool answering = false;
	private bool answered = false;
	private int selection = 0;
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
			// Handle left and right arrows --[
			if (selection == 0) {
				SidebarObject.GetComponent<GUIText>().text = displaytext + "   " + options[selection]+" →";
			}
			else if (selection == options.Count-1) {
				SidebarObject.GetComponent<GUIText>().text = displaytext + "← " + options[selection];
			}
			else {
				SidebarObject.GetComponent<GUIText>().text = displaytext + "← " + options[selection]+" →";
			}
			// ]-- End of handling arrows
			// Handle input --[
			if (Input.GetKeyDown(KeyCode.Return)) {
				answered = true;
				answering = false;
				if (selection != options.IndexOf(correct)) {
					lg.GameOver();
				}
				else {
					lg.taskscompleted[2]++;
					GetComponent<AudioSource>().Play();
					innertext = innertext.Substring(23,innertext.Length-37);
					CodeObject.GetComponent<TextMesh>().text = CodeObject.GetComponent<TextMesh>()
															   .text
															   .Replace(stringLib.RENAME_COLOR_TAG +
															   			innertext +
																		stringLib.CLOSE_COLOR_TAG, options[selection]);
					SidebarObject.GetComponent<GUIText>().text= "";
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
	}

	//.................................>8.......................................
	void OnTriggerEnter2D(Collider2D collidingObj) {
		if (collidingObj.name == stringLib.PROJECTILE_WARP && !answered) {
			Destroy(collidingObj.gameObject);
			SidebarObject.GetComponent<GUIText>().text = displaytext;
			GetComponent<AudioSource>().Play();
			answering = true;
		}
	}

	//.................................>8.......................................
}

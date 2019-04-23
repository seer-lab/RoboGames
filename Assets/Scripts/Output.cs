//**************************************************//
// Class Name: Output
// Class Description: This script is a controller for the pop-up interface in both games.
// Methods:
// 		void Start()
//		void Update()
// Author: Michael Miljanovic
// Date Last Modified: 6/1/2016
//**************************************************//

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Output : MonoBehaviour
{
	public GameObject codescreen;
	public GameObject outputtext;

	private Animator anim;
	private LevelGenerator lg;

	//.................................>8.......................................
	// Use this for initialization
	void Start() {
		anim = GetComponent<Animator>();
		anim.SetBool("Appearing", false);
		anim.SetBool("Hiding", false);
		lg = codescreen.GetComponent<LevelGenerator>();
	}

	//.................................>8.......................................
	// Update is called once per frame
	void Update() {
		bool isText = outputtext.GetComponent<Text>().text != "";
		if (isText) {
			anim.SetBool("Appearing", true);
			anim.SetBool("Hiding", false);
		}
		else {
			anim.SetBool("Appearing", false);
			anim.SetBool("Hiding", true);
		}
		if ((Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) || GlobalState.GameState != stateLib.GAMESTATE_IN_GAME) {
			outputtext.GetComponent<Text>().text = "";
		}
	}

	//.................................>8.......................................
}

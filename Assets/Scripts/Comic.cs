//**************************************************//
// Class Name: comic
// Class Description: This is the controller for the comic strips used in both games.
// Methods:
// 		void Start()
//		void Update()
// Author: Michael Miljanovic
// Date Last Modified: 6/1/2016
//**************************************************//

/// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/// /// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/// /// ///////////////////////////////////////////////THIS IS DEPRECATED CODE///////////////////////////////////////////////////////////////
/// /// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/// /// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/// /// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/// 

using UnityEngine;
using System.Collections;
using System.IO;

public class Comic : MonoBehaviour
{
	public int level;
	public GameObject codescreen;
	public GameObject[] panels = new GameObject[4];
	public GameObject credits;
	private LevelGenerator lg;
	public Sprite[] coms = new Sprite[24];
	public Sprite[] oncoms = new Sprite[28];

	private bool playing = false;
	private float delaytime = 0f;
	private float delay = 0.1f;
	private string credtext = "";

	//.................................>8.......................................
	// Use this for initialization
	void Start() {
		lg = codescreen.GetComponent<LevelGenerator>();
		for (int i = 0; i < 20; i++) {
			oncoms[i] = coms[i];
		}
	}

	//.................................>8.......................................
	// Update is called once per frame
	void Update() {
		if (GlobalState.GameState >= stateLib.GAMESTATE_INITIAL_COMIC) {
			if (!playing) {
				playing = true;
				// comic/level generator, there is a list of all images that is associated with each panel.
				// All of it is hardcoded.
				// Remove comics for now and add it back in later.
				if (GlobalState.CurrentONLevel == GlobalState.StringLib.game_level_zero) {
					level = 0;
				}
				else {
					level = int.Parse(GlobalState.CurrentONLevel.Substring(5, 1));
				}
				if (GlobalState.GameMode != stringLib.GAME_MODE_ON) {
					for (int i = 0; i < 4; i++) {
						panels[i].GetComponent<SpriteRenderer>().sprite = coms[level * 4 + i];
						panels[i].GetComponent<Animator>().Play(0);
					}
				}
				else {
					for (int i = 0; i < 4; i++) {
						panels[i].GetComponent<SpriteRenderer>().sprite = oncoms[level * 4 + i];
						panels[i].GetComponent<Animator>().Play(0);
					}
				}
			}
		}
		else {
			playing = false;
		}

		if (GlobalState.GameState == stateLib.GAMESTATE_INITIAL_COMIC) {
			if (Input.anyKeyDown && !Input.GetMouseButton(0) && delaytime < Time.time) {
                GlobalState.GameState = stateLib.GAMESTATE_LEVEL_START;
			}
		}
		else if (GlobalState.GameState == stateLib.GAMESTATE_STAGE_COMIC) {
			if (Input.anyKeyDown && !Input.GetMouseButton(0)) {
                GlobalState.GameState = stateLib.GAMESTATE_LEVEL_START;
			}
		}
		else {
			delaytime = Time.time + delay;
		}
	}

	//.................................>8.......................................

}

/// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/// /// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/// /// ///////////////////////////////////////////////THIS IS DEPRECATED CODE///////////////////////////////////////////////////////////////
/// /// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/// /// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/// /// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/// 
//**************************************************//
// Class Name: comic
// Class Description:
// Methods:
// 		void Start()
//		void Update()
// Author: Michael Miljanovic
// Date Last Modified: 6/1/2016
//**************************************************//

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
		if (lg.gamestate >= stateLib.GAMESTATE_INITIAL_COMIC) {
			if (!playing) {
				playing = true;
				//@TODO: Remove this hardcode
				// comic/level generator, there is a list of all images that is associated with each panel.
				// All of it is hardcoded.
				// Remove comics for now and add it back in later.
				if (lg.currentlevel == stringLib.GAME_LEVEL_ZERO) {
					level = 0;
				}
				else {
					level = int.Parse(lg.currentlevel.Substring(5, 1));
				}
				if (lg.gamemode != stringLib.GAME_MODE_ON) {
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

		if (lg.gamestate == stateLib.GAMESTATE_INITIAL_COMIC) {
			if (Input.anyKeyDown && !Input.GetMouseButton(0) && delaytime < Time.time) {
				lg.gamestate = stateLib.GAMESTATE_LEVEL_START;
			}
		}
		else if (lg.gamestate == stateLib.GAMESTATE_STAGE_COMIC) {
			if (Input.anyKeyDown && !Input.GetMouseButton(0)) {
				lg.gamestate = stateLib.GAMESTATE_LEVEL_START;
			}
		}
		else if (lg.gamestate == stateLib.GAMESTATE_GAME_END) {
			if (credtext == "") {
				//@TODO: Remove hardcode reference here
				FileInfo fi = new FileInfo(lg.gamemode + @"leveldata/credits.txt");
				StreamReader sr = fi.OpenText();
				string text;
				do {
					text = sr.ReadLine();
					credtext += text + "\n";
				} while(text != null);
				credits.GetComponent<TextMesh>().text = credtext;
				credits.GetComponent<Animator>().SetBool("Ended", true);
			}
			if (Input.anyKeyDown && !Input.GetMouseButton(0)) {
				lg.gamestate = 0;
				credtext = "";
				credits.GetComponent<TextMesh>().text = credtext;
				credits.GetComponent<Animator>().SetBool("Ended", false);
			}
		}
		else {
			delaytime = Time.time + delay;
		}
	}

	//.................................>8.......................................

}

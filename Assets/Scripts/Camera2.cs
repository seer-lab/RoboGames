//**************************************************//
// Class Name: Camera2
// Class Description:
// Methods:
// 		void Start()
//		void Update()
// Author: Michael Miljanovic
// Date Last Modified: 6/1/2016
//**************************************************//

using UnityEngine;
using System.Collections;

public class Camera2 : MonoBehaviour
{
	public int track = 0;
	public GameObject hero;
	public GameObject codescreen;
	public AudioClip menutrack;
	public AudioClip victorytrack;
	public AudioClip newleveltrack;
	public AudioClip losetrack;
	public AudioClip endtrack;
	public AudioClip comictrack;
	public AudioClip[] soundtrack = new AudioClip[15];

	private int laststate = stateLib.GAMESTATE_MENU;
	private int trackmax = 12;
	private AudioSource ads;


	//.................................>8.......................................
	// Use this for initialization
	void Start() {
		ads = GetComponent<AudioSource>();
		ads.clip = menutrack;
	}

	//.................................>8.......................................
	// Update is called once per frame
	void Update() {
		int gs = codescreen.GetComponent<LevelGenerator>().gamestate;
		if (laststate != gs) {
			switch(gs) {
				case stateLib.GAMESTATE_INITIAL_COMIC:
					ads.clip = comictrack;
					ads.loop = false;
					ads.Play();
					break;
				case stateLib.GAMESTATE_STAGE_COMIC:
					ads.clip = comictrack;
					ads.loop = false;
					ads.Play();
					break;
				case stateLib.GAMESTATE_GAME_END:
					ads.clip = endtrack;
					ads.loop = false;
					ads.Play();
					break;
				case stateLib.GAMESTATE_LEVEL_LOSE:
					ads.clip = losetrack;
					ads.loop = false;
					ads.Play();
					break;
				case stateLib.GAMESTATE_LEVEL_WIN:
					ads.clip = victorytrack;
					ads.loop = false;
					ads.Play();
					break;
				case stateLib.GAMESTATE_LEVEL_START:
					ads.clip = newleveltrack;
					ads.loop = false;
					ads.Play();
					break;
				case stateLib.GAMESTATE_IN_GAME:
					track = (track + 1) % trackmax;
					ads.clip = soundtrack[track];
					ads.loop = false;
					ads.Play();
					break;
				case stateLib.GAMESTATE_MENU:
					if (laststate > stateLib.GAMESTATE_MENU) {
						ads.clip = menutrack;
						ads.loop = true;
						ads.Play();
					}
					break;
				default:
					break;
			}
			laststate = gs;
		}
		// Game End
		if (gs == stateLib.GAMESTATE_GAME_END) {
			GetComponent<Camera>().transform.position = new Vector3(54.4f, 50f, -10f);
		}
		// Comic
		else if (gs >= stateLib.GAMESTATE_INITIAL_COMIC && gs < stateLib.GAMESTATE_GAME_END) {
			GetComponent<Camera>().transform.position = new Vector3(50, 50f, -10f);
		}
		// Start, win, lose, etc.
		else if (gs >= stateLib.GAMESTATE_LEVEL_START && gs < stateLib.GAMESTATE_INITIAL_COMIC) {
			GetComponent<Camera>().transform.position = new Vector3(100f, 0f, -10f);
		}
		// In-game
		else if (gs == stateLib.GAMESTATE_IN_GAME) {
			GetComponent<Camera>().transform.position = new Vector3(0f, hero.transform.position.y, -10f);
		}
		// Menu
		else if (gs <= stateLib.GAMESTATE_MENU) {
			GetComponent<Camera>().transform.position = new Vector3(50f, 0f, -10f);
		}
		GetComponent<Camera>().orthographicSize = 6;
	}

	//.................................>8.......................................
}

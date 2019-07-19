using UnityEngine;
using System.Collections;

public class Camera2 : MonoBehaviour
{
	public int track = 0;
	GameObject hero;
	public GameObject codescreen;

	private int laststate = stateLib.GAMESTATE_MENU;
	private int trackmax = 12;
	private AudioSource ads;
	bool fullScreen = false; 

	//.................................>8.......................................
	// Use this for initialization
	void Start() {
		hero = GameObject.Find("Hero"); 
		GlobalState.StringLib.LEFT_CODESCREEN_X_COORDINATE = this.GetComponent<Camera>().ViewportToWorldPoint(new Vector3(0,1,0)).x + 2.32f;
		hero.transform.position = new Vector3(GlobalState.StringLib.LEFT_CODESCREEN_X_COORDINATE+ 0.5f, hero.transform.position.y, hero.transform.position.z);

	}

	//.................................>8.......................................
	// Update is called once per frame
	void Update() {
		if (Screen.fullScreen){
			if (!fullScreen){
				fullScreen = true; 
				GlobalState.StringLib.LEFT_CODESCREEN_X_COORDINATE = this.GetComponent<Camera>().ViewportToWorldPoint(new Vector3(0,1,0)).x + 2.32f;
			}
		}
		else {
			if (fullScreen){
				fullScreen = false; 
				GlobalState.StringLib.LEFT_CODESCREEN_X_COORDINATE = this.GetComponent<Camera>().ViewportToWorldPoint(new Vector3(0,1,0)).x + 2.32f;
			}
		}

		
        int gs = GlobalState.GameState; 

		// Game End
		if (gs == stateLib.GAMESTATE_GAME_END) {
			GetComponent<Camera>().transform.position = new Vector3(54.4f, 50f, -10f);
		}

		// Start, win, lose, etc.
		else if (gs >= stateLib.GAMESTATE_LEVEL_START && gs < stateLib.GAMESTATE_INITIAL_COMIC) {
			GetComponent<Camera>().transform.position = new Vector3(100f, 0f, -10f);
		}
		// In-game
		else if (gs == stateLib.GAMESTATE_IN_GAME) {
			GetComponent<Camera>().transform.position = new Vector3(0f, hero.transform.position.y, -10f);
		}

		GetComponent<Camera>().orthographicSize = 6;
	}

	//.................................>8.......................................
}

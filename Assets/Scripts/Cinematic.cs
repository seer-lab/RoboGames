using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Cinematic : MonoBehaviour
{

		public GameObject codescreen;
		LevelGenerator lg;
		public string introtext = "Level Start Placeholder!";
		public string endtext = "Winner!\nLevel End Placeholder!";
		public string losetext;
		public GameObject prompt2;
		public GameObject menu;
		float delaytime = 0f;
		float delay = 0.1f;
		bool cinerun = false;
		List<GameObject> objs;
		public GameObject[] cinebugs = new GameObject[6];

		// Use this for initialization
		void Start ()
		{
				lg = codescreen.GetComponent<LevelGenerator> ();
				objs = new List<GameObject> ();
				losetext = "You've failed!\n";
		}
	
		// Update is called once per frame
		void Update ()
		{
				if (lg.gamestate == 2) {
						if (!cinerun) {
								cinerun = true;
				if(lg.gamemode != "on"){
								GameObject bug = (GameObject)Instantiate (cinebugs [2]);
								objs.Add (bug);
				}else{GameObject rob = (GameObject)Instantiate(cinebugs[3]); 
					objs.Add(rob);
				}
			}
						GetComponent<TextMesh> ().text = introtext;
						if (Input.GetKeyDown (KeyCode.Return) && delaytime < Time.time) {
								lg.gamestate = 1;
								Destroy (objs [0]);
								cinerun = false;
								objs = new List<GameObject> ();
								lg.GUISwitch (true);
						}
				} else if (lg.gamestate == 3) {
						if (!cinerun) {
								cinerun = true;
								GameObject bug = (GameObject)Instantiate (cinebugs [3]);
								objs.Add (bug);
								bug = (GameObject)Instantiate (cinebugs [0]);
								objs.Add (bug);
						}
						GetComponent<TextMesh> ().text = endtext;
						if (Input.GetKeyDown (KeyCode.Return) && delaytime < Time.time) {
								if (lg.currentlevel.StartsWith ("tut") && lg.gamemode == "bug") {
										lg.gamestate = 11;
								} else {
										lg.gamestate = 2;
								}
								lg.BuildLevel (lg.nextlevel, false);
								Destroy (objs [1]);
								Destroy (objs [0]);
								cinerun = false;
								objs = new List<GameObject> ();
								
						}
				} else if (lg.gamestate == 4) {
						if (!cinerun) {
								cinerun = true;
								GameObject bug = (GameObject)Instantiate (cinebugs [4]);
								objs.Add (bug);
						}
						GetComponent<TextMesh> ().text = losetext;
						prompt2.GetComponent<TextMesh> ().text = "Press Enter to try again\nor ESC to quit";
						if (Input.GetKeyDown (KeyCode.Escape) && delaytime < Time.time) {
								Destroy (objs [0]);
								prompt2.GetComponent<TextMesh> ().text = "Press Enter to continue.";

								cinerun = false;
								objs = new List<GameObject> ();
								lg.gamestate = 0;
						}
						if (Input.GetKeyDown (KeyCode.Return) && delaytime < Time.time) {
								Destroy (objs [0]);
								prompt2.GetComponent<TextMesh> ().text = "Press Enter to continue.";

								cinerun = false;
								objs = new List<GameObject> ();
								menu.GetComponent<Menu> ().gameon = true;
								lg.BuildLevel (lg.gamemode + @"leveldata\" + lg.currentlevel, false);
								lg.gamestate = 2;
						}
				} else {
						delaytime = Time.time + delay;
				}
		}
}

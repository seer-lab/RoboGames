using UnityEngine;
using System.Collections;
using System.IO;

public class Comic : MonoBehaviour
{

	public GameObject codescreen;
	LevelGenerator lg;
	//public RuntimeAnimatorController[] coms = new RuntimeAnimatorController[24];
	public Sprite[] coms = new Sprite[24];
	public Sprite[] oncoms = new Sprite[28];
	public GameObject[] panels = new GameObject[4];
	public GameObject credits;
	bool playing = false;
	string credtext = "";
	float delaytime = 0f;
	float delay = 0.1f;
	public int level;

	// Use this for initialization
	void Start ()
	{
		lg = codescreen.GetComponent<LevelGenerator> ();
		for (int i =0; i<20; i++) {
			oncoms[i]=coms[i];
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (lg.gamestate > 9) {
			if (!playing) {
				playing = true;
				if (lg.currentlevel == "tut1.xml") {
					level = 0;
				} else {
					level = int.Parse (lg.currentlevel.Substring (5, 1));
				}
				if (lg.gamemode != "on"){
				for (int i = 0; i<4; i++) {
					panels [i].GetComponent<SpriteRenderer> ().sprite = coms [level * 4 + i];
					panels [i].GetComponent<Animator> ().Play (0);
					}}else{for (int i = 0; i<4; i++) {
						panels [i].GetComponent<SpriteRenderer> ().sprite = oncoms [level * 4 + i];
						panels [i].GetComponent<Animator> ().Play (0);
					}}
			}

		} else {
			playing = false;
		}
		if (lg.gamestate == 10) {
			if (Input.anyKeyDown && !Input.GetMouseButton (0) && delaytime < Time.time) {
				lg.gamestate = 2;
			}
		} else if (lg.gamestate == 11) {
			if (Input.anyKeyDown && !Input.GetMouseButton (0)) {
				lg.gamestate = 2;
			}
		} else if (lg.gamestate == 12) {
			if (credtext == "") {
				FileInfo fi = new FileInfo (lg.gamemode + @"leveldata/credits.txt");
				StreamReader sr = fi.OpenText ();
				string text;
				do {
					text = sr.ReadLine ();
					credtext += text + "\n";
				} while(text != null);
				credits.GetComponent<TextMesh> ().text = credtext;
				credits.GetComponent<Animator> ().SetBool ("Ended", true);
			}
			if (Input.anyKeyDown && !Input.GetMouseButton (0)) {
				lg.gamestate = 0;
				credtext = "";
				credits.GetComponent<TextMesh> ().text = credtext;
				credits.GetComponent<Animator> ().SetBool ("Ended", false);
			}
		} else {delaytime = Time.time + delay;
		}

	}
}

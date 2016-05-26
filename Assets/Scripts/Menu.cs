using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class Menu : MonoBehaviour
{

	public GameObject codescreen;
	public GameObject cinematic;
	public GameObject[] buttons = new GameObject[5];
	public GameObject[] buttontext = new GameObject[5];
	public GameObject[] m2buttons = new GameObject[2];
	public GameObject[] m2buttontext = new GameObject[2];
	public GameObject[] m2arrows = new GameObject[2];
	public GameObject menu2;
	public Sprite bluebutton;
	public Sprite greenbutton;
	public bool gameon = false;
	int option = 0;
	int levoption = 0;
	LevelGenerator lg;
	float delaytime = 0f;
	float delay = 0.1f;
	string lfile;
	public List<string> levels;
	public List<string> passed;
	bool soundon = true;
	StreamReader sr;

	// Use this for initialization
	void Start ()
	{
		buttontext [0].GetComponent<TextMesh> ().text = "New Game";
		buttontext [1].GetComponent<TextMesh> ().text = "Load Game";
		buttontext [2].GetComponent<TextMesh> ().text = "Sound Options";
		buttontext [3].GetComponent<TextMesh> ().text = "Exit Game";
		buttontext [4].GetComponent<TextMesh> ().text = "Resume Game";
		buttons [4].GetComponent<SpriteRenderer> ().color = Color.grey;
		lg = codescreen.GetComponent<LevelGenerator> ();
		m2switch (false);

	}
	// Update is called once per frame
	void Update ()
	{
		if (!gameon) {
			buttons [4].GetComponent<SpriteRenderer> ().color = Color.grey;
		} else {
			buttons [4].GetComponent<SpriteRenderer> ().color = Color.white;
		}
		if (lg.gamestate == 0) {
			if (Input.GetKeyDown (KeyCode.UpArrow)) {
				buttons [option].GetComponent<SpriteRenderer> ().sprite = bluebutton;
				option = 0 == option ? 0 : option - 1;
			}
			if (Input.GetKeyDown (KeyCode.DownArrow)) {
				buttons [option].GetComponent<SpriteRenderer> ().sprite = bluebutton;
				if (gameon) {
					option = 4 == option ? 4 : option + 1;
				} else {
					option = 3 == option ? 3 : option + 1;
				}
			}
			buttons [option].GetComponent<SpriteRenderer> ().sprite = greenbutton;
			if (Input.GetKeyDown (KeyCode.Return) && delaytime < Time.time) {
				switch (option) {
				case 0:
					lg.gamestate = -3;
					buttons [option].GetComponent<SpriteRenderer> ().sprite = bluebutton;
					option = 0;
					m2switch (true);
					m2buttontext [0].GetComponent<TextMesh> ().text = "Robot ON!";
					m2buttontext [1].GetComponent<TextMesh> ().text = "RoboBUG";
					break;
				case 1:

					lg.gamestate = -4;

					buttons [option].GetComponent<SpriteRenderer> ().sprite = bluebutton;
					option = 0;
					m2switch (true);
					m2buttontext [0].GetComponent<TextMesh> ().text = "Robot ON!";
					m2buttontext [1].GetComponent<TextMesh> ().text = "RoboBUG";
					break;
				case 2:
					lg.gamestate = -2;
					buttons [option].GetComponent<SpriteRenderer> ().sprite = bluebutton;
					option = 0;
					m2switch (true);
					m2buttontext [0].GetComponent<TextMesh> ().text = "Sound: " + (soundon ? "<color=#000000ff>ON</color>" : "<color=#ff0000ff>OFF</color>");
					m2buttontext [1].GetComponent<TextMesh> ().text = "Back";
					break;
				case 3:
					Application.Quit ();
					break;
				case 4:
					lg.gamestate = 1;
					lg.GUISwitch (true);
					break;
				}
			}
		} else if (lg.gamestate == -1) {
			if (levoption < levels.Count - 1 && passed [levoption] == "1") {
				m2arrows [1].GetComponent<SpriteRenderer> ().enabled = true;
			} else {
				m2arrows [1].GetComponent<SpriteRenderer> ().enabled = false;
			}
			if (levoption != 0) {
				m2arrows [0].GetComponent<SpriteRenderer> ().enabled = true;
			} else {
				m2arrows [0].GetComponent<SpriteRenderer> ().enabled = false;
			}
			m2buttons [option].GetComponent<SpriteRenderer> ().sprite = greenbutton;
			if (Input.GetKeyDown (KeyCode.UpArrow)) {
				m2buttons [1].GetComponent<SpriteRenderer> ().sprite = bluebutton;
				option = 0;
			}
			if (Input.GetKeyDown (KeyCode.DownArrow)) {
				m2buttons [0].GetComponent<SpriteRenderer> ().sprite = bluebutton;
				option = 1;
			}
			if (Input.GetKeyDown (KeyCode.RightArrow)) {
				if (levoption < levels.Count - 1 && passed [levoption] == "1") {
					levoption++;
				}
				m2buttontext [0].GetComponent<TextMesh> ().text = levels [levoption];
			}
			if (Input.GetKeyDown (KeyCode.LeftArrow)) {
				levoption = 0 == levoption ? 0 : levoption - 1;
				m2buttontext [0].GetComponent<TextMesh> ().text = levels [levoption];
			}
			if (Input.GetKeyDown (KeyCode.Return)) {
				switch (option) {
				case 0:
					lg.BuildLevel (lg.gamemode + @"leveldata\" + levels [levoption], false);
					lg.gamestate = 2;
					levoption = 0;
					gameon = true;
					buttons [4].GetComponent<SpriteRenderer> ().color = Color.white;
					m2switch (false);
					break;
				case 1:
					lg.gamestate = 0;
					m2buttons [1].GetComponent<SpriteRenderer> ().sprite = bluebutton;

					m2switch (false);
					break;
				}
			}
						
		} else if (lg.gamestate == -2) {
			m2buttons [option].GetComponent<SpriteRenderer> ().sprite = greenbutton;
			if (Input.GetKeyDown (KeyCode.UpArrow)) {
				m2buttons [1].GetComponent<SpriteRenderer> ().sprite = bluebutton;
				option = 0;
			}
			if (Input.GetKeyDown (KeyCode.DownArrow)) {
				m2buttons [0].GetComponent<SpriteRenderer> ().sprite = bluebutton;
				option = 1;
			}
			if (Input.GetKeyDown (KeyCode.Return)) {
				switch (option) {
				case 0:
					soundon = !soundon;
					m2buttontext [0].GetComponent<TextMesh> ().text = "Sound: " + (soundon ? "<color=#000000ff>ON</color>" : "<color=#ff0000ff>OFF</color>");
					AudioListener.volume = soundon ? 1 : 0;
					break;
				case 1:
					lg.gamestate = 0;
					m2buttons [1].GetComponent<SpriteRenderer> ().sprite = bluebutton;
					m2switch (false);
					option = 2;
					break;
				}
								
			}
						
		} else if (lg.gamestate == -3) {
			m2buttons [option].GetComponent<SpriteRenderer> ().sprite = greenbutton;
			if (Input.GetKeyDown (KeyCode.UpArrow)) {
				m2buttons [1].GetComponent<SpriteRenderer> ().sprite = bluebutton;
				option = 0;
			}
			if (Input.GetKeyDown (KeyCode.DownArrow)) {
				m2buttons [0].GetComponent<SpriteRenderer> ().sprite = bluebutton;
				option = 1;
			}
			if (Input.GetKeyDown (KeyCode.Return)) {
				switch (option) {
				case 0:
					lg.gamemode = "on";

					lg.BuildLevel (@"onleveldata\demo.xml", false);
					lg.gamestate = 2;
					break;
				case 1:
					lg.gamemode = "bug";

					lg.BuildLevel (@"bugleveldata\tut1.xml", false);
					lg.gamestate = 10;
					break;
				}
				m2switch (false);
				gameon = true;
				buttons [4].GetComponent<SpriteRenderer> ().color = Color.white;
				
			}
		} else if (lg.gamestate == -4) {

			m2buttons [option].GetComponent<SpriteRenderer> ().sprite = greenbutton;
			if (Input.GetKeyDown (KeyCode.UpArrow)) {
				m2buttons [1].GetComponent<SpriteRenderer> ().sprite = bluebutton;
				option = 0;
			}
			if (Input.GetKeyDown (KeyCode.DownArrow)) {
				m2buttons [0].GetComponent<SpriteRenderer> ().sprite = bluebutton;
				option = 1;
			}
			if (Input.GetKeyDown (KeyCode.Return)) {
				switch (option) {
				case 0:
					lg.gamemode = "on";
					break;
				case 1:
					lg.gamemode = "bug";
					break;
				}

				levels.Clear ();
				passed.Clear ();
				lfile = lg.gamemode + @"leveldata\levels.txt";
				sr = File.OpenText (lfile);
				string line;
				while ((line = sr.ReadLine()) != null) {
					string[] data = line.Split (' ');
					levels.Add (data [0]);
					passed.Add (data [1]);
				}
				sr.Close ();
			
				lg.gamestate = -1;
				option = 0;
				m2buttons [1].GetComponent<SpriteRenderer> ().sprite = bluebutton;
				m2buttontext [0].GetComponent<TextMesh> ().text = levels [levoption];
				m2buttontext [1].GetComponent<TextMesh> ().text = "Back";
			}
		} else {
			delaytime = Time.time + delay;
		}
	}

	public void saveGame (string currentlevel)
	{
		levels.Clear ();
		passed.Clear ();
		lfile = lg.gamemode + @"leveldata\levels.txt";
		sr = File.OpenText (lfile);
		string line;
		while ((line = sr.ReadLine()) != null) {
			string[] data = line.Split (' ');
			levels.Add (data [0]);
			passed.Add (data [1]);
		}
		sr.Close ();

		passed [levels.IndexOf (currentlevel)] = "1";
		StreamWriter sw = File.CreateText (lg.gamemode + @"leveldata\levels.txt");
		for (int i = 0; i<levels.Count; i++) {
			sw.WriteLine (levels [i] + " " + passed [i]);
		}
		sw.Close ();
	}

	void m2switch (bool on)
	{
		if (on) {
			menu2.GetComponent<SpriteRenderer> ().enabled = true;
			foreach (GameObject button in m2buttons) {
				button.GetComponent<SpriteRenderer> ().enabled = true;
			}
		} else {
			menu2.GetComponent<SpriteRenderer> ().enabled = false;
			foreach (GameObject button in m2buttons) {
				button.GetComponent<SpriteRenderer> ().enabled = false;
			}
			foreach (GameObject btext in m2buttontext) {
				btext.GetComponent<TextMesh> ().text = "";
			}
			foreach (GameObject arrow in m2arrows) {
				arrow.GetComponent<SpriteRenderer> ().enabled = false;
			}
		}
	}
}

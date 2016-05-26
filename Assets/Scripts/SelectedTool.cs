using UnityEngine;
using System.Collections;

public class SelectedTool : MonoBehaviour
{
	GUIText tm;
	public int projectilecode = 0;
	Color toolOn = new Color (.7f, .7f, .7f);
	Color toolOff = new Color (.3f, .3f, .3f);
	float lossDelay = 4f;
	public GameObject[] toolIcons = new GameObject[6];
	public int[] toolCounts = new int[6];
	public int[] bonusTools = {0,0,0,0,0,0};
	public GameObject codescreen;
	public GameObject hero;
	public GameObject toolprompt;
	public bool losing = false;
	public bool failed = false;
	float losstime;
	public bool toolget = false;
	public GameObject toolLabel;
	LevelGenerator lg;

	// Use this for initialization
	void Start ()
	{
		tm = this.GetComponent<GUIText> ();
		tm.text = "Bugcatcher";
		lg = codescreen.GetComponent<LevelGenerator> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (lg.gamestate >= 2) {
			losing = false;
			failed = false;
		}
		if (lg.gamestate == 1) {
			toolLabel.GetComponent<GUIText> ().text = "Available Tools:";

			if (toolget) {
				Animator anim = toolprompt.GetComponent<Animator> ();
				anim.Play ("hide");
				toolget = false;
			}
			if (losing || failed) {
				if (Time.time > losstime) {
					failed = false;
					losing = false;
					lg.losing = true;
				}
			}
			for (int i = 0; i<6; i++) {
				if (toolCounts [i] + bonusTools [i] > 0) {
				

					toolIcons [i].GetComponent<GUITexture> ().enabled = true;
					
					losing = false;
					if (projectilecode == -1) {
						projectilecode = i;
					}
				}
			}
			//if (projectilecode >= 0) {
			hero.GetComponent<hero2Controller> ().projectilecode = projectilecode;
			if (Input.GetKeyDown ("tab") && projectilecode >= 0) {
				NextTool ();
			}
			if (hero.GetComponent<hero2Controller> ().throwing) {
				hero.GetComponent<hero2Controller> ().throwing = false;
				if (toolCounts [projectilecode] < 999) {
					if (toolCounts [projectilecode] == 0) {
						bonusTools [projectilecode] -= 1;
					} else {
						toolCounts [projectilecode] -= 1;
					}
				}
				if (projectilecode == 0 && toolCounts [0] == 0 && lg.gamemode != "on") {
					failed = true;
					losstime = Time.time + lossDelay;
				}
				if (toolCounts [projectilecode] == 0 && bonusTools [projectilecode] == 0) {
					toolIcons [projectilecode].GetComponent<GUITexture> ().enabled = false;
					NextTool ();
				}
			}
			//	}
			switch (projectilecode) {
			case 0:
				tm.color = Color.white;
				if (lg.gamemode == "bug") {
					tm.text = "Bugcatcher: " + toolCounts [0].ToString ();
				} else {
					tm.text = "Activator: " + toolCounts [0].ToString ();
				}
				toolIcons [0].GetComponent<GUITexture> ().color = toolOn;
				break;
			case 1:
				tm.color = Color.white;
				if (lg.gamemode == "bug") {
					tm.text = "Printer: " + toolCounts [1].ToString ();
				} else {
					tm.text = "Checker: " + toolCounts [1].ToString ();
				}
				toolIcons [1].GetComponent<GUITexture> ().color = toolOn;
				break;
			case 2:
				tm.color = Color.white;
				if (lg.gamemode == "bug") {
					tm.text = "Warper: " + toolCounts [2].ToString ();
				} else {
					tm.text = "Namer: " + toolCounts [2].ToString ();
				}
				toolIcons [2].GetComponent<GUITexture> ().color = toolOn;
				break;
			case 3:
				tm.color = Color.white;
				tm.text = "Commenter: " + toolCounts [3].ToString (); 
				toolIcons [3].GetComponent<GUITexture> ().color = toolOn;
				break;
			case 4:
				tm.color = Color.white;
				if (lg.gamemode == "bug") {
					tm.text = "Breakpointer: " + toolCounts [4].ToString ();
				} else {
					tm.text = "Un-Commenter: " + toolCounts [4].ToString ();
				}
				toolIcons [4].GetComponent<GUITexture> ().color = toolOn;
				break;
			case 5:
				tm.color = Color.white;
				tm.text = "Helper: " + toolCounts [5].ToString (); 
				toolIcons [5].GetComponent<GUITexture> ().color = toolOn;
				break;
			case -1:
				tm.color = Color.red;
				tm.text = "Out of Tools!!";
				break;
			}
			if (projectilecode >= 0 && bonusTools [projectilecode] > 0) {
				tm.text += " <color=#ff8800ff>+ " + bonusTools [projectilecode].ToString () + "</color>";
			}
		} else {
			for (int i = 0; i<6; i++) {
				toolIcons [i].GetComponent<GUITexture> ().enabled = false;
			}
			tm.text = "";
			losing = false;
			toolLabel.GetComponent<GUIText> ().text = "";
		}
	}

	public void NextTool ()
	{
		int notoolcount = 0;
		toolIcons [projectilecode].GetComponent<GUITexture> ().color = toolOff;
		projectilecode = (projectilecode + 1) % 6;
		while (!toolIcons[projectilecode].GetComponent<GUITexture>().enabled) {
			notoolcount++;
			projectilecode = (projectilecode + 1) % 6;
			if (notoolcount > 7) {
				projectilecode = -1;
				break;
			}
		}
		if (projectilecode == -1) {
			losing = true;
			losstime = Time.time + lossDelay;
			//codescreen.GetComponent<LevelGenerator> ().endTime = Time.time + lossDelay;
		}
	}
	
}

using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;


public class rename : MonoBehaviour {
	
	public string displaytext = "";
	public GameObject sidebar;
	public GameObject code;
	public string innertext;
	public GameObject codescreen;
	bool answering;
	bool answered;
	//public List<string> names;
	public List<string> names;
	public int correct;
	int selection;
	LevelGenerator lg;
	//public GameObject selectTools;
	//public int[] tools = {0,0,0,0,0,0};
	//bool toolgiven = false;
	
	//float initialLineY = 3.5f;
	//float linespacing = 0.825f;
	
	// Use this for initialization
	void Start () {
		answering = false;
		answered = false;
		selection = 0;
		lg = codescreen.GetComponent<LevelGenerator> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (answering) {
			if (selection == 0){
				sidebar.GetComponent<GUIText>().text = displaytext + "   " + names[selection]+" →";
			}else if (selection == names.Count-1){
				sidebar.GetComponent<GUIText>().text = displaytext + "← " + names[selection];
			} else{
				sidebar.GetComponent<GUIText>().text = displaytext + "← " + names[selection]+" →";
			}
			if (Input.GetKeyDown (KeyCode.Return)){
				answered = true;
				answering = false;
				if (selection != correct){
					lg.GameOver();
				}else{
					lg.taskscompleted[2]++;
					GetComponent<AudioSource>().Play();

					innertext = innertext.Substring(23,innertext.Length-37);
					code.GetComponent<TextMesh> ().text = code.GetComponent<TextMesh> ().text.Replace ("<color=#ff00ffff>"+innertext+"</color>", innertext);
					sidebar.GetComponent<GUIText>().text="";
				}
			}
			else if (Input.GetKeyDown(KeyCode.RightArrow)){
				selection = selection+1<=names.Count-1?selection+1:names.Count-1;
			}
			else if (Input.GetKeyDown(KeyCode.LeftArrow)){
				selection = selection-1>=0?selection-1:0;
			}
		}
	}
	void OnTriggerEnter2D(Collider2D c){
		if (c.name == "projectileWarp(Clone)" && !answered){
			//StreamWriter sw = new StreamWriter("toollog.txt",true);
			//sw.WriteLine("Printed,"+((int)((initialLineY-this.transform.position.y)/linespacing)).ToString()+","+Time.time.ToString());
			//sw.Close();
			Destroy(c.gameObject);
			sidebar.GetComponent<GUIText>().text = displaytext;
			GetComponent<AudioSource>().Play();
			answering = true;
		}
	}
}

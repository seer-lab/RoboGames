using UnityEngine;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;


public class checker : MonoBehaviour {
	
	public string displaytext = "";
	public GameObject sidebar;
	public GameObject code;
	public string innertext;
	public GameObject codescreen;
	bool answering;
	bool answered;
	public string expected;
	LevelGenerator lg;
	string input;
	//public GameObject selectTools;
	//public int[] tools = {0,0,0,0,0,0};
	//bool toolgiven = false;
	
	//float initialLineY = 3.5f;
	//float linespacing = 0.825f;
	
	// Use this for initialization
	void Start () {
		answering = false;
		answered = false;
		input = "";
		lg = codescreen.GetComponent<LevelGenerator> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (answering) {
			if (Input.GetKeyDown (KeyCode.Return)){
				answered = true;
				answering = false;
				if (expected != input){
					lg.losing = true;
				}else{
					lg.taskscompleted[1]++;
					GetComponent<AudioSource>().Play();

					innertext = innertext.Substring(23,innertext.Length-37);
					code.GetComponent<TextMesh> ().text = code.GetComponent<TextMesh> ().text.Replace ("<color=#ffff00ff>"+innertext+"</color>", innertext);
				}
			}
			else if (Input.GetKeyDown(KeyCode.Backspace)){
				input = input.Substring(0,input.Length-1);
				sidebar.GetComponent<GUIText>().text = displaytext + input;
			}
			else{
				string thisstr = Input.inputString;
			//	Regex rgx = new Regex ("[A-Za-z0-9]");
				//if (rgx.Equals(thisstr)){
					input += thisstr;
					sidebar.GetComponent<GUIText>().text = displaytext + input;
				//}
			}
		}
	}
	void OnTriggerEnter2D(Collider2D c){
		if (c.name == "projectileActivator(Clone)" && !answered){
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

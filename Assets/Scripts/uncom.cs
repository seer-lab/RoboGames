using UnityEngine;
using System.Collections;
using System.IO;

public class uncom : MonoBehaviour {
	
	public string oldtext="";
	public string blocktext="";
	public GameObject code;
	public GameObject codescreen;
	public bool commented;
	
	//float initialLineY = 3.5f;
	//float linespacing = 0.825f;
	
	LevelGenerator lg;

	// Use this for initialization
	void Start () {
		lg = codescreen.GetComponent<LevelGenerator> ();
	}
	
	// Update is called once per frame
	void Update () {
	}
	
	void OnTriggerEnter2D(Collider2D c){
		if (c.name == "projectileDebug(Clone)") {
			if (commented) {
				lg.GameOver();
			} else {
				Destroy (c.gameObject);
				GetComponent<AudioSource> ().Play ();
				lg.taskscompleted[4]++;
				blocktext = blocktext.Substring(19,blocktext.Length-29);
				code.GetComponent<TextMesh> ().text = code.GetComponent<TextMesh> ().text.Replace ("<color=#ff0000ff>/*" + blocktext+ "*/</color>",blocktext);
				commented = true;
				
			}
		}
	}
}

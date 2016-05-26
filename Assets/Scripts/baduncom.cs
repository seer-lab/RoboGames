using UnityEngine;
using System.Collections;
using System.IO;

public class baduncom : MonoBehaviour {
	
	public string oldtext="";
	public string blocktext="";
	public string righttext="";
	public GameObject code;
	public GameObject rightcomment;
	public GameObject codescreen;
	bool done;
	//float initialLineY = 3.5f;
	//float linespacing = 0.825f;
	
	LevelGenerator lg;
	
	// Use this for initialization
	void Start () {
		done = false;
		lg = codescreen.GetComponent<LevelGenerator> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (rightcomment) {
			if (rightcomment.GetComponent<uncom> ().commented && !done) {
				done = true;
				blocktext = blocktext.Substring(19,blocktext.Length-29);
				code.GetComponent<TextMesh> ().text = code.GetComponent<TextMesh> ().text.Replace ("<color=#ff0000ff>/*" + blocktext+ "*/</color>","<color=#00000000>/*" + blocktext+ "*/</color>");
			}
		}
	}
	
	void OnTriggerEnter2D(Collider2D c){
		if (c.name == "projectileDebug(Clone)") {
			
			Destroy (c.gameObject);
			GetComponent<AudioSource> ().Play ();
			lg.losing = true;
		}
	}
}

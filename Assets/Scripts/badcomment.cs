using UnityEngine;
using System.Collections;
using System.IO;

public class badcomment : MonoBehaviour {
	
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
			if (rightcomment.GetComponent<oncomment> ().commented && !done) {
				done = true;
				code.GetComponent<TextMesh> ().text = code.GetComponent<TextMesh> ().text.Replace (blocktext, "<color=#00000000>" + blocktext + "</color>");
			}
		}
	}
	
	void OnTriggerEnter2D(Collider2D c){
		if (c.name == "projectileComment(Clone)") {

				Destroy (c.gameObject);
				GetComponent<AudioSource> ().Play ();
				lg.losing = true;
			}
		}
}

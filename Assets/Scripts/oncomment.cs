using UnityEngine;
using System.Collections;
using System.IO;

public class oncomment : MonoBehaviour {
	
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
		if (c.name == "projectileComment(Clone)" && !commented) {

				Destroy (c.gameObject);
				GetComponent<AudioSource> ().Play ();
				lg.taskscompleted[3]++;
				code.GetComponent<TextMesh> ().text = code.GetComponent<TextMesh> ().text.Replace (blocktext, "<color=#00ff00ff>/*" + blocktext+ "*/</color>");
				commented = true;


		}
	}
}

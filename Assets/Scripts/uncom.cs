//**************************************************//
// Class Name: uncom
// Class Description:
// Methods:
// 		void Start()
//		void Update()
//		void OnTriggerEnter2D(Collider2D c)
// Author: Michael Miljanovic
// Date Last Modified: 6/1/2016
//**************************************************//

using UnityEngine;
using System.Collections;
using System.IO;

public class uncom : MonoBehaviour {

	public bool commented;
	public string oldtext= "";
	public string blocktext= "";
	public GameObject code;
	public GameObject codescreen;

	private LevelGenerator lg;

	//.................................>8.......................................
	// Use this for initialization
	void Start() {
		lg = codescreen.GetComponent<LevelGenerator>();
	}

	//.................................>8.......................................
	// Update is called once per frame
	void Update() {
	}

	//.................................>8.......................................
	void OnTriggerEnter2D(Collider2D c) {
		if (c.name == stringLib.PROJECTILE_DEBUG) {
			if (commented) {
				lg.GameOver();
			}
			else {
				Destroy(c.gameObject);
				GetComponent<AudioSource>().Play();
				lg.taskscompleted[4]++;
				blocktext = blocktext.Substring(19,blocktext.Length-29);
				code.GetComponent<TextMesh>().text = code.GetComponent<TextMesh>()
														 .text
														 .Replace(stringLib.UNCOMMENT_COLOR_TAG + blocktext+ stringLib.COMMENT_CLOSE_COLOR_TAG, blocktext);
				commented = true;
			}
		}
	}

	//.................................>8.......................................
}

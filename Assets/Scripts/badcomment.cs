//**************************************************//
// Class Name: badcomment
// Class Description: Handles "bad" comment blocks; blocks which should be avoided by the player.
// Methods:
// 		void Start()
//		void Update()
//		void OnTriggerEnter2D(Collider2D collidingObj)
// Author: Michael Miljanovic
// Date Last Modified: 6/1/2016
//**************************************************//

using UnityEngine;
using System.Collections;
using System.IO;

public class badcomment : MonoBehaviour {

	public string oldtext	= "";
	public string blocktext	= "";
	public string righttext	= "";
	public GameObject code;
	public GameObject rightcomment;
	public GameObject codescreen;

	private bool done = false;
	private LevelGenerator lg;

	//.................................>8.......................................
	// Initializes this object
	void Start() {
		lg = codescreen.GetComponent<LevelGenerator>();
	}

	//.................................>8.......................................
	// Update is called once per frame
	void Update() {
		// GameObject must exist
		if (rightcomment) {
			// Commented and badcomment is not done?
			if (rightcomment.GetComponent<oncomment>().commented && !done) {
				// Colorize the TextMesh's text with this blocktext
				done = true;
				code.GetComponent<TextMesh>().text = code.GetComponent<TextMesh>()
														 .text
														 .Replace(blocktext, stringLib.BAD_COMMENT_TEXT_COLOR_TAG +
														 					 blocktext +
														 				     stringLib.CLOSE_COLOR_TAG);
			}
		}
	}

	//.................................>8.......................................
	void OnTriggerEnter2D(Collider2D collidingObj) {
		if (collidingObj.name == stringLib.PROJECTILE_COMMENT) {
			Destroy(collidingObj.gameObject);
			GetComponent<AudioSource>().Play();
			lg.isLosing = true;
		}
	}

	//.................................>8.......................................
}

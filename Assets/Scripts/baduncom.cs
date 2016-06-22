//**************************************************//
// Class Name: baduncom
// Class Description: Handles "bad" uncomment blocks; blocks which should not be uncommented.
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

public class baduncom : MonoBehaviour {

	public string oldtext	= "";
	public string blocktext	= "";
	public string righttext	= "";
	public GameObject CodeObject;
	public GameObject CorrectCommentObject;
	public GameObject CodescreenObject;

	private bool doneUpdating = false;
	private LevelGenerator lg;

	//.................................>8.......................................
	void Start() {
		lg = CodescreenObject.GetComponent<LevelGenerator>();
	}

	//.................................>8.......................................
	// Update is called once per frame
	void Update() {
		// GameObject must exist
		if (CorrectCommentObject) {
			// Commented and badcomment is not done?
			if (CorrectCommentObject.GetComponent<uncom>().isCommented && !doneUpdating) {
				doneUpdating = true;
				// Find this object's text in the code and remove it.
				CodeObject.GetComponent<TextMesh>().text = CodeObject.GetComponent<TextMesh>()
														   .text
														   .Replace(blocktext, "");
			}
		}
	}

	//.................................>8.......................................
	void OnTriggerEnter2D(Collider2D collidingObj) {
		if (collidingObj.name == stringLib.PROJECTILE_DEBUG && !doneUpdating) {
			Destroy(collidingObj.gameObject);
			GetComponent<AudioSource>().Play();
			lg.isLosing = true;
		}
	}

	//.................................>8.......................................
}

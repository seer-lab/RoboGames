//**************************************************//
// Class Name: oncomment
// Class Description:
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

public class oncomment : MonoBehaviour {

	public bool commented;
	public string oldtext = "";
	public string blocktext = "";
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
	void OnTriggerEnter2D(Collider2D collidingObj) {
		if (collidingObj.name == stringLib.PROJECTILE_COMMENT && !commented) {
			Destroy(collidingObj.gameObject);
			GetComponent<AudioSource>().Play();
			lg.taskscompleted[3]++;
			code.GetComponent<TextMesh>().text = code.GetComponent<TextMesh>()
													 .text
													 .Replace(blocktext, stringLib.COMMENT_BLOCK_COLOR_TAG +
													 					 blocktext +
																		 stringLib.COMMENT_CLOSE_COLOR_TAG);
			commented = true;
		}
	}

	//.................................>8.......................................
}

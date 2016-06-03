//**************************************************//
// Class Name: baduncom
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

public class baduncom : MonoBehaviour {

	public string oldtext	= "";
	public string blocktext	= "";
	public string righttext	= "";
	public GameObject code;
	public GameObject rightcomment;
	public GameObject codescreen;

	private bool done = false;
	private LevelGenerator lg;


	//.................................>8.......................................
	void Start() {
		lg = codescreen.GetComponent<LevelGenerator>();
	}

	//.................................>8.......................................
	// Update is called once per frame
	void Update() {
		// GameObject must exist
		if (rightcomment) {
			// Commented and badcomment is not done?
			if (rightcomment.GetComponent<uncom>().commented && !done) {
				// Colorize the TextMesh's text with this blocktext
				done = true;
				//@TODO: Magic numbers need to be explained here.
				//19 characters of how long a color is including the tag. The other 10 is the tag on the other side.
				blocktext = blocktext.Substring(19,blocktext.Length-29);
				code.GetComponent<TextMesh>().text = code.GetComponent<TextMesh>()
														 .text
														 .Replace(stringLib.BAD_UNCOMMENT_TEXT_COLOR_TAG_1 +
														 		  blocktext +
																  stringLib.COMMENT_CLOSE_COLOR_TAG,
																  stringLib.BAD_UNCOMMENT_TEXT_COLOR_TAG_2 +
																  blocktext +
																  stringLib.COMMENT_CLOSE_COLOR_TAG);
			}
		}
	}

	//.................................>8.......................................
	void OnTriggerEnter2D(Collider2D c) {
		if (c.name == stringLib.PROJECTILE_DEBUG) {
			Destroy(c.gameObject);
			GetComponent<AudioSource>().Play();
			lg.losing = true;
		}
	}

	//.................................>8.......................................
}

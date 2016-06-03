//**************************************************//
// Class Name: GenreicBug
// Class Description:
// Methods:
// 		void Start()
//		void Update()
//		void OnTriggerEnter2D(Collider2D p)
// 		void printLogFile(string sMessage)
// Author: Michael Miljanovic
// Date Last Modified: 6/1/2016
//**************************************************//

using UnityEngine;
using System.Collections;
using System.IO;

public class GenericBug : MonoBehaviour {

	public bool dead 	 = false;
	public bool finished = false;
	public Animator anim;
	public GameObject codescreen;

	//.................................>8.......................................
	// Use this for initialization
	void Start() {
		this.GetComponent<Renderer>().enabled = false;
		anim = GetComponent<Animator>();
	}

	//.................................>8.......................................
	// Update is called once per frame
	void Update() {
	}

	//.................................>8.......................................
	void OnTriggerEnter2D(Collider2D p) {
		if (p.name == stringLib.PROJECTILE_BUG) {
			printLogFile(stringLib.LOG_BUG_FOUND);
			this.GetComponent<Renderer>().enabled = true;
			Destroy(p.gameObject);
			anim.SetBool("Dying", true);
			GetComponent<AudioSource>().Play();
			dead = true;
			codescreen.GetComponent<LevelGenerator>().num_of_bugs--;
		}
	}

	//.................................>8.......................................
	void printLogFile(string sMessage)
	{
		int position = (int)((stateLib.GAMESETTING_INITIAL_LINE_Y - this.transform.position.y) / stateLib.GAMESETTING_LINE_SPACING);
		StreamWriter sw = new StreamWriter(stringLib.TOOL_LOGFILE, true);
		sMessage = sMessage + position.ToString() + ", " + Time.time.ToString();
		sw.WriteLine(sMessage);
		sw.Close();
	}

	//.................................>8.......................................

}

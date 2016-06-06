//**************************************************//
// Class Name: beacon
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
using System.Collections.Generic;

public class beacon : MonoBehaviour {

	public int actcounter = 0;
	public List<int> actnumbers;
	public GameObject codescreen;
	public Sprite activebeacon;

	private LevelGenerator lg;

	//.................................>8.......................................
	// Use this for initialization
	void Start() {
		lg = codescreen.GetComponent<LevelGenerator>();
	}

	//.................................>8.......................................
	// Update is called once per frame
	void Update() {
		//@TODO: Needs to be explained.
		// If beacon is done change it to green.
		if (lg.taskscompleted[0] == lg.tasklist[0]) {
			GetComponent<SpriteRenderer>().sprite = activebeacon;
		}
	}

	//.................................>8.......................................
	void OnTriggerEnter2D(Collider2D collidingObj) {
		if (collidingObj.name == stringLib.PROJECTILE_DEBUG) {
			Destroy(collidingObj.gameObject);
			if (GetComponent<SpriteRenderer>().sprite == activebeacon || actnumbers.Count == 0) {
				lg.isLosing = true;
			}
			else if (lg.taskscompleted[0] != actnumbers[actcounter]) {
				lg.isLosing = true;
			}
			else {
				GetComponent<AudioSource>().Play();
				lg.taskscompleted[0]++;
				actcounter++;
				if (actcounter == actnumbers.Count) {
					GetComponent<SpriteRenderer>().sprite = activebeacon;
				}
			}
		}
	}

	//.................................>8.......................................
}

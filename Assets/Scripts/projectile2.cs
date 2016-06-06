//**************************************************//
// Class Name: projectile2
// Class Description:
// Methods:
// 		void Start()
//		void Update()
//		void OnTriggerEnter2D(Collider2D collidingObj)
//		void printLogFile(string sMessage)
// Author: Michael Miljanovic
// Date Last Modified: 6/1/2016
//**************************************************//

using UnityEngine;
using System.Collections;
using System.IO;

public class projectile2 : MonoBehaviour {

	//.................................>8.......................................
	// Use this for initialization
	void Start() {
	}

	//.................................>8.......................................
	// Update is called once per frame
	void Update() {
	}

	//.................................>8.......................................
	void OnTriggerEnter2D(Collider2D collidingObj) {
		if (collidingObj.GetType() == typeof(EdgeCollider2D)) {
			string sMessage = this.name + stringLib.LOG_TOOL_WASTED;
			printLogFile(sMessage);
			Destroy(gameObject);
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

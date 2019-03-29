//**************************************************//
// Class Name: beacon
// Class Description: Instantiable object for the Robot ON! game. This controls the beacons
//                    and corresponds with the Beacon Activator tool.
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

	public int index = -1;
	public int actcounter = 0;
	public bool revOnce = false;
	public List<int> flowOrder;
	public string language;
	public GameObject CodescreenObject;
	public GameObject ToolSelectorObject;
	public AudioSource audioCorrect;
	public AudioSource audioRev;
	public Sprite activebeacon;
	public Sprite inactivebeacon;
	public Sprite progressbeacon;
	int flashCounter = 0;

	private LevelGenerator lg;

	//.................................>8.......................................
	// Use this for initialization
	void Start() {
		lg = CodescreenObject.GetComponent<LevelGenerator>();
	}

	//.................................>8.......................................
	// Update is called once per frame
	void Update() {
		// All beacons complete
		if (lg.taskscompleted[0] == lg.tasklist[0] && !revOnce) {
			revOnce = true;
			GetComponent<SpriteRenderer>().sprite = activebeacon;
			audioRev.Play();
		}
		else if (lg.taskscompleted[0] != lg.tasklist[0] && actcounter > 0)
		{
			flashCounter++;
			if (flashCounter > 50) {
				GetComponent<SpriteRenderer>().sprite = progressbeacon;
			}
			if (flashCounter > 100) {
				GetComponent<SpriteRenderer>().sprite = inactivebeacon;
				flashCounter = 0;
			}
		}
	}

	//.................................>8.......................................
	void OnTriggerEnter2D(Collider2D collidingObj) {
		if (collidingObj.name == stringLib.PROJECTILE_BUG) {
			Destroy(collidingObj.gameObject);
			if (GetComponent<SpriteRenderer>().sprite == activebeacon || flowOrder.Count == 0) {
				ToolSelectorObject.GetComponent<SelectedTool>().outputtext.GetComponent<GUIText>().text = "Beacons must be activated in the right\n order. Sometimes they are activated\n more than once, sometimes not at all.\n You will need to start over.";
				ResetAllBeacons();
			}
			else if (actcounter > flowOrder.Count - 1) {
				ToolSelectorObject.GetComponent<SelectedTool>().outputtext.GetComponent<GUIText>().text = "You have activated this \nbeacon enough times, but the \nsequence is now broken. \nYou will have to start over.";
				ResetAllBeacons();
			}
			else if (lg.taskscompleted[0] != flowOrder[actcounter]) {
				ToolSelectorObject.GetComponent<SelectedTool>().outputtext.GetComponent<GUIText>().text = "You will need to start the \nsequence again. Read the code carefully for \nclues.";
				ResetAllBeacons();
			}
			else {
				// Correct Selection
				audioCorrect.Play();
				lg.taskscompleted[0]++;
				// Award 1 extra use of the tool.
				ToolSelectorObject.GetComponent<SelectedTool>().bonusTools[stateLib.TOOL_CATCHER_OR_ACTIVATOR]++;
				actcounter++;
				/*
				if (actcounter == flowOrder.Count) {
				GetComponent<SpriteRenderer>().sprite = activebeacon;
			}
			*/
		}
		lg.toolsAirborne--;
	}
}

//.................................>8.......................................
void ResetAllBeacons() {
	foreach(GameObject beacon in lg.robotONbeacons) {
		beacon.GetComponent<beacon>().actcounter = 0;
		beacon.GetComponent<SpriteRenderer>().sprite = inactivebeacon;
		beacon.GetComponent<beacon>().flashCounter = 0;
		lg.taskscompleted[0] = 0;
	}
}
}

//**************************************************//
// Class Name: printer
// Class Description: Instantiable task object in the RoboBUG game
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

public class printer : MonoBehaviour {

	public int index = -1;
	public string language = "";
	public string displaytext = "";
	public GameObject SidebarObject;
	public GameObject ToolSelectorObject;
	public GameObject CodescreenObject;
	public int[] tools = new int[stateLib.NUMBER_OF_TOOLS];

	private LevelGenerator lg;
	private bool toolgiven = false;
	//.................................>8.......................................
	// Use this for initialization
	void Start() {
		lg = CodescreenObject.GetComponent<LevelGenerator>();
	}

	//.................................>8.......................................
	// Update is called once per frame
	void Update() {
	}

	//.................................>8.......................................
	void OnTriggerEnter2D(Collider2D collidingObj) {
		if (collidingObj.name == stringLib.PROJECTILE_ACTIVATOR) {
			Logger.printLogFile(stringLib.LOG_PRINTED, this.transform.position);
			Destroy(collidingObj.gameObject);
			SidebarObject.GetComponent<GUIText>().text = displaytext;
			GetComponent<AudioSource>().Play();
			if (!toolgiven) {
				toolgiven = true;
				for (int i = 0; i < stateLib.NUMBER_OF_TOOLS; i++) {
					if (tools[i] > 0) {
						lg.floatingTextOnPlayer(stringLib.INTERFACE_NEW_TOOLS);
					}
					ToolSelectorObject.GetComponent<SelectedTool>().toolCounts[i] += tools[i];
				}
			}
			lg.toolsAirborne--;
		}
	}

	//.................................>8.......................................
}

//**************************************************//
// Class Name: Breakpoint
// Class Description: Instantiable object for the RoboBUG game. This class controls the Breakpoints and corresponds
//                    with the breakpointer tool.
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

public class Breakpoint : MonoBehaviour {

	public int index = -1;
	public int[] tools = new int[stateLib.NUMBER_OF_TOOLS];
	public string values;
	public string language;
	public GameObject SidebarObject;
	public AudioClip[] sound = new AudioClip[2];
	public GameObject ToolSelectorObject;

	private bool activated = false;
	private bool toolgiven = false;

	//.................................>8.......................................
	// Use this for initialization
	void Start() {
		this.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 0.5f);
	}

	//.................................>8.......................................
	// Update is called once per frame
	void Update() {
	}

	//.................................>8.......................................
	void OnTriggerEnter2D(Collider2D collidingObj) {
		if (collidingObj.name == stringLib.PROJECTILE_DEBUG) {
			if (!activated) {
				GetComponent<AudioSource>().clip = sound[0];
				GetComponent<AudioSource>().Play();
				Logger.printLogFile(stringLib.LOG_BREAKPOINT_ON, this.transform.position);
			}
			activated = true;
			this.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 1);
		}
		else if (activated && collidingObj.name == stringLib.PROJECTILE_ACTIVATOR) {
			Logger.printLogFile(stringLib.LOG_BREAKPOINT_ACTIVATED, this.transform.position);
			GetComponent<AudioSource>().clip = sound[1];
			GetComponent<AudioSource>().Play();
			SidebarObject.GetComponent<GUIText>().text = values;
			if (!toolgiven) {
				toolgiven = true;
				for (int i = 0; i < stateLib.NUMBER_OF_TOOLS; i++) {
					if (tools[i] > 0) {
                        // Must be called from level generator, not ToolSelectorObject
						// lg.floatingTextOnPlayer("New Tools!");
					}
					ToolSelectorObject.GetComponent<SelectedTool>().toolCounts[i] += tools[i];
				}
			}
		}
	}

	//.................................>8.......................................

}

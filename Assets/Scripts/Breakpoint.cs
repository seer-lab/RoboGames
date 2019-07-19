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
using UnityEngine.UI; 

public class Breakpoint : Tools {

	public string values;

	Animator anim; 
	private bool activated = false;
	public override void Initialize(){
		anim = GetComponent<Animator>();
	}

	void OnTriggerEnter2D(Collider2D collidingObj) {
        if (!activated && collidingObj.name == stringLib.PROJECTILE_DEBUG) {
			if (!activated) {
				GetComponent<AudioSource>().clip = correct;
				GetComponent<AudioSource>().Play();
				GlobalState.CurrentLevelPoints+= stateLib.POINTS_BREAKPOINT;
			}
			activated = true;
			anim.SetTrigger("Complete");
			//breakpoint boxes turn into printers upon being activated. 
			this.GetComponent<SpriteRenderer>().sprite = Resources.LoadAll<Sprite>("Sprites/yellowbreakpoint")[0];
            Destroy(collidingObj.gameObject);
        }
		else if (activated && collidingObj.name == stringLib.PROJECTILE_ACTIVATOR) {
			Debug.Log("activated");
			GetComponent<AudioSource>().clip = correct;
			GetComponent<AudioSource>().Play();
			output.Text.text = values;

			// give bonus tools upon successful completion of using breakpoints.
			if (!toolgiven) {
				toolgiven = true;
				for (int i = 0; i < tools.Length; i++) {
					if (tools[i] > 0) {
                        // Must be called from level generator, not ToolSelectorObject
						lg.floatingTextOnPlayer(GlobalState.StringLib.COLORS[i]);
					}
					selectedTool.toolCounts[i] += tools[i];
				}
			}
			Destroy(collidingObj.gameObject); 
		}
		else if (collidingObj.name.Contains("projectile")){
		
			audioSource.PlayOneShot(wrong); 
		}
	}

	//.................................>8.......................................

}

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
using UnityEngine.UI; 
using System.Collections;
using System.IO;

public class printer : Tools {
	void Update(){
		if (hero.projectilecode == stateLib.TOOL_PRINTER_OR_QUESTION){
            EmphasizeTool(); 
        }else DeEmphasizeTool(); 
	}

	//.................................>8.......................................
	void OnTriggerEnter2D(Collider2D collidingObj) {
		if (collidingObj.name == stringLib.PROJECTILE_ACTIVATOR) {
			Destroy(collidingObj.gameObject);
			GlobalState.CurrentLevelPoints+= stateLib.POINTS_CHECKER; 
			if (displaytext.Contains("$err$")){
                output.Text.text = "<color=#B30730FF>ERROR: </color>" + displaytext.Replace("$err$", ""); 
            }
			else output.Text.text = displaytext;
			audioSource.PlayOneShot(correct); 
            GlobalState.level.CompletedTasks[1]++;
			if (!toolgiven) {
				toolgiven = true;
				for (int i = 0; i < tools.Length; i++) {
					if (tools[i] > 0) {
						lg.floatingTextOnPlayer(GlobalState.StringLib.COLORS[i]);
					}
					selectedTool.toolCounts[i] += tools[i];
				}
			}

		}
		else if (collidingObj.name.Contains("projectile")){

			audioSource.PlayOneShot(wrong); 
		}
	}

	//.................................>8.......................................
}

//**************************************************//
// Class Name: warper
// Class Description: Instantiable object in the RoboBUG game. This controls the warp objects and
//                    corresponds with the warper tool in that game.
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

public class warper : Tools
{
	public string Filename { get; set; }
	public string WarpToLine { get; set; }

	void OnTriggerEnter2D(Collider2D collidingObj) {
		if (collidingObj.name == stringLib.PROJECTILE_WARP) {
			string sMessage = stringLib.LOG_WARPED + Filename;
			Logger.printLogFile(sMessage, this.transform.position);
			Destroy(collidingObj.gameObject);
			lg.toolsAirborne--;
			if (!toolgiven) {
				toolgiven = true;
				for (int i = 0; i < stateLib.NUMBER_OF_TOOLS; i++) {
					if (tools[i] > 0) lg.floatingTextOnPlayer("New Tools!");
					selectedTool.bonusTools[i] += tools[i];
					if (selectedTool.toolCounts[i] == 0 && selectedTool.bonusTools[i] == 0) {
						selectedTool.toolIcons[i].GetComponent<Image>().enabled = false;
						if (selectedTool.projectilecode == i) selectedTool.NextTool();
						}
					}
				}
            GameObject.Find("Main Camera").GetComponent<GameController>().WarpLevel(GlobalState.GameMode + "leveldata" + GlobalState.FilePath + Filename, WarpToLine);
           
		}
	}
}

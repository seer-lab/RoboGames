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

public class warper : MonoBehaviour
{
	public int index = -1;
	public int[] tools = new int[stateLib.NUMBER_OF_TOOLS];
	public string filename = "";
	public string warpToLine = "";
	public string language;
	public GameObject CodescreenObject;
	public GameObject ToolSelectorObject;
	public GameObject Menu;


	private bool toolgiven = false;
	private LevelGenerator lg;

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
		if (collidingObj.name == stringLib.PROJECTILE_WARP) {
			string sMessage = stringLib.LOG_WARPED + filename;
			Logger.printLogFile(sMessage, this.transform.position);
			Destroy(collidingObj.gameObject);
			lg.toolsAirborne--;
			if (!toolgiven) {
				toolgiven = true;
				for (int i = 0; i < stateLib.NUMBER_OF_TOOLS; i++) {
					if (tools[i] > 0) lg.floatingTextOnPlayer("New Tools!");
					ToolSelectorObject.GetComponent<SelectedTool>().bonusTools[i] += tools[i];
					if (ToolSelectorObject.GetComponent<SelectedTool>().toolCounts[i] == 0 && ToolSelectorObject.GetComponent<SelectedTool>().bonusTools[i] == 0) {
						ToolSelectorObject.GetComponent<SelectedTool>().toolIcons[i].GetComponent<Image>().enabled = false;
						if (ToolSelectorObject.GetComponent<SelectedTool>().projectilecode == i) ToolSelectorObject.GetComponent<SelectedTool>().NextTool();
						}
					}
				}
            GameObject.Find("Main Camera").GetComponent<GameController>().SetLevel(GlobalState.GameMode + "leveldata" + GlobalState.FilePath + GlobalState.CurrentONLevel);
            //lg.BuildLevel(GlobalState.GameMode + "leveldata" + Menu.GetComponent<Menu>().filepath + filename, true, warpToLine);
		}
	}
	//.................................>8.......................................
}

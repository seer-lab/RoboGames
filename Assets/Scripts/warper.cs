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
    void Update(){
        if (hero.projectilecode == stateLib.TOOL_WARPER_OR_RENAMER){
            EmphasizeTool(); 
        }else DeEmphasizeTool(); 
    }
	public string Filename { get; set; }
	public string WarpToLine { get; set; }
	void OnTriggerEnter2D(Collider2D collidingObj) {
		if (collidingObj.name == stringLib.PROJECTILE_WARP) {
            GlobalState.CurrentLevelPoints+= stateLib.POINTS_WARPER; 
			string sMessage = stringLib.LOG_WARPED + Filename;
			Destroy(collidingObj.gameObject);
            if (!toolgiven)
            {
                toolgiven = true;
                for (int i = 0; i < tools.Length; i++)
                {
                    if (tools[i] > 0)
                    {
                        lg.floatingTextOnPlayer(GlobalState.StringLib.COLORS[i]);
                    }
                    selectedTool.toolCounts[i] += tools[i];                    Debug.Log(i + ": " + tools[i]);
                }
            }

            string filepath = "";
            #if UNITY_EDITOR || UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
                filepath = Path.Combine(Application.streamingAssetsPath, GlobalState.GameMode + "leveldata");
                if (GlobalState.Language == "python") filepath = Path.Combine(filepath, "python");
                filepath = Path.Combine(filepath,Filename);
                Debug.Log("warper: OnTriggerEnter2D() WINDOWS");
            #endif

            #if UNITY_WEBGL
                filepath = "StreamingAssets" + "/" + GlobalState.GameMode + "leveldata/";
                if (GlobalState.Language == "python") filepath =  filepath + "python/";
                filepath = filepath + Filename;
                Debug.Log("warper: OnTriggerEnter2D() WEBGL");
            #endif
            //factory = new LevelFactory(filepath);
            GameObject.Find("Main Camera").GetComponent<GameController>().WarpLevel(filepath, WarpToLine);
           
		}
        else{
            //hero.onFail(); 
        }
	}
}

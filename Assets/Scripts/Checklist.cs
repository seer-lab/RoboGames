using UnityEngine;
using System.Collections;

public class Checklist : MonoBehaviour {

	public GameObject codescreen;
	LevelGenerator lg;

	// Use this for initialization
	void Start () {
		GetComponent<GUIText> ().text = "";
		lg = codescreen.GetComponent<LevelGenerator> ();
	}
	
	// Update is called once per frame
	void Update () {
		if(lg.gamestate == 1 && lg.gamemode == "on"){
			GetComponent<GUIText> ().text = "Tasks:";
			if (lg.tasklist[0]>0){
				if (lg.tasklist[0]==lg.taskscompleted[0]){
					GetComponent<GUIText>().text += "\n<color=#00ff00ff>ACTIVATE the beacons in the right order.✓</color>";
				}else{
					GetComponent<GUIText>().text += "\n<color=#cccccccc>ACTIVATE</color> the beacons in the right order.";
				}
			}
			if (lg.tasklist[1]>0){
				if (lg.tasklist[1]==lg.taskscompleted[1]){
					GetComponent<GUIText>().text += "\n<color=#00ff00ff>CHECK the values of the variables.✓</color>";
				}else{
					GetComponent<GUIText>().text += "\n<color=#ffff00ff>CHECK</color> the values of the variables.";
				}
			}
			if (lg.tasklist[2]>0){
				if (lg.tasklist[2]==lg.taskscompleted[2]){
					GetComponent<GUIText>().text += "\n<color=#00ff00ff>NAME the variables with appropriate names.✓</color>";
				}else{
					GetComponent<GUIText>().text += "\n<color=#ff00ffff>NAME</color> the variables with appropriate names.";
				}
			}
			if (lg.tasklist[3]>0){
				if (lg.tasklist[3]==lg.taskscompleted[3]){
					GetComponent<GUIText>().text += "\n<color=#00ff00ff>COMMENT the lines that describe the code.✓</color>";
				}else{
					GetComponent<GUIText>().text += "\n<color=#00ff00ff>COMMENT</color> the lines that describe the code.";
				}
			}
			if (lg.tasklist[4]>0){
				if (lg.tasklist[4]==lg.taskscompleted[4]){
					GetComponent<GUIText>().text += "\n<color=#00ff00ff>UN-COMMENT the code that is correct.✓</color>";
				}else{
					GetComponent<GUIText>().text += "\n<color=#ff0000ff>UN-COMMENT</color> the code that is correct.";
				}
			}
		}else{
			GetComponent<GUIText> ().text = "";
		}
	}
}

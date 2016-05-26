using UnityEngine;
using System.Collections;
using System.IO;

public class Logger : MonoBehaviour {

	public GameObject hero;
	public GameObject codescreen;
	hero2Controller hc;
	LevelGenerator lg;
	int currentstate;
	StreamWriter output;
	/*float refreshTime = 5f;
	float lastTime;*/

	//float initialLineY = 3.5f;
	//float linespacing = 0.825f;

	// Use this for initialization
	void Start () {
		//hc = hero.GetComponent<hero2Controller> ();
		lg = codescreen.GetComponent<LevelGenerator> ();
		currentstate = 10;
		output = new StreamWriter ("statelog.txt",false);
		output.WriteLine("NewState,PreviousState#,Time,CurrentLevel");
		output.Close ();
		output = new StreamWriter("toollog.txt",false);
		output.WriteLine ("Tool,Location,Time");
		output.Close ();
		/*output = new StreamWriter("movelog.txt",false);
		output.WriteLine ("X,Y,Line#,Time");
		output.Close ();*/
	}
	
	// Update is called once per frame
	void Update () {
		if (currentstate != lg.gamestate) {
			output = new StreamWriter ("statelog.txt",true);
			switch(lg.gamestate){
			case 0:
				output.WriteLine("MenuAccessed,"+currentstate+","+Time.time+","+lg.currentlevel);
				break;
			case 1:
				output.WriteLine("LevelBegin,"+currentstate+","+Time.time+","+lg.currentlevel);
				//lastTime = Time.time;
				break;
			case 3:
				output.WriteLine("LevelComplete,"+currentstate+","+Time.time+","+lg.currentlevel);
				break;
			case 4:
				output.WriteLine("LevelFailed,"+currentstate+","+Time.time+","+lg.currentlevel);
				break;
			case 12:
				output.WriteLine("GameFinished,"+currentstate+","+Time.time+","+lg.currentlevel);
				break;
			}
			output.Close();
			currentstate = lg.gamestate;
		}
		/*
		if (lg.gamestate == 1) {
			if(lastTime+refreshTime<Time.time){
				output = new StreamWriter("movelog.txt",true);
				output.WriteLine(hero.transform.position.x+","+hero.transform.position.y+","+((int)((initialLineY-hero.transform.position.y)/linespacing)).ToString()+","+Time.time);
				output.Close();
				lastTime=Time.time;
			}
				}*/
	}
}

//**************************************************//
// Class Name: Logger
// Class Description: Class which stores log data on the filesystem. Anonymous collection of this data
//                    is handled in a different class.
// Methods:
// 		void Start()
//		void Update()
// Author: Michael Miljanovic
// Date Last Modified: 6/1/2016
//**************************************************//

using UnityEngine;
using System.Collections;
using System.IO;

public class Logger : MonoBehaviour {

	public GameObject hero;
	public GameObject codescreen;

	private int currentstate;
	private hero2Controller hc;
	private LevelGenerator lg;
	private StreamWriter output;

	//.................................>8.......................................
	// Use this for initialization
	void Start() {
		lg = codescreen.GetComponent<LevelGenerator>();
		currentstate = stateLib.GAMESTATE_INITIAL_COMIC;
		output = new StreamWriter(stringLib.TOOL_STATELOGFILE, false);
		output.WriteLine("NewState, PreviousState#, Time, CurrentLevel");
		output.Close();
		output = new StreamWriter(stringLib.TOOL_LOGFILE, false);
		output.WriteLine("Tool, Location, Time");
		output.Close();
	}

	//.................................>8.......................................
	// Update is called once per frame
	void Update() {
		if (currentstate != lg.gamestate) {
			output = new StreamWriter(stringLib.TOOL_STATELOGFILE, true);
			switch(lg.gamestate) {
				case stateLib.GAMESTATE_MENU:
					output.WriteLine("MenuAccessed, " + currentstate + ", " + Time.time + ", " + lg.currentlevel);
					break;
				case stateLib.GAMESTATE_IN_GAME:
					output.WriteLine("LevelBegin, " + currentstate + ", " + Time.time + ", " + lg.currentlevel);
					break;
				case stateLib.GAMESTATE_LEVEL_WIN:
					output.WriteLine("LevelComplete, " + currentstate + ", " + Time.time + ", " + lg.currentlevel);
					break;
				case stateLib.GAMESTATE_LEVEL_LOSE:
					output.WriteLine("LevelFailed, " + currentstate + ", " + Time.time + ", " + lg.currentlevel);
					break;
				case stateLib.GAMESTATE_GAME_END:
					output.WriteLine("GameFinished, " + currentstate + ", " + Time.time + ", " + lg.currentlevel);
					break;
				default:
					break;
			}
			output.Close();
			currentstate = lg.gamestate;
		}
}

//.................................>8.......................................
public static void printLogFile(string sMessage, Vector3 objectPosition) {
	int position = (int)((stateLib.GAMESETTING_INITIAL_LINE_Y - objectPosition.y) / stateLib.GAMESETTING_LINE_SPACING);
	StreamWriter sw = new StreamWriter(stringLib.TOOL_LOGFILE, true);
	sMessage = sMessage + position.ToString() + ", " + Time.time.ToString();
	sw.WriteLine(sMessage);
	sw.Close();
}
//.................................>8.......................................


}

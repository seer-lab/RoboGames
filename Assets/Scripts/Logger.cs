using System;
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
using System.Collections.Generic;
using System.IO;

public class Logger
{

    string id;
    bool failed;

    int timeStart, timeEnd, totalTime;
    DateTime time;

    int[] toolUse = new int[stateLib.NUMBER_OF_TOOLS];
	string[] linesUsed = new string[stateLib.NUMBER_OF_TOOLS]; 
	bool hasWritten = false; 

    public Logger()
    {
        timeStart = DateTime.Now.Second;
        failed = false;
		for (int i = 0; i < stateLib.NUMBER_OF_TOOLS; i++)
			linesUsed[i] = ""; 
    }
    public void onGameEnd()
    {
		if(hasWritten){
			return; 
		}
        timeEnd = DateTime.Now.Second;
        totalTime = timeEnd - timeStart;
        if (GlobalState.GameState == stateLib.GAMESTATE_LEVEL_LOSE)
        {
            failed = true;
        }
		hasWritten = true; 
        WriteLog();
    }
    public void onToolUse(int index, int lineNumber)
    {
        toolUse[index]++;
		linesUsed[index] += lineNumber.ToString() + ' '; 
    }
    public void WriteLog()
    {
        using (StreamWriter sw = File.AppendText(Application.dataPath.Replace("/Assets","") + "/Logging/" + id + GlobalState.CurrentONLevel.Replace(".xml", "") + ".txt"))
        {
            if (!failed) sw.WriteLine("Passed Level");
            else sw.WriteLine("Failed Level");
            sw.WriteLine(totalTime.ToString() + " Seconds");
            for (int i = 0; i < GlobalState.level.Tasks.Length; i++)
            {
                if (GlobalState.level.Tasks[i] > 0)
                {
                    if (GlobalState.GameMode == "on")
                        sw.WriteLine(GlobalState.StringLib.namesON[i] + " Tool Use: ");
                    else
                        sw.WriteLine(GlobalState.StringLib.namesBug[i] + " Tool Use: ");

                    sw.WriteLine("\tRequired Tasks: " + GlobalState.level.Tasks[i]);
                    sw.WriteLine("\tCompleted Tasks: " + GlobalState.level.CompletedTasks[i]);
                    sw.WriteLine("\tTimes Tool Used: " + toolUse[i]);
					sw.WriteLine("\tLines Used: " + linesUsed[i]); 
                    sw.WriteLine();
                }
            }
            sw.WriteLine("--------------------------------------------------------");
            sw.Close();
        }
    }

}

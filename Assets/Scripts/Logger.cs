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
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;
/// <summary>
/// A class that logs the game progress
/// </summary>
public class Logger
{

    string id;
    bool failed;

    int timeStart, timeEnd, totalTime;
    DateTime time, startTime, endTime;
	string[] linesUsed = new string[stateLib.NUMBER_OF_TOOLS]; 
	bool hasWritten = false;
    bool progress; 

    private string jsonObj = "";

    public Logger()
    {
        GlobalState.toolUse = new int[stateLib.NUMBER_OF_TOOLS];
        timeStart = DateTime.Now.Second;
        failed = false;
		for (int i = 0; i < stateLib.NUMBER_OF_TOOLS; i++)
			linesUsed[i] = "";

        startLogging();
    }

/// <summary>
/// A method that ends the logging for the current level and sends the log to the DB
/// </summary>
/// <param name="startTime">the start time of the leve</param>
/// <param name="progress">if the level has been passed or not</param>
    public void onGameEnd(DateTime startTime, bool progress)
    {
        this.startTime = startTime;
        this.endTime = DateTime.Now;
        this.progress = progress;
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
    
    public int CalculateTimeBonus(){
        int value = (GlobalState.level.Code.Length*3)/SecondsToCompleteLevel(); 
        //Debug.Log("Seconds to Complete: " + SecondsToCompleteLevel() + "\nCode Length: " + GlobalState.level.Code.Length); 
        if (value > 5) value = 5; 
        return value; 
    }
    public int SecondsToCompleteLevel(){
        if (totalTime > 0)
            return totalTime; 
        return 1; 
    }
    public void onToolUse(int index, int lineNumber)
    {
        GlobalState.toolUse[index]++;
		linesUsed[index] += lineNumber.ToString() + ' '; 
    }

/// <summary>
/// A method that sends and state change to the DB
/// </summary>
    public void onStateChangeJson(int projectileCode, int lineNumber, Vector3 position, 
                                    float energy, float currentEnergy, 
                                    bool progress, int time){
                                        
        LoggerDataStates states = new LoggerDataStates();
        states.position = new LoggerDataXY();
        states.preEnergy = energy.ToString();
        states.finEnergy = currentEnergy.ToString();

        if(GlobalState.GameMode == "on"){
            states.toolName = GlobalState.StringLib.namesON[projectileCode];
        }else{
            states.toolName = GlobalState.StringLib.namesBug[projectileCode];
        }
        states.position.line = lineNumber.ToString();
        states.position.x_pos = position.x.ToString();
        states.position.y_pos = position.y.ToString();
        Regex checkString = new Regex(@"\b" + lineNumber.ToString() + @"\b");
        if(projectileCode == stateLib.TOOL_UNCOMMENTER){
            projectileCode = stateLib.TOOL_COMMENTER;
        }
        Debug.Log(GlobalState.correctLine[projectileCode] + " : " + lineNumber);
        if(checkString.IsMatch(GlobalState.correctLine[projectileCode])){
            states.progress = "true";
        }else{
            states.progress = "false";
        }
        states.time = time.ToString();
        states.timestamp = DateTime.Now.ToString();
        string statesObj = JsonUtility.ToJson(states);
        statesObj = "{\"states\":" + statesObj + "}";
        //Debug.Log(statesObj);
        //Debug.Log(stringLib.DB_URL + GlobalState.GameMode.ToUpper() + "/currentlevel/" + GlobalState.positionalID.ToString() + "/" + GlobalState.currentLevelID + "/states");
        sendDatatoDB(statesObj, stringLib.DB_URL + GlobalState.GameMode.ToUpper() + "/currentlevel/" + GlobalState.sessionID + "/states");
    }

/// <summary>
/// A method that records the Damagae the player took and sends it to DB
/// </summary>
    public void onDamageStateJson(int obstacleCode, int lineNumber, Vector3 position,float energy, float currentEnergy){
        LoggerDataOStates obstacalStates = new LoggerDataOStates();
        obstacalStates.position = new LoggerDataXY();
        obstacalStates.name = GlobalState.StringLib.nameObstacle[obstacleCode];
        obstacalStates.preEnergy = energy.ToString();
        obstacalStates.finEnergy = currentEnergy.ToString();
        obstacalStates.position.line = lineNumber.ToString();
        obstacalStates.position.x_pos = position.x.ToString();
        obstacalStates.position.y_pos = position.y.ToString();
        obstacalStates.timestamp = DateTime.Now.ToString();

        string obstacalStateOBJ = JsonUtility.ToJson(obstacalStates);
        obstacalStateOBJ = "{\"obstacalState\":" +  obstacalStateOBJ+ "}";
        //Debug.Log( stringLib.DB_URL + GlobalState.GameMode.ToUpper() + "/currentlevel/" + GlobalState.positionalID.ToString() + "/" + GlobalState.currentLevelID + "/obstacalState");
        sendDatatoDB(obstacalStateOBJ, stringLib.DB_URL + GlobalState.GameMode.ToUpper() + "/currentlevel/" + GlobalState.sessionID + "/obstacalState");
    }
    
    //Yes I know, bad coding, what can I say ¯\_(ツ)_/¯

/// <summary>
/// A method that writes all the logs
/// </summary>
    public void WriteLog()
    {
        #if UNITY_WEBGL
        jsonObj = "{\"timeEnded\":\"" + DateTime.Now.ToString() + "\"}";
        //Debug.Log(stringLib.DB_URL + GlobalState.GameMode.ToUpper() + "/currentlevel/" + GlobalState.positionalID.ToString() + "/" + GlobalState.currentLevelID + "/timeEnded");
        sendDatatoDB(jsonObj,stringLib.DB_URL + GlobalState.GameMode.ToUpper() + "/currentlevel/" + GlobalState.sessionID + "/timeEnded" );

        if(!failed){
            jsonObj = "{\"progress\":\"Passed\"}";
        }else{
            jsonObj = "{\"progress\":\"Failed\"}";
        }
        //Debug.Log(stringLib.DB_URL + GlobalState.GameMode.ToUpper() + "/currentlevel/" + GlobalState.positionalID.ToString() + "/" + GlobalState.currentLevelID + "/progress");
        sendDatatoDB(jsonObj,stringLib.DB_URL + GlobalState.GameMode.ToUpper() + "/currentlevel/" + GlobalState.sessionID + "/progress");

        
        for(int i = 0; i < GlobalState.level.Tasks.Length; i++){
            LoggerDataTools tools = new LoggerDataTools();
            if(GlobalState.level.Tasks[i] > 0){
                if(GlobalState.GameMode == "on"){
                    tools.name= GlobalState.StringLib.namesON[i];
                }else{
                    tools.name= GlobalState.StringLib.namesBug[i];
                }
                tools.correctLine = GlobalState.correctLine[i];
                tools.reqTask = GlobalState.level.Tasks[i].ToString();
                tools.compTask = GlobalState.level.CompletedTasks[i].ToString();
                tools.timeTool = GlobalState.toolUse[i].ToString();
                tools.lineUsed = linesUsed[i];
            }
            string toolObj = JsonUtility.ToJson(tools);
            toolObj = "{\"tools\":" + toolObj + "}"; 
            //Debug.Log(stringLib.DB_URL + GlobalState.GameMode.ToUpper() + "/currentlevel/" + GlobalState.positionalID.ToString() + "/" + GlobalState.currentLevelID + "/tools");
            sendDatatoDB(toolObj, stringLib.DB_URL + GlobalState.GameMode.ToUpper() + "/currentlevel/" + GlobalState.sessionID + "/tools");

        }
        for(int i = 0; i < GlobalState.StringLib.nameObstacle.Length; i++){
            LoggerDataObstacal obstacal = new LoggerDataObstacal();
            if(GlobalState.obstacleLine[i] == null ||GlobalState.obstacleLine[i] == ""){
                continue;
            }
            obstacal.name = GlobalState.StringLib.nameObstacle[i];
            obstacal.line = GlobalState.obstacleLine[i];
            string obstacalOBJ = JsonUtility.ToJson(obstacal);
            //obstacalOBJ = "{\"obstacal\":" + obstacalOBJ + "}";
            Debug.Log(stringLib.DB_URL + GlobalState.GameMode.ToUpper() + "/currentlevel/" + GlobalState.positionalID.ToString() + "/" + GlobalState.currentLevelID + "/obstacal");
            sendDatatoDB(obstacalOBJ, stringLib.DB_URL + GlobalState.GameMode.ToUpper() + "/currentlevel/" + GlobalState.sessionID + "/obstacal");
        }   
        GlobalState.jsonStates = null;
        GlobalState.jsonOStates = null;
        #endif
    }

    public void sendPoints(){
        jsonObj = "{\"points\":\"" + GlobalState.CurrentLevelPoints.ToString() + "\"}";
        //Debug.Log(stringLib.DB_URL + GlobalState.GameMode.ToUpper() + "/currentlevel/" + GlobalState.positionalID.ToString() + "/" + GlobalState.currentLevelID + "/timeEnded");
        sendDatatoDB(jsonObj,stringLib.DB_URL + GlobalState.GameMode.ToUpper() + "/currentlevel/" + GlobalState.sessionID + "/points" );
        jsonObj = "{\"timeBonus\":\"" + GlobalState.timeBonus + "\"}";
        //Debug.Log(stringLib.DB_URL + GlobalState.GameMode.ToUpper() + "/currentlevel/" + GlobalState.positionalID.ToString() + "/" + GlobalState.currentLevelID + "/progress");
        sendDatatoDB(jsonObj,stringLib.DB_URL + GlobalState.GameMode.ToUpper() + "/currentlevel/" + GlobalState.sessionID + "/timeBonus");
    }

/// <summary>
/// A method that starts the logging, and sends the initial logs to the DB
/// </summary>
    public void startLogging(){
        LoggerDataLevel levelObj = new LoggerDataLevel();
        levelObj.name = GlobalState.CurrentONLevel;
        levelObj.time = "";
        levelObj.progress = "";
        levelObj.timeStarted = DateTime.Now.ToString();
        levelObj.timeEnded = "";
        string jsonOBJ = JsonUtility.ToJson(levelObj);
        jsonOBJ = "{\"levels\" : [" + jsonOBJ + "]}";
        //Debug.Log(stringLib.DB_URL + GlobalState.GameMode.ToUpper() + "/" + GlobalState.sessionID.ToString());
        sendDatatoDB(jsonOBJ,stringLib.DB_URL + GlobalState.GameMode.ToUpper() + "/" + GlobalState.sessionID.ToString());

    }

/// <summary>
/// A method that sends data to DB through PUT
/// </summary>

    public void sendDatatoDB(string jsonObj, string url){
        DatabaseHelperV2.i.url = url;
        DatabaseHelperV2.i.jsonData = jsonObj;
        DatabaseHelperV2.i.PutToDataBase();
    }

/// <summary>
/// A method that sends data to DB through POST
/// </summary>
    public void sendDatatoDBPOST(string jsonObj, string url){
        DatabaseHelperV2.i.url = url;
        DatabaseHelperV2.i.jsonData = jsonObj;
        DatabaseHelperV2.i.PostToDataBase();
    }
}
//--------------------------------------------------------------------------------DATA CLASS---------------------------------------------------------->

[Serializable]
public class LoggerDataLevel{
    public string name;
    public string time;
    public string progress;
    public string timeStarted;
    public string timeEnded;
}
[Serializable]
public class LoggerDataStates{
    public string preEnergy;
    public string finEnergy;
    public string toolName;
    public LoggerDataXY position;
    public string progress;
    public string time;
    public string timestamp;

}

[Serializable]
public class LoggerDataOStates{
    public string preEnergy;
    public string finEnergy;
    public string name;
    public LoggerDataXY position;
    public string timestamp;

}
[Serializable]
public class LoggerDataTools{
    public string name;
    public string correctLine;
    public string reqTask;
    public string compTask;
    public string timeTool;
    public string lineUsed;

}

[Serializable]
public class LoggerDataObstacal{
    public string name;
    public string line;

}

[Serializable]
public class LoggerDataXY{
    public string x_pos;
    public string y_pos;
    public string line;
}
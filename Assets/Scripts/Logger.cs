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

    //Old Variables
    //int timeStart, timeEnd, totalTime;
    int totalTime;
    DateTime time, startTime, endTime;
	string[] linesUsed = new string[stateLib.NUMBER_OF_TOOLS]; 
	bool hasWritten = false;
    bool progress; 

    private string jsonObj = "";

    float currentEnergy;

    public Logger()
    {
        GlobalState.toolUse = new int[stateLib.NUMBER_OF_TOOLS];
        this.startTime = DataTime.Now;

        failed = false;
		for (int i = 0; i < stateLib.NUMBER_OF_TOOLS; i++)
			linesUsed[i] = "";

        startLogging();
    }

    public Logger(bool sendData){

    }

/// <summary>
/// A method that ends the logging for the current level and sends the log to the DB
/// </summary>
/// <param name="startTime">the start time of the leve</param>
/// <param name="progress">if the level has been passed or not</param>
    public void onGameEnd(DateTime startTime, bool progress, float currentEnergy)
    {
        this.startTime = startTime;
        this.endTime = DateTime.Now;
        this.progress = progress;
        this.currentEnergy = currentEnergy;
		if(hasWritten){
			return; 
		}

        totalTime = Convert.ToInt32((endTime.Subtract(startTime).TotalSeconds));

        /* Taken out because it is not in a lose state, use the progress bool instead
        if (GlobalState.GameState == stateLib.GAMESTATE_LEVEL_LOSE)
        {
            failed = true;
        }
        */

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

        try{
            if(checkString.IsMatch(GlobalState.correctLine[projectileCode])){
                states.progress = "true";
            }else{
                states.progress = "false";
            }
        }catch(Exception e){
            if(checkString.IsMatch(GlobalState.bugLine)){
                states.progress = "true";
            }else{
                states.progress = "false";
            }
        }
        states.time = DateTime.Now.Subtract(startTime).TotalSeconds.ToString();
        states.timestamp = DateTime.Now.ToString();
        string statesObj = JsonUtility.ToJson(states);
        statesObj = "{\"states\":" + statesObj + "}";

        //Debug.Log(statesObj);
        //Debug.Log(stringLib.DB_URL + GlobalState.GameMode.ToUpper() + "/currentlevel/" + GlobalState.positionalID.ToString() + "/" + GlobalState.currentLevelID + "/states");
        sendDatatoDB(statesObj, stringLib.DB_URL + GlobalState.GameMode.ToUpper() + "/currentlevel/" + GlobalState.sessionID + "/states");
    }

    public void onStateChangeEnergy(string name, int lineNumber, Vector3 position, float energy, float currentEnergy, bool progress, int time){
        LoggerDataStates states = new LoggerDataStates();
        states.position = new LoggerDataXY();
        states.preEnergy = energy.ToString();
        states.finEnergy = currentEnergy.ToString();
        states.toolName = name;
        states.position.line = lineNumber.ToString();
        states.position.x_pos = position.x.ToString();
        states.position.y_pos = position.y.ToString();
        states.progress = true.ToString();
        states.time = DateTime.Now.Subtract(startTime).TotalSeconds.ToString();
        states.timestamp = DateTime.Now.ToString();

        string statesObj = JsonUtility.ToJson(states);
        statesObj = "{\"states\":" + statesObj + "}";
        
        sendDatatoDB(statesObj, stringLib.DB_URL + GlobalState.GameMode.ToUpper() + "/currentlevel/" + GlobalState.sessionID + "/states");
    }


/// <summary>
/// A method that records the Damagae the player took and sends it to DB
/// </summary>
    public void onDamageStateJson(int obstacleCode, int lineNumber, Vector3 position,float energy, float currentEnergy){
        LoggerDataOStates obstacalStates = new LoggerDataOStates();
        obstacalStates.position = new LoggerDataXY();
        if(obstacleCode == 4){
            obstacalStates.name = "Bug(Box)";
        }else if(obstacleCode == 3){
            obstacalStates.name = "Bug(Tri)";
        }
        obstacalStates.preEnergy = energy.ToString();
        obstacalStates.finEnergy = currentEnergy.ToString();
        obstacalStates.position.line = lineNumber.ToString();
        obstacalStates.position.x_pos = position.x.ToString();
        obstacalStates.position.y_pos = position.y.ToString();
        obstacalStates.timestamp = DateTime.Now.ToString();

        string obstacalStateOBJ = JsonUtility.ToJson(obstacalStates);
        obstacalStateOBJ = "{\"enemy\":" +  obstacalStateOBJ+ "}";

        //Debug.Log( stringLib.DB_URL + GlobalState.GameMode.ToUpper() + "/currentlevel/" + GlobalState.positionalID.ToString() + "/" + GlobalState.currentLevelID + "/obstacalState");
        sendDatatoDB(obstacalStateOBJ, stringLib.DB_URL + GlobalState.GameMode.ToUpper() + "/currentlevel/" + GlobalState.sessionID + "/enemy");
    }
    
    //Yes I know, bad coding, what can I say ¯\_(ツ)_/¯

/// <summary>
/// A method that writes all the logs
/// </summary>
    public void WriteLog()
    {
        jsonObj = "{\"timeEnded\":\"" + DateTime.Now.ToString() + "\"}";
        //Debug.Log(stringLib.DB_URL + GlobalState.GameMode.ToUpper() + "/currentlevel/" + GlobalState.positionalID.ToString() + "/" + GlobalState.currentLevelID + "/timeEnded");
        sendDatatoDB(jsonObj,stringLib.DB_URL + GlobalState.GameMode.ToUpper() + "/currentlevel/" + GlobalState.sessionID + "/timeEnded" );

		jsonObj = "{\"AdaptiveMode\":\"" + GlobalState.AdaptiveMode.ToString() + "\"}";
        sendDatatoDB(jsonObj,stringLib.DB_URL + GlobalState.GameMode.ToUpper() + "/currentlevel/" + GlobalState.sessionID + "/AdaptiveMode" );
		
		jsonObj = "{\"HintMode\":\"" + GlobalState.HintMode.ToString() + "\"}";
        sendDatatoDB(jsonObj,stringLib.DB_URL + GlobalState.GameMode.ToUpper() + "/currentlevel/" + GlobalState.sessionID + "/HintMode" );

        string totalT = endTime.Subtract(startTime).TotalSeconds.ToString();
        jsonObj = "{\"time\" : \"" + totalT + "\"}"; 
        sendDatatoDB(jsonObj,stringLib.DB_URL + GlobalState.GameMode.ToUpper() + "/currentlevel/" + GlobalState.sessionID + "/time");

		jsonObj = "{\"failures\":\"" + GlobalState.failures.ToString() + "\"}";
        sendDatatoDB(jsonObj,stringLib.DB_URL + GlobalState.GameMode.ToUpper() + "/currentlevel/" + GlobalState.sessionID + "/failures" );

        jsonObj = "{\"finalEnergy\" : \"" + this.currentEnergy.ToString() + "\"}"; 
        sendDatatoDB(jsonObj,stringLib.DB_URL + GlobalState.GameMode.ToUpper() + "/currentlevel/" + GlobalState.sessionID + "/finalEnergy");

        if(progress){
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
                if(tools.name == stringLib.INTERFACE_TOOL_NAME_0_ROBOBUG){
                    tools.correctLine = GlobalState.bugLine;
                }else{
                    tools.correctLine = GlobalState.correctLine[i];
                }
                tools.reqTask = GlobalState.level.Tasks[i].ToString();
                tools.compTask = GlobalState.level.CompletedTasks[i].ToString();
                tools.timeTool = GlobalState.toolUse[i].ToString();
                tools.lineUsed = linesUsed[i];
            }
            string toolObj = JsonUtility.ToJson(tools);
            toolObj = "{\"tools\":" + toolObj + "}"; 
            if(tools.name != "" && tools.name != null && tools.correctLine != ""){
                sendDatatoDB(toolObj, stringLib.DB_URL + GlobalState.GameMode.ToUpper() + "/currentlevel/" + GlobalState.sessionID + "/tools");
            }
            //Debug.Log(stringLib.DB_URL + GlobalState.GameMode.ToUpper() + "/currentlevel/" + GlobalState.positionalID.ToString() + "/" + GlobalState.currentLevelID + "/tools");

        }

        if(GlobalState.jsonStates != null){
            string[] obs_enem = GlobalState.jsonStates.Split('\n');
            for(int i = 0; i < obs_enem.Length - 1; i++){
                string[] tmpObs_enem = obs_enem[i].Split(',');
                LoggerDataObstacal obstacal = new LoggerDataObstacal();
                obstacal.name = tmpObs_enem[0];
                obstacal.line = tmpObs_enem[1];

                string obstacalOBJ = JsonUtility.ToJson(obstacal);
                obstacalOBJ = "{\"obstacle\":" + obstacalOBJ + "}";
                sendDatatoDB(obstacalOBJ, stringLib.DB_URL + GlobalState.GameMode.ToUpper() + "/currentlevel/" + GlobalState.sessionID + "/obstacle");
            }

        }

        GlobalState.jsonStates = null;
        GlobalState.jsonOStates = null;
    }

    public void sendPoints(){
        jsonObj = "{\"points\":\"" + GlobalState.CurrentLevelPoints.ToString() + "\"}";
        //Debug.Log(stringLib.DB_URL + GlobalState.GameMode.ToUpper() + "/currentlevel/" + GlobalState.positionalID.ToString() + "/" + GlobalState.currentLevelID + "/timeEnded");
        sendDatatoDB(jsonObj,stringLib.DB_URL + GlobalState.GameMode.ToUpper() + "/currentlevel/" + GlobalState.sessionID + "/points" );

        jsonObj = "{\"timeBonus\":\"" + GlobalState.currentLevelTimeBonus + "\"}";
        //Debug.Log(stringLib.DB_URL + GlobalState.GameMode.ToUpper() + "/currentlevel/" + GlobalState.positionalID.ToString() + "/" + GlobalState.currentLevelID + "/progress");
        sendDatatoDB(jsonObj,stringLib.DB_URL + GlobalState.GameMode.ToUpper() + "/currentlevel/" + GlobalState.sessionID + "/timeBonus");


        jsonObj = "{\"star\":\"" + GlobalState.currentLevelStar.ToString() + "\"}";
        //Debug.Log(stringLib.DB_URL + GlobalState.GameMode.ToUpper() + "/currentlevel/" + GlobalState.positionalID.ToString() + "/" + GlobalState.currentLevelID + "/progress");
        sendDatatoDB(jsonObj,stringLib.DB_URL + GlobalState.GameMode.ToUpper() + "/currentlevel/" + GlobalState.sessionID + "/star");

        jsonObj = "{\"totalPoint\":\"" + GlobalState.totalPointsCurrent.ToString() + "\"}";
        sendDatatoDB(jsonObj,stringLib.DB_URL + GlobalState.GameMode.ToUpper() + "/currentlevel/" + GlobalState.sessionID + "/totalPoint" );
    }

    public void sendUpgrades(string name, int points, int curPoints){
        LoggerDataUpgrades upgrades = new LoggerDataUpgrades();
        upgrades.name = name;
        upgrades.prePoints = points.ToString();
        upgrades.curPoints = curPoints.ToString();
        upgrades.timestamp = DateTime.Now.ToString();

        string jsonOBJ = JsonUtility.ToJson(upgrades);
        jsonOBJ = "{\"upgrades\":" + jsonOBJ + "}";
        sendDatatoDB(jsonOBJ, stringLib.DB_URL + GlobalState.GameMode.ToUpper() + "/currentlevel/" + GlobalState.sessionID.ToString() + "/upgrades");
        
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
        if(GlobalState.LoggingMode == false){
            return;
        }
        DatabaseHelperV2.i.url = url;
        DatabaseHelperV2.i.jsonData = jsonObj;
        DatabaseHelperV2.i.PutToDataBase();
    }

/// <summary>
/// A method that sends data to DB through POST
/// </summary>
    public void sendDatatoDBPOST(string jsonObj, string url){
        if(GlobalState.LoggingMode == false){
            return;
        }
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
    public string finalEnergy;
    public string totalPoint;
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

[Serializable]
public class LoggerDataUpgrades{
    public string name;
    public string prePoints;
    public string curPoints;
    public string timestamp;
}

[Serializable]
public class LoggerDataStart{
    public string name;
    public string username;
    public string timeStarted;
}

[Serializable]
public class LoggerPoints{
    public string totalPoints;
    public string currentPoints;
    public string speedUpgrades;
    public string xpUpgrades;
    public string resistanceUpgrade;
    public string energyUpgrades;

    public static LoggerPoints CreateFromJson(string jsonString){
        try{
            LoggerPoints lg = JsonUtility.FromJson<LoggerPoints>(jsonString);
            return lg;
        }catch(Exception e){
            return null;
        }
    }
}
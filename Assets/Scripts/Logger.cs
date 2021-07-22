using System;
using System.Text.RegularExpressions;
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
/// <summary>
/// A class that logs the game progress
/// </summary>
public class Logger
{

    string id;
    bool failed;

    //int timeStart, timeEnd, totalTime;
    int totalTime;
    DateTime time, startTime, endTime;
    string[] linesUsed = new string[stateLib.NUMBER_OF_TOOLS];
    bool hasWritten = false;
    string progress;

    private string jsonObj = "";

    float currentEnergy;

    public Logger()
    {
        GlobalState.toolUse = new int[stateLib.NUMBER_OF_TOOLS];
        //timeStart = DateTime.Now.Second;
        this.startTime = DateTime.UtcNow;

        failed = false;
        for (int i = 0; i < stateLib.NUMBER_OF_TOOLS; i++)
            linesUsed[i] = "";

        //Checks that the level hasn't been failed before initializing
        if (GlobalState.failures == 0)
        {
            startLogging();
        }
    }

    public Logger(bool sendData)
    {

    }

    /// <summary>
    /// A method that ends the logging for the current level and sends the log to the DB
    /// </summary>
    /// <param name="startTime">the start time of the leve</param>
    /// <param name="progress">if the level has been passed,failed, or ongoing</param>
    public void onGameEnd(DateTime startTime, string progress, float currentEnergy)
    {
        this.startTime = startTime;
        this.endTime = DateTime.UtcNow;
        this.progress = progress;
        this.currentEnergy = currentEnergy;

        /*if(hasWritten){
			return; 
		}*/

        //timeEnd = DateTime.Now.Second;
        totalTime = Convert.ToInt32((endTime.Subtract(startTime).TotalSeconds));

        /* Taken out because it is not in a lose state, use the progress bool instead
        if (GlobalState.GameState == stateLib.GAMESTATE_LEVEL_LOSE)
        {
            failed = true;
        }
        */

        hasWritten = true;

        //Debug.Log("OnGameEnd startTime: " + this.startTime);
        //Debug.Log("OnGameEnd endTime: " + this.endTime);
        //Debug.Log("OnGameEnd timeEnd: " + timeEnd);
        //Debug.Log("OnGameEnd totalTime: " + totalTime);

        WriteLog();
    }

    public int CalculateTimeBonus()
    {
        int value = (GlobalState.level.Code.Length * 3) / SecondsToCompleteLevel();
        //Debug.Log("Seconds to Complete: " + SecondsToCompleteLevel() + "\nCode Length: " + GlobalState.level.Code.Length); 
        if (value > 5) value = 5;
        return value;
    }
    public int SecondsToCompleteLevel()
    {
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
    /// Seems to only be called when throwing the tool
    public void onStateChangeJson(int projectileCode, int lineNumber, Vector3 position,
                                    float energy, float currentEnergy,
                                    bool progress, int time)
    {

        LoggerDataStates states = new LoggerDataStates();

        if (GlobalState.VerboseLoggingMode == 1)
        {
            states.position = new LoggerDataXY();
            states.position.x_pos = position.x.ToString();
            states.position.y_pos = position.y.ToString();
            states.preEnergy = energy.ToString();
            states.finEnergy = currentEnergy.ToString();
            states.comment = "N/A";
        }


        if (GlobalState.GameMode == "on")
        {
            states.eventName = GlobalState.StringLib.namesON[projectileCode];
        }
        else
        {
            states.eventName = GlobalState.StringLib.namesBug[projectileCode];
        }

        Regex checkString = new Regex(@"\b" + lineNumber.ToString() + @"\b");
        if (projectileCode == stateLib.TOOL_UNCOMMENTER)
        {
            projectileCode = stateLib.TOOL_COMMENTER;
        }

        try
        {
            if (checkString.IsMatch(GlobalState.correctLine[projectileCode]))
            {
                states.success = "true";
            }
            else
            {
                states.success = "false";
            }
        }
        catch (Exception e)
        {
            if (checkString.IsMatch(GlobalState.bugLine))
            {
                states.success = "true";
            }
            else
            {
                states.success = "false";
            }
        }

        states.elapsedTime = DateTime.UtcNow.Subtract(startTime).TotalSeconds.ToString();
        states.realTime = DateTime.UtcNow.ToString();
        states.eventType = "ToolUsed";
        states.line = lineNumber.ToString();

        string statesObj = JsonUtility.ToJson(states);
        statesObj = "{\"states\":" + statesObj + "}";

        sendDatatoDB(statesObj, stringLib.DB_URL + GlobalState.GameMode.ToUpper() + "/currentlevel/" + GlobalState.courseCode + "/" + GlobalState.sessionID + "/states");

        if (String.Equals(states.success, "false"))
        {
            GlobalState.failedTool++;
        }
    }

    public void sendFailure()
    {
        jsonObj = "{\"failedToolUse\":\"" + GlobalState.failedTool.ToString() + "\"}";
        sendDatatoDB(jsonObj, stringLib.DB_URL + GlobalState.GameMode.ToUpper() + "/currentlevel/" + GlobalState.courseCode + "/" + GlobalState.sessionID + "/failedToolUse");

        jsonObj = "{\"hitByEnemy\":\"" + GlobalState.hitByEnemy.ToString() + "\"}";
        sendDatatoDB(jsonObj, stringLib.DB_URL + GlobalState.GameMode.ToUpper() + "/currentlevel/" + GlobalState.courseCode + "/" + GlobalState.sessionID + "/hitByEnemy");

        jsonObj = "{\"failures\":\"" + GlobalState.failures.ToString() + "\"}";
        sendDatatoDB(jsonObj, stringLib.DB_URL + GlobalState.GameMode.ToUpper() + "/currentlevel/" + GlobalState.courseCode + "/" + GlobalState.sessionID + "/failures");
    }

    //Called only when a energy pack is obtained
    public void onStateChangeEnergy(string name, int lineNumber, Vector3 position, float energy, float currentEnergy, bool progress, int time)
    {
        LoggerDataStates states = new LoggerDataStates();

        if (GlobalState.VerboseLoggingMode == 1)
        {
            states.position = new LoggerDataXY();
            states.position.x_pos = position.x.ToString();
            states.position.y_pos = position.y.ToString();
            states.preEnergy = energy.ToString();
            states.finEnergy = currentEnergy.ToString();
            states.comment = "N/A";
        }

        states.eventName = name;
        states.line = lineNumber.ToString();
        states.success = true.ToString();
        states.elapsedTime = DateTime.UtcNow.Subtract(startTime).TotalSeconds.ToString();
        states.realTime = DateTime.UtcNow.ToString();

        string statesObj = JsonUtility.ToJson(states);
        statesObj = "{\"states\":" + statesObj + "}";

        sendDatatoDB(statesObj, stringLib.DB_URL + GlobalState.GameMode.ToUpper() + "/currentlevel/" + GlobalState.courseCode + "/" + GlobalState.sessionID + "/states");
    }
    /// <summary>
    /// A method that records the Damagae the player took and sends it to DB
    /// </summary>
    /// Note that this is only used when player is hit by an obstacle
    public void onDamageStateJson(int obstacleCode, int lineNumber, Vector3 position, float energy, float currentEnergy)
    {
        LoggerDataOStates obstacleStates = new LoggerDataOStates();
        obstacleStates.position = new LoggerDataXY();

        if (obstacleCode == 4)
        {
            obstacleStates.eventName = "Bug(Box)";
        }
        else if (obstacleCode == 3)
        {
            obstacleStates.eventName = "Bug(Tri)";
        }

        if (GlobalState.VerboseLoggingMode == 1)
        {
            obstacleStates.preEnergy = energy.ToString();
            obstacleStates.finEnergy = currentEnergy.ToString();
            obstacleStates.position.x_pos = position.x.ToString();
            obstacleStates.position.y_pos = position.y.ToString();
            obstacleStates.comment = "N/A";
        }

        obstacleStates.eventType = "HitByEnemy";
        obstacleStates.line = lineNumber.ToString();
        obstacleStates.realTime = DateTime.UtcNow.ToString();
        obstacleStates.elapsedTime = DateTime.UtcNow.Subtract(startTime).TotalSeconds.ToString();

        string obstacleStateOBJ = JsonUtility.ToJson(obstacleStates);
        obstacleStateOBJ = "{\"enemy\":" + obstacleStateOBJ + "}";

        sendDatatoDB(obstacleStateOBJ, stringLib.DB_URL + GlobalState.GameMode.ToUpper() + "/currentlevel/" + GlobalState.courseCode + "/" + GlobalState.sessionID + "/enemy");

        GlobalState.hitByEnemy++;
    }

    //Yes I know, bad coding, what can I say ¯\_(ツ)_/¯
    //*Edit I made the bad coding into more clear bad coding ¯\_(ツ)_/¯. 

    /// <summary>
    /// Logs information in the database after a level ends.
    /// </summary>
    public void WriteLog()
    {

        jsonObj = "{\"timeEnded\":\"" + DateTime.UtcNow.ToString() + "\"}";
        sendDatatoDB(jsonObj, stringLib.DB_URL + GlobalState.GameMode.ToUpper() + "/currentlevel/" + GlobalState.courseCode + "/" + GlobalState.sessionID + "/timeEnded");

        jsonObj = "{\"AdaptiveCategorization\":\"" + GlobalState.AdaptiveMode.ToString() + "\"}";
        sendDatatoDB(jsonObj, stringLib.DB_URL + GlobalState.GameMode.ToUpper() + "/currentlevel/" + GlobalState.courseCode + "/" + GlobalState.sessionID + "/AdaptiveCategorization");

        jsonObj = "{\"HintMode\":\"" + GlobalState.HintMode.ToString() + "\"}";
        sendDatatoDB(jsonObj, stringLib.DB_URL + GlobalState.GameMode.ToUpper() + "/currentlevel/" + GlobalState.courseCode + "/" + GlobalState.sessionID + "/HintMode");

        jsonObj = "{\"AdaptiveMode\":\"" + GlobalState.AdaptiveOffON.ToString() + "\"}";
        sendDatatoDB(jsonObj, stringLib.DB_URL + GlobalState.GameMode.ToUpper() + "/currentlevel/" + GlobalState.courseCode + "/" + GlobalState.sessionID + "/AdaptiveMode");

        string totalT = endTime.Subtract(startTime).TotalSeconds.ToString();
        jsonObj = "{\"time\" : \"" + totalT + "\"}";
        sendDatatoDB(jsonObj, stringLib.DB_URL + GlobalState.GameMode.ToUpper() + "/currentlevel/" + GlobalState.courseCode + "/" + GlobalState.sessionID + "/time");

        jsonObj = "{\"failures\":\"" + GlobalState.failures.ToString() + "\"}";
        sendDatatoDB(jsonObj, stringLib.DB_URL + GlobalState.GameMode.ToUpper() + "/currentlevel/" + GlobalState.courseCode + "/" + GlobalState.sessionID + "/failures");

        jsonObj = "{\"finalEnergy\" : \"" + this.currentEnergy.ToString() + "\"}";
        sendDatatoDB(jsonObj, stringLib.DB_URL + GlobalState.GameMode.ToUpper() + "/currentlevel/" + GlobalState.courseCode + "/" + GlobalState.sessionID + "/finalEnergy");

        if (String.Equals(progress, "Passed"))
        {
            jsonObj = "{\"progress\":\"Passed\"}";
        }
        else if (String.Equals(progress, "Failed"))
        {
            jsonObj = "{\"progress\":\"Failed\"}";
        }
        else
        {
            jsonObj = "{\"progress\":\"Ongoing\"}";
        }
        sendDatatoDB(jsonObj, stringLib.DB_URL + GlobalState.GameMode.ToUpper() + "/currentlevel/" + GlobalState.courseCode + "/" + GlobalState.sessionID + "/progress");

        jsonObj = "{\"failedToolUse\":\"" + GlobalState.failedTool.ToString() + "\"}";
        sendDatatoDB(jsonObj, stringLib.DB_URL + GlobalState.GameMode.ToUpper() + "/currentlevel/" + GlobalState.courseCode + "/" + GlobalState.sessionID + "/failedToolUse");

        jsonObj = "{\"hitByEnemy\":\"" + GlobalState.hitByEnemy.ToString() + "\"}";
        sendDatatoDB(jsonObj, stringLib.DB_URL + GlobalState.GameMode.ToUpper() + "/currentlevel/" + GlobalState.courseCode + "/" + GlobalState.sessionID + "/hitByEnemy");

        jsonObj = "{\"bugLine\" : \"" + GlobalState.bugLine + "\"}";
        sendDatatoDB(jsonObj, stringLib.DB_URL + GlobalState.GameMode.ToUpper() + "/currentlevel/" + GlobalState.courseCode + "/" + GlobalState.sessionID + "/bugLine");

        /* Old Tool Logging
        for (int i = 0; i < GlobalState.level.Tasks.Length; i++){
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
                sendDatatoDB(toolObj, stringLib.DB_URL + GlobalState.GameMode.ToUpper() + "/currentlevel/" + GlobalState.courseCode + "/"  + GlobalState.sessionID + "/tools");
            }
            //Debug.Log(stringLib.DB_URL + GlobalState.GameMode.ToUpper() + "/currentlevel/" + GlobalState.positionalID.ToString() + "/" + GlobalState.currentLevelID + "/tools");

        }
        */


        /* This is used to log the enemy location and name. No Longer in use.
        if (GlobalState.jsonStates != null)
        {
            string[] obs_enem = GlobalState.jsonStates.Split('\n');
            for (int i = 0; i < obs_enem.Length - 1; i++)
            {
                string[] tmpObs_enem = obs_enem[i].Split(',');
                LoggerDataObstacle obstacle = new LoggerDataObstacle();
                obstacle.eventType = "EnemyLocation";
                obstacle.eventName = tmpObs_enem[0];
                obstacle.line = tmpObs_enem[1];

                if (GlobalState.VerboseLoggingMode == 1)
                {
                    obstacle.comment = "N/A";
                }

                string obstacleOBJ = JsonUtility.ToJson(obstacle);
                obstacleOBJ = "{\"obstacle\":" + obstacleOBJ + "}";
                sendDatatoDB(obstacleOBJ, stringLib.DB_URL + GlobalState.GameMode.ToUpper() + "/currentlevel/" + GlobalState.courseCode + "/" + GlobalState.sessionID + "/obstacle");
            }

        }
        */
        GlobalState.jsonStates = null;
        GlobalState.jsonOStates = null;
    }

    public void sendPoints()
    {
        jsonObj = "{\"points\":\"" + GlobalState.CurrentLevelPoints.ToString() + "\"}";
        sendDatatoDB(jsonObj, stringLib.DB_URL + GlobalState.GameMode.ToUpper() + "/currentlevel/" + GlobalState.courseCode + "/" + GlobalState.sessionID + "/points");

        jsonObj = "{\"timeBonus\":\"" + GlobalState.currentLevelTimeBonus + "\"}";
        sendDatatoDB(jsonObj, stringLib.DB_URL + GlobalState.GameMode.ToUpper() + "/currentlevel/" + GlobalState.courseCode + "/" + GlobalState.sessionID + "/timeBonus");

        jsonObj = "{\"star\":\"" + GlobalState.currentLevelStar.ToString() + "\"}";
        sendDatatoDB(jsonObj, stringLib.DB_URL + GlobalState.GameMode.ToUpper() + "/currentlevel/" + GlobalState.courseCode + "/" + GlobalState.sessionID + "/star");

        jsonObj = "{\"totalPoint\":\"" + GlobalState.totalPointsCurrent.ToString() + "\"}";
        sendDatatoDB(jsonObj, stringLib.DB_URL + GlobalState.GameMode.ToUpper() + "/currentlevel/" + GlobalState.courseCode + "/" + GlobalState.sessionID + "/totalPoint");
    }

    public void sendUpgrades(string name, int points, int curPoints)
    {
        LoggerDataUpgrades upgrades = new LoggerDataUpgrades();
        upgrades.name = name;
        upgrades.prePoints = points.ToString();
        upgrades.curPoints = curPoints.ToString();
        upgrades.timestamp = DateTime.UtcNow.ToString();
        Debug.Log("Send Upgrades timestamp: " + upgrades.timestamp);

        string jsonOBJ = JsonUtility.ToJson(upgrades);
        jsonOBJ = "{\"upgrades\":" + jsonOBJ + "}";
        sendDatatoDB(jsonOBJ, stringLib.DB_URL + GlobalState.GameMode.ToUpper() + "/currentlevel/" + GlobalState.courseCode + "/" + GlobalState.sessionID.ToString() + "/upgrades");

    }

    /// <summary>
    /// A method that starts the logging, and sends the initial logs to the DB
    /// </summary>
    public void startLogging()
    {
        Debug.Log("Resume: " + GlobalState.IsResume);
        Debug.Log("Level: " + GlobalState.CurrentONLevel);

        LoggerDataLevel levelObj = new LoggerDataLevel();
        levelObj.name = GlobalState.CurrentONLevel;
        levelObj.time = "";
        levelObj.progress = "";
        levelObj.timeStarted = DateTime.UtcNow.ToString();
        levelObj.timeEnded = "N/A";
        levelObj.totalPoint = "0";
        string jsonOBJ = JsonUtility.ToJson(levelObj);
        jsonOBJ = "{\"levels\" : [" + jsonOBJ + "]}";

        sendDatatoDB(jsonOBJ, stringLib.DB_URL + GlobalState.GameMode.ToUpper() + "/" + GlobalState.courseCode + "/" + GlobalState.sessionID.ToString());
    }

    /// <summary>
    /// A method that sends data to DB through PUT
    /// </summary>

    public void sendDatatoDB(string jsonObj, string url)
    {
        if (GlobalState.LoggingMode == false)
        {
            return;
        }

        DatabaseHelperV2.i.url = url;
        DatabaseHelperV2.i.jsonData = jsonObj;
        DatabaseHelperV2.i.PutToDataBase();
    }

    /// <summary>
    /// A method that sends data to DB through POST
    /// </summary>
    public void sendDatatoDBPOST(string jsonObj, string url)
    {
        if (GlobalState.LoggingMode == false)
        {
            return;
        }
        DatabaseHelperV2.i.url = url;
        DatabaseHelperV2.i.jsonData = jsonObj;
        DatabaseHelperV2.i.PostToDataBase();
    }
}
//--------------------------------------------------------------------------------DATA CLASS---------------------------------------------------------->

[Serializable]
public class LoggerDataLevel
{
    public string name;
    public string time;
    public string progress;
    public string timeStarted;
    public string timeEnded;
    public string finalEnergy;
    public string totalPoint;
}
[Serializable]
public class LoggerDataStates
{
    public string eventType;
    public string eventName;
    public string line;
    public string success;
    public string realTime;
    public string elapsedTime;

    //Verbose Variables
    public string preEnergy;
    public string finEnergy;
    public LoggerDataXY position;
    public string comment;
}

[Serializable]
public class LoggerDataOStates
{
    public string eventType;
    public string eventName;
    public string line;
    public string realTime;
    public string elapsedTime;

    //Verbose Variables
    public string preEnergy;
    public string finEnergy;
    public LoggerDataXY position;
    public string comment;
}


/* Old Tool Logging
[Serializable]
public class LoggerDataTools{
    public string name;
    public string correctLine;
    public string reqTask;
    public string compTask;
    public string timeTool;
    public string lineUsed;

}
*/

[Serializable]
public class LoggerDataObstacle
{
    public string eventType;
    public string eventName;
    public string line;

    //Verbose Variables
    public string comment;
}

[Serializable]
public class LoggerDataXY
{
    public string x_pos;
    public string y_pos;
}

[Serializable]
public class LoggerDataUpgrades
{
    public string name;
    public string prePoints;
    public string curPoints;
    public string timestamp;
}

[Serializable]
public class LoggerDataStart
{
    public string name;
    public string username;
    public string timeStarted;
}

[Serializable]
public class LoggerPoints
{
    public string totalPoints;
    public string currentPoints;
    public string speedUpgrades;
    public string xpUpgrades;
    public string resistanceUpgrade;
    public string energyUpgrades;

    public static LoggerPoints CreateFromJson(string jsonString)
    {
        try
        {
            LoggerPoints lg = JsonUtility.FromJson<LoggerPoints>(jsonString);
            return lg;
        }
        catch (Exception e)
        {
            return null;
        }
    }
}

public class LoggerCourseCodeStart
{
    public string courseCode;
    public LoggerDataStart studentStart;
}
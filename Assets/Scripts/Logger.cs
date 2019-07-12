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
using System.Text;
using System.IO;

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
        WebHelper.i.url = stringLib.DB_URL + GlobalState.GameMode.ToUpper() + "/totallevel/" + GlobalState.sessionID.ToString();
        WebHelper.i.GetWebDataFromWeb();
        GlobalState.positionalID = Convert.ToInt32(WebHelper.i.webData);

        WebHelper.i.url = stringLib.DB_URL + GlobalState.GameMode.ToUpper() + "/currentlevel/" + GlobalState.sessionID.ToString();
        WebHelper.i.GetWebDataFromWeb();
        GlobalState.currentLevelID = WebHelper.i.webData.Substring(1,WebHelper.i.webData.Length - 2);

        Debug.Log("posID: " + GlobalState.positionalID + " levelID: " + GlobalState.currentLevelID);

         
    }
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
        return totalTime; 
    }
    public void onToolUse(int index, int lineNumber)
    {
        GlobalState.toolUse[index]++;
		linesUsed[index] += lineNumber.ToString() + ' '; 
    }

    public void onStateChangeJson(int projectileCode, int lineNumber, Vector3 position, 
                                    float energy, float currentEnergy, 
                                    bool progress, int time){

        GlobalState.jsonStates += "{\"states\":{";

        GlobalState.jsonStates += "\"preEnergy\":\"" + energy.ToString() + "\",";
        GlobalState.jsonStates += "\"finEnergy\":\"" + currentEnergy.ToString() + "\",";

        if(GlobalState.GameMode == "on"){
            GlobalState.jsonStates += "\"toolName\":\"" + GlobalState.StringLib.namesON[projectileCode] + "\",";
        }else{
            GlobalState.jsonStates += "\"toolName\":\"" + GlobalState.StringLib.namesBug[projectileCode] + "\",";
        }

        GlobalState.jsonStates += "\"position\":{ \"line\":\"" + lineNumber.ToString() + "\",";
        GlobalState.jsonStates += "\"x_pos\":\"" + position.x.ToString() + "\",";
        GlobalState.jsonStates += "\"y_pos\":\"" + position.y.ToString() + "\"},";
        GlobalState.jsonStates += "\"progress\":\"" + progress.ToString() + "\",";
        GlobalState.jsonStates += "\"time\":\"" + time.ToString() + "\",";
        GlobalState.jsonStates += "\"timestamp\":\"" + DateTime.Now.ToString() + "\"}}";
        
        sendDatatoDB(GlobalState.jsonStates, 
                        stringLib.DB_URL + GlobalState.GameMode.ToUpper() + "/currentlevel/" + GlobalState.positionalID.ToString() + "/" + GlobalState.currentLevelID + "/states");
        GlobalState.jsonStates = "";
    }

    public void onDamageStateJson(int obstacleCode, int lineNumber, Vector3 position,float energy, float currentEnergy){
        if(GlobalState.jsonOStates == null || GlobalState.jsonOStates == ""){
            GlobalState.jsonOStates += "{\"obstacalState\":{";
        }else{
            GlobalState.jsonOStates += ",{";
        }

        GlobalState.jsonOStates += "\"name\":\"" + GlobalState.StringLib.nameObstacle[obstacleCode] + "\",";
        GlobalState.jsonOStates += "\"preEnergy\":\"" + energy.ToString() + "\",";
        GlobalState.jsonOStates += "\"finEnergy\":\"" + currentEnergy.ToString() + "\",";
        GlobalState.jsonOStates += "\"position\":{ \"line\":\"" + lineNumber.ToString() + "\",";
        GlobalState.jsonOStates += "\"x_pos\":\"" + position.x.ToString() + "\",";
        GlobalState.jsonOStates += "\"y_pos\":\"" + position.y.ToString() + "\"},";
        GlobalState.jsonOStates += "\"timestamp\":\"" + DateTime.Now.ToString() + "\"}";
        Debug.Log("Damage State Change: " + GlobalState.jsonOStates);

        sendDatatoDB(GlobalState.jsonOStates, stringLib.DB_URL + GlobalState.GameMode.ToUpper() + "/currentlevel/" + GlobalState.positionalID.ToString() + "/" + GlobalState.currentLevelID + "/obstacalState");
        GlobalState.jsonOStates = "";
    }
    
    public void WriteLog()
    {
        #if UNITY_WEBGL
        jsonObj = "{\"timeEnded\":\"" + DateTime.Now.ToString() + "\"}";
        sendDatatoDB(jsonObj,stringLib.DB_URL + GlobalState.GameMode.ToUpper() + "/currentlevel/" + GlobalState.positionalID.ToString() + "/" + GlobalState.currentLevelID + "/timeEnded" );

        if(!failed){
            jsonObj = "{\"progress\":\"Passed\"}";
        }else{
            jsonObj = "{\"progress\":\"Failed\"}";
        }
        sendDatatoDB(jsonObj,stringLib.DB_URL + GlobalState.GameMode.ToUpper() + "/currentlevel/" + GlobalState.positionalID.ToString() + "/" + GlobalState.currentLevelID + "/progress");

        
        for(int i = 0; i < GlobalState.level.Tasks.Length; i++){
            jsonObj = "{\"tools\":";
            if(GlobalState.level.Tasks[i] > 0){
                if(GlobalState.GameMode == "on"){
                    jsonObj+= "{ \"name\": \"" + GlobalState.StringLib.namesON[i] + "\",";
                }else{
                    jsonObj+= "{ \"name\": \"" + GlobalState.StringLib.namesBug[i] + "\",";
                }
                jsonObj += "\"correctLine\": \"" + GlobalState.correctLine[i] + "\",";
                jsonObj += "\"reqTask\": \"" + GlobalState.level.Tasks[i] + "\",";
                jsonObj += "\"compTask\": \"" + GlobalState.level.CompletedTasks[i] + "\",";
                jsonObj += "\"timeTool\": \"" + GlobalState.toolUse[i] + "\",";
                jsonObj += "\"lineUsed\": \"" + linesUsed[i] + "\"}}";
            }
            if(jsonObj != "" && !jsonObj.Equals( "{\"tools\":")){
                sendDatatoDB(jsonObj, stringLib.DB_URL + GlobalState.GameMode.ToUpper() + "/currentlevel/" + GlobalState.positionalID.ToString() + "/" + GlobalState.currentLevelID + "/tools");
            }
            jsonObj = "";
        }
        string obstacleJson = "";
        for(int i = 0; i < GlobalState.StringLib.nameObstacle.Length; i++){
            obstacleJson = "{\"obstacal\":";
            if(GlobalState.obstacleLine[i] == null ||GlobalState.obstacleLine[i] == ""){
                continue;
            }
            obstacleJson+=  "{ \"name\": \"" + GlobalState.StringLib.nameObstacle[i] + "\",";
            obstacleJson +=  "\"line\": \"" + GlobalState.obstacleLine[i] + "\"}";

            if(obstacleJson != "{\"obstacal\":"){
                sendDatatoDB(obstacleJson, stringLib.DB_URL + GlobalState.GameMode.ToUpper() + "/currentlevel/" + GlobalState.positionalID.ToString() + "/" + GlobalState.currentLevelID + "/obstacal");
            }
            obstacleJson = "";
        }   
        GlobalState.jsonStates = null;
        GlobalState.jsonOStates = null;
        #endif

        #if (UNITY_EDITOR || UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN) && !UNITY_WEBGL
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
                    sw.WriteLine("\tTimes Tool Used: " + GlobalState.toolUse[i]);
					sw.WriteLine("\tLines Used: " + linesUsed[i]); 
                    sw.WriteLine();
                }
            }
            sw.WriteLine("--------------------------------------------------------");
            sw.Close();
        }
        #endif
    }

    public void startLogging(){
        jsonObj = "{ \"levels\":[{ \"name\": \"" + GlobalState.CurrentONLevel + "\" ";
        jsonObj += ", \"time\": \"" + totalTime.ToString() + "\" ";
        jsonObj += ", \"progress\": \"\"";   

        jsonObj += ", \"timeStarted\": \"" + DateTime.Now.ToString() + "\" ";
        jsonObj += ", \"timeEnded\": \"" + "\" ";
        jsonObj += ", \"tools\":[";
        jsonObj += "], \"states\" :[], \"obstacal\" : [], \"obstacalState\" : [], \"enemy\" : []}]}"; 

        string url = stringLib.DB_URL + GlobalState.GameMode.ToUpper() + "/" + GlobalState.sessionID.ToString();
        Debug.Log(url);
        sendDatatoDB(jsonObj,stringLib.DB_URL + GlobalState.GameMode.ToUpper() + "/" + GlobalState.sessionID.ToString());

    }

    public void sendDatatoDB(string jsonObj, string url){
        DatabaseHelperV2.i.url = url;
        DatabaseHelperV2.i.jsonData = jsonObj;
        DatabaseHelperV2.i.PutToDataBase();
    }

    public void sendDatatoDBPOST(string jsonObj, string url){
        DatabaseHelperV2.i.url = url;
        DatabaseHelperV2.i.jsonData = jsonObj;
        DatabaseHelperV2.i.PostToDataBase();
    }
}

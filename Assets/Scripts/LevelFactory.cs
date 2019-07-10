using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Text.RegularExpressions;
using System; 
using System.IO;
using UnityEngine;
using UnityEngine.Networking;


public class LevelFactory 
{

    IList<XmlNode> nodelist;
    Level level; 

    public LevelFactory(string filename, bool warp = false)
    {
        level = new Level(); 
        if (warp)
            BuildFromCurrent(filename); 
        else
            BuildLevel(filename); 
    }
    public Level GetLevel()
    {
        return level; 
    }
    private void BuildFile(XmlDocument doc, string filename)
    {
        XmlNode levelnode = doc.FirstChild;
        level.Tags = XMLReader.GetOuterXML(doc);
        //@TODO: This is a bug. InnerXML should not be OuterXML. Need to convert all outerXML to InnerXML.
        //innerXmlLines = outerXmlLines;
        level.LevelNode = levelnode;
        level.CodeNodes = levelnode.ChildNodes;
        //level.Language = "c++";
        level.Language = XMLReader.GetLanguage(doc);
        //Debug.Log(level.Language);
        string innerXMLstring = XMLReader.convertOuterToInnerXML(String.Join("\n", level.Tags), level.Language);
        level.Code = innerXMLstring.Split('\n');

        level.LineCount = XMLReader.GetLineCount(doc);
        level.TaskOnLine = new int[level.LineCount, stateLib.NUMBER_OF_TOOLS];

        level.NodeList = XMLReader.GetToolNodes(doc);
        #if UNITY_WEBGL
            level.FileName = GlobalState.CurrentONLevel;
        #else
            level.FileName = filename.Substring(filename.IndexOf(GlobalState.FilePath) + 1);
        #endif
        level.Failure_Level = XMLReader.GetFailureLevel(doc);
        if (level.Failure_Level == null || level.Failure_Level == ""){
            level.Failure_Level = filename.Split('/').ToList().Last(); 
        }

        level.Description = XMLReader.GetLevelDescription(doc); 

        if (level.FileName.Contains("tutorial")){
            level.IsDemo = true; 
        }else{ 
            level.IsDemo = false;
        }

    }
    private void BuildFromCurrent(string filename)
    {

        level = GlobalState.level;
        XmlDocument doc = null;
        #if (UNITY_EDITOR || UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN) && !UNITY_WEBGL
            Debug.Log("LevelFactory: BuildFromCurrent() WINDOWS");
            doc = XMLReader.ReadFile(filename);
        #endif

        #if UNITY_WEBGL
            doc = new XmlDocument();
            doc.PreserveWhitespace = true;
            doc.LoadXml(filename);
            Debug.Log("LevelFactory: BuildFromCurrent() WEBGL");
        #endif
        BuildFile(doc, filename); 

    }
    
    private void BuildLevel(string filename)
    {
        level.Tasks = new int[5];
        level.CompletedTasks = new int[5];

        XmlDocument doc = null;
        #if (UNITY_EDITOR || UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN) && !UNITY_WEBGL
            //Debug.Log("LevelFactory: BuildLevel() WINDOWS");
            doc = XMLReader.ReadFile(filename);
        #endif

        #if UNITY_WEBGL
            doc = new XmlDocument();
            //Debug.Log("Build Level "+ filename);
            doc.PreserveWhitespace = true;
            doc.LoadXml(filename);
            //Debug.Log("LevelFactory: BuildLevel() WEBGL");
        #endif

        BuildFile(doc, filename); 
        // time
        try
        {
            string sReadTime = XMLReader.GetTimeLimit(doc);
            sReadTime = (sReadTime.ToLower() == "unlimited") ? "9001" : sReadTime;
            level.Time = (float)int.Parse(sReadTime);
        }
        catch(Exception e)
        {
            Debug.Log(e.Message);
            level.Time = 9001; 
        }

        // next level
        //level.NextLevel =Application.streamingAssetsPath+ "/" + GlobalState.GameMode + "leveldata" + GlobalState.FilePath + XMLReader.GetNextLevel(doc);        
        level.NextLevel = XMLReader.GetNextLevel(doc);
        // intro text
        level.IntroText = XMLReader.GetIntroText(doc);
        // end text
        level.ExitText = XMLReader.GetEndText(doc);
        level.Hint = XMLReader.GetHints(doc);
    }
}

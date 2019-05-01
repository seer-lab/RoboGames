using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Text.RegularExpressions;
using System; 
using UnityEngine;


public class LevelFactory 
{

    IList<XmlNode> nodelist;
    Level level; 

    public LevelFactory(string filename)
    {
        level = new Level();
        BuildLevel(filename); 
    }
    public Level GetLevel()
    {
        return level; 
    }
    public void BuildLevel(string filename)
    {
        level.Tasks = new int[5];
        level.CompletedTasks = new int[5]; 
        XmlDocument doc = XMLReader.ReadFile(filename);
        XmlNode levelnode = doc.FirstChild;
        level.Tags = XMLReader.GetOuterXML(doc);
        //@TODO: This is a bug. InnerXML should not be OuterXML. Need to convert all outerXML to InnerXML.
        //innerXmlLines = outerXmlLines;
        level.LevelNode = levelnode; 
        level.CodeNodes = levelnode.ChildNodes;
        level.Language = "c++"; 
        string innerXMLstring = XMLReader.convertOuterToInnerXML(String.Join("\n", level.Tags), level.Language);
        level.Code = innerXMLstring.Split('\n');

        level.LineCount = XMLReader.GetLineCount(doc);
        level.TaskOnLine = new int[level.LineCount, stateLib.NUMBER_OF_TOOLS];

        level.NodeList = XMLReader.GetToolNodes(doc);
        level.FileName = filename.Substring(filename.IndexOf(GlobalState.FilePath) + 1);
        // time
        try
        {
            string sReadTime = XMLReader.GetTimeLimit(doc);
            sReadTime = (sReadTime.ToLower() == "unlimited") ? "9001" : sReadTime;
            level.Time = (float)int.Parse(sReadTime);
        }
        catch(Exception e)
        {
            level.Time = 9001; 
        }
        // next level
        level.NextLevel = GlobalState.GameMode + "leveldata" + GlobalState.FilePath + XMLReader.GetNextLevel(doc);
        // intro text
        level.IntroText = XMLReader.GetIntroText(doc);
        // end text
        level.ExitText = XMLReader.GetEndText(doc);
    }
}

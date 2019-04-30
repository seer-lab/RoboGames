using System.Collections;
using System.Collections.Generic;
using System.IO; 
using UnityEngine;
using System.Xml; 

public class LevelManager
{
    public List<string> levels;
    public List<string> passed;
    private string filepath;

    public GameObject levelBug;

    public List<GameObject> robotONrenamers;
    public List<GameObject> robotONvariablecolors;
    public List<GameObject> lines;
    public List<GameObject> prints;
    public List<GameObject> roboBUGwarps;
    public List<GameObject> bugs;
    public List<GameObject> roboBUGcomments;


    public List<GameObject> robotONcorrectComments;
    public List<GameObject> robotONincorrectComments;
    public List<GameObject> robotONcorrectUncomments;
    public List<GameObject> robotONincorrectUncomments;

    public List<GameObject> robotONquestions;
    public List<GameObject> roboBUGbreakpoints;
    public List<GameObject> roboBUGprizes;
    // Stores the robotONbeacons used in this level.
    public List<GameObject> robotONbeacons;

    CodeProperties properties; 

    public LevelManager(CodeProperties properties)
    {
        this.properties = properties;
        filepath = (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor) ? @"\" : @"/";
        levels = new List<string>();
        passed = new List<string>();
        SetTitle();

        lines = new List<GameObject>();
        prints = new List<GameObject>();
        bugs = new List<GameObject>();
        roboBUGwarps = new List<GameObject>();
        roboBUGcomments = new List<GameObject>();
        roboBUGbreakpoints = new List<GameObject>();
        roboBUGprizes = new List<GameObject>();
        robotONbeacons = new List<GameObject>();
        robotONincorrectComments = new List<GameObject>();
        robotONcorrectUncomments = new List<GameObject>();
        robotONincorrectUncomments = new List<GameObject>();
        robotONrenamers = new List<GameObject>();
        robotONvariablecolors = new List<GameObject>();
        robotONcorrectComments = new List<GameObject>();
        robotONquestions = new List<GameObject>();
    }
    //Resetting the level shouldn't be required anymore but further testing should be done
    //.................................>8.......................................
    //************************************************************************//
    // Method: public void ResetLevel(bool warp)
    // Description: Completely resets the level. Will destroy all game objects,
    // 				and reset task list and bug values to their default state.
    //				If isWarping is TRUE, then retain current tools. If FALSE, reset
    //				the tool count.
    //************************************************************************//

    public void DestroyInstances()
    {
        // Destroy objects
        foreach (GameObject line in lines)
        {
            GameObject.Destroy(line);
        }
        foreach (GameObject robotONbeacon in robotONbeacons)
        {
            GameObject.Destroy(robotONbeacon);
        }
        foreach (GameObject correctComment in robotONcorrectComments)
        {
            GameObject.Destroy(correctComment);
        }
        foreach (GameObject question in robotONquestions)
        {
            GameObject.Destroy(question);
        }
        foreach (GameObject variablerenames in robotONrenamers)
        {
            GameObject.Destroy(variablerenames);
        }
        foreach (GameObject variablecolor in robotONvariablecolors)
        {
            GameObject.Destroy(variablecolor);
        }
        foreach (GameObject print in prints)
        {
            GameObject.Destroy(print);
        }
        foreach (GameObject incorrectComment in robotONincorrectComments)
        {
            GameObject.Destroy(incorrectComment);
        }
        foreach (GameObject warp in roboBUGwarps)
        {
            GameObject.Destroy(warp);
        }
        foreach (GameObject roboBUGComment in roboBUGcomments)
        {
            GameObject.Destroy(roboBUGComment);
        }
        foreach (GameObject correctUncomment in robotONcorrectUncomments)
        {
            GameObject.Destroy(correctUncomment);
        }
        foreach (GameObject incorrectUncomment in robotONincorrectUncomments)
        {
            GameObject.Destroy(incorrectUncomment);
        }
        foreach (GameObject breakpoint in roboBUGbreakpoints)
        {
            GameObject.Destroy(breakpoint);
        }
        foreach (GameObject roboBUGprize in roboBUGprizes)
        {
            GameObject.Destroy(roboBUGprize);
        }
        if (levelBug)
        {
            GameObject.Destroy(levelBug);
        }

        
        // Reset local variables
       
        lines = new List<GameObject>();
        prints = new List<GameObject>();
        roboBUGwarps = new List<GameObject>();
        robotONcorrectComments = new List<GameObject>();
        robotONrenamers = new List<GameObject>();
        robotONvariablecolors = new List<GameObject>();
        robotONincorrectComments = new List<GameObject>();
        robotONquestions = new List<GameObject>();
        robotONcorrectUncomments = new List<GameObject>();
        robotONincorrectUncomments = new List<GameObject>();
        robotONbeacons = new List<GameObject>();
        bugs = new List<GameObject>();
        roboBUGcomments = new List<GameObject>();
        roboBUGbreakpoints = new List<GameObject>();
        roboBUGprizes = new List<GameObject>();
        
    }
    //.................................>8.......................................
    //************************************************************************//
    // Method: int void CreateLevelObjects(XmlNode parentNode)
    // Description: Handle XML parsing. Right now it just returns 1 if a tag was found.
    //              Returns 0 if an XML tag was not found.
    //************************************************************************//
    public GameObject CreateLevelObject(string language, XmlNode childnode, int lineNumber)
    {
        if (childnode.NodeType != XmlNodeType.Element)
        {
            return null;
        }
        ToolFactory toolFactory;
        switch (childnode.Name)
        {
            case stringLib.NODE_NAME_PRINT:
                {
                    toolFactory = new PrinterFactory(childnode, lineNumber);
                    GameObject newoutput = toolFactory.GetGameObject();
                    newoutput.transform.position = new Vector3(stateLib.LEFT_CODESCREEN_X_COORDINATE, properties.initialLineY - lineNumber * properties.linespacing, 1);
                    return newoutput;
                }
            case stringLib.NODE_NAME_WARP:
                {
                    toolFactory = new WarperFactory(childnode, lineNumber);
                    GameObject newWarper = toolFactory.GetGameObject();
                    newWarper.transform.position = new Vector3(stateLib.LEFT_CODESCREEN_X_COORDINATE, properties.initialLineY - (lineNumber) * properties.linespacing, 1);
                    return newWarper;
                }
            case stringLib.NODE_NAME_BUG:
                {
                    int bugsize = int.Parse(childnode.Attributes[stringLib.XML_ATTRIBUTE_SIZE].Value);
                    int row = 0;
                    if (childnode.Attributes[stringLib.XML_ATTRIBUTE_ROW].Value != null)
                    {
                        row = int.Parse(childnode.Attributes[stringLib.XML_ATTRIBUTE_ROW].Value);
                    }
                    int col = 0;
                    if (childnode.Attributes[stringLib.XML_ATTRIBUTE_COL].Value != null)
                    {
                        col = int.Parse(childnode.Attributes[stringLib.XML_ATTRIBUTE_COL].Value);
                    }
                    //RoboBug Implementation 
                    /*
                    levelBug = GameObject.Instantiate(bugobject, new Vector3(properties.bugXshift + col * properties.fontwidth + (bugsize - 1) * properties.levelLineRatio, properties.initialLineY - (lineNumber + row + 0.5f * (bugsize - 1)) * properties.linespacing + 0.4f, 0f), transform.rotation);
                    levelBug.transform.localScale += new Vector3(properties.bugscale * (bugsize - 1), properties.bugscale * (bugsize - 1), 0);
                    GenericBug propertyHandler = levelBug.GetComponent<GenericBug>();
                    GlobalState.level.TaskOnLine[lineNumber, stateLib.TOOL_CATCHER_OR_ACTIVATOR]++;
                    bugs.Add(levelBug);
                    //numberOfBugsRemaining++;
                    return levelBug;
                    */
                    return null; 
                }
            case stringLib.NODE_NAME_COMMENT:
                {
                    toolFactory = new CommentFactory(childnode, lineNumber);
                    GameObject newcomment = toolFactory.GetGameObject();
                    newcomment.transform.position = new Vector3(stateLib.LEFT_CODESCREEN_X_COORDINATE, properties.initialLineY - lineNumber * properties.linespacing, 1);
                    switch (((CommentFactory)toolFactory).Entity)
                    {
                        case stateLib.ENTITY_TYPE_ROBOBUG_COMMENT:
                            roboBUGbreakpoints.Add(newcomment);
                            break;
                        case stateLib.ENTITY_TYPE_CORRECT_COMMENT:
                            robotONcorrectComments.Add(newcomment);
                            break;
                        case stateLib.ENTITY_TYPE_INCORRECT_COMMENT:
                            robotONincorrectComments.Add(newcomment);
                            break;
                        case stateLib.ENTITY_TYPE_CORRECT_UNCOMMENT:
                            robotONcorrectUncomments.Add(newcomment);
                            break;
                        case stateLib.ENTITY_TYPE_INCORRECT_UNCOMMENT:
                            robotONincorrectUncomments.Add(newcomment);
                            break;
                        default: break;
                    }
                    return newcomment;
                }
            case stringLib.NODE_NAME_QUESTION:
                {
                    toolFactory = new QuestionFactory(childnode, lineNumber);
                    GameObject newquestion = toolFactory.GetGameObject();
                    newquestion.transform.position = new Vector3(stateLib.LEFT_CODESCREEN_X_COORDINATE, properties.initialLineY - lineNumber * properties.linespacing, 1);
                    robotONquestions.Add(newquestion);
                    return newquestion;
                }
            case stringLib.NODE_NAME_RENAME:
                {
                    toolFactory = new RenamerFactory(childnode, lineNumber);
                    GameObject newrename = toolFactory.GetGameObject();
                    newrename.transform.position = new Vector3(stateLib.LEFT_CODESCREEN_X_COORDINATE, properties.initialLineY + stateLib.TOOLBOX_Y_OFFSET - lineNumber * properties.linespacing, 1);
                    robotONrenamers.Add(newrename);
                    return newrename;
                }
            case stringLib.NODE_NAME_BREAKPOINT:
                {
                    toolFactory = new BreakpointFactory(childnode, lineNumber);
                    GameObject newbreakpoint = toolFactory.GetGameObject();
                    newbreakpoint.transform.position = new Vector3(-10, properties.initialLineY - lineNumber * properties.linespacing + 0.4f, 1);
                    roboBUGbreakpoints.Add(newbreakpoint);
                    return newbreakpoint;
                }
            case stringLib.NODE_NAME_PRIZE:
                {
                    int bugsize = int.Parse(childnode.Attributes[stringLib.XML_ATTRIBUTE_SIZE].Value);
                    //RoboBug Implementaiton 
                    /*
                    GameObject prizebug = GameObject.Instantiate(prizeobject, new Vector3(-9f + (bugsize - 1) * levelLineRatio, initialLineY - (lineNumber + 0.5f * (bugsize - 1)) * linespacing + 0.4f, 0f), transform.rotation);
                    prizebug.transform.localScale += new Vector3(bugscale * (bugsize - 1), bugscale * (bugsize - 1), 0);
                    PrizeBug propertyHandler = prizebug.GetComponent<PrizeBug>();
                    string[] bonuses = childnode.Attributes[stringLib.XML_ATTRIBUTE_BONUSES].Value.Split(',');
                    for (int i = 0; i < stateLib.NUMBER_OF_TOOLS; i++)
                    {
                        propertyHandler.bonus[i] += int.Parse(bonuses[i]);
                    }
                    roboBUGprizes.Add(prizebug);
                    return prizebug;
                    */
                    return null; 
                }
            case stringLib.NODE_NAME_BEACON:
                {
                    toolFactory = new BeaconFactory(childnode, lineNumber);
                    GameObject newbeacon = toolFactory.GetGameObject();
                    newbeacon.transform.position = new Vector3(-9.95f, properties.initialLineY - lineNumber * properties.linespacing + properties.lineOffset + 0.4f, 1);
                    robotONbeacons.Add(newbeacon);
                    return newbeacon;
                }
            case stringLib.NODE_NAME_VARIABLE_COLOR:
                {
                    toolFactory = new VariableColorFactory(childnode, lineNumber);
                    GameObject newvariablecolor = toolFactory.GetGameObject();
                    newvariablecolor.transform.position = new Vector3(stateLib.LEFT_CODESCREEN_X_COORDINATE, properties.initialLineY - lineNumber * properties.linespacing, 1);
                    robotONvariablecolors.Add(newvariablecolor);
                    return newvariablecolor;
                }
        }
        return null;
    }
    public void ResizeObjects()
    {
        // Resize game objects
        // Each game object needs to know its line number I think. Alternatively, the line number can be derived based on its position.
        foreach (GameObject printer in prints)
        {
            printer.transform.position = new Vector3(stateLib.LEFT_CODESCREEN_X_COORDINATE, properties.initialLineY - printer.GetComponent<printer>().Index * properties.linespacing, 1);
        }
        foreach (GameObject warp in roboBUGwarps)
        {
            warp.transform.position = new Vector3(stateLib.LEFT_CODESCREEN_X_COORDINATE, properties.initialLineY - warp.GetComponent<warper>().Index * properties.linespacing, 1);

        }

        List<GameObject> allComments = new List<GameObject>();
        allComments.AddRange(robotONcorrectComments);
        allComments.AddRange(robotONincorrectComments);
        allComments.AddRange(robotONcorrectUncomments);
        allComments.AddRange(robotONincorrectUncomments);
        foreach (GameObject comment in allComments)
        {
            comment thisComment = comment.GetComponent<comment>();
            comment.transform.position = new Vector3(stateLib.LEFT_CODESCREEN_X_COORDINATE, properties.initialLineY + stateLib.TOOLBOX_Y_OFFSET - thisComment.Index * properties.linespacing, 1);

        }
        foreach (GameObject question in robotONquestions)
        {
            question.transform.position = new Vector3(stateLib.LEFT_CODESCREEN_X_COORDINATE, properties.initialLineY + stateLib.TOOLBOX_Y_OFFSET - question.GetComponent<question>().Index * properties.linespacing, 1);
        }
        foreach (GameObject rename in robotONrenamers)
        {
            rename.transform.position = new Vector3(stateLib.LEFT_CODESCREEN_X_COORDINATE, properties.initialLineY + stateLib.TOOLBOX_Y_OFFSET - rename.GetComponent<rename>().Index * properties.linespacing, 1);
        }

        foreach (GameObject beacon in robotONbeacons)
        {
            beacon.transform.position = new Vector3(-9.95f, properties.initialLineY - beacon.GetComponent<beacon>().Index * properties.linespacing + properties.lineOffset + 0.4f, 1);
        }
        foreach (GameObject varcolor in robotONvariablecolors)
        {
            varcolor.transform.position = new Vector3(stateLib.LEFT_CODESCREEN_X_COORDINATE, properties.initialLineY + stateLib.TOOLBOX_Y_OFFSET - varcolor.GetComponent<VariableColor>().Index * properties.linespacing, 1);
        }
    }
    void SetTitle()
    {
        TextMesh title = GameObject.Find("Description").GetComponent<TextMesh>();
        foreach (XmlNode codenode in GlobalState.level.CodeNodes)
        {
            if (codenode.Name == stringLib.NODE_NAME_DESCRIPTION)
            {
                title.text = codenode.InnerText;
            }
        }
    }
    public void SaveGame()
    {
        levels.Clear();
        passed.Clear();
        string lfile = GlobalState.GameMode + "leveldata" + filepath + "levels.txt";

        StreamReader sr = File.OpenText(lfile);
        string line;
        while ((line = sr.ReadLine()) != null)
        {
            string[] data = line.Split(' ');
            levels.Add(data[0]);
            passed.Add(data[1]);
        }
        sr.Close();
        passed[levels.IndexOf(GlobalState.CurrentONLevel)] = "1";
        StreamWriter sw = File.CreateText(GlobalState.GameMode + "leveldata" + filepath + "levels.txt");
        for (int i = 0; i < levels.Count; i++)
        {
            sw.WriteLine(levels[i] + " " + passed[i]);
        }
        sw.Close();
    } 
}

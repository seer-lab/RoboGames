using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO; 
using UnityEngine;
using System.Xml;

/// <summary>
/// Maintains dynamic data used by Level Generator and facilitates in "managing" 
/// creating and destruction of Tool objects. The nature of LevelManager is that 
/// its operations are self contained and does not reference back to LevelGenerator.
/// </summary>
public class LevelManager
{
    //maintain data for saving the game. 
    public List<string> levels;
    public List<string> passed;
    private string filepath;

    //store the various Tool Objects in the Level. 
    //public GameObject levelBug;

    public List<GameObject> robotONrenamers;
    public List<GameObject> robotONvariablecolors;
    public List<GameObject> lines;
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
    public List<GameObject> roboBugPrint; 
    public List<GameObject> robotONbeacons;

    //Maintain an instance of properties for spacing. 
    CodeProperties properties; 


    public LevelManager(CodeProperties properties)
    {
        this.properties = properties;
        filepath = (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor) ? @"\" : @"/";
        levels = new List<string>();
        passed = new List<string>();
        SetTitle();

        lines = new List<GameObject>();
        bugs = new List<GameObject>();
        roboBUGwarps = new List<GameObject>();
        roboBUGcomments = new List<GameObject>();
        roboBugPrint = new List<GameObject>(); 
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

    /// <summary>
    /// Remove all GameObjects LevelManager has 
    /// </summary>
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
        foreach (GameObject incorrectComment in robotONincorrectComments)
        {
            GameObject.Destroy(incorrectComment);
        }
        foreach (GameObject warp in roboBUGwarps)
        {
            GameObject.Destroy(warp);
        }
        foreach(GameObject print in roboBugPrint)
        {
            GameObject.Destroy(print); 
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
        /*
        if (levelBug)
        {
            GameObject.Destroy(levelBug);
        }
        */

        
        // Reset local variables
       
        lines = new List<GameObject>();
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
        roboBugPrint = new List<GameObject>();
        roboBUGwarps = new List<GameObject>(); 
        
    }
    public GameObject CreateBug(XmlNode childnode, int lineNumber, int column = 0)
    {
        int bugsize = 1;
        int col = column; 
        //RoboBug Implementation 
        GameObject bugObject = Resources.Load<GameObject>("Prefabs/bug");
        GameObject levelBug = GameObject.Instantiate(bugObject, new Vector3(properties.bugXshift + col * properties.fontwidth + (bugsize - 1), properties.initialLineY - (lineNumber + 0.5f * (bugsize - 1) * properties.linespacing + 0.4f), 0f), bugObject.transform.rotation);
        //levelBug.transform.localScale += new Vector3(properties.bugscale * (bugsize - 1), properties.bugscale * (bugsize - 1), 0);
        //Debug.Log(properties.bugXshif + col * properties.fontwidth);
        levelBug.transform.position = new Vector3(GlobalState.StringLib.LEFT_CODESCREEN_X_COORDINATE+0.5f + col * properties.fontwidth, properties.initialLineY - (lineNumber)*0.99f, 0);
        GenericBug propertyHandler = levelBug.GetComponent<GenericBug>();
        propertyHandler.Index = lineNumber;
        GlobalState.level.TaskOnLine[lineNumber, stateLib.TOOL_CATCHER_OR_ACTIVATOR]++;
        bugs.Add(levelBug);
        GlobalState.level.Tasks[0]++;
        //numberOfBugsRemaining++;
        return levelBug;
    }

    public GameObject CreateHint(XmlNode childNode, int lineNumber){

        //@TODO Change the hardcoded value 
        int hintsize = 1;
        int col = 20;

        //@TODO Change the prefab to somthing cooler
        GameObject hintObject = Resources.Load<GameObject>("Prefabs/bugHint");
        GameObject levelHint = GameObject.Instantiate(hintObject, new Vector3(properties.bugXshift + col * properties.fontwidth + (hintsize - 1), properties.initialLineY - (lineNumber + 0.5f * (hintsize - 1) * properties.linespacing + 0.4f), 0f), hintObject.transform.rotation);
    
        levelHint.transform.position = new Vector3(GlobalState.StringLib.LEFT_CODESCREEN_X_COORDINATE+0.5f + col * properties.fontwidth, properties.initialLineY - (lineNumber)*0.99f, 0);
        HintCollider hintCollider = levelHint.GetComponent<HintCollider>();
        levelHint.GetComponent<HintCollider>().outmessage = childNode.Attributes["hint"].Value;
        return levelHint;
    }


    /// <summary>
    /// Creates a gameobject of the appropriate type using an XML node. 
    /// </summary>
    /// <param name="childnode">XML node being parsed</param>
    /// <param name="lineNumber">Line number the Object is on in code</param>
    /// <returns>Corresponding child GameObject, if non identifiable will return null</returns>
    public GameObject CreateLevelObject(XmlNode childnode, int lineNumber)
    {
        if (childnode.NodeType != XmlNodeType.Element)
        {
            return null;
        }
        ToolFactory toolFactory;
        //Identify the kind of child node and use the correct factory for the creation of 
        //the tool. 
        switch (childnode.Name)
        {
            case stringLib.NODE_NAME_PRINT:
                {
                    toolFactory = new PrinterFactory(childnode, lineNumber);
                    GameObject newoutput = toolFactory.GetGameObject();
                    newoutput.transform.position = new Vector3(GlobalState.StringLib.LEFT_CODESCREEN_X_COORDINATE, properties.initialLineY - lineNumber, 1);
                    roboBugPrint.Add(newoutput);
                    return newoutput;
                }
            case stringLib.NODE_NAME_WARP:
                {
                    toolFactory = new WarperFactory(childnode, lineNumber);
                    GameObject newWarper = toolFactory.GetGameObject();
                    newWarper.transform.position = new Vector3(GlobalState.StringLib.LEFT_CODESCREEN_X_COORDINATE, properties.initialLineY + stateLib.TOOLBOX_Y_OFFSET - lineNumber * properties.linespacing, 1);
                    roboBUGwarps.Add(newWarper);
                    return newWarper;
                }
            case stringLib.NODE_NAME_BUG:
                {

                    return CreateBug(childnode, lineNumber);
                }
            case stringLib.NODE_NAME_COMMENT:
                {
                    toolFactory = new CommentFactory(childnode, lineNumber);
                    GameObject newcomment = toolFactory.GetGameObject();
                    newcomment.transform.position = new Vector3(GlobalState.StringLib.LEFT_CODESCREEN_X_COORDINATE, properties.initialLineY - lineNumber * properties.linespacing, 1);
                    switch (((CommentFactory)toolFactory).Entity)
                    {
                        case stateLib.ENTITY_TYPE_ROBOBUG_COMMENT:
                            roboBUGcomments.Add(newcomment);
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
                    newquestion.transform.position = new Vector3(GlobalState.StringLib.LEFT_CODESCREEN_X_COORDINATE, properties.initialLineY - lineNumber * properties.linespacing, 1);
                    robotONquestions.Add(newquestion);
                    return newquestion;
                }
            case stringLib.NODE_NAME_RENAME:
                {
                    toolFactory = new RenamerFactory(childnode, lineNumber);
                    GameObject newrename = toolFactory.GetGameObject();
                    newrename.transform.position = new Vector3(GlobalState.StringLib.LEFT_CODESCREEN_X_COORDINATE, properties.initialLineY + stateLib.TOOLBOX_Y_OFFSET - lineNumber * properties.linespacing, 1);
                    robotONrenamers.Add(newrename);
                    return newrename;
                }
            case stringLib.NODE_NAME_BREAKPOINT:
                {
                    toolFactory = new BreakpointFactory(childnode, lineNumber);
                    GameObject newbreakpoint = toolFactory.GetGameObject();
                    newbreakpoint.transform.position = new Vector3(GlobalState.StringLib.LEFT_CODESCREEN_X_COORDINATE, properties.initialLineY + stateLib.TOOLBOX_Y_OFFSET- lineNumber *properties.linespacing, 1);
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
                    newbeacon.transform.position = new Vector3(GlobalState.StringLib.LEFT_CODESCREEN_X_COORDINATE, properties.initialLineY - lineNumber * properties.linespacing + properties.lineOffset + 0.4f, 1);
                    robotONbeacons.Add(newbeacon);
                    return newbeacon;
                }
            case stringLib.NODE_NAME_VARIABLE_COLOR:
                {
                    toolFactory = new VariableColorFactory(childnode, lineNumber);
                    GameObject newvariablecolor = toolFactory.GetGameObject();
                    newvariablecolor.transform.position = new Vector3(GlobalState.StringLib.LEFT_CODESCREEN_X_COORDINATE, properties.initialLineY - lineNumber * properties.linespacing + properties.lineOffset + 0.4f, 1);
                    robotONvariablecolors.Add(newvariablecolor);
                    return newvariablecolor;
                }
        }
        return null;
    }
    /// <summary>
    /// Resize all gameobjects so they fit correctly on the display. 
    /// </summary>
    public void ResizeObjects()
    {
        // Resize game objects
        // Each game object needs to know its line number I think. Alternatively, the line number can be derived based on its position.
        
        List<GameObject> allComments = new List<GameObject>();
        allComments.AddRange(robotONcorrectComments);
        allComments.AddRange(robotONincorrectComments);
        allComments.AddRange(robotONcorrectUncomments);
        allComments.AddRange(robotONincorrectUncomments);
        allComments.AddRange(roboBUGcomments);
        foreach (GameObject comment in allComments)
        {
            comment thisComment = comment.GetComponent<comment>();
            comment.transform.position = new Vector3(GlobalState.StringLib.LEFT_CODESCREEN_X_COORDINATE, properties.initialLineY + stateLib.TOOLBOX_Y_OFFSET - thisComment.Index * properties.linespacing, 1);

        }
        foreach (GameObject question in robotONquestions)
        {
            question.transform.position = new Vector3(GlobalState.StringLib.LEFT_CODESCREEN_X_COORDINATE, properties.initialLineY + stateLib.TOOLBOX_Y_OFFSET - question.GetComponent<question>().Index * properties.linespacing, 1);
        }
        foreach (GameObject printer in roboBugPrint)
        {
            printer.transform.position = new Vector3(GlobalState.StringLib.LEFT_CODESCREEN_X_COORDINATE, properties.initialLineY +stateLib.TOOLBOX_Y_OFFSET - printer.GetComponent<printer>().Index * properties.linespacing, 1);
        }
        foreach (GameObject rename in robotONrenamers)
        {
            rename.transform.position = new Vector3(GlobalState.StringLib.LEFT_CODESCREEN_X_COORDINATE, properties.initialLineY + stateLib.TOOLBOX_Y_OFFSET - rename.GetComponent<rename>().Index * properties.linespacing, 1);
        }
        foreach (GameObject warp in roboBUGwarps)
        {
            warp.transform.position = new Vector3(GlobalState.StringLib.LEFT_CODESCREEN_X_COORDINATE, properties.initialLineY + stateLib.TOOLBOX_Y_OFFSET - warp.GetComponent<warper>().Index * properties.linespacing, 1);
        }
        foreach (GameObject beacon in robotONbeacons)
        {
            beacon.transform.position = new Vector3(GlobalState.StringLib.LEFT_CODESCREEN_X_COORDINATE, properties.initialLineY - beacon.GetComponent<beacon>().Index * properties.linespacing + properties.lineOffset + 0.4f, 1);
        }
        foreach(GameObject breakpoint in roboBUGbreakpoints){
            breakpoint.transform.position = new Vector3(GlobalState.StringLib.LEFT_CODESCREEN_X_COORDINATE, properties.initialLineY + stateLib.TOOLBOX_Y_OFFSET - breakpoint.GetComponent<Breakpoint>().Index*properties.linespacing, 1); 
        }
        foreach (GameObject varcolor in robotONvariablecolors)
        {
            varcolor.transform.position = new Vector3(GlobalState.StringLib.LEFT_CODESCREEN_X_COORDINATE, properties.initialLineY + stateLib.TOOLBOX_Y_OFFSET - varcolor.GetComponent<VariableColor>().Index * properties.linespacing, 1);
        }
    }
    //Update the Title object of the level. 
    public void SetTitle()
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
    /// <summary>
    /// Save the game state to a file. 
    /// </summary>
    public void SaveGame()
    {
        levels.Clear();
        passed.Clear();
        string lfile = Application.streamingAssetsPath +"/" + GlobalState.GameMode + "leveldata" + filepath + "levels.txt";
        Debug.Log("LevelManager.cs SaveGame() path: " + lfile);
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
        StreamWriter sw = File.CreateText(Application.streamingAssetsPath+"/"+GlobalState.GameMode + "leveldata" + filepath + "levels.txt");
        for (int i = 0; i < levels.Count; i++)
        {
            sw.WriteLine(levels[i] + " " + passed[i]);
        }
        sw.Close();
    } 
}

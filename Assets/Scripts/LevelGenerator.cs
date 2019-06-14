using UnityEngine;
using UnityEngine.UI; 
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Text;
using System.Xml;
using UnityEngine.SceneManagement; 
using System.Text.RegularExpressions;
using System;

/// <summary>
/// Responsible for Generating and maintaining Code on-screen and the 
/// properties arround them. Based on GlobalStates level. 
/// </summary>
public partial class LevelGenerator : MonoBehaviour {

	public int numberOfBugsRemaining = 0;
	public int renamegroupidCounter = 0;

	// Lines of code stored in an array. innerXmlLines is the colorized text from NodeToColorString(), outerXmlLnes is the line with the tags.
	public string[] lineNumbers;

	// Stores the icons for each tool.
	public GameObject[] toolIcons = new GameObject[stateLib.NUMBER_OF_TOOLS];

	// Stores the level text, the lines of code the player sees.
	public GameObject leveltext;

    public GameObject lineobject;
	GameObject hero;
    AudioClip newTool, warped; 
	public Sprite whiteCodescreen;
	public Sprite blackCodescreen;

    // Reference to SelectedTool object. When ProvisionToolsFromXml() is called, 
    //tools are provisioned and then passed to SelectedTool object.
	public GameObject selectedtool;
	public GameObject toolprompt;
    private Output output; 
    private SidebarController sidebar;
    private BackgroundController background; 

	// Player has been notified of less than 30 seconds remaining on the clock.
	private bool winning;
	private bool storedDefaultPlayArea = false;
	private bool initialresize = false;

    CodeProperties properties; 

    public LevelManager manager; 

	// Use this for initialization
	private void Start() { 
        hero = GameObject.Find("Hero");
        newTool = Resources.Load<AudioClip>("Sound/Triggers/NewTool"); 
        warped = Resources.Load<AudioClip>("Sound/Triggers/warp"); 
        toolprompt = hero.transform.GetChild(0).gameObject;
        properties = new CodeProperties(); 
		GlobalState.GameState 					 = stateLib.GAMESTATE_IN_GAME;
        GlobalState.level.Tasks = new int[5];
        GlobalState.level.CompletedTasks = new int[5]; 
		for (int i = 0; i < 5; i++) {
            GlobalState.level.Tasks[i] = 0;
            GlobalState.level.CompletedTasks[i] = 0;
		}
		winning = false;
        manager = new LevelManager(properties);
        output = GameObject.Find("OutputCanvas").transform.GetChild(0).gameObject.GetComponent<Output>();
        sidebar = GameObject.Find("Sidebar").GetComponent<SidebarController>();
        background = GameObject.Find("BackgroundCanvas").GetComponent<BackgroundController>();
        GlobalState.correctLine = new string[stateLib.NUMBER_OF_TOOLS];
        GlobalState.obstacleLine= new string[stateLib.NUMBER_OF_OBSTACLE];
        BuildLevel(); 

	}
    /// <summary>
    /// Essentially Generates the Level Visually.
    /// Uses data from the GlobalState Level. 
    /// </summary>
    public void BuildLevel(bool warp = false)
    {
        if (warp) GetComponent<AudioSource>().PlayOneShot(warped); 
        manager.SetTitle();
        ResetLevel(warp);
        CreateLevelLines(GlobalState.level.LineCount);
        PlaceObjects(GlobalState.level.LevelNode);
        ProvisionToolsFromXml();
        
        // Resize the boundaries of the level to correspond with how many lines we have
        if (leveltext.GetComponent<TextMesh>().fontSize == stateLib.TEXT_SIZE_VERY_LARGE)
        {
            this.transform.position -= new Vector3(0, GlobalState.level.LineCount * properties.linespacing / 2, 0);
            this.transform.position += new Vector3(2.2f, 0, 0);
            this.transform.localScale += new Vector3(2, properties.levelLineRatio * GlobalState.level.LineCount, 0);
        }
        else
        {
            this.transform.position -= new Vector3(0, GlobalState.level.LineCount * properties.linespacing / 2, 0);
            this.transform.localScale += new Vector3(0.1f, properties.levelLineRatio * GlobalState.level.LineCount, 0);
        }
        DrawInnerXmlLinesToScreen();
        if (!initialresize)
        {
            // Make the text large in size for first run.
            initialresize = true;
            TransformTextSize(leveltext.GetComponent<TextMesh>().fontSize);
        }

    }

    /// <summary>
    /// Move the player to the correct position after Warping. 
    /// </summary>
    /// <param name="warpToLine">Line the player Warps To</param>
    public void WarpPlayer(string warpToLine)
    {
        if (warpToLine == null)
            warpToLine = "0";
        hero.transform.position = (warpToLine != "") ? new Vector3(-8, properties.initialLineY - (int.Parse(warpToLine) - 1) * properties.linespacing, 1) : hero.transform.position;
    }

    /// <summary>
    /// Create the grey level line objects between each line of code
    /// </summary>
    /// <param name="linecount">Numer of Lines</param>
    public void CreateLevelLines(int linecount) {
        // Create the grey line objects for each line.
		lineNumbers = new string[linecount];
		for (int i = 0; i < linecount; i++) {
			float fTransform = properties.initialLineY - i * properties.linespacing + properties.lineOffset;
			GameObject newline = (GameObject)Instantiate(lineobject, new Vector3(properties.initialLineX, fTransform, 1.1f), transform.rotation);
			newline.transform.localScale += new Vector3(0.35f, 0, 0);
			if (leveltext.GetComponent<TextMesh>().fontSize == stateLib.TEXT_SIZE_VERY_LARGE) {
				newline.transform.position += new Vector3(2.4f, 0, 0);
				newline.transform.localScale += new Vector3(0.8f, 0, 0);
			}
			manager.lines.Add(newline);
		}
	}


    /// <summary>
    /// Updates the code the player sees on the screen. Also adds the line
    /// numbers to the code
    /// </summary>
    /// <param name="bRedrawLineNumbers"></param>
    public void DrawInnerXmlLinesToScreen(bool bRedrawLineNumbers = true) {
		string drawCode = "";
		for (int x = 1 ; x < GlobalState.level.Code.GetLength(0) + 1; x++) {
			//draw the line number next to the text
			if (bRedrawLineNumbers) {
				// Color the number next to the line depending on the tasks on the line.
				
				string lineNumber = (x).ToString();

				lineNumbers[x-1] = lineNumber;
			}
            string lineN = lineNumbers[x-1];   
            //string lineN = lineNumbers[x-1];
			drawCode += lineN + "\t" + GlobalState.level.Code[x-1];
			drawCode += "\n";
		}
		//print("Drawcode is: " + drawCode);
        
		leveltext.GetComponent<TextMesh>().text = drawCode;
	}

    void HandleTutorialPrompts(XmlNode node, int line){
        DemoBotControl controller = hero.GetComponent<DemoBotControl>(); 
        string text = node.InnerText; 
        ActionFactory factory; 
        if (text.Contains("@")){
            factory = new DialogFactory(node, line); 
            if (controller.callstack == null) controller.callstack = new List<Action>(); 
            controller.callstack.AddRange(factory.GetActions());
        }
        if (text.Contains("!!!")){
            factory = new FireFactory(node, line); 
            controller.callstack.AddRange(factory.GetActions()); 
        }
        DrawInnerXmlLinesToScreen(); 
    }

    /// <summary>
    ///  Read through levelnode XML and create the interactable game objects.
    /// </summary>
    /// <param name="levelnode"></param>
    private void PlaceObjects(XmlNode levelnode) {
		foreach (XmlNode codenode in levelnode.ChildNodes) {
			if (codenode.Name != stringLib.NODE_NAME_CODE) {
				 continue;
			}
			int indexOf = 0;
			foreach(XmlNode childNode in codenode.ChildNodes)
			{
                
                if (childNode.InnerText.Contains("$bug"))
                {
                    string[] lines = childNode.InnerText.Split('\n');
                    int row = 0;
                    for (int i = 0; i < lines.Length; i++)
                    {
                        if (lines[i].Contains("$bug"))
                        {
                            row = indexOf + i;
                            
                        }
                    }
                    manager.CreateBug(childNode, row);
                    TextColoration color = new TextColoration();
                     GlobalState.level.Code[row] = color.ColorizeText(GlobalState.level.Code[row], GlobalState.level.Language);
                     DrawInnerXmlLinesToScreen();
                }
                if (childNode.InnerText.Contains("$O")){
                    manager.CreateObstacle(childNode, indexOf); 
                }
                Debug.Log("Is Demo: " + GlobalState.level.IsDemo); 
                if (GlobalState.level.IsDemo){
                    HandleTutorialPrompts(childNode, indexOf); 
                }
                manager.CreateLevelObject(childNode, indexOf);
                foreach (char c in childNode.OuterXml)
				{
					if (c == '\n') indexOf++;
				}
                
			}
            // indexOf = 0;
            // int tmp = 0;
            // foreach (XmlNode node in GlobalState.level.LevelNode){
            //     indexOf = 0;
            //     foreach(XmlNode childs in node.ChildNodes){
            //         try{
            //             if(childs.Attributes != null && childs.Attributes["hint"].Value != null){
            //                 tmp = indexOf +childs.InnerText.Split('\n').Length;
            //                 manager.CreateHint(childs,tmp);
            //             }
            //         }catch(Exception e){
            //             Debug.Log(e.Message);
            //         }
            //         indexOf++;
            //     }
            // }
            // These are counters to update the blocktext of each object
            int numberOfroboBUGcomments = 0;
            int numberOfrobotONcorrectComments = 0;
            int numberOfrobotONincorrectComments = 0;
            int numberOfrobotONcorrectUncomments = 0;
            int numberOfrobotONincorrectUncomments = 0;

            manager.ResizeObjects();
            GameObject thisObject;
            for (int i = 0; i < codenode.ChildNodes.Count; i++)
            {
                if (codenode.ChildNodes[i].Name == stringLib.NODE_NAME_COMMENT)
                {
                    switch (codenode.ChildNodes[i].Attributes[stringLib.XML_ATTRIBUTE_TYPE].Value)
                    {
                        case "robobug":
                            // RoboBUG comment
                            thisObject = manager.roboBUGcomments[numberOfroboBUGcomments];
                            TextColoration color = new TextColoration(); 
                            thisObject.GetComponent<BugComment>().blocktext = codenode.ChildNodes[i].InnerXml;
                            //Debug.Log(thisObject.GetComponent<BugComment>().blocktext);
                            string[] text = thisObject.GetComponent<BugComment>().blocktext.Split('\n');
                            GlobalState.level.Code[thisObject.GetComponent<BugComment>().Index] = "<color=#00ff00ff>/**/</color>" + color.ColorizeText(text[0], GlobalState.level.Language);

                            thisObject.transform.position = new Vector3(GlobalState.StringLib.LEFT_CODESCREEN_X_COORDINATE, properties.initialLineY + stateLib.TOOLBOX_Y_OFFSET - (thisObject.GetComponent<comment>().Index)*0.99f, 0f);
                            //Removed; using sprites instead:
                            numberOfroboBUGcomments++;
                            break;
                        case "description":
                            if (codenode.ChildNodes[i].Attributes[stringLib.XML_ATTRIBUTE_CORRECT].Value == "true")
                            {
                                // Correct Comment
                                thisObject = manager.robotONcorrectComments[numberOfrobotONcorrectComments];
                                thisObject.GetComponent<comment>().blocktext = codenode.ChildNodes[i].InnerText;
                                thisObject.GetComponent<comment>().size = thisObject.GetComponent<comment>().blocktext.Split('\n').Length;
                                // Colorize all multi-comment line numbers green
                                for (int j = 1; j < thisObject.GetComponent<comment>().size; j++)
                                {
                                    GlobalState.level.TaskOnLine[thisObject.GetComponent<comment>().Index + j, stateLib.TOOL_COMMENTER]++;
                                }
                                // Resize the hitbox for this comment to cover all lines (if multi-line comment)
                                thisObject.transform.position = new Vector3(GlobalState.StringLib.LEFT_CODESCREEN_X_COORDINATE, properties.initialLineY + stateLib.TOOLBOX_Y_OFFSET - (thisObject.GetComponent<comment>().Index) * properties.linespacing, 0f);

                                float yPos = (properties.textscale * (thisObject.GetComponent<comment>().size - 1) > 0) ? properties.textscale * (thisObject.GetComponent<comment>().size - 1) : 1.0f;
                                //Removed; using sprites instead:						
                                numberOfrobotONcorrectComments++;
                            }
                            else if (codenode.ChildNodes[i].Attributes[stringLib.XML_ATTRIBUTE_CORRECT].Value == "false")
                            {
                                // Incorrect comment
                                thisObject = manager.robotONincorrectComments[numberOfrobotONincorrectComments];
                                thisObject.GetComponent<comment>().blocktext = codenode.ChildNodes[i].InnerText;

                                thisObject.GetComponent<comment>().size = thisObject.GetComponent<comment>().blocktext.Split('\n').Length;

                                // Colorize all multi-comment line numbers green
                                for (int j = 1; j < thisObject.GetComponent<comment>().size; j++)
                                {
                                    GlobalState.level.TaskOnLine[thisObject.GetComponent<comment>().Index + j, stateLib.TOOL_COMMENTER]++;

                                }
                                // Resize the hitbox for this comment to cover all lines (if multi-line comment)
                                float yPos = (properties.textscale * (thisObject.GetComponent<comment>().size - 1) > 0) ? properties.textscale * (thisObject.GetComponent<comment>().size - 1) : 1.0f;
                                thisObject.transform.position = new Vector3(GlobalState.StringLib.LEFT_CODESCREEN_X_COORDINATE, properties.initialLineY + stateLib.TOOLBOX_Y_OFFSET - (thisObject.GetComponent<comment>().Index) * properties.linespacing, 0f);

                                numberOfrobotONincorrectComments++;
                            }
                            break;
                        case "code":
                            if (codenode.ChildNodes[i].Attributes[stringLib.XML_ATTRIBUTE_CORRECT].Value == "true")
                            {
                                // Correct Uncomment
                                thisObject = manager.robotONcorrectUncomments[numberOfrobotONcorrectUncomments];
                                thisObject.GetComponent<comment>().blocktext = codenode.ChildNodes[i].InnerText;
           
                                thisObject.GetComponent<comment>().size = thisObject.GetComponent<comment>().blocktext.Split('\n').Length;
                                // Colorize all multi-comment line numbers red
                                for (int j = 1; j < thisObject.GetComponent<comment>().size; j++)
                                {
                                    GlobalState.level.TaskOnLine[thisObject.GetComponent<comment>().Index + j, stateLib.TOOL_UNCOMMENTER]++;

                                }
                                // Resize the hitbox for this comment to cover all lines (if multi-line comment)
                                float yPos = (properties.textscale * (thisObject.GetComponent<comment>().size - 1) > 0) ? properties.textscale * (thisObject.GetComponent<comment>().size - 1) : 1.0f;
                                thisObject.transform.position = new Vector3(GlobalState.StringLib.LEFT_CODESCREEN_X_COORDINATE, properties.initialLineY + stateLib.TOOLBOX_Y_OFFSET - (thisObject.GetComponent<comment>().Index) * properties.linespacing, 0f);

                                //Removed; using sprites instead:
                                //thisObject.transform.localScale = new Vector3(thisObject.transform.localScale.x, yPos, thisObject.transform.localScale.z);
                                numberOfrobotONcorrectUncomments++;
                            }
                            else if (codenode.ChildNodes[i].Attributes[stringLib.XML_ATTRIBUTE_CORRECT].Value == "false")
                            {
                                // Incorrect Uncomment
                                thisObject = manager.robotONincorrectUncomments[numberOfrobotONincorrectUncomments];
                                thisObject.GetComponent<comment>().blocktext = codenode.ChildNodes[i].InnerText;
                                thisObject.GetComponent<comment>().size = thisObject.GetComponent<comment>().blocktext.Split('\n').Length;
                                // Colorize all multi-comment line numbers red
                                for (int j = 1; j < thisObject.GetComponent<comment>().size; j++)
                                {
                                    GlobalState.level.TaskOnLine[thisObject.GetComponent<comment>().Index + j, stateLib.TOOL_UNCOMMENTER]++;
                                }
                                // Resize the hitbox for this comment to cover all lines (if multi-line comment)
                                float yPos = (properties.textscale * (thisObject.GetComponent<comment>().size - 1) > 0) ? properties.textscale * (thisObject.GetComponent<comment>().size - 1) : 1.0f;
                                thisObject.transform.position = new Vector3(GlobalState.StringLib.LEFT_CODESCREEN_X_COORDINATE, properties.initialLineY + stateLib.TOOLBOX_Y_OFFSET - (thisObject.GetComponent<comment>().Index) * properties.linespacing, 0f);

                                numberOfrobotONincorrectUncomments++;
                            }
                            break;
                        default:
                            break;
                    }
                }
            }


			// Pair Incorrect Comments to their corresponding correct comments --[
			foreach (GameObject incorrectComment in manager.robotONincorrectComments) {
				foreach (GameObject correctComment in manager.robotONcorrectComments) {
					if (incorrectComment.GetComponent<comment>().groupid == correctComment.GetComponent<comment>().groupid) {
						incorrectComment.GetComponent<comment>().CorrectCommentObject = correctComment;
						break;
					}
				}
			}


			foreach (GameObject incorrectUncomment in manager.robotONincorrectUncomments) {
				foreach (GameObject correctUncomment in manager.robotONcorrectUncomments) {
					if (incorrectUncomment.GetComponent<comment>().groupid == correctUncomment.GetComponent<comment>().groupid) {
						incorrectUncomment.GetComponent<comment>().CorrectCommentObject = correctUncomment;
						break;
					}
				}
			}
            DrawInnerXmlLinesToScreen(); 
		}
	}

    /// <summary>
    /// Read through levelnode XML and provision the tools for this level
    /// levelnode is typically the parent XML node in the XML document.
    /// </summary>
    /// <param name="nodelist"></param>
    public void ProvisionToolsFromXml()
    {
        foreach (XmlNode tool in GlobalState.level.NodeList)
        {
            // Set the tool count for each tool node --[
            int toolnum = 0;
            //Debug.Log("Working with node: " + tool.OuterXml);
            switch (tool.Attributes[stringLib.XML_ATTRIBUTE_NAME].Value)
            {
                case "catcher":
                    toolnum = stateLib.TOOL_CATCHER_OR_CONTROL_FLOW;
                    break;
                case "controlflow":
                    toolnum = stateLib.TOOL_CATCHER_OR_CONTROL_FLOW;
                    break;
                case "printer":
                    toolnum = stateLib.TOOL_PRINTER_OR_QUESTION;
                    break;
                case "breakpointer":
                    toolnum=stateLib.TOOL_UNCOMMENTER; 
                    break;
                case "checker":
                    toolnum = stateLib.TOOL_PRINTER_OR_QUESTION;
                    break;
                case "warper":
                    toolnum = stateLib.TOOL_WARPER_OR_RENAMER;
                    break;
                case "namer":
                    toolnum = stateLib.TOOL_WARPER_OR_RENAMER;
                    break;
                case "commenter":
                    toolnum = stateLib.TOOL_COMMENTER;
                    break;
                case "uncommenter":
                    toolnum = stateLib.TOOL_UNCOMMENTER;
                    break;
                case "hint":
                    toolnum = stateLib.TOOL_HINTER;
                    break;
                default:
                    break;
            }
            //Debug.Log("Adding Tool: " + toolnum);
            toolIcons[toolnum].GetComponent<Image>().enabled = bool.Parse(tool.Attributes[stringLib.XML_ATTRIBUTE_ENABLED].Value);
            toolIcons[toolnum].transform.GetChild(0).GetComponent<Text>().enabled = bool.Parse(tool.Attributes[stringLib.XML_ATTRIBUTE_ENABLED].Value);
            selectedtool.GetComponent<SelectedTool>().toolCounts[toolnum] = (tool.Attributes[stringLib.XML_ATTRIBUTE_COUNT].Value == "unlimited") ? 999 : int.Parse(tool.Attributes[stringLib.XML_ATTRIBUTE_COUNT].Value);
            // ]-- End of tool count for each tool node
        }
    }

    private void ResetLevel(bool warping)
    {
        manager.DestroyInstances(); 
        if (output != null) output.text.GetComponent<Text>().text = "";
        // Reset tool counts if not warping to this level
        if (!warping)
        {
            for (int i = 0; i < properties.totalNumberOfTools; i++)
            {
                toolIcons[i].GetComponent<Image>().enabled = false;
                toolIcons[i].transform.GetChild(0).GetComponent<Text>().enabled = false;
                toolIcons[i].GetComponent<Image>().color = new Color(0.3f, 0.3f, 0.3f);
                selectedtool.GetComponent<SelectedTool>().toolCounts[i] = 0;
                selectedtool.GetComponent<SelectedTool>().bonusTools[i] = 0;
                selectedtool.GetComponent<SelectedTool>().projectilecode = 0;
            }
        }

        // Reset bug count
        numberOfBugsRemaining = 0;

        // Reset counter for Renames / Variable Color
        renamegroupidCounter = 0;

        // Reset play area size
        if (!storedDefaultPlayArea)
        {
            storedDefaultPlayArea = true;
            properties.defaultPosition = this.transform.position;
            properties.defaultLocalScale = this.transform.localScale;
        }
        this.transform.position = properties.defaultPosition;
        this.transform.localScale = properties.defaultLocalScale;


        // Move player to default position
        //hero.transform.position = leveltext.transform.position;
    }


    //.................................>8.......................................
    //************************************************************************//
    // Method: public void TransformTextSize(int nTextSizeConst)
    // Description: Transform the play area to correspond to a new text size.
    //************************************************************************//
    public void TransformTextSize(int nTextSizeConst = -1) {
        int[] sizes = new int[]{stateLib.TEXT_SIZE_VERY_LARGE, stateLib.TEXT_SIZE_SMALL, stateLib.TEXT_SIZE_NORMAL, stateLib.TEXT_SIZE_LARGE};
        if (nTextSizeConst == -1){
            nTextSizeConst = leveltext.GetComponent<TextMesh>().fontSize;
        }
        nTextSizeConst = sizes[GlobalState.TextSize]; 
		//hero.transform.position = new Vector3(GlobalState.StringLib.LEFT_CODESCREEN_X_COORDINATE+ 0.5f, properties.initialLineY, hero.transform.position.z);
        hero.GetComponent<Rigidbody2D>().gravityScale = 1; 
        leveltext.transform.position = new Vector3(GlobalState.StringLib.LEFT_CODESCREEN_X_COORDINATE+0.5f, leveltext.transform.position.y, leveltext.transform.position.z);
		switch (nTextSizeConst) {
			case stateLib.TEXT_SIZE_SMALL:
			    leveltext.GetComponent<TextMesh>().fontSize = stateLib.TEXT_SIZE_NORMAL;
			    properties.levelLineRatio = 0.55f;
                properties.linespacing = 0.825f;
                properties.lineOffset	= -0.3f;
                properties.textscale = 1.75f;
			    break;
			case stateLib.TEXT_SIZE_NORMAL:
			leveltext.GetComponent<TextMesh>().fontSize = stateLib.TEXT_SIZE_LARGE;
                properties.levelLineRatio = 0.55f * (float)stateLib.TEXT_SIZE_LARGE / (float)stateLib.TEXT_SIZE_NORMAL;
                properties.linespacing = 0.825f * (float)stateLib.TEXT_SIZE_LARGE / (float)stateLib.TEXT_SIZE_NORMAL;
                properties.lineOffset	= -0.3f * (float)stateLib.TEXT_SIZE_LARGE / (float)stateLib.TEXT_SIZE_NORMAL;
                properties.textscale = 1.75f * (float)stateLib.TEXT_SIZE_LARGE / (float)stateLib.TEXT_SIZE_NORMAL;
			    break;
			case stateLib.TEXT_SIZE_LARGE:
			    leveltext.GetComponent<TextMesh>().fontSize = stateLib.TEXT_SIZE_VERY_LARGE;
                properties.levelLineRatio = 0.55f * (float)stateLib.TEXT_SIZE_VERY_LARGE / (float)stateLib.TEXT_SIZE_LARGE * (float)stateLib.TEXT_SIZE_LARGE / (float)stateLib.TEXT_SIZE_NORMAL;
                properties.linespacing = 0.825f * (float)stateLib.TEXT_SIZE_VERY_LARGE / (float)stateLib.TEXT_SIZE_LARGE * (float)stateLib.TEXT_SIZE_LARGE / (float)stateLib.TEXT_SIZE_NORMAL;
                properties.lineOffset	= -0.3f * (float)stateLib.TEXT_SIZE_VERY_LARGE / (float)stateLib.TEXT_SIZE_LARGE * (float)stateLib.TEXT_SIZE_LARGE / (float)stateLib.TEXT_SIZE_NORMAL;
                properties.textscale = 1.75f * (float)stateLib.TEXT_SIZE_VERY_LARGE / (float)stateLib.TEXT_SIZE_LARGE * (float)stateLib.TEXT_SIZE_LARGE / (float)stateLib.TEXT_SIZE_NORMAL;
			    break;
			case stateLib.TEXT_SIZE_VERY_LARGE:
			    leveltext.GetComponent<TextMesh>().fontSize = stateLib.TEXT_SIZE_SMALL;
                properties.levelLineRatio = 0.55f * (float)stateLib.TEXT_SIZE_SMALL / (float)stateLib.TEXT_SIZE_VERY_LARGE * (float)stateLib.TEXT_SIZE_VERY_LARGE / (float)stateLib.TEXT_SIZE_LARGE * (float)stateLib.TEXT_SIZE_LARGE / (float)stateLib.TEXT_SIZE_NORMAL;
                properties.linespacing = 0.825f * (float)stateLib.TEXT_SIZE_SMALL / (float)stateLib.TEXT_SIZE_VERY_LARGE * (float)stateLib.TEXT_SIZE_VERY_LARGE / (float)stateLib.TEXT_SIZE_LARGE * (float)stateLib.TEXT_SIZE_LARGE / (float)stateLib.TEXT_SIZE_NORMAL;
                properties.lineOffset	= -0.3f * (float)stateLib.TEXT_SIZE_SMALL / (float)stateLib.TEXT_SIZE_VERY_LARGE * (float)stateLib.TEXT_SIZE_VERY_LARGE / (float)stateLib.TEXT_SIZE_LARGE * (float)stateLib.TEXT_SIZE_LARGE / (float)stateLib.TEXT_SIZE_NORMAL;
                properties.textscale = 1.75f * (float)stateLib.TEXT_SIZE_SMALL / (float)stateLib.TEXT_SIZE_VERY_LARGE * (float)stateLib.TEXT_SIZE_VERY_LARGE / (float)stateLib.TEXT_SIZE_LARGE * (float)stateLib.TEXT_SIZE_LARGE / (float)stateLib.TEXT_SIZE_NORMAL;
			    break;
			default:
			    break;
		}
		this.transform.position = properties.defaultPosition;
		this.transform.localScale = properties.defaultLocalScale;
		this.transform.position -= new Vector3(0, GlobalState.level.LineCount * properties.linespacing / 2, 0);
		this.transform.localScale += new Vector3(0, properties.levelLineRatio * GlobalState.level.LineCount, 0);
		if (nTextSizeConst == stateLib.TEXT_SIZE_LARGE) {
			this.transform.position += new Vector3(2.2f, 0, 0);
			this.transform.localScale += new Vector3(2, 0, 0);
		}
		// Redraw lines --[
		foreach (GameObject line in manager.lines) {
			Destroy(line);
		}
		manager.lines.Clear();
		for (int i = 0; i < GlobalState.level.LineCount; i++) {
			float fTransform = properties.initialLineY - i * properties.linespacing + properties.lineOffset;
			GameObject newline =(GameObject)Instantiate(lineobject, new Vector3(properties.initialLineX, fTransform, 1.1f), transform.rotation);
			newline.transform.localScale += new Vector3(0.35f, 0, 0);
			if (nTextSizeConst == stateLib.TEXT_SIZE_LARGE) {
				// Transition from Large to Very Large. In Very Large, the lines need to be longer.
				newline.transform.position += new Vector3(2.4f, 0, 0);
				newline.transform.localScale += new Vector3(0.8f, 0, 0);
			}
			manager.lines.Add(newline);
		}
        // ]-- Redraw lines

        manager.ResizeObjects(); 

    }
    /// <summary>
    /// Toggle Code Elements Light
    /// </summary>
    public void ToggleLight()
    {
        this.GetComponent<SpriteRenderer>().sprite = whiteCodescreen;
        this.GetComponent<SpriteRenderer>().color = new Color(0.94f, 0.97f, 0.99f, 0.8f);
        GameObject.Find("Description").GetComponent<TextMesh>().color = Color.black;
        leveltext.GetComponent<TextMesh>().color = Color.black;
        foreach (GameObject line in manager.lines)
        {
            line.GetComponent<SpriteRenderer>().color = new Color(0.95f, 0.95f, 0.95f, 1);
        }
     
        foreach (GameObject renameObj in manager.robotONrenamers)
        {
            rename propertyHandler = renameObj.GetComponent<rename>();
            propertyHandler.innertext = propertyHandler.innertext.Replace(GlobalState.StringLib.node_color_rename, GlobalState.StringLib.node_color_rename_dark);
        }
        foreach (GameObject varcolorObj in manager.robotONvariablecolors)
        {
            VariableColor propertyHandler = varcolorObj.GetComponent<VariableColor>();
            propertyHandler.innertext = propertyHandler.innertext.Replace(GlobalState.StringLib.node_color_rename, GlobalState.StringLib.node_color_rename_dark);
        }
        foreach (GameObject questionObj in manager.robotONquestions)
        {
            question propertyHandler = questionObj.GetComponent<question>();
            propertyHandler.innertext = propertyHandler.innertext.Replace(GlobalState.StringLib.node_color_question, GlobalState.StringLib.node_color_question_dark);
        }

    }

    /// <summary>
    /// Toggle Code elements Dark
    /// </summary>
    public void ToggleDark() {
	 
		    this.GetComponent<SpriteRenderer>().sprite 				= blackCodescreen;
		    this.GetComponent<SpriteRenderer>().color 				= Color.black;
            GameObject.Find("Description").GetComponent<TextMesh>().color 					= Color.white;
		    leveltext.GetComponent<TextMesh>().color 				= Color.white;;
		    toolprompt.GetComponent<TextMesh>().color				= Color.white;
		    foreach (GameObject line in manager.lines) {
			    line.GetComponent<SpriteRenderer>().color 			= Color.white;
		    }
           
		    foreach (GameObject renameObj in manager.robotONrenamers) {
			    rename propertyHandler = renameObj.GetComponent<rename>();
			    propertyHandler.innertext = propertyHandler.innertext.Replace(GlobalState.StringLib.node_color_rename, GlobalState.StringLib.node_color_rename_light);
		    }
		    foreach (GameObject varcolorObj in manager.robotONvariablecolors) {
			    VariableColor propertyHandler = varcolorObj.GetComponent<VariableColor>();
			    propertyHandler.innertext = propertyHandler.innertext.Replace(GlobalState.StringLib.node_color_rename, GlobalState.StringLib.node_color_rename_light);
		    }
		    foreach (GameObject questionObj in manager.robotONquestions) {
			    question propertyHandler = questionObj.GetComponent<question>();
			    propertyHandler.innertext = propertyHandler.innertext.Replace(GlobalState.StringLib.node_color_question, GlobalState.StringLib.node_color_question_light);
		    }
        
	    DrawInnerXmlLinesToScreen();
    }

    //.................................>8.......................................
    public void floatingTextOnPlayer(Color color) {
        hero.transform.Find("NewTool").GetComponent<SpriteRenderer>().color = color; 
	    Animator anim = hero.transform.Find("NewTool").GetComponent<Animator>(); 
	    anim.SetTrigger("onNewTool");
        GetComponent<AudioSource>().PlayOneShot(newTool, 0.5f); 
        
    }

//.................................>8.......................................
}

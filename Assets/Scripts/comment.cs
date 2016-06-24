//**************************************************//
// Class Name: comment
// Class Description: Handler for uncom, baduncom, oncom, badcomment
// Methods:
// 		void Start()
//		void Update()
//		void OnTriggerEnter2D(Collider2D collidingObj)
// Author: Scott McLean
// Date Last Modified: 6/24/2016
//**************************************************//

using UnityEngine;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;

public class comment : MonoBehaviour {

	public bool isCommented;
    public int entityType = -1;
    public int groupid    = -1;
    public int[] tools = new int[stateLib.NUMBER_OF_TOOLS];
	public string oldtext   = "";
	public string blocktext = "";
    public string righttext	= "";
    public string errmsg    = "";
	public GameObject CodeObject;
	public GameObject CodescreenObject;
    public GameObject CorrectCommentObject;
    public GameObject SidebarObject;
    public GameObject ToolSelectorObject;

	private LevelGenerator lg;
	private bool doneUpdating = false;
    private const int ENTITY_TYPE_CORRECT_COMMENT       = 1;
    private const int ENTITY_TYPE_CORRECT_UNCOMMENT     = 2;
    private const int ENTITY_TYPE_INCORRECT_COMMENT     = 3;
    private const int ENTITY_TYPE_INCORRECT_UNCOMMENT   = 4;
    private const int ENTITY_TYPE_ROBOBUG_COMMENT       = 5;

    private bool resetting  = false;
	private bool toolgiven = false;
	private float resetTime = 0f;
	private float timeDelay = 30f;

	//.................................>8.......................................
	// Use this for initialization
	void Start() {
		lg = CodescreenObject.GetComponent<LevelGenerator>();
	}

	//.................................>8.......................................
	// Update is called once per frame
	void Update() {
        if (entityType == ENTITY_TYPE_INCORRECT_COMMENT) {
            UpdateIncorrectComment();
        }
        else if (entityType == ENTITY_TYPE_INCORRECT_UNCOMMENT) {
            UpdateIncorrectUncomment();
        }
        else if (entityType == ENTITY_TYPE_ROBOBUG_COMMENT) {
            UpdateRoboBUGComment();
        }
	}

	//.................................>8.......................................
	void OnTriggerEnter2D(Collider2D collidingObj) {
        if (entityType == ENTITY_TYPE_CORRECT_COMMENT) {
            TriggerCorrectComment(collidingObj);
        }
        else if (entityType == ENTITY_TYPE_CORRECT_UNCOMMENT) {
            TriggerCorrectUncomment(collidingObj);
        }
        else if (entityType == ENTITY_TYPE_INCORRECT_COMMENT) {
            TriggerIncorrectComment(collidingObj);
        }
        else if (entityType == ENTITY_TYPE_INCORRECT_UNCOMMENT) {
            TriggerIncorrectUncomment(collidingObj);
        }
        else if (entityType == ENTITY_TYPE_ROBOBUG_COMMENT) {
            TriggerRoboBUGComment(collidingObj);
        }
	}

    //.................................>8.......................................
    void TriggerCorrectComment(Collider2D collidingObj) {
        if (collidingObj.name == stringLib.PROJECTILE_COMMENT && !isCommented) {
            isCommented = true;
            Destroy(collidingObj.gameObject);
            GetComponent<AudioSource>().Play();
            lg.taskscompleted[3]++;
            CodeObject.GetComponent<TextMesh>().text = CodeObject.GetComponent<TextMesh>()
                                                                 .text
                                                                 .Replace(blocktext, stringLib.COMMENT_BLOCK_COLOR_TAG +
                                                                                     blocktext +
                                                                                     stringLib.COMMENT_CLOSE_COLOR_TAG);
        }
    }

	//.................................>8.......................................
    void TriggerCorrectUncomment(Collider2D collidingObj) {
        if (collidingObj.name == stringLib.PROJECTILE_DEBUG) {
            if (isCommented) {
                // lg.GameOver();
            }
            else {
                Destroy(collidingObj.gameObject);
                GetComponent<AudioSource>().Play();
                lg.taskscompleted[4]++;
                blocktext = blocktext.Substring(19, blocktext.Length - 29);
                print(blocktext);
                CodeObject.GetComponent<TextMesh>().text = CodeObject.GetComponent<TextMesh>()
                                                                     .text
                                                                     .Replace(stringLib.UNCOMMENT_COLOR_TAG +
                                                                              blocktext +
                                                                              stringLib.COMMENT_CLOSE_COLOR_TAG, lg.ColorizeKeywords(blocktext, true));
                // update the text
                // code.GetComponent<TextMesh>().text = lg.ColorizeKeywords(blocktext);
                isCommented = true;
            }
        }
    }

    //.................................>8.......................................
    void TriggerIncorrectComment(Collider2D collidingObj) {
        if (collidingObj.name == stringLib.PROJECTILE_COMMENT && !doneUpdating) {
			Destroy(collidingObj.gameObject);
			GetComponent<AudioSource>().Play();
			lg.isLosing = true;
		}
    }

    //.................................>8.......................................
    void TriggerIncorrectUncomment(Collider2D collidingObj) {
        if (collidingObj.name == stringLib.PROJECTILE_DEBUG && !doneUpdating) {
			Destroy(collidingObj.gameObject);
			GetComponent<AudioSource>().Play();
			lg.isLosing = true;
		}
    }

    //.................................>8.......................................
    void TriggerRoboBUGComment(Collider2D collidingObj) {
        if (collidingObj.name == stringLib.PROJECTILE_COMMENT) {
            printLogFile(stringLib.LOG_COMMENT_ON);
            Destroy(collidingObj.gameObject);
            GetComponent<AudioSource>().Play();
            CodeObject.GetComponent<TextMesh>().text = oldtext.Replace(blocktext, stringLib.COMMENT_BLOCK_COLOR_TAG +
                                                                                  blocktext.Replace("/**/","") +
                                                                                  stringLib.COMMENT_CLOSE_COLOR_TAG);
            SidebarObject.GetComponent<GUIText>().text = errmsg;
            resetTime = Time.time + timeDelay;
            resetting = true;

            if (!toolgiven) {
                toolgiven = true;
                for (int i = 0; i < stateLib.NUMBER_OF_TOOLS; i++) {
                    if (tools[i] > 0) {
                        ToolSelectorObject.GetComponent<SelectedTool>().notifyToolAcquisition();
                    }
                    ToolSelectorObject.GetComponent<SelectedTool>().toolCounts[i] += tools[i];
                }
            }
        }
    }

    //.................................>8.......................................
    void UpdateIncorrectComment() {
        // GameObject must exist
        if (CorrectCommentObject) {
            // Commented and badcomment is not done?
            if (CorrectCommentObject.GetComponent<comment>().isCommented && !doneUpdating) {
                // Colorize the TextMesh's text with this blocktext
                doneUpdating = true;
                CodeObject.GetComponent<TextMesh>().text = CodeObject.GetComponent<TextMesh>()
                                                           .text
                                                           .Replace(blocktext, stringLib.BAD_COMMENT_TEXT_COLOR_TAG +
                                                                               blocktext +
                                                                               stringLib.CLOSE_COLOR_TAG);
            }
        }
    }

    //.................................>8.......................................
    void UpdateIncorrectUncomment() {
        // GameObject must exist
		if (CorrectCommentObject) {
			// Commented and badcomment is not done?
			if (CorrectCommentObject.GetComponent<comment>().isCommented && !doneUpdating) {
				doneUpdating = true;
				// Find this object's text in the code and remove it.
				CodeObject.GetComponent<TextMesh>().text = CodeObject.GetComponent<TextMesh>()
														             .text
														             .Replace(blocktext, "");
			}
		}
    }

    //.................................>8.......................................
    void UpdateRoboBUGComment() {
        if (resetting) {
			if (CodeObject.GetComponent<TextMesh>().text != oldtext.Replace(blocktext, stringLib.COMMENT_BLOCK_COLOR_TAG +
																				       blocktext.Replace("/**/","") +
																				       stringLib.COMMENT_CLOSE_COLOR_TAG)) {
				resetting = false;
			}
			else if (Time.time > resetTime || Input.GetKeyDown(KeyCode.Return)) {
				resetting = false;
				SidebarObject.GetComponent<GUIText>().text = "";
				CodeObject.GetComponent<TextMesh>().text = oldtext;
			}
		}
    }

    //.................................>8.......................................
    void printLogFile(string sMessage)
    {
        int position = (int)((stateLib.GAMESETTING_INITIAL_LINE_Y - this.transform.position.y) / stateLib.GAMESETTING_LINE_SPACING);
        StreamWriter sw = new StreamWriter(stringLib.TOOL_LOGFILE, true);
        sMessage = sMessage + position.ToString() + ", " + Time.time.ToString();
        sw.WriteLine(sMessage);
        sw.Close();
    }

    //.................................>8.......................................

}

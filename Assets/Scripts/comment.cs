using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;
using System.Text.RegularExpressions;

/// <summary>
/// Comment is super class for all the various types comments, 
/// and now inherits from Tools. All children must inherit OnTriggerProtol 
/// and can optionally inherit UpdateProtocol. 
/// </summary>
public abstract class comment : Tools
{
    public bool isCommented;
    public string commentStyle;
    public int entityType = -1;
    public int groupid = -1;
    public int size = -1;

    public string oldtext = "";
    public string blocktext = "";
    public string errmsg = "";

    //public GameObject CodeObject;
    public GameObject CorrectCommentObject;

    protected Sprite descSpriteOff;
    protected Sprite descSpriteOn;
    protected Sprite codeSpriteOff;
    protected Sprite codeSpriteOn;
    protected bool isAnswering = false;

    public bool doneUpdating = false;

    protected bool resetting = false;

    protected float resetTime = 0f;
    protected float timeDelay = 30f;
    protected Animator anim;
    protected GameObject rightArrow, leftArrow;
    protected bool arrowShown = false;
    protected TextColoration textColoration;

    /// <summary>
    /// Checks the blocktext for artifacts from bugs and tutorial objects.
    /// </summary>
    public void CleanBlocktext()
    {
        if (blocktext.Contains("$bug"))
        {
            Regex ansRgx = new Regex(stringLib.BUG_REGEX);
            string answer = ansRgx.Match(blocktext).Value;
            blocktext = blocktext.Replace("$bug" + answer + "$", "");
        }
        if (blocktext.Contains("@"))
        {
            Regex paramRgx = new Regex(stringLib.DIALOG_REGEX);
            Match match = paramRgx.Match(blocktext);
            while (match.Success)
            {
                string value = match.Value;
                blocktext = blocktext.Replace("@" + value + "@", "");
                match = match.NextMatch();
            }
        }
        if (blocktext.Contains("!!!"))
        {
            blocktext = blocktext.Replace("!!!", "");
        }
        if (blocktext.Contains("???"))
        {
            blocktext = blocktext.Replace("???", "");
        }
        string[] text = blocktext.Split('\n');
        for (int i = 0; i < text.Length; i++)
        {
            GlobalState.level.Code[index + i] = text[i];
        }
    }
    public override void Initialize()
    {
        rightArrow = GameObject.Find("OutputCanvas").transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).gameObject;
        leftArrow = GameObject.Find("OutputCanvas").transform.GetChild(0).transform.GetChild(0).transform.GetChild(1).gameObject;
        string path = "Sprites/";
        anim = GetComponent<Animator>();
        descSpriteOff = Resources.LoadAll<Sprite>(path + "dComment")[2];
        descSpriteOn = Resources.LoadAll<Sprite>(path + "dComment")[0];
        codeSpriteOff = Resources.LoadAll<Sprite>(path + "cComment")[2];
        codeSpriteOn = Resources.LoadAll<Sprite>(path + "cComment")[0];
        if (entityType == stateLib.ENTITY_TYPE_CORRECT_COMMENT || entityType == stateLib.ENTITY_TYPE_INCORRECT_COMMENT)
        {
            if (isCommented) this.gameObject.GetComponent<SpriteRenderer>().sprite = descSpriteOn;
            else this.gameObject.GetComponent<SpriteRenderer>().sprite = descSpriteOff;
        }
        else if (entityType == stateLib.ENTITY_TYPE_ROBOBUG_COMMENT)
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = descSpriteOn;
        }
        else
        {
            if (isCommented) this.gameObject.GetComponent<SpriteRenderer>().sprite = codeSpriteOn;
            else this.gameObject.GetComponent<SpriteRenderer>().sprite = codeSpriteOff;
        }
        textColoration = new TextColoration();
    }


    // Update is called once per frame
    void Update()
    {
        if (GlobalState.GameMode == stringLib.GAME_MODE_ON &&
            ((entityType == stateLib.ENTITY_TYPE_CORRECT_COMMENT || entityType == stateLib.ENTITY_TYPE_INCORRECT_COMMENT)
            && hero.projectilecode == stateLib.TOOL_COMMENTER) ||
            ((entityType == stateLib.ENTITY_TYPE_CORRECT_UNCOMMENT || entityType == stateLib.ENTITY_TYPE_INCORRECT_UNCOMMENT)
            && hero.projectilecode == stateLib.TOOL_UNCOMMENTER)
            )
        {
            EmphasizeTool();
        }
        else if (GlobalState.GameMode == stringLib.GAME_MODE_BUG && hero.projectilecode == stateLib.TOOL_COMMENTER) EmphasizeTool();
        else DeEmphasizeTool();

        if (entityType == stateLib.ENTITY_TYPE_CORRECT_COMMENT || entityType == stateLib.ENTITY_TYPE_INCORRECT_COMMENT)
        {
            HandleInput();
        }
        UpdateProtocol();

    }

    /// <summary>
    /// Hide/show the arrows for selecting whether a comment is true or not.
    /// </summary>
    /// <param name="active">Should the arrows be shown</param>
    void ToggleArrows(bool active)
    {
        rightArrow.GetComponent<Image>().enabled = active;
        rightArrow.transform.GetChild(0).GetComponent<Text>().enabled = active;
        leftArrow.GetComponent<Image>().enabled = active;
        leftArrow.transform.GetChild(0).GetComponent<Text>().enabled = active;
        output.enter.GetComponent<Image>().enabled = !active;
        output.enter.transform.GetChild(0).GetComponent<Text>().enabled = !active;
    }

    /// <summary>
    /// Handles interaction with the dialog box when the player uses 
    /// the descriptive commenters.
    /// </summary>
    protected virtual void HandleInput()
    {
        if (isAnswering)
        {
            if (!arrowShown)
            {
                ToggleArrows(true);
                arrowShown = true;
                if (!GlobalState.level.IsDemo)
                {
                    rightArrow.GetComponent<Button>().onClick.AddListener(OnRightArrowClick);
                    leftArrow.GetComponent<Button>().onClick.AddListener(OnLeftArrowClick);

                }
            }
            if (!GlobalState.level.IsDemo)
            {
                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    OnLeftArrowClick();
                }
                else if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    OnRightArrowClick();
                }
            }
        }
        else if (arrowShown)
        {
            ToggleArrows(false);
            output.enter.transform.GetChild(0).GetComponent<Text>().text = "OK!";
            arrowShown = false;
            if (!GlobalState.level.IsDemo)
            {
                rightArrow.GetComponent<Button>().onClick.RemoveListener(OnRightArrowClick);
                leftArrow.GetComponent<Button>().onClick.RemoveListener(OnLeftArrowClick);
            }
        }
    }
    /// <summary>
    /// Default behaviors to be preformed after the dialog is done being used.
    /// </summary>
    protected void HandleClick()
    {
        isAnswering = false;
        Output.IsAnswering = false;
        output.Text.text = "";
    }
    /// <summary>
    /// This Function will be called when the Right arrow key is clicked or the 
    /// button is used.
    /// </summary>
    protected virtual void OnRightArrowClick() { }
    /// <summary>
    /// This Function will be called when the Left arrow key is clicked or the 
    /// button is used.
    /// </summary>
    protected virtual void OnLeftArrowClick() { }
    public virtual void UpdateProtocol() { }


    void OnTriggerEnter2D(Collider2D collidingObj)
    {

        OnTriggerProtocol(collidingObj);
    }
    protected abstract void OnTriggerProtocol(Collider2D collidingObj);


}

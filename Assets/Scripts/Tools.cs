using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Super class for all tools 
/// </summary>
public class Tools : MonoBehaviour
{
    protected int index = -1;
    protected string language = "";
    protected string displaytext = "";
    protected SidebarController sidebar;
    protected SelectedTool selectedTool;
    protected Output output;
    protected LevelGenerator lg;
    public int[] tools = new int[stateLib.NUMBER_OF_TOOLS];
    protected bool toolgiven = false; 

    protected AudioSource audioSource; 
    protected AudioClip correct, wrong; 
    protected hero2Controller hero; 
    bool select = true; 
    public int Index
    {
        get
        {
            return index; 
        }
        set
        {
            index = value;
        }
    }
    public string DisplayText
    {
        set
        {
            displaytext = value; 
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        lg = GameObject.Find("CodeScreen").GetComponent<LevelGenerator>();
        output = GameObject.Find("OutputCanvas").transform.GetChild(0).GetComponent<Output>();
        sidebar = GameObject.Find("Sidebar").GetComponent<SidebarController>();
        selectedTool = sidebar.gameObject.transform.GetChild(2).transform.Find("Sidebar Tool").gameObject.GetComponent<SelectedTool>();
        language = GlobalState.level.Language; 
        hero = GameObject.Find("Hero").GetComponent<hero2Controller>(); 
        audioSource = this.GetComponent<AudioSource>(); 
        correct = Resources.Load<AudioClip>("Sound/Triggers/correct"); 
        wrong = Resources.Load<AudioClip>("Sound/Triggers/wrong"); 
        Initialize(); 
    }
    /// <summary>
    /// Will be called at Start, use this for correct ordering of Instantiation. 
    /// </summary>
    public virtual void Initialize() { }

    protected void EmphasizeTool(){
        if (!select){
            StopAllCoroutines(); 
            StartCoroutine(FadeTool(true)); 
            select = true; 
        }
    }
    protected void DeEmphasizeTool(){
        if (select){
            StopAllCoroutines(); 
            StartCoroutine(FadeTool(false)); 
            select = false; 
        }
    }
    IEnumerator FadeTool(bool emphasize){
        float speed = 30f; 
        if (!emphasize){
            speed = -speed; 
        }
        SpriteRenderer canvas = GetComponent<SpriteRenderer>(); 
        float iter = 1f/speed; 
        while((canvas.color.a < 1 && emphasize) || (canvas.color.a > 0.6f && !emphasize)){
            canvas.color = new Color(canvas.color.r, canvas.color.g, canvas.color.b, canvas.color.a + iter) ; 
            yield return null; 
        }
    }

}

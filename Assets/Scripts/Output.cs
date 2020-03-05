//**************************************************//
// Class Name: Output
// Class Description: This script is a controller for the pop-up interface in both games.
// Methods:
// 		void Start()
//		void Update()
// Author: Michael Miljanovic
// Date Last Modified: 6/1/2016
//**************************************************//

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Output : MonoBehaviour
{
	public GameObject codescreen;

	public GameObject text, panel , enter;
	public string hint = "";
	private Animator anim;
    private string enterText; 
    public bool isComment = false; 
    public int narrator = 0; 
    int original = 0; 
    public static bool IsAnswering { get; set; }
    bool entered = false; 
    Sprite[] panels = new Sprite[8];
    DemoBotControl demoBot; 

    private void LoadPanels()
    {
        string path = "Sprites/panel-";
        for (int i = 0; i < panels.Length; i++)
        {
            panels[i] = Resources.Load<Sprite>(path + (i + 2).ToString());
        }
    }
    public void onEnter(){
        entered = true; 
    }
    public Text Text
    {
        get
        {
            return text.GetComponent<Text>();
        }

    }
    public void ToggleLight()
    {
        Text.color = Color.black;
        enter.transform.GetChild(0).GetComponent<Text>().color = Color.black;
        panel.GetComponent<Image>().sprite = panels[5]; 
    }
    public void ToggleDark()
    {
        Text.color = Color.white;
        enter.transform.GetChild(0).GetComponent<Text>().color = Color.black;
        panel.GetComponent<Image>().sprite = panels[3]; 
    }
	//.................................>8.......................................
	// Use this for initialization
	void Start() {
		anim = GetComponent<Animator>();
		anim.SetBool("Appearing", false);
		anim.SetBool("Hiding", false);
        LoadPanels();
        if (GlobalState.level.IsDemo) {
            enter.GetComponent<Image>().enabled = false; 
            enterText = enter.transform.GetChild(0).GetComponent<Text>().text;
            enter.transform.GetChild(0).GetComponent<Text>().text =""; 
            demoBot = GameObject.Find("Hero").GetComponent<DemoBotControl>(); 
        }
        IsAnswering = false; 
        if (GlobalState.level.IsDemo){
            narrator = 0; 
        }
        else if (GlobalState.Character == "Girl") narrator = 2; 
        else if (GlobalState.Character == "Boy") narrator = 1;
        else{
            narrator = Random.Range(1,3); 
        }
        original = narrator; 
            
	}
    public void PlayCharacterOutput(string newText){
        int value = 0; 
        if (GlobalState.Character == "Boy") value = 2; 
        else if (GlobalState.Character == "Girl") value = 1; 
        narrator = value; 
        anim.SetInteger("Character", value); 
        text.GetComponent<Text>().text = newText; 
    }

	//.................................>8.......................................
	// Update is called once per frame
	void Update() {
		bool isText = text.GetComponent<Text>().text != "";
		if (isText) {
			anim.SetBool("Appearing", true);
			anim.SetBool("Hiding", false);
            anim.SetInteger("Character", narrator); 
		}
		else if(!GlobalState.level.IsDemo) {
			anim.SetBool("Appearing", false);
			anim.SetBool("Hiding", true);
		}
		if ((Input.GetKeyDown(KeyCode.Return)|| entered || Input.GetKeyDown(KeyCode.KeypadEnter)) || GlobalState.GameState != stateLib.GAMESTATE_IN_GAME) {
			if (hint != "" && !IsAnswering){ //Some sloppy code for adding a second 'page' for the hint text
				text.GetComponent<Text>().text = hint;
				hint = "";
			}
			else{
				if (!GlobalState.level.IsDemo && !IsAnswering)
					text.GetComponent<Text>().text = "";
				narrator = original; 
				entered = false; 
			}
		}
        if (GlobalState.level.IsDemo){
            if (!isComment && demoBot.currentIndex >= 0 && demoBot.callstack[demoBot.currentIndex].Category == ActionType.Output){
                enter.GetComponent<Image>().enabled = true; 
                enter.transform.GetChild(0).GetComponent<Text>().text =enterText; 
            }
            else {
                enter.GetComponent<Image>().enabled = false; 
                enter.transform.GetChild(0).GetComponent<Text>().text =""; 
            }
        }
        
	}

	//.................................>8.......................................
}

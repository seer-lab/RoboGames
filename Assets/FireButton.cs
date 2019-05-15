using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class FireButton : MonoBehaviour
{
    Color[] colours; 
    string[] icons; 
    hero2Controller hero; 
    public GameObject toolObject; 
    private SelectedTool tool; 
    private bool isFiring; 
    public bool IsFiring{get{
        return isFiring; 
    }}
    int code = -1; 
    void Start(){
        hero = GameObject.Find("Hero").GetComponent<hero2Controller>(); 
        tool = toolObject.GetComponent<SelectedTool>(); 
        colours = new Color[]{Color.white, Color.yellow, Color.magenta, Color.green,
        Color.cyan, Color.blue, Color.blue};
        icons = new string[]{"bugcatcher", "activator","warp","comment", "breakpoint","help", "help"};
        code = tool.projectilecode; 
        UpdateLook();
    }
    void UpdateLook(){
        GetComponent<Image>().color = colours[code];
        GameObject.Find("Image").GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/" + icons[code]);
    }
    void Update(){
        if (code != tool.projectilecode && tool.projectilecode >= 0){
            code = tool.projectilecode; 
            UpdateLook();
        }
        if (Input.GetKeyDown("left ctrl")  || Input.GetKeyDown("right ctrl"))
            GetComponent<Animator>().SetTrigger("Fire");

        if (GlobalState.GameState == stateLib.GAMESTATE_IN_GAME){
            this.transform.parent.GetComponent<Canvas>().enabled = true;
        }
        else if (GlobalState.GameState != stateLib.GAMESTATE_IN_GAME) transform.parent.GetComponent<Canvas>().enabled = false; 
    }
    public void onClick(){
        if (Input.GetKey(KeyCode.KeypadEnter) || Input.GetKey(KeyCode.Return))
            return; 
        hero.ThrowTool(); 
        GetComponent<Animator>().SetTrigger("Fire");
        StartCoroutine(onFire());
    }
    IEnumerator onFire(){
        isFiring = true; 
        yield return new WaitForSecondsRealtime(0.1f); 
        isFiring = false; 
    }
}

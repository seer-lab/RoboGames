using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI; 

public class FireButton : MonoBehaviour
{
    hero2Controller hero; 
    public GameObject toolObject; 
    private SelectedTool tool; 
    private bool isFiring; 
    public bool mouseOver = false; 
    public void UpdateMouse(bool isHover){
        mouseOver = isHover; 
        Debug.Log(mouseOver); 
    }
    public bool IsFiring{get{
        return isFiring; 
    }}
    int code = -1; 
    void Start(){
        hero = GameObject.Find("Hero").GetComponent<hero2Controller>(); 
        tool = toolObject.GetComponent<SelectedTool>(); 
        code = tool.projectilecode; 
        UpdateLook();
        if (GlobalState.HideToolTips || SystemInfo.operatingSystem.Contains("Android") || SystemInfo.operatingSystem.Contains("iOS")){
            transform.parent.GetChild(1).GetComponent<Text>().text =""; 
        }
        else if (SystemInfo.operatingSystem.Contains("Mac")){
            transform.parent.GetChild(1).GetComponent<Text>().text = "Control"; 
        }
    }
    void UpdateLook(){
        GetComponent<Image>().color = GlobalState.StringLib.COLORS[code];
        transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/icons/" + ((GlobalState.GameMode == stringLib.GAME_MODE_ON) ? GlobalState.StringLib.onIcons[code] : GlobalState.StringLib.bugIcons[code]));
    }
    void Update(){
        if (code != tool.projectilecode && tool.projectilecode >= 0){
            code = tool.projectilecode; 
            UpdateLook();
        }
        if (GlobalState.GameState == stateLib.GAMESTATE_IN_GAME){
            this.transform.parent.GetComponent<Canvas>().enabled = true;
        }
        else if (GlobalState.GameState != stateLib.GAMESTATE_IN_GAME) transform.parent.GetComponent<Canvas>().enabled = false; 
    }
    public void onClick(){
        if (Input.GetKey(KeyCode.KeypadEnter) || Input.GetKey(KeyCode.Return))
            return; 
        //GameObject.Find("Hero").GetComponent<hero2Controller>().StopCoroutine("MoveToPosition"); 
        if (!GlobalState.level.IsDemo)
            hero.ThrowTool(); 
        
    }
    public void Fire(){
        GetComponent<Animator>().SetTrigger("Fire");
        StartCoroutine(onFire());
    }
    IEnumerator onFire(){
        isFiring = true; 
        yield return new WaitForSecondsRealtime(0.1f); 
        isFiring = false; 
    }
}

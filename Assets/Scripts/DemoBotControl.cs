using System.IO;
using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class DemoBotControl : MonoBehaviour
{
    public List<Action> callstack;
    private hero2Controller controller;
    private Output output;
    int indexOfAction = 0;
    public int currentIndex = -1;
    bool entered = false;
    float timeDelay;
    float enterDelay;
    bool autoEnabled = true; 
    //Next Action is true when the game should auto complete, false when after the 
    //action the game should wait for the player to add input.
    bool nextAction = false; 
    GameObject hand; 
    HandControl handControl; 

    // Start is called before the first frame update
    void Start()
    {
        controller = this.GetComponent<hero2Controller>();
        callstack = new List<Action>();
        timeDelay = 0.5f;
        output = GameObject.Find("OutputCanvas").transform.GetChild(0).GetComponent<Output>();
        StartCoroutine(AutoPlay()); 
        hand = Instantiate(Resources.Load<GameObject>("Prefabs/hand")); 
        handControl = hand.GetComponent<HandControl>(); 
    }
    IEnumerator AutoPlay()
    {
        yield return new WaitForSecondsRealtime(timeDelay); 
        while (autoEnabled)
        {
            if (nextAction){
                nextAction = false; 
                entered = true; 
            }
            yield return new WaitForSecondsRealtime(timeDelay);
        }
    }
    void UpdateDelay(Action action)
    {
        if (action.Category == ActionType.Dialog) timeDelay = 0f;
        else if (action.Category == ActionType.SwitchTool) timeDelay = 0.5f;
        else if (action.Category == ActionType.Throw) timeDelay = 2f;
        Debug.Log(timeDelay); 
    }
    public void InsertOptionAction(int projectilecode, int row = 0){
        callstack.Insert(currentIndex+1, new Action(new CodeProperties(), ActionType.Output, controller.projectilecode,row));
    }
    // Update is called once per frame
    void Update()
    {
        if (enterDelay < 0)
        {
            if (callstack.Count > 0 && indexOfAction < callstack.Count && currentIndex != indexOfAction && controller.reachedPosition)
            {
                
                currentIndex = indexOfAction;
                
                if (callstack[currentIndex].Category == ActionType.Dialog)
                {
                    controller.reachedPosition = false;
                    StartCoroutine(controller.MoveToPosition(controller.RoundPosition(callstack[currentIndex].Position)));
                }
                else if (callstack[currentIndex].Category == ActionType.Throw)
                {
                    controller.ThrowTool();
                    if (GlobalState.GameMode == stringLib.GAME_MODE_ON){
                        nextAction = true; 
                    }
                    else{
                        if  (controller.projectilecode == 2 || controller.projectilecode == 4)
                            nextAction = true; 
                         
                    }
                    
 
                }
                else if (callstack[currentIndex].Category == ActionType.SwitchTool)
                {
                    //controller.selectedTool.GetComponent<SelectedTool>().NextTool();
                    nextAction = true; 
                }
                else if (callstack[currentIndex].Category == ActionType.Output){
                    handControl.HandleAction(callstack[currentIndex]); 
                    enterDelay+= 3f; 
                }
                
            }
            if (controller.reachedPosition && indexOfAction < callstack.Count && callstack[indexOfAction].Category == ActionType.Dialog)
            {
                output.text.GetComponent<Text>().text = callstack[currentIndex].text;
            }
            if ((Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0)|| entered) && controller.reachedPosition)
            {
                entered = false;
                indexOfAction++;
                enterDelay = 1f;
                if (indexOfAction < callstack.Count){
                    
                    if (callstack[indexOfAction].Category == ActionType.SwitchTool){
                         controller.selectedTool.GetComponent<SelectedTool>().NextTool();
                    }
                    handControl.HandleAction(callstack[indexOfAction], controller.selectedTool.GetComponent<SelectedTool>().projectilecode); 
                }
            }
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0)){
                //autoEnabled = false;
            }

        }
        enterDelay -= Time.deltaTime;
    }
}

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
    int currentIndex = -1; 

    // Start is called before the first frame update
    void Start()
    {
        controller = this.GetComponent<hero2Controller>(); 
        callstack = new List<Action>(); 
        output = GameObject.Find("OutputCanvas").transform.GetChild(0).GetComponent<Output>(); 
    }

    // Update is called once per frame
    void Update()
    {
        
        if (callstack.Count > 0 && indexOfAction < callstack.Count && currentIndex != indexOfAction){
            currentIndex = indexOfAction; 
            if (callstack[currentIndex].Category == ActionType.Dialog){
                controller.reachedPosition = false; 
                StartCoroutine(controller.MoveToPosition(controller.RoundPosition(callstack[currentIndex].Position))); 
            }
            else if (callstack[currentIndex].Category == ActionType.Throw){
                controller.ThrowTool(); 
            }
        }
        if(controller.reachedPosition && indexOfAction < callstack.Count && callstack[indexOfAction].Category == ActionType.Dialog){
            output.text.GetComponent<Text>().text = callstack[indexOfAction].text; 
        }
        if (Input.GetKeyDown(KeyCode.Return)){
            indexOfAction++; 
        }
        
    }
}

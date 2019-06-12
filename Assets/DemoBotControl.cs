using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoBotControl : MonoBehaviour
{
    public List<Action> callstack; 
    private hero2Controller controller; 
    int indexOfAction = 0; 
    int currentIndex = 0; 

    // Start is called before the first frame update
    void Start()
    {
        controller = this.GetComponent<hero2Controller>(); 
        callstack = new List<Action>(); 
    }

    // Update is called once per frame
    void Update()
    {
        
        if (callstack.Count > 0 && currentIndex != indexOfAction){
            currentIndex = indexOfAction; 
            StartCoroutine(controller.MoveToPosition(callstack[currentIndex].Position)); 
        }
        if (!controller.isMoving && Input.GetKeyDown(KeyCode.Return)){
            indexOfAction++; 
        }
        
    }
}

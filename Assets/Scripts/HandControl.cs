using System.Collections;
using System; 
using System.Collections.Generic;
using UnityEngine;

public class HandControl : MonoBehaviour
{
    // Start is called before the first frame update
    Transform handPos; 
    RectTransform[] sidebarPositions = new RectTransform[5]; //store the positions of the sidebar to interact with them
    RectTransform rightArrowPos, leftArrowPos, enterPos;  //store the positions of the output buttons
    bool reachedPosition = true; 
    void Start(){
        handPos = this.GetComponent<Transform>(); 
        for (int i = 2; i < 2 + 5; i++){
            //Debug.Log(GameObject.Find("Sidebar").transform.GetChild(2).transform.GetChild(i).name + " : " + (i-2).ToString()); 
            sidebarPositions[i-2] = GameObject.Find("Sidebar").transform.GetChild(2).transform.GetChild(i).GetComponent<RectTransform>(); 
        }
        rightArrowPos = GameObject.Find("OutputCanvas").transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).GetComponent<RectTransform>();
        leftArrowPos =  GameObject.Find("OutputCanvas").transform.GetChild(0).transform.GetChild(0).transform.GetChild(1).GetComponent<RectTransform>();
        enterPos = GameObject.Find("OutputCanvas").transform.GetChild(0).transform.Find("OutputEnter").GetComponent<RectTransform>();  
    }
    /// <summary>
    /// The hand will move to the position specified. The speed of this 
    /// movement is consistent, thus further distances will be made in the same
    /// time as shorter ones.
    /// </summary>
    /// <param name="position">Position in Game/World Space</param>
    /// <returns></returns>
    IEnumerator MoveToPosition(Vector3 position){
        //Debug.Log("Mouse Position: " + position.ToString()); 
        reachedPosition = false; 
        float speed = 30f; 
        float distX = (position.x - transform.position.x)/speed; 
        float distY = (position.y - transform.position.y)/speed; 
        while(Math.Abs(position.x - handPos.position.x) > 0.1f || Math.Abs(position.y - handPos.position.y) > 0.1f){
            if (Math.Abs(position.x - handPos.position.x) > 0.1f){
                handPos.position = new Vector3(handPos.position.x + distX, handPos.position.y, handPos.position.z); 
            }
            if (Math.Abs(position.y - handPos.position.y) > 0.1f){
                handPos.position = new Vector3(handPos.position.x, handPos.position.y + distY, handPos.position.z); 
            }
            yield return null; 
        }
        reachedPosition = true; 
    }
    /// <summary>
    /// Interact with the right arrow before putting the hand over
    /// the enter button. 
    /// </summary>
    /// <returns></returns>
    IEnumerator ArrowClick(){
        var worldCorners = new Vector3[4]; 
        rightArrowPos.GetWorldCorners(worldCorners); 
        StartCoroutine(MoveToPosition(new Vector3(worldCorners[1].x + 0.5f, worldCorners[2].y -1f, worldCorners[0].z))); 
      
        while (!reachedPosition) yield return null; 
        yield return new WaitForSecondsRealtime(0.7f); 

        worldCorners = new Vector3[4]; 
        enterPos.GetWorldCorners(worldCorners); 
        StartCoroutine(MoveToPosition(new Vector3(worldCorners[1].x + 0.5f, worldCorners[2].y -1f, worldCorners[0].z))); 

    }
    /// <summary>
    /// Interact with the Left Arrow only.
    /// </summary>
    /// <returns></returns>
    IEnumerator ClickLeftArrow(){
        var worldCorners = new Vector3[4]; 
        leftArrowPos.GetWorldCorners(worldCorners); 
        StartCoroutine(MoveToPosition(new Vector3(worldCorners[1].x + 0.5f, worldCorners[2].y -1f, worldCorners[0].z))); 
      
        while (!reachedPosition) yield return null; 
    }
    /// <summary>
    /// Interact with the Right Arrow Only
    /// </summary>
    /// <returns></returns>
    IEnumerator ClickRightArrow(){
        //convert screenspace to world space
        var worldCorners = new Vector3[4]; 
        rightArrowPos.GetWorldCorners(worldCorners); 
        //dispatch movement
        StartCoroutine(MoveToPosition(new Vector3(worldCorners[1].x + 0.5f, worldCorners[2].y -1f, worldCorners[0].z))); 
      
        while (!reachedPosition) yield return null; 
    }
    /// <summary>
    /// Takes the action sent by the demo bot from the call stack, then 
    /// dispatches actions to coroutines.
    /// </summary>
    /// <param name="action">Callstack Action</param>
    /// <param name="projectileCode">Tool currently in use.</param>
    public void HandleAction(Action action, int projectileCode = -1){
        StopAllCoroutines();  
        reachedPosition = true; 
        if (action.Category == ActionType.Dialog || action.Category == ActionType.Hack){
            StartCoroutine(MoveToPosition(new Vector3(action.Position.x, action.Position.y - 0.5f, 1))); 
        }
        else if (action.Category == ActionType.Throw){
            StartCoroutine(MoveToPosition(Camera.main.ScreenToWorldPoint(new Vector3(30,30,1)))); 
        }
        else if (action.Category == ActionType.SwitchTool){
            var worldCorners = new Vector3[4];           
            sidebarPositions[projectileCode].GetWorldCorners(worldCorners);
            StartCoroutine(MoveToPosition(new Vector3(worldCorners[1].x + 0.5f, worldCorners[2].y - 1f, worldCorners[0].z))); 
        }
        else if (action.Category == ActionType.Output){
            if (GlobalState.GameMode == stringLib.GAME_MODE_ON && (action.lineNumber == 1 || action.lineNumber == 2)){
                StartCoroutine(ArrowClick()); 
            }
            else if (GlobalState.GameMode == stringLib.GAME_MODE_ON && (action.lineNumber == stateLib.TOOL_COMMENTER)){
                if(action.Column == 1){
                    StartCoroutine(ClickLeftArrow()); 
                }else StartCoroutine(ClickRightArrow()); 
            }
            else{
                var worldCorners = new Vector3[4]; 
                enterPos.GetWorldCorners(worldCorners); 
                StartCoroutine(MoveToPosition(new Vector3(worldCorners[1].x + 0.5f, worldCorners[2].y -1f, worldCorners[0].z))); 
            }
        }
    }
}

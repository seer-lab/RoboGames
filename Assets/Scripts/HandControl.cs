using System.Collections;
using System; 
using System.Collections.Generic;
using UnityEngine;

public class HandControl : MonoBehaviour
{
    // Start is called before the first frame update
    Transform handPos; 
    RectTransform[] sidebarPositions = new RectTransform[5]; 
    RectTransform rightArrowPos, enterPos; 
    bool reachedPosition = true; 
    void Start(){
        handPos = this.GetComponent<Transform>(); 
        for (int i = 2; i < 2 + 5; i++){
            Debug.Log(GameObject.Find("Sidebar").transform.GetChild(2).transform.GetChild(i).name + " : " + (i-2).ToString()); 
            sidebarPositions[i-2] = GameObject.Find("Sidebar").transform.GetChild(2).transform.GetChild(i).GetComponent<RectTransform>(); 
        }
        rightArrowPos = GameObject.Find("OutputCanvas").transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).GetComponent<RectTransform>();
        enterPos = GameObject.Find("OutputCanvas").transform.GetChild(0).transform.Find("OutputEnter").GetComponent<RectTransform>();  
    }
    IEnumerator MoveToPosition(Vector3 position){
        Debug.Log("Mouse Position: " + position.ToString()); 
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
    public void HandleAction(Action action, int projectileCode = -1){
        StopCoroutine("MoveToPosition"); 
        if (action.Category == ActionType.Dialog){
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
            else{
                var worldCorners = new Vector3[4]; 
                enterPos.GetWorldCorners(worldCorners); 
                StartCoroutine(MoveToPosition(new Vector3(worldCorners[1].x + 0.5f, worldCorners[2].y -1f, worldCorners[0].z))); 
            }
        }
    }
}

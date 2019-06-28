using System.Collections;
using System; 
using System.Collections.Generic;
using UnityEngine;

public class HandControl : MonoBehaviour
{
    // Start is called before the first frame update
    Transform handPos; 
    RectTransform[] sidebarPositions = new RectTransform[5]; 
    void Start(){
        handPos = this.GetComponent<Transform>(); 
        for (int i = 2; i < 2 + 5; i++){
            sidebarPositions[i-2] = GameObject.Find("Sidebar").transform.GetChild(2).transform.GetChild(i).GetComponent<RectTransform>(); 
        }
    }
    IEnumerator MoveToPosition(Vector3 position){
        float speed = 30f; 
        float distX = (position.x - transform.position.x)/speed; 
        float distY = (position.y - transform.position.y)/speed; 
        while(Math.Abs(handPos.position.x - position.x) > 0.2f && Math.Abs(handPos.position.y - position.y) > 0.2f){
            if (Math.Abs(handPos.position.x - position.x) > 0.2f){
                handPos.position = new Vector3(handPos.position.x + distX, handPos.position.y, handPos.position.z); 
            }
            if (Math.Abs(handPos.position.y - position.y) > 0.2f){
                handPos.position = new Vector3(handPos.position.x, handPos.position.y + distY, handPos.position.z); 
            }
            yield return null; 
        }
    }
    public void HandleAction(Action action, int projectileCode = -1){
        if (action.Category == ActionType.Dialog){
            StartCoroutine(MoveToPosition(action.Position)); 
        }
        else if (action.Category == ActionType.Throw){
            StartCoroutine(MoveToPosition(Camera.main.ScreenToWorldPoint(new Vector3(10,10,1)))); 
        }
        else if (action.Category == ActionType.SwitchTool){
            var worldCorners = new Vector3[4];
            
            sidebarPositions[(projectileCode + 1 < sidebarPositions.Length) ? projectileCode +1 : 0].GetWorldCorners(worldCorners); 
            StartCoroutine(MoveToPosition(Camera.main.ScreenToWorldPoint(new Vector3(worldCorners[0].x, worldCorners[0].y, 1)))); 
        }
    }
}

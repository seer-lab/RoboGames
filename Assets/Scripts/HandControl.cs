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
            Debug.Log(GameObject.Find("Sidebar").transform.GetChild(2).transform.GetChild(i).name + " : " + (i-2).ToString()); 
            sidebarPositions[i-2] = GameObject.Find("Sidebar").transform.GetChild(2).transform.GetChild(i).GetComponent<RectTransform>(); 
        }
    }
    IEnumerator MoveToPosition(Vector3 position){
        Debug.Log("Mouse Position: " + position.ToString()); 
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
    }
    public void HandleAction(Action action, int projectileCode = -1){
        if (action.Category == ActionType.Dialog){
            StartCoroutine(MoveToPosition(new Vector3(action.Position.x, action.Position.y - 0.5f, 1))); 
        }
        else if (action.Category == ActionType.Throw){
            Debug.Log("Throw Mouse"); 
            StartCoroutine(MoveToPosition(Camera.main.ScreenToWorldPoint(new Vector3(30,30,1)))); 
        }
        else if (action.Category == ActionType.SwitchTool){
            Debug.Log("Projectile Code: " + projectileCode);
            var worldCorners = new Vector3[4];           
            sidebarPositions[projectileCode-1].GetWorldCorners(worldCorners);
            StartCoroutine(MoveToPosition(new Vector3(worldCorners[1].x, worldCorners[2].y - 0.5f, worldCorners[0].z))); 
        }
    }
}

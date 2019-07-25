using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxBug : Enemies
{
    private float distanceX = 10f; 
    private float distanceY = 5f; 
    float xOffset; 
    float speed = 100f; 
    Vector3 originalPos; 
    protected override void InitializeEnemyMovement(){
        if (properties == null) properties = new CodeProperties(); 
        xOffset = Random.Range(0,10); 
        Position = new Vector3(GlobalState.StringLib.LEFT_CODESCREEN_X_COORDINATE + xOffset, properties.initialLineY + stateLib.TOOLBOX_Y_OFFSET - (index-2)*properties.linespacing,1); 
        originalPos = Position; 
        StartCoroutine(MoveEnemy()); 
    }
    protected override IEnumerator MoveEnemy(){
        while(true){
            yield return null; 
            float addition = (originalPos.x + distanceX - Position.x)/speed; 
            while ((originalPos.x + distanceX > Position.x && distanceX > 0 )||( originalPos.x + distanceX < Position.x && distanceX < 0)){
                Position = new Vector3(Position.x + addition, Position.y, Position.z); 
                yield return null; 
            }
            addition = (originalPos.y + distanceY - Position.y)/speed; 
            while((originalPos.y + distanceX > Position.y && distanceY > 0)|| (originalPos.y + distanceY < Position.y && distanceY < 0)){
                Position = new Vector3(Position.x, Position.y + addition, Position.z); 
                yield return null; 
            }
            originalPos = new Vector3(originalPos.x + distanceX, originalPos.y + distanceY, originalPos.z); 
            distanceX*= -1; 
            distanceY*= -1; 
        }
    }

    protected override float GetDamage(){
        return 50f; 
    }
    protected override int GetCode(){
        return 4; 
    }
    
}

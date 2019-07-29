using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxBug : Enemies
{
    private float distanceX = 10f; 
    private float distanceY = 1f; 
    float xOffset; 
    float speedX = 200f; 
    float speedY = 40f; 
    Vector3 originalPos; 
    protected override void InitializeEnemyMovement(){
        if (properties == null) properties = new CodeProperties(); 
        xOffset = Random.Range(0,10); 
        distanceY = 2*properties.linespacing;
        Position = new Vector3(GlobalState.StringLib.LEFT_CODESCREEN_X_COORDINATE, properties.initialLineY + stateLib.TOOLBOX_Y_OFFSET - (index+2)*properties.linespacing,1); 
        originalPos = Position; 
        Position = new Vector3(GlobalState.StringLib.LEFT_CODESCREEN_X_COORDINATE + xOffset, properties.initialLineY + stateLib.TOOLBOX_Y_OFFSET - (index+2)*properties.linespacing,1); 
        if (xOffset > 5){
            originalPos = new Vector3(originalPos.x + distanceX, originalPos.y + distanceY, originalPos.z); 
            distanceX*= -1; 
            distanceY*= -1; 
        }
        StartCoroutine(MoveEnemy()); 
    }
    protected override IEnumerator MoveEnemy(){
        while(true){
            yield return null; 
            if (distanceX > 0 && !GetComponent<SpriteRenderer>().flipX){
                GetComponent<SpriteRenderer>().flipX = true; 
            }
            else if (distanceX < 0 && GetComponent<SpriteRenderer>().flipX){
                GetComponent<SpriteRenderer>().flipX = false; 
            }
            float addition = (distanceX)/speedX; 
            while ((originalPos.x + distanceX > Position.x && distanceX > 0 )||( originalPos.x + distanceX < Position.x && distanceX < 0)){
                Position = new Vector3(Position.x + addition, Position.y, Position.z); 
                yield return null; 
                while(Output.IsAnswering) yield return null; 
            }
            addition = (distanceY)/speedY; 
            while((originalPos.y + distanceY > Position.y && distanceY > 0)|| (originalPos.y + distanceY < Position.y && distanceY < 0)){
                Position = new Vector3(Position.x, Position.y + addition, Position.z); 
                yield return null; 
                while(Output.IsAnswering) yield return null; 
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

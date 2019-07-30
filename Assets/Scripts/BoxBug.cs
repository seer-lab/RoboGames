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

        //Initialize position twice. Once for the initial area so the bug stays within the code screen. 
        //A second time to add a sense of randomness.
        Position = new Vector3(GlobalState.StringLib.LEFT_CODESCREEN_X_COORDINATE, properties.initialLineY + stateLib.TOOLBOX_Y_OFFSET - (index+2)*properties.linespacing,1); 
        originalPos = Position; 
        Position = new Vector3(GlobalState.StringLib.LEFT_CODESCREEN_X_COORDINATE + xOffset, properties.initialLineY + stateLib.TOOLBOX_Y_OFFSET - (index+2)*properties.linespacing,1); 
        //randomly choose a direction to travel. 
        if (xOffset > 5){
            originalPos = new Vector3(originalPos.x + distanceX, originalPos.y + distanceY, originalPos.z); 
            distanceX*= -1; 
            distanceY*= -1; 
        }
        StartCoroutine(MoveEnemy()); 
    }
    protected override IEnumerator MoveEnemy(){
        while(true){
            yield return null; //allows infinite loops in coroutines. 
            //decide facing direction
            if (distanceX > 0 && !GetComponent<SpriteRenderer>().flipX){
                GetComponent<SpriteRenderer>().flipX = true; 
            }
            else if (distanceX < 0 && GetComponent<SpriteRenderer>().flipX){
                GetComponent<SpriteRenderer>().flipX = false; 
            }
            //calculate travel distance along x. 
            float addition = (distanceX)/speedX; 
            while ((originalPos.x + distanceX > Position.x && distanceX > 0 )||( originalPos.x + distanceX < Position.x && distanceX < 0)){
                Position = new Vector3(Position.x + addition, Position.y, Position.z); 
                yield return null; 
                while(Output.IsAnswering || GlobalState.GameState != stateLib.GAMESTATE_IN_GAME) yield return null; 
            }
            //calculate travel distance along y. 
            addition = (distanceY)/speedY; 
            while((originalPos.y + distanceY > Position.y && distanceY > 0)|| (originalPos.y + distanceY < Position.y && distanceY < 0)){
                Position = new Vector3(Position.x, Position.y + addition, Position.z); 
                yield return null; 
                while(Output.IsAnswering || GlobalState.GameState != stateLib.GAMESTATE_IN_GAME) yield return null; 
            }
            //flip the original position with the new position. 
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

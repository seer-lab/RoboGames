using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriBug : Enemies
{
    float xOffset; 
    float distanceY; 
    float distanceX = 4; 
    Vector3 originalPos; 
    float speed = 80f; 
    protected override void InitializeEnemyMovement(){
        if (properties == null) properties = new CodeProperties(); 
        xOffset = Random.Range(0,10); 
        distanceY = 3*properties.linespacing;
        Position = new Vector3(GlobalState.StringLib.LEFT_CODESCREEN_X_COORDINATE, properties.initialLineY + stateLib.TOOLBOX_Y_OFFSET - (index+2)*properties.linespacing,1);
        originalPos = Position; 
    }
    protected override IEnumerator MoveEnemy(){
        bool isRight = true; 
        while(true){
            if ((int)xOffset % 2 == 0){
                isRight = false; 
            }
            float addition = distanceX/speed; 
            while(Position.x < originalPos.x + distanceX){
                Position = new Vector3 (Position.x + addition, Position.y, Position.z); 
                yield return null; 
            }
            float additionX = -distanceX/speed; 
            float additionY = distanceY/speed; 
            while(Position.y < originalPos.y + distanceY){
                Position = new Vector3(Position.x + additionX, Position.y + additionY, Position.z); 
                yield return null; 
            }
            additionX = -distanceX/speed; 
            additionY = -distanceY/speed; 
            while(Position.y < originalPos.y + distanceY){
                Position = new Vector3(Position.x + additionX, Position.y + additionY, Position.z); 
                yield return null; 
            }
            
        }
    }
    protected override float GetDamage(){
        return 50f; 
    }
    protected override int GetCode(){
        return 3; 
    }
}

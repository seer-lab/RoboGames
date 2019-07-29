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
        Position = new Vector3(GlobalState.StringLib.LEFT_CODESCREEN_X_COORDINATE + xOffset, properties.initialLineY + stateLib.TOOLBOX_Y_OFFSET - (index+2)*properties.linespacing,1);
        originalPos = Position; 
        StartCoroutine(MoveEnemy()); 
    }
    protected override IEnumerator MoveEnemy(){
        bool isRight = true; 
        while(true){
            yield return null; 
            if ((int)xOffset % 2 == 0){
                isRight = false; 
            }
            
            float addition = distanceX/speed; 
            while(Position.x < originalPos.x + distanceX){
                Position = new Vector3 (Position.x + addition, Position.y, Position.z); 
                yield return null; 
                while(Output.IsAnswering) yield return null;
            }
            float additionX = -distanceX/(2*speed); 
            float additionY = distanceY/speed; 
            while(Position.y < originalPos.y + distanceY){
                Position = new Vector3(Position.x + additionX, Position.y + additionY, Position.z); 
                yield return null; 
                while(Output.IsAnswering) yield return null;
            } 
            additionY = -distanceY/speed; 
            while(Position.y > originalPos.y){
                Position = new Vector3(Position.x + additionX, Position.y + additionY, Position.z); 
                yield return null; 
                while(Output.IsAnswering) yield return null;
            }
            originalPos = Position; 
        }
    }
    protected override float GetDamage(){
        return 50f; 
    }
    protected override int GetCode(){
        return 3; 
    }
}

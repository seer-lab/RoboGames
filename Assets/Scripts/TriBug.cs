using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriBug : Enemies
{
    float xOffset; 
    float distanceY; 
    float distanceX = 6; 
    Vector3 originalPos; 
    float speed = 80f+(GlobalState.AdaptiveMode*(-20f)*(GlobalState.HintMode-1));
    public override void InitializeEnemyMovement(){
        StopAllCoroutines();
        if (properties == null) properties = new CodeProperties(); 
        if (GlobalState.level.IsDemo)xOffset = 0;
        else xOffset = Random.Range(0,8); 
        distanceY = 4*properties.linespacing;
        Position = new Vector3(GlobalState.StringLib.LEFT_CODESCREEN_X_COORDINATE + xOffset, properties.initialLineY + stateLib.TOOLBOX_Y_OFFSET - (index+2)*properties.linespacing,1);
        originalPos = Position; 
        StartCoroutine(MoveEnemy()); 
    }
    protected override IEnumerator MoveEnemy(){
        bool isRight = true; 
        if ((int)xOffset % 2 == 0){
                isRight = false; 
            }
        while(true){
            yield return null; 
            
            //move along x. 
            float addition = distanceX/speed; 
            while(Position.x < originalPos.x + distanceX){
                Position = new Vector3 (Position.x + addition, Position.y, Position.z); 
                yield return null; 
                while(Output.IsAnswering) yield return null;
            }
            //move up and back along x & y. 
            float additionX = -distanceX/(2*speed); 
            float additionY = distanceY/speed; 
            while(Position.y < originalPos.y + distanceY){
                Position = new Vector3(Position.x + additionX, Position.y + additionY, Position.z); 
                yield return null; 
                while(Output.IsAnswering || GlobalState.GameState != stateLib.GAMESTATE_IN_GAME) yield return null;
            } 
            //move down and back along x & y. Returning to original postion. 
            additionY = -distanceY/speed; 
            while(Position.y > originalPos.y){
                Position = new Vector3(Position.x + additionX, Position.y + additionY, Position.z); 
                yield return null; 
                while(Output.IsAnswering || GlobalState.GameState != stateLib.GAMESTATE_IN_GAME) yield return null;
            }
            originalPos = Position; 
        }
    }
    protected override float GetDamage(){
        return GlobalState.Stats.DamageLevel;  
    }
    protected override int GetCode(){
        return 3; 
    }
}

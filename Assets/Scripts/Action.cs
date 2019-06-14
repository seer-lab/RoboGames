using System.Collections; 
using System.Collections.Generic; 
using UnityEngine; 

public enum ActionType{Dialog, Throw, SwitchTool}
public class Action{
    Vector3 position;  
    string text; 
    int lineNumber, Column; 
    CodeProperties properties; 
    public Vector3 Position {
        get{
            return new Vector3(properties.bugXshift+ Column, properties.initialLineY- properties.linespacing*lineNumber, 0); 
        }
    }
    int projectileCode {get;set;} 
    ActionType Category {get;set;}
    public Action(ActionType type, int row, int col, string dialog = "", int code = -1){
        Category = type; 
        projectileCode = code; 
        lineNumber = row; 
        properties = new CodeProperties(); 
        Column = col; 
    }
    public void setPosition(Vector3 pos){
        this.position = pos; 
    }
}
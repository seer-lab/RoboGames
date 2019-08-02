
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Obstacle : MonoBehaviour
{
    protected int index = -1;  
    protected CodeProperties properties; 
    public Vector3 Position {get;set;}

    /// <summary>
    /// The line number the Obstacle originates from.
    /// </summary>
    /// <value>Index: Line number</value>
    public int Index{
        get {
            return index; 
        }
        set{
            index = value; 
        }
    }
    public CodeProperties Properties{
        set{
            properties = value; 
        }
    }
    void Start(){
        properties = new CodeProperties(); 
        Initialize(); 
        SetPosition();
    }
    protected virtual void Initialize(){}
    void Update(){
        UpdateProtocol(); 
    }
    protected virtual void UpdateProtocol(){

    }
    AudioSource audioSource; 

    /// <summary>
    /// set the position in the game.
    /// </summary>
    public virtual void SetPosition(){
        if (properties == null) properties = new CodeProperties(); 
        if (Position == null){
            float xoffset = Random.Range(2,6); 
            if (GlobalState.level.IsDemo) xoffset = 0; 
            this.transform.position = new Vector3(properties.initialLineX + 0.5f + xoffset, properties.initialLineY - 0.8f + stateLib.TOOLBOX_Y_OFFSET - index* properties.linespacing + properties.lineOffset, 1);
        }
        else {
            transform.position = Position; 
        }
    } 

    /// <summary>
    /// name of the type of the obstacle.
    /// </summary>
    /// <returns>Name</returns>
    public abstract string GetObstacleType(); 
}

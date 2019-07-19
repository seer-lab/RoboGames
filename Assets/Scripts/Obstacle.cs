using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Obstacle : MonoBehaviour
{
    protected int index = -1;  
    protected CodeProperties properties; 

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
    void Start(){
        properties = new CodeProperties(); 
    }
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
        this.transform.position = new Vector3(properties.initialLineX + 0.5f, properties.initialLineY + stateLib.TOOLBOX_Y_OFFSET - Index * properties.linespacing, 1);
    } 

    /// <summary>
    /// name of the type of the obstacle.
    /// </summary>
    /// <returns>Name</returns>
    public abstract string GetObstacleType(); 
}

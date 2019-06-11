using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Obstacle : MonoBehaviour
{
    protected int index = -1;  
    protected CodeProperties properties; 
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
    public virtual void SetPosition(){
        if (properties == null) properties = new CodeProperties(); 
        this.transform.position = new Vector3(properties.initialLineX + 0.5f, properties.initialLineY + stateLib.TOOLBOX_Y_OFFSET - Index * properties.linespacing, 1);
    } 
    public abstract string GetObstacleType(); 
}

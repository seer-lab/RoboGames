using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirewallController : MonoBehaviour
{
    public int Index {get;set;}
    public int Damage {get;set;}
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D collidingObj){
        if (collidingObj.name == "Hero"){
            collidingObj.GetComponent<hero2Controller>().onTakeDamange(Damage); 
        }
    }
    
}

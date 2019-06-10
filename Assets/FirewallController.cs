using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirewallController : MonoBehaviour
{
    public int Index {get;set;}
    public int Damage {get;set;}

    Collider2D lastHero; 
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (lastHero != null && GetComponent<BoxCollider2D>().IsTouching(lastHero)){
            lastHero.GetComponent<hero2Controller>().onTakeDamange(Damage); 
        }
    }
    void OnTriggerEnter2D(Collider2D collidingObj){
        if (collidingObj.name == "Hero"){
            if (collidingObj.GetComponent<hero2Controller>().onTakeDamange(Damage))
                GetComponent<AudioSource>().Play(); 
            lastHero = collidingObj; 
        }
    }

}

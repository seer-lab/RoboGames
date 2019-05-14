using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintCollider : Tools
{
    public bool isTriggered{get; set;}
    public bool isFinished{get; set;}
    public string outmessage{get; set;}
    public Animator anim;

    public override void Initialize(){
        isTriggered = false;
        isFinished = false;
        this.GetComponent<Renderer>().enabled = true;
        anim = GetComponent<Animator>();
        output = GameObject.Find("OutputCanvas").transform.GetChild(0).GetComponent<Output>();

    }

    void OnTriggerEnter2D(Collider2D colliderObj){
        if(colliderObj.name == stringLib.PROJECTILE_HINT){
            Debug.Log("Enters the collider");
            this.GetComponent<Renderer>().enabled = true;
            Destroy(colliderObj.gameObject);
            anim.SetBool("Dying", true);
            isTriggered = true;
            output.Text.text = outmessage;
        }
    }
}

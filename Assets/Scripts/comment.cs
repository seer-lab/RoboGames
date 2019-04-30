//**************************************************//
// Class Name: comment
// Class Description: Instantiable object in the Robot ON! game. This class is the controller for
//                    the comment tasks, and is paired with the Commenter and Un-commenter tool.
// Methods:
// 		void Start()
//		void Update()
//		void OnTriggerEnter2D(Collider2D collidingObj)
// Author: Scott McLean
// Date Last Modified: 6/24/2016
//**************************************************//

using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI; 
using System.Text.RegularExpressions;

public abstract class comment : Tools {
	public bool isCommented;
	public string commentStyle;
	public int entityType = -1;
	public int groupid    = -1;
	public int size = -1;

	public string oldtext   = "";
	public string blocktext = "";
	public string errmsg    = "";

	//public GameObject CodeObject;
	public GameObject CorrectCommentObject;
	
	protected Sprite descSpriteOff;
	protected Sprite descSpriteOn;
	protected Sprite codeSpriteOff;
	protected Sprite codeSpriteOn;


	protected bool doneUpdating = false;

	protected bool resetting  = false;

	protected float resetTime = 0f;
	protected float timeDelay = 30f;

    protected TextColoration textColoration; 

    public override void Initialize()
    {

        string path = "Sprites/";
        descSpriteOff = Resources.LoadAll<Sprite>(path + "dComment")[2];
        descSpriteOn = Resources.LoadAll<Sprite>(path + "dComment")[0]; 
        codeSpriteOff = Resources.LoadAll<Sprite>(path + "cComment")[2];
        codeSpriteOn = Resources.LoadAll<Sprite>(path + "cComment")[0];
        if (entityType == stateLib.ENTITY_TYPE_CORRECT_COMMENT || entityType == stateLib.ENTITY_TYPE_INCORRECT_COMMENT)
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = descSpriteOff;
        }
        else
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = codeSpriteOff;
        }
        textColoration = new TextColoration();
    }
    //.................................>8.......................................
    // Update is called once per frame
    void Update() {
        UpdateProtocol(); 
	}
    public virtual void UpdateProtocol() {
       
    }

	//.................................>8.......................................
	void OnTriggerEnter2D(Collider2D collidingObj) {
        OnTriggerProtocol(collidingObj); 
	}
    protected abstract void OnTriggerProtocol(Collider2D collidingObj); 

	//.................................>8.......................................
	void UpdateIncorrect() {
		
	}

}

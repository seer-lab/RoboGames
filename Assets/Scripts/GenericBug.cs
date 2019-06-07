//**************************************************//
// Class Name: GenericBug
// Class Description: This is an instantiable object for the RoboBUG game. This is the controller for
//                    <bug/> tags.
// Methods:
// 		void Start()
//		void Update()
//		void OnTriggerEnter2D(Collider2D collidingObj)
// Author: Michael Miljanovic
// Date Last Modified: 6/1/2016
//**************************************************//

using UnityEngine;
using System.Collections;
using System.IO;

public class GenericBug : Tools {

	public bool IsDead { get; set; }
    public bool Finished { get; set; }
	public Animator anim;

	public string answer; 
	AudioClip glitch; 


    public override void Initialize()
    {
		glitch = Resources.Load<AudioClip>("Sound/Triggers/Glitch"); 
        IsDead = false;
        Finished = false; 
        this.GetComponent<Renderer>().enabled = false;
        anim = GetComponent<Animator>();
    }
	IEnumerator GlitchText(){		
		audioSource.PlayOneShot(glitch, 0.5f); 
		TextMesh text = GetComponent<TextMesh>(); 
		if (!GlobalState.IsDark) text.color = Color.black; 
		text.text = GlobalState.level.Code[index]; 
		GetComponent<MeshRenderer>().enabled = true; 
		text.fontSize = new int[]{stateLib.TEXT_SIZE_SMALL, stateLib.TEXT_SIZE_NORMAL, stateLib.TEXT_SIZE_LARGE, stateLib.TEXT_SIZE_VERY_LARGE}[GlobalState.TextSize]; 
		GlobalState.level.Code[index] = " "; 
		lg.DrawInnerXmlLinesToScreen(); 
		text.font = Resources.Load<Font>("Fonts/HACKED"); 
		yield return new WaitForSeconds(0.12f); 
		text.font = Resources.Load<Font>("Fonts/CFGlitchCity-Regular"); 
		yield return new WaitForSeconds(0.12f); 
		TextColoration color = new TextColoration(); 
		text.text = color.ColorizeText(answer, GlobalState.level.Language);  
		yield return new WaitForSeconds(0.1f); 
		text.font = Resources.Load<Font>("Fonts/HACKED"); 
		yield return new WaitForSeconds(0.1f); 
		text.font = Resources.Load<Font>("Fonts/Inconsolata"); 

	}
	//.................................>8.......................................
	void OnTriggerEnter2D(Collider2D collidingObj) {
		if (collidingObj.name == stringLib.PROJECTILE_BUG) {
		 	foreach(GameObject comment in lg.manager.roboBUGcomments){
				 comment.GetComponent<BugComment>().Uncomment(); 
			 }
			//this.GetComponent<Renderer>().enabled = true;
			Destroy(collidingObj.gameObject);
			//anim.SetBool("Dying", true);
			//GetComponent<AudioSource>().Play();
			IsDead = true;
			lg.numberOfBugsRemaining--;
            GlobalState.level.CompletedTasks[0]++;
			// Award 1 extra use of the tool.
			selectedTool.bonusTools[stateLib.TOOL_CATCHER_OR_CONTROL_FLOW]++;
			StartCoroutine(GlitchText()); 
		}
	}

	//.................................>8.......................................

}

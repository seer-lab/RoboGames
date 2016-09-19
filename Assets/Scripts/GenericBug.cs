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

public class GenericBug : MonoBehaviour {

	public bool isDead	= false;
	public bool finished = false;
	public string language;
	public Animator anim;
	public GameObject CodescreenObject;
	public GameObject ToolSelectorObject;
	private LevelGenerator lg;

	//.................................>8.......................................
	// Use this for initialization
	void Start() {
		this.GetComponent<Renderer>().enabled = false;
		anim = GetComponent<Animator>();
		lg = CodescreenObject.GetComponent<LevelGenerator>();
	}

	//.................................>8.......................................
	// Update is called once per frame
	void Update() {
	}

	//.................................>8.......................................
	void OnTriggerEnter2D(Collider2D collidingObj) {
		if (collidingObj.name == stringLib.PROJECTILE_BUG) {
			Logger.printLogFile(stringLib.LOG_BUG_FOUND, this.transform.position);
			this.GetComponent<Renderer>().enabled = true;
			Destroy(collidingObj.gameObject);
			anim.SetBool("Dying", true);
			GetComponent<AudioSource>().Play();
			isDead = true;
			lg.numberOfBugsRemaining--;
			// Award 1 extra use of the tool.
			ToolSelectorObject.GetComponent<SelectedTool>().bonusTools[stateLib.TOOL_CATCHER_OR_ACTIVATOR]++;
			lg.toolsAirborne--;
		}
	}

	//.................................>8.......................................

}

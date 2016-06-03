//**************************************************//
// Class Name: PrizeBug
// Class Description:
// Methods:
// 		void Start()
//		void Update()
//		void OnTriggerEnter2D(Collider2D p)
// Author: Michael Miljanovic
// Date Last Modified: 6/1/2016
//**************************************************//

using UnityEngine;
using System.Collections;

public class PrizeBug : MonoBehaviour {
	public Animator anim;
	public bool dead = false;
	public bool finished = false;
	public GameObject tools;
	public int[] bonus = new int[stateLib.NUMBER_OF_TOOLS];

	//.................................>8.......................................
	// Use this for initialization
	void Start() {
		this.GetComponent<Renderer>().enabled = false;
		anim = GetComponent<Animator>();
	}

	//.................................>8.......................................
	// Update is called once per frame
	void Update() {
	}

	//.................................>8.......................................
	void OnTriggerEnter2D(Collider2D p) {
		if (p.name == stringLib.PROJECTILE_BUG && this.GetComponent<Renderer>().enabled == false) {
			this.GetComponent<Renderer>().enabled = true;
			Destroy(p.gameObject);
			anim.SetBool("Dying", true);
			GetComponent<AudioSource>().Play();
			dead = true;
			for (int i = 0; i < stateLib.NUMBER_OF_TOOLS; i++) {
				tools.GetComponent<SelectedTool>().bonusTools[i] += bonus[i];
			}
		}
	}

	//.................................>8.......................................
}

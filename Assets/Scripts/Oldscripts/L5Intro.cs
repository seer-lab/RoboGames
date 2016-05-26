using UnityEngine;
using System.Collections;

public class L5Intro : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GetComponent<TextMesh> ().text = "" +
			"You have access to a debugger tool" +
				"\nthat can help find the last bug." +
				"\n\nUse a combination of BREAKPOINTERS" +
				"\nand TESTERS to observe the" +
				"\nfunction's behaviour and " +
				"\nfigure out what is wrong.";
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

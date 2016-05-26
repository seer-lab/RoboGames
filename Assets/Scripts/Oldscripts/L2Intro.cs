using UnityEngine;
using System.Collections;

public class L2Intro : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GetComponent<TextMesh> ().text = "" +
			"A bug has infected functions" +
				"\nthat are hidden from you. " +
				"\n\nYou must attempt to" +
				"\nlocate the bug by " +
				"\nimplementing tests to " +
				"\ncatch it using " +
				"\nthe TESTER tool.";
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

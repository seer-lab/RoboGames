using UnityEngine;
using System.Collections;

public class L1Intro : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GetComponent<TextMesh> ().text = "" +
						"The bug is hiding somewhere" +
						"\nin the function. " +
						"\n\nLocate the bug, then press" +
						"\nCTRL to throw a " +
						"\nBUGCATCHER at it.";
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

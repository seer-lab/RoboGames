using UnityEngine;
using System.Collections;

public class L3Intro : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GetComponent<TextMesh> ().text = "" +
			"A bug has managed to infiltrate the" +
				"\nthreat assessment function. " +
				"\n\nObserve the output of the function" +
				"\nby printing values using the " +
				"\nACTIVATOR.";
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

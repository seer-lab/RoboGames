using UnityEngine;
using System.Collections;

public class L4Intro : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GetComponent<TextMesh> ().text = "" +
			"A bug has managed to dig deep into" +
				"\nthe color database. " +
				"\n\nTo find it," +
				"\ntry commenting out some of the" +
				"\ntables to isolate the bug's" +
				"\nlocation.";
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

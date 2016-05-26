using UnityEngine;
using System.Collections;

public class TextSetterblack : MonoBehaviour {

	string testing = "struct colour blacktab[] = {" +
		"\n { \"black\",       0,  0,  0  }," +
		"\n { \"ivory black\", 41, 36, 33 }," +
		"\n { \"lamp black\",  46, 71, 59 }" +
		"\n};";

	// Use this for initialization
	void Start () {
		TextMesh Tm = GetComponent<TextMesh>();
		Tm.text = testing;	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

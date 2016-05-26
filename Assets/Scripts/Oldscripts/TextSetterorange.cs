using UnityEngine;
using System.Collections;

public class TextSetterorange : MonoBehaviour {

	string testing = "struct colour orangetab[] = {" +
		"\n { \"cadmium orange\",    255,  97,   3 }," +
		"\n { \"cadmium red_light\", 255,   3,  13 }," +
		"\n { \"carrot\",            237, 145,  33 }," +
		"\n { \"dark orange\",       255, 140,   0 }," +
		"\n { \"mars orange\",       150,  69,  20 }," +
		"\n { \"mars yellow\",       227, 112,  26 }," +
		"\n { \"orange\",            255, 128,   0 }," +
		"\n { \"orange red\",        255,  69,   0 }," +
		"\n { \"yellow ochre\",      227, 130,  23 }" +
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

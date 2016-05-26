using UnityEngine;
using System.Collections;

public class TextSetternamed : MonoBehaviour {

	string testing = "struct colour namedtab[] = {" +
		"\n { \"aqua\",      0, 255, 255 }," +
		"\n { \"black\",     0,   0,   0 }," +
		"\n { \"blue\",      0,   0, 255 }," +
		"\n { \"fuchsia\", 255,   0, 255 }," +
		"\n { \"gray\",    128, 128, 128 }," +
		"\n { \"green\",     0, 128,   0 }," +
		"\n { \"lime\",      0, 255,   0 }," +
		"\n { \"maroon\",  128,   0,   0 }," +
		"\n { \"navy\",      0,   0, 128 }," +
		"\n { \"olive\",   128, 128,   0 }," +
		"\n { \"purple\",  128,   0, 128 }," +
		"\n { \"red\",     255,   0,   0 }," +
		"\n { \"silver\",  192, 192, 192 }," +
		"\n { \"teal\",      0, 128, 128 }," +
		"\n { \"white\",   255, 255, 255 }," +
		"\n { \"yellow\",  255, 255,   0 }" +
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

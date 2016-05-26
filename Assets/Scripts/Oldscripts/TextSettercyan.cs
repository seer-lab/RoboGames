using UnityEngine;
using System.Collections;

public class TextSettercyan : MonoBehaviour {

	string testing = "struct colour cyantab[] = {" +
		"\n { \"aquamarine\",        127, 255, 212 }," +
		"\n { \"aquamarine medium\", 102, 205, 170 }," +
		"\n { \"cyan\",              000, 255, 255 }," +
		"\n { \"cyan white\",        224, 255, 255 }," +
		"\n { \"turquoise\",         064, 224, 208 }," +
		"\n { \"turquoise dark\",    000, 206, 209 }," +
		"\n { \"turquoise medium\",  072, 209, 204 }," +
		"\n { \"turquoise pale\",    175, 238, 238 }" +
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

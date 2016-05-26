using UnityEngine;
using System.Collections;

public class TextSettergrey : MonoBehaviour {

	string testing = "struct colour greytab[] = {" +
		"\n { \"cold grey\",           128, 138, 135 }," +
		"\n { \"dim grey\",            105, 105, 105 }," +
		"\n { \"grey\",                192, 192, 192 }," +
		"\n { \"light grey\",          211, 211, 211 }," +
		"\n { \"slate grey\",          112, 128, 144 }," +
		"\n { \"slate grey dark\",      47,  79,  79 }," +
		"\n { \"slate grey light\",    119, 136, 153 }," +
		"\n { \"warm grey\",           128, 128, 105 }" +
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

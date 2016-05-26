using UnityEngine;
using System.Collections;

public class TextSetterred : MonoBehaviour {

	string testing = "struct colour redtab[] = {" +
		"\n { \"alizarin crimson\",    227,  38,  54 }," +
		"\n { \"brick\",               156, 102,  31 }," +
		"\n { \"cadmium red deep\",    227,  23,  13 }," +
		"\n { \"coral\",               255, 127,  80 }," +
		"\n { \"coral light\",         240, 128, 128 }," +
		"\n { \"deep pink\",           255,  20, 147 }," +
		"\n { \"english red\",         212,  61,  26 }," +
		"\n { \"firebrick\",           178,  34,  34 }," +
		"\n { \"geranium lake\",       227,  18,  48 }," +
		"\n { \"hot pink\",            255, 105, 180 }," +
		"\n { \"indian red\",          176,  23,  31 }," +
		"\n { \"light salmon\",        255, 160, 122 }," +
		"\n { \"madder lake deep\",    227,  46,  48 }," +
		"\n { \"maroon\",              176,  48,  96 }," +
		"\n { \"pink\",                255, 192, 203 }," +
		"\n { \"pink light\",          255, 182, 193 }," +
		"\n { \"raspberry\",           135,  38,  87 }," +
		"\n { \"red\",                 255,   0,   0 }," +
		"\n { \"rose madder\",         227,  54,  56 }," +
		"\n { \"salmon\",              250, 128, 114 }," +
		"\n { \"tomato\",              255,  99,  71 }," +
		"\n { \"venetian red\",        212,  26,  31 }" +
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

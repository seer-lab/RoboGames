using UnityEngine;
using System.Collections;

public class TextSetteryellow : MonoBehaviour {

	string testing = "struct colour yellowtab[] = {" +
		"\n { \"aureoline yellow\",     255, 168,  36 }," +
		"\n { \"banana\",               227, 207,  87 }," +
		"\n { \"cadmium lemon\",        255, 227,   3 }," +
		"\n { \"cadmium yellow\",       255, 153,  18 }," +
		"\n { \"cadmium yellow light\", 255, 176,  15 }," +
		"\n { \"gold\",                 255, 215,   0 }," +
		"\n { \"goldenrod\",            218, 165,  32 }," +
		"\n { \"goldenrod dark\",       184, 134,  11 }," +
		"\n { \"goldenrod light\",      250, 250, 210 }," +
		"\n { \"goldenrod pale\",       238, 232, 170 }," +
		"\n { \"light goldenrod\",      238, 221, 130 }," +
		"\n { \"melon\",                227, 168, 105 }," +
		"\n { \"naples yellow deep\",   255, 168,  18 }," +
		"\n { \"yellow\",               255, 255,   0 }," +
		"\n { \"yellow light\",         255, 255, 224 " +
		"}\n};";

	// Use this for initialization
	void Start () {
		TextMesh Tm = GetComponent<TextMesh>();
		Tm.text = testing;	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

using UnityEngine;
using System.Collections;

public class TextSettergreen : MonoBehaviour {

	string testing = "struct colour greentab[] = {" +
		"\n { \"chartreuse\",          127, 255,   0 }," +
		"\n { \"chrome oxide green\",  102, 128,  20 }," +
		"\n { \"cinnabar green\",       97, 179,  41 }," +
		"\n { \"cobalt green\",         61, 145,  64 }," +
		"\n { \"emerald green\",         0, 201,  87 }," +
		"\n { \"forest green\",         34, 139,  34 }," +
		"\n { \"green\",                 0, 255,   0 }," +
		"\n { \"green dark\",            0, 100,   0 }," +
		"\n { \"green pale\",          152, 251, 152 }," +
		"\n { \"green yellow\",        173, 255,  47 }," +
		"\n { \"lawn green\",          124, 252,   0 }," +
		"\n { \"lime green\",           50, 205,  50 }," +
		"\n { \"mint\",                189, 252, 201 }," +
		"\n { \"olive\",                59,  94,  43 }," +
		"\n { \"olive drab\",          107, 142,  35 }," +
		"\n { \"olive green dark\",     85, 107,  47 }," +
		"\n { \"permanent green\",      10, 201,  43 }," +
		"\n { \"sap green\",            48, 128,  20 }," +
		"\n { \"sea green\",            46, 139,  87 }," +
		"\n { \"sea green dark\",      143, 188, 143 }," +
		"\n { \"sea green medium\",     60, 179, 113 }," +
		"\n { \"sea green light\",      32, 178, 170 }," +
		"\n { \"spring green\",          0, 255, 127 }," +
		"\n { \"spring green medium\",   0, 250, 154 }," +
		"\n { \"terre verte\",          56,  94,  15 }," +
		"\n { \"viridian light\",      110, 255, 112 }," +
		"\n { \"yellow green\",        154, 205,  50 }," +
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

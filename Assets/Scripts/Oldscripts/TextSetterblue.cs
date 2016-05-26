using UnityEngine;
using System.Collections;

public class TextSetterblue : MonoBehaviour {

	string testing = "struct colour bluetab[] = {" +
		"\n { \"alice blue\",        240, 248, 255 }," +
		"\n { \"blue\",                0,   0, 255 }," +
		"\n { \"blue light\",        173, 216, 230 }," +
		"\n { \"blue medium\",         0,   0, 205 }," +
		"\n { \"cadet\",              95, 158, 160 }," +
		"\n { \"cobalt\",             61,  89, 171 }," +
		"\n { \"cornflower\",        100, 149, 237 }," +
		"\n { \"cerulean\",            5, 184, 204 }," +
		"\n { \"dodger blue\",        30, 144, 255 }," +
		"\n { \"indigo\",              8,  46,  84 }," +
		"\n { \"manganese blue\",      3, 168, 158 }," +
		"\n { \"midnight blue\",      25,  25, 112 }," +
		"\n { \"navy\",                0,   0, 128 }," +
		"\n { \"peacock\",            51, 161, 201 }," +
		"\n { \"powder blue\",       176, 264, 230 }," +
		"\n { \"royal blue\",         65, 105, 225 }," +
		"\n { \"slate blue\",        106,  90, 205 }," +
		"\n { \"slate blue dark\",    72,  61, 139 }," +
		"\n { \"slate blue light\",  132, 112, 255 }," +
		"\n { \"slate blue medium\", 123, 104, 238 }," +
		"\n { \"sky blue\",          135, 206, 235 }," +
		"\n { \"sky blue deep\",       0, 191, 255 }," +
		"\n { \"sky blue light\",    135, 206, 250 }," +
		"\n { \"steel blue\",         70, 130, 180 }," +
		"\n { \"steel blue light\",  176, 196, 222 }," +
		"\n { \"turquoise blue\",      0, 199, 140 }," +
		"\n { \"ultramarine\",        18,  10, 143 }" +
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

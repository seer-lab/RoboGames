using UnityEngine;
using System.Collections;

public class TextSettermagenta : MonoBehaviour {

	string testing = "struct colour magentatab[] = {" +
		"\n { \"blue violet\",          138,  43, 226 }," +
		"\n { \"cobalt violet deep\",   145,  33, 158 }," +
		"\n { \"magenta\",              255,   0, 255 }," +
		"\n { \"orchid\",               218, 112, 214 }," +
		"\n { \"orchid dark\",          153,  50, 204 }," +
		"\n { \"orchid medium\",        186,  85, 211 }," +
		"\n { \"permanent red violet\", 219,  38,  69 }," +
		"\n { \"plum\",                 221, 160, 221 }," +
		"\n { \"purple\",               160,  32, 240 }," +
		"\n { \"purple medium\",        147, 112, 219 }," +
		"\n { \"ultramarine violet\",    92,  36, 110 }," +
		"\n { \"violet\",               143,  94, 153 }," +
		"\n { \"violet dark\",          148,   0, 211 }," +
		"\n { \"violet red\",           208,  32, 144 }," +
		"\n { \"violet red medium\",    199,  21, 133 }," +
		"\n { \"violet red pale\",      219, 112, 147 }" +
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

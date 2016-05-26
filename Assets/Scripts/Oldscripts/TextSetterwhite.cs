using UnityEngine;
using System.Collections;

public class TextSetterwhite : MonoBehaviour {

	string testing = "struct colour whitetab[] = {" +
		"\n { \"antique white\",       250, 235, 215 }," +
		"\n { \"azure\",               240, 255, 255 }," +
		"\n { \"bisque\",              255, 228, 196 }," +
		"\n { \"blanched almond\",     255, 235, 205 }," +
		"\n { \"cornsilk\",            255, 248, 220 }," +
		"\n { \"eggshell\",            252, 230, 201 }," +
		"\n { \"floral white\",        255, 250, 240 }," +
		"\n { \"gainsboro\",           220, 220, 220 }," +
		"\n { \"ghost white\",         248, 248, 255 }," +
		"\n { \"honeydew\",            240, 255, 240 }," +
		"\n { \"ivory\",               255, 255, 240 }," +
		"\n { \"lavender\",            230, 230, 250 }," +
		"\n { \"lavender blush\",      255, 240, 245 }," +
		"\n { \"lemon chiffon\",       255, 250, 205 }," +
		"\n { \"linen\",               250, 240, 230 }," +
		"\n { \"mint cream\",          245, 255, 250 }," +
		"\n { \"misty rose\",          255, 228, 225 }," +
		"\n { \"moccasin\",            255, 228, 181 }," +
		"\n { \"navajo white\",        255, 222, 173 }," +
		"\n { \"old lace\",            253, 245, 230 }," +
		"\n { \"papaya whip\",         255, 239, 213 }," +
		"\n { \"peach puff\",          255, 218, 185 }," +
		"\n { \"seashell\",            255, 245, 238 }," +
		"\n { \"snow\",                255, 250, 250 }," +
		"\n { \"thistle\",             216, 191, 216 }," +
		"\n { \"titanium white\",      252, 255, 240 }," +
		"\n { \"wheat\",               245, 222, 179 }," +
		"\n { \"white\",               255, 255, 255 }," +
		"\n { \"white smoke\",         245, 245, 245 }," +
		"\n { \"zinc white\",          253, 248, 255 }" +
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

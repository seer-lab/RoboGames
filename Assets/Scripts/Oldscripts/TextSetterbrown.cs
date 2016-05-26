using UnityEngine;
using System.Collections;

public class TextSetterbrown : MonoBehaviour {

	string testing = "struct colour browntab[] = {" +
		"\n { \"beige\",               163, 148, 128 }," +
		"\n { \"brown\",               128,  42,  42 }," +
		"\n { \"brown madder\",        219,  41,  41 }," +
		"\n { \"brown ochre\",         135,  66,  31 }," +
		"\n { \"burlywood\",           222, 184, 135 }," +
		"\n { \"burnt sienna\",        138,  54,  15 }," +
		"\n { \"burnt umber\",         138,  51,  36 }," +
		"\n { \"chocolate\",           210, 105,  30 }," +
		"\n { \"deep ochre\",          115,  61,  26 }," +
		"\n { \"flesh\",               255, 125,  64 }," +
		"\n { \"flesh ochre\",         255,  87,  33 }," +
		"\n { \"gold ochre\",          199, 120,  38 }," +
		"\n { \"greenish umber\",      255,  61,  13 }," +
		"\n { \"khaki\",               240, 230, 140 }," +
		"\n { \"khaki dark\",          189, 183, 107 }," +
		"\n { \"light beige\",         245, 245, 220 }," +
		"\n { \"peru\",                205, 133,  63 }," +
		"\n { \"rosy brown\",          188, 143, 143 }," +
		"\n { \"raw sienna\",          199,  97,  20 }," +
		"\n { \"raw umber\",           115,  74,  18 }," +
		"\n { \"sepia\",                94,  38,  18 }," +
		"\n { \"sienna\",              160,  82,  45 }," +
		"\n { \"saddle brown\",        139,  69,  19 }," +
		"\n { \"sandy brown\",         244, 164,  96 }," +
		"\n { \"tan\",                 210, 180, 140 }," +
		"\n { \"van dyke brown\",       94,  38,   5 }" +
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

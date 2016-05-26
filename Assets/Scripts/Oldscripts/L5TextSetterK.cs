using UnityEngine;
using System.Collections;

public class L5TextSetterK : MonoBehaviour {

	string main = "" +
		"\n" +
		"\n" +
		"\n" +
		"\n" +
		"\n" +
		"\n#include " +
		"\n#include" +
		"\n#include" +
		"\nusing namespace" +
		"\nstruct " +
			"\n    char " +
			"\n    int " +
			"\n    int " +
			"\n    int " +
		"\n" +
		"\nstruct              getNearbyObjects();" +
		"\n" +
		"\n" +
		"\n" +
		"\n" +
		"\n" +
		"\nvoid" +
			"\n    enum " +
			"\n                " +
			"\n                " +
			"\n    enum " +
			"\n                " +
			"\n                 " +
			"\n    struct" +
			"\n    int" + //\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n
			"\n    for" +
		"\n     " +
		"\n     " +
		"\n     "+
			"\n        for" +
			"\n       " +
			"\n       " +
			"\n      " +
//		"\n       brighter = Compare(red1,green1,blue1,red2,green2,blue2);" +
		"\n\n    " +
		"\n   " +
		"\n  " +
		"\n " +
		"\n" +
		"\n" +
		"\n";

	// Use this for initialization
	void Start () {
		TextMesh Tm = GetComponent<TextMesh>();
		Tm.text = main;
		Tm.color = new Color(61f/255f, 189f/255f, 232f/255f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

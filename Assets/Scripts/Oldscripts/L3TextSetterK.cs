using UnityEngine;
using System.Collections;

public class L3TextSetterK : MonoBehaviour {

	string printing = "" +
		"\n" +
			"\n" +
			"\n#include " + 
			"\n#include " + 
			"\n#include  " + 
			"\n#include " +
			"\nint []            int              int                  " +
			"\n	" +
			"\n	" +
			"\n    int " + 
			"\n\n    while  " +
			"\n\n        while  " +
			"\n\n            if  " + 
			"\n\n                swap " +
			"\n\n     " + 
			"\n\n	 " +
			"\n\n" +
			"\n\n    return " +
			"\n  ";

	// Use this for initialization
	void Start () {
		TextMesh Tm = GetComponent<TextMesh>();
		Tm.text = printing;	
		Tm.color = new Color(61f/255f, 189f/255f, 232f/255f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

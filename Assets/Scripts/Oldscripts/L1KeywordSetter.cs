using UnityEngine;
using System.Collections;

public class L1KeywordSetter : MonoBehaviour {

	string tracing = "" +
			"\n " +
			"\n" +
			"\n	" +
			"\n#include<stdio.h>  " +
			"\n#include<conio.h>  " +
			"\nfloat          int          int " +
			"\n  " +
			"\n        int " +
			"\n        float " +
			"\n    " +
			"\n        for " +
			"\n    " +
			"\n		" +
			"\n	                    float        " +
			"\n       " +
			"\n		       " +
			"\n        return      " +
			"\n      " +
			"\n " +
			"\n" +
			"\n";

	// Use this for initialization
	void Start () {
		TextMesh Tm = GetComponent<TextMesh>();
		Tm.text = tracing;	
		Tm.color = new Color(61f/255f, 189f/255f, 232f/255f);

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

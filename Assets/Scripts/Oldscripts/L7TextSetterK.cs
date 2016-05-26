using UnityEngine;
using System.Collections;

public class L7TextSetterK : MonoBehaviour {

	string main = "" +
		"\n " +
		"\n" +
		"\n"+
		"\n#include " +
		"\n#include" + "\n"+
	//	"\n#include <namedcolors.h>" +
		"\n" +
		"\nusing namespace" +
		"\n" +
		"\nstruct" +
		"\n char" +
		"\n int " +
		"\n int " +
		"\n int " +
		"\n" +
		"\n" +
		"\n" +
		"\n" +
		"\nstruct part" +
		"\n" +
		"\n" +
		"\nvoid " +
		"\n  enum part " +
		"\n                 " +
		"\n                " +
		"\n  enum part " +
		"\n                 " +
		"\n                 " +
		"\n  struct part " +
		"\n int " + //\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n
		"\n   for                                          length()" +
		"\n     " +
		"\n     " +
		"\n     "+
		"\n     for" +
		"\n       " +
		"\n       " +
		"\n       " +
		"\n                         Compare" +
		"\n       " +
		"\n    " + "" +
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
		Tm.color = Color.blue;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

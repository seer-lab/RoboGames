using UnityEngine;
using System.Collections;

public class L7TextSetter : MonoBehaviour {

	string main = "" +
		"\n " +
		"\n" +
		"\n"+
			"\n                <stdio.h>" +
			"\n                <robotmain.h>" + "\n"+
	//	"\n#include <namedcolors.h>" +
		"\n" +
			"\n                                robotmain;" +
		"\n" +
			"\n             part {" +
			"\n          name[28];" +
		"\n        power;" +
			"\n        cond;t" +
			"\n        effic; " +
		"\n};" +
		"\n" +
		"\n" +
		"\n" +
			"\n                  *p = getAllParts();" +
		"\n" +
		"\n" +
		"\n         diagnosticComparision() {" +
			"\n                       { p[0], p[1], p[2], p[3], p[4], p[5], p[6]," +
		"\n                 p[7], p[8], p[9], p[10], p[11], p[12]," +
		"\n                 p[13], p[14], p[15] } part1;" +
			"\n                       { p[0], p[1], p[2], p[3], p[4], p[5], p[6]," +
		"\n                 p[7], p[8], p[9], p[10], p[11], p[12]," +
		"\n                 p[13], p[14], p[15] } part2;" +
			"\n                        *similar;" +
		"\n        power1 = 0, cond1 = 0, effic1 = 0, power2 = 0, cond2 = 0, effic3 = 0;" + //\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n
		"\n         (part1 = p[0]; part1 < p.                ;) {" +
		"\n     power1   = part1.power;" +
		"\n     cond1 = part1.cond;" +
		"\n     effic1  = part1.effic;"+
		"\n           (part2 = p[15]; part2 > part1;) {" +
		"\n       power2   = part2.power;" +
		"\n       cond2 = part2.cond;" +
		"\n       effic2  = part2.effic;" +
		"\n       similar =                     (part1,part2,similar);" +
		"\n       part2--;" +
		"\n    }" + "part1++;" +
		"\n   }" +
		"\n  }" +
		"\n }" +
		"\n" +
		"\n" +
		"\n}";

	// Use this for initialization
	void Start () {
		TextMesh Tm = GetComponent<TextMesh>();
		Tm.text = main;	
		Tm.color = Color.black;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

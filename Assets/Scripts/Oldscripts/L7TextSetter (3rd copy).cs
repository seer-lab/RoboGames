using UnityEngine;
using System.Collections;

public class L7TextSetterD : MonoBehaviour {

	string main = "//This function is set up to prioritize and compare robot" +
		"\n//parts and systems. In particular, this function will identify " +
		"\n//which part/system is most similar to each given system in the " +
		"\n//three areas of power level, condition, and efficiency"+
		"\n#include <stdio.h>" +
		"\n#include <robotmain.h>" + "\n"+
	//	"\n#include <namedcolors.h>" +
		"\n" +
		"\nusing namespace robotmain;" +
		"\n" +
		"\nstruct part {" +
		"\n char name[28];" +
		"\n int power; //current power level of part" +
		"\n int cond; //condition of part" +
		"\n int effic; //performance efficiency of part " +
		"\n};" +
		"\n" +
		"\n//TEST CASE SCENARIO" +
		"\n//RUN CALCULATION OVER ALL PARTS" +
		"\nstruct part *p = getAllParts();" +
		"\n" +
		"\n" +
		"\nvoid diagnosticComparision(void) {" +
		"\n  enum part { p[0], p[1], p[2], p[3], p[4], p[5], p[6]," +
		"\n                 p[7], p[8], p[9], p[10], p[11], p[12]," +
		"\n                 p[13], p[14], p[15] } part1;" +
		"\n  enum part { p[0], p[1], p[2], p[3], p[4], p[5], p[6]," +
		"\n                 p[7], p[8], p[9], p[10], p[11], p[12]," +
		"\n                 p[13], p[14], p[15] } part2;" +
		"\n  struct part *similar;" +
		"\n int power1 = 0, cond1 = 0, effic1 = 0, power2 = 0, cond2 = 0, effic3 = 0;" + //\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n
		"\n   for(part1 = p[0]; part1 < p.length();) {" +
		"\n     power1   = part1.power;" +
		"\n     cond1 = part1.cond;" +
		"\n     effic1  = part1.effic;"+
		"\n     for(part2 = p[15]; part2 > part1;) {" +
		"\n       power2   = part2.power;" +
		"\n       cond2 = part2.cond;" +
		"\n       effic2  = part2.effic;" +
		"\n       similar = Compare(part1,part2,similar);" +
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
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

using UnityEngine;
using System.Collections;

public class L7TextSetterC : MonoBehaviour {

	string main = "//This function is set up to prioritize and compare robot" +
		"\n//parts and systems. In particular, this function will identify " +
		"\n//which part/system is most similar to each given system in the " +
		"\n//three areas of power level, condition, and efficiency"+
		"\n" +
		"\n" + "\n"+
	//	"\n#include <namedcolors.h>" +
		"\n" +
		"\n" +
		"\n" +
		"\n" +
		"\n" +
		"\n                           //current power level of part" +
		"\n                           //condition of part" +
		"\n                           //performance efficiency of part " +
		"\n" +
		"\n" +
		"\n//TEST CASE SCENARIO" +
		"\n//RUN CALCULATION OVER ALL PARTS";

	// Use this for initialization
	void Start () {
		TextMesh Tm = GetComponent<TextMesh>();
		Tm.text = main;	
		Tm.color = Color.grey;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

using UnityEngine;
using System.Collections;

public class L5TextSetterD : MonoBehaviour {

	string main = "//Robot needs to interpret surrounds appropriately" +
		"\n//this code will analyze the coordinates of nearby objects " +
		"\n//and calculate the closest of each pair of objects to itself" +
		"\n//NOTE: Robot is located at coordinates (0,0,0)" +
		"\n//where (x,y,z) represents location in 3-D Space" +
		"\n" +
		"\n#include <stdio.h>" +
		"\n#include <robotmanager.h>" +
		"\n#include <environment.h>" +
		"\nusing namespace robotmanager" +
		"\nstruct object {" +
		"\n char name[28];" +
		"\n int x;" +
		"\n int y;" +
		"\n int z;" +
		"\n};" +
		"\nstruct object *nO = getNearbyObjects();" +
		"\n" +
		"\n//TEST CASE SCENARIO" +
		"\n//RUN CALCULATION OVER ALL OBJECTS" +
		"\n//IN DEMONSTRATION AREA" +
		"\n" +
		"\nvoid identifyClosest(void) {" +
			"\n  enum objects { nO[0], nO[1], nO[2], nO[3], nO[4], nO[5], nO[6]," +
			"\n                 nO[7], nO[8], nO[9], nO[10], nO[11], nO[12]," +
			"\n                 nO[13], nO[14], nO[15] } object1;" +
			"\n  enum objects { nO[0], nO[1], nO[2], nO[3], nO[4], nO[5], nO[6]," +
			"\n                 nO[7], nO[8], nO[9], nO[10], nO[11], nO[12]," +
			"\n                 nO[13], nO[14], nO[15] } object2;" +
		"\n  struct object closer;" +
			"\n int x1 = 0, y1 = 0, z1 = 0, x2 = 0, y2 = 0, z2 = 0;" + //\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n
		"\n   for(object1 = nO[0]; object1 < nO.length(); object1++) {" +
		"\n     x1   = object1.x;" +
		"\n     y1 = object1.y;" +
		"\n     z1  = object1.z;"+
		"\n     for(object2 = nO[15]; object2 > object1; object2--) {" +
			"\n       x2   = object2.x;" +
			"\n       y2 = object2.y;" +
			"\n       z2  = object2.z;" +
//		"\n       brighter = Compare(red1,green1,blue1,red2,green2,blue2);" +
		"\n\n    }" +
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

using UnityEngine;
using System.Collections;

public class L5TextSetter : MonoBehaviour {

	string main = "" +
		"\n" +
		"\n" +
		"\n" +
		"\n" +
		"\n" +
			"\n         <stdio.h>" +
			"\n         <robotmanager.h>" +
			"\n         <environment.h>" +
		"\n                robotmanager" +
		"\n       object {" +
			"\n         name[28];" +
			"\n        x;" +
			"\n        y;" +
			"\n        z;" +
		"\n};" +
		"\n       object *nO = " +
		"\n" +
		"\n" +
		"\n" +
		"\n" +
		"\n" +
		"\n     identifyClosest() {" +
			"\n         objects { nO[0], nO[1], nO[2], nO[3], nO[4]," +
			"\n    nO[5], nO[6], nO[7], nO[8], nO[9], nO[10], nO[11],," +
			"\n    nO[12], nO[13], nO[14], nO[15] } object1;" +
			"\n         objects { nO[0], nO[1], nO[2], nO[3], nO[4]," +
			"\n    nO[5], nO[6], nO[7], nO[8], nO[9], nO[10], nO[11]," +
			"\n    nO[12], nO[13], nO[14], nO[15] } object2;" +
			"\n           object closer;" +
			"\n        x1 = 0, y1 = 0, z1 = 0, x2 = 0, y2 = 0, z2 = 0;" + //\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n
			"\n       (object1 = nO[0]; object1 < nO.length(); object1++) {" +
			"\n        x1 = object1.x;" +
			"\n        y1 = object1.y;" +
			"\n        z1 = object1.z;"+
			"\n           (object2 = nO[15]; object2 > object1; object2--) {" +
			"\n                x2 = object2.x;" +
			"\n                y2 = object2.y;" +
			"\n                z2  = object2.z;" +
//		"\n       brighter = Compare(red1,green1,blue1,red2,green2,blue2);" +
//			"\n\n                        }" +
			"\n\n                }" +
			"\n        }" +
		"\n}" +
		"\n" +
		"\n" +
		"\n";

	// Use this for initialization
	void Start () {
		TextMesh Tm = GetComponent<TextMesh>();
		Tm.text = main;	
		Tm.color = Color.white;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

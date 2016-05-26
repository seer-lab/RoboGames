using UnityEngine;
using System.Collections;

public class l3mintextsetter : MonoBehaviour {

	string printing = "\n                <stdio.h>  " +
		"\n                <conio.h>" + 
			"\n                <swapvalues.h>" + 
			"\n                <arraysort.h>" + 
			"\n                <numberorder.h>" + 
			"\n        i = 0;" + 
			"\n        minimum(       num1,        num2)" + 
			"\n{" +
			"\n             (i){" + 
			"\n           0:" + 
			"\n                 min(num1, num2);" + 
			"\n" + 
			"\n           1:" + 
			"\n                  min(num1, num2);" + 
			"\n   " + 
			"\n           2:" + 
			"\n                  min(num1, num2);" + 
			"\n   " + 
			"\n           3:" + 
			"\n                  min(num1, num2);" + 
			"\n   " + 
			"\n           4:" + 
			"\n                  min(num1, num2);" + 
			"\n   " + 
			"\n           5:" + 
			"\n                  min(num1, num2);" + 
			"\n   " + 
			"\n           6:" + 
			"\n                  min(num1, num2);" + 
			"\n   " + 
			"\n}"+
			"\n}\n\n\n\n" +
			"\n        maximum(       num1,        num2)" + 
			"\n{" +
			"\n             (i){" + 
			"\n           0:" + 
			"\n                  max(num1, num2);" + 
			"\n   " + 
			"\n           1:" + 
			"\n                  max(num1, num2);" + 
			"\n   " + 
			"\n           2:" + 
			"\n                  max(num1, num2);" + 
			"\n   " + 
			"\n           3:" + 
			"\n                  max(num1, num2);" + 
			"\n   " + 
			"\n           4:" + 
			"\n                  max(num1, num2);" + 
			"\n   " + 
			"\n           5:" + 
			"\n                  max(num1, num2);" + 
			"\n   " + 
			"\n           6:" + 
			"\n                  max(num1, num2);" + 
			"\n   " + 
			"\n}"+
			"\n}";
	
	// Use this for initialization
	void Start () {
		TextMesh Tm = GetComponent<TextMesh>();
		Tm.text = printing;	
		Tm.color = Color.black;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

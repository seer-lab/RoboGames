using UnityEngine;
using System.Collections;

public class L3TextSetter : MonoBehaviour {

	string printing = "" +
		"\n" +
			"\n" +
			"\n         <algorithm.h> " + 
			"\n         <conio.h> " + 
			"\n         <stdio.h> " + 
			"\n         <iostream.h> " +
			"\n       Prioritize(    priorities[],    numOfThreats){ " +
			"\n" +
			"\n" +
			"\n        i=1,j=1;" + 
			"\n\n          (i<numOfThreats){ " +
			"\n\n	             (j<numOfThreats){ " +
			"\n\n               (priorities[i]>priorities[j]){ " + 
			"\n\n		                    (priorities[i],priorities[j]); " +
			"\n\n             } j++;" + 
			"\n\n         } i++; j=1;" +
			"\n\n     } " +
			"\n\n           priorities;" +
			"\n}  ";

	// Use this for initialization
	void Start () {
		TextMesh Tm = GetComponent<TextMesh>();
		Tm.text = printing;	
		Tm.color = Color.white;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

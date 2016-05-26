using UnityEngine;
using System.Collections;

public class L3TextSetterD : MonoBehaviour {

	string printing = "//Prioritize external threats by ordering them based" +
		"\n//on their threat levels from most dangerous" +
			"\n//(highest) to least dangerous (lowest)." +
			"\n#include <algorithm.h> " + 
			"\n#include <conio.h> " + 
			"\n#include <stdio.h> " + 
			"\n#include <iostream.h> " +
			"\nint[] Prioritize(int priorities[], int numOfSystems){ " +
			"\n		//test using pre-chosen values for systems " +
			"\n		//int priorities[] = [1,3,0,4,2]; int numOfSystems = 5;" +
			"\n		int i,j=1;" + 
			"\n\n	while (i<numOfSystems){ " +
			"\n\n		while (j<numOfSystems){ " +
			"\n\n			if (priorities[i]>priorities[j]){ " + 
			"\n\n				swap(priorities[i],priorities[j]); " +
			"\n\n       	} j++;" + 
			"\n\n		} i++; " +
			"\n\n	} " +
			"\n\n    return priorities;" +
			"\n}  ";

	// Use this for initialization
	void Start () {
		TextMesh Tm = GetComponent<TextMesh>();
		Tm.text = printing;	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

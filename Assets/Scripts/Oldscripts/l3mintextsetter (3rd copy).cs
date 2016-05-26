using UnityEngine;
using System.Collections;

public class l3mintextsetterD : MonoBehaviour {

	string printing = "\n#include<stdio.h>  " +
		"\n#include<conio.h>" + 
			"\n#include<swapvalues.h>" + 
			"\n#include<arraysort.h>" + 
			"\n#include<numberorder.h>" + 
			"\n int i = 0;" + 
			"\n int minimum(int num1, int num2)" + 
			"\n{" +
			"\n switch(i){" + 
			"\n case 0:" + 
			"\n   return min(num1, num2);" + 
			"\n   break;" + 
			"\n case 1:" + 
			"\n   return min(num1, num2);" + 
			"\n   break;" + 
			"\n case 2:" + 
			"\n   return min(num1, num2);" + 
			"\n   break;" + 
			"\n case 3:" + 
			"\n   return min(num1, num2);" + 
			"\n   break;" + 
			"\n case 4:" + 
			"\n   return min(num1, num2);" + 
			"\n   break;" + 
			"\n case 5:" + 
			"\n   return min(num1, num2);" + 
			"\n   break;" + 
			"\n case 6:" + 
			"\n   return min(num1, num2);" + 
			"\n   break;" + 
			"\n}"+
			"\n}\n\n\n\n" +
			"\n int maximum(int num1, int num2)" + 
			"\n{" +
			"\n switch(i){" + 
			"\n case 0:" + 
			"\n   return max(num1, num2);" + 
			"\n   break;" + 
			"\n case 1:" + 
			"\n   return max(num1, num2);" + 
			"\n   break;" + 
			"\n case 2:" + 
			"\n   return max(num1, num2);" + 
			"\n   break;" + 
			"\n case 3:" + 
			"\n   return max(num1, num2);" + 
			"\n   break;" + 
			"\n case 4:" + 
			"\n   return max(num1, num2);" + 
			"\n   break;" + 
			"\n case 5:" + 
			"\n   return max(num1, num2);" + 
			"\n   break;" + 
			"\n case 6:" + 
			"\n   return max(num1, num2);" + 
			"\n   break;" + 
			"\n}"+
			"\n}";
	
	// Use this for initialization
	void Start () {
		TextMesh Tm = GetComponent<TextMesh>();
		Tm.text = printing;	
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

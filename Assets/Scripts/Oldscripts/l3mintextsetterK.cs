using UnityEngine;
using System.Collections;

public class l3mintextsetterK : MonoBehaviour {

	string printing = "\n#include" +
		"\n#include" + 
			"\n#include" + 
			"\n#include" + 
			"\n#include" + 
			"\n int" + 
			"\n int                     int              int" + 
			"\n" +
			"\n switch" + 
			"\n case" + 
			"\n   return " + 
			"\n   break;" + 
			"\n case" + 
			"\n   return" + 
			"\n   break;" + 
			"\n case" + 
			"\n   return" + 
			"\n   break;" + 
			"\n case" + 
			"\n   return" + 
			"\n   break;" + 
			"\n case" + 
			"\n   return" + 
			"\n   break;" + 
			"\n case" + 
			"\n   return" + 
			"\n   break;" + 
			"\n case" + 
			"\n   return" + 
			"\n   break;" + 
			"\n"+
			"\n\n\n\n\n" +
			"\n int                    int             int" + 
			"\n" +
			"\n switch" + 
			"\n case " + 
			"\n   return" + 
			"\n   break;" + 
			"\n case " + 
			"\n   return" + 
			"\n   break;" + 
			"\n case " + 
			"\n   return" + 
			"\n   break;" + 
			"\n case " + 
			"\n   return" + 
			"\n   break;" + 
			"\n case " + 
			"\n   return" + 
			"\n   break;" + 
			"\n case " + 
			"\n   return" + 
			"\n   break;" + 
			"\n case " + 
			"\n   return" + 
			"\n   break;" + 
			"\n"+
			"\n";
	
	// Use this for initialization
	void Start () {
		TextMesh Tm = GetComponent<TextMesh>();
		Tm.text = printing;	
		Tm.color = Color.blue;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

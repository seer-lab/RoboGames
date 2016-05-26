using UnityEngine;
using System.Collections;

public class L1TextSetter : MonoBehaviour {

	string tracing = "" +
			"\n " +
			"\n" +
			"\n	" +
			"\n " +
			"\n " +
			"\n      avgForce(    forces[],    numOfForces) " +
			"\n{  " +
			"\n            sum, i=0; " +
			"\n              avgf; " +
			"\n    " +
			"\n            (i=0;i<numOfForces;i++) " +
			"\n        { " +
			"\n                sum=sum+forces[i];" +
			"\n	               avgf=     (sum/numOfForces);" +
			"\n        }  " +
			"\n        avgf++;" +
			"\n               avgf;" +
			"\n        } " +
			"\n} " +
			"\n" +
			"\n";

	// Use this for initialization
	void Start () {
		TextMesh Tm = GetComponent<TextMesh>();
		Tm.text = tracing;	
		Tm.color = Color.white;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

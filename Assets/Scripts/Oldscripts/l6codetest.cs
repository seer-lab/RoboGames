using UnityEngine;
using System.Collections;

public class l6codetest : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		TextMesh Tm = GetComponent<TextMesh>();
		Tm.text = "" +
						"\n" +
						"\n" +
						"\n               PassRequirements (           levels);" +
						"\n" +
				"\n               FailRequirements (           levels);" +
						"\n" +
				"\n              PassTest (){" +
						"\n	       levels[] = " +
						"\n" +
				//		"\n	return sumCalc(values[]) == total;" +
						"\n" +
						"\n}" +
						"\n\n\n" +
				"\n               FailTest ();{" +
						"\n	       levels[] = " +
						"\n" +
				//		"\n	return medianCalc(values[]) == mid;" +
						"\n" +
						"\n}";
		Tm.color = Color.black;
	}
}

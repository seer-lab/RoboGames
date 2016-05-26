using UnityEngine;
using System.Collections;

public class l6codetestD : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		TextMesh Tm = GetComponent<TextMesh>();
		Tm.text = "//These functions test the parts and systems of the robot  " +
						"\n//Values are in the form (P,C,E) " +
						"\n//where P = Power, C = [Physical] Condition, E = Efficiency" +
						"\nstring[] PassRequirements (int[] levels);" +
						"\n//this function returns all parts/systems that meet the input levels" +
						"\nstring[] FailRequirements (int[] levels);" +
						"\n//this function returns all parts/systems that fail to meet the input levels" +
						"\nstring[] PassTest (){" +
						"\n	int levels[] = " +
						"\n" +
				//		"\n	return sumCalc(values[]) == total;" +
						"\n" +
						"\n}" +
						"\n\n\n" +
						"\nstring[] FailTest ();{" +
						"\n	int levels[] = " +
						"\n" +
				//		"\n	return medianCalc(values[]) == mid;" +
						"\n" +
						"\n}";
	}
}

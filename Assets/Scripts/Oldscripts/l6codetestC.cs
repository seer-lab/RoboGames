using UnityEngine;
using System.Collections;

public class l6codetestC : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		TextMesh Tm = GetComponent<TextMesh>();
		Tm.text = "//These functions test the parts and systems of the robot  " +
						"\n//Values are in the form (P,C,E) " +
						"\n//where P = Power, C = [Physical] Condition, E = Efficiency" +
						"\n" +
						"\n//this function returns all parts/systems that meet the input levels" +
						"\n" +
						"\n//this function returns all parts/systems that fail to meet the input levels";
		Tm.color = Color.grey;
	}
}

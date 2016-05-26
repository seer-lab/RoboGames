using UnityEngine;
using System.Collections;

public class L5TextSetterC : MonoBehaviour {

	string main = "//Robot needs to interpret surroundings appropriately" +
		"\n//this code will analyze the coordinates of nearby objects " +
		"\n//and calculate the closest of each pair of objects to itself" +
		"\n//NOTE: Robot is located at coordinates (0,0,0)" +
		"\n//where (x,y,z) represents location in 3-D Space" +
		"\n" +
		"\n" +
		"\n" +
		"\n" +
		"\n" +
		"\n" +
		"\n" +
		"\n" +
		"\n" +
		"\n" +
		"\n" +
		"\n" +
		"\n" +
		"\n//TEST CASE SCENARIO" +
		"\n//RUN CALCULATION OVER ALL OBJECTS" +
		"\n//IN DEMONSTRATION AREA";

	// Use this for initialization
	void Start () {
		TextMesh Tm = GetComponent<TextMesh>();
		Tm.text = main;	
		Tm.color = new Color (166f/255f,226f/255f,46f/255f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

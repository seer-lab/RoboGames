using UnityEngine;
using System.Collections;

public class L3TextSetterC : MonoBehaviour {

	string printing = "//Prioritize external threats by ordering them based" +
		"\n//on their threat levels from most dangerous" +
			"\n//(highest) to least dangerous (lowest)." +
			"\n " + 
			"\n" + 
			"\n" + 
			"\n" +
			"\n " +
			"\n    //test using pre-chosen values for systems " +
			"\n    //int priorities[] = [1,3,0,4,2]; int numOfThreats = 5;";


	// Use this for initialization
	void Start () {
		TextMesh Tm = GetComponent<TextMesh>();
		Tm.text = printing;	
		Tm.color = new Color (166f/255f,226f/255f,46f/255f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

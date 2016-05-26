using UnityEngine;
using System.Collections;

public class l6dtextsetter : MonoBehaviour {
	
	string testing = "//Diagnostic of Robot Parts/Systems"+
			"\nstruct part parts[] = {" +
			"\n {\"Senses\",100,100,100}," +
		//	"{IO/Communications,0,0,0}," +
			"\n\n{\"MotorControl\",25,85,35}," +
			"\n{\"CPU\",50,100,100}," +
			"\n{\"Memory\",100,100,100}," +
			"\n{\"EnergyManagement\",10,100,100}," +
			"\n{\"Head\",100,100,75}," +
			"\n{\"RightHand\",50,95,40}," +
			"\n{\"LeftHand\",100,100,80}," +
			"\n{\"RightArm\",50,50,70}," +
			"\n{\"LeftArm\",30,50,60}," +
			"\n{\"RightLeg\",100,90,80}," +
			"\n{\"LeftLeg\",75,75,75}," +
			"\n{\"RightFoot\",25,25,50}," +
			"\n{\"LeftFoot\",100,50,100}," +
			"\n{\"Torso\",50,100,100}\n}";
	
	// Use this for initialization
	void Start () {
		TextMesh Tm = GetComponent<TextMesh>();
		Tm.text = testing;
		Tm.color = Color.black;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

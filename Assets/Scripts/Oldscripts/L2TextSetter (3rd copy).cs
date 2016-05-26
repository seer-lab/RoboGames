using UnityEngine;
using System.Collections;

public class L2TextSetterD : MonoBehaviour {
	
	string testing = "int WeightCalc (int weights[]);" +
		"\n//Calculate the robot's weight based on the weight of its parts" +
			"\nint EnergyCalc (int powers[]);" +
			"\n//Calculate the robot's energy level based \n//on the median power level of its parts" +
			"\nfloat TempCalc (float temps[]);" +
			"\n//Calculate the robot's temperature based \n//on the average temperature of its parts" +
			"" +
			"\nbool WeightCalcTest (){" +
			"\n	int weights[] = " +
			"\n	int totalweight = " +
			//		"\n	return sumCalc(values[]) == total;" +
			"\n" +
			"\n}" +
			"\n" +
			"\nint EnergyCalc ();{" +
			"\n	int powers[] = " +
			"\n	int powerlevel = " +
			//		"\n	return medianCalc(values[]) == mid;" +
			"\n" +
			"\n}" +
			"\nfloat TempCalcTest ();{" +
			"\n	float temps[] = " +
			"\n	float averageTemp = " +
			//		"\n	return avgCalc(values[]) == average;" +
			"\n" +
			"\n}" +
			"\n";
	
	// Use this for initialization
	void Start () {
		TextMesh Tm = GetComponent<TextMesh>();
		Tm.text = testing;	
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

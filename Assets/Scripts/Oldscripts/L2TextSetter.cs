using UnityEngine;
using System.Collections;

public class L2TextSetter : MonoBehaviour {
	
	string testing = "      DistanceCalc(      distances[]);" +
		"\n" +
	//		"\n      testValues [] = {\n};" + "\n      expectedResult = \n" +
			"\n     DistanceCalcTest (){" +
			"\n          distanceValues[] = " +
			"\n	         expectedTotal = " +
			//		"\n	return sumCalc(values[]) == total;" +
			"\n" +
			"\n}" +
			"\n\n      EnergyCalc(      powers[]);" +
			"\n\n" +
			"\n     EnergyCalcTest (){" +
			"\n          powerValues[] = " +
			"\n	         expectedMedian = " +
			//		"\n	return medianCalc(values[]) == mid;" +
			"\n" +
			"\n}\n\n      TempCalc(      temps[]);" +
			"\n\n" +
			"\n     TempCalcTest (){" +
			"\n          tempValues[] = " +
			"\n	         expectedAverage = " +
			//		"\n	return avgCalc(values[]) == average;" +
			"\n" +
			"\n}" +
			"\n";
	
	// Use this for initialization
	void Start () {
		TextMesh Tm = GetComponent<TextMesh>();
		Tm.text = testing;	
		Tm.color = Color.white;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

using UnityEngine;
using System.Collections;

public class L2TextSetterK : MonoBehaviour {
	
	string testing = "float              float" +
		"\n" +
	//		"\nfloat\n\nfloat\n" +
			"\nbool " +
			"\n    float" +
			"\n    float" +
			//		"\n	return sumCalc(values[]) == total;" +
			"\n" +
			"\n" +
			"\n\nfloat            float" +
			"\n\n" +
			"\nbool " +
			"\n    float " +
			"\n    float " +
			//		"\n	return medianCalc(values[]) == mid;" +
			"\n" +
			"\n\n\nfloat          float" +
			"\n\n" +
			"\nbool " +
			"\n    float" +
			"\n    float" +
			//		"\n	return avgCalc(values[]) == average;" +
			"\n" +
			"\n" +
			"\n";
	
	// Use this for initialization
	void Start () {
		TextMesh Tm = GetComponent<TextMesh>();
		Tm.text = testing;	
		Tm.color = new Color(61f/255f, 189f/255f, 232f/255f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

using UnityEngine;
using System.Collections;

public class l6codetestK : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		TextMesh Tm = GetComponent<TextMesh>();
		Tm.text = "" +
						"\n " +
						"\n" +
						"\nstring[]                                        int[]" +
						"\n" +
						"\nstring[]                                      int[]" +
						"\n" +
						"\nstring[]" +
						"\n	int" +
						"\n" +
				//		"\n	return sumCalc(values[]) == total;" +
						"\n" +
						"\n" +
						"\n\n\n" +
						"\nstring[] " +
						"\n	int " +
						"\n" +
				//		"\n	return medianCalc(values[]) == mid;" +
						"\n" +
						"\n";
		Tm.color = Color.blue;
	}
}

using UnityEngine;
using System.Collections;

public class L5ReportTextSetterK : MonoBehaviour {

	// Use this for initialization
	void Start () {
		TextMesh tm = GetComponent<TextMesh> ();
		tm.color = new Color(61f/255f, 189f/255f, 232f/255f);
		tm.text = "object" +
				"\n" +
				"\n" + 
				"\n    int" + 
				"\n" +
				"\n" +
				"\n" +
				"\n    if                      return " +
				"\n    else   return";
		/*tm.text = "The bug is that the wrong objects are compared" +
				"\nThe bug is that the darker object is chosen" +
				"\nThe bug is that not all objects are compared" +
				"\nThe bug is that the more green object is chosen" +
				"\nThe bug is that the more red object is chosen" +
				"\nThe bug is that the more blue object is chosen" +
				"\nThe bug is that the blue component isn't used" +
				"\nThe bug is that the green component isn't used" +
				"\nThe bug is that the red component isn't used";*/
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

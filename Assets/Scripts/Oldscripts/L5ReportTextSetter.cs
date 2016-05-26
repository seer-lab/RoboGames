using UnityEngine;
using System.Collections;

public class L5ReportTextSetter : MonoBehaviour {

	// Use this for initialization
	void Start () {
		TextMesh tm = GetComponent<TextMesh> ();
		tm.color = Color.white;
		tm.text = "       *compareThreats(object1, object2){" +
			"\n\t" +
				"\n\t" + 
				"\n\t        xval, yval, zval = 0,0,0;" + 
				"\n\t    xval = xval-object1.x+object2.x;" +
				"\n\t    yval -= (object2.y-object1.y);" +
				"\n\t    zval += (object1.z-object2.z);" +
				"\n\t      (xval+yval+zval > 0){        object1;}" +
				"\n\t         {        object2;}\n}";
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

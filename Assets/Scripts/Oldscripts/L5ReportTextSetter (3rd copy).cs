using UnityEngine;
using System.Collections;

public class L5ReportTextSetterD : MonoBehaviour {

	// Use this for initialization
	void Start () {
		TextMesh tm = GetComponent<TextMesh> ();
		tm.color = Color.black;
		tm.text = "object *compare(object1, object2){" +
				"\n\t//compare object 1 to object 2" +
				"\n\t//return the closer object to self at (0,0,0)" + 
				"\n\tint xval, yval, zval = 0,0,0; //use of all 3 objects" + 
				"\n\txval += ((red1-red2); //x component" +
				"\n\tyval += (blue1-blue2); //y component" +
				"\n\tzval += (green1-green2); //z component" +
				"\n\tif (xval+yval+zval > 0){return object1;}" +
				"\n\telse{return object2};";
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

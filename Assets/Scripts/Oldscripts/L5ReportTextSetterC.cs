using UnityEngine;
using System.Collections;

public class L5ReportTextSetterC : MonoBehaviour {

	// Use this for initialization
	void Start () {
		TextMesh tm = GetComponent<TextMesh> ();
		tm.color = new Color (166f/255f,226f/255f,46f/255f);
		tm.text = "" +
				"\n\t//compare object 1 to object 2" +
				"\n\t//return the closer object to self at (0,0,0)" + 
				"\n\t                                  //use of all 3 objects" + 
				"\n\t                                     //x component" +
				"\n\t                                    //y component" +
				"\n\t                                   //z component" +
				"\n\t" +
				"\n\t";
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

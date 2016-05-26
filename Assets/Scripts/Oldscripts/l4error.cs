using UnityEngine;
using System.Collections;

public class l4error : MonoBehaviour {

	public GameObject bugtext;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		TextMesh tm = this.GetComponent<TextMesh> ();
		TextMesh bugt = bugtext.GetComponent<TextMesh> ();
		if (bugt.text == "    //coltab[BLUE].table = bluetab;" +
		    "\n\n    //coltab[BLUE].tabsize = sizeof(bluetab)") {
			tm.color = Color.green;
			tm.text = "No Error";
		} 
		else {
			tm.color = Color.red;
			tm.text = "Error:\nGreen value out of bounds";
		}
	}
}

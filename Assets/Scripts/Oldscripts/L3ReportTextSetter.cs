using UnityEngine;
using System.Collections;

public class L3ReportTextSetter : MonoBehaviour {

	// Use this for initialization
	void Start () {
		TextMesh tm = GetComponent<TextMesh> ();
		tm.color = Color.magenta;
		tm.text = "";
		for (int i = 0;i<8;i++){
			tm.text += "The bug is found when i = " + System.Convert.ToString(i) + "\n";
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

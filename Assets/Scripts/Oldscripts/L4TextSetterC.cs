using UnityEngine;
using System.Collections;

public class L4TextSetterC : MonoBehaviour {

	string main = "//Robot Vision Compatability Function" +
		"\n//Load database of colours and sub-categories of colours" +
		"\n//match colour RGB (Red/Green/Blue) values with names" +
		"\n//Valid input values range from 0 to 255, e.g. (0,0,0)" +
		"\n//represents black while (0,255,255) represents cyan.";

	// Use this for initialization
	void Start () {
		TextMesh Tm = GetComponent<TextMesh>();
		Tm.text = main;	
		Tm.color = new Color (166f/255f,226f/255f,46f/255f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

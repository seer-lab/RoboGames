using UnityEngine;
using System.Collections;

public class l6errortext : MonoBehaviour {

	public GameObject l6dblack;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (l6dblack.GetComponent<TextMesh>().text == "{\"IO/Communications\",0,0,0},"){
			GetComponent<TextMesh>().text = "No error!";
		}
	}
}

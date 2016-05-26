using UnityEngine;
using System.Collections;

public class l6dblack : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D p){
		if (p.name == "projectileComment(Clone)") {
			TextMesh tm = GetComponent<TextMesh>();
			tm.color = Color.green;
			tm.text = "{\"IO/Communications\",0,0,0},";
		}
	}
}

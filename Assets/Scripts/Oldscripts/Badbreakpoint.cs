using UnityEngine;
using System.Collections;

public class Badbreakpoint : MonoBehaviour {

	float textdelay = 5f;
	private float removetext = 0.0f;

	// Use this for initialization
	void Start () {
		this.GetComponent<Renderer>().enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time > removetext) {
			this.GetComponent<Renderer>().enabled = false;
		}
	}

	void OnTriggerEnter2D(Collider2D c){
		if (c.name == "projectileDebug(Clone)") {
			this.GetComponent<Renderer>().enabled = true;
			removetext = Time.time + textdelay;
		}
	}
}

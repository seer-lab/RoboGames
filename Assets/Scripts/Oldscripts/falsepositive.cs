using UnityEngine;
using System.Collections;

public class falsepositive : MonoBehaviour {

	public GameObject text;
	public GameObject fplabel;

	float showdelay = 5f;
	private float hideself = 0.0f;

	// Use this for initialization
	void Start () {
		this.GetComponent<Renderer>().enabled = false;
		text.GetComponent<Renderer>().enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time > hideself) {
			this.GetComponent<Renderer>().enabled = false;
			text.GetComponent<Renderer>().enabled = false;
		}
	}

	void OnTriggerEnter2D(Collider2D p){
		if (p.name == "projectileBug(Clone)") {
		//	Destroy (p.gameObject);
			this.GetComponent<Renderer>().enabled = true;
			text.GetComponent<Renderer>().enabled = true;
			GetComponent<AudioSource>().Play();
			fplabel.GetComponent<TextMesh>().text = "Caught";
			hideself = Time.time + showdelay;
		}
	}
}

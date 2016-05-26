using UnityEngine;
using System.Collections;

public class l6bug : MonoBehaviour {
	

	// Use this for initialization
	void Start () {
		this.GetComponent<Renderer>().enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void OnTriggerEnter2D(Collider2D p){
		if (p.name == "projectileBug(Clone)") {
			this.GetComponent<Renderer>().enabled = true;
			GetComponent<Animator>().SetBool("Dying", true);
			Destroy(p.gameObject);
		}
	}
}

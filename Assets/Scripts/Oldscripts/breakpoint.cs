using UnityEngine;
using System.Collections;

public class breakpoint : MonoBehaviour {

	public GameObject next;
	public GameObject continuer;

	// Use this for initialization
	void Start () {
		//SpriteRenderer sr = GetComponent<SpriteRenderer> ();
	//	sr.color = Color.black;
		this.GetComponent<Renderer>().enabled=false;
		continuer.GetComponent<Renderer>().enabled=false;
	}
	
	// Update is called once per frame
	void Update () {
	}

	void OnTriggerEnter2D(Collider2D c){
		if (c.name == "projectileDebug(Clone)") {
			SpriteRenderer sr = GetComponent<SpriteRenderer> ();
			if (sr.color == Color.black){
				this.GetComponent<Renderer>().enabled=true;
				continuer.GetComponent<Renderer>().enabled=true;
				sr.color = Color.red;
				GetComponent<AudioSource>().Play();
			}
			//else if (sr.color == Color.red){
			else{
				sr.color = Color.black;
				this.GetComponent<Renderer>().enabled=false;
				continuer.GetComponent<Renderer>().enabled=false;
			}
		}
	}
}

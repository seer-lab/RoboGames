using UnityEngine;
using System.Collections;

public class l4bug : MonoBehaviour {
	
	public GameObject nextCode;
	public GameObject hero;
	public Animator anim;
	public bool dead = false;
	float deathtime = 1.5f;
	float deathdelay = 0f;
	public bool finished = false;
	
	// Use this for initialization
	void Start () {
		this.GetComponent<Renderer>().enabled = false;
		anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		if (dead && !finished) {
			if (Time.time > deathdelay){
				hero.transform.position = new Vector3 (nextCode.transform.position.x, nextCode.transform.position.y, 0);
				finished = true;
			}
		}
	}
	
	void OnTriggerEnter2D(Collider2D p){
		if (p.name == "projectileBug(Clone)") {
			this.GetComponent<Renderer>().enabled = true;
			Destroy (p.gameObject);
			anim.SetBool("Dying", true);
			GetComponent<AudioSource>().Play();
			dead = true;
			deathdelay = Time.time + deathtime;
		}
		//if (this.renderer.enabled && p.name == "hero"){
		//	p.transform.position = new Vector3 (nextCode.transform.position.x, nextCode.transform.position.y, 0);
		//}
	}
}

using UnityEngine;
using System.Collections;

public class l2bug : MonoBehaviour {

	public GameObject text;
	bool played = false;
	public string listname;
	public string resultname;
	public GameObject valuelabel;
	public GameObject valueslabel;
	public GameObject currenttest;

	// Use this for initialization
	void Start () {
		this.GetComponent<Renderer>().enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (text.GetComponent<TextMesh>().text == "ERROR!!!"){
			this.GetComponent<Renderer>().enabled = true;
			GetComponent<Animator>().SetBool("Dying", true);
			if (!played){
				GetComponent<AudioSource>().Play();
				played = true;
			}
		}
	}
	void OnTriggerEnter2D(Collider2D c){
		if (c.name == "hero"){
			valuelabel.GetComponent<GUIText>().text = resultname;
			valueslabel.GetComponent<GUIText>().text = listname;
			currenttest.GetComponent<TextMesh>().text = listname;
		}
	}
}

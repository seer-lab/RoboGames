using UnityEngine;
using System.Collections;

public class Return : MonoBehaviour {
	
	public BoxCollider2D destination;
	public Collider2D coll;
	public GameObject hero;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

	}
	void OnTriggerEnter2D(Collider2D c){
		if (c.name == "hero"){
			TextMesh tm = GetComponent<TextMesh>();
			tm.color = Color.green;
		}
		else if (c.name == "projectileWarp(Clone)"){
			hero.transform.position = new Vector3 (destination.transform.position.x+1f, destination.transform.position.y, 0);
			GetComponent<AudioSource>().Play();
			Destroy(c.gameObject);
		}
	}

	void OnTriggerExit2D(Collider2D c){
		//coll = c;
		if (c.name == "hero"){		
			TextMesh tm = GetComponent<TextMesh>();
			tm.color = new Color(61f/255f, 189f/255f, 232f/255f);
		}
	}
}

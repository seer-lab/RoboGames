using UnityEngine;
using System.Collections;

public class breakpointReset : MonoBehaviour {

	public GameObject breakpointHandler;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D c){
				if (c.name == "projectileTest(Clone)") {
						breakpointHandler.GetComponent<SpriteRenderer> ().color = Color.cyan;
						Destroy (c.gameObject);
						GetComponent<AudioSource> ().Play ();
				} else if (c.name == "hero") {
					GetComponent<TextMesh>().color = Color.green;
				}
		}

	void OnTriggerExit2D (Collider2D c)
	{
		if (c.name == "hero") {
			GetComponent<TextMesh>().color = new Color(61f/255f, 189f/255f, 232f/255f);
		}
	}
}

using UnityEngine;
using System.Collections;

public class projectile : MonoBehaviour {
	
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D c){
		if (c.GetType() == typeof(EdgeCollider2D)) {
						Destroy (gameObject);
				}
	}
}

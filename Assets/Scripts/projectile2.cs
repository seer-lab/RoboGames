using UnityEngine;
using System.Collections;
using System.IO;

public class projectile2 : MonoBehaviour {

	float initialLineY = 3.5f;
	float linespacing = 0.825f;
	
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D c){
		if (c.GetType() == typeof(EdgeCollider2D)) {
						StreamWriter sw = new StreamWriter("toollog.txt",true);
			sw.WriteLine(this.name+"Wasted,"+((int)((initialLineY-this.transform.position.y)/linespacing)).ToString()+","+Time.time.ToString());
						sw.Close();
						Destroy (gameObject);
				}
	}
}

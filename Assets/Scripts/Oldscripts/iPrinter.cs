using UnityEngine;
using System.Collections;

public class iPrinter : MonoBehaviour {

	public bool output = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		TextMesh tm = GetComponent<TextMesh>();
		if (output) {
			tm.color = Color.cyan;
			//tm.text = "cout<<priorities[i];";
			tm.text = "cout<<i;";
		}
		else{
			//tm.color = new Color (.25f, .25f, .25f);
			tm.color = Color.grey;
			//tm.text = "//cout<<priorities[i];";
			tm.text = "//cout<<i;";
		}
	}
	void OnTriggerEnter2D(Collider2D p){
		if (p.name == "projectileActivator(Clone)") {
			output = !output;
			Destroy(p.gameObject);
		}
	}
}

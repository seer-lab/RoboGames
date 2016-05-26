using UnityEngine;
using System.Collections;

public class l5variables : MonoBehaviour {

	public GameObject level;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		string lvl = level.GetComponent<TextMesh>().text;
		if (lvl == "5") {
			this.GetComponent<GUIText>().enabled = true;
		}
		else{
			this.GetComponent<GUIText>().enabled = false;
		}
	}
}

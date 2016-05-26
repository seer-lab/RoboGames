using UnityEngine;
using System.Collections;

public class l2valuelabels : MonoBehaviour {

	public GameObject level;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		int levelnum = System.Convert.ToInt16(level.GetComponent<TextMesh> ().text);
		if (levelnum == 2) {
			this.GetComponent<GUIText>().enabled = true;
		} else {
			this.GetComponent<GUIText>().enabled = false;
		}
	}
}

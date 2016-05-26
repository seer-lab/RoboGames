using UnityEngine;
using System.Collections;

public class sidebartools : MonoBehaviour {

	public GameObject level;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		int levelnum = System.Convert.ToInt16(level.GetComponent<TextMesh> ().text);
		if (levelnum > 0 && levelnum < 100) {
			this.GetComponent<GUIText>().text = "ACTIVE TOOLS:";
				} else {
			this.GetComponent<GUIText>().text = "";
				}
	}
}

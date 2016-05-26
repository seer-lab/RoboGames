using UnityEngine;
using System.Collections;

public class HUDText : MonoBehaviour {

	public GameObject level;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		GUIText gi = GetComponent<GUIText> ();
		TextMesh tm = level.GetComponent<TextMesh> ();
		int levelnum = System.Convert.ToInt16(level.GetComponent<TextMesh> ().text);
		if (levelnum > 0 && levelnum < 100) {
						gi.text = "LEVEL " + tm.text;
				} else {
			gi.text = "";
				}
	}

}

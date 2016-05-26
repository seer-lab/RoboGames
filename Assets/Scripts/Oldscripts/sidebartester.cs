using UnityEngine;
using System.Collections;

public class sidebartester : MonoBehaviour {

	public GameObject level;
	int levelnum;
	public GameObject projectilecode;
	int projectilenum;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		levelnum = System.Convert.ToInt16(level.GetComponent<TextMesh> ().text);
		projectilenum = System.Convert.ToInt16(projectilecode.GetComponent<TextMesh> ().text);
		if (levelnum != 2 && levelnum != 5) {
			this.GetComponent<GUITexture> ().color = new Color (0.5f,0.5f,0.5f,0);
		}
		else if (projectilenum == 2) {
			this.GetComponent<GUITexture> ().color = new Color (0.5f,0.5f,0.5f,1);
		} else {
			this.GetComponent<GUITexture> ().color = new Color (0.5f,0.5f,0.5f,0.2f);
		}
	}
}

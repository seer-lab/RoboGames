using UnityEngine;
using System.Collections;

public class sidebaractivator : MonoBehaviour {

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
		if (levelnum < 3 || levelnum > 4) {
			this.GetComponent<GUITexture> ().color = new Color (0.5f,0.5f,0.5f,0);
		}
		else if (projectilenum == 3) {
			this.GetComponent<GUITexture> ().color = new Color (0.5f,0.5f,0.5f,1);
		} else {
			this.GetComponent<GUITexture> ().color = new Color (0.5f,0.5f,0.5f,0.2f);
		}
	}
}

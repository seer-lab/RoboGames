using UnityEngine;
using System.Collections;

public class sidebarhelppanel : MonoBehaviour {
	
	public GameObject level;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		int levelnum = System.Convert.ToInt16 (level.GetComponent<TextMesh> ().text);
		if (levelnum == 2 || levelnum == 4 || levelnum == 5 ) {
		//	this.guiTexture.enabled = true;
		} else {
		//	this.guiTexture.enabled = false;
		}
	}
}

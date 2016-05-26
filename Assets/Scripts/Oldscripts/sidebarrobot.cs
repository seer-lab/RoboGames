using UnityEngine;
using System.Collections;

public class sidebarrobot : MonoBehaviour {

	public GameObject level;
	int levelnum;
	public Texture2D lvl1;
	public Texture2D lvl2;
	public Texture2D lvl3;
	public Texture2D lvl4;
	public Texture2D lvl5;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		levelnum = System.Convert.ToInt16(level.GetComponent<TextMesh> ().text);
		GUITexture img = GetComponent<GUITexture> ();
		if (levelnum > 0 && levelnum < 100) {
				img.enabled = true;
				//Animator anim = GetComponent<Animator>();
			//anim.SetInteger("levelnum", levelnum);
			switch(levelnum){
			case 1:
				img.texture = lvl1;
				break;
			case 2:
				img.texture = lvl2;
				break;
			case 3:
				img.texture = lvl3;
					break;
			case 4:
				img.texture = lvl4;
					break;
			case 5:
				img.texture = lvl5;
					break;
			}
				} else {
				img.enabled = false;
				}
					
	
	}
}

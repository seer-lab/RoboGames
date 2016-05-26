using UnityEngine;
using System.Collections;

public class sidebarselected : MonoBehaviour {



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
		GUIText tm = GetComponent<GUIText> ();
		if (levelnum > 0 && levelnum < 6) {
						switch (projectilenum) {
			case 0: tm.color = Color.red;
				break;
						case 1:
				tm.color = Color.white;
				tm.text = "Bugcatcher";
								break;
			case 2: tm.color = Color.white;
				tm.text = "Tester";
								break;
			case 3: tm.color = Color.white;
				tm.text = "Activator";
								break;
			case 4: tm.color = Color.white;
				tm.text = "Breakpointer";
								break;
			case 5: tm.color = Color.white;
				tm.text = "Warper";
								break;
						}
				} else {
			tm.text = "";
				}
	}
}

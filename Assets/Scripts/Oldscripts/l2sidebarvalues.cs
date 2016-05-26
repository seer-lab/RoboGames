using UnityEngine;
using System.Collections;

public class l2sidebarvalues : MonoBehaviour
{

		public GameObject currentTest;
		public GameObject text1;
		public GameObject text2;
		public GameObject text3;
		public GameObject level;

		// Use this for initialization
		void Start ()
		{
				GetComponent<GUIText> ().text = "";
		}
	
		// Update is called once per frame
		void Update ()
		{
				int levelnum = System.Convert.ToInt16 (level.GetComponent<TextMesh> ().text);
				if (levelnum == 2) {
						string test = currentTest.GetComponent<TextMesh> ().text;
						if (test == "distanceValues") {
								GetComponent<GUIText> ().text = text1.GetComponent<TextMesh> ().text;
						} else if (test == "powerValues") {
								GetComponent<GUIText> ().text = text2.GetComponent<TextMesh> ().text;
						} else {
								GetComponent<GUIText> ().text = text3.GetComponent<TextMesh> ().text;
						}
						
				} else {
						GetComponent<GUIText> ().text = "";
				}
		}
}

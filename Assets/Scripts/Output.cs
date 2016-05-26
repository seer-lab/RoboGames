using UnityEngine;
using System.Collections;

public class Output : MonoBehaviour
{
		public GameObject codescreen;
		public GameObject outputtext;
		Animator anim;

		// Use this for initialization
		void Start ()
		{
				anim = GetComponent<Animator> ();
				anim.SetBool ("Appearing", false);
				anim.SetBool ("Hiding", false);
		}
	
		// Update is called once per frame
		void Update ()
		{
				bool isText = outputtext.GetComponent<GUIText> ().text != "";
				if (isText) {
						anim.SetBool ("Appearing", true);
						anim.SetBool ("Hiding", false);
				} else {
						anim.SetBool ("Appearing", false);
						anim.SetBool ("Hiding", true);
				}
		if (Input.GetKeyDown (KeyCode.Return) || codescreen.GetComponent<LevelGenerator>().gamestate != 1) {
			outputtext.GetComponent<GUIText> ().text = "";
				}
		}
}

using UnityEngine;
using System.Collections;

public class l4Return : MonoBehaviour
{
	
		public BoxCollider2D destination;
		public bool inside = false;
		public Collider2D coll;
		public string colorTable;
		public GameObject hero;

		// Use this for initialization
		void Start ()
		{
				TextMesh tm = GetComponent<TextMesh> ();
		tm.color = new Color(61f/255f, 189f/255f, 232f/255f);
//		tm.text = "        coltab[" + colorTable.ToUpper () + "].table = " + colorTable + "tab;" +
//			"\n\n        coltab[" + colorTable.ToUpper () + "].tabsize = sizeof(" + colorTable + "tab) / sizeof(" + colorTable + "tab[0]);";
				tm.text = "    coltable[" + colorTable.ToUpper () + "].table = " + colorTable + "tab;" +
						"\n    coltable[" + colorTable.ToUpper () + "].tablesize = sizeof(" + colorTable + "tab)";
		}
	
		// Update is called once per frame
		void Update ()
		{

		}

		void OnTriggerEnter2D (Collider2D c)
		{
				TextMesh tm = GetComponent<TextMesh> ();
				if (c.name == "projectileActivator(Clone)") {
						if (tm.text == "    //coltable[" + colorTable.ToUpper () + "].table = " + colorTable + "tab;" +
								"\n    //coltable[" + colorTable.ToUpper () + "].tabsize = sizeof(" + colorTable + "tab)") {

				tm.color = new Color(61f/255f, 189f/255f, 232f/255f);
								tm.text = "    coltable[" + colorTable.ToUpper () + "].table = " + colorTable + "tab;" +
										"\n    coltable[" + colorTable.ToUpper () + "].tabsize = sizeof(" + colorTable + "tab)";

						} else {
						tm.color = Color.grey;
								tm.text = "    //coltable[" + colorTable.ToUpper () + "].table = " + colorTable + "tab;" +
										"\n    //coltable[" + colorTable.ToUpper () + "].tabsize = sizeof(" + colorTable + "tab)";
						}
				} else if (c.name == "hero") {
			if (tm.color == new Color(61f/255f, 189f/255f, 232f/255f)) {
								inside = true;
								coll = c;
								tm.color = Color.green;
						}
				} else if (c.name == "projectileWarp(Clone)") {
						if (tm.color != Color.grey) {
								hero.transform.position = new Vector3 (destination.transform.position.x + 1f, destination.transform.position.y, 0);
								GetComponent<AudioSource> ().Play ();
								Destroy (c.gameObject);
						}
				}
		}

		void OnTriggerExit2D (Collider2D c)
		{
				TextMesh tm = GetComponent<TextMesh> ();
				if (tm.color == Color.green && c.name == "hero") {
						inside = false;
				tm.color = new Color(61f/255f, 189f/255f, 232f/255f);
		}
		}
}

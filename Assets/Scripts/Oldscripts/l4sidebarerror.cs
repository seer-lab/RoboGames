using UnityEngine;
using System.Collections;

public class l4sidebarerror : MonoBehaviour
{
	
		public GameObject bugtext;
		public GameObject[] tables;
		public bool[] tableStates;
		public GameObject level;
		bool compiling = false;
		float compileDelay = 10.5f;
		float compileTime = 0f;
	
		// Use this for initialization
		void Start ()
		{
				tableStates = new bool[] {
						true,
						true,
						true,
						true,
						true,
						true,
						true,
						true,
						true,
						true,
						true,
						true,
				};
		}
	
		// Update is called once per frame
		void Update ()
		{
				int levelnum = System.Convert.ToInt16 (level.GetComponent<TextMesh> ().text);
				GUIText tm = this.GetComponent<GUIText> ();
				TextMesh bugt = bugtext.GetComponent<TextMesh> ();
				if (levelnum == 4) {
						if (compiling && Time.time < compileTime) {
								tm.color = Color.yellow;
								tm.text = "Compiling...\nready in " + Mathf.Round (compileTime - Time.time) + " seconds.";
						} else if (compiling && Time.time >= compileTime) {
								compiling = false;
						} 
						for (int i = 0; i<12; i++) {
				if (tables [i].GetComponent<TextMesh> ().text.StartsWith ("    //") && tableStates [i] == true) 
				    			{
									compiling = true;
									compileTime = Time.time + compileDelay;
									tableStates[i] = false;
								}
				else if (!tables [i].GetComponent<TextMesh> ().text.StartsWith ("    //") && tableStates [i] == false){
									compiling = true;
									compileTime = Time.time + compileDelay;
									tableStates[i] = true;
								}
						}
						if (!compiling){
							if (bugt.text == "    //coltable[BLUE].table = bluetab;" +
							    "\n    //coltable[BLUE].tabsize = sizeof(bluetab)"){
								tm.color = Color.green;
								tm.text = "No Error";
							}
							else{
								tm.color = Color.red;
								tm.text = "Error:\nGreen value\nout of bounds";
							}
						}
				} else {
						tm.text = "";
				}
		}
}
/*if (compiling) {
							
						} else {
								if (bugt.text == "        //coltab[BLUE].table = bluetab;" +
										"\n\n        //coltab[BLUE].tabsize = sizeof(bluetab)" && error) {
										//	tm.color = Color.green;
										//	tm.text = "No Error";
									compiling = true;
									error = false;
								} else if ({
										tm.color = Color.red;
										tm.text = "Error:\nGreen value out of bounds";
								}
						}*/
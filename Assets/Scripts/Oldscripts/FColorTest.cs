using UnityEngine;
using System.Collections;

public class FColorTest : MonoBehaviour
{
	
		public GameObject result;
		public string resultText = "";
		public GameObject input;
		public bool inside = false;
		public Collider2D coll;
		float textdelay = 5f;
		private float removetext = 0.0f;
		public string namedcolors = "\"Senses\",\"100\",\"100\",\"100\"},{\"IO/Communications\",\"0\",\"0\",\"0\"}," +
				"{\"MotorControl\",\"25\",\"85\",\"35\"},{\"CPU\",\"50\",\"100\",\"100\"}," +
				"{\"Memory\",\"100\",\"100\",\"100\"},{\"EnergyManagement\",\"10\",\"100\",\"100\"},{\"Head\",\"100\",\"100\",\"75\"}," +
				"{\"RightHand\",\"50\",\"95\",\"40\"},{\"LeftHand\",\"100\",\"100\",\"80\"},{\"RightArm\",\"50\",\"50\",\"70\"}," +
				"{\"LeftArm\",\"30\",\"50\",\"60\"},{\"RightLeg\",\"100\",\"90\",\"80\"},{\"LeftLeg\",\"75\",\"75\",\"75\"}," +
				"{\"RightFoot\",\"25\",\"25\",\"50\"},{\"LeftFoot\",\"100\",\"50\",\"100\"},{\"Torso\",\"50\",\"100\",\"100\"";
		public string[] colors;
	
		// Use this for initialization
		void Start ()
		{
				TextMesh tm = GetComponent<TextMesh> ();
				tm.color = Color.blue;
				colors = namedcolors.Replace ("},{", "@").Replace ("\"", "").Split ('@');
		}
	
		// Update is called once per frame
		void Update ()
		{
				result.GetComponent<TextMesh> ().text = resultText;
				if (Time.time > removetext) {
						resultText = "";
				}

		
		}

		void OnTriggerEnter2D (Collider2D c)
		{
				string inputText = input.GetComponent<TextMesh> ().text;
				if (inputText != "" && inputText != "<INVALID INPUT>") {
						if (c.name == "hero") {
								TextMesh tm = GetComponent<TextMesh> ();
								tm.color = Color.green;
						} else if (c.name == "projectileTest(Clone)") {
								int elements = 0;
								inputText = input.GetComponent<TextMesh> ().text;
								string[] color = inputText.Split (',');
								if (color.Length == 3) {
										if (color [2] != "") {
												string[] col;
												string farcol = "";
												foreach (string s in colors) {
														col = s.Split (',');
														if (System.Convert.ToInt32 (color [0]) <= System.Convert.ToInt32 (col [1])) {
																if (System.Convert.ToInt32 (color [1]) <= System.Convert.ToInt32 (col [2])) {
																		if (System.Convert.ToInt32 (color [2]) <= System.Convert.ToInt32 (col [3])) {
																				if (elements > 0) {
																						farcol += ", ";
																				}
																				elements++;
																				if (elements % 5 == 0) {
																						farcol += "\n";
																				}
																				farcol += col [0];
																
																		}
																}
														} 
												}
												resultText = farcol;
												removetext = Time.time + textdelay;
										}
								}
						}
				}
		}

		void OnTriggerExit2D (Collider2D c)
		{
				//coll = c;
				if (c.name == "hero") {		
						TextMesh tm = GetComponent<TextMesh> ();
						tm.color = Color.red;
				}
		}
}

using UnityEngine;
using System.Collections;

public class sumTest : MonoBehaviour
{

		public GameObject result;
		public string resultText = "";
		public GameObject input;
		public GameObject output;
		public bool inside = false;
		public Collider2D coll;
		float textdelay = 5f;
		private float removetext = 0.0f;
		// Use this for initialization
		void Start ()
		{
				TextMesh tm = GetComponent<TextMesh> ();
				tm.color = new Color (61f / 255f, 189f / 255f, 232f / 255f);
		}
	
		// Update is called once per frame
		void Update ()
		{
				result.GetComponent<TextMesh> ().text = resultText;
				if (Time.time > removetext) {
						result.GetComponent<Renderer>().enabled = false;
				}


		}

		void OnTriggerEnter2D (Collider2D c)
		{
				string inputText = input.GetComponent<TextMesh> ().text;
				string outputText = output.GetComponent<TextMesh> ().text;
				//	if (inputText != "" && outputText != "" && inputText != "<INVALID INPUT>") {
				if (inputText != "" && outputText != "" && inputText != "<INVALID INPUT>") {
						if (c.name == "hero") {
								TextMesh tm = GetComponent<TextMesh> ();
								tm.color = Color.green;
						} else if (c.name == "projectileTest(Clone)") {
								inputText = input.GetComponent<TextMesh> ().text;
								outputText = output.GetComponent<TextMesh> ().text;
								if (inputText != "<INVALID INPUT>") {
										if (inputText.Contains (".")) {
												result.GetComponent<TextMesh> ().color = Color.red;
												resultText = "ERROR!!!";
												removetext = Time.time + textdelay;
												result.GetComponent<Renderer>().enabled = true;
										} else {
												string[] vals = inputText.Split (',');
												int sum = 0;
												foreach (string s in vals) {
														sum += System.Convert.ToInt32 (s);
												}
												if (sum == System.Convert.ToDouble (outputText)) {
														resultText = "True.";
														removetext = Time.time + textdelay;
														result.GetComponent<Renderer>().enabled = true;
												} else {
														resultText = "False.";
														removetext = Time.time + textdelay;
														result.GetComponent<Renderer>().enabled = true;
												}
										}
								}
						}
				}
		}
				
		//	}

		void OnTriggerExit2D (Collider2D c)
		{
				//coll = c;
				if (c.name == "hero") {		
						TextMesh tm = GetComponent<TextMesh> ();
						tm.color = new Color (61f / 255f, 189f / 255f, 232f / 255f);
				}
		}
}

using UnityEngine;
using System.Collections;

public class midTest : MonoBehaviour
{

		public GameObject result;
		public string resultText = "";
		public GameObject input;
		public GameObject output;
		public bool inside = false;
		public Collider2D coll;
		float textdelay = 5f;
		private float removetext = 0.0f;
		public double mid;
		public string[] vals;
	
		// Use this for initialization
		void Start ()
		{
				TextMesh tm = GetComponent<TextMesh> ();
		tm.color = new Color(61f/255f, 189f/255f, 232f/255f);
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
				if (inputText != "" && outputText != "" && inputText != "<INVALID INPUT>") {
						if (c.name == "hero") {
								TextMesh tm = GetComponent<TextMesh> ();
								tm.color = Color.green;
						} else if (c.name == "projectileTest(Clone)") {
								
								if (inputText != "" && outputText != "" && inputText != "<INVALID INPUT>") {
										vals = inputText.Split (',');
										bool sorted = true;
										for (int i = 0; i<vals.Length; i++) {
												if (i < vals.Length - 1) {
														if ((double)System.Convert.ToDouble (vals [i]) > (double)System.Convert.ToDouble (vals [i + 1])) {
																sorted = false;
														}
												}
										}
										if (!sorted) {
												result.GetComponent<TextMesh> ().color = Color.red;
												resultText = "ERROR!!!";
												result.GetComponent<Renderer>().enabled = true;
												removetext = Time.time + textdelay;
										} else if (inputText != "" && outputText != "") {
												if (vals.Length % 2 == 0) {
														mid = ((double)System.Convert.ToDouble (vals [vals.Length / 2 - 1]) + System.Convert.ToDouble (vals [vals.Length / 2])) / 2.0;
												} else {
														mid = System.Convert.ToDouble (vals [vals.Length / 2]);
												}
				
												if (mid == System.Convert.ToDouble (outputText)) {
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

		void OnTriggerExit2D (Collider2D c)
		{
				//coll = c;
				if (c.name == "hero") {		
						TextMesh tm = GetComponent<TextMesh> ();
			tm.color = new Color(61f/255f, 189f/255f, 232f/255f);
				}
		}
}

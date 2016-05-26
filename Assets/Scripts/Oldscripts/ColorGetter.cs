using UnityEngine;
using System.Collections;

public class ColorGetter : MonoBehaviour
{

		public GameObject values;
		public string valueText = "";
		public bool validInput = true;
		public Collider2D coll;
		public int digits = 0;
		public int commas = 0;
		public int thisnum = 0;
		public bool inside = false;

		// Use this for initialization
		void Start ()
		{
				TextMesh Tm = GetComponent<TextMesh> ();
				Tm.text = "Levels = [\n\n];";
		}
	
		// Update is called once per frame
		void Update ()
		{
				values.GetComponent<TextMesh> ().text = valueText;
				if (inside) {
						if (validInput && coll.attachedRigidbody) {
								if (Input.GetKeyDown ("1")) {
										if (digits == 3) {
												validInput = false;
												valueText = "<INVALID INPUT>";
										} else {
												thisnum += 100 / (int) (Mathf.Pow(10,digits));
												digits++;
												valueText += "1";
										}
								} else if (Input.GetKeyDown ("2")) {

										if (digits == 3) {
												validInput = false;
												valueText = "<INVALID INPUT>";
										} else {
												thisnum += 200 / (int) (Mathf.Pow(10,digits));
												digits++;
												valueText += "2";
										}
								} else if (Input.GetKeyDown ("3")) {

										if (digits == 3) {
												validInput = false;
												valueText = "<INVALID INPUT>";
										} else {
												thisnum += 300 / (int) (Mathf.Pow(10,digits));
												digits++;
												valueText += "3";
										}
								} else if (Input.GetKeyDown ("4")) {

										if (digits == 3) {
												validInput = false;
												valueText = "<INVALID INPUT>";
										} else {
												thisnum += 400 / (int) (Mathf.Pow(10,digits));
												digits++;
												valueText += "4";
										}
								} else if (Input.GetKeyDown ("5")) {

										if (digits == 3) {
												validInput = false;
												valueText = "<INVALID INPUT>";
										} else {
												thisnum += 500 / (int) (Mathf.Pow(10,digits));
												digits++;
												valueText += "5";
										}
								} else if (Input.GetKeyDown ("6")) {

										if (digits == 3) {
												validInput = false;
												valueText = "<INVALID INPUT>";
										} else {
												thisnum += 600 / (int) (Mathf.Pow(10,digits));
												digits++;
												valueText += "6";
										}
								} else if (Input.GetKeyDown ("7")) {

										if (digits == 3) {
												validInput = false;
												valueText = "<INVALID INPUT>";
										} else {
												thisnum += 700 / (int) (Mathf.Pow(10,digits));
												digits++;
												valueText += "7";
										}
								} else if (Input.GetKeyDown ("8")) {

										if (digits == 3) {
												validInput = false;
												valueText = "<INVALID INPUT>";
										} else {
												thisnum += 800 / (int) (Mathf.Pow(10,digits));
												digits++;
												valueText += "8";
										}
								} else if (Input.GetKeyDown ("9")) {

										if (digits == 3) {
												validInput = false;
												valueText = "<INVALID INPUT>";
										} else {
												thisnum += 900 / (int) (Mathf.Pow(10,digits));
												digits++;
												valueText += "9";
										}
								} else if (Input.GetKeyDown ("0")) {

										if (digits == 3) {
												validInput = false;
												valueText = "<INVALID INPUT>";
										} else {
												digits++;
												valueText += "0";
										}
								} else if (Input.GetKeyDown (",")) {
										if (digits == 0 || commas == 3) {
												validInput = false;
												valueText = "<INVALID INPUT>";
										} else {
												digits = 0;
												commas++;
												thisnum = 0;
												valueText += ",";
										}
								}
								if (digits == 3 && thisnum > 100) {
										validInput = false;
										valueText = "<INVALID INPUT>";
								}
						} 
						if (Input.GetKeyDown ("backspace")) {
								validInput = true;
								digits = 0;
								commas = 0;
								thisnum = 0;
								valueText = "";
						}	
				}
		}

		void OnTriggerEnter2D (Collider2D c)
		{
				inside = true;
				coll = c;
		}

		void OnTriggerExit2D (Collider2D c)
		{
				inside = false;
		}
}

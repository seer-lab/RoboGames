using UnityEngine;
using System.Collections;

public class ValuesGetter : MonoBehaviour {

	public GameObject values;
	bool commaOkay = false;
	bool minusOkay = true;
	bool decOkay = true;
	public string valueText = "";
	public bool validInput = true;
	public Collider2D coll;
	public int numdigits;

	public bool inside = false;

	// Use this for initialization
	void Start () {
		TextMesh Tm = GetComponent<TextMesh>();
		Tm.text = "";
		numdigits = 0;
	}
	
	// Update is called once per frame
	void Update () {
				values.GetComponent<TextMesh> ().text = valueText;
				if (inside) {
					if (validInput && coll.attachedRigidbody){
						if (Input.GetKeyDown ("1")) {
								commaOkay = true;
								minusOkay = false;
								valueText += "1";
					numdigits++;
						} else if (Input.GetKeyDown ("2")) {
								commaOkay = true;
								minusOkay = false;
					valueText += "2";numdigits++;
						} else if (Input.GetKeyDown ("3")) {
								commaOkay = true;
								minusOkay = false;
					valueText += "3";numdigits++;
						} else if (Input.GetKeyDown ("4")) {
								commaOkay = true;
								minusOkay = false; 
					valueText += "4";numdigits++;
						} else if (Input.GetKeyDown ("5")) {
								commaOkay = true;
								minusOkay = false;
					valueText += "5";numdigits++;
						} else if (Input.GetKeyDown ("6")) {
								commaOkay = true;
								minusOkay = false;
					valueText += "6";numdigits++;
						} else if (Input.GetKeyDown ("7")) {
								commaOkay = true;
								minusOkay = false;
					valueText += "7";numdigits++;
						} else if (Input.GetKeyDown ("8")) {
								commaOkay = true;
								minusOkay = false;
					valueText += "8";numdigits++;
						} else if (Input.GetKeyDown ("9")) {
								commaOkay = true;
								minusOkay = false;
					valueText += "9";numdigits++;
						} else if (Input.GetKeyDown ("0")) {
								commaOkay = true;
								minusOkay = false;
					valueText += "0";numdigits++;
						} else if (Input.GetKeyDown ("-")) {
								if (!minusOkay) {
									validInput = false;
									valueText = "<INVALID INPUT>";
						TextMesh Tm = GetComponent<TextMesh> ();
						Tm.text = "**PRESS BACKSPACE TO RETRY**";
								}
								else{
								minusOkay = false;
								commaOkay = false;
								valueText += "-";
								}
						} else if (Input.GetKeyDown (".")) {
							if (!decOkay) {
								validInput = false;
								valueText = "<INVALID INPUT>";
						TextMesh Tm = GetComponent<TextMesh> ();
						Tm.text = "**PRESS BACKSPACE TO RETRY**";
							}
							else{
								minusOkay = false;
								commaOkay = false;
								decOkay = false;
								valueText += ".";
								numdigits = 0;
							}
						}else if (Input.GetKeyDown (",")) {
							if (!commaOkay) {
									validInput = false;
									valueText = "<INVALID INPUT>";
						TextMesh Tm = GetComponent<TextMesh> ();
						Tm.text = "**PRESS BACKSPACE TO RETRY**";
							}
							else{
							commaOkay = false;
							minusOkay = true;
							decOkay = true;
							valueText += ",";
							numdigits = 0;
							}
						}
					}
			if (valueText.Length >= 24 || numdigits > 4){
				validInput = false;
				valueText = "<INVALID INPUT>";
				TextMesh Tm = GetComponent<TextMesh> ();
				Tm.text = "**PRESS BACKSPACE TO RETRY**";
			}
					if (Input.GetKeyDown ("backspace")) {
						commaOkay = false;
						minusOkay = true;
						decOkay = true;
						validInput = true;
						valueText = "";
						numdigits = 0;
				TextMesh Tm = GetComponent<TextMesh> ();
				Tm.text = "**TYPE IN A LIST OF NUMBERS**";
					}	
			}
	}
	void OnTriggerEnter2D(Collider2D c){
		inside = true;
		coll = c;
		if (c.name == "hero") {
						TextMesh Tm = GetComponent<TextMesh> ();
						Tm.color = Color.yellow;
						if (validInput){
							Tm.text = "**TYPE IN A LIST OF NUMBERS**";
							}
						else{
							Tm.text = "**PRESS BACKSPACE TO RETRY**";
			}
				}
	}
	void OnTriggerExit2D(Collider2D c){
		inside = false;
		if (c.name == "hero") {
		TextMesh Tm = GetComponent<TextMesh>();
			Tm.color = Color.white;
			Tm.text = "{" + valueText + "};";}
	}
}

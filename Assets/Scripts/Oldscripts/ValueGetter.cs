using UnityEngine;
using System.Collections;

public class ValueGetter : MonoBehaviour {
	
	public GameObject value;
	public string valueText = "";
	public Collider2D coll;
	public bool minusOkay = true;
	public bool decOkay = true;
	public bool inside = false;
	public bool validInput = true;
	public int numdigits;
	
	// Use this for initialization
	void Start () {
		TextMesh Tm = GetComponent<TextMesh>();
		Tm.text = "";
		numdigits = 0;
	}
	
	// Update is called once per frame
	void Update () {
		value.GetComponent<TextMesh> ().text = valueText;
		if (inside) {
				if (validInput){
				if (Input.GetKeyDown ("1")) {
					valueText += "1";minusOkay = false;numdigits++;
				} else if (Input.GetKeyDown ("2")) {
					valueText += "2";minusOkay = false;numdigits++;
				} else if (Input.GetKeyDown ("3")) {
					valueText += "3";minusOkay = false;numdigits++;
				} else if (Input.GetKeyDown ("4")) {
					valueText += "4";minusOkay = false;numdigits++;
				} else if (Input.GetKeyDown ("5")) {
					valueText += "5";minusOkay = false;numdigits++;
				} else if (Input.GetKeyDown ("6")) {
				minusOkay = false;
					valueText += "6";numdigits++;
				} else if (Input.GetKeyDown ("7")) {
				minusOkay = false;
					valueText += "7";numdigits++;
				} else if (Input.GetKeyDown ("8")) {
				minusOkay = false;
					valueText += "8";numdigits++;
				} else if (Input.GetKeyDown ("9")) {
				minusOkay = false;
					valueText += "9";numdigits++;
				} else if (Input.GetKeyDown ("0")) {
				minusOkay = false;
					valueText += "0";numdigits++;
				} 
			 if (Input.GetKeyDown("-") && minusOkay){
				minusOkay = false;
				valueText += "-";
			}
			if (Input.GetKeyDown(".") && decOkay){
				minusOkay = false;
				decOkay = false;
				valueText += ".";
					numdigits = 0;
				}}
			if (valueText.Length >= 10 || numdigits>4){
				validInput = false;
				valueText = "<INVALID INPUT>";
				TextMesh Tm = GetComponent<TextMesh> ();
				Tm.text = "**PRESS BACKSPACE TO RETRY**";
			}
			 if (Input.GetKeyDown ("backspace")) {
				valueText = "";
				minusOkay = true;
				decOkay = true;
				validInput = true;
				numdigits = 0;
				TextMesh Tm = GetComponent<TextMesh> ();
				Tm.text = "**TYPE IN A NUMBER**";
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
				Tm.text = "**TYPE IN A NUMBER**";
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
			Tm.text = valueText + ";";}
	}
}

using UnityEngine;
using System.Collections;

public class WWWFormImage : MonoBehaviour {

	public string databaseURL= "http://www.my-server.com/cgi-bin/screenshot.pl";

	// Use this for initialization
	void Start () {
		StartCoroutine(sendDataToDatabase());
	}

	IEnumerator sendDataToDatabase() {
		// Create a Web Form
		WWWForm form = new WWWForm();
		form.AddField("frameCount", Time.frameCount.ToString());
		//form.AddBinaryData("fileUpload", bytes, "screenShot.png", "image/png");

		// Upload to a cgi script
		WWW w = new WWW(databaseURL, form);
		yield return w;
		if (!string.IsNullOrEmpty(w.error)) {
			print(w.error);
		}
		else {
			print("Finished Uploading");
		}
	}
}

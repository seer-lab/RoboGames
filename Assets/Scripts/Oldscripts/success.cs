using UnityEngine;
using System.Collections;

public class success : MonoBehaviour {

	public GameObject level;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		TextMesh tm = GetComponent<TextMesh> ();
		int levelnum = System.Convert.ToInt16(level.GetComponent<TextMesh> ().text);
		switch (levelnum) {
		case 200:
			tm.text = "Congratulations!\n\nYou've finished level 1\nand eliminated the bug\nusing CODE TRACING.\n\n\n\n\n\n\nPress Space to Continue";
			break;
		case 300:
			tm.text = "Congratulations!\n\nYou've finished level 2\nand eliminated the bug\nusing BLACK BOX TESTING.\n\n\n\n\n\n\nPress Space to Continue";
			break;
		case 400:
			tm.text = "Congratulations!\n\nYou've finished level 3\nand eliminated the bug\nusing PRINT STATEMENTS.\n\n\n\n\n\n\nPress Space to Continue";
			break;
		case 500:
			tm.text = "Congratulations!\n\nYou've finished level 4\nand eliminated the bug\nusing ERROR MESSAGES\nand DIVIDE AND CONQUER.\n\n\n\n\n\nPress Space to Continue";
			break;
		case 999:
			tm.text = "Congratulations!\n\nYou've finished level 5\nand eliminated the bug\nusing BREAKPOINTS.\n\n\n\n\n\n\nPress Space to Continue";
			break;
				}
	}
}

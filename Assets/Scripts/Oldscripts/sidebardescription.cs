using UnityEngine;
using System.Collections;

public class sidebardescription : MonoBehaviour {

	public GameObject level;
	public int levelnum;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		levelnum = System.Convert.ToInt16(level.GetComponent<TextMesh> ().text);
		switch (levelnum) {
		case 1:
			GetComponent<GUIText>().text = "OBJECTIVE:\n\nFind the bug\nHit it with a Bugcatcher\nPress CTRL to Throw";
			break;
		case 2:
			GetComponent<GUIText>().text = "OBJECTIVE:\n\nType values while standing in the yellow text\nUse the TESTER tool at the return lines\nIdentify an error in one of the functions\n";
			break;
		case 3:
			GetComponent<GUIText>().text = "OBJECTIVE:\n\nPress Tab to cycle through your tools\nUse the ACTIVATOR tool to toggle printing\nUse the WARPER on the very top line\nCatch the bug with a BUGCATCHER";
			break;
		case 4:
			GetComponent<GUIText>().text = "OBJECTIVE:\n\nUse the ACTIVATOR tool to comment out tables\nWatch the error after commenting\nWARP to the table causing the error\nThrow a BUGCATCHER at the bug";
			break;
		case 5:
			GetComponent<GUIText>().text = "OBJECTIVE:\nObserve variable behaviour using the debugger\nThrow BREAKPOINTERS left to toggle breakpoints\nThrow TESTERS right to run the program\nUse the TESTER tool if you want to reset\nYou can WARP to the compare function";
			break;
		case 6:
			GetComponent<GUIText>().text = "OBJECTIVE:\n1)figure out which function is faulty and throw a Bugcatcher at it\n2)use the error message to determine the source of bug#1 and fix it\n3)Use the breakpoints to find bug#2\n4)Throw a bugcatcher at the exact place the bug occurs\n5)Find and catch the final bug";
			break;
		case 0:
			GetComponent<GUIText>().text = "";
			break;
		case 100:
			GetComponent<GUIText>().text = "";
			break;
		case 200:
			GetComponent<GUIText>().text = "";
			break;
		case 300:
			GetComponent<GUIText>().text = "";
			break;
		case 400:
			GetComponent<GUIText>().text = "";
			break;
		case 500:
			GetComponent<GUIText>().text = "";
			break;
		case 999:
			GetComponent<GUIText>().text = "";
			break;
		case 7:
			GetComponent<GUIText>().text = "";
			break;
		}
	}
}

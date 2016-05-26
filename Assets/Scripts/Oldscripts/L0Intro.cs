using UnityEngine;
using System.Collections;

public class L0Intro : MonoBehaviour {

	public int stopY0;
	public int stopY1;
	public int stopY2;
	public int stopY3;
	public int stopY4;
	public int stopY5;
	public int stopYend;

	public GameObject l1time;
	public GameObject l2time;
	public GameObject l3time;
	public GameObject l4time;
	public GameObject l5time;

	public float moveSpeed;

	public string L0Text;

	public string L1Text;

	public string L2Text;

	public string L3Text;

	public string L4Text;

	public string L5Text;

	public string endText;

	public GameObject points;

	// Use this for initialization
	void Start () {
		stopY0 = 20;
		stopY1 = 19;
		stopY2 = 22;
		stopY3 = 23;
		stopY4 = 24;
		stopY5 = 26;
		stopYend = 128;
		moveSpeed = 0.01f;

		L0Text = "" +
			"\nROBOBUG: THE DEBUGGING GAME" +
			"\n" +
			"\nThe year is 2114, and the " +
			"\nmalicious alien bugs of " +
				"\n<color=red>PLANET Z</color> have launched an " +
			"\nall-out assault on Earth." +
			"\n" +
			"\nAs Earth's greatest" +
			"\nprogrammer, you are the best" +
			"\nhope for humanity. However, " +
			"\nThe bugs have already" +
				"\ninfested your armoured <color=#3dbde8>MECH" +
				"\nSUIT</color>." +
			"\n" +
				"\nIn your <color=#3dbde8>DEBUG ARMOUR</color>, you" +
			"\nmust find and exterminate" +
			"\nall of the bugs that are" +
			"\ncausing the mech suit to " +
			"\nmalfunction." +
			"\n" +
				"\n<color=#3dbde8>PRESS SPACE TO START</color>";

		L1Text = "" +
			"\n<color=#3dbde8>LEVEL 1</color>" +
			"\n" +
			"\nThe mech suit is unable to" +
				"\nperform <color=#3dbde8>basic movement</color>. It" +
			"\nseems like it is having " +
			"\ndifficulty adjusting to " +
				"\nexternal <color=#3dbde8>forces</color> and properly" +
			"\nmeasuring them. As a result," +
			"\nit is making sporadic" +
			"\nand unpredictable movements." +
			"\n" +
			"\nYou must trace through the " +
				"\nsource code of the <color=#3dbde8>AVGFORCE()</color>" +
			"\nfunction and find the bug." +
			"\n" +
				"\n<color=#3dbde8>PRESS SPACE TO START</color>";

		L2Text = "" +
			"\n<color=#3dbde8>LEVEL 2</color>" +
			"\n" +
				"\nThe <color=#3dbde8>MECH LASER CANNON MkII</color>" +
			"\nis a powerful bug-killing" +
			"\nweapon that can target as many" +
			"\nas ten bugs per laser burst." +
			"\n" +
			"\nThe top secret prototype of" +
			"\nthe MkII has been installed " +
			"\ninto your mech suit, but the " +
			"\ncode is confidential and " +
				"\nhidden in a <color=#3dbde8>\"BLACK BOX\"</color>." +
			"\n" +
			"\nA bug has infiltrated " +
			"\nthe code that calculates the" +
				"\n<color=#3dbde8>target distance, energy values," +
				"\nand cannon temperature</color>. You " +
			"\nmust find the bug by using " +
				"\n<color=#3dbde8>tests</color> since you cannot view" +
			"\nthe MkII source code directly." +
			"\n" +
				"\n<color=#3dbde8>PRESS SPACE TO START</color>";

		L3Text = "" +
			"\n<color=#3dbde8>LEVEL 3</color>" +
			"\n" +
			"\nNow that the MkII laser cannon" +
			"\nis functional, the next " +
			"\nessential system to repair is the" +
				"\n<color=#3dbde8>EXTERNAL DEFENSE SYSTEM</color>." +
			"\n" +
				"\nThis system <color=#3dbde8>prioritizes</color> threats" +
			"\nand determines the most dangerous" +
			"\ntarget within range. However, it" +
			"\nis currently non-functional and " +
			"\nthe mech suit cannot identify" +
			"\ncritical threats." +
			"\n" +
			"\nYou must use the mech suit's " +
				"\n<color=#3dbde8>PRINT OUTPUT SYSTEM</color> to obtain" +
			"\nfeedback and locate the bug " +
			"\ninfecting the system." +
			"\n" +
				"\n<color=#3dbde8>PRESS SPACE TO START</color>";

		L4Text = "" +
			"\n<color=#3dbde8>LEVEL 4</color>" +
			"\n" +
			"\nReparing the external defense" +
			"\nsystem has unveiled a new error" +
				"\nlocated in the robot's <color=#3dbde8>VISION" +
				"\nSYSTEM</color>." +
			"\n" +
			"\nThe system that identifies objects" +
			"\nbased on colour and shape is crashing" +
				"\ndue to a bug in the robot's <color=#3dbde8>COLOUR" +
				"\nDATABASE</color>. Without locating the bug" +
			"\nin the database, all vision" +
			"\ncapabilities are currently non-" +
			"\nfunctional." +
			"\n" +
			"\nThe code in the database is massive" +
				"\nand divided into <color=#3dbde8>tables</color> based on colour"+
			"\nTo find the bug, you should use a " +
				"\n<color=#3dbde8>DIVIDE AND CONQUER</color> approach by " +
			"\ncommenting out the tables that do not" +
				"\ncontain the bug. Use the <color=#3dbde8>error message</color> " +
			"\non your heads-up-display to check for" +
			"\nfaulty tables." +
			"\n" +
				"\n<color=#3dbde8>PRESS SPACE TO START</color>";

		L5Text = "" +
			"\n<color=#3dbde8>LEVEL 5</color>" +
			"\n" +
			"\nNow that the vision system is online," +
			"\nthe mech suit needs to be able to" +
			"\nreassess the external defense system " +
				"\nthreat levels based on the <color=#3dbde8>distance</color>" +
			"\nof each object from the mech suit." +
			"\n" +
				"\nThe <color=#3dbde8>COMPARETHREATS()</color> function " +
			"\ncompares potential threats at varying " +
				"\n<color=#3dbde8>coordinates</color> and returns the threat " +
			"\nthat is closest to the mech suit. " +
			"\nHowever, a bug has caused it to" +
			"\nmiscalculate the correct distances." +
			"\n"+
				"\nFortunately, the <color=#3dbde8>DEBUGGER SYSTEM</color> " +
			"\nhas finally loaded and become " +
			"\navailable for you to use. You can" +
				"\n<color=#3dbde8>observe function behavior</color> during " +
				"\nrun-time by setting up <color=#3dbde8>breakpoints</color> " +
			"\nin the code and running the code until" +
			"\nit reaches the next breakpoint." +
			"\nUse this system to follow the code " +
			"\nand discover where the bug is. " +
			"\n"+
				"\n<color=#3dbde8>PRESS SPACE TO START</color>";

		/*endText = "" + 
			"\nVICTORY" +
			"\n" +
			"\nYou've repaired the Mech suit and" +
			"\nsuccessfully eliminated all of the " +
			"\nbugs that were in your way." +
			"\n" +
			"\nCongratulations!" +
			"\n" +
			"\nCreated by: Michael Miljanovic" +
			"\nunder supervision of Dr. Jeremy Bradbury" +
			"\nin SQRLab, University of Ontario" +
			"\nInstitute of Technology" +
			"\n" +
			"\n" +
			"\nCREDITS:" +
			"\n" +
			"\nWebsites" +
			"\n" +
			"\nhttp://www.amon.co\n" +
				"\nhttp://www.opengameart.org\n" + 
				"\nhttp://opsound.org/\n" + "\n" + 
				"\nGraphics Credits\n" + 
				"\nhttp://www.opengameart.org\n" + 
				"\nhttps://openclipart.org\n" +
				"\nGui-Set by Rawdanitsu\n" +
				"\nStephen \"Redshrike\" Challener " +
				"\n- graphic artist \n" +
				"\nWilliam.Thompsonj " +
				"\n- contributor.\n" + 
				"\nA robot \nby Anton Yu. \nFrom 0.18 OCAL database.\n" +
				"\nglyph.png \nby Jinn (Submitted by Andrettin)\n" +
				"\nFREE Keyboard and controllers \nprompts pack" +
				"\nby xelu\n" +
				"\nSpace Gui in various colors\nby Rawdanitsu\n" +
				"\nIcons by phaelax\n" +
				"\nangled metal tracks on an \nelectronic circuit board\nfrom creative103.com\n" + 
				"\n" + 
				"\nMusic Credits from http://opsound.org/\n" + 
				"\n(IT) ANTI-MATTER(S) \nby LDX#40\n" +
				"\nTHIRTY \nby _AA_\n" + 
				"\nACD8 \nby PERAMIDES\n" +
				"\nFILTHYFILTER \nby Kid Cholera's VASCULOID\n" +
				"\nNIGHTTIME \nby Kid Cholera's VASCULOID\n" +
				"\nON THE DOWNLOAD \nby Kid Cholera's VASCULOID\n" + 
				"\nSPIDERTWO \nby DAVE HOWES\n" + 
				"\nAudio Credits\n" + 
				"\nSounds from Freesound:\n" + 
				"\nalien_screecn_1.wav \nby CosmicD\n" +
				"\nconcrete_step_3.wav \nby movingplaid\n" + 
				"\nzoom up 1 (quicker delay).wav \nby Chriddof\n" +
				"\nWhoosh_Swish_03.wav \nby mich3d\n" + 
				"\nError.wav \nby Autistic Lucario\n" +
				"\n\n" +
				"\nThese works are licensed under a" + 
				"\nCreative Commons " + 
				"\nAttribution-ShareAlike3.0 " +
				"\nUnported License.\n" +
				"\n" +
				"\n" +
				"\nThanks for playing!" +
				"\nFINAL SCORE: " + points.GetComponent<TextMesh>().text + 
				"\nPress Escape to quit";*/
	}
	
	// Update is called once per frame
	void Update () {
		endText = "" + 
			"\n<color=#3dbde8>VICTORY</color>" +
				"\n" +
				"\nYou've repaired the Mech suit and" +
				"\nsuccessfully eliminated all of the " +
				"\nbugs that were in your way." +
				"\n" +
				"\n<color=#3dbde8>Congratulations!</color>" +
				"\n" +
				"\nCreated by: Michael Miljanovic" +
				"\nunder supervision of Dr. Jeremy Bradbury" +
				"\nin SQRLab, University of Ontario" +
				"\nInstitute of Technology" +
				"\n" +
				"\n" +
				"\nCREDITS:" +
				"\n" +
				"\nWebsites" +
				"\n" +
				"\nhttp://www.amon.co\n" +
				"\nhttp://www.opengameart.org\n" + 
				"\nhttp://opsound.org/\n" + "\n" + 
				"\nGraphics Credits\n" + 
				"\nhttp://www.opengameart.org\n" + 
				"\nhttps://openclipart.org\n" +
				"\nGui-Set by Rawdanitsu\n" +
				"\nStephen \"Redshrike\" Challener " +
				"\n- graphic artist \n" +
				"\nWilliam.Thompsonj " +
				"\n- contributor.\n" + 
				"\nA robot \nby Anton Yu. \nFrom 0.18 OCAL database.\n" +
				"\nglyph.png \nby Jinn (Submitted by Andrettin)\n" +
				"\nFREE Keyboard and controllers \nprompts pack" +
				"\nby xelu\n" +
				"\nSpace Gui in various colors\nby Rawdanitsu\n" +
				"\nIcons by phaelax\n" +
				"\nangled metal tracks on an \nelectronic circuit board\nfrom creative103.com\n" + 
				"\n" + 
				"\nMusic Credits from http://opsound.org/\n" + 
				"\n(IT) ANTI-MATTER(S) \nby LDX#40\n" +
				"\nTHIRTY \nby _AA_\n" + 
				"\nACD8 \nby PERAMIDES\n" +
				"\nFILTHYFILTER \nby Kid Cholera's VASCULOID\n" +
				"\nNIGHTTIME \nby Kid Cholera's VASCULOID\n" +
				"\nON THE DOWNLOAD \nby Kid Cholera's VASCULOID\n" + 
				"\nSPIDERTWO \nby DAVE HOWES\n" + 
				"\nAudio Credits\n" + 
				"\nSounds from Freesound:\n" + 
				"\nalien_screecn_1.wav \nby CosmicD\n" +
				"\nconcrete_step_3.wav \nby movingplaid\n" + 
				"\nzoom up 1 (quicker delay).wav \nby Chriddof\n" +
				"\nWhoosh_Swish_03.wav \nby mich3d\n" + 
				"\nError.wav \nby Autistic Lucario\n" +
				"\n\n" +
				"\nThese works are licensed under a" + 
				"\nCreative Commons " + 
				"\nAttribution-ShareAlike3.0 " +
				"\nUnported License.\n" +
				"\n" +
				"\n" +
				"\nThanks for playing!" +
				"\nFINAL SCORE: " + points.GetComponent<TextMesh>().text + 
				"\nLevel 1 Time: " + l1time.GetComponent<TextMesh>().text + " seconds" + 
				"\nLevel 2 Time: " + l2time.GetComponent<TextMesh>().text + " seconds" + 
				"\nLevel 3 Time: " + l3time.GetComponent<TextMesh>().text + " seconds" + 
				"\nLevel 4 Time: " + l4time.GetComponent<TextMesh>().text + " seconds" + 
				"\nLevel 5 Time: " + l5time.GetComponent<TextMesh>().text + " seconds" + 
				"\nYou can now return to the survey website." +
				"\nPress Escape to Quit";


		TextMesh txt = GetComponent<TextMesh> ();
		txt.color = Color.white;
		if (txt.text == "start0") {
			txt.text = L0Text;
		}
		else if (txt.text == "start1") {
			txt.text = L1Text;
		}
		else if (txt.text == "start2") {
			txt.text = L2Text;
		}
		else if (txt.text == "start3") {
			txt.text = L3Text;
		}
		else if (txt.text == "start4") {
			txt.text = L4Text;
		}
		else if (txt.text == "start5") {
			txt.text = L5Text;
		}
		else if (txt.text == "end") {
			this.transform.position = new Vector3(100f,-3f,0f);
			txt.text = endText;
		//	Output to desktop
			string username = System.Environment.GetEnvironmentVariable("username");
			
			using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\Users\" + username + @"\desktop\RoboLOG.txt"))
				{
				file.WriteLine(l1time.GetComponent<TextMesh>().text);
				file.WriteLine(l2time.GetComponent<TextMesh>().text);
				file.WriteLine(l3time.GetComponent<TextMesh>().text);
				file.WriteLine(l4time.GetComponent<TextMesh>().text);
				file.WriteLine(l5time.GetComponent<TextMesh>().text);
			}

		}
	/*	else if (Input.GetButtonDown ("Jump")) {
			this.transform.position = new Vector3(100f,0f,0f);
			txt.text = "L";
		}*/

	//	TextMesh txt = GetComponent<TextMesh> ();
		if (txt.text == L0Text && this.transform.position.y < stopY0) {
			this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + moveSpeed, this.transform.position.z);
		}
		else if (txt.text == L1Text && this.transform.position.y < stopY1) {
			this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + moveSpeed, this.transform.position.z);
		}
		else if (txt.text == L2Text && this.transform.position.y < stopY2) {
			this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + moveSpeed, this.transform.position.z);
		}
		else if (txt.text == L3Text && this.transform.position.y < stopY3) {
			this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + moveSpeed, this.transform.position.z);
		}
		else if (txt.text == L4Text && this.transform.position.y < stopY4) {
			this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + moveSpeed, this.transform.position.z);
		}
		else if (txt.text == L5Text && this.transform.position.y < stopY5) {
			this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + moveSpeed, this.transform.position.z);
		}
		else if (txt.text == endText && this.transform.position.y < stopYend) {
			this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + moveSpeed, this.transform.position.z);
		}
		if (Input.GetAxisRaw ("Vertical") == 1) {
						moveSpeed = 0.1f;
				} else {
					moveSpeed = 0.01f;
				}
	}
	void FixedUpdate(){

	}
}

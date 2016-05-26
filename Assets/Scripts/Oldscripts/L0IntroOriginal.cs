using UnityEngine;
using System.Collections;

public class L0IntroOriginal : MonoBehaviour {

	public int stopY0;
	public int stopY1;
	public int stopY2;
	public int stopY3;
	public int stopY4;
	public int stopY5;
	public int stopYend;

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
			"\nPLANET Z have launched an " +
			"\nall-out assault on Earth." +
			"\n" +
			"\nAs Earth's greatest" +
			"\nprogrammer, you are the best" +
			"\nhope for humanity. However, " +
			"\nThe bugs have already" +
			"\ninfested your armoured MECH" +
			"\nSUIT." +
			"\n" +
			"\nIn your DEBUG ARMOUR, you" +
			"\nmust find and exterminate" +
			"\nall of the bugs that are" +
			"\ncausing the mech suit to " +
			"\nmalfunction." +
			"\n" +
			"\nPRESS SPACE TO START";

		L1Text = "" +
			"\nLEVEL 1" +
			"\n" +
			"\nThe mech suit is unable to" +
			"\nperform basic movement. It" +
			"\nseems like it is having " +
			"\ndifficulty adjusting to " +
			"\nexternal forces and properly" +
			"\nmeasuring them. As a result," +
			"\nit is making sporadic" +
			"\nand unpredictable movements." +
			"\n" +
			"\nYou must trace through the " +
			"\nsource code of the AVGFORCE()" +
			"\nfunction and find the bug." +
			"\n" +
			"\nPRESS SPACE TO START";

		L2Text = "" +
			"\nLEVEL 2" +
			"\n" +
			"\nThe MECH LASER CANNON MkII" +
			"\nis a powerful bug-killing" +
			"\nweapon that can target as many" +
			"\nas ten bugs per laser burst." +
			"\n" +
			"\nThe top secret prototype of" +
			"\nthe MkII has been installed " +
			"\ninto your mech suit, but the " +
			"\ncode is confidential and " +
			"\nhidden in a \"BLACK BOX\"." +
			"\n" +
			"\nThe bugs have infiltrated " +
			"\nthe code that calculates the" +
			"\ntarget distance, energy values," +
			"\nand cannon temperature. You " +
			"\nmust find the bugs by using " +
			"\ntests since you cannot view" +
			"\nthe MkII source code directly." +
			"\n" +
			"\nPRESS SPACE TO START";

		L3Text = "" +
			"\nLEVEL 3" +
			"\n" +
			"\nNow that the MkII laser cannon" +
			"\nis functional, the next " +
			"\nessential system to repair is the" +
			"\nEXTERNAL DEFENSE SYSTEM." +
			"\n" +
			"\nThis system prioritizes threats" +
			"\nand determines the most dangerous" +
			"\ntarget within range. However, it" +
			"\nis currently non-functional and " +
			"\nthe mech suit cannot identify" +
			"\ncritical threats." +
			"\n" +
			"\nYou must use the mech suit's " +
			"\nPRINT OUTPUT SYSTEM to obtain" +
			"\nfeedback and locate the bug " +
			"\ninfecting the system." +
			"\n" +
			"\nPRESS SPACE TO START";

		L4Text = "" +
			"\nLEVEL 4" +
			"\n" +
			"\nReparing the external defense" +
			"\nsystem has unveiled a new error" +
			"\nlocated in the robot's VISION" +
			"\nSYSTEM." +
			"\n" +
			"\nThe system that identifies objects" +
			"\nbased on colour and shape is crashing" +
			"\ndue to a bug in the robot's COLOUR" +
			"\nDATABASE. Without locating the bug" +
			"\nin the database, all vision" +
			"\ncapabilities are currently non-" +
			"\nfunctional." +
			"\n" +
			"\nThe code in the database is massive" +
			"\nand divided into tables based on colour"+
			"\nTo find the bug, you should use a " +
			"\nDIVIDE AND CONQUER approach by " +
			"\ncommenting out the tables that do not" +
			"\ncontain the bug. Use the error message " +
			"\nat the bottom of the code to check for" +
			"\nfaulty tables." +
			"\n" +
			"\nPRESS SPACE TO START";

		L5Text = "" +
			"\nLEVEL 5" +
			"\n" +
			"\nNow that the vision system is online," +
			"\nthe mech suit needs to be able to" +
			"\nreassess the external defense system " +
			"\nthreat levels based on the distance" +
			"\nof each object from the mech suit." +
			"\n" +
			"\nThe COMPARETHREATS() function " +
			"\ncompares potential threats at varying " +
			"\ncoordinates and returns the threat " +
			"\nthat is closest to the mech suit. " +
			"\nHowever, a bug has caused it to" +
			"\nmiscalculate the correct distances." +
			"\n"+
			"\nFortunately, the DEBUGGER SYSTEM " +
			"\nhas finally loaded and become " +
			"\navailable for you to use. You can" +
			"\n observe function behavior during " +
			"\nrun-time by setting up breakpoints " +
			"\nin the code and running the code until" +
			"\nit reaches the next breakpoint." +
			"\nUse this system to follow the code " +
			"\nand discover where the bug is. " +
			"\n"+
			"\nPRESS SPACE TO START";

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
				"\nPress Escape to quit";


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

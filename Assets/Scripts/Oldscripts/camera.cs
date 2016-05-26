using UnityEngine;
using System.Collections;

public class camera : MonoBehaviour {

	public GameObject target;

	//levels/screens
	public GameObject lintro;
	public Vector3 intropos;

	public GameObject l10;
	public GameObject l1;

	public GameObject l20;
	public GameObject l2;
	public GameObject l2a;
	public GameObject l2b;
	public GameObject l2c;
	public GameObject l2d;
	public GameObject l2e;
	public GameObject l2f;

	public GameObject l30;
	public GameObject l3;
	public GameObject l31;

	public GameObject l40;
	public GameObject l4;
	public GameObject l4bla; //1
	public GameObject l4blu; //2
	public GameObject l4br; //3
	public GameObject l4c; //4
	public GameObject l4gree; //5
	public GameObject l4grey; //6
	public GameObject l4m; //7
	public GameObject l4n; //8
	public GameObject l4r; //9
	public GameObject l4o; //10
	public GameObject l4w; //11
	public GameObject l4y; //12

	public GameObject l5;
	public GameObject l50;

	public GameObject l6;
	public GameObject l60;
	public GameObject l6a;
	public GameObject l6b;
	public GameObject l6c;
	public GameObject l6d;

	public GameObject l7;

	public GameObject end;

	public GameObject levelText;

	public GameObject l2Objective1;
	bool L2O1;
		bool L2O2;
			bool L2O3;
	public GameObject l2Objective2;
	public GameObject l2Objective3;
	public GameObject l3Report;

	public GameObject l5report;

	public GameObject l6report;

	public GameObject lastbug;
	public GameObject verylastbug;

	public GameObject success;

	public bool xvalue = true;
	public int level = -1;
	public int stars = 0;

	public AudioClip texttheme;
	public AudioClip introtheme;
	public AudioClip theme1;
	public AudioClip theme2;
	public AudioClip theme3;
	public AudioClip theme4;
	public AudioClip theme5;

	bool mute = false;

	//Vector3 startScreen = new Vector3 (27, 4, -5f);
	Vector3 instructScreen = new Vector3 (28, -6, -5f);
	Vector3 level1Start = new Vector3(-3f,15.12f,0);
	Vector3 level3Start = new Vector3(-97f,-28.1f,0);
	Vector3 level4Start = new Vector3(-146f,-68.8f,0);
	Vector3 level5Start = new Vector3(-252f,109.2f,0);
	Vector3 level6Start = new Vector3(-286f,73.2f,0);
	Vector3 level6cStart = new Vector3(-343f,109.2f,0);
	Vector3 lastlevelstart = new Vector3 (-424,80,0);

	int level6testVBoundary = -295;
	int level6testHBoundary = 50;

	int level6debugVBoundary = -358;

	int level2LeftBoundary = -37;
	/*int level21TopBoundary = -8;
	int level23TopBoundary = 11;
	int level22LeftBoundary = -55;*/

	int l3boundary = -100;

	int level4topVBoundary = 86;
	int level4midVBoundary = 36;
	int level4botVBoundary = -16;
	int level4lefHBoundary = -175;
	int level4midHBoundary = -135;
	int level4rigHBoundary = -95;

	float delayamount = 1.5f;
	float delaytime = 0f;

	// Use this for initialization
	void Start () {
		intropos = new Vector3(100f,0f,-5f);
		AudioSource ad = GetComponent<AudioSource> ();
		ad.Play();
		L2O1 = false;
		L2O2 = false;
		L2O3 = false;
	}
	
	// Update is called once per frame
	void Update () {

		AudioSource ad = GetComponent<AudioSource> ();
		TextMesh tm = levelText.GetComponent<TextMesh> ();
		TextMesh ltxt = lintro.GetComponent<TextMesh> ();


	//	if (Input.GetKeyDown ("m")) {
	//		mute = !mute;
	//	}
		AudioListener.volume = 0f;
			if (mute == false){
				if (ad.clip == texttheme) {
					ad.volume = 0.2f;
				} else {
					ad.volume = 0.05f;
				}
			}
		if (mute) {
			ad.volume = 0f;
				}







		//level determinants
		//level 1
		if (target.transform.position.x < -15 && level==1) {
			level = 200;
			tm.text = "200";
		//	ltxt.text = "start2";
			ad.Pause();
			ad.clip = introtheme;
			ad.Play();
		}

		//level 2
	/*	if (level == 2) {
			if (target.transform.position.x < level22LeftBoundary){
				if (target.transform.position.y > level23TopBoundary){
					level = 25;
				}
				else if (target.transform.position.y > level21TopBoundary){
					level = 23;
				}
				else{
					level = 21;				
				}
			}
			else if (target.transform.position.x < level2LeftBoundary){
				if (target.transform.position.y > level23TopBoundary){
					level = 26;
				}
				else if (target.transform.position.y > level21TopBoundary){
					level = 24;
				}
				else{
					level = 22;				
				}
			}
		}*/
		if (21<=level && level <=26 && target.transform.position.x > level2LeftBoundary){
			level = 2;
		}

		//level 3
		if (level == 2 && l2Objective1.GetComponent<TextMesh> ().text == "ERROR!!!") {
			L2O1 = true;
				}
		if (level == 2 && l2Objective2.GetComponent<TextMesh> ().text == "ERROR!!!") {
			L2O2 = true;
		}
		if (level == 2 && l2Objective3.GetComponent<TextMesh> ().text == "ERROR!!!") {
			L2O3 = true;
		}
		if (level == 2 && (L2O1 || L2O2 || L2O3)){
			if (delaytime == 0f){
				delaytime = Time.time + delayamount;
			}
			else if (Time.time > delaytime){
				delaytime = 0f;
				level = 300;
				tm.text = "300";
		//		ltxt.text = "start3";
				ad.Pause();
				ad.clip = introtheme;
				ad.Play();
			}
		}

		//level 4
		if (level == 3 && target.transform.position.y < l3boundary) {
			level = 31;
		}
		if (level == 31 && target.transform.position.y > l3boundary) {
			level = 3;
		}
		if (level == 3 && l3Report.GetComponent<TextMesh>().text == "Correct!"){
			if (delaytime == 0f){
				delaytime = Time.time + delayamount;
			}
			else if (Time.time > delaytime){
				delaytime = 0f;
				level = 400;
				tm.text = "400";
		//		ltxt.text = "start4";
				ad.Pause();
				ad.clip = introtheme;
				ad.Play();
			}
		}
		if (level == 4) {
			if (target.transform.position.y < level4botVBoundary){
				if (target.transform.position.x < level4lefHBoundary){level=412;}
			}
			else if (target.transform.position.y < level4midVBoundary){
				if (target.transform.position.x < level4lefHBoundary){level=49;}
				else if (target.transform.position.x < level4midHBoundary) {level=410;}
				else{level=411;}
			}
			else if (target.transform.position.y < level4topVBoundary){
				if (target.transform.position.x < level4lefHBoundary){level=45;}
				else if (target.transform.position.x < level4midHBoundary) {level=46;}
				else if (target.transform.position.x < level4rigHBoundary) {level=47;}
				else{level=48;}
			}
			else {
				if (target.transform.position.x < level4lefHBoundary){level=41;				}
				else if (target.transform.position.x < level4midHBoundary) {level=42;}
				else if (target.transform.position.x < level4rigHBoundary) {level=43;}
				else{level=44;}
			}
		}
		if ((41 <= level && level <= 49) || (410 <= level && level <= 412)) {
			if (target.transform.position.x > 0){
				tm.text = "500";
				level = 500;
		//		ltxt.text = "start5";
				ad.Pause();
				ad.clip = introtheme;
				ad.Play();
			}
			else if (target.transform.position.y < level4botVBoundary){
				if (target.transform.position.x > level4lefHBoundary){level=4;}
			}
		
		}

		if (level == 5){
		//	if (l5report.GetComponent<TextMesh>().text == "Correct!"){level = 60; tm.text = "6";}
			if (l5report.GetComponent<TextMesh>().text == "Correct!"){ad.Pause();
				ad.clip = introtheme;
				ad.Play();
			//	ltxt.text = "end";
				level = 999; tm.text = "999";}
		}

		if (level == 6) {
			if (target.transform.position.x < level6testVBoundary){
				if (target.transform.position.y > level6testHBoundary){
					level = 62;
				}
				else{
					level = 61;
				}
			}
		}
		if (level == 61 || level == 62) {
			if (target.transform.position.x > level6testVBoundary){
				level = 6;
			}
		}

		if (level == 6 && l6report.GetComponent<Renderer>().enabled == true){
			if (delaytime == 0f){
				delaytime = Time.time + delayamount;
			}
			else if (Time.time > delaytime){
				delaytime = 0f;
				level = 63;
				target.transform.position = level6cStart;
			}

		}

		if (level == 63 && target.transform.position.x < level6debugVBoundary){
			level = 64;
		}
		if (level == 64 && target.transform.position.x > level6debugVBoundary){
			level = 63;
		}

		if (level == 63 && lastbug.GetComponent<Renderer>().enabled == true) {

			if (delaytime == 0f){
				delaytime = Time.time + delayamount;
			}
			else if (Time.time > delaytime){
				delaytime = 0f;
				level = 7;
				target.transform.position = lastlevelstart;
			}
		}

		if (level == 7 && verylastbug.GetComponent<Renderer>().enabled == true) {
			if (delaytime == 0f){
				delaytime = Time.time + delayamount;
			}
			else if (Time.time > delaytime){
				delaytime = 0f;
				level = 999;
				tm.text = "999";
				ad.Pause();
				ad.clip = texttheme;
				ad.Play();
				}
			}



		//camera positions
		switch (level){ 
		case -1:
			tm.text = "0";
			//camera.transform.position = startScreen;
			GetComponent<Camera>().transform.position = intropos;
			GetComponent<Camera>().orthographicSize = 6;
			if (Input.GetButtonDown("Jump")){
				level = 0;
			}
			if (Input.GetKeyDown("3")){
				ltxt.transform.position = new Vector3(100f,-3f,0f);
				level = 300;
				tm.text = "300";
				ltxt.text = "start3";
				ad.Pause();
				ad.clip = texttheme;
				ad.Play();
			}
			if (Input.GetKeyDown("4")){
				ltxt.transform.position = new Vector3(100f,-3f,0f);
				level = 400;
				tm.text = "400";
				ltxt.text = "start4";
				ad.Pause();
				ad.clip = texttheme;
				ad.Play();
			}
			if (Input.GetKeyDown("5")){
				ltxt.transform.position = new Vector3(100f,-3f,0f);
				level = 500;
				tm.text = "500";
				ltxt.text = "start5";
				ad.Pause();
				ad.clip = texttheme;
				ad.Play();
			}
		/*	if (Input.GetKeyDown("6")){
				level = 60;
				tm.text = "6";
			}*/
			break;
		case 0:
			GetComponent<Camera>().transform.position = instructScreen;
			GetComponent<Camera>().orthographicSize = 6;
			if (Input.GetButtonDown("Jump")){
				level = 100;
				ltxt.text = "start1";
				ltxt.transform.position = new Vector3(100f,-3f,0f);
				ad.Pause();
				ad.clip = texttheme;
				ad.Play();
			}
			break;
		case 100:
			GetComponent<Camera>().transform.position = intropos;
			GetComponent<Camera>().orthographicSize = 6;
			if (Input.GetButtonDown("Jump")){
				level = 10;
				ltxt.text = "L";
				ltxt.transform.position = new Vector3(100f,-3f,0f);
				ad.Pause();
				ad.clip = introtheme;
				ad.Play();

			}
			break;
		case 10:
			GetComponent<Camera>().transform.position = new Vector3 (l10.transform.position.x, l10.transform.position.y, -5f);
			GetComponent<Camera>().orthographicSize = 8;
			if (Input.GetButtonDown("Jump")){
				level = 1;
				target.transform.position = level1Start;
				tm.text = "1";
				ad.Pause();
				ad.clip = theme1;
				ad.Play();
			}
			break;
		case 1:
			GetComponent<Camera>().transform.position = new Vector3 (l1.transform.position.x+4, target.transform.position.y, -5f);
			GetComponent<Camera>().orthographicSize = 7;
			break;

		case 200:
			GetComponent<Camera>().orthographicSize = 8;
			GetComponent<Camera>().transform.position = new Vector3(success.transform.position.x, success.transform.position.y, -5f);
			if (Input.GetButtonDown("Jump")){
				ltxt.text = "start2";
				level = 201;
				ad.Pause();
				ad.clip = texttheme;
				ad.Play();
			}
			break;
		case 201:
			GetComponent<Camera>().orthographicSize = 6;
			GetComponent<Camera>().transform.position = intropos;
			if (Input.GetButtonDown("Jump")){
				level = 20;
				ltxt.text = "L";
				ltxt.transform.position = new Vector3(100f,-3f,0f);
				ad.Pause();
				ad.clip = introtheme;
				ad.Play();
			}
			break;

		case 20:
			GetComponent<Camera>().transform.position = new Vector3 (l20.transform.position.x, l20.transform.position.y, -5f);
			GetComponent<Camera>().orthographicSize = 8;
			if (Input.GetButtonDown("Jump")){
				level = 2;
				tm.text = "2";
				ad.Pause();
				ad.clip = theme2;
				ad.Play();
			}
			break;
		case 2:
			GetComponent<Camera>().transform.position = new Vector3 (l2.transform.position.x+4, target.transform.position.y, -5f);
			GetComponent<Camera>().orthographicSize = 7;
			break;
		case 21:
			GetComponent<Camera>().transform.position = new Vector3 (l2a.transform.position.x, l2a.transform.position.y, -5f);
			GetComponent<Camera>().orthographicSize = 6;
			break;
		case 22:
			GetComponent<Camera>().transform.position = new Vector3 (l2b.transform.position.x, l2b.transform.position.y, -5f);
			GetComponent<Camera>().orthographicSize = 6;
			break;
		case 23:
			GetComponent<Camera>().transform.position = new Vector3 (l2c.transform.position.x, l2c.transform.position.y, -5f);
			GetComponent<Camera>().orthographicSize = 6;
			break;
		case 24:
			GetComponent<Camera>().transform.position = new Vector3 (l2d.transform.position.x, l2d.transform.position.y, -5f);;
			GetComponent<Camera>().orthographicSize = 6;
			break;
		case 25:
			GetComponent<Camera>().transform.position = new Vector3 (l2e.transform.position.x, l2e.transform.position.y, -5f);;
			GetComponent<Camera>().orthographicSize = 6;
			break;
		case 26:
			GetComponent<Camera>().transform.position = new Vector3 (l2f.transform.position.x, l2f.transform.position.y, -5f);;
			GetComponent<Camera>().orthographicSize = 6;
			break;

		case 300:
			GetComponent<Camera>().orthographicSize = 8;
			GetComponent<Camera>().transform.position = new Vector3(success.transform.position.x, success.transform.position.y, -5f);
			if (Input.GetButtonDown("Jump")){
				ltxt.text = "start3";
				level = 301;
				ad.Pause();
				ad.clip = texttheme;
				ad.Play();
			}
			break;
		case 301:
			GetComponent<Camera>().orthographicSize = 6;
			GetComponent<Camera>().transform.position = intropos;
			if (Input.GetButtonDown("Jump")){
				level = 30;
				ltxt.text = "L";
				ltxt.transform.position = new Vector3(100f,-3f,0f);
				ad.Pause();
				ad.clip = introtheme;
				ad.Play();
			}
			break;
		case 30:
			GetComponent<Camera>().transform.position = new Vector3 (l30.transform.position.x, l30.transform.position.y, -5f);
			GetComponent<Camera>().orthographicSize = 8;
			if (Input.GetButtonDown("Jump")){
				level = 3;
				tm.text = "3";
				target.transform.position = level3Start;
				ad.Pause();
				ad.clip = theme3;
				ad.Play();
			}
			break;
		case 3:
			GetComponent<Camera>().transform.position = new Vector3 (l3.transform.position.x+4, target.transform.position.y, -5f);
			GetComponent<Camera>().orthographicSize = 7;
			break;
		case 31:
			GetComponent<Camera>().transform.position = new Vector3 (l31.transform.position.x+4, target.transform.position.y, -5f);
			GetComponent<Camera>().orthographicSize = 7;
			break;

		case 400:
			GetComponent<Camera>().orthographicSize = 8;
			GetComponent<Camera>().transform.position = new Vector3(success.transform.position.x, success.transform.position.y, -5f);
			if (Input.GetButtonDown("Jump")){
				ltxt.text = "start4";
				level = 401;
				ad.Pause();
				ad.clip = texttheme;
				ad.Play();
			}
			break;
		case 401:
			GetComponent<Camera>().orthographicSize = 6;
			GetComponent<Camera>().transform.position = intropos;
			if (Input.GetButtonDown("Jump")){
				level = 40;
				ltxt.text = "L";
				ltxt.transform.position = new Vector3(100f,-3f,0f);
				ad.Pause();
				ad.clip = introtheme;
				ad.Play();
			}
			break;
		case 40:
			GetComponent<Camera>().transform.position = new Vector3 (l40.transform.position.x, l40.transform.position.y, -5f);
			GetComponent<Camera>().orthographicSize = 8;
			if (Input.GetButtonDown("Jump")){
				level = 4;
				tm.text = "4";
				target.transform.position = level4Start;
				ad.Pause();
				ad.clip = theme4;
				ad.Play();
			}
			break;
		case 4:
			GetComponent<Camera>().transform.position = new Vector3 (l4.transform.position.x+4, target.transform.position.y, -5f);;
			GetComponent<Camera>().orthographicSize = 7;
			break;
		case 41:
			GetComponent<Camera>().transform.position = new Vector3 (l4bla.transform.position.x+4, target.transform.position.y, -5f);;
			GetComponent<Camera>().orthographicSize = 7;
			break;
		case 42:
			GetComponent<Camera>().transform.position = new Vector3 (l4blu.transform.position.x+4, target.transform.position.y, -5f);;
			GetComponent<Camera>().orthographicSize = 7;
			break;
		case 43:
			GetComponent<Camera>().transform.position = new Vector3 (l4br.transform.position.x+4, target.transform.position.y, -5f);;
			GetComponent<Camera>().orthographicSize = 7;
			break;
		case 44:
			GetComponent<Camera>().transform.position = new Vector3 (l4c.transform.position.x+4, target.transform.position.y, -5f);;
			GetComponent<Camera>().orthographicSize = 7;
			break;
		case 45:
			GetComponent<Camera>().transform.position = new Vector3 (l4gree.transform.position.x+4, target.transform.position.y, -5f);;
			GetComponent<Camera>().orthographicSize = 7;
			break;
		case 46:
			GetComponent<Camera>().transform.position = new Vector3 (l4grey.transform.position.x+4, target.transform.position.y, -5f);;
			GetComponent<Camera>().orthographicSize = 7;
			break;
		case 47:
			GetComponent<Camera>().transform.position = new Vector3 (l4m.transform.position.x+4, target.transform.position.y, -5f);;
			GetComponent<Camera>().orthographicSize = 7;
			break;
		case 48:
			GetComponent<Camera>().transform.position = new Vector3 (l4n.transform.position.x+4, target.transform.position.y, -5f);;
			GetComponent<Camera>().orthographicSize = 7;
			break;
		case 49:
			GetComponent<Camera>().transform.position = new Vector3 (l4o.transform.position.x+4, target.transform.position.y, -5f);;
			GetComponent<Camera>().orthographicSize = 7;
			break;
		case 410:
			GetComponent<Camera>().transform.position = new Vector3 (l4r.transform.position.x+4, target.transform.position.y, -5f);;
			GetComponent<Camera>().orthographicSize = 7;
			break;
		case 411:
			GetComponent<Camera>().transform.position = new Vector3 (l4w.transform.position.x+4, target.transform.position.y, -5f);;
			GetComponent<Camera>().orthographicSize = 7;
			break;
		case 412:
			GetComponent<Camera>().transform.position = new Vector3 (l4y.transform.position.x+4, target.transform.position.y, -5f);;
			GetComponent<Camera>().orthographicSize = 7;
			break;

		case 500:
			GetComponent<Camera>().orthographicSize = 8;
			GetComponent<Camera>().transform.position = new Vector3(success.transform.position.x, success.transform.position.y, -5f);
			if (Input.GetButtonDown("Jump")){
				ltxt.text = "start5";
				level = 501;
				ad.Pause();
				ad.clip = texttheme;
				ad.Play();
			}
			break;
		case 501:
			GetComponent<Camera>().orthographicSize = 6;
			GetComponent<Camera>().transform.position = intropos;
			if (Input.GetButtonDown("Jump")){
				level = 50;
				ltxt.text = "L";
				ltxt.transform.position = new Vector3(100f,-3f,0f);
				ad.Pause();
				ad.clip = introtheme;
				ad.Play();
			}
			break;
		case 5:
			GetComponent<Camera>().transform.position = new Vector3 (l5.transform.position.x+4, target.transform.position.y, -5f);;
			GetComponent<Camera>().orthographicSize = 7;
			break;
		case 50:
			GetComponent<Camera>().transform.position = new Vector3 (l50.transform.position.x, l50.transform.position.y, -5f);
			GetComponent<Camera>().orthographicSize = 8;
			if (Input.GetButtonDown("Jump")){
				level = 5;
				tm.text = "5";
				target.transform.position = level5Start;
				ad.Pause();
				ad.clip = theme5;
				ad.Play();
			}
			break;

		case 6:
			GetComponent<Camera>().transform.position = new Vector3 (l6.transform.position.x+2, target.transform.position.y, -5f);;
			GetComponent<Camera>().orthographicSize = 5;
			break;
		case 60:
			GetComponent<Camera>().transform.position = new Vector3 (l60.transform.position.x, l60.transform.position.y, -5f);
			GetComponent<Camera>().orthographicSize = 6;
			if (Input.GetButtonDown("Jump")){
				level = 6;
				target.transform.position = level6Start;
			}
			break;
		case 61:
			GetComponent<Camera>().transform.position = new Vector3 (l6a.transform.position.x, l6a.transform.position.y, -5f);
			GetComponent<Camera>().orthographicSize = 6;
			break;
		case 62:
			GetComponent<Camera>().transform.position = new Vector3 (l6b.transform.position.x, l6b.transform.position.y, -5f);
			GetComponent<Camera>().orthographicSize = 6;
			break;
		case 63:
			GetComponent<Camera>().transform.position = new Vector3 (l6c.transform.position.x+2, target.transform.position.y, -5f);;
			GetComponent<Camera>().orthographicSize = 5;
			break;
		case 64:
			GetComponent<Camera>().transform.position = new Vector3 (l6d.transform.position.x, target.transform.position.y, -5f);
			GetComponent<Camera>().orthographicSize = 5;
			break;

		case 7:
			GetComponent<Camera>().transform.position = new Vector3 (l7.transform.position.x+2, target.transform.position.y, -5f);
			GetComponent<Camera>().orthographicSize = 5;
			break;

		case 999:
			GetComponent<Camera>().orthographicSize = 8;
			GetComponent<Camera>().transform.position = new Vector3(success.transform.position.x, success.transform.position.y, -5f);
			if (Input.GetButtonDown("Jump")){
				ltxt.text = "end";
				level = 9999;
				ad.Pause();
				ad.clip = texttheme;
				ad.Play();
			}
			break;
		case 9999:
		//	camera.transform.position = new Vector3 (end.transform.position.x, end.transform.position.y, -5f);;
	//		camera.orthographicSize = 3;
			GetComponent<Camera>().transform.position = intropos;
			GetComponent<Camera>().orthographicSize = 6;
			break;
		}

	}
}

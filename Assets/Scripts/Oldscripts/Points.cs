using UnityEngine;
using System.Collections;

public class Points : MonoBehaviour
{

		public GameObject level;
		public GameObject starttimer;
		public GameObject endtext;

	public GameObject l1time;
	public GameObject l2time;
	public GameObject l3time;
	public GameObject l4time;
	public GameObject l5time;

		public int levelnum;
		public int currentlevel;
		public int starttime;
		public int points = 0;
	int pointsGet = 0;
		public GameObject falsepositive;
		int pointpenalty = 500;
	public GameObject pointnumber;

		int secs;

		// Use this for initialization
		void Start ()
		{
				currentlevel = 0;
				this.GetComponent<GUIText> ().text = "";
		}
	
		// Update is called once per frame
		void Update ()
		{
				pointnumber.GetComponent<TextMesh> ().text = System.Convert.ToString (points);
				levelnum = System.Convert.ToInt16 (level.GetComponent<TextMesh> ().text);
				if (falsepositive.GetComponent<TextMesh> ().text == "Caught") {
					points -= pointpenalty;
						falsepositive.GetComponent<TextMesh> ().text = "Pointed";
				}
				if (levelnum > 0) {
					if (levelnum < 100){this.GetComponent<GUIText> ().text = System.Convert.ToString (points) + " points";}
					else{this.GetComponent<GUIText> ().text = "";}
					if (levelnum != currentlevel) {
								currentlevel = levelnum;
								switch (levelnum) {
								case 1:
										starttime = (int)Time.time;
										starttimer.GetComponent<GUIText> ().text = "Level " + System.Convert.ToString (levelnum) + " start time = " + System.Convert.ToString ((int)Time.time / 60) + ":";
										secs = (int)Time.time % 60;
										if (secs < 10) {
												starttimer.GetComponent<GUIText> ().text += "0" + System.Convert.ToString (secs);
										} else {
												starttimer.GetComponent<GUIText> ().text += System.Convert.ToString (secs);
										}
										break;
								case 2:
								//		points += Mathf.Max (50, (300 - (int)Time.time - starttime));
										starttime = (int)Time.time;
										starttimer.GetComponent<GUIText> ().text = "Level " + System.Convert.ToString (levelnum) + " start time = " + System.Convert.ToString ((int)Time.time / 60) + ":";
										secs = (int)Time.time % 60;
										if (secs < 10) {
												starttimer.GetComponent<GUIText> ().text += "0" + System.Convert.ToString (secs);
										} else {
												starttimer.GetComponent<GUIText> ().text += System.Convert.ToString (secs);
										}
										break;
								case 3:
								//		points += Mathf.Max (50, (300 - (int)Time.time - starttime)) * 2;
										starttime = (int)Time.time;
										starttimer.GetComponent<GUIText> ().text = "Level " + System.Convert.ToString (levelnum) + " start time = " + System.Convert.ToString ((int)Time.time / 60) + ":";
										secs = (int)Time.time % 60;
										if (secs < 10) {
												starttimer.GetComponent<GUIText> ().text += "0" + System.Convert.ToString (secs);
										} else {
												starttimer.GetComponent<GUIText> ().text += System.Convert.ToString (secs);
										}				
										break;
								case 4:
								//		points += Mathf.Max (50, (300 - (int)Time.time - starttime)) * 3;
										starttime = (int)Time.time;
										starttimer.GetComponent<GUIText> ().text = "Level " + System.Convert.ToString (levelnum) + " start time = " + System.Convert.ToString ((int)Time.time / 60) + ":";
										secs = (int)Time.time % 60;
										if (secs < 10) {
												starttimer.GetComponent<GUIText> ().text += "0" + System.Convert.ToString (secs);
										} else {
												starttimer.GetComponent<GUIText> ().text += System.Convert.ToString (secs);
										}				
										break;
								case 5:
								//		points += Mathf.Max (50, (300 - (int)Time.time - starttime)) * 4;
										starttime = (int)Time.time;
										starttimer.GetComponent<GUIText> ().text = "Level " + System.Convert.ToString (levelnum) + " start time = " + System.Convert.ToString ((int)Time.time / 60) + ":";
										secs = (int)Time.time % 60;
										if (secs < 10) {
												starttimer.GetComponent<GUIText> ().text += "0" + System.Convert.ToString (secs);
										} else {
												starttimer.GetComponent<GUIText> ().text += System.Convert.ToString (secs);
										}				
										break;
								case 6:
								//		points += Mathf.Max (50, (300 - (int)Time.time - starttime)) * 5;
										starttime = (int)Time.time;
										starttimer.GetComponent<GUIText> ().text = "Level " + System.Convert.ToString (levelnum) + " start time = " + System.Convert.ToString ((int)Time.time / 60) + ":";
										secs = (int)Time.time % 60;
										if (secs < 10) {
												starttimer.GetComponent<GUIText> ().text += "0" + System.Convert.ToString (secs);
										} else {
												starttimer.GetComponent<GUIText> ().text += System.Convert.ToString (secs);
										}				
										break;
					/*	case 7:
										points += Mathf.Max (50, (300 - (int)Time.time - starttime)) * 6;
										starttime = (int)Time.time;
										starttimer.GetComponent<GUIText> ().text = "Level " + System.Convert.ToString (levelnum - 1) + " end time = " + System.Convert.ToString ((int)Time.time / 60) + ":";
										secs = (int)Time.time % 60;
										if (secs < 10) {
												starttimer.GetComponent<GUIText> ().text += "0" + System.Convert.ToString (secs);
										} else {
												starttimer.GetComponent<GUIText> ().text += System.Convert.ToString (secs);
										}				
										break;*/


			
				case 200:
					starttimer.GetComponent<GUIText>().text = "";
					pointsGet = Mathf.Max (50, (300 - (int)Time.time - starttime));
					points += pointsGet;
					endtext.GetComponent<TextMesh> ().text = "Level " + System.Convert.ToString (levelnum / 100 - 1) + " end time = " + System.Convert.ToString ((int)Time.time / 60) + ":";
					secs = (int)Time.time % 60;
					if (secs < 10) {
						endtext.GetComponent<TextMesh> ().text += "0" + System.Convert.ToString (secs);
					} else {
						endtext.GetComponent<TextMesh> ().text += System.Convert.ToString (secs);
					}
					endtext.GetComponent<TextMesh> ().text += "\nTime elapsed: " + System.Convert.ToString ((int)Time.time - starttime) + " seconds";
					l1time.GetComponent<TextMesh>().text = System.Convert.ToString ((int)Time.time - starttime);
					endtext.GetComponent<TextMesh> ().text += "\nPoint Bonus: " + System.Convert.ToString (pointsGet) + " points!";
				//	starttime = (int)Time.time;
					break;
				case 300:
					starttimer.GetComponent<GUIText>().text = "";
					pointsGet = Mathf.Max (50, (300 - (int)Time.time - starttime)) * 2;
					points += pointsGet;
					endtext.GetComponent<TextMesh> ().text = "Level " + System.Convert.ToString (levelnum / 100 - 1) + " end time = " + System.Convert.ToString ((int)Time.time / 60) + ":";
					secs = (int)Time.time % 60;
					if (secs < 10) {
						endtext.GetComponent<TextMesh> ().text += "0" + System.Convert.ToString (secs);
					} else {
						endtext.GetComponent<TextMesh> ().text += System.Convert.ToString (secs);
					}			
					endtext.GetComponent<TextMesh> ().text += "\nTime elapsed: " + System.Convert.ToString ((int)Time.time - starttime) + " seconds";
					endtext.GetComponent<TextMesh> ().text += "\nPoint Bonus: " + System.Convert.ToString (pointsGet) + " points!";
					l2time.GetComponent<TextMesh>().text = System.Convert.ToString ((int)Time.time - starttime);
				//	starttime = (int)Time.time;
					break;
				case 400:
					starttimer.GetComponent<GUIText>().text = "";
					pointsGet = Mathf.Max (50, (300 - (int)Time.time - starttime)) * 3;
					points += pointsGet;
					endtext.GetComponent<TextMesh> ().text = "Level " + System.Convert.ToString (levelnum / 100 - 1) + " end time = " + System.Convert.ToString ((int)Time.time / 60) + ":";
					secs = (int)Time.time % 60;
					if (secs < 10) {
						endtext.GetComponent<TextMesh> ().text += "0" + System.Convert.ToString (secs);
					} else {
						endtext.GetComponent<TextMesh> ().text += System.Convert.ToString (secs);
					}			
					endtext.GetComponent<TextMesh> ().text += "\nTime elapsed: " + System.Convert.ToString ((int)Time.time - starttime) + " seconds";
					endtext.GetComponent<TextMesh> ().text += "\nPoint Bonus: " + System.Convert.ToString (pointsGet) + " points!";
					l3time.GetComponent<TextMesh>().text = System.Convert.ToString ((int)Time.time - starttime);
				//	starttime = (int)Time.time;
					break;
				case 500:
					starttimer.GetComponent<GUIText>().text = "";
					pointsGet = Mathf.Max (50, (300 - (int)Time.time - starttime)) * 4;
					points += pointsGet;
					endtext.GetComponent<TextMesh> ().text = "Level " + System.Convert.ToString (levelnum / 100 - 1) + " end time = " + System.Convert.ToString ((int)Time.time / 60) + ":";
					secs = (int)Time.time % 60;
					if (secs < 10) {
						endtext.GetComponent<TextMesh> ().text += "0" + System.Convert.ToString (secs);
					} else {
						endtext.GetComponent<TextMesh> ().text += System.Convert.ToString (secs);
					}		
					endtext.GetComponent<TextMesh> ().text += "\nTime elapsed: " + System.Convert.ToString ((int)Time.time - starttime) + " seconds";
					endtext.GetComponent<TextMesh> ().text += "\nPoint Bonus: " + System.Convert.ToString (pointsGet) + " points!";
					l4time.GetComponent<TextMesh>().text = System.Convert.ToString ((int)Time.time - starttime);
				//	starttime = (int)Time.time;
					break;
				case 999:
					starttimer.GetComponent<GUIText>().text = "";
					pointsGet = Mathf.Max (50, (300 - (int)Time.time - starttime)) * 5;
					points += pointsGet;
				//	this.GetComponent<GUIText> ().text = "FINAL SCORE = " + System.Convert.ToString (points) + " points";
					endtext.GetComponent<TextMesh> ().text = "Level 5 end time = " + System.Convert.ToString ((int)Time.time / 60) + ":";
					secs = (int)Time.time % 60;
					if (secs < 10) {
						endtext.GetComponent<TextMesh> ().text += "0" + System.Convert.ToString (secs);
					} else {
						endtext.GetComponent<TextMesh> ().text += System.Convert.ToString (secs);
					}		
					endtext.GetComponent<TextMesh> ().text += "\nTime elapsed: " + System.Convert.ToString ((int)Time.time - starttime) + " seconds";
					endtext.GetComponent<TextMesh> ().text += "\nPoint Bonus: " + System.Convert.ToString (pointsGet) + " points!";
					l5time.GetComponent<TextMesh>().text = System.Convert.ToString ((int)Time.time - starttime);
					endtext.GetComponent<TextMesh> ().text += "\nFinal score: " + points + " points";
				//	starttime = (int)Time.time;
					break;
				}
						}
				}
		}
}

using UnityEngine;
using System.Collections;

public class l6breakpointhandler : MonoBehaviour
{
	
		public GameObject[] breakpoints;
		public GameObject[] debugtexts;
		public GameObject l6dblack;
		public GameObject hero;
		public int[] breakstate;
		public int col1num = 0;
		public int col2num = 15;
		public int stepnum = 0;
		public string closestcolor = "";
		public int closeness = 1000;
		public bool stop = false;
		public string teststr;
	public string namedcolors = "\"Senses\",\"100\",\"100\",\"100\"},{\"IO/Communications\",\"0\",\"0\",\"0\"}," +
		"{\"MotorControl\",\"25\",\"85\",\"35\"},{\"CPU\",\"50\",\"100\",\"100\"}," +
			"{\"Memory\",\"100\",\"100\",\"100\"},{\"EnergyManagement\",\"10\",\"100\",\"100\"},{\"Head\",\"100\",\"100\",\"75\"}," +
			"{\"RightHand\",\"50\",\"95\",\"40\"},{\"LeftHand\",\"100\",\"100\",\"80\"},{\"RightArm\",\"50\",\"50\",\"70\"}," +
			"{\"LeftArm\",\"30\",\"50\",\"60\"},{\"RightLeg\",\"100\",\"90\",\"80\"},{\"LeftLeg\",\"75\",\"75\",\"75\"}," +
			"{\"RightFoot\",\"25\",\"25\",\"50\"},{\"LeftFoot\",\"100\",\"50\",\"100\"},{\"Torso\",\"50\",\"100\",\"100\"";
		public string[] colors;
		public string[] col1;
		public string[] col2;
	
	
	
		// Use this for initialization
		void Start ()
		{
				this.GetComponent<Renderer>().enabled = false;
				colors = namedcolors.Replace ("},{", "@").Split ('@');
				//colors = namedcolors.Split('@');
				breakstate = new int[] {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};
				stepnum -= 1;
		}
	
		// Update is called once per frame
		void Update ()
		{
				for (int i =0; i<11; i++) {
						SpriteRenderer sr = breakpoints [i].GetComponent<SpriteRenderer> ();
						if (sr.color == Color.black) {
								breakstate [i] = 0;
						} else if (sr.color == Color.red) {
								breakstate [i] = 1;
						} else {
								breakstate [i] = 2;
						}
				}
				SpriteRenderer slf = GetComponent<SpriteRenderer> ();
				if (slf.color == Color.magenta) {
						slf.color = Color.black;
						stop = true;
						foreach (int i in breakstate) {
								if (i != 0) {
										stop = false;
								}
						}
						while (col1num < 15 && !stop) {
								closeness = 1000;
								closestcolor = "";
								while (col2num >= col1num && !stop) {
										if (col2num % 6 == 0) {
												closeness = 0;
												closestcolor = "\"MEMORYLEAK\"";
										}
										if ((col2num + 1) % 6 == 0) {
												closeness = 1000;
												closestcolor = "";
										}
										if (stepnum >= 0) {
												step ();
												SpriteRenderer current = breakpoints [stepnum].GetComponent<SpriteRenderer> ();
												if (current.color == Color.magenta) {
														current.color = Color.red;
														hero.transform.position = new Vector3(hero.transform.position.x, breakpoints[stepnum].transform.position.y, 0);
												}
										}
										stepnum++;
										if (stepnum == 10) {
												if (col2num > col1num) {
														stepnum = 4;
												}
										}
										if (stepnum > 10) {
												stepnum = 0;
												closeness = 1000;
												closestcolor = "";
												//col1num++;
												col2num = 15;
										}
										SpriteRenderer sr = breakpoints [stepnum].GetComponent<SpriteRenderer> ();
										if (sr.color == Color.red) {
												sr.color = Color.magenta;
												stop = true;
										}
											if (l6dblack.GetComponent<TextMesh> ().text != "{\"IO/Communications\",0,0,0},") {
												if (col2num == 1) {
														for (int i =0; i<11; i++) {
																SpriteRenderer ssr = breakpoints [i].GetComponent<SpriteRenderer> ();
																if (ssr.color == Color.magenta) {
																		ssr.color = Color.red;
																		hero.transform.position = new Vector3(hero.transform.position.x, breakpoints[i].transform.position.y, 0);
																}
														}
														col1num = 0;
														col2num = 15;
														stepnum = 0;
														closeness = 1000;
														closestcolor = "";
														stop = true;
												}
										}
								}
						}
						if (col1num == 15) {
								for (int i =0; i<11; i++) {
										SpriteRenderer sr = breakpoints [i].GetComponent<SpriteRenderer> ();
										if (sr.color == Color.magenta) {
												sr.color = Color.red;
										}
								}
								col1num = 0;
								col2num = 15;
								stepnum = 0;
						}
						stop = false;
				}
		}

		void step ()
		{
				switch (stepnum) {
				case 0:
						col1 = colors [col1num].Split (',');
						debugtexts [7].GetComponent<GUIText> ().text = "part1 = " + col1 [0];
						break;
				case 1:
						debugtexts [0].GetComponent<GUIText> ().text = "power1 = " + col1 [1];
						break;
				case 2:
						debugtexts [1].GetComponent<GUIText> ().text = "cond1 = " + col1 [2];
						break;
				case 3:
						debugtexts [2].GetComponent<GUIText> ().text = "effic1 = " + col1 [3];
						break;
				case 4:
						col2 = colors [col2num].Split (',');
						debugtexts [8].GetComponent<GUIText> ().text = "part2 = " + col2 [0];
						break;
				case 5:
						debugtexts [3].GetComponent<GUIText> ().text = "power2 = " + col2 [1];
						break;
				case 6:
						debugtexts [4].GetComponent<GUIText> ().text = "cond2 = " + col2 [2];
						break;
				case 7:
						debugtexts [5].GetComponent<GUIText> ().text = "effic2 = " + col2 [3];
						break;
				case 8:
						debugtexts [6].GetComponent<GUIText> ().text = "similar = " + closer (col1, col2);
						break;
				case 9:
						col2num--;
						debugtexts [10].GetComponent<GUIText> ().text = "part2num = " + System.Convert.ToString (col2num);
						break;
				case 10:
						col1num++;
						debugtexts [9].GetComponent<GUIText> ().text = "part1num = " + System.Convert.ToString (col1num);
						break;
				}
		}
	
		string closer (string[] col1, string[] col2)
		{
				int newclose = (int)Mathf.Abs (System.Convert.ToInt32 (col1 [1].Replace ("\"", "")) - System.Convert.ToInt32 (col2 [1].Replace ("\"", "")));
				newclose += ((int)Mathf.Abs (System.Convert.ToInt32 (col1 [2].Replace ("\"", "")) - System.Convert.ToInt32 (col2 [2].Replace ("\"", ""))));
				newclose += ((int)Mathf.Abs (System.Convert.ToInt32 (col1 [3].Replace ("\"", "")) - System.Convert.ToInt32 (col2 [3].Replace ("\"", ""))));
				if (closeness > newclose) {
						closeness = newclose;
						closestcolor = col2 [0];
				}
				return closestcolor;
		}
	
		void OnTriggerEnter2D (Collider2D c)
		{
				if (c.name == "projectileDebug(Clone)") {
						SpriteRenderer nxt = GetComponent<SpriteRenderer> ();
						nxt.color = Color.magenta;
				}
		}
}

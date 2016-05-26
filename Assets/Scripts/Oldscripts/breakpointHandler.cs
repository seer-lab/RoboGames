using UnityEngine;
using System.Collections;

public class breakpointHandler : MonoBehaviour {

	public GameObject[] breakpoints;
	public GameObject[] debugtexts;
	public int[] breakstate;
	public int col1num = 0;
	public int col2num = 15;
	public int stepnum = 0;
	public bool stop = false;
	public GameObject hero;

	public string teststr;

	public string namedcolors = "\"EnemyRobot\",\"255\",\"0\",\"0\"},{\"RobotController\",\"0\",\"64\",\"0\"}," +
		"{\"ChargingStation\",\"64\",\"64\",\"0\"},{\"Projectile\",\"0\",\"255\",\"255\"}," +
			"{\"FlyingDrone\",\"255\",\"255\",\"255\"},{\"LightSource\",\"0\",\"128\",\"255\"},{\"FallingDebris\",\"0\",\"0\",\"192\"}," +
			"{\"LaboratoryComputer\",\"128\",\"0\",\"0\"},{\"Exit\",\"255\",\"255\",\"0\"},{\"Explosive\",\"128\",\"128\",\"0\"}," +
			"{\"NetworkCenter\",\"0\",\"128\",\"0\"},{\"Human\",\"255\",\"0\",\"0\"},{\"MechanicalArm\",\"192\",\"192\",\"192\"}," +
			"{\"UnstableTerrain\",\"64\",\"64\",\"0\"},{\"UnknownObject\",\"255\",\"128\",\"255\"},{\"Camera\",\"128\",\"255\",\"255\"";
	public string[] colors;
	public string[] col1;
	public string[] col2;



	// Use this for initialization
	void Start () {
		this.GetComponent<Renderer>().enabled = false;
		colors = namedcolors.Replace ("},{", "@").Split('@');
		//colors = namedcolors.Split('@');
		breakstate = new int[] {0, 0, 0, 0, 0, 0, 0, 0, 0, 0};
		stepnum -= 1;
	}
	
	// Update is called once per frame
	void Update () {
		for (int i =0;i<9;i++){
			SpriteRenderer sr = breakpoints[i].GetComponent<SpriteRenderer>();
			if (sr.color == Color.black){
				breakstate[i] = 0;
			}
			else if (sr.color == Color.red){
				breakstate[i]=1;
			}
			else{
				breakstate[i]=2;
			}
		}
		SpriteRenderer slf = GetComponent<SpriteRenderer> ();
		if (slf.color == Color.magenta) {
			slf.color = Color.black;
			while(col1num < 15 && !stop){
				while(col2num > col1num && !stop){
					if (stepnum>=0){step();
						SpriteRenderer current = breakpoints[stepnum].GetComponent<SpriteRenderer>();
						if (breakstate[stepnum]==2){
							current.color = Color.red;
						}
					}
					stepnum++;
					if (stepnum == 9){
						stepnum = 4;
						col2num--;
						if (col2num == col1num){
							stepnum = 0;
							col2num = 15;
							col1num++;
						}
					}
					SpriteRenderer sr = breakpoints[stepnum].GetComponent<SpriteRenderer>();
					if (breakstate[stepnum]>0){
						sr.color = Color.magenta;
						stop = true;
						hero.transform.position = new Vector3(hero.transform.position.x, breakpoints[stepnum].transform.position.y, 0);
					}
				}
			}
			if (col1num == 15 && !stop){
				for (int i =0;i<9;i++){
					SpriteRenderer sr = breakpoints[i].GetComponent<SpriteRenderer>();
					if (sr.color == Color.magenta){
						sr.color = Color.red;
					}
				}
				col1num = 0;
				col2num = 15;
				stepnum = 0;
				debugtexts[0].GetComponent<GUIText>().text = "x1 = ";
				debugtexts[1].GetComponent<GUIText>().text = "y1 = ";
				debugtexts[2].GetComponent<GUIText>().text = "z1 = ";
				debugtexts[3].GetComponent<GUIText>().text = "x2 = ";
				debugtexts[4].GetComponent<GUIText>().text = "y2 = ";
				debugtexts[5].GetComponent<GUIText>().text = "z2 = ";
				debugtexts[6].GetComponent<GUIText>().text = "closer.name = \"\"";
				debugtexts[7].GetComponent<GUIText>().text = "object1.name = \"\"";
				debugtexts[8].GetComponent<GUIText>().text = "object2.name = \"\"";
			}
			stop = false;
		}
		else if (slf.color == Color.cyan){
			slf.color = Color.black;
			col1num = 0;
			col2num = 15;
			stepnum = 0;
			debugtexts[0].GetComponent<GUIText>().text = "x1 = ";
			debugtexts[1].GetComponent<GUIText>().text = "y1 = ";
			debugtexts[2].GetComponent<GUIText>().text = "z1 = ";
			debugtexts[3].GetComponent<GUIText>().text = "x2 = ";
			debugtexts[4].GetComponent<GUIText>().text = "y2 = ";
			debugtexts[5].GetComponent<GUIText>().text = "z2 = ";
			debugtexts[6].GetComponent<GUIText>().text = "closer.name = \"\"";
			debugtexts[7].GetComponent<GUIText>().text = "object1.name = \"\"";
			debugtexts[8].GetComponent<GUIText>().text = "object2.name = \"\"";
		}
	}
	void step(){
		switch (stepnum) {
		case 0:
			col1 = colors[col1num].Split(',');
			debugtexts[7].GetComponent<GUIText>().text = "object1.name = \n" + col1[0];
			break;
		case 1:
			debugtexts[0].GetComponent<GUIText>().text = "x1 = " + col1[1];
			break;
		case 2:
			debugtexts[1].GetComponent<GUIText>().text = "y1 = " + col1[2];
			break;
		case 3:
			debugtexts[2].GetComponent<GUIText>().text = "z1 = " + col1[3];
			break;
		case 4:
			col2 = colors[col2num].Split(',');
			debugtexts[8].GetComponent<GUIText>().text = "object2.name = \n" + col2[0];
			break;
		case 5:
			debugtexts[3].GetComponent<GUIText>().text = "x2 = " + col2[1];
			break;
		case 6:
			debugtexts[4].GetComponent<GUIText>().text = "y2 = " + col2[2];
			break;
		case 7:
			debugtexts[5].GetComponent<GUIText>().text = "z2 = " + col2[3];
			break;
		case 8:
			debugtexts[6].GetComponent<GUIText>().text = "closer.name = \n" + closer(col1,col2);
			break;
		}
	}

	string closer(string[] col1, string[] col2){

		int sum1 = (int) System.Convert.ToInt32(col1[2].Replace("\"", "")) + System.Convert.ToInt32(col1[3].Replace("\"", ""));
		int sum2 = (int) System.Convert.ToInt32(col2[2].Replace("\"", "")) + System.Convert.ToInt32(col2[3].Replace("\"", ""));
		if (sum1 < sum2) {
			return col1[0];
		}
		else{
			return col2[0];
		}
	}

	void OnTriggerEnter2D(Collider2D c){
		if (c.name == "projectileTest(Clone)") {
				SpriteRenderer nxt = GetComponent<SpriteRenderer>();
				nxt.color = Color.magenta;
				Destroy(c.gameObject);
			GetComponent<AudioSource>().Play();
		}
	}
}

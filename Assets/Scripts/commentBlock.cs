using UnityEngine;
using System.Collections;
using System.IO;

public class commentBlock : MonoBehaviour {

	public string oldtext="";
	public string blocktext="";
	public string errmsg="";
	public GameObject code;
	public GameObject sideoutput;
	float timeDelay = 30f;
	float resetTime = 0f;
	bool resetting=false;

	public GameObject selectTools;
	public int[] tools = {0,0,0,0,0,0};
	bool toolgiven = false;

	float initialLineY = 3.5f;
	float linespacing = 0.825f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (resetting){
			if (code.GetComponent<TextMesh>().text != oldtext.Replace(blocktext, "<color=#00ff00ff>/*" + blocktext.Replace("/**/","") + "*/</color>")){
				resetting = false;
			}
			else if (Time.time > resetTime || Input.GetKeyDown(KeyCode.Return)){
				resetting = false;
				sideoutput.GetComponent<GUIText>().text="";
				code.GetComponent<TextMesh>().text = oldtext;
			}
		}
	}

	void OnTriggerEnter2D(Collider2D c){
		if (c.name == "projectileComment(Clone)"){
			StreamWriter sw = new StreamWriter("toollog.txt",true);
			sw.WriteLine("Commented,"+((int)((initialLineY-this.transform.position.y)/linespacing)).ToString()+","+Time.time.ToString());
			sw.Close();
			Destroy(c.gameObject);
			GetComponent<AudioSource>().Play();

			code.GetComponent<TextMesh>().text = oldtext.Replace(blocktext, "<color=#00ff00ff>/*" + blocktext.Replace("/**/","") + "*/</color>");
			sideoutput.GetComponent<GUIText>().text=errmsg;
			resetTime = Time.time + timeDelay;
			resetting = true;

			if(!toolgiven){
				toolgiven = true;
				for (int i = 0;i<6;i++){
					if (tools[i]>0){
						selectTools.GetComponent<SelectedTool>().toolget = true;
					}
					selectTools.GetComponent<SelectedTool>().toolCounts[i] += tools[i];
				}
			}
		}
	}
}

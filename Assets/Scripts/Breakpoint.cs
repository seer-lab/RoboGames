using UnityEngine;
using System.Collections;
using System.IO;

public class Breakpoint : MonoBehaviour {

	public GameObject sidebaroutput;
	public string values;
	bool activated = false;
	public AudioClip[] sound = new AudioClip[2];
	public GameObject selectTools;
	public int[] tools = {0,0,0,0,0,0};
	bool toolgiven = false;

	float initialLineY = 3.5f;
	float linespacing = 0.825f;

	// Use this for initialization
	void Start () {
		this.GetComponent<SpriteRenderer>().color = new Color(1,0,0,0.5f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnTriggerEnter2D(Collider2D c){
		if (c.name == "projectileDebug(Clone)"){
			if (!activated){
				GetComponent<AudioSource>().clip = sound[0];
				GetComponent<AudioSource>().Play();
				StreamWriter sw = new StreamWriter("toollog.txt",true);
				sw.WriteLine("BreakpointOn,"+((int)((initialLineY-this.transform.position.y)/linespacing)).ToString()+","+Time.time.ToString());
				sw.Close();
			}
			activated = true;
			this.GetComponent<SpriteRenderer>().color = new Color(1,0,0,1);
		}
		else if (activated && c.name == "projectileActivator(Clone)") {
			StreamWriter sw = new StreamWriter("toollog.txt",true);
			sw.WriteLine("BreakpointActivated,"+((int)((initialLineY-this.transform.position.y)/linespacing)).ToString()+","+Time.time.ToString());
			sw.Close();
			GetComponent<AudioSource>().clip = sound[1];
			GetComponent<AudioSource>().Play();
			sidebaroutput.GetComponent<GUIText>().text = values;
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

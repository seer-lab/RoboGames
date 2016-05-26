using UnityEngine;
using System.Collections;
using System.IO;

public class printer : MonoBehaviour {
	
	public string displaytext = "";
	public GameObject sidebar;
	
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
		
	}
	void OnTriggerEnter2D(Collider2D c){
		if (c.name == "projectileActivator(Clone)"){
			StreamWriter sw = new StreamWriter("toollog.txt",true);
			sw.WriteLine("Printed,"+((int)((initialLineY-this.transform.position.y)/linespacing)).ToString()+","+Time.time.ToString());
			sw.Close();
			Destroy(c.gameObject);
			sidebar.GetComponent<GUIText>().text = displaytext;
			GetComponent<AudioSource>().Play();
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

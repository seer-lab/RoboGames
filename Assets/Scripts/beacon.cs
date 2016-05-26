using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class beacon : MonoBehaviour {

	public List<int> actnumbers;
	public int actcounter = 0;
	public GameObject codescreen;
	public Sprite activebeacon;
	LevelGenerator lg;

	// Use this for initialization
	void Start () {
		lg = codescreen.GetComponent<LevelGenerator> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (lg.taskscompleted [0] == lg.tasklist[0]) {
			GetComponent<SpriteRenderer>().sprite = activebeacon;
		}
	}

	void OnTriggerEnter2D(Collider2D p){
		if (p.name == "projectileBug(Clone)") {
			Destroy(p.gameObject);
			if (GetComponent<SpriteRenderer>().sprite == activebeacon || actnumbers.Count == 0){
				lg.losing = true;
			}
			else if (lg.taskscompleted[0] != actnumbers[actcounter]){
				lg.losing = true;
			}else{
				GetComponent<AudioSource>().Play();
				lg.taskscompleted[0]++;
				actcounter++;
				if (actcounter == actnumbers.Count){
					GetComponent<SpriteRenderer>().sprite = activebeacon;
				}
			}
		}
	}
}

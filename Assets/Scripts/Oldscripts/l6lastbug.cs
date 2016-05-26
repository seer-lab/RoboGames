using UnityEngine;
using System.Collections;

public class l6lastbug : MonoBehaviour {

	public GameObject l6dblack;
	public GameObject col2num;
	public GameObject breakpoint;

	public bool cond1 = false;
	public bool cond2 = false;
	public bool cond3 = false;

	// Use this for initialization
	void Start () {
		this.GetComponent<Renderer>().enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		cond1 = l6dblack.GetComponent<TextMesh> ().text == "{\"IO/Communications\",0,0,0},";
		cond2 = System.Convert.ToInt32 (col2num.GetComponent<GUIText> ().text.Substring (11)) % 6 == 0;
		cond3 = breakpoint.GetComponent<Renderer>().enabled == true;
	}

	void OnTriggerEnter2D(Collider2D p){
		if (p.name == "projectileBug(Clone)") {
			if (l6dblack.GetComponent<TextMesh>().text=="{\"IO/Communications\",0,0,0},"){
				if (System.Convert.ToInt32(col2num.GetComponent<GUIText>().text.Substring(11))%6==0){
					if (breakpoint.GetComponent<Renderer>().enabled == true){
						this.GetComponent<Renderer>().enabled = true;
						Destroy(p.gameObject);
						GetComponent<Animator>().SetBool("Dying", true);
					}
				}
			}
		}
	}
}

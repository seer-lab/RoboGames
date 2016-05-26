using UnityEngine;
using System.Collections;
using System.IO;

public class GenericBug : MonoBehaviour {
		public Animator anim;
		public bool dead = false;
		public bool finished = false;
	public GameObject codescreen;

	float initialLineY = 3.5f;
	float linespacing = 0.825f;
		
		// Use this for initialization
		void Start () {
			this.GetComponent<Renderer>().enabled = false;
			anim = GetComponent<Animator>();
		}
		
		// Update is called once per frame
		void Update () {
		}
		
		void OnTriggerEnter2D(Collider2D p){
			if (p.name == "projectileBug(Clone)") {
			StreamWriter sw = new StreamWriter("toollog.txt",true);
			sw.WriteLine("BugCaught,"+((int)((initialLineY-this.transform.position.y)/linespacing)).ToString()+","+Time.time.ToString());
			sw.Close();
				this.GetComponent<Renderer>().enabled = true;
				Destroy (p.gameObject);
				anim.SetBool("Dying", true);
				GetComponent<AudioSource>().Play();
				dead = true;
				codescreen.GetComponent<LevelGenerator>().num_of_bugs--;
			}
		}
	}

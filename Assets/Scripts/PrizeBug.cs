using UnityEngine;
using System.Collections;

public class PrizeBug : MonoBehaviour {
		public Animator anim;
		public bool dead = false;
		public bool finished = false;
		public GameObject tools;
		public int[] bonus = {0,0,0,0,0,0};
		
		// Use this for initialization
		void Start () {
			this.GetComponent<Renderer>().enabled = false;
			anim = GetComponent<Animator>();
		}
		
		// Update is called once per frame
		void Update () {
		}
		
		void OnTriggerEnter2D(Collider2D p){
			if (p.name == "projectileBug(Clone)" && this.GetComponent<Renderer>().enabled == false) {
				this.GetComponent<Renderer>().enabled = true;
				Destroy (p.gameObject);
				anim.SetBool("Dying", true);
				GetComponent<AudioSource>().Play();
				dead = true;
				for (int i = 0;i<6;i++){
					tools.GetComponent<SelectedTool>().bonusTools[i] += bonus[i];
				}
			}
		}
	}

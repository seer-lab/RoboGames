using UnityEngine;
using System.Collections;

public class Answer : MonoBehaviour
{


		public GameObject level;
		public int thislevel;
		float startingTime = 0;

		// Use this for initialization
		void Start ()
		{
				this.GetComponent<Renderer>().enabled = false;
				
		}
	
		// Update is called once per frame
		void Update ()
		{
				int currentlevel = System.Convert.ToInt16 (level.GetComponent<TextMesh> ().text);
				if (currentlevel == thislevel) {
						if (startingTime == 0) {
								startingTime = Time.time;
						}
						if (Time.time > 600 + startingTime) {
								this.GetComponent<Renderer>().enabled = true;
						}
				}
		}
}

using UnityEngine;
using System.Collections;

public class Camera2 : MonoBehaviour
{

		public GameObject hero;
		public GameObject codescreen;
		public AudioClip[] soundtrack = new AudioClip[15];
		public int track = 0;
		int trackmax = 12;
		public AudioClip menutrack;
		public AudioClip victorytrack;
		public AudioClip newleveltrack;
		public AudioClip losetrack;
		public AudioClip endtrack;
		public AudioClip comictrack;
		AudioSource ads;
		int laststate = 0;

		// Use this for initialization
		void Start ()
		{
				ads = GetComponent<AudioSource> ();
				ads.clip = menutrack;
		}
	
		// Update is called once per frame
		void Update ()
		{
				int gs = codescreen.GetComponent<LevelGenerator> ().gamestate;
				if (laststate != gs) {
						switch (gs) {
						case 10:
								ads.clip = comictrack;
								ads.loop = false;
								ads.Play ();
								break;
						case 11:
								ads.clip = comictrack;
								ads.loop = false;
								ads.Play ();
								break;
						case 12:
								ads.clip = endtrack;
								ads.loop = false;
								ads.Play ();
								break;
						case 4:
								ads.clip = losetrack;
								ads.loop = false;
								ads.Play ();
								break;
						case 3:
								ads.clip = victorytrack;
								ads.loop = false;
								ads.Play ();
								break;
						case 2:
								ads.clip = newleveltrack;
								ads.loop = false;
								ads.Play ();
								break;
						case 1:
								track = (track + 1) % trackmax;
								ads.clip = soundtrack [track];
								ads.loop = false;
								ads.Play ();
								break;
						case 0:
								if (laststate > 0) {
										ads.clip = menutrack;
										ads.loop = true;
										ads.Play ();
								}
								break;
						default:
								break;
						}
						laststate = gs;
				}
				if (gs == 12) {
						GetComponent<Camera> ().transform.position = new Vector3 (54.4f, 50f, -10f);
				} else if (gs > 9 && gs < 12) {
						GetComponent<Camera> ().transform.position = new Vector3 (50, 50f, -10f);
				} else if (gs >= 2 && gs <= 9) {
						GetComponent<Camera> ().transform.position = new Vector3 (100f, 0f, -10f);
				} else if (gs == 1) {
						GetComponent<Camera> ().transform.position = new Vector3 (0f, hero.transform.position.y, -10f);
				} else if (gs <= 0) {
						GetComponent<Camera> ().transform.position = new Vector3 (50f, 0f, -10f);
				}
				GetComponent<Camera> ().orthographicSize = 6;

		}

}

using UnityEngine;
using System.Collections;

public class hero2Controller : MonoBehaviour
{

		public float maxSpeed = 10f;
		public float climbSpeed = 10f;
		public Rigidbody2D[] projectiles = new Rigidbody2D[6];
		public GameObject[] toolIcons = new GameObject[6];
		private bool walkloop = false;
		public int projectilecode = 0;
		public bool onWall = false;
		bool facingRight = true;
		float fireRate = 0.5f;
		private float nextFire = 0.0f;
		float animTime = 0.3f;
		private float animDelay = 0.0f;
		Animator anim;
		public bool dropping = false;
		float dropTime = 0.25f;
		public float dropDelay = 0.0f;
		bool quitting = false;
		public bool throwing = false;
		public GameObject codescreen;
		float climbTime;
		float climbDelay = 0.2f;
		public float up;
		LevelGenerator lg;

		// Use this for initialization
		void Start ()
		{
				anim = GetComponent<Animator> ();
				climbTime = 0f;
		lg = codescreen.GetComponent<LevelGenerator> ();
		}

		void FixedUpdate ()
		{
				if (lg.gamestate == 1 && !lg.losing) {
						//movement
						float move = Input.GetAxis ("Horizontal");
						up = Input.GetAxis ("Vertical");
						if (up > 0) {
								GetComponent<Rigidbody2D> ().gravityScale = 0;

								if (!onWall) {
										onWall = true;
										climbTime = Time.time + climbDelay;
								}
						}
						if (climbTime > Time.time) {
								up = up > 0.5f ? up : 0.5f;
						}
						if (up < 0) {
								up = -0.1f;
								onWall = false;
								GetComponent<Rigidbody2D> ().gravityScale = 1;
						}

						//set animation for movement
						anim.SetBool ("onWall", onWall);
						anim.SetFloat ("speed", Mathf.Abs (move));
						anim.SetFloat ("climbSpeed", up);
						if (move > 0) {
								facingRight = true;
						} else if (move < 0) {
								facingRight = false;
						}
						anim.SetBool ("facingRight", facingRight);



						//code for falling down through platforms
						//	if (Input.GetAxisRaw("Vertical") == -1 && Input.GetButton("Jump")){
						//	if (Input.GetButton("Jump")){
						if (up < 0 && !onWall && !dropping) {
								dropDelay = Time.time + dropTime;
								dropping = true;
						}
						else if (Time.time > dropDelay && up==0) {
								dropping = false;
						}

						//new dropcode
						if (up == 0 && move == 0 && onWall && (Time.time > climbDelay)) {
								onWall = false;
								GetComponent<Rigidbody2D> ().gravityScale = 1;
						}
		
						//Physics2D.IgnoreLayerCollision (0, 8, onWall || dropping || Input.GetKey ("up"));
						Physics2D.IgnoreLayerCollision (0, 8, onWall || dropping);
						//move up if on the wall, otherwise let gravity do the work
						if (dropping) {
								if (GetComponent<Rigidbody2D> ().velocity.y == 0) {
										GetComponent<Rigidbody2D> ().isKinematic = true;
										GetComponent<Rigidbody2D> ().AddForce (Vector2.up * -50);
										GetComponent<Rigidbody2D> ().isKinematic = false;
								}
								GetComponent<Rigidbody2D> ().velocity = new Vector2 (move * maxSpeed, GetComponent<Rigidbody2D> ().velocity.y);
						} else {
								if (!onWall) {
										GetComponent<Rigidbody2D> ().velocity = new Vector2 (move * maxSpeed, Mathf.Min (0f, GetComponent<Rigidbody2D> ().velocity.y));
								} else {
										GetComponent<Rigidbody2D> ().isKinematic = true;
										GetComponent<Rigidbody2D> ().velocity = new Vector2 (move * maxSpeed, up * climbSpeed);
										GetComponent<Rigidbody2D> ().isKinematic = false;
								}
						}
				} else if (lg.losing){
					GetComponent<Rigidbody2D> ().velocity = new Vector2(0,0);
				}
		}

		void Update ()
		{
				if (codescreen.GetComponent<LevelGenerator> ().gamestate == 1) {
						AudioSource ad = GetComponent<AudioSource> ();
						if (!walkloop && Input.GetAxis ("Horizontal") != 0f && GetComponent<Rigidbody2D> ().velocity.y == 0 && !onWall) {
								ad.Play ();
								walkloop = true;
								ad.loop = true;
						}
						if (Input.GetAxis ("Horizontal") == 0f || GetComponent<Rigidbody2D> ().velocity.y != 0 || onWall) {
								ad.loop = false;
								walkloop = false;
						}

						//firing
						if ((Input.GetKeyDown ("left ctrl") || Input.GetKeyDown ("right ctrl")) && Time.time > nextFire && !onWall 
								&& GetComponent<Rigidbody2D> ().velocity == Vector2.zero && projectilecode >= 0) {
								throwing = true;
								anim.SetBool ("throw", true);
								nextFire = Time.time + fireRate;
								animDelay = Time.time + animTime;
								Rigidbody2D newstar = (Rigidbody2D)Instantiate (projectiles [projectilecode], transform.position, transform.rotation);
								if (facingRight) {
										newstar.GetComponent<Rigidbody2D> ().AddForce (Vector2.right * 300);
								} else {
										newstar.GetComponent<Rigidbody2D> ().AddForce (Vector2.right * -300);
								}
						}
						if (Time.time > animDelay) {
								anim.SetBool ("throw", false);
						}



						//quit
						if (Input.GetKeyDown (KeyCode.Escape) == true) {
								quitting = true;
						}
						if (quitting) {
								if (Input.GetKeyDown ("y")) {
										Application.Quit ();
								} else if (Input.GetKeyDown ("n")) {
										quitting = false;
								
								}
						}
				} else {
						GetComponent<AudioSource> ().loop = false;
				}
		}

}

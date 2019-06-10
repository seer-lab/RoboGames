using System.Linq;
using System.Net.Sockets;
using System.Diagnostics.SymbolStore;
using System.Xml.Schema;
//**************************************************//
// Class Name: hero2Controller
// Class Description: This class is the controller for the hero. It controls movement, throwing wrenches
//                    and horizontal/vertical translation of the game avatar.
// Methods:
// 		void Start()
//		void FixedUpdate()
//		void Update()
// Author: Michael Miljanovic
// Date Last Modified: 6/1/2016
//**************************************************//

using UnityEngine;
using System; 
using System.Collections;

// BOOKMARK -END 5/31/2016
public class hero2Controller : MonoBehaviour
{

	public bool onWall = false;
	public bool dropping = false;
	public bool throwing = false;
	public float maxSpeed = 10f;
	public float climbSpeed = 10f;
	public float dropDelay = 0.0f;
	public float fMoveVelocityVertical = 0.0f;
	public int projectilecode = 0;
	public GameObject codescreen;
	public GameObject selectedTool;
	public Rigidbody2D[] projectiles = new Rigidbody2D[stateLib.NUMBER_OF_TOOLS];
	private GameController controller; 
	private bool walkloop = false;
	private bool facingRight = true;
	private bool quitting = false;
	private float fireRate = 0.5f;
	private float nextFire = 0.0f;
	private float animTime = 0.2f;
	private float animDelay = 0.0f;
	private float dropTime = 0.25f;
	private float climbTime;
	private float climbDelay = 0.2f;
	private Animator anim;
	private LevelGenerator lg;
	private int lastLineNumberactive; 
	private float verticalMovement = 1f; 
	private bool isMovingX = false; 
	private bool reachedPosition = true; 
	EnergyController energyController; 
	private FireButton fire; 
	bool canTakeDamage = true; 

	int timeStart, timeEnd, totalTime, timeCurrent;
    DateTime time;

	private AudioClip throwTool; 
	AudioSource audioSource; 
	//.................................>8.......................................
	// Use this for initialization
	void Start() {
		audioSource = this.GetComponent<AudioSource>(); 
		codescreen = GameObject.Find("CodeScreen");
		energyController = GameObject.Find("Energy").GetComponent<EnergyController>();
		fire = GameObject.Find("FireTool").transform.GetChild(0).GetComponent<FireButton>();
		selectedTool = GameObject.Find("Sidebar").transform.Find("Sidebar Tool").gameObject;
		projectiles[0] = Resources.Load<GameObject>("Prefabs/projectileBug").GetComponent<Rigidbody2D>();
		projectiles[1] =  Resources.Load<GameObject>("Prefabs/projectileActivator").GetComponent<Rigidbody2D>();
		projectiles[2] =  Resources.Load<GameObject>("Prefabs/projectileWarp").GetComponent<Rigidbody2D>();
		projectiles[3] =  Resources.Load<GameObject>("Prefabs/projectileComment").GetComponent<Rigidbody2D>();
		projectiles[4] =  Resources.Load<GameObject>("Prefabs/projectileDebug").GetComponent<Rigidbody2D>();
		projectiles[5] =  Resources.Load<GameObject>("Prefabs/projectileHelp").GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
		throwTool = Resources.Load<AudioClip>("Sound/Triggers/throw");  
		climbTime = 0f;
		lg = codescreen.GetComponent<LevelGenerator>();
		controller = Camera.main.GetComponent<GameController>();
		timeStart = DateTime.Now.Second;
	}
	void Flip(){
		if (facingRight && anim.GetCurrentAnimatorClipInfo(0)[0].clip.name.Contains("left")){
			//this.GetComponent<SpriteRenderer>().flipX = true; 
		}
	}
	public void onFail(){
		energyController.onFail(projectilecode); 
	}
	IEnumerator DamageDelay(){
		canTakeDamage = false; 
		float seconds = 2f; 
		int blinks = 3; 

		for (int i = 0; i < blinks; i++){
			GetComponent<SpriteRenderer>().color = new Color(0.3f, 0.3f, 0.3f); 
			yield return new WaitForSecondsRealtime(seconds/(blinks*2)); 
			GetComponent<SpriteRenderer>().color = new Color(1,1,1); 
			yield return new WaitForSecondsRealtime(seconds/(blinks*2)); 
		}
		canTakeDamage = true; 
	}
	public bool onTakeDamange(float damage){
		if (canTakeDamage){
			energyController.onDamange(damage); 
			StartCoroutine(DamageDelay()); 
			return true; 
		}
		return false; 
	}
	//.................................>8.......................................
	void FixedUpdate() {
		if (GlobalState.GameState == stateLib.GAMESTATE_IN_GAME && !Output.IsAnswering) {
			//movement
			float fMoveVelocityHorizontal = Input.GetAxis("Horizontal");
			fMoveVelocityVertical = Input.GetAxis("Vertical");
			if (Input.GetMouseButton(0) && !reachedPosition) {
				if (isMovingX)
					fMoveVelocityHorizontal = (facingRight) ? 1f: -1f; 
				else fMoveVelocityVertical = verticalMovement; 
			}
			if (fMoveVelocityVertical > 0) {
				GetComponent<Rigidbody2D>().gravityScale = 0;
				if (!onWall) {
					onWall = true;
					climbTime = Time.time + climbDelay;
				}
			}
			if (climbTime > Time.time) {
				fMoveVelocityVertical = fMoveVelocityVertical > 0.5f ? fMoveVelocityVertical : 0.5f;
			}
			if (fMoveVelocityVertical < 0) {
				fMoveVelocityVertical = -0.1f;
				onWall = false;
				GetComponent<Rigidbody2D>().gravityScale = 1;
			}

			//set animation for movement
			anim.SetBool("onWall", onWall);
			anim.SetFloat("speed", Mathf.Abs(fMoveVelocityHorizontal));
			anim.SetFloat("climbSpeed", fMoveVelocityVertical);
			if (fMoveVelocityHorizontal > 0) {
				facingRight = true;

			}
			else if (fMoveVelocityHorizontal < 0) {
				facingRight = false;
			}
			anim.SetBool("facingRight", facingRight);

			if (this.GetComponent<SpriteRenderer>().flipX == facingRight && GlobalState.Character == "Boy"){
				float offset = 0.8f; 
				Transform pos = this.GetComponent<Transform>(); 
				Transform newTool = this.transform.Find("NewTool").GetComponent<Transform>(); 
				if (!facingRight) offset*= -1; 
				newTool.position = new Vector3(newTool.position.x - offset/2,newTool.position.y,newTool.position.z); 
				pos.position = new Vector3(pos.position.x + offset, pos.position.y, pos.position.z); 
			}
			this.GetComponent<SpriteRenderer>().flipX = !facingRight;

			//code for falling down through platforms
			if (fMoveVelocityVertical < 0 && !onWall && !dropping) {
				dropDelay = Time.time + dropTime;
				dropping = true;
			}
			else if (Time.time > dropDelay && fMoveVelocityVertical == 0) {
				dropping = false;
			}

			//new dropcode
			if (fMoveVelocityVertical == 0 && fMoveVelocityHorizontal == 0 && onWall &&(Time.time > climbDelay)) {
				onWall = false;
				GetComponent<Rigidbody2D>().gravityScale = 1;
			}

			//Physics2D.IgnoreLayerCollision(0, 8, onWall || dropping || Input.GetKey("fMoveVelocityVertical"));
			Physics2D.IgnoreLayerCollision(0, 8, onWall || dropping);
			//move up if on the wall, otherwise let gravity do the work
			if (dropping) {
				if (GetComponent<Rigidbody2D>().velocity.y == 0) {
					GetComponent<Rigidbody2D>().isKinematic = true;
					GetComponent<Rigidbody2D>().AddForce(Vector2.up * -50);
					GetComponent<Rigidbody2D>().isKinematic = false;
				}
				GetComponent<Rigidbody2D>().velocity = new Vector2(fMoveVelocityHorizontal * maxSpeed, GetComponent<Rigidbody2D>().velocity.y);
			}
			else {
				if (!onWall) {
					GetComponent<Rigidbody2D>().velocity = new Vector2(fMoveVelocityHorizontal * maxSpeed, Mathf.Min(0f, GetComponent<Rigidbody2D>().velocity.y));
				}
				else {
					GetComponent<Rigidbody2D>().isKinematic = true;
					GetComponent<Rigidbody2D>().velocity = new Vector2(fMoveVelocityHorizontal * maxSpeed, fMoveVelocityVertical * climbSpeed);
					GetComponent<Rigidbody2D>().isKinematic = false;
				}
			}
		}
		else if (Output.IsAnswering) {
			GetComponent<Rigidbody2D>().velocity = new Vector2(0,0);
		}
	}
	public void ThrowTool(){
		if (Time.time > nextFire &&
			   !onWall &&
			   !Output.IsAnswering &&
			   GameObject.FindGameObjectsWithTag("Projectile").Length == 0 &&
			   GetComponent<Rigidbody2D>().velocity == Vector2.zero &&
			   projectilecode >= 0 &&
			   selectedTool.GetComponent<SelectedTool>().toolCounts[projectilecode] + selectedTool.GetComponent<SelectedTool>().bonusTools[projectilecode] > 0) {
				throwing = true;
				audioSource.PlayOneShot(throwTool, 2f); 
   				anim.SetBool("throw", true);
				float currentEnergy = energyController.currentEnergy;
				energyController.onThrow(projectilecode); 
				GameObject.Find("FireTool").transform.GetChild(0).GetComponent<FireButton>().Fire();  
				nextFire = Time.time + fireRate;
   				animDelay = Time.time + animTime;
   				Rigidbody2D newstar =(Rigidbody2D)Instantiate(projectiles[projectilecode], RoundPosition(transform.position), transform.rotation);
				controller.logger.onToolUse(projectilecode, lastLineNumberactive);
				timeCurrent = DateTime.Now.Second - timeStart;
				controller.logger.onStateChangeJson(projectilecode, lastLineNumberactive, currentEnergy,energyController.currentEnergy, true, timeCurrent); 
   				if (facingRight) {
   					newstar.GetComponent<Rigidbody2D>().AddForce(Vector2.right * 300);
   				}
   				else {
   					newstar.GetComponent<Rigidbody2D>().AddForce(Vector2.right * -300);
   				}
			}
	}
	IEnumerator MoveToPosition(Vector3 position){
		if (position.x > transform.position.x)
			facingRight = true; 
		else facingRight = false; 
		anim.SetBool("facingRight", facingRight);
		yield return new WaitForSecondsRealtime(0.2f);
		if (Input.GetMouseButton(0)) {		
			reachedPosition = false; 
			isMovingX = true; 

			while(Math.Abs(GetComponent<Transform>().localPosition.x-position.x) > 0.6f){
				if (this.transform.position.x - position.x < 0) facingRight = true;
				else facingRight = false; 
				yield return null;
			}
			isMovingX = false; 
			while(Math.Abs(GetComponent<Transform>().localPosition.y - position.y) > 0.6f){
				if (GetComponent<Transform>().localPosition.y - position.y < 0)
					verticalMovement = 0.5f; 
				else verticalMovement = -1f; 
				yield return null; 
			}
			reachedPosition = true; 
		}
	}
	Vector3 RoundPosition(Vector3 position){
		Transform lineAbove=null, lineBelow = null; 
		int lineNumber = 0; 
		foreach(GameObject line in lg.manager.lines){
			lineNumber++; 
			if (line.GetComponent<Transform>().position.y < position.y){
				lineBelow = line.GetComponent<Transform>(); 
				break; 
			} else lineAbove = line.GetComponent<Transform>(); 
		}
		lastLineNumberactive = lineNumber; 
		if (lineAbove == null || lineBelow == null){
			return position; 
		}
		return new Vector3(position.x, (lineAbove.position.y-lineBelow.position.y)/2 + lineBelow.position.y, position.z); 
	}
	//.................................>8.......................................
	void Update() {
		if (GlobalState.GameState == stateLib.GAMESTATE_IN_GAME && 
			!anim.GetCurrentAnimatorStateInfo(0).IsName(GlobalState.Character.ToLower() + "Die")
			&& !anim.GetCurrentAnimatorStateInfo(0).IsName(GlobalState.Character.ToLower() + "Dead")) {
			AudioSource ad = GetComponent<AudioSource>();
			if (!walkloop && (Input.GetAxis("Horizontal") != 0f && Input.GetAxis("Mouse X") != 0f) &&
			Input.GetMouseButton(0)&&
			GetComponent<Rigidbody2D>().velocity.y == 0 &&
			!onWall) {
				ad.Play();
				walkloop = true;
				ad.loop = true;
			}
			if (Input.GetAxis("Horizontal") == 0f && (Input.GetAxis("Mouse X") == 0f && Input.GetMouseButton(0))||
			GetComponent<Rigidbody2D>().velocity.y != 0 ||
			onWall) {
				ad.loop = false;
				walkloop = false;
			}

			//firing
			if ((Input.GetKeyDown("left ctrl") || Input.GetKeyDown("right ctrl"))){
				ThrowTool();
			}
			if (Input.GetMouseButtonDown(0)){
				Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition); 
				Bounds collider = GameObject.Find("CodeScreen").GetComponent<EdgeCollider2D>().bounds; 
				if (pos.x < collider.center.x + collider.size.x/2 && pos.y > collider.center.y - collider.size.y/2
					&& !fire.IsFiring) {
					StartCoroutine(MoveToPosition(RoundPosition(pos))); 
				}
			}
			else if (Input.GetMouseButtonUp(0)){
				StopAllCoroutines();
			}		   
			if (Time.time > animDelay) {
				anim.SetBool("throw", false);
			}
			//quit
			if (Input.GetKeyDown(KeyCode.Escape) == true) {
				quitting = true;
			}
			if (quitting) {
				if (Input.GetKeyDown("y")) {
					Application.Quit();
				}
				else if (Input.GetKeyDown("n")) {
					quitting = false;
				}
			}
		}
		else {
			GetComponent<AudioSource>().loop = false;
		}
	}


	//.................................>8.......................................

}

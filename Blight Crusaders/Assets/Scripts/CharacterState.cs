using UnityEngine;
using System.Collections;

public class CharacterState : MonoBehaviour {
	//the max health of a character, also its starting health
	public double health_max;
	//initial offset from the center of the screen.  cannot be 0, as this is used to determine which way this should move when damaged
	public float distance_initial_offset;

	bool active;
	bool inrange = false;

	//the total length of the screen.  It gets this from outside this scrip.
	private float screen_length;
	//used for keeping time
	private float time_next_second;
	//the amount of time left before this character can act.  In seconds.
	private int cooldown;
	//current health.  use get_health() to access and take_damage(...) to modify.
	private double health_current;
	//current health as a percentage of the max health
	private float health_percent;
	//how far it should be from the closest edge of the screen
	private float distance;


	// Use this for initialization
	void Start () {
		this.active = true;
		this.time_next_second = 0;
		this.cooldown = Random.Range(3,7);

		this.health_max = 100;

		this.health_current = this.health_max;
		this.health_percent = (float)this.health_current / (float)this.health_max;


		this.screen_length = Camera.main.orthographicSize; 
		print ("!!!ALERT!!!:  CharacterState.cs USING DUMMY   (/0 ^0)/ ^ I_____I");

		this.distance = ((this.screen_length / 2) - Mathf.Abs(distance_initial_offset)) * this.health_percent;


		//if the initial offset is wrong, complain and blow up
		if (distance_initial_offset == 0){
			throw new UnityException("CharacterState.cs exception:  distance_initial_offset is set to 0 on entity: " + this.name);
		}
		if (Mathf.Abs(distance_initial_offset) >= (this.screen_length / 2)){
			throw new UnityException("CharacterState.cs exception:  distance_initial_offset >= (screen_length / 2)");
		}

		move_according_to_health ();



		print ("CharacterState Debug button enabled");
	}
	
	// Update is called once per frame
	void Update () {
		if ((transform.position.x < -13) || (transform.position.x > 14)) {
			death ();
		}
		//Debug.Log ("SCREEN SIZE IS: "+ screen_length);
		if (Time.time >= this.time_next_second) { 
			time_next_second = Time.time + 1;
			if (cooldown > 0 && getActive()){
			cooldown--;
			}
			//print ("Cooldown:  UPDATE  |  time left:  " + this.cooldown + " | on_cd?: " + this.on_cooldown_huh());
		}

		if(Input.GetKeyDown("f")){
			print ("CharacterState Debug button pressed");
			this.take_damage(10);
		}
	}

	//put this character on cooldown (make it unable to act) for 'given_cooldown' seconds
	public void cooldown_start(int given_cooldown){
		this.cooldown = given_cooldown;
	}

	//is this currently on cooldown (not able to act)?  if yes, return true, else false.
	public bool on_cooldown_huh(){
		return this.cooldown > 0;
	}

	//what's this character's current health?
	public double get_health(){
		return health_current;
	}

	public void setInactive(){
		this.active = false;
	}

	public void setActive(){
		this.active = true;
	}

	public bool getActive(){
		return this.active;
	}


	//make this character take 'given_damage' amount of damage
	//NOTE:  this will move the character as well
	public void take_damage(double given_damage){
		this.health_current = this.health_current - given_damage;
		if(health_current <= 0){
			death();
		}
		this.health_percent = (float)this.health_current / (float)this.health_max;
		this.distance = ((this.screen_length / 2) - Mathf.Abs(distance_initial_offset)) * this.health_percent;
		move_according_to_health();
	}

	/*
	// Update is called once per frame
	void Update () {
		
		//know if other enemies are attacking, (even itself)
		bool e1at = e1.GetComponent<EnemyAttack> ().attacking;
		
		bool e2at = e2.GetComponent<EnemyAttack> ().attacking; 
		bool e3at = e3.GetComponent<EnemyAttack> ().attacking;

		if ((!e1at && !e2at && !e3at) || attacking) {
			attackcycle (p1);
		}
		if (!attacking) {
			
			getback ();
			animator.SetInteger("Direction",0);
		}
	}

	public void attackcycle(GameObject player){
		if (!this.on_cooldown_huh () && this.getActive()) {
			Debug.Log("HEY");
			attacking = true;
			// Decide(k
			moveto (p1);
			attack += Time.deltaTime;
			
			
		} else {
			attacking =false;
		}
		
		if (attack > 1.0) {
			state.cooldown_start (Random.Range (7, 10));
			attack = 0.0;
		}
	}*/


	bool inRange(Vector3 origpos, Vector3 destpos){
		return Vector3.Distance(destpos, origpos) < 1;
	}
	
	//enemy approaches to player, preform melee attack
	//returns new posn/*
	public IEnumerator moveTo(GameObject dest){
		Debug.Log("HEY");
		Vector3 origposn = transform.position;
		Vector3 destposn = dest.transform.position - new Vector3(1,0,0);
		if (Vector3.Distance(origposn, destposn) < 3) {
			inrange=true;
			Animator animator = GetComponent<Animator>();
			animator.SetInteger("Direction", 1);
			//StartCoroutine(dest.GetComponent<CharacterState>().takeOtherDamage(dest));
		}
		while (!inrange) {
			transform.position = Vector3.Lerp(origposn, destposn, .05f);
			yield return null;
		}

		
	}/*
	
	//move the enemy to the starting position
	void getback(){
		Vector3 origposn = transform.position;
		if (inrange) {
			transform.position = Vector3.Lerp(origposn, selforigposn, .05f);
		}
		if ((Mathf.Abs (origposn.x - selforigposn.x) < 1) &&
		    (Mathf.Abs (origposn.y - selforigposn.y) < 1) &&
		    (Mathf.Abs (origposn.z - selforigposn.z) < 1)) {
			inrange=false;
		}
	}*/

	//no touchie
	private void move_according_to_health(){
		float new_x_position;
		//if it's on the left side
		if(distance_initial_offset < 0){
			new_x_position = distance;
		}
		//if it's on the right side
		else{
			new_x_position = screen_length - distance;
		}
		print (new_x_position);
		this.transform.position = Vector3.Scale(transform.localScale, new Vector3 (new_x_position, this.transform.position.y, this.transform.position.z));
	}

	private void death(){
		if (this.gameObject.name == "P1") {
			Application.Quit ();
		} else {
			Destroy (this.gameObject);
			GameManager.instance.PopCharacter (this.gameObject);
		}
	}
}

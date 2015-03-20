using UnityEngine;
using System.Collections;

public class CharacterState : MonoBehaviour {
	//the max health of a character, also its starting health
	public double health_max;
	//initial offset from the center of the screen.  cannot be 0, as this is used to determine which way this should move when damaged
	public float distance_initial_offset;

	bool active;
	bool canQueue = true;
	bool inrange = false;
	Animator animator;

	public AnimationClip hurt;
	public AnimationClip attack;

	//the total length of the screen.  It gets this from outside this scrip.
	private float screen_length;
	//used for keeping time
	private float time_next_second;
	//the amount of time left before this character can act.  In seconds.
	private int cooldown;
	//current health.  use get_health() to access and take_damage(...) to modify.
	public double health_current;
	//current health as a percentage of the max health
	private float health_percent;
	//how far it should be from the closest edge of the screen
	private float distance;
	float time = 1.0f;
	float elaspedTime = 0.0f;



	// Use this for initialization
	void Start () {
		this.active = true;
		this.time_next_second = 0;
		this.cooldown = Random.Range(3,7);
		this.animator = GetComponent<Animator> ();

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

		if(this.on_cooldown_huh()){

		}

		if(Input.GetKeyDown("f")){
			print ("CharacterState Debug button pressed");
			this.take_damage(10);
		}
	}

	//put this character on cooldown (make it unable to act) for 'given_cooldown' seconds
	public void cooldown_start(int given_cooldown){
		this.cooldown = given_cooldown;
		float i = 0.0f;
		while(i < given_cooldown){
			Color.Lerp(Color.black, Color.white, given_cooldown);
			i += Time.deltaTime;
		}
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

	public void setCanQueue(){
		this.canQueue = true;
	}
	
	public void setCannotQueue(){
		this.canQueue = false;
	}
	
	public bool getCanQueue(){
		return this.canQueue;
	}

	public float returnHurt(){
		return hurt.length;
	}

	public IEnumerator getHurt(){
		animator.SetInteger("Direction", 1);
		yield return new WaitForSeconds(attack.length);
		Debug.Log(hurt.length);
		animator.SetInteger("Direction", 0);
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
		move_according_to_health ();
	}
	
	//enemy approaches to player, preform melee attack
	//returns new posn/*
	public IEnumerator moveTo(GameObject dest){
		Vector3 origposn = transform.position;
		Vector3 destposn = dest.transform.position + new Vector3(1,0,0);
		while (elaspedTime < time) {
			transform.position = Vector3.Lerp(origposn, destposn, (elaspedTime / time));
			elaspedTime += Time.deltaTime;
			yield return null;
		}
		return;
	}

	
	//move the enemy to the starting position
	IEnumerator getback(Vector3 originalPos){
		Vector3 currentPos = transform.position;
		while (elaspedTime < time) {
			transform.position = Vector3.Lerp(currentPos, originalPos, (elaspedTime / time));
			elaspedTime += Time.deltaTime;
			yield return null;
		}
		if (Vector3.Distance(transform.position, originalPos) < 1) {
			GameManager.instance.UnFreezeCharacters();
			inrange=false;
			elaspedTime = 0.0f;
			this.cooldown_start(4);
		}
	}

	IEnumerator slowTime(){
		Time.timeScale = 0.5f;
		yield return new WaitForSeconds(0.03f);
		Time.timeScale = 1.0f;
	}

	//no touchie
	private void move_according_to_health(){
		//this.animator.SetInteger ("Direction", 2);
		//yield return new WaitForSeconds (hurt.length);
		//this.animator.SetInteger ("Direction", 0);
		float new_x_position;
		//if it's on the left side
		if (distance_initial_offset < 0) {
			new_x_position = distance;
		}
		//if it's on the right side
		else {
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

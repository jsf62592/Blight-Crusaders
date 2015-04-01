using UnityEngine;
using System.Collections;

public class CharacterState : MonoBehaviour {
	//the max health of a character, also its starting health
	public int health_max;
	//initial offset from the center of the screen.  cannot be 0, as this is used to determine which way this should move when damaged
	public float distance_initial_offset;

	//a char is set inactive when another char is attacking
	bool activehuh;

	bool canQueue = true;
	//are you next to your target and thus, should stop moving and start attacking
	bool inrange = false;
	Animator animator;

	public AnimationClip hurt;
	public AnimationClip attack;

	//the total length of the screen.  It gets this from outside this scrip.
	private float screen_length;
	//used for keeping time
	private float time_next_second;
	//the amount of time left before this character can act.  In seconds.
	private float cooldown;
	//current health.  use get_health() to access and take_damage(...) to modify.
	private double health_current;
	//current health as a percentage of the max health
	private float health_percent;
	//how far it should be from the closest edge of the screen
	private float distance;
	float time = 1.0f;
	float elaspedTime = 0.0f;
	SpriteRenderer sp;


	// Use this for initialization
	void Start () {
		sp = GetComponent<SpriteRenderer> ();
		Debug.Log("THIS IS THE COLOR: " + sp.color);
		this.activehuh = true;
		this.time_next_second = 0;
		this.cooldown = 5.0f;
		this.animator = GetComponent<Animator> ();

		this.health_max = 100;

		this.health_current = this.health_max;
		this.health_percent = (float)this.health_current / (float)this.health_max;


		this.screen_length = Camera.main.orthographicSize; 
		print ("!!!ALERT!!!:  CharacterState.cs USING DUMMY   (/0 ^0)/ ^ I_____I");

		this.distance = ((this.screen_length / 2) - Mathf.Abs(distance_initial_offset)) * this.health_percent;


		//the initial offset is used in other places to determine a gameobject's team, so it needs to be non-zero
		if (distance_initial_offset == 0){
			throw new UnityException("CharacterState.cs exception:  distance_initial_offset is set to 0 on entity: " + this.name);
		}
		//if the initial offset is larger than half of the screen, that means something is starting off-screen, so complain and blow up.
		if (Mathf.Abs(distance_initial_offset) >= (this.screen_length / 2)){
			throw new UnityException("CharacterState.cs exception:  distance_initial_offset >= (screen_length / 2) | name: " + this.name + " | screen_length: " + screen_length);
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
	public void cooldown_start(float given_cooldown){
		this.cooldown = given_cooldown;
		StartCoroutine(changeColor (given_cooldown));
	}

	IEnumerator changeColor(float cooldown){
		float t = 0.0f;
		while (t < cooldown) {
			if(this.getActive()){
				sp.color = Color.Lerp (Color.black, Color.white, t / cooldown);
				t += Time.deltaTime;
				yield return null;
			}
			else {
				yield return null;
			}
		}
		if(t >= cooldown)
		{
			sp.color = Color.white;
		}
		elaspedTime = 0.0f;
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
		this.activehuh = false;
	}

	public void setActive(){
		this.activehuh = true;
	}

	public bool getActive(){
		return this.activehuh;
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

	public IEnumerator getHurt(){
		animator.SetInteger("Direction", 1);
		yield return new WaitForSeconds(attack.length);
		Debug.Log(hurt.length);
		animator.SetInteger("Direction", 0);
	}

	//make this character take 'given_damage' amount of damage
	//NOTE:  this will move the character as well
	public IEnumerator take_damage(double given_damage){
		this.health_current = this.health_current - given_damage;
		this.animator.SetInteger ("Direction", 2);
		yield return  new WaitForSeconds (hurt.length);
		this.animator.SetInteger ("Direction", 0);
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
		elaspedTime = 0.0f;
		Vector3 origposn = transform.position;
		Vector3 destposn;
		if(dest.tag == "Player"){
			destposn = dest.transform.position + new Vector3(1,0,0);
		} else {
			destposn = dest.transform.position - new Vector3(1,0,0);
		}
		while (elaspedTime < time) {
			transform.position = Vector3.Lerp(origposn, destposn, (elaspedTime / time));
			elaspedTime += Time.deltaTime;
			yield return null;
		}
		if (Vector3.Distance(transform.position, destposn) < 1) {
			inrange=true;
			Animator animator = GetComponent<Animator>();
			animator.SetInteger("Direction", 1);
			dest.GetComponent<Animator>().SetInteger("Direction", 2);
			dest.AddComponent<SE_Enemy_Fireball>().apply_effect();
			StartCoroutine("slowTime");
			float x = dest.transform.position.x;
			if(dest.tag != "Player"){
				x += 1;
			} else {
				x -= 1;
			}
			dest.transform.position = new Vector3(x, dest.transform.position.y, dest.transform.position.z);
			yield return new WaitForSeconds(attack.length);
			animator.SetInteger("Direction", 0);
			dest.GetComponent<Animator>().SetInteger("Direction", 0);
			elaspedTime = 0.0f;
			StartCoroutine(getback(origposn));
		}
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
	public void move_according_to_health(){
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

using UnityEngine;
using System.Collections;

public class CharacterState : MonoBehaviour {
	//the max health of a character, also its starting health
	public int health_max = 100;
	//initial offset from the center of the screen.  cannot be 0, as this is used to determine which way this should move when damaged
	public float distance_initial_offset;

	//a char is set inactive when another char is attacking
	bool activehuh;

	bool canQueue = true;
	//are you next to your target and thus, should stop moving and start attacking
	bool inrange = false;
	Animator animator;

	public AnimationClip hurt_animation;
	public AnimationClip attack_animation;

	//the total length of the screen.  It gets this from outside this scrip.
	private float screen_length;
	//used for keeping time
	private float time_next_second;
	//the amount of time left before this character can act.  In seconds.
	private float cooldown;
	//current health.  use get_health() to access and take_damage(...) to modify.
	public double health_current;
	//current health as a percentage of the max health
	private float health_percent;
	//how far it should be from the middle of the screen.  (x=0)
	//note that this is only calculated at start() and when moving according to health
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


		this.health_current = this.health_max;
		this.health_percent = (float)this.health_current / (float)this.health_max;



		print ("!!!ALERT!!!:  CharacterState.cs USING DUMMY.  THIS IS IN CAPS SO IT IS IMPORTANT.");
		//this.screen_length = Camera.main.orthographicSize; 
		this.screen_length = 20f;


		//calculate the distance from the middle
		distance = (((screen_length / 2) - Mathf.Abs(distance_initial_offset)) * (1 - health_percent)) + Mathf.Abs(distance_initial_offset);


		//the initial offset is used in other places to determine a gameobject's team, so it needs to be non-zero
		if (distance_initial_offset == 0){
			throw new UnityException("CharacterState.cs:  distance_initial_offset is set to 0 on entity: " + this.name);
		}
		//if the initial offset is larger than half of the screen, that means something is starting off-screen, so complain and blow up.
		if (Mathf.Abs(distance_initial_offset) >= (this.screen_length / 2)){
			throw new UnityException("CharacterState.cs:  distance_initial_offset >= (screen_length / 2) | name: " + this.name + " | screen_length: " + screen_length);
		}

		if (attack_animation == null){
			throw new UnityException("CharacterState.cs:  no attack_animation set: " + this.name);
		}
		if (hurt_animation == null){
			throw new UnityException("CharacterState.cs:  no hurt_animation set: " + this.name);
		}

		move_according_to_health ();



		print ("CharacterState Debug button enabled");
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time >= this.time_next_second) { 
			time_next_second = Time.time + 1;
			if (cooldown > 0 && activehuh){
				cooldown--;
			}
			//print ("Cooldown:  UPDATE  |  time left:  " + this.cooldown + " | on_cd?: " + this.on_cooldown_huh());
		}
		apply_effects();
		if(Input.GetKeyDown("f")){
			print ("CharacterState Debug button pressed");
			take_damage(10);
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
	}

	public IEnumerator attackMelee(){
		this.animator.SetInteger ("Direction", 1);
		yield return  new WaitForSeconds (attack_animation.length);
		this.animator.SetInteger ("Direction", 0);
	}

	protected IEnumerator getHurt(){
		this.animator.SetInteger ("Direction", 2);
		yield return  new WaitForSeconds (hurt_animation.length);
		this.animator.SetInteger ("Direction", 0);
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
	
	//make this character take 'given_damage' amount of damage
	//NOTE:  this will move the character as well
	public void take_damage(int given_damage){
		StartCoroutine (getHurt ());
		//modify the current health
		health_current = health_current - given_damage;
		//if no health is left, call death()
		if(health_current <= 0){
			//death();
		}
		//set health_percent for the new current health
		health_percent = (float)health_current / (float)health_max;
		//move appropriately
		move_according_to_health();
	}
	
	//DO NOT TOUCH THIS.  UNLESS YOUR NAME IS JAMES O'BRIEN, DO NOT TOUCH THIS.
	public void move_according_to_health(){
		float new_x_position;

		//calculate the distance from the middle
		distance = (((screen_length / 2) - Mathf.Abs(distance_initial_offset)) * (1 - health_percent)) + Mathf.Abs(distance_initial_offset);

		//if it's on the left side
		if(distance_initial_offset < 0){
			new_x_position = -1 * distance;
		}
		//if it's on the right side
		else{
			new_x_position = distance;
		}
		print ("CharacterState | " + this.name + "moved from x-coord: " + transform.position.x + " to new x-coord: " + new_x_position);
		transform.position = new Vector3 (new_x_position, transform.position.y, transform.position.z);
	}

	private void death(){
		if (this.gameObject.name == "P1") {
			Application.Quit ();
		} else {
			Destroy (this.gameObject);
			GameManager.instance.PopCharacter (this.gameObject);
		}
	}

	//this applies status effects on this gameobject.  should only be used in update().
	private void apply_effects(){
		foreach(Status_Effect effect in this.gameObject.GetComponents<Status_Effect>()){
			effect.apply_effect();
		}
	}
}

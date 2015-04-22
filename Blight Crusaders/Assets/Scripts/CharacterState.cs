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
	public AnimationClip throw_animation;
	public AnimationClip buff_animation;

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
	bool dead;
	bool attacking;

	// Use this for initialization
	void Start () {
		dead = false;
		sp = GetComponent<SpriteRenderer> ();
		Debug.Log("THIS IS THE COLOR: " + sp.color);
		this.activehuh = true;
		this.time_next_second = 0;
		this.animator = GetComponent<Animator> ();

		this.attacking = false;

		this.health_current = this.health_max;
		this.health_percent = (float)this.health_current / (float)this.health_max;

		if (this.gameObject.name == "P1") {
			this.cooldown_start(6);
		}

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

		if (cooldown == 0 && getActive () && gameObject.tag != "Player" && !isDead()) {
			GameObject prefab1 = (GameObject) Instantiate(Resources.Load("Prefabs/ready"), transform.position, transform.rotation);
			Destroy(prefab1, 1.0f);
		}


		if(!attacking){
			apply_effects();
		}
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

	public void setBlack(){
		sp.color = Color.black;
	}

	IEnumerator changeColor(float cooldown){
		float t = 0.0f;
		while (t < cooldown) {
			if (this.getActive ()) {
				if (get_health () > this.health_max / 2) {
					sp.color = Color.Lerp (Color.black, Color.white, t / cooldown);
				} else {
					
					sp.color = Color.Lerp (new Color32 (0, 0, 10, 255), new Color32 (200, 200, 255, 255), t / cooldown);
				}
				t += Time.deltaTime;
				yield return null;
			}
			else {
				yield return null;
			}
		}
		if (t >= cooldown) {
			if (get_health () > this.health_max / 2) {
				sp.color = Color.white;
			} else {
				sp.color = new Color32 (200, 200, 255, 255);
				
			}
		}
	}

	public IEnumerator attackMelee(){
		this.animator.SetInteger ("Direction", 1);
		yield return  new WaitForSeconds (attack_animation.length);
		this.animator.SetInteger ("Direction", 0);
	}

	public void moveMelee(){
		this.animator.SetInteger("Direction", 4);
	}

	public void returnIdle(){
		Vector3 xScale = this.gameObject.transform.localScale;
		xScale.x *= -1;
		this.gameObject.transform.localScale = xScale;
		this.animator.SetInteger("Direction",0);
	}

	public IEnumerator rangedThrow(){
		this.animator.SetInteger ("Direction", 5);
		yield return new WaitForSeconds (throw_animation.length);
		this.animator.SetInteger ("Direction", 0);
	}

	public void moveBackMelee(){
		Vector3 xScale = transform.localScale;
		xScale.x *= -1;
		transform.localScale = xScale;
		this.animator.SetInteger("Direction", 4);
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

	public bool isDead(){
		return dead;
	}

	public bool getAttacking(){
		return attacking;
	}

	public void setAttacking(){
		attacking = true;
	}

	public void setNotAttacking(){
		attacking = false;
	}

	public void setInactive(){
		this.activehuh = false;
	}

	public bool returnAttacking(){
		return attacking;
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
		if(given_damage < 0){
			throw new UnityException("CharacterState.cs:  take_damage() given negative given_damage: " + this.name);
		}
		StartCoroutine (getHurt ());
		if(!isDead()){
			//modify the current health
			health_current = health_current - given_damage;
			//if no health is left, call death()
			if(health_current <= 0){
				health_current = 0;
				health_percent = (float)health_current / (float)health_max;
				//move appropriately
				move_according_to_health();
				death();
			} else {
				//set health_percent for the new current health
				health_percent = (float)health_current / (float)health_max;
				//move appropriately
				move_according_to_health();
			}
		}
	}

	//make this character heal 'given_heal' amount of damage
	//NOTE:  this will move the character as well
	public void heal(int given_heal){
		if(isDead()){
			return;
		}
		StartCoroutine (getBuffed ());
		
		if(((health_current + given_heal) <= health_max)){
			//modify the current health
			health_current = health_current + given_heal;
			//set health_percent for the new current health
			health_percent = (float)health_current / (float)health_max;
			//move appropriately
		}
		else{
			//modify the current health
			health_current = health_max;
			//set health_percent for the new current health
			health_percent = (float)health_current / (float)health_max;
			//move appropriately
		}
		if (this.gameObject.GetComponent<SE_Plague_Bolt> () != null) {
			DestroyObject(this.gameObject.GetComponent<SE_Plague_Bolt>());
		}
		
		
	}

	IEnumerator getBuffed(){
		this.animator.SetInteger ("Direction", 6);
		GameObject go = (GameObject)Instantiate (Resources.Load ("Prefabs/heal"), transform.position, transform.rotation);
		yield return new WaitForSeconds (buff_animation.length);
		this.animator.SetInteger ("Direction", 0);
		Destroy (go, .25f);
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
			this.animator.SetInteger("Direction", 3);
			GameManager.instance.FreezeOtherCharacters(this.gameObject);
			Interface.instance.Dead();
			Interface.instance.GameOver();
			dead = true;
		} else {
			this.animator.SetInteger("Direction", 3);
			GameManager.instance.EnemyDeath();
			dead = true;
		}
	}

	//this applies status effects on this gameobject.  should only be used in update().
	private void apply_effects(){
		foreach(Status_Effect effect in this.gameObject.GetComponents<Status_Effect>()){
			effect.apply_effect();
		}
	}
}

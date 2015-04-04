using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*================================================================
This comment will lay out how the ability system works and is
intended for people who just want to make a new ability and maybe
understand a bit how the process works.

This is how to make a new ability:

1.(optional) make a status effect

2.  make an ability script
	-this must have a 'setup(...)' function in 'start()'
	-this should override 'attachEffects(...)' such that it adds
		the status affects it should apply to the target

3.  attach the ability script to the gameobject(s) in the editor as 
	a component


This is how the ability works:

1.  something calls 'add_to_queue(...)'
2.  'add_to_queue(...)' makes a message and puts it on the queue
3.  that message eventually gets dequeued, at which point it 
	calls the ability's 'do_stuff(...)' method
4.  'do_stuff(...)' moves the character "casting" the ability to
	the appropriate spot, plays an attack animation, adds status 
	effects to the target as components, and then moves the 
	"caster" back to its original position

================================================================*/

public abstract class Ability : MonoBehaviour {
	
	//this is the cooldown on the ability
	protected int max_cooldown;
	
	//this is the CharacterState of what this is attached to
	protected CharacterState state;

	//is this ability melee or ranged?
	//determines where the character moves to begin attack animation
	bool meleehuh;
	
	float time = 2.0f;
	Vector3 original_position;
	Vector3 original_enemy_position;
	Vector3 attack_position;


	//used for keeping time
	private float time_next_second;

	protected float movement_progress = 0f;
	public float movement_rate = .1f;
	protected GameObject projectile_prefab;
	protected GameObject projectile_instance;

	void Start(){
		time_next_second = 0;
	}
	
	//call this in Start() and set the max_cooldown with it
	//complains if this ability is on something that doesn't have a CharacterState
	protected void setup(int given_max_cooldown, bool given_meleehuh, string given_projectile_loadpath){
		max_cooldown = given_max_cooldown;
		meleehuh = given_meleehuh;
		if((! given_meleehuh) && (Resources.Load(given_projectile_loadpath) == null)){
			throw new UnityException("Ability: " + this.name + " could not load prefab");
		}
		projectile_prefab = (GameObject) Resources.Load(given_projectile_loadpath);
		state = this.GetComponent<CharacterState> ();
		if(state == null){
			throw new UnityException("Ability: " + this.name +" could not find a CharacterState component");
		}
	}

	//get the amount of cooldown time for this ability
	public int get_max_cooldown(){
		return max_cooldown;
	}

	//add a message onto the ability queue
	public void add_to_queue(GameObject given_target){
		Message message = new Message(this.gameObject, given_target, this);
		GameManager.instance.AddQueueAction(message);
	}

	//this is what you redefine every ability.
	//this should only be called in do_stuff()
	//attaches various Status_Effects and tells the given_target to add_attached_status_effects()
	protected virtual void attachEffects(GameObject given_target){

}



	/*================================================================
	//YOU SHOULD PROBABLY IGNORE EVERYTHING BELOW THIS 
	================================================================*/



	public void do_stuff(GameObject given_target){
		StartCoroutine(omfg(given_target));
	}

	//this calls movement, animation, and status effect stuff
	//NOTE:  should only be called by a message on the ability queue
	public IEnumerator omfg(GameObject given_target){
		movement_progress = 0f;
		original_position = transform.position;
		original_enemy_position = given_target.transform.position;



		//move to the appropriate place to attack
		while (movement_progress <= 1){
			move_attack(movement_progress, given_target);
			movement_progress += movement_rate;
			yield return 0;
		}

		//play the attack animation
		yield return StartCoroutine (playAnimation());


		//attach all the status effects
		print ("attached");
		attachEffects (given_target);

		//move back to the original position
		while (movement_progress >= 0){
			move_back (movement_progress, given_target);
			movement_progress -= movement_rate;
			yield return 0;
		}

	}

	protected void move_attack(float given_lerp_proportion, GameObject given_target){
		//move to the appropriate place if this is a melee ability
		if (meleehuh){
			move_attack_melee(given_lerp_proportion, given_target);
		}
		//else this is ranged and should move appropriately
		else{
			move_attack_ranged(given_lerp_proportion, given_target);
		}
	}

	protected void move_back(float given_lerp_proportion, GameObject given_target){
		//move to the appropriate place if this is a melee ability
		if (meleehuh){
			move_attack_melee(given_lerp_proportion, given_target);
		}
	}

	//moves the character to be in front of the target
	protected void move_attack_melee(float given_lerp_proportion, GameObject given_target){
		if(given_target.tag == "Player"){
			attack_position = original_enemy_position + new Vector3(1,0,0);
		} else {
			attack_position = original_enemy_position - new Vector3(1,0,0);
		}
		transform.position = Vector3.Lerp(original_position, attack_position, given_lerp_proportion);
	}

	//move the character to shoot a ranged ability
	protected void move_attack_ranged(float given_lerp_proportion, GameObject given_target){

		if(given_target.tag == "Player"){
			if(given_lerp_proportion == 0){
				projectile_instance = (GameObject) Instantiate (projectile_prefab, transform.position + new Vector3(1,0,0), transform.rotation);
			}
			attack_position = original_enemy_position - new Vector3(1,0,0);
		} else {
			if(given_lerp_proportion == 0){
				projectile_instance = (GameObject) Instantiate (projectile_prefab, transform.position + new Vector3(1,0,0), transform.rotation);
			}
			attack_position = original_enemy_position - new Vector3(1,0,0);
		}

		projectile_instance.transform.position = Vector3.Lerp(original_position, attack_position, given_lerp_proportion);

		print (given_lerp_proportion);
		if (given_lerp_proportion >= .9){
			Destroy(projectile_instance.gameObject);
		}
	}

	//plays the attack animation
	protected IEnumerator playAnimation(){
		CharacterState cs = GetComponent<CharacterState> ();
		if (meleehuh) {
			yield return StartCoroutine (cs.attackMelee ());
		}
	}
}
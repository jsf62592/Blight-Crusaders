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

	//is this ability melee or ranged?
	//affects attack movement and animations
	bool meleehuh;

	//the prefab that holds the sprite of the projectile.  It also holds a transform
	protected GameObject projectile_prefab;
	//this is the instance of projectile_prefab that is made and moved around
	protected GameObject projectile_instance;

	//the position of this character before this ability was "cast"
	Vector3 original_position;
	//the position of the target before this ability was "cast"
	Vector3 original_enemy_position;
	//the position this character should be in when this ability is "cast"
	Vector3 attack_position;

	//used for tracking movement towards the target when this ability is "cast"
	protected float movement_progress;
	//how fast the character moves towards the target when this ability is "cast"
	//note:  movement speed is also affected by the distance between this character and the target
	protected float movement_rate = .025f;
	protected float projectile_rate = 0.05f;

	//this is the CharacterState component of what this is attached to
	protected CharacterState state;
	
	//this is how much the attacker should be offset from the target's position
	protected Vector3 melee_offset = new Vector3(1,0,0);
	//this is how much the projectile should be offset from the original position when spawned
	//and the target's position when destroyed
	protected Vector3 ranged_offset = new Vector3(1,0,0);


	//call this in Start() and set the max_cooldown with it
	//complains if this ability is on something that doesn't have a CharacterState
	protected void setup(int given_max_cooldown, bool given_meleehuh, string given_projectile_loadpath){
		max_cooldown = given_max_cooldown;
		meleehuh = given_meleehuh;

		//look for a CharacterState on what this is a component of
		state = this.GetComponent<CharacterState> ();
		//make sure this was setup(...) correctly
		//if this was attached to something without a CharacterState component, complain
		if(state == null){
			throw new UnityException("Ability: " + this.name +" could not find a CharacterState component");
		}

		//if this is a player, invert the melee_offset
		//if this isn't the player, invert the ranged_offset
		if(this.tag == "Player"){
			melee_offset.x = melee_offset.x * -1;
		}
		else{
			ranged_offset.x = ranged_offset.x * -1;
		}


		//if this is a ranged ability: load the prefab from resource
		if(! given_meleehuh){
			projectile_prefab = (GameObject) Resources.Load(given_projectile_loadpath);
			//if there wasn't a prefab at the given path, complain
			if(Resources.Load(given_projectile_loadpath) == null){
				throw new UnityException("Ability: " + this.name + " is ranged but could not load the prefab");
			}
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
	protected virtual void attachEffects(GameObject given_target){}



	/*================================================================
	//YOU SHOULD PROBABLY IGNORE EVERYTHING BELOW THIS 
	================================================================*/



	//only the message should call this
	public void do_stuff_wrapper(GameObject given_target){
		StartCoroutine(do_stuff(given_target));
	}

	//this calls movement, animation, and status effect stuff
	//NOTE:  should only be called by a message on the ability queue
	public IEnumerator do_stuff(GameObject given_target){
		//reset movement such that it starts at the beginning
		movement_progress = 0f;
		//record the original_position of this character
		original_position = transform.position;
		//record the original position of the target
		original_enemy_position = given_target.transform.position;

		//move to the appropriate place to attack
		while (movement_progress <= 1){
			move_attack(movement_progress, given_target);
			if(meleehuh){
				movement_progress += movement_rate;
			} else {
				movement_progress += projectile_rate;
			}
			yield return 0;
		}

		//play the attack animation
		yield return StartCoroutine (playAnimation());

		//attach all the status effects
		attachEffects (given_target);

		//move back to the original position
		while (movement_progress >= 0){
			move_back (movement_progress, given_target);
			movement_progress -= movement_rate;
			yield return 0;
		}

		//unfreeze other characters
		GameManager.instance.UnFreezeCharacters();
		state.cooldown_start (max_cooldown);
	}

	//move to attack.  does different things depending on whether this is a melee ability or not
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

	//move back to the original position if this is a melee ability
	protected void move_back(float given_lerp_proportion, GameObject given_target){
		//move to the appropriate place if this is a melee ability
		if (meleehuh){
			move_attack_melee(given_lerp_proportion, given_target);
		}
	}

	//moves the character to be in front of the target
	protected void move_attack_melee(float given_lerp_proportion, GameObject given_target){

		attack_position = original_enemy_position + melee_offset;
		transform.position = Vector3.Lerp(original_position, attack_position, given_lerp_proportion);
	}

	//move the projectile of a ranged ability
	protected void move_attack_ranged(float given_lerp_proportion, GameObject given_target){
		//if this is the first frame, spawn a new projectile
		if(given_lerp_proportion == 0){
			projectile_instance = (GameObject) Instantiate (projectile_prefab, original_position + ranged_offset, transform.rotation);
		}
		//calculate the position where this projectile should 'hit'
		attack_position = original_enemy_position - ranged_offset;

		projectile_instance.transform.position = Vector3.Lerp(original_position + ranged_offset, attack_position, given_lerp_proportion);

		if (given_lerp_proportion >= (1 - projectile_rate)){
			Destroy(projectile_instance.gameObject);
		}
	}

	//plays the attack animation
	protected IEnumerator playAnimation(){
		if (meleehuh) {
			yield return StartCoroutine (state.attackMelee ());
		}
	}
}
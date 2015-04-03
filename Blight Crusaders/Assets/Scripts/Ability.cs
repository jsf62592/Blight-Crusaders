﻿using UnityEngine;
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
	
	float elaspedTime = 0.0f;
	float time = 2.0f;
	public AnimationClip attack_animation;
	Vector3 original_position;
	Vector3 attack_position;


	//used for keeping time
	private float time_next_second;

	

	void Start(){
		time_next_second = 0;
	}
	
	//call this in Start() and set the max_cooldown with it
	//complains if this ability is on something that doesn't have a CharacterState
	protected void setup(int given_max_cooldown, bool given_meleehuh){
		max_cooldown = given_max_cooldown;
		meleehuh = given_meleehuh;
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
		float movement_progress = 0f;
		float movement_rate = .1f;

		//move to the appropriate place to attack
		while (movement_progress < 1){
			move_attack(movement_progress, given_target);
			movement_progress += movement_rate;
			yield return 0;
		}

		//play the attack animation
		//playAnimation ();

		//attach all the status effects
		print ("attached");
		attachEffects (given_target);

		//move back to the original position
		while (movement_progress > 0){
			//move_back (movement_progress);
			movement_progress -= movement_rate;
			yield return 0;
		}


		/*
		//move to the appropriate place if this is a melee ability
		if (meleehuh){
			moveto_melee(given_target);
		}
		//else this is ranged and should move appropriately
		else{
			moveto_ranged(given_target);
		}

		//play the attack animation
		StartCoroutine("playAnimation");

		//attach all the status effects
		attachEffects (given_target);
		if(meleehuh){
			//move back to the original position
			print("this should be changed ~<:|");
			StartCoroutine(moveto_destination (origposn));
		}
		*/

	}

	protected void move_attack(float given_lerp_proportion, GameObject given_target){
		//move to the appropriate place if this is a melee ability
		if (meleehuh){
			move_attack_melee(given_lerp_proportion, given_target);
		}
		/*
		//else this is ranged and should move appropriately
		else{
			move_attack_ranged(given_lerp_proportion, given_target);
		}
		*/
	}
	protected void move_back(float given_lerp_proportion){}

	//plays the attack animation
	protected void playAnimation(){
		Animator animator = GetComponent<Animator>();
		animator.SetInteger("Direction", 1);
		//yield return new WaitForSeconds(attack_animation.length);
		animator.SetInteger("Direction", 0);
	}

	//a helper function
	//moves the character to the destination
	protected void moveto_destination(Vector3 given_destination){
		float move_proportion = 0f;
		float duration = 15.0f;	
		while (move_proportion < 1f){
			print ("time: " + Time.time + " next: " + time_next_second);
		/*
			if (Time.time >= time_next_second) { 
				time_next_second = Time.time + .25f;
				print (move_proportion);
				//transform.position = Vector3.Lerp(origposn, given_destination, move_proportion);
				move_proportion += .25f;
			}
		*/
			move_proportion += Time.deltaTime/duration;
			time_next_second = Time.time + .25f;
		}
	}

	//moves the character to be in front of the target
	protected void move_attack_melee(float given_lerp_proportion, GameObject given_target){
		elaspedTime = 0.0f;
		original_position = transform.position;

		if(given_target.tag == "Player"){
			attack_position = given_target.transform.position + new Vector3(1,0,0);
		} else {
			attack_position = given_target.transform.position - new Vector3(1,0,0);
		}
		transform.position = Vector3.Lerp(original_position, attack_position, given_lerp_proportion);
	}

	//move the character to shoot a ranged ability
	protected void move_attack_ranged(float given_lerp_proportion, GameObject given_target){
		elaspedTime = 0.0f;
		original_position = transform.position;
		Vector3 destposn;
		if(given_target.tag == "Player"){
			destposn = transform.position + new Vector3(1,0,0);
		} else {
			destposn = transform.position - new Vector3(1,0,0);
		} 
		moveto_destination(destposn);
	}
}
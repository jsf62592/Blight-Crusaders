using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Ability : MonoBehaviour {
	
	//this is the cooldown on the ability
	protected int max_cooldown;
	
	//this is the CharacterState of what this is attached to
	protected CharacterState state;
	
	//call this in Start() and set the max_cooldown with it
	//complains if this ability is on something that doesn't have a CharacterState
	protected void setup(int given_max_cooldown){
		max_cooldown = given_max_cooldown;
		
		state = this.GetComponent<CharacterState> ();
		if(state == null){
			throw new UnityException("Ability: " + this.name +" could not find a CharacterState component");
		}
	}
	
	public int get_max_cooldown(){
		return max_cooldown;
	}

	protected virtual void playAnimation(){

	}

	protected virtual void attachEffects(){

	}
	
	//attaches various Status_Effects and tells the given_target to add_attached_status_effects()
	//NOTE:  should only be called by a message on the ability queue
	public virtual void do_stuff(GameObject selected, GameObject given_target){}
	
	//add a message onto the ability queue
	public void add_to_queue(GameObject given_target){
		Message message = new Message(this.gameObject, given_target, this);
		GameManager.instance.AddQueueAction(message);
	}
}
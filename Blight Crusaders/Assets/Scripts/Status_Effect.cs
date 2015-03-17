﻿using UnityEngine;
using System.Collections;

public abstract class Status_Effect : MonoBehaviour {

	protected int duration;

	protected bool applied_immediatehuh = false;

	protected CharacterState state;
	
	//does the stuff this should do.
	//the effect kills itself when it runs its course
	public void apply_effect(){
		//if the immediate effect has been applied and the duration isn't 0, apply the persistant_effect()
		if((applied_immediatehuh) && (duration > 0)){
			persistant_effect();
			duration--;
		}

		//if the immediate_effect() hasn't been applied, apply it
		if(! applied_immediatehuh){
			applied_immediatehuh = true;
			immediate_effect();
		}

		//if the status effect is over, apply the final_effect() and commit suicide
		if(duration <= 0){
			final_effect();
			Destroy(this);
		}

	}

	//call this in Start() and set the duration with it
	protected void setup(int given_duration){
		duration = given_duration;

		state = this.GetComponent<CharacterState> ();
		if(state == null){
			throw new UnityException("Status_Effect: " + this.name +" could not find a CharacterState component");
		}
	}

	//gets done once immediately
	protected virtual void immediate_effect(){}

	//applied over time (every second).  Begins one second after immediate_effect()
	//the amount of times this occurs is this.duration
	protected virtual void persistant_effect(){}

	//happens at the end of the effect's duration.  this should typically undo things (ex:  remove a blindness effect)
	protected virtual void final_effect(){}
}

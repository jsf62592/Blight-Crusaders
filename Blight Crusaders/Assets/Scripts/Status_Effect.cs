using UnityEngine;
using System.Collections;

public abstract class Status_Effect : MonoBehaviour {

	protected int duration;

	protected bool applied_immediatehuh = false;

	protected CharacterState state;
	
	void Start(){
		CharacterState state = this.GetComponent<CharacterState> ();
		if(state == null){
			throw new UnityException("Status_Effect: " + this.name +" could not find a CharacterState component");
		}
	}



	//does the stuff this should do.
	//returns true iff the effect ends.  "ends" meaning !(applied_immediatehuh || duration)
	//NOTE:  this can still be called after it ends, but it won't do anything other than return true.
	public bool apply_effect(){
		if((applied_immediatehuh) && (duration > 0)){
			persistant_effect();
			duration--;
		}

		if(! applied_immediatehuh){
			applied_immediatehuh = true;
			immediate_effect();
		}

		if((applied_immediatehuh) && (duration <= 0)){
			return true;
		}
		return false;
	}

	//gets done once immediately
	protected virtual void immediate_effect(){}

	//happens over time, the amount of seconds is this.persistance
	protected virtual void persistant_effect(){}
}

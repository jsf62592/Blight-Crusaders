using UnityEngine;
using System.Collections;

public class SE_Alch_Bomb : Status_Effect {
	
	void Start(){
		duration = 4;
		state = this.GetComponent<CharacterState> ();
		if(state == null){
			throw new UnityException("Status_Effect: " + this.name +" could not find a CharacterState component");
		}
		state.add_status_effect (this); print ("holy shit get rid of this debug thing");
	}

	protected override void immediate_effect(){
		state.take_damage (50);
	}

	protected override void persistant_effect(){
		state.take_damage (10);
	}
}

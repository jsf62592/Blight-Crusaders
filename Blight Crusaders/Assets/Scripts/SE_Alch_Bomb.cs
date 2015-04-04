using UnityEngine;
using System.Collections;

public class SE_Alch_Bomb : Status_Effect {
	
	void Start(){
		setup (4);
	}

	protected override void immediate_effect(){
		state.take_damage (50);
	}

	protected override void persistant_effect(){
		state.take_damage (10);
	}
}

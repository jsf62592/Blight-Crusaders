using UnityEngine;
using System.Collections;

public class SE_Strike : Status_Effect {
	
	void Start(){
		setup (10);
	}
	
	protected override void immediate_effect(){
		state.take_damage (30);
	}

	protected override void persistant_effect(){
		state.take_damage (10);
	}
}


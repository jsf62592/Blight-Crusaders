using UnityEngine;
using System.Collections;

public class SE_Strike2 : Status_Effect {
	
	void Awake() {
		setup (0);
	}
	
	protected override void immediate_effect(){
		state.take_damage(50);
	}
}

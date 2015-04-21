using UnityEngine;
using System.Collections;

public class SE_EnemyHeal : Status_Effect {
	
	// Use this for initialization
	void Awake () {
		setup (0);
	}
	
	protected override void immediate_effect(){
		//state.take_damage (50);
		state.heal (50);
	}
}

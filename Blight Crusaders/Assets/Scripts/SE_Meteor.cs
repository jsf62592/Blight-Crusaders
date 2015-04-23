using UnityEngine;
using System.Collections;

public class SE_Meteor : Status_Effect {

	// Use this for initialization
	void Awake () {
		setup (1);
	}
	
	protected override void persistant_effect(){
		state.take_damage (100);
	}
}

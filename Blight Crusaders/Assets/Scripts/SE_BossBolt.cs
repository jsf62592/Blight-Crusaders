using UnityEngine;
using System.Collections;

public class SE_BossBolt : Status_Effect {

	// Use this for initialization
	void Start () {
		setup (3);
	}
	
	protected override void immediate_effect(){
		state.take_damage (50);
	}
	
	protected override void persistant_effect(){
		state.take_damage (10);
	}
}

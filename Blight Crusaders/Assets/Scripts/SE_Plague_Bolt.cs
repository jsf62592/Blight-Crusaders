using UnityEngine;
using System.Collections;

public class SE_Plague_Bolt : Status_Effect {

	void Start(){
		setup (7);
	}
	protected override void persistant_effect(){
		state.take_damage (10);
	}
}

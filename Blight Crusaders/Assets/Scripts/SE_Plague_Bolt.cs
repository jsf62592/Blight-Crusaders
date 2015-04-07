using UnityEngine;
using System.Collections;

public class SE_Plague_Bolt : Status_Effect {

	void Start(){
		setup (4);
	}
	protected override void persistant_effect(){
		state.take_damage (10);
	}
}

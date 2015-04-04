using UnityEngine;
using System.Collections;

public class SE_Basic_Attack : Status_Effect {

	void Start(){
		setup (4);
	}
	
	protected override void immediate_effect(){
		state.take_damage (50);
	}
}

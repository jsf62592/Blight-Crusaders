using UnityEngine;
using System.Collections;

public class Ability_Basic_Attack : Ability {

	void Start(){
		setup (5, true, null);
	}

	protected override void attachEffects(GameObject given_target){
		given_target.AddComponent<SE_Basic_Attack> ();
	}
}

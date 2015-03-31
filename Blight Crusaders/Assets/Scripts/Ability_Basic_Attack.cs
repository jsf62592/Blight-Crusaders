using UnityEngine;
using System.Collections;

public class Ability_Basic_Attack : Ability {

	void Start(){
		Animator animation = GetComponent<Animator> ();
		setup (5, true);
	}

	protected override void attachEffects(GameObject given_target){
		given_target.AddComponent<SE_Basic_Attack> ();
	}
}

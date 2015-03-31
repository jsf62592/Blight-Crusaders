﻿using UnityEngine;
using System.Collections;

public class EnemyFireball : Ability {

	void Start(){
		setup(5, true);
	}
	protected override void attachEffects(GameObject given_target){
		given_target.AddComponent<SE_Basic_Attack> ();
	}
}

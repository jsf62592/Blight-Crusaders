using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Ability_Alch_Bomb : Ability {
	void Start(){
		setup (5);
	}
	public override void do_stuff(GameObject selected, GameObject given_target){
		given_target.AddComponent<SE_Alch_Bomb> ();
	}
}



using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Ability_debug : Ability {
	void Start(){
		setup (5, true);
	}
	protected override void attachEffects(GameObject given_target){
		given_target.AddComponent<SE_hellomynameisdebug> ();
	}
	void Update(){
		if(Input.GetKeyDown("g")){
			print ("Kamehameha");
			do_stuff(this.gameObject);
		}
	}
}
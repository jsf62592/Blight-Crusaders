using UnityEngine;
using System.Collections;
using System.Collections.Generic;



//!!!IMPORTANT!!!NOTE!!!IMPORTANT!!!
//!!!IMPORTANT!!!NOTE!!!IMPORTANT!!!
//!!!IMPORTANT!!!NOTE!!!IMPORTANT!!!
//THIS SHOULD NOT SERVE AS AN EXAMPLE FOR ANYONE LOOKING TO MAKE A NEW ABILITY.
//IF YOU NEED AN EXAMPLE, Ability_Alch_Bomb.cs IS A GOOD PLACE TO LOOK.  NOT HERE.
//!!!IMPORTANT!!!NOTE!!!IMPORTANT!!!
//!!!IMPORTANT!!!NOTE!!!IMPORTANT!!!
//!!!IMPORTANT!!!NOTE!!!IMPORTANT!!!



public class Ability_debug : Ability {
	void Start(){
		setup (5, true, null);
	}
	protected override void attachEffects(GameObject given_target){
		given_target.AddComponent<SE_hellomynameisdebug> ();
	}
	void Update(){
		if(Input.GetKeyDown("g")){
			print ("Kamehameha");
			//do not  do this.  ever.  do not do this.
			do_stuff(this.gameObject);  //do not  do this.  do not do this.
			//do not  do this.  ever.  do not do this.
		}
	}
}
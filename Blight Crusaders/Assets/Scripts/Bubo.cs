using UnityEngine;
using System.Collections;

public class Bubo : Enemy {

	Ability ba;
	Ability fb;
	Ability st;

	double target_Health;


	void Start(){
		ba = GetComponent<Ability_Basic_Attack> ();
		fb = GetComponent<Ability_Frostbolt> ();
		st = GetComponent<Ability_Strike> ();
	}

	public override void decision(GameObject target){
		target_Health = target.GetComponent<CharacterState> ().get_health ();
		if (target_Health > 70) {
			ba.add_to_queue (target);
		} else if (target_Health > 40) {
			st.add_to_queue(target);	
		} else {
			fb.add_to_queue(target);
		}
		GameManager.instance.FreezeOtherCharacters(this.gameObject);
		GetComponent<CharacterState>().setInactive();
	}
}

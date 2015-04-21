using UnityEngine;
using System.Collections;

public class Bloat : Enemy {
	
	Ability ba;
	Ability fb;
	Ability fw;
	Ability hl;
	Ability lc;
	
	double target_health;

	CharacterState state;
	
	// Use this for initialization
	void Start () {
		ba = GetComponent<Ability_Basic_Attack> ();
		fb = GetComponent<Ability_Frostbolt> ();
		fw = GetComponent<Ability_Firewalk> ();
		hl = GetComponent<Ability_EnemyHeal> ();
		lc = GetComponent<Ability_Leech> ();

		state = GetComponent<CharacterState> ();

	}
	
	public override void decision(GameObject target){
		target_health = target.GetComponent<CharacterState>().get_health();
		//fb.add_to_queue (target);

		if (state.get_health () < 100) {
			lc.add_to_queue (target);
		}else if (target_health > 230) {
			ba.add_to_queue (target);
		}
		else if (target_health > 150){
			fb.add_to_queue(target);
		
		}else{
			fw.add_to_queue(target);
		}

		
		GameManager.instance.FreezeOtherCharacters (this.gameObject);
		GetComponent<CharacterState> ().setInactive ();
	}
}

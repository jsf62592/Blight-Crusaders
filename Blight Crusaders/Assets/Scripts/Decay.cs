using UnityEngine;
using System.Collections;

public class Decay : Enemy {

	Ability ba;
	Ability ae;
	Ability fw;

	double target_health;

	// Use this for initialization
	void Start () {
		ba = GetComponent<Ability_Basic_Attack> ();
		ae = GetComponent<Ability_Frostbolt> ();
		fw = GetComponent<Ability_Firewalk> ();
	}
	
	public override void decision(GameObject target){
		target_health = target.GetComponent<CharacterState> ().get_health ();
		if(target_health > 230){
			ba.add_to_queue(target);
		} else if (target_health > 150){
			ae.add_to_queue(target);
		} else {
			fw.add_to_queue(target);
		}

		GameManager.instance.FreezeOtherCharacters (this.gameObject);
		GetComponent<CharacterState> ().setInactive ();
	}
}

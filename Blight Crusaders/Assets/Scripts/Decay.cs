using UnityEngine;
using System.Collections;

public class Decay : Enemy {
	
	Ability ba;
	Ability ae;
	Ability fw;
	
	double target_Health;
	
	
	void Start(){
		ba = GetComponent<Ability_Basic_Attack> ();
		ae = GetComponent<Ability_Aoe> ();
		fw = GetComponent<Ability_Firewalk> ();
	}
	
	public void decision(GameObject target){
		target_Health = target.GetComponent<CharacterState> ().get_health ();
		if (target_Health > 70) {
			ba.add_to_queue (target);
		} else if (target_Health > 40) {
			ae.add_to_queue(target);	
		} else {
			fw.add_to_queue(target);
		}
		GameManager.instance.FreezeOtherCharacters(this.gameObject);
		GetComponent<CharacterState>().setInactive();
	}
}

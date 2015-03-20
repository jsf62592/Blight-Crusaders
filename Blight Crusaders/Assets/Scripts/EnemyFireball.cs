using UnityEngine;
using System.Collections;

public class EnemyFireball : Ability {

	void Start(){
		setup(5);
	}

	public override void do_stuff(GameObject selected, GameObject given_target){
		CharacterState s = selected.GetComponent<CharacterState>();
		StartCoroutine(s.moveTo(given_target));
		//given_target.AddComponent<SE_Enemy_Fireball>().apply_effect();

	}
}

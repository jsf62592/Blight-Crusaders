using UnityEngine;
using System.Collections;

public class EnemyFireball : Ability {

	void Start(){
		setup(5);
	}

	public override void do_stuff(GameObject selected, GameObject given_target){
		CharacterState s = selected.GetComponent<CharacterState>();
		StartCoroutine(s.moveTo(given_target));

		move_to ();
		/*
		if (Vector3.Distance(transform.position, destposn) < 1) {
			inrange=true;
			Animator animator = GetComponent<Animator>();
			animator.SetInteger("Direction", 1);
			dest.GetComponent<Animator>().SetInteger("Direction", 2);


			//dest.AddComponent<SE_Enemy_Fireball>();


			//StartCoroutine("slowTime");
			yield return new WaitForSeconds(attack.length);
			animator.SetInteger("Direction", 0);
			dest.GetComponent<Animator>().SetInteger("Direction", 0);
			elaspedTime = 0.0f;




			StartCoroutine(getback(origposn));
		}*/
	}

	protected override IEnumerator playAnimation(){
		Animator.SetInteger ("Direction", 1);

	}

	protected override void apply_effects(){

	}
}

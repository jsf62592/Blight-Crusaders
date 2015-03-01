using UnityEngine;
using System.Collections;

public class Fireball : Ability {

	public void UseAbility(GameObject selected, GameObject targeted){
		Debug.Log ("PLAYER: " + selected.name);
		Debug.Log ("ENEMY: " + targeted.name);
	}
}

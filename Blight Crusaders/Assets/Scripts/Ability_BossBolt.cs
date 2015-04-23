using UnityEngine;
using System.Collections;

public class Ability_BossBolt : Ability {

	// Use this for initialization
	void Start () {
		setup (6, Visual_Types.ranged_projectile, "Prefabs/frostbolt");
	}
	
	protected override void attachEffects(GameObject given_target){
		//will be given some other attributes
		given_target.AddComponent<SE_BossBolt> ();
	}
}

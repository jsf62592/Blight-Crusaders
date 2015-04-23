using UnityEngine;
using System.Collections;

public class Ability_Meteor : Ability {

	// Use this for initialization
	void Start () {
		setup (10, Visual_Types.ranged_ascending, "Prefabs/Meteor");
	}
	
	protected override void  attachEffects(GameObject given_target){
		given_target.AddComponent<SE_Meteor> ();
	}
}

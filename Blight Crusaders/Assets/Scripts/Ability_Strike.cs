//citation of artwork --- Strike  
//original author : Max@wordpress http://orangemushroom.net/author/highonmushrooms/
//original artwork page: http://orangemushroom.net/2013/05/28/kmst-ver-1-2-478-adventurer-warrior-and-magician-reorganizations/




using UnityEngine;
using System.Collections;

public class Ability_Strike : Ability {

	// Use this for initialization
	void Start () {
		setup (7, Visual_Types.ranged_projectile, "Prefabs/strike");
	}
	
	protected override void attachEffects(GameObject given_target){
		given_target.AddComponent<SE_Strike> ();
	}
}

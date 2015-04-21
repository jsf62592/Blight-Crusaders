//citation of artwork --- Heal  
//original author : Max@wordpress http://orangemushroom.net/author/highonmushrooms/
//original artwork page: http://orangemushroom.net/2013/05/28/kmst-ver-1-2-478-adventurer-warrior-and-magician-reorganizations/



using UnityEngine;
using System.Collections;

public class Ability_EnemyHeal : Ability {
	
	// Use this for initialization
	void Start () {
		//Needs to launch the ability to the location
		setup (5, Visual_Types.self, "Prefabs/frenzy");
	}
	
	protected override void attachEffects(GameObject given_target){
		//will be given some other attributes
		this.gameObject.AddComponent<SE_EnemyHeal> ();
		//negative damage taken
	}
}

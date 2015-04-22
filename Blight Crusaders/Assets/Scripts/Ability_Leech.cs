//citation of artwork --- Leech  
//original author : Max@wordpress http://orangemushroom.net/author/highonmushrooms/
//original artwork page: http://orangemushroom.net/2013/05/28/kmst-ver-1-2-478-adventurer-warrior-and-magician-reorganizations/



using UnityEngine;
using System.Collections;

public class Ability_Leech: Ability {
	
	CharacterState state;
	void Start(){
		
		state = GetComponent<CharacterState> ();
		setup (5, Visual_Types.ranged_projectile, "Prefabs/leech");
	}
	protected override void attachEffects(GameObject given_target){
		double health = state.get_health();
		if (health > state.health_max/2){
		given_target.AddComponent<SE_Strike> (); //20
		}else{
			given_target.AddComponent<SE_Strike2>();	//50
		}
		this.gameObject.AddComponent<SE_EnemyHeal>();
	}
	
}
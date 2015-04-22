﻿//citation of artwork --- Firewalk  
//original author : Max@wordpress http://orangemushroom.net/author/highonmushrooms/
//original artwork page: http://orangemushroom.net/2013/05/28/kmst-ver-1-2-478-adventurer-warrior-and-magician-reorganizations/



using UnityEngine;
using System.Collections;

public class Ability_Firewalk : Ability {
	
	// Use this for initialization
	void Start () {
		setup (6, Ability.Visual_Types.ranged_ascending, "Prefabs/firewalk");
	}
	
	protected override void attachEffects(GameObject given_target){
		//will be given some other attributes
		given_target.AddComponent<SE_Strike> ();
	}
}

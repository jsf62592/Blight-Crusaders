using UnityEngine;
using System.Collections;

public class Ability_Alch_Bomb : Ability {

	public override void do_stuff(){

		state.add_status_effect (new SE_Alch_Bomb ());
	}
}

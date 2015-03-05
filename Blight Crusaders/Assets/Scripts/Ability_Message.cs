using UnityEngine;
using System.Collections;

public class Ability_Message : MonoBehaviour {
	//the ability that made it
	private Ability creator;

	//reference to target
	private GameObject target;
	


	public Ability_Message (Ability given_creator, GameObject given_target) {
		creator = given_creator;
		target = given_target;

		//complain if it gets null
		if ((creator == null) || (target == null)){
			throw new UnityException("Ability_Message given null");
		}
	}

	

	public Ability get_creator(){
		return creator;
	}

	public GameObject get_target(){
		return target;
	}
	
}

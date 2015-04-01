using UnityEngine;
using System.Collections;

public class Ability_Message : MonoBehaviour {
	//the ability that made it
	private Ability creator_ability;
	
	//the gameobject that created this message
	private GameObject creator_gameobject;
	
	//target the ability does stuff to
	private GameObject target;
	
	public Ability_Message (Ability given_creator, GameObject given_target) {
		creator_ability = given_creator;
		target = given_target;
		
		//complain if it gets null
		if ((creator_ability == null) || (target == null)){
			throw new UnityException("Ability_Message given null");
		}
		
		creator_gameobject = creator_ability.gameObject;
	}
	
	//call this to make
	public void make_ability_do_stuff(){
		creator_ability.do_stuff (target);
	}
	
	public Ability get_creator_ability(){
		return creator_ability;
	}
	
	public GameObject get_creator_gameobject(){
		return creator_gameobject;
	}
	
	public GameObject get_target(){
		return target;
	}
}

using UnityEngine;
using System.Collections;

public class Message {

	GameObject selected;
	GameObject targeted;
	Ability ability;

	public Message(GameObject selected, GameObject targeted, Ability ability){
		this.selected = selected;
		this.targeted = targeted;
		this.ability = ability;
	}

	public GameObject ReturnSelected(){
		return selected;
	}

	public GameObject ReturnTargeted(){
		return targeted;
	}

	public void UseAbility(){
		ability.do_stuff (selected, targeted);
	}
}

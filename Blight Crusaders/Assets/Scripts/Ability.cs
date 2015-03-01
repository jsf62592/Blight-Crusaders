using UnityEngine;
using System.Collections;

public abstract class Ability : MonoBehaviour {

	//status effects that will be inflicted upon this ability's target
	public ArrayList status_effects;

	//this is the cooldown on the ability
	public int max_cooldown = 1;

	protected CharacterState state;

	// Use this for initialization
	void Start () {
		state = this.GetComponent<CharacterState> ();
		if(state == null){
			throw new UnityException("Ability: " + this.name +" could not find a CharacterState component");
		}
	}
	
	//does what this ability does.  should only be called by a message on the ability queue
	//it's late though and i currently cannot be bothered to figure out a good way to restrict access to this
	//so expect this to be changed
	public virtual void do_stuff(){}

	//add a message onto the ability queue
	public void add_to_queue(GameObject given_target){
		Ability_Message message = new Ability_Message(this, ref given_target);
	}
}

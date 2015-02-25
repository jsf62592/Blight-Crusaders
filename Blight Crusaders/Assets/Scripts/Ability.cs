using UnityEngine;
using System.Collections;

public abstract class Ability : MonoBehaviour {

	//status effects that will be inflicted upon this ability's target
	public ArrayList status_effects;

	//this is the cooldown on the ability
	public int max_cooldown = 1;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {}

	//does what this ability does.  should only be called by a message on the ability queue
	//it's late though and i currently cannot be bothered to figure out a good way to restrict access to this
	//so expect this to be changed
	public static void message_callback(){


	}

	//add a message onto the ability queue
	public void add_to_queue(){
		Ability_Message message = new Ability_Message();
	}
}

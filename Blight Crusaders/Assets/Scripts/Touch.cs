using UnityEngine;
using System.Collections;

public class Touch : MonoBehaviour {

	double attack = 0.0; 
	CharacterState state;

	// Use this for initialization
	void Start () {
		state = GetComponent<CharacterState> ();
	}

	void Update(){
<<<<<<< HEAD
<<<<<<< HEAD
		if (cooldown > 0.0){ 
=======
		if (cooldown > 0){ 
>>>>>>> production
			Debug.Log(name + "cooldown: " + cooldown); 
			cooldown -= Time.deltaTime;
=======
		if (attack > 0.0){ 
			attack -= Time.deltaTime;
>>>>>>> origin/James
		}else{
			gameObject.renderer.material.color = Color.white;
		}
	}

	public void TouchResponse(){
<<<<<<< HEAD
		Debug.Log(name);
<<<<<<< HEAD
		if (tag == "Player" && cooldown <= 0.0) {
			
			Debug.Log ("touch.cs touched");
		//if(true){
=======
		if (tag == "Player" && cooldown <= 0) {
>>>>>>> production
=======
		Debug.Log (state.on_cooldown_huh ());
		if (tag == "Player" && !state.on_cooldown_huh()) {
>>>>>>> origin/James
			gameObject.renderer.material.color = Color.blue;
			attack = 1.0;
			state.cooldown_start(Random.Range(1,5));
		}
	}
<<<<<<< HEAD

=======
>>>>>>> production
}

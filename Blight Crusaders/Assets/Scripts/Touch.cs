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
		if (attack > 0.0){ 
			attack -= Time.deltaTime;
		}else{
			gameObject.renderer.material.color = Color.white;
		}
	}

	public void TouchResponse(){
		Debug.Log (state.on_cooldown_huh ());
		if (tag == "Player" && !state.on_cooldown_huh()) {
			gameObject.renderer.material.color = Color.blue;
			attack = 1.0;
			state.cooldown_start(Random.Range(1,5));
		}
	}
}

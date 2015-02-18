using UnityEngine;
using System.Collections;

public class EnemyAttack : MonoBehaviour {

	CharacterState state;
	double attack = 0.0;
	
	// Use this for initialization
	void Start () {
		state = GetComponent<CharacterState> ();
		state.cooldown_start (Random.Range(1,5));
	}
	
	// Update is called once per frame
	void Update () {
		if(!state.on_cooldown_huh()){
			gameObject.renderer.material.color = Color.red;
			attack += Time.deltaTime;
		} 

		if(attack > 1.0){
			state.cooldown_start(Random.Range (1, 5));
			gameObject.renderer.material.color = Color.white;
			attack = 0.0;
		}
	}
}

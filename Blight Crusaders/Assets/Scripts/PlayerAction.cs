using UnityEngine;
using System.Collections;
using System;

public class PlayerAction : MonoBehaviour {
	public double changeColors = 0.0;
	GameObject target;
	CharacterState state;
	
	// Use this for initialization
	void Start () {
		state = GetComponent<CharacterState> ();
		target = null;
	}
	
	void Update(){
		if (changeColors > 0.0) { 
			changeColors -= Time.deltaTime; 
		}
		if (changeColors == 0 && target != null) { 
			target.renderer.material.color = Color.white;
			target = null;
		}
	}
	
	public void Attack(GameObject targeted){
		target = targeted;
		Debug.Log ("HEALTH: " + target.GetComponent<CharacterState> ().get_health ());
		target.GetComponent<CharacterState> ().take_damage (5);
		Debug.Log (name + " Attacks " + target.name);
		target.renderer.material.color = Color.red;
		changeColors = 10;
		GetComponent<CharacterState> ().cooldown_start(UnityEngine.Random.Range(1,5));

	}
	
	void ChangeColors(){
		target.renderer.material.color = Color.red;
		
		changeColors = 10;
	}
	
	public void Select(){
		gameObject.renderer.material.color = Color.blue;
	}
	
	public void DeSelect(){
		gameObject.renderer.material.color = Color.white;
	}
	
}

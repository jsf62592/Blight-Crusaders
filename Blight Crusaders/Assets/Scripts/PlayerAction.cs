using UnityEngine;
using System.Collections;
using System;

public class PlayerAction : MonoBehaviour {
	public double changeColors = 0.0;
	GameObject target;
	CharacterState state;
	Boolean selected;
	Boolean selectable;
	public Texture Button1;
	public Texture Button2;
	public Texture Button3;

	// Use this for initialization
	void Start () {
		state = GetComponent<CharacterState> ();
		target = null;
	}
	
	void Update(){
		if (!state.on_cooldown_huh () && state.getActive ()) {
			selectable = true;
				} else {
			selectable = false;
				}

		if (selected) {
			gameObject.renderer.material.color = Color.blue;
		}else if (selectable) {
			gameObject.renderer.material.color = Color.red;
		}else{
			gameObject.renderer.material.color = Color.white;
		}


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
		target.GetComponent<CharacterState> ().take_damage (.5);
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
		GameManager.instance.FreezeOtherCharacters(this.gameObject);
		selected = true;
	}
	
	public void DeSelect(){
		selected = false;
	}

	public Texture GetButton1(){
		return Button1;
	}
	public Texture GetButton2(){
		return Button2;
	}
	public Texture GetButton3(){
		return Button3;
	}
	
}

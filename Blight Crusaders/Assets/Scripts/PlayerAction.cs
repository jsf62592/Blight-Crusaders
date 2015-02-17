using UnityEngine;
using System.Collections;

public class PlayerAction : MonoBehaviour {
	public int changeColors = 0;
	GameObject target;

	// Use this for initialization
	void Start () {
		target = null;
	}

	void Update(){
		if (changeColors > 0) { changeColors--; }
		if (changeColors == 0 && target != null) { 
			target.renderer.material.color = Color.white;
			target = null;
		}
	}

	public void Attack(GameObject targeted){
		target = targeted;
		Debug.Log (name + " Attacks " + target.name);
		ChangeColors();
		GetComponent<CharacterState> ().cooldown_start();
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

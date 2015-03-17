using UnityEngine;
using System.Collections;

public class AttackAndHurt : MonoBehaviour {

	Vector3 selforigposn;
	CharacterState state;
	bool inrange = false;
	Animator animator;
	

	
	// Use this for initialization
	void Start () {
		selforigposn = transform.position;
		animator = GetComponent<Animator> ();
		state = GetComponent<CharacterState> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//enemy approaches to player, preform melee attack
	//returns new posn
	public void moveto(GameObject dest){
		
		dest.GetComponent<CharacterState>().takeOtherDamage(dest);
		Vector3 origposn = transform.position;
		Vector3 destposn = dest.transform.position;
		if (!inrange) {
				transform.position = new Vector3 (approach (origposn.x, destposn.x),
	                                  approach (origposn.y, destposn.y),
	                                  approach (origposn.z, destposn.z));
		}
		if ((Mathf.Abs (origposn.x - destposn.x) < 3) &&
				(Mathf.Abs (origposn.y - destposn.y) < 3) &&
				(Mathf.Abs (origposn.z - destposn.z) < 3)) {
				inrange = true;
		}
	}
	
	
	
	//move the enemy to the starting position
	public void getback(){
		Vector3 origposn = transform.position;
		if (inrange) {
			transform.position = new Vector3 (approach (origposn.x, selforigposn.x),
			                                  approach (origposn.y, selforigposn.y),
			                                  approach (origposn.z, selforigposn.z));
		}
		if ((Mathf.Abs (origposn.x - selforigposn.x) < 1) &&
		    (Mathf.Abs (origposn.y - selforigposn.y) < 1) &&
		    (Mathf.Abs (origposn.z - selforigposn.z) < 1)) {
			inrange=false;
		}
	}
	
	
	
	//increase or decrease the first number gradually to match the second number
	float approach(float n1, float n2){
		if (n1 > n2) {
			if (n1 < (n2 + 1)) {
				return n1; }else{
				n1 -= (Mathf.Abs(n1-n2))/15;
			}
		}else if (n1 < n2){
			if(n1>(n2-1)){
				return n1;
			}else{
				n1 += (Mathf.Abs(n1-n2))/15;
			}
		}
		return n1;
	}
}

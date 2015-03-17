using UnityEngine;
using System.Collections;

public class EnemyAttack : MonoBehaviour {
	
	CharacterState state;
	double attack = 0.0;

	//health of players
	GameObject p1;
	GameObject p2;
	double p1HP;
	double p2HP;
	double p1HPprecentage;
	double p2HPprecentage;
	Animator animator;
	
	
	//position of players
	Vector3 p1posn;
	Vector3 p2posn;
	
	
	//position recorder to go back from a melee attack
	Vector3 p1origposn;
	Vector3 p2origposn;
	
	Vector3 selforigposn;
	//determines if the enemy is close enough to a player
	// in order to perform attack or know that it's time to come back
	bool inrange;
	
	
	//knows peers (if other enemy is attacking), in order to wait wisely
	// 3 enemies
	GameObject e1;
	GameObject e2;
	GameObject e3;
	
	//a boolean indicates if the enemy is attacking
	bool attacking;


	
	// Use this for initialization
	void Start () {
		animator = gameObject.GetComponent<Animator> ();
		//get players' health and max health, in order to know the precentage.
		p1 = GameObject.Find("P1");
		p1HP = p1.GetComponent<CharacterState> ().get_health ();
		double p1HPMAX = p1.GetComponent<CharacterState> ().health_max;
		p1HPprecentage = p1HP / p1HPMAX;
		
		p2 = GameObject.Find("P2");
		p2HP = p2.GetComponent<CharacterState> ().get_health ();
		double p2HPMAX = p2.GetComponent<CharacterState> ().health_max;
		p2HPprecentage = p2HP / p2HPMAX;
		
		//find enemies
		e1 = GameObject.Find("E1");
		
		e2 = GameObject.Find("E2");
		
		e3 = GameObject.Find("E3");
		//position of players
		
		p1posn = p1.transform.position;
		p2posn = p2.transform.position;
		
		// record the starting position of the enemy
		
		selforigposn = transform.position;
		
		
		//
		
		state = GetComponent<CharacterState> ();
		state.cooldown_start (Random.Range(3,5));
	}
	/*
	// Update is called once per frame
	void Update () {
		if(!state.on_cooldown_huh() && state.getActive()){
			EnemyFireball f = new EnemyFireball();
			GameObject p1 = GameObject.Find ("P1");
			Message m = new Message(this.gameObject, p1, f);
			GameManager.instance.AddEnemyAction(m);
			state.cooldown_start(Random.Range (3, 5));
		} 
	}*/
	
	
	// Update is called once per frame
	void Update () {
		
		//know if other enemies are attacking, (even itself)
		bool e1at = e1.GetComponent<EnemyAttack> ().attacking;
		
		bool e2at = e2.GetComponent<EnemyAttack> ().attacking; 
		bool e3at = e3.GetComponent<EnemyAttack> ().attacking;

		if ((!e1at && !e2at && !e3at) || attacking) {
			attackcycle (p1);
		}
		if (!attacking) {
			
			getback ();
			animator.SetInteger("Direction",0);
		}
	}
	
	void attackcycle(GameObject player){
		if (!state.on_cooldown_huh () && state.getActive()) {
			attacking = true;
			// Decide(k
			moveto (p1);
			attack += Time.deltaTime;

			
		} else {
			attacking =false;
		}
		
		if (attack > 1.0) {
			state.cooldown_start (Random.Range (7, 10));
			attack = 0.0;
		}
	}
	
	
	//enemy approaches to player, preform melee attack
	//returns new posn/*
	void moveto(GameObject dest){
		Vector3 origposn = transform.position;
		Vector3 destposn = dest.transform.position;
		if (!inrange) {
			transform.position = Vector3.Lerp(origposn, destposn, .05f);

		}
		if (Vector3.Distance(origposn, destposn) < 3) {
			inrange=true;
			animator.SetInteger("Direction", 1);
			StartCoroutine(dest.GetComponent<CharacterState>().takeOtherDamage(dest));
		}

	}
	
	//move the enemy to the starting position
	void getback(){
		Vector3 origposn = transform.position;
		if (inrange) {
			transform.position = Vector3.Lerp(origposn, selforigposn, .05f);
		}
		if ((Mathf.Abs (origposn.x - selforigposn.x) < 1) &&
		    (Mathf.Abs (origposn.y - selforigposn.y) < 1) &&
		    (Mathf.Abs (origposn.z - selforigposn.z) < 1)) {
			inrange=false;
		}
	}
}

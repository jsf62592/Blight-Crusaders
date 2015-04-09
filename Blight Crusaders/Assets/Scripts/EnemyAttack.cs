using UnityEngine;
using System.Collections;

public class EnemyAttack : MonoBehaviour {
	
	CharacterState state;
	double attack = 0.0;
	CharacterState p1State;
	
	//health of player
	GameObject p1;
	double p1HP;
	double p1HPprecentage;
	
	
	//position of player
	Vector3 p1posn;
	
	
	//position recorder to go back from a melee attack
	Vector3 p1origposn;
	
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
		//get players' health and max health, in order to know the precentage.
		p1 = GameObject.Find("P1");
		p1State = p1.GetComponent<CharacterState> ();
		p1HP = p1State.get_health ();
		double p1HPMAX = p1.GetComponent<CharacterState> ().health_max;
		p1HPprecentage = p1HP / p1HPMAX;

		
		//find enemies
		e1 = GameObject.Find("E1");
		e2 = GameObject.Find("E2");
		e3 = GameObject.Find("E3");
		
		//position of player
		p1posn = p1.transform.position;
		
		// record the starting position of the enemy
		selforigposn = transform.position;
		
		state = this.gameObject.GetComponent<CharacterState> ();
		if (this.gameObject.name == "E1") {
			state.cooldown_start(5.0f);
		}
		if (this.gameObject.name == "E2") {
			state.cooldown_start(7.0f);
		}
		if (this.gameObject.name == "E3") {
			state.cooldown_start(12.0f);
		}
	}
	
	
	// Update is called once per frame
	void Update () {
		if ((! state.on_cooldown_huh ()) && state.getActive () && !p1State.isDead() && !state.isDead()) {
			 this.GetComponent<Enemy>().decision(p1);
		}
	}
}
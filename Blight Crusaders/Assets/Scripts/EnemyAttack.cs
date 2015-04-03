using UnityEngine;
using System.Collections;

public class EnemyAttack : MonoBehaviour {
	
	CharacterState state;
	double attack = 0.0;

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
		p1HP = p1.GetComponent<CharacterState> ().get_health ();
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
		
		
		//
		
		state = GetComponent<CharacterState> ();
		state.cooldown_start (Random.Range(3,5));
	}

	/*
	// Update is called once per frame
	void Update () {
		if(!state.on_cooldown_huh() && state.getActive()){
			Ability f = GetComponent<Ability_Basic_Attack>();
			if(f == null){
				throw new UnityException("EnemyAttack: " + this.name +" could not find an ability");
			}
			GameObject p1 = GameObject.Find ("P1");
			f.add_to_queue(p1);
			GameManager.instance.FreezeOtherCharacters(this.gameObject);
			state.setInactive();
		} 
	}
	*/
}

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
	EnemyFireball f;
	
	
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
		f = GetComponent<EnemyFireball>();
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

	// Update is called once per frame
	void Update () {
		if(!state.on_cooldown_huh() && state.getActive() && state.getCanQueue()){
			Debug.Log("HEY ITS ME: " + gameObject.name);
			if(f == null){
				f = this.gameObject.AddComponent<EnemyFireball>();
			}
			GameObject p1 = GameObject.Find ("P1");
			f.add_to_queue(p1);
			state.setCannotQueue();
		} 
	}
}

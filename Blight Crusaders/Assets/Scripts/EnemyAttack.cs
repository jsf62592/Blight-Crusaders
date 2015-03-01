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
			EnemyFireball f = new EnemyFireball();
			GameObject p1 = GameObject.Find ("P1");
			Message m = new Message(this.gameObject, p1, f);
			GameManager.instance.AddEnemyAction(m);
			state.cooldown_start(Random.Range (3, 5));
		} 
	}
}
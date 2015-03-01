using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	bool canEnemyAttack;
	public static GameManager instance;
	Queue<Message> playerAction;
	Queue<Message> enemyAction;

	// Use this for initialization
	void Start () {
		instance = this;
		playerAction = new Queue<Message> ();
		enemyAction = new Queue<Message> ();
		canEnemyAttack = true;
	}
	
	// Update is called once per frame
	void Update () {
		while (playerAction.Count > 0) {
			Message action = playerAction.Dequeue();
			action.UseAbility();
		}

		while (enemyAction.Count > 0) {
			Message action = enemyAction.Dequeue();
			action.UseAbility();
		}
	}

	IEnumerator UseEnemyAction(Message action){
		GameObject selected = action.ReturnSelected ();		
		selected.renderer.material.color = Color.red;
		action.UseAbility ();
		yield return new WaitForSeconds (1f);
		selected.renderer.material.color = Color.white;
	}

	public void AddPlayerAction(Message action){
		playerAction.Enqueue (action);
	}

	public void AddEnemyAction(Message action){
		enemyAction.Enqueue (action);
	}
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

//	bool canEnemyAttack;
	public static GameManager instance;
	Queue<Message> playerAction;
	Queue<Message> enemyAction;
	GameObject[] characters;

	// Use this for initialization
	void Start () {
		characters = new GameObject[5];
		PopulateCharacters (characters);
		instance = this;
		playerAction = new Queue<Message> ();
		enemyAction = new Queue<Message> ();
	}

	void PopulateCharacters(GameObject[] characters){
		characters [0] = GameObject.Find ("P1");
		characters [1] = GameObject.Find ("P2");
		characters [2] = GameObject.Find ("E1");
		characters [3] = GameObject.Find ("E2");
		characters [4] = GameObject.Find ("E3");
	}
	
	// Update is called once per frame
	void Update () {
		while (playerAction.Count > 0) {
			Message action = playerAction.Dequeue();
			StartCoroutine(UsePlayerAction(action));
		}

		while (enemyAction.Count > 0) {
			Message action = enemyAction.Dequeue();
			StartCoroutine(UseEnemyAction(action));
		}
	}

	void FreezeOtherCharacters(GameObject characterAttacking){
		int i = 0;
		while (i < characters.Length) {
			string name = characterAttacking.name;
			if(characters[i].name != name){
				characters[i].GetComponent<CharacterState>().setInactive();
			}
			i++;
		}
	}

	void UnFreezeCharacters(GameObject characterAttacking){
		int i = 0;
		while (i < characters.Length) {
			string name = characterAttacking.gameObject.name;
			if(characters[i].name != name){
				characters[i].GetComponent<CharacterState>().setActive();
			}
			i++;
		}
	}

	IEnumerator UsePlayerAction(Message action){
		GameObject selected = action.ReturnSelected ();
		FreezeOtherCharacters (selected);
		action.UseAbility ();
		yield return new WaitForSeconds (3f);
		UnFreezeCharacters (selected);
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

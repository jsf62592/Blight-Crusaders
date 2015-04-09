using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

//	bool canEnemyAttack;
	public static GameManager instance;
	Queue<Message> QueueAction;
	List<GameObject> characters;
	bool canDequeue = true;
	int enemyDeath = 0;

	// Use this for initialization
	void Start () {
		characters = new List<GameObject> ();
		PopulateCharacters (characters);
		instance = this;
		QueueAction = new Queue<Message> ();
	}

	void PopulateCharacters(List<GameObject> characters){
		characters.Add(GameObject.Find ("P1"));
		characters.Add(GameObject.Find ("E1"));
		characters.Add(GameObject.Find ("E2"));
		characters.Add(GameObject.Find ("E3"));
	}

	public void PopCharacter(GameObject character){
		int i = 0;
		while (i < characters.Count) {
			if(characters[i] == character){
				characters.Remove (characters[i]);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(enemyDeath >= 3){
			Interface.instance.GameOver();
		}

		while (QueueAction.Count > 0) {
			if(canDequeue) {
				Message action = QueueAction.Dequeue();
				GameObject selected = action.ReturnSelected();			
				if(!selected.GetComponent<CharacterState>().isDead()){
					action.UseAbility ();
					FreezeOtherCharacters (selected);
					canDequeue = false;
				}
			}
			else return;
		}
	}

	void DoubleCheck(){
		if(QueueAction.Count >= 2){
			Message action1 = QueueAction.Dequeue();
			Message action2 = QueueAction.Dequeue();
			if(action1.ReturnSelected() == action2.ReturnSelected()){
				QueueAction.Enqueue(action1);
			} else {
				QueueAction.Enqueue(action1);
				QueueAction.Enqueue(action2);
			}
		} else return;
	}

	public void FreezeOtherCharacters(GameObject characterAttacking){
		int i = 0;
		while (i < characters.Count) {
			string name = characterAttacking.name;
			if(characters[i].name != name){
				characters[i].GetComponent<CharacterState>().setInactive();
			}
			i++;
		}
	}

	public void UnFreezeCharacters(){
		int i = 0;
		while (i < characters.Count) {
			characters [i].GetComponent<CharacterState> ().setActive ();
			i++;
		}
		canDequeue = true;
	}

	public void EnemyDeath(){
		enemyDeath++;
	}

	public void AddQueueAction(Message action){
		QueueAction.Enqueue (action);
	}
}

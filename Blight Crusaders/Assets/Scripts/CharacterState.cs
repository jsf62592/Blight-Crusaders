using UnityEngine;
using System.Collections;

public class CharacterState : MonoBehaviour {
	private float time_next_tick = 0;
	private int cooldown = 20;
	private int max_cooldown = 20;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time >= this.time_next_tick) { 
			time_next_tick = Time.time + 1;
			if (cooldown > 0){
			cooldown--;
			}
			print ("Cooldown:  UPDATE  |  time left:  " + this.cooldown + " | on_cd?: " + this.on_cooldown_huh());
		}
	}

	void cooldown_start(){
		this.cooldown = this.max_cooldown;
	}

	bool on_cooldown_huh(){
		return this.cooldown > 0;
	}
}

using UnityEngine;
using System.Collections;
using System;

public class EnemyAttack : MonoBehaviour {

	//double colddown = GenRand(0.0, 2.5);


	//Enemies attack randomly so far, 
	static System.Random rand = new System.Random();
	double colddown= 0.0 + rand.NextDouble() * (2.5);

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
				colddown += Time.deltaTime;
				//bool cd = GameObject.GetComponent<CharacterState>().on_cooldown_huh;

				if (colddown > 2.5 && colddown < 4.5) {
						gameObject.renderer.material.color = Color.red;
				} else if (colddown > 5.0 && colddown < 6.5) {
			blink();
				} else if (colddown > 6.5) {

						//attack here

						colddown = 0.0;
			
						gameObject.renderer.material.color = Color.white;
				}
		}

	
	void blink(){
				if (((int)(colddown*10)) % 2 == 0) {
			
						gameObject.renderer.material.color = Color.red;
			
				} else {
						gameObject.renderer.material.color = Color.white;
				}
		}


}

using UnityEngine;
using System.Collections;

public class EnemyAttack : MonoBehaviour {

	double colddown = 0.0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
				colddown += Time.deltaTime;
				//bool cd = GameObject.GetComponent<CharacterState>().on_cooldown_huh;

				if (colddown > 3.0 && colddown < 5.0) {
						gameObject.renderer.material.color = Color.red;
				} else if (colddown > 5.0) {

						//attack here

						colddown = 0.0;
			
						gameObject.renderer.material.color = Color.white;
				}
		}
}

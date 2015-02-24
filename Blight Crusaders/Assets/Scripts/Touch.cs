using UnityEngine;
using System.Collections;

public class Touch : MonoBehaviour {

	double cooldown = 0.0; //temporary cooldown variable for initial touch story color change

	// Use this for initialization
	void Start () {
	
	}

	void Update(){
<<<<<<< HEAD
		if (cooldown > 0.0){ 
=======
		if (cooldown > 0){ 
>>>>>>> production
			Debug.Log(name + "cooldown: " + cooldown); 
			cooldown -= Time.deltaTime;
		}else{
			gameObject.renderer.material.color = Color.white;
		}
	}

	public void TouchResponse(){
		Debug.Log(name);
<<<<<<< HEAD
		if (tag == "Player" && cooldown <= 0.0) {
			
			Debug.Log ("touch.cs touched");
		//if(true){
=======
		if (tag == "Player" && cooldown <= 0) {
>>>>>>> production
			gameObject.renderer.material.color = Color.blue;
			cooldown = 3.0;
		}
	}
<<<<<<< HEAD

=======
>>>>>>> production
}

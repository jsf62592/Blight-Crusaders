using UnityEngine;
using System.Collections;
using System;

//ATTACH THIS SCRIPT TO CAMERA
//Other Assumptions
//-PCs are tagged as "Player" and enemies are tagged as "Enemy", and both have colliders
//-PlayerAction script attached to player characters and has "Attack(GameObject)"
//-(From James)CharacterState script attached, player can only attack when on_cooldown_huh()   
public class Interface : MonoBehaviour {
	
	public GameObject selected;
	public GameObject targeted;
	CharacterState state;
	
	// Use this for initialization
	void Start () {
		selected = null;
		targeted = null;
		
	}
	
	RuntimePlatform platform = Application.platform;
	
	void Update(){
		//If this is on mobile, detect touch input
		if (platform == RuntimePlatform.Android || platform == RuntimePlatform.IPhonePlayer || platform == RuntimePlatform.WindowsPlayer ) {
			if (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Began) {
				RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
				
				//If an object is touched? FOR THE FUTURE: do nothing if the master object says an enemy is attacking
				if (hit != null) {
					Debug.Log (hit.collider.name + " touched");
					 state = hit.collider.GetComponent<CharacterState>();
					//if that object is a player off cooldown, they are selected
					if (hit.collider.tag == "Player" && !state.on_cooldown_huh () && state.getActive()) {
						selected = hit.collider.gameObject;
						Debug.Log (selected + " is selected");
						selected.GetComponent<PlayerAction> ().Select ();
					}
					
				}
			}
			//if touch on a player and is released over another character, attack
			if (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Ended && state.getActive()) {
				RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint (Input.mousePosition), Vector2.zero);

				
				//If an object is touched? 
				if (hit != null) {
					targeted = hit.collider.gameObject;
					Debug.Log (hit.collider.name + " targeted");
					
					if (targeted != selected) {
						selected.GetComponent<PlayerAction> ().Attack (targeted);
					}
					
					selected.GetComponent<PlayerAction> ().DeSelect ();
					ResetInput();
					
				} else {
					selected.GetComponent<PlayerAction> ().DeSelect ();
					ResetInput();
				}
			}
			
			//if this is in Unity detect mouse instead of touch
		}else if(platform == RuntimePlatform.WindowsEditor) {
			if (Input.GetMouseButtonDown (0)) {
				RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint (new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10)), Vector2.zero);
				
				//if an object is clicked FOR THE FUTURE: do nothing if the master object says an enemy is attacking
				if (hit.collider != null) {
					Debug.Log (hit.collider.name + " touched");
					state = hit.collider.GetComponent<CharacterState>();
					//if that object is a player off cooldown, they are selected
					if (hit.collider.tag == "Player" && !state.on_cooldown_huh() && state.getActive()) {
						selected = hit.collider.gameObject;
						Debug.Log (selected + " is selected");
						selected.GetComponent<PlayerAction>().Select();
					}
				}
			}
			//if mouse was clicked on a player and is released over another character, attack
			if (Input.GetMouseButtonUp (0) && selected!= null) {
					RaycastHit2D hit2 = Physics2D.Raycast(Camera.main.ScreenToWorldPoint (new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10)), Vector2.zero);
				
				if (hit2.collider != null) {
					targeted = hit2.collider.gameObject;
					state = hit2.collider.GetComponent<CharacterState>();
					if(targeted != selected){
						Debug.Log("HGERE");
						Fireball f = new Fireball();
						Message m = new Message(selected, targeted, f);
						GameManager.instance.AddPlayerAction(m);
             		}
					
					selected.GetComponent<PlayerAction>().DeSelect();
					ResetInput();
					
				}else{
					selected.GetComponent<PlayerAction>().DeSelect();
					ResetInput();
				}
			}
		}
	}
	
	
	
	public void ResetInput(){
		selected = null;
		targeted = null;
		Debug.Log("input reset");
	}
}

/*
		 * //if nothing already selected
						if(selected == null && !hit.collider.gameObject.GetComponent<CharacterState>().on_cooldown_huh()){
							selected = hit.collider.gameObject;
							Debug.Log(selected.name + " now selected");

							//For the future, use show when character is selected 
							//selected.GetComponent<PlayerAction>().Select();
						
						//if something else selected, nothing targeted
						}else if(selected != null && targeted == null){
							targeted = hit.collider.gameObject;
							Debug.Log(targeted.name + " now targeted");

							//Attack the target object
							selected.GetComponent<PlayerAction>().Attack(targeted);

							ResetInput();
						
						//if something else selected AND targeted
						}else if(selected != null && targeted != null){
							Debug.Log(selected.name + " selected and " + targeted.name + " targeted already");
						}
					}else if(hit.collider.tag == "Enemy"){
						
						//if nothing already selected
						if(selected == null){
							Debug.Log("nothing selected to target this");

						//if something selected, nothing targeted
						}else if(selected != null && targeted == null){
							targeted = hit.collider.gameObject;
							Debug.Log(targeted.name + " now targeted");
							
							//Attack the target object
							selected.GetComponent<PlayerAction>().Attack(targeted);

							ResetInput();
							
						//if something else selected AND targeted
						}else if(selected != null && targeted != null){
							Debug.Log(selected.name + " selected and " + targeted.name + " targeted already");
						}
					}
				}else{
					ResetInput();
				}
*/
using UnityEngine;
using System.Collections;

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
		state = GetComponent<CharacterState> ();
		
	}
	
	RuntimePlatform platform = Application.platform;
	
	void Update(){
		
		//If this is on mobile, detect touch input
		if (platform == RuntimePlatform.Android || platform == RuntimePlatform.IPhonePlayer) {
			if (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Began) {
				Ray ray = Camera.main.ScreenPointToRay (Input.GetTouch (0).position);
				RaycastHit hit;
				
				//If an object is touched? FOR THE FUTURE: do nothing if the master object says an enemy is attacking
				if (Physics.Raycast (ray, out hit)) {
					Debug.Log (hit.collider.name + " touched");
					
					//if that object is a player off cooldown, they are selected
					if (hit.collider.tag == "Player" && !state.on_cooldown_huh ()) {
						selected = hit.collider.gameObject;
						Debug.Log (selected + " is selected");
						selected.GetComponent<PlayerAction> ().Select ();
					}
					
				}
			}
			//if touch on a player and is released over another character, attack
			if (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Ended) {
				Ray ray = Camera.main.ScreenPointToRay (Input.GetTouch (0).position);
				RaycastHit hit;
				
				//If an object is touched? 
				if (Physics.Raycast (ray, out hit)) {
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
				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
				RaycastHit hit;
				
				//if an object is clicked FOR THE FUTURE: do nothing if the master object says an enemy is attacking
				if (Physics.Raycast (ray, out hit)) {
					Debug.Log (hit.collider.name + " touched");
					
					//if that object is a player off cooldown, they are selected
					if (hit.collider.tag == "Player" && !hit.collider.gameObject.GetComponent<CharacterState>().on_cooldown_huh()) {
						selected = hit.collider.gameObject;
						Debug.Log (selected + " is selected");
						selected.GetComponent<PlayerAction>().Select();
					}
				}
			}
			//if mouse was clicked on a player and is released over another character, attack
			if (Input.GetMouseButtonUp (0) && selected!= null) {
				Ray ray2 = Camera.main.ScreenPointToRay (Input.mousePosition);
				RaycastHit hit2;
				
				if (Physics.Raycast (ray2, out hit2)) {
					targeted = hit2.collider.gameObject;
					Debug.Log (hit2.collider.name + " targeted");
					
					if(targeted != selected){ selected.GetComponent<PlayerAction>().Attack (targeted); }
					
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
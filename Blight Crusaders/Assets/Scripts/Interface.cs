using UnityEngine;
using System.Collections;
using System;

//ATTACH THIS SCRIPT TO CAMERA 
public class Interface : MonoBehaviour {
	
	public GameObject selected; //the player character that takes input
	public GameObject target; //the targeted enemy
	public Boolean targeted; //is there a taget
	public Boolean targeting; //are we looking for a target
	CharacterState state; //the state of the selected player character
	Boolean drawButtons = false; //draw the ability input buttons	

	//declare these somewhere based on ability button prefabs
	public Texture ability1Texture;
	public Texture ability2Texture;
	public Texture ability3Texture;

	public int touchX; //current input touch position
	public int touchY; 
	public int button = 0; //button that was hit
	
	public Boolean touch; //are we taking touch input instead of mouse
	public Boolean turn; //is it the player's turn
	public Vector3 retouch; //touch position fixed for scene positions
	

	// Use this for initialization
	void Start () {
		selected = GameObject.Find("P1");
		state = selected.GetComponent<CharacterState> ();
		target = null;
		targeted = false; 
		turn = false;
	}

	//find out what platform is running the code
	RuntimePlatform platform = Application.platform;
	
	void Update(){
		//Pop player input UI
		if (!state.on_cooldown_huh () && state.getActive ()) {
			retouch = camera.WorldToScreenPoint (selected.transform.position);
			GameManager.instance.FreezeOtherCharacters (selected);
			turn = true;
		}
		
		//If on a touch platform, detect touch instead
		if (platform == RuntimePlatform.Android || platform == RuntimePlatform.IPhonePlayer || platform == RuntimePlatform.WindowsPlayer) {
			touch = true;
		} else {
			touch = false;
		}

		//if you click an enemy during targeting
		if ((touch && Input.touchCount > 0) || (!touch && Input.GetMouseButtonDown (0))) {
			RaycastHit2D hit;

			//raycast based on mouse position
			if (touch) {
				hit = Physics2D.Raycast (Camera.main.ScreenToWorldPoint (Input.mousePosition), Vector2.zero); 
			} else {
				hit = Physics2D.Raycast (Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, 10)), Vector2.zero);
			}

			//if an object is clicked
			if (hit != null && hit.collider != null && hit.collider.tag == "Enemy" && targeting) {
				target = hit.collider.gameObject;
				targeted = true;
			}
		}

	
		//if we have a target enemy
		if(targeted){ 

			//Which ability
			if(button == 1){ 
				Debug.Log ("ABLITY1 USED"); 
				Fireball();
			}else if(button == 2){ 
				Debug.Log ("ABLITY2 USED");
				Fireball();
			}else if(button == 3){ 
				Debug.Log ("ABLITY3 USED"); 
				Fireball();
			}

		ResetInput();
		}

		if (button != 0) { targeting = true;} //Once an ability is selected, next input should be a target enemy
		if(!targeting && !targeted && turn){ drawButtons = true; }
		if (!turn) { drawButtons = false; }
	}

		
	public void OnGUI(){

		//Draw the buttons for abilities id the player is selected
		if (drawButtons) {
			Rect button3 = new Rect(retouch.x - 25 +25, Screen.height - retouch.y +50 -10,50,50);
			Rect button2 = new Rect(retouch.x + 25, Screen.height - retouch.y -10,50,50);
			Rect button1 = new Rect(retouch.x -25 + 25, Screen.height - retouch.y -50 -10,50,50);

			GUI.DrawTexture(button1,  ability1Texture);
			GUI.DrawTexture(button2,  ability2Texture);
			GUI.DrawTexture(button3,  ability3Texture);
			Event e = Event.current;
			if (e.type == EventType.MouseDown) {
				if (button1.Contains(e.mousePosition)) {
					Debug.Log ("Button1 hit");
					button = 1;
					drawButtons = false;
				}

				if (button2.Contains(e.mousePosition)) {
					Debug.Log ("Button2 hit");
					button = 2;
					drawButtons = false;
				}

				if (button3.Contains(e.mousePosition)) {
					Debug.Log ("Button3 hit");
					button = 3;
					drawButtons = false;
				}
			}
		}
	}

	//Reset after player attack
	public void ResetInput(){
		state.setInactive();
		selected.GetComponent<PlayerAction>().DeSelect();
		GameManager.instance.UnFreezeCharacters();
		button = 0;
		drawButtons = false;
		targeting = false;
		targeted = false;
		target = null;
		turn = false;
		Debug.Log("input reset");
	}

	//Fireball ability
	public void Fireball(){
		Fireball f = selected.GetComponent<Fireball>();
		if(f == null){
			f = selected.gameObject.AddComponent<Fireball>();
		}
		f.add_to_queue(target);
	}

}

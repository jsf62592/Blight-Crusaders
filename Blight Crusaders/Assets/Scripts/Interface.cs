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
	public GameObject abilButton1;
	public GameObject abilbutton2;
	public GameObject abilButton3;

	public int touchX; //current input touch position
	public int touchY; 
	public int button = 0; //button that was hit

	//positions of the buttons
	public float abilButton1x;
	public float abilButton1y;
	public float abilButton2x;
	public float abilButton2y;
	public float abilButton3x;
	public float abilButton3y;

	public Boolean touch; //are we taking touch input instead of mouse
	public Vector3 retouch; //touch position fixed for scene positions
	

	// Use this for initialization
	void Start () {
		selected = GameObject.Find("P1");
		state = selected.GetComponent<CharacterState> ();
		target = null;
		targeted = false; 

		//FOR FUTURE instantiate abilitiy buttons and hide them
	}

	//find out what platform is running the code
	RuntimePlatform platform = Application.platform;
	
	void Update(){
		//Pop player input UI
		if (!state.on_cooldown_huh () && state.getActive ()) {
			retouch = camera.WorldToScreenPoint (selected.transform.position);
			drawButtons = true;
			GameManager.instance.FreezeOtherCharacters (selected);
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

			//figure out which ability got clicked, if any
			//FOR FUTURE detect touches on button objects
			if (drawButtons) {
				button = 0;
				Vector2 mousePlace = retouch;
				Debug.Log ("mouse x: " + Input.mousePosition.x + " mousey: " + Input.mousePosition.y);
				if (Vector2.Distance (mousePlace, new Vector2 (abilButton1x, abilButton1y)) <= 25) { 
					Debug.Log ("button1 hit");
					button = 1;
					drawButtons = false;
				} else if (Vector2.Distance (mousePlace, new Vector2 (abilButton2x, abilButton2y)) <= 25) { 
					Debug.Log ("button2 hit");
					button = 2;
					drawButtons = false;
				} else if (Vector2.Distance (mousePlace, new Vector2 (abilButton3x, abilButton3y)) <= 25) { 
					Debug.Log ("button3 hit");
					button = 3;
					drawButtons = false;
				} else {
					Debug.Log ("buttons missed");
				}
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
			}else if(button == 3){ 
				Debug.Log ("ABLITY3 USED"); 
			}

		ResetInput();
		}

		if (button != 0) { targeting = true;} //Once an ability is selected, next input should be a target enemy
	}

		
			
	
	public void OnGUI(){

		//Draw the buttons for abilities id the player is selected
		//FOR FUTURE simple show button objects
		if (drawButtons) {
			//GUI.DrawTexture(new Rect(retouch.x - 25, Screen.height - retouch.y +50,50,50), abilButton1, ScaleMode.StretchToFill);
			abilButton1x = retouch.x;       //-25
			abilButton1y = retouch.y +25;  //-25
			//GUI.DrawTexture(new Rect(retouch.x + 25, Screen.height - retouch.y,50,50), abilButton1, ScaleMode.StretchToFill);
			abilButton2x = retouch.x + 50;
			abilButton2y = retouch.y -25;
			//GUI.DrawTexture(new Rect(retouch.x -25, Screen.height - retouch.y -50,50,50), abilButton1, ScaleMode.StretchToFill);
			abilButton3x = retouch.x;
			abilButton3y = retouch.y - 75;
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

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
	Boolean drawButtons = false;	
	
	public Vector3 targetScreenPosition; 
	public Vector2 target1; //top right
	public Vector2 target2; //bottom right
	public Vector2 target3; //bottom left
	public Vector2 target4; //top left
	public float targetOffset;
	public float gestureTargetRadius;
	public int[] LastFourTargetHits; //which target was hit 1-4
	
	public Boolean draw;
	public Texture circleImage;
	public Texture xImage;

	public Texture abilButton1;
	public Texture abilbutton2;
	public Texture abilButton3;
	
	public Vector2 mousePos;
	public String gesture;
	public Boolean drawImage;
	public int drawTime;
	public int drawTimer;

	public int touchX;
	public int touchY;
	public int button = 0;
	public Boolean picking = false;
	public float abilButton1x;
	public float abilButton1y;
	public float abilButton2x;
	public float abilButton2y;
	public float abilButton3x;
	public float abilButton3y;


	
	
	
	// Use this for initialization
	void Start () {
		selected = null;
		targeted = null;
		circleImage = Resources.Load("circleImage") as Texture;
		xImage = Resources.Load("xImage") as Texture;
		targetOffset = 30; 
		gestureTargetRadius = 20;
		LastFourTargetHits = new int[4];
		drawImage = false;
		drawTime = 20;
		drawTimer = drawTime;

	}
	
	RuntimePlatform platform = Application.platform;
	
	void Update(){
		//If this is on mobile, detect touch input
		if (platform == RuntimePlatform.Android || platform == RuntimePlatform.IPhonePlayer || platform == RuntimePlatform.WindowsPlayer ) {
			if (Input.touchCount > 0) {
				RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
				
				//If an object is touched? FOR THE FUTURE: do nothing if the master object says an enemy is attacking
				if (hit != null) {
					state = hit.collider.GetComponent<CharacterState>();
					//if that object is a player off cooldown, they are selected
					if (hit.collider.tag == "Player" && !state.on_cooldown_huh () && state.getActive()) {
						selected = hit.collider.gameObject;
						selected.GetComponent<PlayerAction> ().Select ();
						abilButton1 = selected.GetComponent<PlayerAction> ().GetButton1();
					    abilbutton2 = selected.GetComponent<PlayerAction> ().GetButton2();
						abilButton3 = selected.GetComponent<PlayerAction> ().GetButton3();
						touchX = (int) Input.mousePosition.x;
						touchY = (int) Input.mousePosition.y;
						drawButtons = true;
						GameManager.instance.FreezeOtherCharacters(selected);
					}
				} else {
					selected.GetComponent<PlayerAction>().DeSelect();
					ResetInput();
					drawButtons = false;
				}
			}
			//if attack input is is progress (a player was clicked)
			if (selected!= null && targeted == null) {
				RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint (Input.mousePosition), Vector2.zero);
				
				if (hit.collider != null && hit.collider.gameObject != selected) {
					targeted = hit.collider.gameObject;
					state = hit.collider.GetComponent<CharacterState>();
				}
			}
			
			if(targeted != null){ //if we have a target
				targetScreenPosition = Camera.main.WorldToScreenPoint(targeted.transform.position);//the onscreen position of the target
				//setTargets(); //set the values for the targets based on the screen position
				//recordTargetHits(Input.mousePosition.x, Input.mousePosition.y); //record the last targets hit
			}
	
			//reset input if mouse is up
			if(Input.GetTouch (0).phase == TouchPhase.Ended){
				//checkTargetHits(); //check if they form an X or O
				
				if(targeted != null){
					float delta = Input.mousePosition.y - targetScreenPosition.y;
					if(delta >= 100){ upInput(); }
					if(delta <= -100){ downInput(); }
				}
				selected.GetComponent<PlayerAction>().DeSelect ();
				ResetInput();
			}
			
			//if this is in Unity detect mouse instead of touch
		}else if(platform == RuntimePlatform.WindowsEditor) {

			if (Input.GetMouseButtonDown (0)) {
				RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint (new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10)), Vector2.zero);
				
				//if an object is clicked FOR THE FUTURE: do nothing if the master object says an enemy is attacking
				if (hit.collider != null) {
					state = hit.collider.GetComponent<CharacterState>();
					//if that object is a player off cooldown, they are selected
					if (hit.collider.tag == "Player" && !state.on_cooldown_huh() && state.getActive()) {
						selected = hit.collider.gameObject;
						selected.GetComponent<PlayerAction> ().Select ();
						abilButton1 = selected.GetComponent<PlayerAction> ().GetButton1();
						abilbutton2 = selected.GetComponent<PlayerAction> ().GetButton2();
						abilButton3 = selected.GetComponent<PlayerAction> ().GetButton3();
						touchX = (int) Input.mousePosition.x;
						touchY = (int) Input.mousePosition.y;
						drawButtons = true;
						GameManager.instance.FreezeOtherCharacters(selected);

					}
					else if (hit.collider.tag == "Enemy" && !picking){
						targeted = hit.collider.gameObject;
					}

				

				} else if(!picking) {
					selected.GetComponent<PlayerAction>().DeSelect();
					ResetInput();
					drawButtons = false;
				}


				if (picking){
					//figure out which ability got clicked or reset input
					button = 0;
					Vector2 mousePlace = new Vector2(Input.mousePosition.x,Input.mousePosition.y);
					Debug.Log("picking");
					if(Vector2.Distance(mousePlace,new Vector2(abilButton1x,abilButton1y)) <= 25){ 
						Debug.Log("button1 hit");
						button = 1;
						picking = false;
						drawButtons = false;
					}else if(Vector2.Distance(mousePlace,new Vector2(abilButton2x,abilButton2y)) <= 25){ 
							Debug.Log("button2 hit");
							button = 2;
							picking = false;
							drawButtons = false;
					}else if(Vector2.Distance(mousePlace,new Vector2(abilButton3x,abilButton3y)) <= 25){ 
						Debug.Log("button3 hit");
						button = 3;
						picking = false;
						drawButtons = false;
					}else{
						Debug.Log("button1 missed mousex: " + Input.mousePosition.x + " mousey: " + Input.mousePosition.y);
						ResetInput();
					}
				}

				if(selected != null && targeted == null && button == 0){
					Debug.Log("picking true");
					picking = true;
				}

			}/*
			//if attack input is is progress (a player was clicked)
			if (selected!= null && targeted == null) {
				RaycastHit2D hit2 = Physics2D.Raycast(Camera.main.ScreenToWorldPoint (new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10)), Vector2.zero);
				
				if (hit2.collider != null && hit2.collider.gameObject != selected) {
					targeted = hit2.collider.gameObject;
					state = hit2.collider.GetComponent<CharacterState>();
				}
			}*/





			if(targeted != null){ //if we have a target
				if(button == 1){ Debug.Log ("ABLITY1 USED"); }
				else if(button == 2){ Debug.Log ("ABLITY2 USED"); }
				else if(button == 3){ Debug.Log ("ABLITY3 USED"); }
				targetScreenPosition = Camera.main.WorldToScreenPoint(targeted.transform.position);
				Fireball f = selected.GetComponent<Fireball>();
				if(f == null){
					f = selected.gameObject.AddComponent<Fireball>();
				}
				f.add_to_queue(targeted);
				state.setInactive();
				selected.GetComponent<PlayerAction>().DeSelect();
				ResetInput();//the onscreen position of the target
				//setTargets(); //set the values for the targets based on the screen position
				//recordTargetHits(Input.mousePosition.x, Input.mousePosition.y); //record the last targets hit
			}

			
			/*
			//reset input if mouse is up
			if(Input.GetMouseButtonUp(0)){
				//checkTargetHits(); //check if they form an X or O
				
				if(targeted != null){
					float delta = Input.mousePosition.y - targetScreenPosition.y;
					if(delta >= 100){ upInput(); }
					if(delta <= -100){ downInput(); }
				}
				selected.GetComponent<PlayerAction>().DeSelect ();
				ResetInput();
			}*/
			
			
		}
	}
	
	public void OnGUI(){
		if (drawImage) {
			drawTimer--;
			if(gesture == "o"){
				//GUI.DrawTexture(new Rect(), circleImage);
				//GUI.DrawTexture(new Rect(10,10,60,60),circleImage);
			}
			if(gesture == "x"){
				//GUI.DrawTexture(new Rect(),xImage);
				//GUI.DrawTexture(new Rect(10,10,60,60),xImage);
			}
		}
		
		if(drawTimer == 0){
			drawImage = false;
			drawTimer = drawTime;
		}

		if (drawButtons) {

			
			GUI.DrawTexture(new Rect(touchX-25+50,Screen.height - touchY,50,50), abilButton1, ScaleMode.StretchToFill);
			GUI.DrawTexture(new Rect(touchX-25,Screen.height -touchY+50,50,50), abilButton1, ScaleMode.StretchToFill);
			GUI.DrawTexture(new Rect(touchX-25-50,Screen.height - touchY,50,50), abilButton1, ScaleMode.StretchToFill);
			abilButton1x = touchX+50;
			abilButton1y = touchY;
			abilButton2x = touchX;
			abilButton2y = touchY - 75;
			abilButton3x = touchX-50;
			abilButton3y = touchY;
			//abilButton2x;
			//abilButton2y;
			//abilButton3x;
			//abilButton3y;

		}
	
		
	}
	
	
	public void setTargets(){
		target1 = new Vector2 (targetScreenPosition.x + targetOffset, targetScreenPosition.y + targetOffset);
		target2 = new Vector2 (targetScreenPosition.x + targetOffset, targetScreenPosition.y - targetOffset);
		target3 = new Vector2 (targetScreenPosition.x - targetOffset, targetScreenPosition.y - targetOffset);
		target4 = new Vector2 (targetScreenPosition.x - targetOffset, targetScreenPosition.y + targetOffset);
	}
	
	
	//if the target
	public void recordTargetHits(float X, float Y){
		mousePos = new Vector2(X, Y);
		if(!(Math.Abs(Vector2.Distance(targetScreenPosition,mousePos)) <= 5)){
			if (Math.Abs(Vector2.Distance(target1,mousePos)) <= gestureTargetRadius) { targetArrayAdd(1); }
			if (Math.Abs(Vector2.Distance(target2,mousePos)) <= gestureTargetRadius) { targetArrayAdd(2); }
			if (Math.Abs(Vector2.Distance(target3,mousePos)) <= gestureTargetRadius) { targetArrayAdd(3); }
			if (Math.Abs(Vector2.Distance(target4,mousePos)) <= gestureTargetRadius) { targetArrayAdd(4); }
		}
	}
	
	public void checkTargetHits(){
		if (LastFourTargetHits [0] == 1 && LastFourTargetHits [1] == 2 && LastFourTargetHits [2] == 3 && LastFourTargetHits [3] == 4 ||
		    LastFourTargetHits [0] == 1 && LastFourTargetHits [1] == 4 && LastFourTargetHits [2] == 3 && LastFourTargetHits [3] == 2 ||
		    LastFourTargetHits [0] == 2 && LastFourTargetHits [1] == 3 && LastFourTargetHits [2] == 4 && LastFourTargetHits [3] == 1 ||
		    LastFourTargetHits [0] == 2 && LastFourTargetHits [1] == 1 && LastFourTargetHits [2] == 4 && LastFourTargetHits [3] == 3 ||
		    LastFourTargetHits [0] == 3 && LastFourTargetHits [1] == 4 && LastFourTargetHits [2] == 1 && LastFourTargetHits [3] == 2 ||
		    LastFourTargetHits [0] == 3 && LastFourTargetHits [1] == 2 && LastFourTargetHits [2] == 1 && LastFourTargetHits [3] == 4 ||
		    LastFourTargetHits [0] == 4 && LastFourTargetHits [1] == 1 && LastFourTargetHits [2] == 2 && LastFourTargetHits [3] == 3 ||
		    LastFourTargetHits [0] == 4 && LastFourTargetHits [1] == 3 && LastFourTargetHits [2] == 2 && LastFourTargetHits [3] == 1) {
			Debug.Log ("CIRCLE");
			circleInput();
		} else if (LastFourTargetHits [0] == 1 && LastFourTargetHits [1] == 3 && LastFourTargetHits [2] == 4 && LastFourTargetHits [3] == 2 ||
		           LastFourTargetHits [0] == 1 && LastFourTargetHits [1] == 3 && LastFourTargetHits [2] == 2 && LastFourTargetHits [3] == 4 ||
		           LastFourTargetHits [0] == 2 && LastFourTargetHits [1] == 4 && LastFourTargetHits [2] == 3 && LastFourTargetHits [3] == 1 ||
		           LastFourTargetHits [0] == 2 && LastFourTargetHits [1] == 4 && LastFourTargetHits [2] == 1 && LastFourTargetHits [3] == 3 ||
		           LastFourTargetHits [0] == 3 && LastFourTargetHits [1] == 1 && LastFourTargetHits [2] == 4 && LastFourTargetHits [3] == 2 ||
		           LastFourTargetHits [0] == 3 && LastFourTargetHits [1] == 1 && LastFourTargetHits [2] == 2 && LastFourTargetHits [3] == 4 ||
		           LastFourTargetHits [0] == 4 && LastFourTargetHits [1] == 2 && LastFourTargetHits [2] == 3 && LastFourTargetHits [3] == 1 ||
		           LastFourTargetHits [0] == 4 && LastFourTargetHits [1] == 2 && LastFourTargetHits [2] == 1 && LastFourTargetHits [3] == 3) {
			Debug.Log ("X (CROSS)");
			xInput();
		} else {
			Debug.Log ("NO SHAPE");
		}
	}
	
	public void targetArrayAdd(int i){
		//Place the *new* int in the last empty bucket or move them all down
		if (LastFourTargetHits [0] == null) { LastFourTargetHits[0] = i; }
		else if (LastFourTargetHits [1] == null && LastFourTargetHits[0] != i) { LastFourTargetHits[1] = i; }
		else if (LastFourTargetHits [2] == null && LastFourTargetHits[1] != i) { LastFourTargetHits[2] = i; }
		else if (LastFourTargetHits [3] == null && LastFourTargetHits[2] != i) { LastFourTargetHits[3] = i; }
		else if (LastFourTargetHits[3] != i){ 
			LastFourTargetHits[0] = LastFourTargetHits[1];
			LastFourTargetHits[1] = LastFourTargetHits[2];
			LastFourTargetHits[2] = LastFourTargetHits[3];
			LastFourTargetHits[3] = i; 
		}
	}
	
	
	public void ResetInput(){
		Debug.Log(LastFourTargetHits.ToString());
		selected = null;
		targeted = null;
		LastFourTargetHits = new int[4];
		Debug.Log("input reset");
		GameManager.instance.UnFreezeCharacters();
		button = 0;
		picking = false;
		drawButtons = false;
	}
	
	public void circleInput(){
		gesture = "o";
		drawImage = true;
		
	}
	
	public void xInput(){
		gesture = "x";
		drawImage = true;
		
	}
	
	public void upInput(){
		Debug.Log ("UP");
		gesture = "o";
		drawImage = true;
		Fireball f = selected.GetComponent<Fireball>();
		if(f == null){
			f = selected.gameObject.AddComponent<Fireball>();
		}
		f.add_to_queue(targeted);
		state.setInactive();

	}
	
	public void downInput(){
		Debug.Log ("DOWN");
		gesture = "x";
		drawImage = true;
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
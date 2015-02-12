using UnityEngine;
using System.Collections;

public class CharacterState : MonoBehaviour {

	//the total length of the screen.  It gets this from outside this scrip.
	private float screen_length;

	private float time_next_tick;
	private int cooldown;

	public int health_max;
	public int health_current;
	public float health_percent;

	//how far it should be from the edge of the screen
	public float distance;
	//initial offset from the center of the screen.  cannot be 0, as this is used to determine which way this should move when damaged
	public float distance_initial_offset;

	// Use this for initialization
	void Start () {
		this.time_next_tick = 0;
		this.cooldown = 0;

		this.health_current = this.health_max;
		this.health_percent = (float)this.health_current / (float)this.health_max;


		this.screen_length = 20; print ("!!!ALERT!!!:  CharacterState.cs USING DUMMY   (/0 ^0)/ ^ I_____I");

		this.distance = ((this.screen_length / 2) - Mathf.Abs(distance_initial_offset)) * this.health_percent;


		//if the initial offset is wrong, complain and blow up
		if (distance_initial_offset == 0){
			throw new UnityException("CharacterState.cs exception:  distance_initial_offset is set to 0 on entity: " + this.name);
		}
		if (Mathf.Abs(distance_initial_offset) >= (this.screen_length / 2)){
			throw new UnityException("CharacterState.cs exception:  distance_initial_offset >= (screen_length / 2)");
		}

		move_according_to_health ();



		print ("CharacterState Debug button enabled");
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time >= this.time_next_tick) { 
			time_next_tick = Time.time + 1;
			if (cooldown > 0){
			cooldown--;
			}
			//print ("Cooldown:  UPDATE  |  time left:  " + this.cooldown + " | on_cd?: " + this.on_cooldown_huh());
		}

		if(Input.GetKeyDown("f")){
			print ("CharacterState Debug button pressed");
			this.take_damage(10);
		}
	}

	public void cooldown_start(int given_cooldown){
		this.cooldown = given_cooldown;
	}

	public bool on_cooldown_huh(){
		return this.cooldown > 0;
	}

	public void take_damage(int given_damage){
		this.health_current = this.health_current - given_damage;
		if(health_current <= 0){
			death();
		}
		this.health_percent = (float)this.health_current / (float)this.health_max;
		this.distance = ((this.screen_length / 2) - Mathf.Abs(distance_initial_offset)) * this.health_percent;
		move_according_to_health();
	}

	private void move_according_to_health(){
		float new_x_position;
		//if it's on the left side
		if(distance_initial_offset < 0){
			new_x_position = distance;
		}
		//if it's on the right side
		else{
			new_x_position = screen_length - distance;
		}
		print (new_x_position);
		this.transform.position = new Vector3 (new_x_position, this.transform.position.y, this.transform.position.z);
	}

	private void death(){
		Destroy(this.gameObject);
	}
}

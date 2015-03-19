using UnityEngine;
using System.Collections;

//this is for debugging abilities.  attach it to an object, set the "target" field, and hit "f".
//all abilities on the attached object will be used on "target"
public class debug_force_abilities : MonoBehaviour {
	//target the ability does stuff to
	public GameObject target;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown("f")){
			Debug.LogWarning("debug_force_abilities script forced all abilities!!!");
			foreach(Ability attached_ability in GetComponents<Ability>()){
				attached_ability.do_stuff(target);
			}
		}
	}
}

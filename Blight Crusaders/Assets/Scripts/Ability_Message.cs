using UnityEngine;
using System.Collections;

public class Ability_Message : MonoBehaviour {
	//the ability that made it
	private Ability creator;

	//reference to target
	private GameObject ref_to_target;
	

	//NOTE:  if you don't know what "ref" is, look at:
	//http://answers.unity3d.com/questions/64105/c-unity-gameobject-pointers.html
	public Ability_Message (Ability given_creator, ref GameObject given_target) {
		creator = given_creator;
		ref_to_target = given_target;

		//complain if it gets null
		if ((creator == null) || (ref_to_target == null)){
			throw new UnityException("Ability_Message given null");
		}
	}

	

	public Ability get_creator(){
		return creator;
	}

	public GameObject get_target(){
		return ref_to_target;
	}
	
}

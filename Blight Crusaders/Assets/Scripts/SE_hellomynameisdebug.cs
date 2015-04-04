using UnityEngine;
using System.Collections;

public class SE_hellomynameisdebug : Status_Effect {
	private string oldname;

	// Use this for initialization
	void Start () {
		setup (4);
	}

	protected override void immediate_effect(){
		oldname = this.name;
		this.name = "hellomynameisdebug";
		//state.take_damage (state.health_max / 2);
		state.take_damage (10);
	}

	protected override void persistant_effect(){
		print (duration);
	}

	protected override void final_effect(){		
		this.name = oldname;
	}
}



using UnityEngine;
using System.Collections;

public class BackgroundBossMusic : MonoBehaviour {


	public static BackgroundBossMusic instance;
	public AudioClip stinger;
	public bool sting;
	// Use this for initialization
	void Start () {
		instance = this;
		sting = false;
	}
	
	public void basicStinger(){
		if(!sting){
			audio.Stop ();
			audio.clip = stinger;
			audio.Play ();
			sting = true;
		}
	}
}

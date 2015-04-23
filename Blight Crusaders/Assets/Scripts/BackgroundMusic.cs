using UnityEngine;
using System.Collections;

public class BackgroundMusic : MonoBehaviour {

	public AudioClip intro;
	public AudioClip loop;
	public AudioClip stinger;
	bool sting;
	public static BackgroundMusic instance;
	// Use this for initialization
	void Start () {
		instance = this;
		sting = false;
	}

	public void StartBackground(){
		StartCoroutine (PlayMusic ());
	}
	
	IEnumerator PlayMusic(){
		yield return new WaitForSeconds (.8f);
		audio.clip = intro;
		audio.Play ();
		yield return new WaitForSeconds (audio.clip.length);
		audio.Stop ();
		audio.clip = loop;
		audio.Play ();
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

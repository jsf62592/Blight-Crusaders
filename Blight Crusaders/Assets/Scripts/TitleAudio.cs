using UnityEngine;
using System.Collections;

public class TitleAudio : MonoBehaviour {

	public AudioClip transition;
	public AudioClip buttonPress;

	AudioSource source;

	public static TitleAudio aud;
	// Use this for initialization
	void Start () {
		aud = this;
		source = GetComponent<AudioSource> ();
		source.Play ();
	}
	
	public void Transition(){
		source.Stop ();
		audio.PlayOneShot (buttonPress);
		audio.PlayOneShot (transition);
		BackgroundMusic.instance.StartBackground ();
	}
}

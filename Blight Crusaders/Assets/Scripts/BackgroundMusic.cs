using UnityEngine;
using System.Collections;

public class BackgroundMusic : MonoBehaviour {

	public AudioClip intro;
	public AudioClip loop;
	public static BackgroundMusic instance;
	// Use this for initialization
	void Start () {
		instance = this;
	}

	public void StartBackground(){
		StartCoroutine (PlayMusic ());
	}
	
	IEnumerator PlayMusic(){
		audio.clip = intro;
		audio.Play ();
		yield return new WaitForSeconds (audio.clip.length);
		audio.Stop ();
		audio.clip = loop;
		Debug.Log (audio.clip.name);
		audio.Play ();
	}
}

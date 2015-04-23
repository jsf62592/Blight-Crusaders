using UnityEngine;
using System.Collections;

public class SoundTest : MonoBehaviour {

	public AudioClip aud;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Debug.Log ("HEY");
		audio.clip = aud;
		audio.Play ();
		Debug.Log (audio.timeSamples);
	}
}

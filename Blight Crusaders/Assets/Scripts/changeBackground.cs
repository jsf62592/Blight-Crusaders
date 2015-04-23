using UnityEngine;
using System.Collections;

public class changeBackground : MonoBehaviour {

	public Texture2D BloatDies;
	public Texture2D MaryDies;

	public static changeBackground instance;

	// Use this for initialization
	void Start () {
		instance = this;
	}
	
	public void MaryLose(){
		renderer.material.mainTexture = MaryDies;
	}

	public void BloatLose(){
		renderer.material.mainTexture = BloatDies;
	}
}

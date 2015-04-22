using UnityEngine;
using System.Collections;

public class FadeScript : MonoBehaviour {


	// FadeInOut
	public static FadeScript instance;
	public Texture2D fadeTexture;
	float fadeSpeed = .8f;
	float location = 10;

	int drawDepth = -1000;	
	private float alpha = 1.0f; 
	private int fadeDir = -1;

	void Start(){
		instance = this;
	}

	void OnGUI(){
		alpha += fadeDir * fadeSpeed * Time.deltaTime;  
		alpha = Mathf.Clamp01(alpha);   
		GUI.color = new Color (GUI.color.r, GUI.color.g, GUI.color.b, alpha);

		GUI.depth = drawDepth;
		
		GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeTexture);
	}


	public void BeginFade(int direction){
		fadeDir = direction;
	}
	
}

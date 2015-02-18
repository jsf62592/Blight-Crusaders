using UnityEngine;
using System.Collections;

public class View : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

	RuntimePlatform platform = Application.platform;

	void Update(){
		if (platform == RuntimePlatform.Android || platform == RuntimePlatform.IPhonePlayer){
			if (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Began) {
				Ray ray = Camera.main.ScreenPointToRay (Input.GetTouch (0).position);
				RaycastHit hit;

				if (Physics.Raycast (ray, out hit)) {
						hit.collider.GetComponent<Touch>().TouchResponse ();  
				}
			}
		}else if(platform == RuntimePlatform.WindowsEditor){
			if (Input.GetMouseButtonDown(0)) {
				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
				RaycastHit hit;
				
				if (Physics.Raycast (ray, out hit)) {
					hit.collider.GetComponent<Touch>().TouchResponse ();  
				}
			}
		}
	}
}

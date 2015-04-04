//citation of artwork --- Frenzy  
//original author : Max@wordpress http://orangemushroom.net/author/highonmushrooms/
//original artwork page: http://orangemushroom.net/2013/05/28/kmst-ver-1-2-478-adventurer-warrior-and-magician-reorganizations/


using UnityEngine;
using System.Collections;


public class Ability_Frenzy : Ability {
	public GameObject fb1;
	public Transform prefab;
	public GameObject p1;
	
	
	//decide in which anger the bolt flies
	public float xpara;
	public float ypara;
	
	
	Vector3 origposn;
	Transform bolt;
	
	float elaspedTime = 0.0f;
	float time = 2.0f;
	
	GameObject target;
	
	void Start(){
		//origposn = transform.position;
		
		//		setup(5);
		//p1 = GameObject.Find("P1");
		//GameObject fb1 = Instantiate(fb) as GameObject;
		/*
		fb = GameObject.Find("Frostbolt");
		Instantiate(fb);
		for (int i = 0; i < 5; i++) {
			Instantiate(fb, new Vector3(i * 20.0F, 0, 0), Quaternion.identity);

		}
		*/
		//fb1 = Instantiate(Resources.Load("frostbolt")) as GameObject;
		//fb1.transform.position = new Vector3 (1, 1, 0);
		//fb1.transform.position = Vector3 (100, 100, 0);
		
		
		//prefab.transform.position= new Vector3 (5, 5, 0);
		//fb1.transform.position = new Vector3 (5, 5, 0);
		
		//bolt.position = new Vector3(1, 1, 0);
		
		//detectpara (p1);
		
	}
	
	void Update(){
		//launchto (target);
	}
	
	
	//call this function then a frost bolt will be instantiated and fired to the player
	void launchto(GameObject dest){
		
		Vector3 currentposn = prefab.position;
		prefab.position = new Vector3 (currentposn.x + xpara, currentposn.y + ypara, currentposn.z);
	}
	
	
	/*
	IEnumerator attackTarget(GameObject given_target){
		float t = 0.0f;
		while (t < 4) {
			bolt.transform.position = Vector3.Lerp(bolt, given_target.transform, (t / 4));
			t += Time.deltaTime;
			yield return null;
		}
	}
*/
	
	
	
	void detectpara(GameObject dest){
		Vector3 destposn = dest.transform.position;
		
		xpara = (dest.transform.position.x-origposn.x)/15;
		ypara = (dest.transform.position.y-origposn.y)/15;
	}
	
	/*
	IEnumerator movefrostbolt(Transform prefab, GameObject given_target){
		while (elaspedTime < time) {
			prefab.transform.position = Vector3.Lerp(origposn, given_target.transform.position, (elaspedTime / time));
			elaspedTime += Time.deltaTime;
			yield return null;
		}
	
	}
*/
	
	protected override void attachEffects(GameObject given_target){
		
		target = given_target;
		
		origposn = transform.position;
		
		detectpara (target);
		
		prefab = (Transform) Instantiate(prefab, origposn + new Vector3(-0.1f,1.5f,0), transform.rotation);// as Transform;
		
		//movefrostbolt (prefab, given_target);
		
		
		//prefab.transform.position = Vector3.Lerp(prefab, given_target.transform)
		
		//CharacterState s = selected.GetComponent<CharacterState>();
		
		//detectpara (given_target);
		
		//	launchto(given_target);
		//StartCoroutine (attackTarget ());
		
		//StartCoroutine(s.moveTo(given_target));
		//given_target.AddComponent<SE_Enemy_Fireball>().apply_effect();
		
	}
	
	
	
}
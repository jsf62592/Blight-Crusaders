using UnityEngine;
using System.Collections;

public abstract class Character : MonoBehaviour {



	private float speed;
	private int attack;

	protected void setup(float _speed, int _attack){
		speed = _speed;
		attack = _attack;
	}

	void SetAttack(int _attack){
		attack = _attack;
	}
}

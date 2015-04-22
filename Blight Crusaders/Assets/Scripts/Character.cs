using UnityEngine;
using System.Collections;

public abstract class Character : MonoBehaviour {


	private float projectileSpeed;
	private float speed;
	private int attack;

	protected void setup(float _speed, int _attack, float _projectileSpeed){
		speed = _speed;
		attack = _attack;
		projectileSpeed = _projectileSpeed;
	}

	public void SetAttack(int _attack){
		attack = _attack;
	}

	public float getAttack(){
		return attack;
	}

	public float getMoveSpeed(){
		return speed;
	}

	public float getProjectileSpeed(){
		return projectileSpeed;
	}
}

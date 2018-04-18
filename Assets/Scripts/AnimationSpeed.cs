using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSpeed : MonoBehaviour {
	public float speedScaler;

	Animator anim;
	WorldMover worldMoverReference;
	GameObject worldMoverFinder;
	float speed;

	// Use this for initialization
	void Start () {
		worldMoverFinder = GameObject.Find("Road");
		anim = GetComponent<Animator> ();
		worldMoverReference = worldMoverFinder.GetComponent<WorldMover> ();
		speed = 0;
		speedScaler = 30.0f;
	}
	
	// Update is called once per frame
	void Update () {
		speed = worldMoverReference.GetCurrentSpeed() / speedScaler;
		anim.speed = 1 + speed;
		Debug.Log ("speed " + speed + "anim speed " + anim.speed) ;
	}
}

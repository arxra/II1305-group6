using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldMover : MonoBehaviour {

	[Tooltip("Gameobjects with this tag will be moved by this script")]
	public string AffectedTag; 
	
	[Tooltip("A normalized directional vector pointing in the direction that the world should move")]
	public Vector3 MovementDirection;

	[Tooltip("The magnitude of the movement")]
	public float Magnitude;

	[Tooltip("Limits the maximum speed of the world")]
	public float MaxSpeed;


	private List<GameObject> targets; 

	// Use this for initialization
	void Start () {
		targets = new List<GameObject>(GameObject.FindGameObjectsWithTag(AffectedTag));
	}
	
	// Update is called once per frame
	void Update () {
		
		// Moving objects
		// ##############

		foreach(GameObject target in targets) {
			Rigidbody body = target.GetComponent<Rigidbody>();			

			if (body.velocity.magnitude < MaxSpeed)
				body.AddForce(MovementDirection * Magnitude);
		}

		// Endless road
		// ############

		Transform transf = this.gameObject.GetComponent<Transform>();
	
		if (transf.position.magnitude > 10)
			transf.SetPositionAndRotation(Vector3.zero, transf.rotation);		
	}
}

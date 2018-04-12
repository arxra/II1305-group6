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
  private float minSpeed;

  [Tooltip("Curretn moving speed")]
    public float currentSpeed;

  private List<GameObject> targets; 

  // Use this for initialization
  void Start () {
    MaxSpeed = 50;
    minSpeed = 25;

    targets = new List<GameObject>(GameObject.FindGameObjectsWithTag(AffectedTag));
  }

  // Update is called once per frame
  void Update () {
    Magnitude = Time.deltaTime/10;
    currentSpeed += Magnitude;
    if ( currentSpeed > MaxSpeed )
      currentSpeed = MaxSpeed;
    else if (currentSpeed < minSpeed)
      currentSpeed = minSpeed;

    // Moving objects
    // ##############

    foreach(GameObject target in targets) {
      Rigidbody body = target.GetComponent<Rigidbody>();			

      if (body.velocity.magnitude < currentSpeed)
        body.velocity = MovementDirection * currentSpeed;
    }

    // Endless road
    // ############

    Transform transf = gameObject.GetComponent<Transform>();

    if (transf.position.magnitude > 10)
      transf.SetPositionAndRotation(Vector3.zero, transf.rotation);		
  }
}

using System.Collections;

using System.Collections.Generic;

using UnityEngine;

public class PlayerControl : MonoBehaviour {

    [Tooltip("A gameobject containing child gameobjects tranformed at the x-value of each lane respectively")]
    public GameObject LaneOriginPoints;

    [Tooltip("A distance close enough to consider for locking the player into a lane")]
    public float CloseEnoughToLaneDistance = 1f;

    [Tooltip("A time that gives the player a chance to move instead locking him into the previous lane")]
    public float TimeToReachMiddle = 0.2f;

    [Tooltip("Movement magnitude factor")]
    public float MovementMagnitudeFactor;

    [Tooltip("Jump magnitude factor")]
    public float JumpMagnitudeFactor;

    private bool jump;
    private Transform transf;
    private Rigidbody rb;

    private bool isGrounded;
    private float timeout;

	private int maxLanechanges;


    // Use this for initialization

    void Start()
    {

        rb = this.gameObject.GetComponent<Rigidbody>();
        transf  = this.gameObject.GetComponent<Transform>();
		maxLanechanges = 0; // -1 = left, 0 = center, 1 = right
    }

    void FixedUpdate()

    {

        if (jump)
        {

            jump = false;
            rb.AddForce(0, rb.mass * JumpMagnitudeFactor, 0, ForceMode.Impulse);
        }
    }
    void OnCollisionEnter()
    {

        isGrounded = true;

    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded){
            jump = true;
            isGrounded = false;
        }

        float xMovement = rb.mass * MovementMagnitudeFactor;
        bool left = Input.GetKeyDown(KeyCode.LeftArrow); 

		if ((left && maxLanechanges > -1) || (Input.GetKeyDown(KeyCode.RightArrow) && maxLanechanges < 1)){
			if (left) {
				maxLanechanges--;
			}
			else {
				maxLanechanges++;
			}
			rb.AddForce(xMovement * (left ? -1 : 1), 0, 0, ForceMode.Impulse);
            rb.velocity = new Vector3(0, rb.velocity.y, rb.velocity.z);
            timeout = TimeToReachMiddle;
        }
        else if (timeout <= 0){
            foreach(Transform laneTransf in LaneOriginPoints.transform) {
                if (Mathf.Abs(transf.position.x - laneTransf.position.x) < CloseEnoughToLaneDistance)  {
                    transf.position = new Vector3(laneTransf.position.x, transf.position.y, transf.position.z);
                    rb.velocity = new Vector3(0, rb.velocity.y, rb.velocity.z);
                }
            }
        }
        else {
            timeout -= Time.deltaTime;
        }
	}
}

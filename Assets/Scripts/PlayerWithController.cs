using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWithController : MonoBehaviour {

	CharacterController player;
	public float speed;
	public float verticalVelocity;
	private int desiredLane;
	public float jumpforce;
	public float gravity; 
	private TouchPlot plot;
	public const float LANE_DISTANCE = 7.0f;
	float zedOrigin;
	bool isSliding;
	int currentLane;
	public float closeEnoughToLane;
	public Transform CameraReference;
	WorldMover WorldMoverref;
	public bool isGround;
	private bool slideFromAir;

	// Use this for initialization
	void Start () {
		player = GetComponent<CharacterController>();
		plot = new TouchPlot();
		gravity = 37.0f;
		jumpforce = 20.0f;
		desiredLane = 0;
		speed = 80f;
		verticalVelocity = 0.0f;
		zedOrigin = gameObject.transform.position.z;
		slideFromAir = false;
		isSliding = false;
		currentLane = desiredLane;
		closeEnoughToLane = 0.05f;
		player.stepOffset = 0.3f;
		player.skinWidth = player.radius * 0.1f;
		player.slopeLimit = 45;
		WorldMoverref = FindObjectOfType<WorldMover>();
	}

	// Update is called once per frame
	void Update () {
		isGround = isGrounded();
		RaycastHit hit;
		if (Physics.Raycast (new Ray (
			new Vector3 (player.bounds.center.x,
				player.bounds.center.y - player.bounds.extents.y + 0.001f,
				player.bounds.center.z + player.bounds.extents.z - 0.2f),
			Vector3.forward), out hit, player.bounds.extents.z + 2.0f)) {
			isGround = true;
			float ang = (Vector3.Angle (Vector3.up, hit.normal) * Mathf.Deg2Rad);

			Debug.DrawRay (new Vector3 (player.bounds.center.x,
				(player.bounds.center.y - player.bounds.extents.y + 0.1f),
				player.bounds.center.z + player.bounds.extents.z), Vector3.forward, Color.cyan, 0.3f); 

			//calculate requiered vertical velocity for current slope;
			if (ang < Mathf.PI / 4) {
				isGround = true;
				float boostUp = (Mathf.Tan (ang) * (WorldMoverref.GetCurrentSpeed () * Time.deltaTime));
				player.Move (Vector3.up * boostUp);
			}
		}
		Debug.Log (isGround);
		TouchPlot.SwipeDirection swipe = plot.GetSwipeDirection(25f);
		bool pressingLeft  = Input.GetKeyDown(KeyCode.LeftArrow)  || swipe == TouchPlot.SwipeDirection.LEFT,
		pressingRight = Input.GetKeyDown(KeyCode.RightArrow) || swipe == TouchPlot.SwipeDirection.RIGHT,
		pressingUp    = Input.GetKeyDown(KeyCode.Space)      || swipe == TouchPlot.SwipeDirection.UP,
		pressingDown  = Input.GetKeyDown(KeyCode.DownArrow)  || swipe == TouchPlot.SwipeDirection.DOWN;

		// Desiered Lane
		if (pressingLeft)
			moveLane (false);
		if (pressingRight)
			moveLane (true);

		//the direction of Desiered Lane
		Vector3 towards = Vector3.zero;
		if (desiredLane == -1)
			towards += Vector3.left * LANE_DISTANCE;
		else if (desiredLane == 1)
			towards += Vector3.right * LANE_DISTANCE;

		// Current Lane
		if (gameObject.transform.position.x > -0.1f * LANE_DISTANCE || gameObject.transform.position.x < 0.1f * LANE_DISTANCE) 
			currentLane = 0;
		if (gameObject.transform.position.x < -0.9f * LANE_DISTANCE)
			currentLane = -1;
		if (gameObject.transform.position.x > 0.9f * LANE_DISTANCE)
			currentLane = 1;	

		//actual vector that will perform move operation
		Vector3 moveVector = Vector3.zero;
		if (Mathf.Abs ((towards - gameObject.transform.position).x) < closeEnoughToLane) //when close enough teleport, stops from overshooting!
			gameObject.transform.position = new Vector3 (currentLane * LANE_DISTANCE, gameObject.transform.position.y, gameObject.transform.position.z);

		if (isGround) {
			verticalVelocity = -0.1f;

			if (pressingUp) {
				verticalVelocity = jumpforce;
			}  
			else if (pressingDown || slideFromAir) {
				verticalVelocity = 0.0f;
				slideFromAir = false;
				if (!isSliding) {
					startSliding ();
					Invoke ("stopSliding", 1.0f);
				}
			}
		}
		else {
			verticalVelocity -= gravity * Time.deltaTime;
			if (pressingDown) {
				verticalVelocity = -2 * jumpforce;
				slideFromAir = true;
			}
		}

		moveVector.x = (towards - gameObject.transform.position).normalized.x * speed;
		moveVector.y = verticalVelocity;

		player.Move(Vector3.right * moveVector.x * Time.deltaTime);
		if ((player.collisionFlags & CollisionFlags.Sides) != 0) {
			desiredLane = currentLane;
		}
		player.Move (Vector3.up * moveVector.y * Time.deltaTime);

		gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, zedOrigin);
	}

	void moveLane(bool goRight) {
		if (goRight) {
			desiredLane++;
			if (desiredLane > 1)
				desiredLane = 1;
		}  else {
			desiredLane--;
			if (desiredLane < -1)
				desiredLane = -1;
		}
	}

	void startSliding() {
		isSliding = true;
		player.height /= 2;
		player.center = new Vector3(player.center.x, player.center.y / 2, player.center.z);
	}
	void stopSliding() {
		if (isSliding) {
			isSliding = false;
			player.height *= 2;
			player.center = new Vector3 (player.center.x, player.center.y * 2, player.center.z);
		}
	}

	void OnControllerColliderHit(ControllerColliderHit other)
	{

	}

	void LateUpdate() {
		CameraReference.position = new Vector3 (
			CameraReference.position.x, 
			Mathf.Max (transform.position.y, 0), 
			CameraReference.position.z
		);
	}
	public bool isGrounded() {
		Ray groundRay = new Ray (
			new Vector3 (player.bounds.center.x,
				(player.bounds.center.y),
				player.bounds.center.z),
			Vector3.down);
		if (Physics.Raycast(groundRay, player.bounds.extents.y + 0.1f))
			Debug.DrawRay (groundRay.origin, groundRay.direction * (player.bounds.extents.y + 0.2f));
		bool grounded = Physics.Raycast (groundRay, player.bounds.extents.y + 0.2f);
		return grounded;
	}
}

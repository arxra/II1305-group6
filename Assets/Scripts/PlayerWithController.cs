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
	const float LANE_DISTANCE = 7.0f;
	float zedOrigin;
	bool slideFromAir;
	bool isSliding;
	float slideTimer;
	int currentLane;

	// Use this for initialization
	void Start () {
		player = GetComponent<CharacterController>();
		plot = new TouchPlot();
		gravity = 20.0f;
		jumpforce = 12.0f;
		desiredLane = 0;
		speed = 42.0f;
		verticalVelocity = 0.0f;
		zedOrigin = gameObject.transform.position.z;
		slideFromAir = false;
		isSliding = false;
		slideTimer = 0.0f;
		currentLane = desiredLane;
	}

	// Update is called once per frame
	void Update () {

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
		Vector3 towards = gameObject.transform.position.z * Vector3.forward;
		if (desiredLane == -1)
			towards += Vector3.left * LANE_DISTANCE;
		else if (desiredLane == 1)
			towards += Vector3.right * LANE_DISTANCE;

		// Current Lane
		if (gameObject.transform.position.x > -0.2f * LANE_DISTANCE || gameObject.transform.position.x < 0.2f * LANE_DISTANCE) 
			currentLane = 0;
		if (gameObject.transform.position.x < -0.8f * LANE_DISTANCE)
			currentLane = -1;
		if (gameObject.transform.position.x > 0.8f * LANE_DISTANCE)
			currentLane = 1;

		//actual vector that will perform move operation
		Vector3 moveVector = Vector3.zero;

		if (Mathf.Abs((towards - gameObject.transform.position).x) < 0.3) //when close enough teleport, stops from overshooting!
			gameObject.transform.position =  new Vector3(currentLane * LANE_DISTANCE, gameObject.transform.position.y, gameObject.transform.position.z);


		moveVector.x = (towards - gameObject.transform.position).normalized.x * speed;
		moveVector.z = zedOrigin - gameObject.transform.position.z; 
		Debug.Log (moveVector.z);

		if (player.isGrounded) {
			if (pressingUp) {
				verticalVelocity = jumpforce;
				if (isSliding) {
					isSliding = false;
					slideTimer = 0;
					stopSliding ();
				}
			}  else {
				verticalVelocity = -0.1f;
			}
			if (pressingDown || slideFromAir) {
				slideFromAir = false;
				slideTimer = 1.0f;
				if (!isSliding) {
					startSliding ();
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
		moveVector.y = verticalVelocity;
		player.Move(moveVector * Time.deltaTime);
		if (isSliding) {
			slideTimer -= Time.deltaTime;
		}
		if (slideTimer < 0) {
			slideTimer = 0;
			stopSliding ();
			isSliding = false;
		}

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
		if(player.center.y > 0)
			player.center = new Vector3(player.center.x, player.center.y / 2, player.center.z);
		else
			player.center = new Vector3(player.center.x, 2 * player.center.y, player.center.z);

		verticalVelocity = -10.0f; //make sure new hitbox is instantly grounded.
	}
	void stopSliding() {
		player.height *= 2;
		if(player.center.y > 0)
			player.center = new Vector3(player.center.x, 2 * player.center.y, player.center.z);
		else
			player.center = new Vector3(player.center.x, player.center.y / 2, player.center.z);
	}

	void OnControllerColliderHit(ControllerColliderHit other)
	{
		if (!other.gameObject.CompareTag ("Background")) {
			desiredLane = currentLane;
		}
	}
}

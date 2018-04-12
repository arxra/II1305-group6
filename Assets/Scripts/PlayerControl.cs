using System.Collections;

using System.Collections.Generic;

using UnityEngine;

public class PlayerControl : MonoBehaviour {

    [Tooltip("A gameobject containing child gameobjects tranformed at the x-value of each lane respectively")]
    public GameObject LaneOriginPoints;

    [Tooltip("A distance close enough to consider for locking the player into a lane")]
    public float CloseEnoughToLaneDistance;

    [Tooltip("Movement magnitude factor")]
    public float MovementMagnitudeFactor;

    [Tooltip("Jump magnitude factor")]
    public float JumpMagnitudeFactor;

    [Tooltip("Gravity magnitude factor")]
    public float GravityMagnitudeFactor;

    [Tooltip("The clockwise angle that the player will be sliding in")]
    public float SlidingAngle;

    [Tooltip("The number of seconds that each sliding occurs")]
    public float SlideTime;

    private Transform transf;
    private bool isGrounded;
    private float laneLockingTimeout;
    private int currentLane;
    private Vector3 laneMovementDirection;
    private bool isSliding;
    private float slidingTimer;
    private bool slideFromAir;
    private float verticalVelocity;

    // Use this for initialization

    void Start(){
        transf  = this.gameObject.GetComponent<Transform>();
        laneMovementDirection = Vector3.zero;
        currentLane = 1;
        slideFromAir = false;
    }

    // Update is called once per frame
    void Update () {
        isGrounded = Physics.Raycast(new Ray(transf.position, Vector3.down), this.gameObject.GetComponent<BoxCollider>().size.y / 2 - 0.5f);

        if (Input.GetKeyDown(KeyCode.LeftArrow))
            moveToLane(currentLane - 1);
        else if (Input.GetKeyDown(KeyCode.RightArrow))
            moveToLane(currentLane + 1);
        
        if (laneMovementDirection != Vector3.zero) {
            // The following deals with the x-dimension:
            float destination =  LaneOriginPoints.transform.GetChild(currentLane).transform.position.x;
            float speed = MovementMagnitudeFactor * Mathf.Abs(destination - transf.position.x);
            
            transf.position += laneMovementDirection * speed * Time.deltaTime;
        }
        else if (laneLockingTimeout <= 0) {
            foreach(Transform laneTransf in LaneOriginPoints.transform) {
                if (laneLockingTimeout <= 0 && Mathf.Abs(transf.position.x - laneTransf.position.x) < CloseEnoughToLaneDistance)  {
                    transf.position = new Vector3(laneTransf.position.x, transf.position.y, transf.position.z);
                    laneMovementDirection = Vector3.zero;
                }
            }
        }
    

        bool pressingDown = Input.GetKeyDown(KeyCode.DownArrow);   

        // Jumping & gravity calculation
        // #############################
        

        if (isGrounded){
            // On the ground:
            
            if (Input.GetKeyDown(KeyCode.Space)){
                verticalVelocity = JumpMagnitudeFactor;
                slidingTimer = 0f;
                slideFromAir = false;
            }
            else{
                if (pressingDown || slideFromAir){
                    slidingTimer = SlideTime;
                    slideFromAir = false;
                }

                verticalVelocity = Mathf.Max(0, verticalVelocity);    
            }
        }
        else {
            // In the air:

            if(pressingDown) {
                verticalVelocity = -JumpMagnitudeFactor;
                slideFromAir = true;
            }
            else
                verticalVelocity -= GravityMagnitudeFactor * Time.deltaTime;
        }


        transf.Rotate(getSlidingAngleDelta(), 0f, 0f);
        transf.position += Vector3.up * verticalVelocity * Time.deltaTime;

        laneLockingTimeout -= Time.deltaTime;
        slidingTimer -= Time.deltaTime;
	}

    public float getSlidingAngleDelta() {
        float currentClockwiseAngle = transf.eulerAngles.x;
        float a = 0, b = 0;

        if (slidingTimer > 0f) {
            a = (SlidingAngle - currentClockwiseAngle) * Time.deltaTime;
            b = (SlidingAngle - currentClockwiseAngle - 360) * Time.deltaTime;
            isSliding = true; 
        }
        else if (isSliding) {
            a = -currentClockwiseAngle;
            b = 360 - currentClockwiseAngle;
            isSliding = Mathf.Abs(currentClockwiseAngle) > 1f;

            if (isSliding){
                a *= Time.deltaTime;
                b *= Time.deltaTime;
            }
        }

        return 10f * (Mathf.Abs(a) < Mathf.Abs(b) ? a : b);
    }

    public void moveToLane(int lane) {
        laneMovementDirection = Vector3.zero;
        int validNewLane = Mathf.Clamp(lane, 0, LaneOriginPoints.transform.childCount - 1);
        
        if (currentLane != validNewLane) {
            laneMovementDirection = (validNewLane < currentLane ? Vector3.left : Vector3.right);
            currentLane = validNewLane;   
        }
    }
}

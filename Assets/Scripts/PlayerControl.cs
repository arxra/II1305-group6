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

    [Tooltip("The player's distance to the origin")]
    public float GroundLevel;

    private Transform transf;
    private BoxCollider collider;
    private bool isGrounded;
    private float laneLockingTimeout;
    private int currentLane;
    private Vector3 laneMovementDirection;
    private bool isSliding;
    private float slidingTimer;
    private bool slideFromAir;
    private float verticalVelocity;

    private TouchPlot plot;

    // Use this for initialization

    void Start(){
        transf  = this.gameObject.GetComponent<Transform>();
        laneMovementDirection = Vector3.zero;
        currentLane = 1;
        slideFromAir = false;
        collider = this.gameObject.GetComponent<BoxCollider>();
        plot = new TouchPlot();
    }

    // Update is called once per frame
    void Update () {
        isGrounded = Physics.Raycast(new Ray(transf.position + collider.center, Vector3.down), getDistanceToCharacterBottom() + GroundLevel);
        
        // Touch & Keyboard Input
        // ######################

        TouchPlot.SwipeDirection swipe = plot.GetSwipeDirection(25f);
        bool pressingLeft  = Input.GetKeyDown(KeyCode.LeftArrow)  || swipe == TouchPlot.SwipeDirection.LEFT,
             pressingRight = Input.GetKeyDown(KeyCode.RightArrow) || swipe == TouchPlot.SwipeDirection.RIGHT,
             pressingUp    = Input.GetKeyDown(KeyCode.Space)      || swipe == TouchPlot.SwipeDirection.UP,
             pressingDown  = Input.GetKeyDown(KeyCode.DownArrow)  || swipe == TouchPlot.SwipeDirection.DOWN;  

        // Lane movement
        // #############

        if (pressingLeft)
            moveToLane(currentLane - 1);
        else if (pressingRight)
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

        // Jumping & gravity calculation
        // #############################
        

        if (isGrounded){
            // On the ground:
            
            if (pressingUp) {
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

        transf.position = new Vector3(
            transf.position.x, 
            Mathf.Max(transf.position.y, GroundLevel), 
            transf.position.z
        );
	}

    public float getSlidingAngleDelta() {
        float currentClockwiseAngle = transf.eulerAngles.x;
        float a = 0, b = 0;
        const float speedFactor = 10f;

        if (slidingTimer > 0f) {
            a = (SlidingAngle - currentClockwiseAngle) * Time.deltaTime * speedFactor;
            b = (SlidingAngle - currentClockwiseAngle - 360) * Time.deltaTime * speedFactor;
            isSliding = true; 
        }
        else if (isSliding) {     
            a = -currentClockwiseAngle;
            b = 360 - currentClockwiseAngle;
            isSliding = Mathf.Abs(currentClockwiseAngle) > 1f;

            if (isSliding){
                a *= Time.deltaTime * speedFactor;
                b *= Time.deltaTime * speedFactor;
            }
        }

        return (Mathf.Abs(a) < Mathf.Abs(b) ? a : b);
    }

    public void moveToLane(int lane) {
        int validNewLane = Mathf.Clamp(lane, 0, LaneOriginPoints.transform.childCount - 1);
        
        if (currentLane != validNewLane) {
            laneMovementDirection = (validNewLane < currentLane ? Vector3.left : Vector3.right);
            currentLane = validNewLane;   
        }
    }

    public float getDistanceToCharacterBottom() {
        return Mathf.Sqrt(
            Mathf.Pow(collider.size.y * Mathf.Sin(transf.rotation.eulerAngles.x) / 2, 2) +
            Mathf.Pow(collider.size.y * Mathf.Cos(transf.rotation.eulerAngles.x) / 2, 2)
        );
    }
}

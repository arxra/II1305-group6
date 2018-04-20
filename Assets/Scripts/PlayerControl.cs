using System;
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
    public float GroundLevelMargin;

    [Tooltip("Reference to the camera")]
    public Transform CameraReference;
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

    private WorldMover worldMover;

    private float distanceBetweenEachLane;

    // Use this for initialization

    void Start(){
        laneMovementDirection = Vector3.zero;
        currentLane = 1;
        slideFromAir = false;
        collider = this.gameObject.GetComponent<BoxCollider>();
        plot = new TouchPlot();

        Func<int, int, float>  calcDistanceBetweenLane = (l1, l2) => Mathf.Abs(
            LaneOriginPoints.transform.GetChild(l2).transform.position.x - 
            LaneOriginPoints.transform.GetChild(l1).transform.position.x
        );

        for(int i = 1; i < LaneOriginPoints.transform.childCount; i++){
            if (i < 2)
                distanceBetweenEachLane = calcDistanceBetweenLane(i - 1, i);
            else if(Debug.isDebugBuild)
                Debug.Assert(calcDistanceBetweenLane(i - 1, i) == distanceBetweenEachLane, "The distance between lanes must consistent");
        }
    }

    // Update is called once per frame
    void Update () {
        // Stay on the floor
        // #################
        
        RaycastHit hit;
        float distanceToGround = GroundLevelMargin + getEllipsRadius();
        isGrounded = Physics.Raycast(new Ray(transform.position + collider.center, Vector3.down), out hit, distanceToGround);
        
        if (isGrounded) 
            transform.position = new Vector3(
                transform.position.x, 
                Mathf.Max(hit.point.y + distanceToGround, transform.position.y), 
                transform.position.z
            );
        

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
            Debug.Log(currentLane);
            // The following deals with the x-dimension:
            float destination =  LaneOriginPoints.transform.GetChild(currentLane).transform.position.x;
            float speed = Mathf.Min(MovementMagnitudeFactor * Mathf.Abs(destination - transform.position.x), 100f);
        
            transform.position += laneMovementDirection * speed * Time.deltaTime;
        }
        else if (laneLockingTimeout <= 0) {
            foreach(Transform laneTransf in LaneOriginPoints.transform) {
                if (laneLockingTimeout <= 0 && Mathf.Abs(transform.position.x - laneTransf.position.x) < CloseEnoughToLaneDistance)  {
                    transform.position = new Vector3(laneTransf.position.x, transform.position.y, transform.position.z);
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
                verticalVelocity = -4f * JumpMagnitudeFactor;
                slideFromAir = true;
            }
            else
                verticalVelocity -= GravityMagnitudeFactor * Time.deltaTime;
        }


        transform.Rotate(getSlidingAngleDelta(), 0f, 0f);
        transform.position += Vector3.up * verticalVelocity * Time.deltaTime;

        laneLockingTimeout -= Time.deltaTime;
        slidingTimer -= Time.deltaTime;

        transform.position = new Vector3(
            transform.position.x, 
            Mathf.Max(transform.position.y, GroundLevelMargin), 
            transform.position.z
        );
	}

    void LateUpdate() {
        CameraReference.position = new Vector3(
            CameraReference.position.x, 
            Mathf.Max(transform.position.y, 0), 
            CameraReference.position.z
        );
    }

    public float getSlidingAngleDelta() {
        float currentClockwiseAngle = transform.eulerAngles.x;
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

            if (Physics.Raycast(new Ray(transform.position + collider.center, laneMovementDirection), distanceBetweenEachLane)) {
                // Obstacle in the way
                laneMovementDirection = Vector3.zero;
                return;
            }

            currentLane = validNewLane;   
        }
    }

    private float getEllipsRadius() {
        float a = collider.size.y / 2;
        float b = collider.size.z / 2;
        float angle = Mathf.Deg2Rad * transform.rotation.eulerAngles.x;

        return (
            a * b / 
            Mathf.Sqrt(
                a * a * Mathf.Pow(Mathf.Sin(angle), 2f) + 
                b * b * Mathf.Pow(Mathf.Cos(angle), 2f)
            )
        );
    }
}

using System.Collections.Generic;
using UnityEngine;

public class WorldMover : MonoBehaviour {

    [Tooltip("How much can the background move for realistic movement before it needs to reset?")]
    public float BackgroundMaxOffset;

    
    [Tooltip("A normalized directional vector pointing in the direction that the world should move")]
    public Vector3 MovementDirection;


    [Tooltip("The acceleration of the movement [m/s^2]")]
    public float Acceleration;

    [Tooltip("Limits the maximum speed of the world [m/s]")]
    public float MaxSpeed;

    private float currentSpeed;


    // Use this for initialization
    void Start () {
        currentSpeed = 0f;
    }

  // Update is called once per frame
    void Update () {
        currentSpeed = Mathf.Clamp(currentSpeed + Acceleration * Time.deltaTime, 0f, MaxSpeed);
        Vector3 offset = MovementDirection * currentSpeed * Time.deltaTime;

        // Foreground objects
        // ##################

        foreach(GameObject target in ObjectFilter.EntitiesWithTags(ObjectFilter.Tag.Foreground)) 
            target.transform.position += offset;


        // Background objects
        // ##################

        foreach(GameObject target in ObjectFilter.EntitiesWithTags(ObjectFilter.Tag.Background)) {
            target.transform.position += offset;    

            float bgOffset = target.transform.position.magnitude;
            if (bgOffset > BackgroundMaxOffset && BackgroundMaxOffset > 0f)
                target.transform.position -= MovementDirection * bgOffset;
        }	
    }

    public float GetDistanceMoved() {
        float timeToReachMax = MaxSpeed / Acceleration;
 
        return currentSpeed / 2f * timeToReachMax + MaxSpeed * (Time.time - timeToReachMax);
        //     --------- acceleration -----------   -------------- constant ----------------
    }

    public float GetCurrentSpeed() {
        return currentSpeed;
    }
}



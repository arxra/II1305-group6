using System.Linq;
using System.Collections.Generic;
using UnityEngine;

//World mover

public class WorldMover : MonoBehaviour {

    public Transform BackgroundStart, BackgroundStop;
    
    [Tooltip("A normalized directional vector pointing in the direction that the world should move")]
    public Vector3 MovementDirection;

    [Tooltip("The acceleration of the movement [m/s^2]")]
    public float Acceleration;

    [Tooltip("Limits the maximum speed of the world [m/s]")]
   
	public float MaxSpeed;

    public bool IsKill;

    private float currentSpeed;

    private GameObject partA, partB;

    private Vector3 posA, posB;

   
    void Start () {
        currentSpeed = 0f;

        partA = GameObject.Find("Part A");
        partB = GameObject.Find("Part B");

        posA = partA.transform.position;
        posB = partB.transform.position;
    }

  
    void Update () {

		//If to be killed - slowly decrease
        if (IsKill && currentSpeed != 0){
            if (currentSpeed > 0)
                currentSpeed *= -1f;

            if (Mathf.Abs(currentSpeed) > 1f)
                currentSpeed *= 0.5f;
            else
                currentSpeed = 0f;
        }
        else 
			//If not - increase speed
            currentSpeed = Mathf.Clamp(currentSpeed + Acceleration * Time.deltaTime, 0f, MaxSpeed);    
        

        Vector3 offset = MovementDirection * currentSpeed * Time.deltaTime;

       /**********************************************************************************************/
		// Foreground objects

        foreach(GameObject target in ObjectFilter.EntitiesWithTags(ObjectFilter.Tag.Foreground)) 
            target.transform.position += offset;

		/**********************************************************************************************/
        // Background objects

        partA.transform.position += offset;
        partB.transform.position += offset;

       /************************************************************************************************/
		// Switch the positioning of part A and B

        if (partA.transform.Find("End").transform.position.z <= 0){
            
            partA.transform.position = posB;
            partB.transform.position = posA;

            var tmp = partA;
            partA = partB;
            partB = tmp;
        }
    }

    public float GetTotalDistanceMoved() {
        float timeToReachMax = MaxSpeed / Acceleration;
 
        return currentSpeed / 2f * timeToReachMax + MaxSpeed * (Time.time - timeToReachMax);
        //     --------- acceleration -----------   -------------- constant ----------------
    }

    public float GetCurrentSpeed() {
        return currentSpeed;
    }
}

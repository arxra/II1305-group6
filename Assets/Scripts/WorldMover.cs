using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class WorldMover : MonoBehaviour {

    public Transform BackgroundStart, BackgroundStop;
    
    [Tooltip("A normalized directional vector pointing in the direction that the world should move")]
    public Vector3 MovementDirection;


    [Tooltip("The acceleration of the movement [m/s^2]")]
    public float Acceleration;

    [Tooltip("Limits the maximum speed of the world [m/s]")]
    public float MaxSpeed;

    private float currentSpeed;

    private KeyValuePair<GameObject, Bounds> partA, partB;


    // Use this for initialization
    void Start () {
        currentSpeed = 0f;

        var a = GameObject.Find("Part A");
        Bounds bounds_a = new Bounds(a.transform.position, Vector3.zero);
        foreach(Renderer renderer in a.GetComponentsInChildren<Renderer>()) 
            bounds_a.Encapsulate(renderer.bounds);
        partA = new KeyValuePair<GameObject, Bounds>(a, bounds_a);

        var b = GameObject.Find("Part B");
        Bounds bounds_b = new Bounds(b.transform.position, Vector3.zero);
        foreach(Renderer renderer in b.GetComponentsInChildren<Renderer>()) 
            bounds_b.Encapsulate(renderer.bounds);
        partB = new KeyValuePair<GameObject, Bounds>(b, bounds_b);
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

        partA.Key.transform.position += offset;
        partB.Key.transform.position += offset;

        // |  A  |  B  | 
        var criticalZPos = partA.Key.transform.Find("End").transform.position.z;
        if (criticalZPos <= 0){
            partA.Key.transform.position += Vector3.forward * (-criticalZPos + partA.Value.size.z + partB.Value.size.z - 23f);
            
            var tmp = partA;
            partA = partB;
            partB = tmp;
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



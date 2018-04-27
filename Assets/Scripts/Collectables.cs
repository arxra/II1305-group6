using UnityEngine;

public class Collectables : MonoBehaviour {

  [Tooltip("Value of pickup in score")]
    public int value = 100; 

  [Tooltip("Duration")]
    public float _time;

  [Tooltip("Multiplier value")]
    public float _mult;

  [Tooltip("Size modifier in x'times scale")]
    public float _sizeMultiplier;

  void Update() {
    // Rotate the object around its local X axis at 1 degree per second
    transform.Rotate(Vector3.up * Time.deltaTime * 75);

  }

}


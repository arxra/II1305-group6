using UnityEngine;

public class Collectables : MonoBehaviour {

  [Tooltip("Value of pickup in score")]
    public int value = 100; 

  [Tooltip("Duration")]
    public float _time;

  [Tooltip("Multiplier value")]
    public float _mult;

  [Tooltip("Food factor, for boost and currency")]
    public int _sizeMultiplier;

  public Upgrades up;

  public void Start() {
    foreach (Upgrades.Upgrade t in up.CurrentUpgrades()){
      if(t.IsMyName("multiplier_duration"))
        _time += t.Level() * 5;
    }
  }

  void Update() {
    // Rotate the object around its local X axis at 1 degree per second
    transform.Rotate(Vector3.up * Time.deltaTime * 75);

  }

}


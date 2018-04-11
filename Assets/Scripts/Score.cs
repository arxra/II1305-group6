using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour {

  [Tooltip("The current players score")]
    public int score;

  //Read the speed from the WorldMover script to calclate the points.
  private WorldMover mv; 
  // Use this for initialization
  void Start () {
mv : WorldMover = GetComponent(WorldMover);
    score = 0;
  }

  // Update is called once per frame
  void Update () {
    score += WorldMover.Magnitude/100;
  }
}

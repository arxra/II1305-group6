using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour {

  [Tooltip("The current players score")]
    public int score;

  //Read the speed from the WorldMover script to calclate the points.
  private WorldMover mv = null; 
  // Use this for initialization
  void Start () {
    score = 0;
  }

  // Update is called once per frame
  void Update () {
    int multiplier = 1; // Change this one later to get it's variable from the multiplier
    int scoreUpdate = 0;
    if(Time.timeScale == 1)
      scoreUpdate += Mathf.RoundToInt(GameObject.Find("Road").GetComponent<WorldMover>().Magnitude/1000);

    score += scoreUpdate * multiplier;
  }
}

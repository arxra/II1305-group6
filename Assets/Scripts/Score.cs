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
    if(Time.timeScale == 1)
      score += Mathf.RoundToInt(GameObject.Find("Road").GetComponent<WorldMover>().Magnitude/1000);
  }
}

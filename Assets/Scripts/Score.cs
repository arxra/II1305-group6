using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour {

  [Tooltip("The current players score")]
    public int score;

  // Use this for initialization
  void Start () {
    score = 0;
  }

  // Update is called once per frame
  void Update () {
    int multiplier = 1; // Change this one later to get it's variable from the multiplier
    float scoreUpdate = 0;
    if(Time.timeScale == 1)
      scoreUpdate += GameObject.Find("Road").GetComponent<WorldMover>().Magnitude/700;

    score += Mathf.RoundToInt(scoreUpdate * multiplier);
  }
}

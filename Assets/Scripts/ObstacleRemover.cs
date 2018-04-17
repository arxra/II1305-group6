using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleRemover : MonoBehaviour {
	public string ObstacleTag = "Obstacle";
  public string ObstacleTag2 = "Pick Up";

	void OnTriggerEnter(Collider other) {
		if (other.tag.Contains(ObstacleTag) || other.tag.Contains(ObstacleTag2))
        	Destroy(other.gameObject);
    }
}

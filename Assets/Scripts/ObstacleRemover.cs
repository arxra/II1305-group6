using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleRemover : MonoBehaviour {
	public string ObstacleTag = "Obstacle";

	void OnTriggerEnter(Collider other) {
		if (other.tag.Contains(ObstacleTag))
        	Destroy(other.gameObject);
    }
}

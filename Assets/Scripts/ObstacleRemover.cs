using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleRemover : MonoBehaviour {
	public string FilterTag;

	void OnTriggerExit(Collider other) {
		if (ObjectFilter.RelativesHasTags(other.gameObject, ObjectFilter.Tag.Foreground))	
        	Destroy(other.gameObject, 10f);
    }
}

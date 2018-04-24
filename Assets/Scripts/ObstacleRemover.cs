using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleRemover : MonoBehaviour {
	public string FilterTag;

	void OnTriggerEnter(Collider other) {
		var enemy = ObjectFilter.RelativesHasTags(other.gameObject, ObjectFilter.Tag.Foreground);

		if (enemy != null)	
        	Object.Destroy(enemy, 10f);
    }
}

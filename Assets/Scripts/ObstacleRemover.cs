using UnityEngine;

public class ObstacleRemover : MonoBehaviour {
	public ObjectFilter.Tag _filterTag = ObjectFilter.Tag.Foreground;

	void OnTriggerEnter(Collider other) {
		if (ObjectFilter.RelativesHasTags(other.gameObject, _filterTag))	
        	Destroy(other.gameObject, 10f);
    }
}

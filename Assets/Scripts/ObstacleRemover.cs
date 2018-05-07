using UnityEngine;

//Removes the parent of obstacle
public class ObstacleRemover : MonoBehaviour {
	public ObjectFilter.Tag _filterTag = ObjectFilter.Tag.Foreground;

	void OnTriggerEnter(Collider other) {
		//var enemy = ObjectFilter.RelativesHasTags(other.gameObject, ObjectFilter.Tag.Foreground);

    var enemy = other.gameObject;
		if (enemy != null)	
        	Object.Destroy(enemy, 10f);
    }
}

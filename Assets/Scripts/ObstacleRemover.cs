using UnityEngine;

//Removes the parent of obstacle
public class ObstacleRemover : MonoBehaviour {
	public ObjectFilter.Tag _filterTag = ObjectFilter.Tag.Foreground;
    public WorldMover wm;
	void OnTriggerEnter(Collider other) {
		//var enemy = ObjectFilter.RelativesHasTags(other.gameObject, ObjectFilter.Tag.Foreground);

    var enemy = other.gameObject;
        if (enemy != null)
        {
           
            if (enemy.tag.Equals("Foreground Collectable"))
            {
                Object.Destroy(enemy);
                wm.removeFromList(enemy);
            }
            else
            {
                Object.Destroy(enemy.transform.parent.gameObject);
                wm.removeFromList(enemy.transform.parent.gameObject);
            }
        }
    }
}

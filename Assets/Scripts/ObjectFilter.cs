using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ObjectFilter {
	public enum Tag {
		Foreground,
		
		Background,

		Enemy,
		Static,
		Collectable
	}



	public static bool EntityHasTags(GameObject entity, params Tag[] tags) {
		return new List<GameObject>(EntitiesWithTags(tags)).Contains(entity);
	}

	public static bool RelativesHasTags(GameObject entity, params Tag[] tags) {
		Transform parent = entity.transform.parent;

		return EntityHasTags(entity, tags) || (parent != null ? RelativesHasTags(parent.gameObject, tags) : false);
	}

	public static IEnumerable<GameObject> EntitiesWithTags(params Tag[] tags) {
		bool hasTags;

		foreach(GameObject obj in GameObject.FindObjectsOfType(typeof (GameObject))){
			hasTags = true;
			
			foreach(Tag tag in tags) {
				if(!obj.tag.Contains(tag.ToString()))
        			hasTags = false;
			}

			if(hasTags)
				yield return obj;
		}
	}
}

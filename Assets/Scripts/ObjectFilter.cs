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

	public static GameObject RelativesHasTags(GameObject entity, params Tag[] tags) {
		List<GameObject> entities = new List<GameObject>(EntitiesWithTags(tags));

		Transform current = entity.transform;
		while (current != null) {
			if (entities.Contains(current.gameObject))
				return current.gameObject;
			else
				current = current.transform.parent;
		}

		return null;
	}

	public static IEnumerable<GameObject> EntitiesWithTags(params Tag[] tags) {
		bool hasTags;

		foreach(GameObject obj in GameObject.FindObjectsOfType(typeof (GameObject))){
			
			hasTags = true;
			
			foreach(Tag tag in tags) {
				if(!obj.tag.Contains(tag.ToString()))
        			hasTags = false;
			}

			if(hasTags){
				
				yield return obj;
				
			}

		}
	}
}

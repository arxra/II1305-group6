using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Spawn logic

public class SpawnPolicy : MonoBehaviour {

	[Header("This script will override the default spawn settings")]
	
	//Default spawning values

	[Header("(default value = 10)")]
	[Tooltip("An indicator to how often this gameobject should spawn in relation to other candidates")]
	public int Popularity;

	[Header("(default value = -1)")]
	[Tooltip("The number of remaining instances this candidate should spawn (-1 means infinity)")]
	public int RemainingInstances;

	[Header("(default value = true)")]
	[Tooltip("A list of lanes you don't want this candidate to spawn in")]
	public bool SpawnInLaneA = true;
	public bool SpawnInLaneB = true;
	public bool SpawnInLaneC = true;

	private GameObject candidate;

	public SpawnPolicy(){
		Popularity = 10;
		RemainingInstances = -1;
		SpawnInLaneA = true;
		SpawnInLaneB = true;
		SpawnInLaneC = true;
	}

	public void Start() {
		candidate = gameObject;
	}

	public SpawnPolicy(GameObject candidate, int popularity = 10, int remainingInstances = -1) {
		this.candidate = candidate;
		Popularity = popularity;
		RemainingInstances = remainingInstances; 
	}

	//Spawn candidate at position
	public bool InstantiateAt(Vector3 position) {
		if (RemainingInstances == 0)
			return false;
		else if(RemainingInstances > 0)
			RemainingInstances--;

		if (candidate == null)
			candidate = gameObject;

		UnityEngine.Object.Instantiate(candidate, position, candidate.transform.rotation);
		return true;
	}

	//Check and choose lanes 
	public Vector3 GetSuitableSpawnPoint(Transform spawnPoints, ref int lastLane) {
		List<bool> activeSpawnPoints = new List<bool> {SpawnInLaneA, SpawnInLaneB, SpawnInLaneC};
		activeSpawnPoints[lastLane] = false;

		var range = Enumerable.Range(0, spawnPoints.childCount).Where(v => activeSpawnPoints[v]);

		if (range.Any()){
			lastLane = range.ElementAt(UnityEngine.Random.Range(0, range.Count()));

			return (
				spawnPoints.GetChild(lastLane).position + 
				Vector3.up * candidate.transform.position.y
			);
		}
		else
			throw new UnityException("You fucked up! Too many excluded lanes :(");
	}
} 
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Master of spawn policies

public class ObstacleCreator : MonoBehaviour {
	[Tooltip("The time between each obstacle spawn [seconds]")]
	public float AutoSpawnInterval = 1;

	private List<SpawnPolicy> policies;

	private int totalPopularity;
	private float timeout;
	private int lastLane;


	void Start () {
		policies = new List<SpawnPolicy>();
		timeout = AutoSpawnInterval;	
		totalPopularity = 0;
		lastLane = 1;

		//Scan candidates
		foreach(Transform transf in transform.Find("Candidates").transform) {
			SpawnPolicy policy = transf.GetComponent<SpawnPolicy>();

			//Create default value if none preexisting 
			if (policy == null){
				transf.gameObject.AddComponent(typeof(SpawnPolicy));
				policy = transf.GetComponent<SpawnPolicy>();
			}

			totalPopularity += policy.Popularity;
			policies.Add(policy);
		}
	}
	

	void Update () {

		//Sets continuous spawning intervals
		if (timeout > 0) {
			timeout -= Time.deltaTime;
			return;
		}
		else
			timeout = AutoSpawnInterval;

		//Check if spawned to spawn 
		bool hasSpawned = false;
		SpawnPolicy session;
		Vector3 suitableSpawnPoint;

		while(!hasSpawned) {
			session = SpawnNow();
			
			if (session != null){
				suitableSpawnPoint = session.GetSuitableSpawnPoint(transform.Find("Spawn Points"), ref lastLane);
				hasSpawned = session.InstantiateAt(suitableSpawnPoint);
				
				if (!hasSpawned && suitableSpawnPoint != Vector3.zero){
					// Stop spawning this object
					policies.Remove(session);
					totalPopularity -= session.Popularity;
				}
			}
			else {
				Debug.LogWarning("ObstacleCreator: Disabling since there are no more obstacles left to spawn. Is this intentional?");
				enabled = false;
				return;
			}
		}
	}

	//Returns a randomly selected gameobject to spawn 
	public SpawnPolicy SpawnNow() {
		int rand = Random.Range(0, totalPopularity);
		int pos = 0;

		foreach(SpawnPolicy policy in policies) {
			if (rand >= pos & rand < (pos += policy.Popularity))
				return policy;
		}

		return null;
	}

}

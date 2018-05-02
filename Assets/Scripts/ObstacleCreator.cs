using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleCreator : MonoBehaviour {
	[Tooltip("The time between each obstacle spawn [seconds]")]
	public float AutoSpawnInterval = 1;

	private List<SpawnPolicy> policies;

	private int totalPopularity;
	private float timeout;
	private int lastLane;

	// Use this for initialization
	void Start () {
		policies = new List<SpawnPolicy>();
		timeout = AutoSpawnInterval;	
		totalPopularity = 0;
		lastLane = 1;

		foreach(Transform transf in transform.Find("Candidates").transform) {
			SpawnPolicy policy = transf.GetComponent<SpawnPolicy>();

			if (policy == null){
				transf.gameObject.AddComponent(typeof(SpawnPolicy));
				policy = transf.GetComponent<SpawnPolicy>();
			}

			totalPopularity += policy.Popularity;
			policies.Add(policy);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (timeout > 0) {
			timeout -= Time.deltaTime;
			return;
		}
		else
			timeout = AutoSpawnInterval;

		SpawnPolicy session = SpawnNow();
		if (session != null){
			if (!session.InstantiateAt(session.GetSuitableSpawnPoint(transform.Find("Spawn Points"), ref lastLane))){
				policies.Remove(session);
				totalPopularity -= session.Popularity;
			}
		}
		else {
			Debug.LogWarning("ObstacleCreator: Disabling since there are no more obstacles left to spawn. Is this intentional?");
			enabled = false;
		}
	}

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

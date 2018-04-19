using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleCreator : MonoBehaviour {

	[System.Serializable]
	public class SpawnSession {
		[Tooltip("A pointer to the candidate gameobject you want to override the spawn settings for")]
		public GameObject Candidate;
		
		[Tooltip("An indicator to how often this gameobject should spawn in relation to other candidates")]
		public int Popularity;

		[Tooltip("The number of remaining instances this candidate should spawn (-1 means infinity)")]
		public int RemainingInstances;

		[Tooltip("A list of lanes you don't want this candidate to spawn in")]
		public List<int> ExcludeLanes;

		public SpawnSession(GameObject candidate, int popularity = 10, int remainingInstances = -1) {
			Candidate = candidate;
			Popularity = popularity;
			RemainingInstances = remainingInstances; 
			ExcludeLanes = new List<int>();
		}

		public bool InstantiateAt(Vector3 position) {
			if (RemainingInstances == 0)
				return false;
			else if(RemainingInstances > 0)
				RemainingInstances--;

			Object.Instantiate(Candidate, position, Candidate.transform.rotation);
			return true;
		}

		public Vector3 GetSuitableSpawnPoint(Transform spawnPoints, ref int lastLane) {
			ExcludeLanes.Add(lastLane);
			var range = Enumerable.Range(0, spawnPoints.childCount).Where(v => !ExcludeLanes.Contains(v));
			ExcludeLanes.Remove(lastLane);

			if (range.Any()){
				lastLane = range.ElementAt(Random.Range(0, range.Count()));

				return (
					spawnPoints.GetChild(lastLane).position + 
					Vector3.up * Candidate.transform.position.y
				);
			}
			else
				throw new UnityException("You fucked up! Too many excluded lanes :(");
		}
	} 

	[Tooltip("The time between each obstacle spawn [seconds]")]
	public float AutoSpawnInterval = 1;
	
	[Tooltip("Overrides the default spawn settings for a candidate")]
	public List<SpawnSession> SpawnOverrides = new List<SpawnSession>();

	private int totalPopularity;
	private float timeout;
	private int lastLane;

	public ObstacleCreator() {
		SpawnOverrides = new List<SpawnSession>() {
			new SpawnSession(null)
		};
	}

	// Use this for initialization
	void Start () {
		timeout = AutoSpawnInterval;	
		totalPopularity = 0;	
		lastLane = -1;

		foreach(Transform transf in transform.Find("Candidates")) {
			var session = SpawnOverrides.Find(s => s.Candidate.Equals(transf.gameObject));
			
			if (session == null) {
				session = new SpawnSession(transf.gameObject);
				SpawnOverrides.Add(session);
			}

			totalPopularity += session.Popularity;
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

		var session = SpawnNow();
		if (session != null){
			if (session.Candidate == null || !session.InstantiateAt(session.GetSuitableSpawnPoint(transform.Find("Spawn Points"), ref lastLane))){
				SpawnOverrides.Remove(session);
				totalPopularity -= session.Popularity;
			}
		}
		else {
			Debug.LogWarning("ObstacleCreator: Disabling since there are no more obstacles left to spawn. Is this intentional?");
			enabled = false;
		}
	}

	public SpawnSession SpawnNow() {
		int rand = Random.Range(0, totalPopularity);
		int pos = 0;

		foreach(SpawnSession session in SpawnOverrides) {
			if (rand >= pos & rand < (pos += session.Popularity))
				return session;
		}

		return null;
	}
}

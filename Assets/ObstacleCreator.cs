using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleCreator : MonoBehaviour {

	[Tooltip("A gameobject containg child gameobjects each located at the possible spawn points")]
	public GameObject SpawnPoints;

	[Tooltip("The number of seconds in-between each obstacle spawn")]
	public float SpawnTimeInterval = 5;

	private float timeout;

	private int lastLane;

	// Use this for initialization
	void Start () {
		timeout = SpawnTimeInterval;	
	}
	
	// Update is called once per frame
	void Update () {
		if (timeout > 0) {
			timeout -= Time.deltaTime;
		}
		else {
			timeout = SpawnTimeInterval;

			int newLane = lastLane;
			while (newLane == lastLane)
				newLane = Random.Range(0, SpawnPoints.transform.childCount);

			Transform candidates = this.gameObject.transform.Find("Candidates");

			Instantiate(
				candidates.GetChild(Random.Range(0, candidates.childCount)), 
				SpawnPoints.transform.GetChild(newLane).position, 
				Quaternion.identity
			);

			lastLane = newLane;
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleCreator : MonoBehaviour {

	[Tooltip("A gameobject containg child gameobjects each located at the possible spawn points")]
	public GameObject SpawnPoints;

	[Tooltip("The number of seconds in-between each obstacle spawn")]
	public float SpawnTimeInterval = 5;

	
	public List<string> OnlyInLaneA;
	public List<string> OnlyInLaneB;
	public List<string> OnlyInLaneC;

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

			Transform candidates = this.gameObject.transform.Find("Candidates");
			Transform randomChild = candidates.GetChild(Random.Range(0, candidates.childCount));

			int newLane = lastLane;
			if (OnlyInLaneA.Contains(randomChild.name))
				newLane = 0;
			else if (OnlyInLaneB.Contains(randomChild.name))
				newLane = 1;
			else if (OnlyInLaneC.Contains(randomChild.name))
				newLane = 2;
			else {
				while (newLane == lastLane)
					newLane = Random.Range(0, SpawnPoints.transform.childCount);
			}

			Transform spawnPoint = SpawnPoints.transform.GetChild(newLane);

			Instantiate(
				randomChild, 
				new Vector3(
					spawnPoint.position.x, 
					spawnPoint.position.y + randomChild.transform.position.y,
					spawnPoint.position.z
				),
				Quaternion.identity
			);

			lastLane = newLane;
		}
	}
}

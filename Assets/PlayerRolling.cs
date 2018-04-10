using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRolling : MonoBehaviour {
	private GameObject player;

	// Use this for initialization
	void Start () {
		player = GameObject.Find("Player");
	}
	
	// Update is called once per frame
	void Update () {
		Transform transf = player.GetComponent<Transform>();
		transf.RotateAround(transf.position, new Vector3(1f, 0f, 0f), 2f);
	}
}

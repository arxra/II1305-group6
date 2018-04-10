using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisonDetect : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.name == "player")
        {
            Debug.Break();
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}

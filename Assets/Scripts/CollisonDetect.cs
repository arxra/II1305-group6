using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisonDetect : MonoBehaviour {

    public GameObject Player;

	// Use this for initialization
	void Start () {
		
	}
    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.Equals(Player))
        {
            SceneManager.LoadScene(1);
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}

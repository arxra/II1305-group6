using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickup : MonoBehaviour {
	public string countText;
	private int count;


	// Use this for initialization
	void Start () {
		count = 0;
		SetCountText ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter(Collision other) 
	{
		if (other.collider.gameObject.CompareTag ("Collectable"))
		{
			other.gameObject.SetActive (false);
			count++;
			SetCountText();
		}
	}

	void SetCountText () {
		countText = "Count: " + count.ToString(); 
	}

	void OnGUI () {
		GUI.Box (new Rect (0, 50, Screen.width, Screen.height - 50), countText);
	}
}

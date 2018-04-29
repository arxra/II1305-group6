using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
	void Start(){
		FindObjectOfType<AudioManager> ().play ("Menu");
	}

	public void PlayGame(){
		SceneManager.LoadScene (1);
		FindObjectOfType<AudioManager> ().mute ("Menu");


	}

	public void QuitGame(){

		Debug.Log ("Quit");
		Application.Quit ();
	}
}

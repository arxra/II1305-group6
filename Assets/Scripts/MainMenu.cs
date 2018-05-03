using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

	//public Text highscoreText;

	public bool soundOn = true;
	void Start(){
		FindObjectOfType<AudioManager> ().play ("Menu");
		//highscoreText.text = "Highscore: " + PlayerPrefs.GetFloat ("highscore");
	}

	void Update(){
		if (!soundOn) {
			FindObjectOfType<AudioManager> ().mute ("Menu");
		}
	
	}

	public void PlayGame(){
		SceneManager.LoadScene (1);
		FindObjectOfType<AudioManager> ().mute ("Menu");


	}

	public void MuteSound(bool toggle){
		if (!toggle) {
			FindObjectOfType<AudioManager> ().mute ("Music");
			soundOn = false;
		}

	}

	public void QuitGame(){

		//Debug.Log ("Quit");
		Application.Quit ();
	}
}

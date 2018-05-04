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

	public void Upgrades() {
		upgradesAnim.SetTrigger("UpgradesPressed");
		abortUpgrades.SetActive(true);
		foreach(Upgrades.Upgrade u in upgradesScript.CurrentUpgrades()) {
			var upgradeGO = upgrades.transform.Find(u._name);
			
			if (upgradeGO != null)
				upgradeGO.Find("Price").GetComponent<Text>().text = u._cost + ":-";
		}
		
	}

	public void AbortUpgrades() {
		upgradesAnim.SetTrigger("Abort");
		abortUpgrades.SetActive(false);
	}

	public void SetBackgroundMusicEnabled(bool enabled) {
		
	}

	public void Update() {
		if (playedPressed) {
			screenFadeTimer -= Time.deltaTime;

			if (screenFadeTimer <= 0f)
				SceneManager.LoadScene(1);
			
			float vel = (1.0f - (BlackoutTime - screenFadeTimer) / BlackoutTime) * Time.deltaTime * 2f;


			screenFade.color = new Color(
				screenFade.color.r, 
				screenFade.color.g, 
				screenFade.color.b, 
				screenFade.color.a + vel
			);
		}
	}
}

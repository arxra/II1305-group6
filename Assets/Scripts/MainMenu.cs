using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

	private GameObject upgrades;

	private GameObject abortUpgrades;
	private Animator playAnim, upgradesAnim;

	private Upgrades upgradesScript;
	private const float BlackoutTime = 2f;

	private bool playedPressed = false;
	

	private Image screenFade;
	private float targetAngle = 180f;

	private float screenFadeTimer = BlackoutTime;

	public void Start() {
		upgrades = GameObject.Find("Foreground").transform.Find("Upgrades").gameObject;
		playAnim = GameObject.Find("Play").GetComponent<Animator>();
		upgradesAnim = upgrades.GetComponent<Animator>();
		abortUpgrades = upgrades.transform.Find("Abort").gameObject;
		upgradesScript = upgrades.GetComponent<Upgrades>();
		abortUpgrades.SetActive(false);
		screenFade = GameObject.Find("ScreenFade").GetComponent<Image>();
	}	

	public void PlayGame(){
		playAnim.SetTrigger("PlayedPressed");
		playedPressed = true;
	}

	public void QuitGame(){
		Debug.Log ("Quit");
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

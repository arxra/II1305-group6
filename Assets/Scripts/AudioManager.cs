using UnityEngine.Audio;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
public class AudioManager : MonoBehaviour {


	public Sound[] music;



	void Awake () {

		// for every track set settings value
		foreach (Sound s in music) {

			s.source = gameObject.AddComponent<AudioSource> ();
			s.source.clip = s.clip;
			s.source.volume = s.volume;
			s.source.pitch = s.pitch;
			s.source.loop = s.loop;
		}
		
	}

	void Start(){
		if (SceneManager.GetActiveScene ().name == "MainGame")
			play ("Music");
	}

	void Update(){
		if (Input.GetKeyDown (KeyCode.N)) {
			unMute ("Music");
		}
	
		if (Input.GetKeyDown (KeyCode.M)) {
			mute ("Music");
		}
	}
	public void unMute(string name){
		Sound s = Array.Find (music, sound => sound.name == name);
		s.source.volume = s.volume;
	}
	public void mute(string name){
		Sound s = Array.Find (music, sound => sound.name == name);
		s.source.volume = 0;
	
	}
	
	public void play(string name){

		Sound s = Array.Find (music, sound => sound.name == name);
		if (s == null)
			return;
		s.source.Play ();
	
	}

}

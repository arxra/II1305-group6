using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**********************
 * To do
 * add audio source for bg music (independent of this script) playOnAwake and loop
 * add audio source component for player
 * add sound for jump, dash, collect, running, hit obstacle and name accordingly
 * Set up "PlayerSounds sound;" and init with "sound = GetComponent<PlayerSounds> ();" in all files that will use sounds
 * Call sound function with "sound.playSound("name");" sound will be played once
 * Call playSoundOnRepeat with "sound.playSoundOnRepeat("name", true);" to play a sound on repeat
*/

public class PlayerSounds : MonoBehaviour {
	AudioSource playerAudio;
	AudioSource playOnce;
	GameObject player;
	public List<SoundEffect> sounds;
	bool playOnRepeat;

	// Use this for initialization
	void Start () {
		playerAudio = GetComponent<AudioSource> ();
		playOnRepeat = false;
	}
	


	// Update is called once per frame
	void Update () {
		if (!playerAudio.isPlaying && playOnRepeat)
			playerAudio.Play ();
	}

	public void playSound(string name) {
		playOnce.clip = sounds.Find(item => item.name == name).clip;
		playOnce.Play ();
	}

	public void playSoundOnRepeat(string name, bool play) {
		if (play) {
			playerAudio.clip = sounds.Find (item => item.name == name).clip;
			playOnRepeat = true;
		}
		else {
			playOnRepeat = false;
		}
	}
}

[System.Serializable]
public class SoundEffect {
	public string name;

	public AudioClip clip;
}

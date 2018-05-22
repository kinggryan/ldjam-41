using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class TransitionAndName {
	public string name;
	public MusicTransition transition;
}

public class MusicEngine : MonoBehaviour {

	public AudioMixerSnapshot paused;
	public AudioMixerSnapshot unpaused;
	public float pauseTransitionTime;
	public TransitionAndName[] events;

		/* void Awake(){
		GameObject.DontDestroyOnLoad(gameObject);
		} */
    
	public void ChangeMusicWithName(string name) {
		var playedSound = false;
		foreach (var eventAndName in events) {
			if (eventAndName.name == name) {
				eventAndName.transition.ChangeMusic();
				playedSound = true;
			}
		}

		// Debugging thing
		if (!playedSound) {
			Debug.LogError("Couldn't find sound for name: " + name);
		}
	}

	public void PauseMusic(){
		paused.TransitionTo(pauseTransitionTime);
	}

	public void UnpauseMusic(){
		unpaused.TransitionTo(pauseTransitionTime);
		
	}



}

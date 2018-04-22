using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TransitionAndName {
	public string name;
	public MusicTransition transition;
}

public class MusicEngine : MonoBehaviour {

	public TransitionAndName[] events;

		void Awake(){
		GameObject.DontDestroyOnLoad(gameObject);
	}
    
	public void ChangeMusicWithName(string name) {
		print("Picking sound");
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



}

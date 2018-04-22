using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class EventAndName {
	public string name;
	public SoundEvent soundEvent;
}

public class SoundEngine : MonoBehaviour {
	static SoundEngine instance = null;

	public EventAndName[] events;

	void Awake ()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            print ("Duplicate SoundEngine Destroyed!");
        }
        else 
        {
            instance = this;
        }

        GameObject.DontDestroyOnLoad(gameObject);
    }
    

	public void PlaySoundWithName(string name) {
		var playedSound = false;
		foreach (var eventAndName in events) {
			if (eventAndName.name == name) {
				eventAndName.soundEvent.PlaySound ();
				playedSound = true;
			}
		}

		// Debugging thing
		if (!playedSound) {
			Debug.LogError("Couldn't find sound for name: " + name);
		}
	}
}

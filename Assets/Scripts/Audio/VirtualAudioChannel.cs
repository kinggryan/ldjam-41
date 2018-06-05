using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualAudioChannel : MonoBehaviour {

		// Use this for initialization
	public void OnSoundPlayed (AudioSource lastSoundPlayed){
		AudioSource[] sources = GetComponentsInChildren<AudioSource>();
		foreach (AudioSource source in sources){
			if (source != lastSoundPlayed){
				source.Stop();
				Debug.Log("Sound " + source.name + "stopped");
			}
		}
	}
}
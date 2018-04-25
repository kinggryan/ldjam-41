using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMusic : MonoBehaviour {

	private SoundEngine soundEngine;
	private MusicEngine musicEngine;

	// Use this for initialization
	void Start () {
		soundEngine = Object.FindObjectOfType<SoundEngine>();
		musicEngine = Object.FindObjectOfType<MusicEngine>();

		soundEngine.PlaySoundWithName("Start");
		musicEngine.ChangeMusicWithName("MelodyUp");
	}
	
}

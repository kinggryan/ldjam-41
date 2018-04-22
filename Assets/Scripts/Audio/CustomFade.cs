using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class CustomFade : MonoBehaviour {

	private AudioLowPassFilter filter;
	[Range(0.0f, 1.0f)]
	public float startVolume;
	[Range(0.0f, 1.0f)]
	public float endVolume;
	[Range(10f, 22000f)]
	public float startCutoff;
	[Range(10f, 22000f)]
	public float endCutoff;
	public float transitionTime;

	private bool fade = false;

	private SoundEvent source;

	// Use this for initialization
	void Start () {
		filter = gameObject.AddComponent<AudioLowPassFilter>();
		filter.cutoffFrequency = startCutoff;
		source = GetComponent<SoundEvent>();
		source.volume = startVolume;
	}
	
	// Update is called once per frame
	void Update () {
		
		if (fade){
			filter.cutoffFrequency += (Time.deltaTime / transitionTime) * (endCutoff - startCutoff);
			source.externalVolumeModifier += (Time.deltaTime / transitionTime) * (endVolume - startVolume);
			if (filter.cutoffFrequency >= endCutoff && source.externalVolumeModifier >= endVolume){
				fade = false;
				print("custom fade complete");
				Destroy(this);
			}
		}
		if (!source){
			print ("Sound Event = null");
			source = gameObject.GetComponent<SoundEvent>();
			source.externalVolumeModifier = startVolume;
		}
		if (!filter){
			print ("filter = null");
			filter = gameObject.AddComponent<AudioLowPassFilter>();
			filter.cutoffFrequency = startCutoff;
		}
		

	}

	void OnLevelWasLoaded(int level) {
        print("Loaded level " + level);
		if (level == 1){
            fade = true;
		}
	}

}

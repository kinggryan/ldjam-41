using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class VolumeSlider : MonoBehaviour {
	public AudioMixer audioMixer;
	public Slider slider;
	public string paramName;

	// Use this for initialization
	void Start () {
		slider.value = getParamValue();
	}

	public void changeVolumeLevel (float sliderValue){
		audioMixer.SetFloat(paramName, slider.value);
	}

	public float getParamValue(){
		float value;
        bool result =  audioMixer.GetFloat(paramName, out value);
        if(result){
             return value;
         }else{
             return 0f;
         }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

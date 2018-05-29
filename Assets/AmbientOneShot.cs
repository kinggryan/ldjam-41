using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientOneShot : MonoBehaviour {
	public SoundEvent sound;
	public bool startOnAwake;
	public float minTime;
	public float maxTime;
	// Use this for initialization
	void Start () {
		if (startOnAwake){
			StartCoroutine(WaitARandomAmountOfTimeThenPlayTheSound(minTime,maxTime));
		}
	}
	
	public IEnumerator WaitARandomAmountOfTimeThenPlayTheSound(float timeMin,float timeMax){
		float randomTime = Random.Range(timeMin, timeMax);
		Debug.Log("waiting " + randomTime
		 + " secs before playing sound");
		yield return new WaitForSeconds(randomTime);
		sound.PlaySound();
		StartCoroutine(WaitARandomAmountOfTimeThenPlayTheSound(timeMin,timeMax));
	}

}

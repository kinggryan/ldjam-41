using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicTransition : MonoBehaviour {

	public SoundEvent source;
	public SoundEvent destination;

	public float transitionTime;
	public bool crossfade;
	public bool verticalLayer;
	public float vertTargetVol;
	public float vertStartVol;
	public float fadeAfterTime;
	public bool fadeAfterInterval;
	public float timer = 0;
	public bool transition = false;

	public void ChangeMusic(){
		print("changing music");
		transition = true;
		if (!crossfade && !verticalLayer){
			source.StopSound();
		}
	}

	

	// Use this for initialization
	void Start () {
		source.externalVolumeModifier = 1;
		if (!verticalLayer){
			destination.externalVolumeModifier = 0;
		}
		else {
			 destination.externalVolumeModifier = vertStartVol;
			print("vol mod: " + destination.externalVolumeModifier);
		}
	}


	
	// Update is called once per frame
	void Update () {

		/*if (Input.GetKeyDown("space")){
            ChangeMusic();
        }*/
		if (transition){
			if (crossfade){
				source.externalVolumeModifier -= Time.deltaTime / transitionTime;
				destination.externalVolumeModifier += Time.deltaTime / transitionTime;
				if (source.externalVolumeModifier <= 0){
					transition = false;
				}
			} 
			else if (verticalLayer){
				if (vertTargetVol >= vertStartVol){
					//Debug.Log("Turning UP");
					if (destination.externalVolumeModifier >= vertTargetVol && fadeAfterTime <= 0){
						transition = false;
					}
					else if (timer >= fadeAfterTime && fadeAfterInterval){
						destination.externalVolumeModifier -= (Time.deltaTime / transitionTime) * (vertTargetVol - vertStartVol);

						if (destination.externalVolumeModifier <= 0){
							timer = 0;
							destination.externalVolumeModifier = 0;
							transition = false;
						}
					}
					else if (destination.externalVolumeModifier >= vertTargetVol && fadeAfterInterval){
						timer += (Time.deltaTime / fadeAfterTime);
					}
					else {
						destination.externalVolumeModifier += (Time.deltaTime / transitionTime) * (vertTargetVol - vertStartVol);
					}
				}else if (vertTargetVol < vertStartVol){
					if (destination.externalVolumeModifier <= vertTargetVol && fadeAfterTime <= 0){
						transition = false;
					}
					else if (timer >= fadeAfterTime && fadeAfterInterval){
						destination.externalVolumeModifier -= (Time.deltaTime / transitionTime) * (vertTargetVol - vertStartVol);

						if (destination.externalVolumeModifier <= 0){
							timer = 0;
							destination.externalVolumeModifier = 0;
							transition = false;
						}
					}
					else if (destination.externalVolumeModifier <= vertTargetVol){
						timer += (Time.deltaTime / fadeAfterTime);
					}
					else {
						destination.externalVolumeModifier += (Time.deltaTime / transitionTime) * (vertTargetVol - vertStartVol);
					}
				}
			}
			else if (!verticalLayer){
				if (source.fadeOutTimer <= 0.02f){
					destination.externalVolumeModifier = vertTargetVol;
					destination.PlaySound();
					transition = false;
				}

			}
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class FadeOutAndLoadLevel : MonoBehaviour {

	public float fadeTime;
	public string levelToLoad;
	private Image image;
	private bool fading;
	private Color imageStartColor;
	private float alphaChange;
	private CRTFlicker crtFlicker;
	void Awake(){
		image = GetComponent<Image>();
		imageStartColor = image.color;
		alphaChange = ((1 - imageStartColor.a) * (Time.deltaTime / fadeTime)); 
		crtFlicker = GetComponent<CRTFlicker>();
		Debug.Log("imgalpha: " + imageStartColor.a);
		
	}
	// Use this for initialization
	public void FadeAndLoadLevel(){
		crtFlicker.enabled = !crtFlicker.enabled;
		Debug.Log("FADING");
		imageStartColor = image.color;
		alphaChange = ((1 - imageStartColor.a) * (Time.deltaTime / fadeTime)); 
		fading = true;
	}

	void Update(){
		if (fading){
			/*if (crtFlicker.enabled){
				crtFlicker.enabled = !crtFlicker.enabled;
			}*/
			image.color = image.color + new Color(0,0,0,alphaChange);
			if (image.color.a >= 1){
				Debug.Log("Fade Complete");
				LoadWinScene();
				fading= false;
			}
		}
	}

	void LoadWinScene(){
		SceneManager.LoadScene(levelToLoad);
	}

}

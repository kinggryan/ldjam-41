using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashScreen : MonoBehaviour {
    
    private	FadeOutAndLoadLevel screenDarken;
    
    public float waitTime;
    public float fadeTime;

	// Use this for initialization
	void Start () {
        screenDarken = GameObject.FindGameObjectWithTag("ScreenDarken").GetComponent<FadeOutAndLoadLevel>();
        //screenDarken.fadeTime = fadeTime;
		StartCoroutine("StartGame");
        
	}
	
	IEnumerator StartGame()
    {
        yield return new WaitForSeconds(waitTime);
        screenDarken.FadeAndLoadLevel();
    }
}

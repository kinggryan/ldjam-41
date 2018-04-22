using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashScreen : MonoBehaviour {
    
    public LevelManager levelManager;
    
    public float waitTime;

	// Use this for initialization
	void Start () {
		StartCoroutine("StartGame");
	}
	
	IEnumerator StartGame()
    {
        yield return new WaitForSeconds(waitTime);
        levelManager.LoadLevel("01a Start", 2f);
    }
}

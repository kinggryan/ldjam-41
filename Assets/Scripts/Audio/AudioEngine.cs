using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioEngine : MonoBehaviour {

	static AudioEngine instance = null;
    private MusicEngine musicEngine;

	
	void Awake ()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            print ("Duplicate AudioEngine Destroyed!");
        }
        else 
        {
            instance = this;
        }

        GameObject.DontDestroyOnLoad(gameObject);
    }

    void Start(){
        musicEngine = Object.FindObjectOfType<MusicEngine>();
        //musicEngine.ChangeMusicWithName("BassUp");
    }
    
}

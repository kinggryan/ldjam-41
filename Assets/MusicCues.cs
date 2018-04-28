using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cradle;

public class MusicCues : MonoBehaviour {
	private SoundEngine soundEngine;
    private MusicEngine musicEngine;

	// Use this for initialization
	void Start () {

		soundEngine = Object.FindObjectOfType<SoundEngine>();
        musicEngine = Object.FindObjectOfType<MusicEngine>();
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void Garage_Enter(){

		musicEngine.ChangeMusicWithName("KickUp");
	}
	void Garage_Update()
	{
	// Runs every frame like a normal Update method,
	// but only when the current passage is Garage
	}

	void Garage_Exit(){	
		
	}

	[StoryCue("Hack_Logic", "Enter")]
	void enterHack(){
		// ...
	}

	[StoryCue("Hack_Logic", "Exit")]
	void exitHack(){
		// ...
	}

	void Warehouse_Enter(){

	
	}

	void Warehouse_Update()
	{
	// Runs every frame like a normal Update method,
	// but only when the current passage is Garage
	}

	void Warehouse_Exit(){	
		
	}

}

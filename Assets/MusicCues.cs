using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cradle;

public class MusicCues : MonoBehaviour {
	public SoundEngine soundEngine;
    private MusicEngine musicEngine;
	private bool playedTitleEnterSound = false;
	private bool firstHackEntered = false;
	private bool firstLookEntered = false;

	// Use this for initialization
	void Start () {

		soundEngine = Object.FindObjectOfType<SoundEngine>();
        musicEngine = Object.FindObjectOfType<MusicEngine>();
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void Title_Enter(){
		Debug.Log("Title Enter");
	}

	void Title_Update(){
		if(soundEngine != null && musicEngine != null && !playedTitleEnterSound){
			soundEngine.PlaySoundWithName("ImproperShutdown");
			musicEngine.ChangeMusicWithName("PadsUp");
			musicEngine.ChangeMusicWithName("BassUp");
			musicEngine.ChangeMusicWithName("RoomToTank");
			//musicEngine.ChangeMusicWithName("BassUp");
			playedTitleEnterSound = true;
		}
	}

	/*
	void Title_Exit(){
		Debug.Log("Title Exit");
		if(soundEngine != null){
			soundEngine.PlaySoundWithName("SystemCheckComplete");
		}else{
			Debug.Log("SoundEngine missing");
		}
	}
	*/

	void Garage_Enter(){
		Debug.Log("Garage Enter");
		musicEngine.ChangeMusicWithName("MelodyUp");
		soundEngine.PlaySoundWithName("OpenHatch");
		musicEngine.ChangeMusicWithName("RoomToTank");
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
		if(!firstHackEntered){
			Debug.Log("1st HACK ENTER");
			soundEngine.PlaySoundWithName("SystemCheckComplete");
			musicEngine.ChangeMusicWithName("BassUp");
			firstHackEntered = true;
		}
		Debug.Log("HACK ENTER");
		// ...
	}

	[StoryCue("Look_Logic", "Enter")]
	void enterLook(){
		if(!firstLookEntered){
			Debug.Log("1st Look ENTER");
			soundEngine.PlaySoundWithName("EnterCommand");
			musicEngine.ChangeMusicWithName("KickUp");
			firstLookEntered = true;
		}
		Debug.Log("LOOK ENTER");
		// ...
	}


	void Warehouse_Enter(){
		Debug.Log("WAREHOUSE ENTERED");
		soundEngine.PlaySoundWithName("WarehouseEnter");
	
	}

	void Warehouse_Update()
	{
	// Runs every frame like a normal Update method,
	// but only when the current passage is Garage
	}

	void Warehouse_Exit(){	
		Debug.Log("WAREHOUSE EXIT");
	}

}

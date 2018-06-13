using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Cradle;

public class MusicCues : MonoBehaviour {
	public SoundEngine soundEngine;
    private MusicEngine musicEngine;

	void Awake () {

		soundEngine = Object.FindObjectOfType<SoundEngine>();
        musicEngine = Object.FindObjectOfType<MusicEngine>();
	}


	[StoryCue("Title", "Enter")]
	void TitleEnter(){
		Debug.Log("Title ENTER");
		if (soundEngine != null){
			soundEngine.PlaySoundWithName("ImproperShutdown");
			
		}
		if (musicEngine != null){
			Debug.Log("Getting current music snapshot");
			musicEngine.mixerSnapshots[0].TransitionTo(musicEngine.pauseTransitionTime);
			musicEngine.currentMusicSnapshot = musicEngine.mixerSnapshots[2];
		}
	}
	[StoryCue(" HackTitle", "Enter")]
	void HackTitleEnter(){
		Debug.Log("HackTitle Enter");
		soundEngine.PlaySoundWithName("SystemCheckComplete");
		musicEngine.currentMusicSnapshot = musicEngine.mixerSnapshots[3];
	}

	[StoryCue(" LookTitle", "Enter")]
	void LookTitleEnter(){
		Debug.Log("LookTitle Enter");
		soundEngine.PlaySoundWithName("SystemCheckComplete");
		musicEngine.currentMusicSnapshot = musicEngine.mixerSnapshots[4];
		
	}
	
	[StoryCue(" Garage", "Enter")]
	void GarageEnter(){
		Debug.Log("Garage Enter");
		soundEngine.PlaySoundWithName("OpenHatch");
		musicEngine.currentMusicSnapshot = musicEngine.mixerSnapshots[5];
	}
	
	[StoryCue(" LookGarage", "Enter")]
	void LookGarageEnter(){
		Debug.Log("Look Garage Enter");
		soundEngine.PlaySoundWithName("DeskLook");
	}
	
	[StoryCue(" NorthGarage", "Enter")]
	void NorthGarageEnter(){
		Debug.Log("North Garage Enter");
	}
	[StoryCue("Warehouse", "Enter")]
	void WarehouseEnter(){
		Debug.Log("Warehouse Enter");
		soundEngine.PlaySoundWithName("DoorOpen");
		musicEngine.currentMusicSnapshot = musicEngine.mixerSnapshots[5];
	}
	[StoryCue(" LookWare", "Enter")]
	void LookWareEnter(){
		Debug.Log("LookWare Enter");
	}
	[StoryCue(" GetCoat", "Enter")]
	void GetCoatEnter(){
		Debug.Log("GetCoat Enter");
	}
	[StoryCue(" Server", "Enter")]
	void ServerEnter(){
		Debug.Log("Server Enter");
		soundEngine.PlaySoundWithName("DoorOpen");
		musicEngine.currentMusicSnapshot = musicEngine.mixerSnapshots[6];
	}	
	
	[StoryCue(" HackServ", "Enter")]
	void HackServEnter(){
		Debug.Log("HackServ Enter");
		soundEngine.PlaySoundWithName("SystemCheckComplete");
		musicEngine.currentMusicSnapshot = musicEngine.mixerSnapshots[7];
	}

	[StoryCue(" UseCure", "Enter")]
	void UseCureEnter(){
		Debug.Log("UseCure Enter");
		soundEngine.PlaySoundWithName("Cure");
		musicEngine.currentMusicSnapshot = musicEngine.mixerSnapshots[6];
	
	}

	[StoryCue(" Security", "Enter")]
	void SecurityEnter(){
		Debug.Log("Server Enter");
		soundEngine.PlaySoundWithName("DoorOpen");
		soundEngine.PlaySoundWithName("GuardAlert");
		musicEngine.currentMusicSnapshot = musicEngine.mixerSnapshots[8];
	}
	
	[StoryCue(" LookSec", "Enter")]
	void LookSecEnter(){
		Debug.Log("LookSec Enter");
		soundEngine.PlaySoundWithName("GuardShoot");
	}

	[StoryCue("Lab", "Enter")]
	void LabEnter(){
		Debug.Log("Lab Enter");
		soundEngine.PlaySoundWithName("Elevator");
		musicEngine.currentMusicSnapshot = musicEngine.mixerSnapshots[10];
	}
	[StoryCue("AI", "Enter")]
	void AIEnter(){
		Debug.Log("AI Enter");
		soundEngine.PlaySoundWithName("Kapcha");
	} 
	[StoryCue("AI_Win", "Enter")]
	void AI_WinEnter(){
		Debug.Log("AI_Win Enter");
		soundEngine.PlaySoundWithName("CleanroomDoor");
		musicEngine.currentMusicSnapshot = musicEngine.mixerSnapshots[11];
	}
	[StoryCue(" Clean", "Enter")]
	void CleanEnter(){
		Debug.Log("Clean Enter");
		soundEngine.PlaySoundWithName("CleanroomDoor");
		musicEngine.currentMusicSnapshot = musicEngine.mixerSnapshots[12];
	}
	[StoryCue(" End", "Enter")]
	void EndEnter(){
		Debug.Log("End Enter");
		soundEngine.PlaySoundWithName("Hack");
	}


}

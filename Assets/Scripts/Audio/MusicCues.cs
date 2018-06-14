using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Cradle;

public class MusicCues : MonoBehaviour {
	public SoundEngine soundEngine;
    private MusicEngine musicEngine;
	private TwineTextPlayer twineTextPlayer;
	private	FadeOutAndLoadLevel screenDarken;
	

	void Awake () {

		soundEngine = Object.FindObjectOfType<SoundEngine>();
        musicEngine = Object.FindObjectOfType<MusicEngine>();
	}

	bool GetTwineBool(string boolToGet){
		return twineTextPlayer.GetTwineVarState(boolToGet);
	}


	[StoryCue("Title", "Enter")]
	void TitleEnter(){
		Debug.Log("Title ENTER");
		twineTextPlayer = Object.FindObjectOfType<TwineTextPlayer>();
		if (twineTextPlayer != null){
			Debug.Log("TwineTextPlayer found");
		}else{
			Debug.Log("TwineTextPlayer NOT found");
		}
		if (soundEngine != null){
			soundEngine.PlaySoundWithName("ImproperShutdown");
			soundEngine.PlaySoundWithName("FirstText");
			
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
		musicEngine.currentMusicSnapshot = musicEngine.mixerSnapshots[13];
	}
	[StoryCue(" LookWare", "Enter")]
	void LookWareEnter(){
		Debug.Log("LookWare Enter");
		if(!twineTextPlayer.Story.Vars["inv"]["GUN"]){
			Debug.Log("Gun: False");
			soundEngine.PlaySoundWithName("GetGun");
		}else{
			Debug.Log("Gun: True");
		}
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
		Debug.Log("Secutiry Enter");
		if (GetTwineBool("dead_g")){
			Debug.Log("Guard: Dead");
			musicEngine.currentMusicSnapshot = musicEngine.mixerSnapshots[9];
		}else if (twineTextPlayer.Story.Vars["inv"]["COAT"] || GetTwineBool("ser_door"))
			{
			Debug.Log("Guard: Friendly");
			soundEngine.PlaySoundWithName("GuardAlert");
			musicEngine.currentMusicSnapshot = musicEngine.mixerSnapshots[9];
		}else{
			Debug.Log("Guard: Aggro");
			musicEngine.currentMusicSnapshot = musicEngine.mixerSnapshots[8];
			soundEngine.PlaySoundWithName("GuardAlert");
		}
		soundEngine.PlaySoundWithName("DoorOpen");
		
		
	}

	[StoryCue(" TalkSec", "Enter")]
	void TalkSecEnter(){
		Debug.Log("TalkSec Enter");
		if (GetTwineBool("dead_g")){
			Debug.Log("Guard: Dead");
		}else if (GetTwineBool("guard"))
			{
			Debug.Log("Guard: Aggro");
			soundEngine.PlaySoundWithName("GuardShoot");
		}else{
			Debug.Log("Guard: Friendly");
			musicEngine.currentMusicSnapshot = musicEngine.mixerSnapshots[9];
		}
	}

	[StoryCue(" UseGun", "Enter")]
	void UseGunEnter(){
		Debug.Log("UseGun Enter");
		soundEngine.PlaySoundWithName("PlayerShoot");
		musicEngine.currentMusicSnapshot = musicEngine.mixerSnapshots[9];
	}
	
	[StoryCue(" LookSec", "Enter")]
	void LookSecEnter(){
		Debug.Log("LookSec Enter");
		if (GetTwineBool("guard")){
			Debug.Log("Guard: Aggro");
			soundEngine.PlaySoundWithName("GuardShoot");
		}else{
			Debug.Log("Guard: Friendly");
		}
	}

	[StoryCue("Lab", "Enter")]
	void LabEnter(){
		Debug.Log("Lab Enter");
		soundEngine.PlaySoundWithName("Elevator");
		musicEngine.currentMusicSnapshot = musicEngine.mixerSnapshots[10];
	}
	[StoryCue(" AI", "Enter")]
	void AIEnter(){
		Debug.Log("AI Enter");
		if (!GetTwineBool("open_clean")){
			soundEngine.PlaySoundWithName("Kapcha");
		}
	} 
	[StoryCue("AI_Win", "Enter")]
	void AI_WinEnter(){
		Debug.Log("AI_Win Enter");
		soundEngine.PlaySoundWithName("CleanroomDoor");
		musicEngine.currentMusicSnapshot = musicEngine.mixerSnapshots[11];
	}
	[StoryCue("AIGun", "Enter")]
	void AIGun(){
		Debug.Log("AIGun Enter");
		soundEngine.PlaySoundWithName("SystemCheckComplete");
	}
	
	[StoryCue("AICoat", "Enter")]
	void AICoat(){
		Debug.Log("AICoat Enter");
		soundEngine.PlaySoundWithName("SystemCheckComplete");
	}
	[StoryCue("AIFob", "Enter")]
	void AIFob(){
		Debug.Log("AIFob Enter");
		soundEngine.PlaySoundWithName("SystemCheckComplete");
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
		musicEngine.mixerSnapshots[14].TransitionTo(27);
		musicEngine.currentMusicSnapshot = musicEngine.mixerSnapshots[14];
		Debug.Log("YOU WON");
        screenDarken = GameObject.FindGameObjectWithTag("ScreenDarken").GetComponent<FadeOutAndLoadLevel>();
		if (screenDarken != null){
			Debug.Log("Darkened found");
		}
        screenDarken.FadeAndLoadLevel();
	}


}

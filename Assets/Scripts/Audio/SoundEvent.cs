using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundEvent : MonoBehaviour {


public bool random;
public bool playOnAwake;
[Range(0.0f, 1.0f)]
public float volume;
[Range(-12.0f, 12.0f)]
public float pitch;
[Range(-12.0f, 12.0f)]
public float pitchRandomization;
public float fadeInTime;
private float fadeInTimer = 0;
private bool fadeIn = false;
public float fadeOutTime;
public float fadeOutTimer = 1;
private bool fadeOut = false;
public double startDelay;

public AudioClip[] audioClip;
    
private int clip = 0;
private int randomClip;
public float waitTime;
public float waitRange;
private float actualPitch;
public Transform listener;
public bool threeDee;
public float threeDeeMultiplier = 1;
public float panMultiplier;
public float externalVolumeModifier;
public float externalPitchModifier = 0;
public AudioMixerGroup output;
//private AudioSource audioSource;
public int clipToLoop;
    
private float playerDistance;
public float playerXDistance;
private float playerYDistance;
private Transform transform;
private AudioSource nextClipToPlay;
public enum Type
{
    Loop,
    Sequence,
    OneShot,
}

public Type type;
    
private bool soundPlayed = false;

public double timer;
private VirtualAudioChannel audioChannel;

    void Awake(){
        audioChannel = gameObject.transform.parent.GetComponent<VirtualAudioChannel>();
    }

   void Start ()
    {
        transform = gameObject.GetComponent<Transform>();
        PrepareFIrstSoundToPlay();
        if (playOnAwake)
        {
            PlaySound();
        }
        listener = GameObject.FindObjectOfType<Camera>().transform;
        
    }
    

    public void PlaySound ()
    {
        //nextClipToPlay.Play();
        double clipStartTime = AudioSettings.dspTime + startDelay+ Time.deltaTime;
        double clipEndTime = clipStartTime + nextClipToPlay.clip.length;
        double clipLength = nextClipToPlay.clip.length;
        if (audioChannel != null){
            audioChannel.OnSoundPlayed(nextClipToPlay);
        }
        nextClipToPlay.PlayScheduled(clipStartTime);
        //nextClipToPlay.SetScheduledEndTime(clipEndTime);
        //Debug.Log("Current clip end time: " + clipEndTime);
        //Debug.Log("Current clip length: " + clipLength);
        //Debug.Log("Played sound " + nextClipToPlay);
        soundPlayed = true;
        PrepareNextSoundToPlay(clipStartTime, clipEndTime);
        
    }

    void PrepareFIrstSoundToPlay(){
        AudioSource source = gameObject.AddComponent<AudioSource>();
        source.clip = audioClip[0];
        source.outputAudioMixerGroup = output;
        SetStartVolume(source);
        SetPitch(source);
        nextClipToPlay = source;
    }

    void PrepareNextSoundToPlay(double currentClipStartTime, double currentClipEndTime){
        //Debug.Log("Preparing sound");
        AudioSource source = gameObject.AddComponent<AudioSource>();
        source.outputAudioMixerGroup = output;
        SelectNextClip(source);
        SetStartVolume(source);
        SetPitch(source);
        nextClipToPlay = source;
        if (type == Type.Loop || type == Type.Sequence && soundPlayed){
            ScheduleNextClipToPlayAtEndOfCurrentClip(source, currentClipStartTime, currentClipEndTime);
        }

    }

    void SetStartVolume(AudioSource source){
        if(fadeInTime > 0){
            fadeIn = true;
            source.volume = 0;
        }
        else {
            source.volume = volume * externalVolumeModifier;
        }
    }

    void SelectNextClip(AudioSource source){
        if (random)
        {
            randomClip = Random.Range(0, audioClip.Length);
            checkIfSameAsLast(clip, randomClip);
            clip = randomClip;

        }
        else
        {
            if (type == Type.Loop){
                if (clip == clipToLoop){

                }else{
                    clip += 1;
                }
            }
        }
        source.clip = audioClip[clip];
    }

    void SetPitch(AudioSource source){
        actualPitch = pitch + externalPitchModifier + Random.Range(-pitchRandomization, pitchRandomization);
        source.pitch = Mathf.Pow(1.05946f, actualPitch);
    }
     
    public void StopSound(){
        if(fadeOutTime > 0){
            fadeOut = true;
        }else{
            AudioSource[] audioSources = GetComponents<AudioSource>();
            foreach (AudioSource audioSource in audioSources)
            {
                audioSource.Stop();
                Destroy(audioSource);
            }
        }
    }
    
    void checkIfSameAsLast(int last, int current)
    {
        if (last == current && !random)
        {
            randomClip = Random.Range(0, audioClip.Length);
            checkIfSameAsLast(last, randomClip);
        }
    }
    
 

    double CurrentSample(){
        double currentDspTime = AudioSettings.dspTime;
        double sampleRate = AudioSettings.outputSampleRate;
        double sample = AudioSettings.dspTime * sampleRate;
        //Debug.Log("DSP Time: " + currentDspTime);
        //Debug.Log("Sample Rate: " + sampleRate);
        //Debug.Log("Current Sample: " + sample);
        return sample;
    }

    void ScheduleNextClipToPlayAtEndOfCurrentClip(AudioSource source, double clipStartTime, double clipEndTime){
        source.PlayScheduled(clipEndTime);
        //Debug.Log("Sound scheduled to play at " + clipEndTime);
        float endTimeOfNextClipToPlay = (float)clipEndTime + audioClip[clip].length;
        //Destroy(source, audioClip[clip].length + 0.7f);
        clipStartTime = clipEndTime;
        clipEndTime = clipEndTime + audioClip[clip].length;
        StartCoroutine(PrepareSoundAfterDelay(audioClip[clip].length + 0.7f, clipStartTime, clipEndTime));
    }

    IEnumerator PrepareSoundAfterDelay(double delay, double clipStartTime, double clipEndTime){
        //Debug.Log("Waiting for " + delay + "s before preparing sound");
        
        float delayTime = (float)delay;
        yield return new WaitForSeconds(delayTime);
        PrepareNextSoundToPlay(clipStartTime, clipEndTime);
        DestroyNonPlayingAudioSources();

    }

    void DestroyNonPlayingAudioSources(){
        AudioSource[] audioSources = GetComponents<AudioSource>();
            foreach (AudioSource audioSource in audioSources)
            {
                if (!audioSource.isPlaying){
                    Destroy(audioSource);
                }
            }
    }

    void Update()
    {
        //timer = AudioSettings.dspTime;

        if (fadeIn){
                fadeInTimer += Time.deltaTime / fadeInTime;
                //print("fadeInTimer: " + fadeInTimer);
            if (fadeInTimer >= 1)
            {
                //print("Fade done");
                fadeInTimer = 0;
                fadeIn = false;
            }
        }
        else if (fadeOut){
                fadeOutTimer -= Time.deltaTime / fadeOutTime;
                //print("fadeOutTimer: " + fadeOutTimer);
            if (fadeOutTimer <= 0)
            {
                //print("Fade done");
                fadeOutTimer = 1;
                //soundPlayed = false;
                //Destroy(audioSource);
                fadeOut = false;
            }
        }
        
        if(listener != null){
        playerXDistance = transform.position.x - listener.position.x;
        playerYDistance = transform.position.y - listener.position.y;
        }
        if (Mathf.Abs(playerXDistance) >= Mathf.Abs(playerYDistance))
        {
            playerDistance = Mathf.Abs(playerXDistance);
        }
        else
        {
            playerDistance = Mathf.Abs(playerYDistance);
        }
        
        
        float threeDeeVolume = 1 / (playerDistance * threeDeeMultiplier);
        
        AudioSource[] audioSources = GetComponents<AudioSource>();
            foreach (AudioSource audioSource in audioSources)
            {
                if (soundPlayed && audioSource != null && audioSource.isPlaying)
                {
                    if (threeDee)
                    {
                        audioSource.volume = Mathf.Lerp(0, volume, Mathf.Clamp(volume * threeDeeVolume, 0, volume));
                        audioSource.panStereo = Mathf.Clamp((playerXDistance) * panMultiplier, -1, 1);
                    }
                    else if (threeDee && fadeIn)
                    {
                        audioSource.volume = Mathf.Clamp(volume * threeDeeVolume * fadeInTimer, 0, volume);
                        audioSource.panStereo = Mathf.Clamp((playerXDistance) * panMultiplier, -1, 1);
                    }
                    else if (threeDee && fadeOut)
                    {
                        audioSource.volume = Mathf.Clamp(volume * threeDeeVolume * fadeInTimer, 0, volume);
                        audioSource.panStereo = Mathf.Clamp((playerXDistance) * panMultiplier, -1, 1);
                    }
                    else if (fadeIn)
                    {
                        audioSource.volume = volume * externalVolumeModifier * fadeInTimer;
                    }
                    else if (fadeOut)
                    {
                        audioSource.volume = volume * externalVolumeModifier * fadeOutTimer;
                    }
                    else if (type == Type.Loop)
                    {
                        audioSource.volume = volume * externalVolumeModifier;
                    }
                    else
                    {

                    }
            }

        
        }
    }
    
}

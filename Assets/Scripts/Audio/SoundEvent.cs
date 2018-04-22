using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundEvent : MonoBehaviour {


public bool loop;
public bool random;
public bool sequence;
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
private AudioSource audioSource;
public int elementToLoop;
    
private float playerDistance;
public float playerXDistance;
private float playerYDistance;
private Transform transform;


    
private bool soundPlayed = false;
    
    
    public void PlaySound ()
    {
        AudioSource source = gameObject.AddComponent<AudioSource>();
        source.outputAudioMixerGroup = output;
        soundPlayed = true;
        if(fadeInTime > 0){
            fadeIn = true;
            source.volume = 0;
        }
        else {
            source.volume = volume * externalVolumeModifier;
        }
        if (random)
        {
            randomClip = Random.Range(0, audioClip.Length);
            checkIfSameAsLast(clip, randomClip);
            source.clip = audioClip[randomClip];
            clip = randomClip;

        }
        else
        {
            clip += 1;
            if (clip >= audioClip.Length)
            {
                clip = 0;
            }
            source.clip = audioClip[clip];
        }
        actualPitch = pitch + externalPitchModifier + Random.Range(-pitchRandomization, pitchRandomization);
        source.pitch = Mathf.Pow(1.05946f, actualPitch);
        if (audioClip.Length <= 1){
            source.loop = loop;
        }
        else if (loop && sequence && clip == elementToLoop){
            source.loop = true;
        }
        
        audioSource = source;

        source.Play();
        //print("Played Sound " + source.clip);
        if (!loop)
        {
            Destroy(source, audioClip[clip].length);
            //print("Destroyed");
        }
        else {
            //sourceList.Add(source);
        }

    }

    public void StopSound(){
        if(fadeOutTime > 0){
            fadeOut = true;
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
    
    void Start ()
    {
        transform = gameObject.GetComponent<Transform>();
        if (playOnAwake)
        {
            PlaySound();
        }
        listener = GameObject.FindObjectOfType<Camera>().transform;
    }
    
    IEnumerator NextClip(){
        float duration = Random.Range(waitTime - (waitRange/2), waitTime + (waitRange/2));
        yield return new WaitForSeconds(duration);
        PlaySound();
    }

    void Update()
    {
        if (audioSource && loop && !audioSource.isPlaying && soundPlayed){
            StartCoroutine("NextClip");
            soundPlayed = false;
        }

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
                soundPlayed = false;
                Destroy(audioSource);
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
        
        if (soundPlayed && audioSource != null)
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
            else if (loop)
            {
                 audioSource.volume = volume * externalVolumeModifier;
            }
            else
            {

            }
        }
    }
    
}

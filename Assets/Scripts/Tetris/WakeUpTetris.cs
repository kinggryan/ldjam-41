using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WakeUpTetris : MonoBehaviour {

    public GameObject pressButtonToPlayParent;
    public UnityEngine.UI.Text[] gameTextToFadeOut;
    public TetrisManager tetris;
    public Color fadedOutGameTextColor;
    public Color fadedInGameTextColor;
    public float minPauseDuration = 10f;

    private float pauseTimer;

    private MusicEngine musicEngine;

    private void Start()
    {
        fadedInGameTextColor = gameTextToFadeOut[0].color;
        musicEngine = Object.FindObjectOfType<MusicEngine>();
        PausePlay();
    }

    // Update is called once per frame
    void Update () {
        pauseTimer += Time.deltaTime;
        if (pauseTimer >= minPauseDuration && tetris.IsPaused() && (Input.GetButtonUp("left") || Input.GetButtonUp("right") || Input.GetButtonUp("up") || Input.GetButtonUp("down") || Input.GetButtonUp("rotateclockwise") || Input.GetButtonUp("rotatecounterclockwise") ))
        {
            BeginPlay();
        }
	}

    public void PausePlay()
    {
        pauseTimer = 0;
        Debug.Log("Pausing play");
        musicEngine.PauseMusic();
        tetris.PausePlay();
        pressButtonToPlayParent.SetActive(true);
        // Fade in text
        foreach (var text in gameTextToFadeOut)
        {
            text.color = fadedOutGameTextColor;
        }
    }

    void BeginPlay()
    {
        Debug.Log("Resuming play");
        pressButtonToPlayParent.SetActive(false);
        tetris.ResumePlay();
        // Fade in text
        foreach (var text in gameTextToFadeOut)
        {
            text.color = fadedInGameTextColor;
        }
    }
}

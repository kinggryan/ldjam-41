using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {

    //public GameObject colorFadePanel;
    
    private float transitionTime;
    
    private bool timerOn = false;
    public float transitionTimer;
    private string levelToLoad;
    
    private GameObject canvas;
    
    void Awake ()
    {
        canvas = GameObject.Find("Canvas");
    }
    
    public void LoadLevel(string name, float loadTransitionTime)
    {
        transitionTime = loadTransitionTime;
        levelToLoad = name;
        timerOn = true;
        Debug.Log("Level load requested for " + levelToLoad + " Time on = " + timerOn + ".");
        //FadeTransition(transitionTime);
        
    }
    
    public void QuitRequest()
    {
        Debug.Log("Quit requested");
        Application.Quit();
    }
    
    /*void FadeTransition (float fadeTime)
    {
        GameObject transitionPanel = Instantiate(colorFadePanel, canvas.transform.position, canvas.transform.rotation) as GameObject;
        
        transitionPanel.GetComponent<FadeFromColor>().fadeIn = false;
        //transitionPanel.GetComponent<FadeFromColor>().currentColor.a = 0;
        transitionPanel.GetComponent<FadeFromColor>().fadeTime = fadeTime;
        
        transitionPanel.transform.SetParent(canvas.transform, false);
        
    }*/
    
    void Update ()
    {
        /* if (timerOn)
        {
            transitionTimer += Time.deltaTime;
        }
        
        if (transitionTimer > transitionTime)
        {
            timerOn = false;
            transitionTimer = 0f;
            Application.LoadLevel(levelToLoad);
        }*/
    }
    
    //UI LOAD BUTTON
    
    public void ButtonLoadTime (float loadTransitionTime)
    {
        transitionTime = loadTransitionTime;
    }
    
    public void ButtonLoadLevel (string name)
    {
        levelToLoad = name;
        timerOn = true;
        Debug.Log("Level load requested for " + levelToLoad + " Time on = " + timerOn + ".");
        Application.LoadLevel(levelToLoad);
    }
    

    
}

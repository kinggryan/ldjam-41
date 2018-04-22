using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeFromColor : MonoBehaviour {
    
    private Image image;
    public Color currentColor;
    
    public float timeSinceAwake = 0;
    
    public float fadeTime;
    public bool fadeIn;

	// Use this for initialization
	void Start ()
    {
        print("COLOR FADE AWAKE");
        
        if (fadeIn)
        {
            currentColor.a = 1;
        }
        else
        {
            currentColor.a = 0;
        }
        
        image = GetComponent<Image>();
        image.color = currentColor;

    }

    void Update ()
    {
        timeSinceAwake += Time.deltaTime; 
        
        if (timeSinceAwake < fadeTime)
        {
            // Fade In
            float alphaChange = Time.deltaTime / fadeTime;
            if (fadeIn)
            {
                currentColor.a -= alphaChange;
            }
            else
            {
                currentColor.a += alphaChange;
            }
            
            image.color = currentColor;
            
            
        }
        else
        {
            if (fadeIn)
            {
            Destroy(gameObject);
            }
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CRTScroller : MonoBehaviour {

    public UnityEngine.UI.Image overlay;
    float scrollSpeed = 100f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        overlay.material.SetTextureOffset("_MainTex", new Vector2(0, Time.time * scrollSpeed));
    }
}

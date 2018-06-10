using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CRTFlicker : MonoBehaviour {

	public UnityEngine.UI.Image image;
	public AnimationCurve flickerCurve;
	public float curveMinSpeed;
	public float curveMaxSpeed;
	public float minLineBrightness = 0.3f;
	public float maxLineBrightness = 0.33f;

	private float curveVal = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		curveVal += Random.Range(curveMinSpeed,curveMaxSpeed)*Time.deltaTime;
		var newAlpha = minLineBrightness + (maxLineBrightness - minLineBrightness)*flickerCurve.Evaluate(curveVal);
		var newColor = image.color;
		newColor.a = newAlpha;
		image.color = newColor;
	}
}

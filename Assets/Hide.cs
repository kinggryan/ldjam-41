using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cradle;

public class Hide : MonoBehaviour
{
	public TextrisTwinePlayer twinePlayer;
	public GameObject infectedPan;
	public GameObject dynamicText;
	public GameObject stackAndQueue;
	// Use this for initialization
	void Start()
	{
		twinePlayer = Object.FindObjectOfType<TextrisTwinePlayer>();
		infectedPan.SetActive(false);

	}

	// Update is called once per frame
	void Update()
	{
		if (Isinfected())
		{         
			dynamicText.SetActive(false);
			stackAndQueue.SetActive(false);
			infectedPan.SetActive(true);         
		}

		else
		{
			dynamicText.SetActive(true);
			stackAndQueue.SetActive(true);
			infectedPan.SetActive(false);         
		}

	}


	bool Isinfected()
	{
		return twinePlayer.GetTwineVarState("infected");
	}
}

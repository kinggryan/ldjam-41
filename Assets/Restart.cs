using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Restart : MonoBehaviour {
	private SoundEngine soundEngine;
	// Use this for initialization
	void Start () {
		soundEngine = Object.FindObjectOfType<SoundEngine>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
		{
			soundEngine.PlaySoundWithName("Enter");
			SceneManager.LoadScene("Menu");
		}
	}
}

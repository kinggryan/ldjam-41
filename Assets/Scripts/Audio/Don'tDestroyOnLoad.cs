using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour {

	static SoundEngine instance = null;

	void Awake(){
		GameObject.DontDestroyOnLoad(gameObject);
	}
}

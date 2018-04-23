using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WakeUpTetris : MonoBehaviour {

    public TetrisManager tetris;

	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown("left") || Input.GetButtonDown("right") || Input.GetButtonDown("up") || Input.GetButtonDown("down") || Input.GetButtonDown("rotateclockwise") || Input.GetButtonDown("rotatecounterclockwise") )
        {
            tetris.playingEnabled = true;
        }
	}
}

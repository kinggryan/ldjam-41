﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cradle;

//Prints Current Available Commands as Buttons for Testing
public class TestDyanmic : MonoBehaviour {
	public IEnumerable<Cradle.StoryLink> commandText;
	public TextrisTwinePlayer twinePlayer;
	public GUIStyle style = new GUIStyle();
	
	// Use this for initialization
	void Start () {
		twinePlayer = Object.FindObjectOfType<TextrisTwinePlayer>();
		commandText = twinePlayer.Story.GetCurrentLinks();			
	}
	
	// Update is called once per frame
	void Update () {

	}
	void OnGUI() {
		int x = 32;
		GUI.Label (new Rect (10,10,150,20), "CMD:/", style);
		foreach(var link in commandText){
			
			var text = link.Text;
			//Longer commands like "GET COAT" do not enter TETRIS space
			if (text.Contains(" ")) { 
				x += 40;
				string[] words = text.Split(null);

				if(GUI.Button (new Rect (10,x,150,20), words[0], style)){
					twinePlayer.Story.DoLink(text);
				}
				if (GUI.Button (new Rect (10,x += 32,150,20), words[1], style)){
					twinePlayer.Story.DoLink(text);
				}
				
			}

			else{
				x += 40;
				if(GUI.Button (new Rect (10,x,150,20), link.Text, style)){
					twinePlayer.Story.DoLink(text);
				}
				
			}
			
			
		}	
	}
}

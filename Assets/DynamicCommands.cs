using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cradle;

public class DynamicCommands : MonoBehaviour {
	public IEnumerable<Cradle.StoryLink> commandText;
	public TextrisTwinePlayer twinePlayer;
	public Cradle.StoryVar items;
	public GUIStyle style = new GUIStyle();
	
	// Use this for initialization
	void Start () {
		twinePlayer = Object.FindObjectOfType<TextrisTwinePlayer>();
		commandText = twinePlayer.Story.GetCurrentLinks();
		items = twinePlayer.Story.Vars["inv"];				
	}
	
	// Update is called once per frame
	void Update () {
		items = twinePlayer.Story.Vars["inv"];	
	}
	void OnGUI() {
		int x = 10;
		GUI.Label (new Rect (10,x,150,20), "CMD:/", style);
		foreach(var link in commandText){
			
			var text = link.Text;
			//Longer commands like "GET COAT" do not enter TETRIS space
			if (text.Contains(" ")) { 
				x += 40;
				string[] words = text.Split(null);
				GUI.Label (new Rect (10,x,150,20), words[0], style);
				GUI.Label (new Rect (10,x += 32,150,20), words[1], style);
				
			}

			else{
				x += 40;
				GUI.Label (new Rect (10,x,150,20), link.Text, style);
				
			}
			
			
		}	
			x += 40;
			GUI.Label (new Rect (10,x,150,20), "INV:/", style);
		

			//I would like to just iterate over items but it won't let me	
			if(items["NOTE"] == true){
				x += 40;
				GUI.Label (new Rect (10,x,150,20), "NOTE" , style);
				
			}
			if(items["COAT"] == true){
				x += 40;
				GUI.Label (new Rect (10,x,150,20), "COAT" , style);
				
			}	
			if(items["GUN"] == true){
				x += 40;
				GUI.Label (new Rect (10,x,150,20), "GUN" , style);
				
			}	
			if(items["FOB"] == true){
				x += 40;
				GUI.Label (new Rect (10,x,150,20), "FOB" , style);
				
			}		
			if(items["CURE"] == true){
				x += 40;
				GUI.Label (new Rect (10,x,150,20), "CURE" , style);
				
			}	
	}
}

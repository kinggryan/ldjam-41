using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CursorNav : MonoBehaviour {
	public Text [] textList;
	public Transform cursor;
	public int curIndex = 1;
	public int  currentScene;
	public Color lime = new Color(0, 255, 0); 

	// Use this for initialization
	void Start () {
		textList = GetComponentsInChildren<Text>();
		cursor = gameObject.transform.Find("Cursor");
		currentScene = SceneManager.GetActiveScene().buildIndex;
       
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("down")){
			if(curIndex < 2 ){
				textList[curIndex].color = lime;
				curIndex++;
                cursor.transform.position += Vector3.down * 51;
				textList[curIndex].color = Color.white;
			}
            
		}

		if (Input.GetButtonDown("up"))
        {
			if (curIndex >= 1)
            {
				textList[curIndex].color = lime;
                curIndex--;
				textList[curIndex].color = Color.white;
                cursor.transform.position += Vector3.up * 51;

            }
        }
        
		if(Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space)){
			if (currentScene == 0)
			{
				SceneManager.LoadScene(curIndex + 1);
			}

			else{
				if (curIndex == 2){
					SceneManager.LoadScene("01a Start");
				}
				else{
					SceneManager.LoadScene(curIndex + 1);
				}
			}
		}
		
	}
}

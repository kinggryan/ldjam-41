using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CursorNav : MonoBehaviour {
	public Text [] textList;
	public Slider[] sliders;
	public Transform cursor;
	public int curIndex = 0;
	public Scene currentScene;
	public Color lime = new Color(0, 255, 0);
	public int cursorOffset = 62;

	// Use this for initialization
	void Start () {
		textList = GetComponentsInChildren<Text>();
		cursor = gameObject.transform.Find("Cursor");
		currentScene = SceneManager.GetActiveScene();

		if (currentScene == SceneManager.GetSceneByName("Options"))
		{
			sliders = GetComponentsInChildren<Slider>();
			sliders[0].Select();
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("down")){
			if(curIndex < 6  && currentScene == SceneManager.GetSceneByName("Options")){
				textList[curIndex].color = lime;
				if (curIndex == 2)
                {
                    cursor.transform.position += Vector3.down * 22;
					foreach(Slider s in sliders){
						s.interactable = false;
					}
                }
				curIndex++;
				Debug.Log(textList[curIndex].text + ", " + curIndex);

				cursor.transform.position += Vector3.down * cursorOffset;
				textList[curIndex].color = Color.white;
			}

			if (curIndex < 3 && currentScene == SceneManager.GetSceneByName("Menu") )
            {
                textList[curIndex].color = lime;
                curIndex++;
                Debug.Log(textList[curIndex].text + ", " + curIndex);

				cursor.transform.position += Vector3.down * cursorOffset;
                textList[curIndex].color = Color.white;
            }
            
		}

		if (Input.GetButtonDown("up"))
        {
			if (curIndex >= 1)
            {
				textList[curIndex].color = lime;
				if (curIndex == 3 && currentScene == SceneManager.GetSceneByName("Options"))
                {
                    cursor.transform.position += Vector3.up * 22;
					foreach (Slider s in sliders)
                    {
						s.interactable = true;
                    }
					sliders[2].Select();
                }
                curIndex--;
				Debug.Log(textList[curIndex].text + ", " + curIndex);
				textList[curIndex].color = Color.white;
				cursor.transform.position += Vector3.up * cursorOffset;
                
            }
        }
        
		if(Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space)){

			if (currentScene == SceneManager.GetSceneByName("Menu"))

			{
				if (curIndex == 0){
					SceneManager.LoadScene("Game");
				}
                
				if (curIndex == 1){
					SceneManager.LoadScene("TEST");
				}

				if(curIndex == 2){
					SceneManager.LoadScene("Options");
				}


			}
			if (currentScene == SceneManager.GetSceneByName("Options")){

				if (curIndex == 3)
                {
                    SceneManager.LoadScene("Game");
                }

                if (curIndex == 4)
                {
                    SceneManager.LoadScene("TEST");
                }

				if(curIndex == 5)
                {
                    SceneManager.LoadScene("Menu");
                }
				
			}

		}
		
	}
}

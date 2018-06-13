using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class CursorNav : MonoBehaviour {
	public Text [] textList;
	public Slider[] sliders;
	public Transform cursor;
	public int curIndex = 0;
	public Scene currentScene;
	public Color lime = new Color(0, 255, 0);
	public int cursorOffset = 62;
	private SoundEngine soundEngine;

	private bool[] sliderMaxReached = new bool[]{ false, false, false};
	private bool[] sliderMinReached = new bool[]{ false, false, false};


	// Use this for initialization
	void Start () {
		textList = GetComponentsInChildren<Text>();
		soundEngine = Object.FindObjectOfType<SoundEngine>();
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
				soundEngine.PlaySoundWithName("MenuNav");
			}
            
			if (curIndex < 3 && currentScene == SceneManager.GetSceneByName("Menu") )
            {
                textList[curIndex].color = lime;
				curIndex++;

				cursor.transform.position += Vector3.down * cursorOffset;
                textList[curIndex].color = Color.white;
				Debug.Log(textList[curIndex].text + ", " + curIndex + ", " + cursor.transform.position.ToString());
				soundEngine.PlaySoundWithName("MenuNav");
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
				soundEngine.PlaySoundWithName("MenuNav");
                
            }
        }
        
		if(Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space)){
			soundEngine.PlaySoundWithName("Enter");
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

				if (curIndex == 3)
                {
                    SceneManager.LoadScene("Credits");
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

				if (curIndex == 6)
                {
                    SceneManager.LoadScene("Credits");
                }
				
			}

		}

		//Slider SFX
		if (curIndex >= 0 && curIndex <= 2){
			if (Input.GetKeyDown(KeyCode.LeftArrow)){
			Debug.Log("Left Arrow");
				for (int i = 0; i <= sliders.Length; i++){
					Slider slider = sliders[i];
					if (slider.gameObject == EventSystem.current.currentSelectedGameObject){
						if (slider.value > slider.minValue){
							if (!sliderMinReached[i]){
							soundEngine.PlaySoundWithName("MenuNavSub");
							}
							sliderMaxReached[i] = false;
						}else if (!sliderMinReached[i]){
							soundEngine.PlaySoundWithName("MenuNavSub");
							sliderMinReached[i] = true;
						}
					}
				}
			}else if (Input.GetKeyDown(KeyCode.RightArrow)){
			Debug.Log("Right Arrow");
				for (int i = 0; i <= sliders.Length; i++){
					Slider slider = sliders[i];
					if (slider.gameObject == EventSystem.current.currentSelectedGameObject){
						if (slider.value < slider.maxValue){
							soundEngine.PlaySoundWithName("MenuNavSub");
							sliderMinReached[i] = false;
						}else if (!sliderMaxReached[i]){
							soundEngine.PlaySoundWithName("MenuNavSub");
							sliderMaxReached[i] = true;
						}
					}
				}
			}
		}
	}
}

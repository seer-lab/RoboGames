using System.Collections;
using System.Collections.Generic;
using UnityEngine.Video;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class GamePicker : MonoBehaviour
{
    int indexSelcted; 
    public GameObject[] Items = new GameObject[2]; 
    SelectTitle[] titles = new SelectTitle[2]; 
    Fade fade; 
    VideoPlayer player;

    
    // Start is called before the first frame update
    void Start()
    {

        player = GameObject.Find("Video Player").GetComponent<VideoPlayer>();
        #if UNITY_WEBGL
            player.url = Application.persistentDataPath + "/" + stringLib.MOVIE_INTRO_MENU;
        #endif

        indexSelcted = 0; 
        for (int i = 0; i < Items.Length; i++){
            titles[i] = Items[i].GetComponent<SelectTitle>(); 
        }
        titles[indexSelcted].Select(); 
        fade = GameObject.Find("Fade").GetComponent<Fade>(); 
        fade.onFadeIn(); 
    }
    public void SelectItem(int index){
        if (index != indexSelcted){
            titles[indexSelcted].Deselect(); 
            indexSelcted = index; 
            titles[indexSelcted].Select(); 
        }
        else {
            if (indexSelcted == 1){
                GlobalState.GameMode = stringLib.GAME_MODE_BUG; 
            }
            else GlobalState.GameMode = stringLib.GAME_MODE_ON; 
            StartCoroutine(LoadIntroScene());            
        }

    }
    IEnumerator LoadIntroScene(){
        fade.onFadeOut(); 
        yield return new WaitForSecondsRealtime(0.5f); 
        SceneManager.LoadScene("IntroScene");
    }
    // Update is called once per frame
    void Update()
    {
        int currentIndex = indexSelcted; 
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.DownArrow)){
            indexSelcted ++; 
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.UpArrow)){
            if (indexSelcted >0)
                indexSelcted--; 
            else indexSelcted++; 
        }
        if (currentIndex != indexSelcted%2){
            indexSelcted = indexSelcted %2; 
            titles[indexSelcted].Select(); 
            titles[currentIndex].Deselect(); 
        }
        if (Input.GetKeyDown(KeyCode.Return)){
            if (indexSelcted == 1){
                GlobalState.GameMode = stringLib.GAME_MODE_BUG; 
            }
            else GlobalState.GameMode = stringLib.GAME_MODE_ON; 
            StartCoroutine(LoadIntroScene()); 
        }
    }
}

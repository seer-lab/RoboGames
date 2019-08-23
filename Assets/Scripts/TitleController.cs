using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.Video; 
using UnityEngine.UI; 
using UnityEngine.SceneManagement; 

public class TitleController : MonoBehaviour
{
    VideoPlayer player; 
    Animator robot, boy, girl; 
    Text text; 
    bool isCharacterUp = false; 
    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log(SystemInfo.operatingSystem);

        //Mac has a weird issue with cached storage, such that if there is a file related to the game
        // then the video wont play. This is here to deal with that issue
        if(SystemInfo.operatingSystem.Contains("Mac") || SystemInfo.operatingSystem.Contains("iOS")){
            if(PlayerPrefs.HasKey("sessionID")){
                String sessionID = PlayerPrefs.GetString("sessionID");
                Debug.Log("MAC SessionID: " + sessionID);
                if(sessionID != ""|| sessionID != null){
                    GlobalState.sessionID = Convert.ToInt64(sessionID);
                }

                //Grab the Menu Preference
                //First Check if it exist
                if(PlayerPrefs.HasKey("language")){
                    GlobalState.Language = PlayerPrefs.GetString("language", "c++");
                }
                if(PlayerPrefs.HasKey("textsize")){
                    GlobalState.TextSize = PlayerPrefs.GetInt("textsize", 1);
                }
                if(PlayerPrefs.HasKey("soundon")){
                    GlobalState.soundon = Convert.ToBoolean(PlayerPrefs.GetInt("soundon", 1));
                }
                if(PlayerPrefs.HasKey("themes")){
                    GlobalState.IsDark = Convert.ToBoolean(PlayerPrefs.GetInt("themes", 1));
                }
                if(PlayerPrefs.HasKey("tooltips")){
                    GlobalState.HideToolTips = Convert.ToBoolean(PlayerPrefs.GetInt("tooltips", 1));
                }
            }
            PlayerPrefs.DeleteAll();
        }
        
        player = GameObject.Find("Video Player").GetComponent<VideoPlayer>(); 
        #if UNITY_WEBGL && !UNITY_EDITOR     
            String url = GlobalState.URL_MOVIE;          
            Debug.Log("URL : " + url + " Movie: " + stringLib.MOVIE_INTRO);
            if(url == "" || url == null){
                Debug.Log("Playing Movie from Server");
                player.url = stringLib.SERVER_URL + stringLib.STREAMING_ASSETS + stringLib.MOVIE_INTRO;
            }else{
                Debug.Log("Playing Movie from cache, url: " + url + ", length: " + url.Length);
                player.url = url;
            }
        #endif
        robot = this.transform.GetChild(0).GetComponent<Animator>(); 
        girl = this.transform.GetChild(1).GetComponent<Animator>(); 
        boy = this.transform.GetChild(2).GetComponent<Animator>(); 
        text = this.transform.GetChild(3).GetComponent<Text>(); 
    }   

    IEnumerator ShowCharacters(){
        isCharacterUp = true; 
        robot.SetTrigger("Jump"); 
        yield return new WaitForSecondsRealtime(0.3f);
        girl.SetTrigger("Jump"); 
        yield return new WaitForSecondsRealtime(0.3f);
        boy.SetTrigger("Jump");
        yield return new WaitForSecondsRealtime(0.4f); 
        while(text.color.a < 0.95f){
            text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a + 0.05f); 
            yield return null; 
        }
    }
    IEnumerator LoadGame(){
        GameObject.Find("Fade").GetComponent<Fade>().onFadeOut(); 
        yield return new WaitForSecondsRealtime(1f); 
        if (GlobalState.RestrictGameMode){
            if(GlobalState.GamemodeON_BUG){
                GlobalState.GameMode = stringLib.GAME_MODE_ON;
            }else{
                GlobalState.GameMode = stringLib.GAME_MODE_BUG;
            }
            SceneManager.LoadScene("IntroScene");
        }
        else SceneManager.LoadScene("TitleMenu");

    }
    // Update is called once per frame
    void Update()
    {
        if (!player.isPlaying && !isCharacterUp && Time.timeSinceLevelLoad > 2){
            StartCoroutine(ShowCharacters()); 
        }
        if (Input.anyKey || Input.GetMouseButton(0)){
            StartCoroutine(LoadGame()); 
        }
    }
}

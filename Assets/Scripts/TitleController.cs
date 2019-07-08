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
        WebHelper.i.DownloadVideoAssetBundle(stringLib.SERVER_URL + stringLib.ASSETS_BUNDLE + "menu");
        // WebHelper.i.SaveMovieDataFromWeb(stringLib.MOVIE_INTRO);
        // WebHelper.i.SaveMovieDataFromWeb(stringLib.MOVIE_INTRO_MENU);
        // WebHelper.i.SaveMovieDataFromWeb(stringLib.MOVIE_BUG);
        // WebHelper.i.SaveMovieDataFromWeb(stringLib.MOVIE_ON);

        //Debug.Log(SystemInfo.operatingSystem);
        if(SystemInfo.operatingSystem.Contains("Mac") || SystemInfo.operatingSystem.Contains("iOS")){

            if(PlayerPrefs.HasKey("sessionID")){
                String sessionID = PlayerPrefs.GetString("sessionID");
                Debug.Log("MAC SessionID: " + sessionID);
                if(sessionID != ""|| sessionID != null){
                    GlobalState.sessionID = Convert.ToInt64(sessionID);
                //Debug.Log("GLOBALSTATE SessionID" + GlobalState.sessionID);
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

        // WebHelper.i.RequestMovieFromIndexedDB(stringLib.MOVIE_INTRO);
        // WebHelper.i.RequestMovieFromIndexedDB(stringLib.MOVIE_INTRO_MENU);
        // WebHelper.i.RequestMovieFromIndexedDB(stringLib.MOVIE_ON);
        // WebHelper.i.RequestMovieFromIndexedDB(stringLib.MOVIE_BUG);

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
        SceneManager.LoadScene("TitleMenu");

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

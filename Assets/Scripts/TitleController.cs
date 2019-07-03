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
        Debug.Log(SystemInfo.operatingSystem);
        if(SystemInfo.operatingSystem.Contains("Mac") || SystemInfo.operatingSystem.Contains("iOS")){

            if(PlayerPrefs.HasKey("sessionID")){
                String sessionID = PlayerPrefs.GetString("sessionID");
                Debug.Log("MAC SessionID: " + sessionID);
                if(sessionID != ""|| sessionID != null){
                //GlobalState.sessionID = Convert.ToInt64(sessionID);
                //Debug.Log("GLOBALSTATE SessionID" + GlobalState.sessionID);
                }
            }
            PlayerPrefs.DeleteAll();
        }
        WebHelper.i.GetMovieDataFromWeb(stringLib.MOVIE_INTRO);
        WebHelper.i.GetMovieDataFromWeb(stringLib.MOVIE_INTRO_MENU);
        WebHelper.i.GetMovieDataFromWeb(stringLib.MOVIE_BUG);
        WebHelper.i.GetMovieDataFromWeb(stringLib.MOVIE_ON);
        player = GameObject.Find("Video Player").GetComponent<VideoPlayer>(); 
        #if UNITY_WEBGL                    
            //Console.WriteLine(stringLib.SERVER_URL + filepath);
            if(File.Exists(Application.persistentDataPath + "/" + stringLib.MOVIE_INTRO)){
                try{
                    player.url = Application.persistentDataPath + "/" + stringLib.MOVIE_INTRO;
                }catch(Exception e){
                    Debug.Log(e.Message);
                }
            }else{
                player.url = stringLib.SERVER_URL + stringLib.STREAMING_ASSETS + stringLib.MOVIE_INTRO;
            }  
            Debug.Log("TitleController Start() WEBGL");
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

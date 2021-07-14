﻿using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

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
#if UNITY_WEBGL && !UNITY_EDITOR
            String url = GlobalState.URL_MOVIE_MENU;          
            Debug.Log("URL : " + url + " Movie: " + stringLib.MOVIE_INTRO_MENU);
            if(url == "" || url == null){
                Debug.Log("Playing Movie from Server");
                player.url = stringLib.SERVER_URL + stringLib.STREAMING_ASSETS + stringLib.MOVIE_INTRO_MENU;
            }else{
                Debug.Log("Playing Movie from cache, url: " + url + ", length: " + url.Length);
                player.url = url;
            }
#endif

        indexSelcted = 0;
        for (int i = 0; i < Items.Length; i++)
        {
            titles[i] = Items[i].GetComponent<SelectTitle>();
        }
        titles[indexSelcted].Select();
        fade = GameObject.Find("Fade").GetComponent<Fade>();
        fade.onFadeIn();
    }
    /// <summary>
    /// Highlights the item, and if already highlighted will select that item.
    /// </summary>
    /// <param name="index">index of the item</param>
    public void SelectItem(int index)
    {
        if (index != indexSelcted)
        {
            titles[indexSelcted].Deselect();
            indexSelcted = index;
            titles[indexSelcted].Select();
        }
        else
        {
            if (indexSelcted == 1)
            {
                GlobalState.GameMode = stringLib.GAME_MODE_BUG;
            }
            else GlobalState.GameMode = stringLib.GAME_MODE_ON;
            String sessionID = PlayerPrefs.GetString("sessionID");
            Debug.Log("SESSISONIOHFKJS: " + sessionID);
            if ((sessionID == "" && sessionID == null) && GlobalState.LeaderBoardMode)
            {
                StartCoroutine(LoadStartScene());
            }
            else if ((sessionID == "" || sessionID == null) && GlobalState.RestrictGameMode)
            {
                StartCoroutine(LoadStartScene());
            }
            else
            {
                StartCoroutine(LoadIntroScene());
            }
            // if((sessionID == "" && sessionID == null || GlobalState.sessionID == 0)&& GlobalState.LeaderBoardMode){
            //     StartCoroutine(LoadStartScene());  
            // }else{
            //     StartCoroutine(LoadIntroScene());  
            // }           
        }

    }
    IEnumerator LoadIntroScene()
    {
        fade.onFadeOut();
        yield return new WaitForSecondsRealtime(0.5f);
        SceneManager.LoadScene("IntroScene");
    }

    IEnumerator LoadStartScene()
    {
        fade.onFadeOut();
        yield return new WaitForSecondsRealtime(0.5f);
        SceneManager.LoadScene("StartScene");
    }

    IEnumerator CourseStartScene()
    {
        fade.onFadeOut();
        yield return new WaitForSecondsRealtime(0.5f);
        SceneManager.LoadScene("CourseCode");
    }

    // Update is called once per frame
    void Update()
    {
        int currentIndex = indexSelcted;
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            indexSelcted++;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (indexSelcted > 0)
                indexSelcted--;
            else indexSelcted++;
        }
        if (currentIndex != indexSelcted % 2)
        {
            indexSelcted = indexSelcted % 2;
            titles[indexSelcted].Select();

            titles[currentIndex].Deselect();
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (indexSelcted == 1)
            {
                GlobalState.GameMode = stringLib.GAME_MODE_BUG;
            }
            else GlobalState.GameMode = stringLib.GAME_MODE_ON;

            String sessionID = PlayerPrefs.GetString("sessionID");
            Debug.Log("SESSISONIOHFKJS: " + sessionID);


            if ((sessionID == "" && sessionID == null) && GlobalState.LeaderBoardMode)
            {
                StartCoroutine(CourseStartScene());
            }
            else if ((sessionID == "" || sessionID == null) && GlobalState.RestrictGameMode)
            {
                StartCoroutine(CourseStartScene());
            }
            else
            {
                StartCoroutine(CourseStartScene());
            }

            // if((sessionID == "" && sessionID == null || GlobalState.sessionID == 0) && GlobalState.LeaderBoardMode){
            //     StartCoroutine(LoadStartScene());  
            // }else{
            //     StartCoroutine(LoadIntroScene());  
            // } 
        }
    }
}

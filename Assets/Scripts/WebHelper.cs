using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class WebHelper : MonoBehaviour
{
    public static WebHelper i;
    public string url {get; set;}
    public string someData {get; set;}

    public string webData {get; set;}
    public AssetBundle assetBundle {get; set;}

    void Awake(){
        if(!i){
            i = this;
            DontDestroyOnLoad(gameObject);
        }else{
            DestroyImmediate(gameObject);
        }
    }

    #if UNITY_WEBGL && !UNITY_EDITOR
        [DllImport("__Internal")]
        private static extern string GetData(string url);
        [DllImport("__Internal")]
        private static extern string GetUrlFromIndexedDB(string str);
        [DllImport("__Internal")]
        private static extern void SaveVideoInIndexedDB(string path, string filename);
    #endif

    IEnumerator GetXMLFromServer(string url) {
        UnityWebRequest www = UnityWebRequest.Get(url);
        www.SendWebRequest();
        System.Threading.Thread.Sleep(stringLib.DOWNLOAD_TIME);        
        if (www.isNetworkError || www.isHttpError) {
            Debug.Log(www.error);
            someData = "File not found!";
        }else {
            //Debug.Log(www.downloadHandler.text);
            someData = www.downloadHandler.text;
        }
        yield return new WaitForSeconds(0.5f);
    }

    IEnumerator GetMovieFromServer(string filename,string url){
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        if(www.isNetworkError){
            Debug.Log(url + " ERROR: " + www.error);
        }else{
            File.WriteAllBytes(Application.persistentDataPath + "/" + filename, www.downloadHandler.data);
        }
    }

    IEnumerator GetAssetBundlesM(string url){
        //Debug.Log("URI PORTS " + url );
        // WWW www = WWW.LoadFromCacheOrDownload(url, 0);

        // while(!www.isDone){
        //     Debug.Log("Progress: " + www.progress);
        //     yield return null;
        // }

        // if(www.error == null){
        //     assetBundle = www.assetBundle;
        //     Debug.Log("Success");
        // }else{
        //     Debug.Log(www.error);
        // }


        UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(url, 0);
        yield return request.SendWebRequest();
        
        while(!request.isDone){
            Debug.Log("Progress :" + request.downloadProgress);
            yield return null;
        }

        if(request.isNetworkError || request.isHttpError){
            Debug.Log(request.error);
        }else{
            Debug.Log("Download Success!");
            assetBundle = DownloadHandlerAssetBundle.GetContent(request);
        }
    }
    public string GetWebDataFromWeb(){
        #if UNITY_WEBGL && !UNITY_EDITOR
            // Console.WriteLine("Download On the web");
            // IEnumerator e = WebRequest(url);
            // while(e.MoveNext()){
            //     if(e.Current != null){
            //         Debug.Log(e.Current as string);
            //         this.webData = e.Current as string;
            //     }
            // }
            this.webData = GetData(this.url); //Synchronous
            // Console.WriteLine(this.webData);
        #elif UNITY_WEBGL
            StartCoroutine(GetXMLFromServer(this.url));
            this.webData = this.someData;
        #endif

        return this.webData;
    }

    public void SaveMovieDataFromWeb(string filename){
        //StartCoroutine(GetMovieFromServer(filename,stringLib.SERVER_URL + "StreamingAssets/" + filename ));
        //Get the URL from INDEXEDDB
        #if UNITY_WEBGL && !UNITY_EDITOR
            SaveVideoInIndexedDB(stringLib.SERVER_URL + stringLib.STREAMING_ASSETS + filename, filename);
        #endif
    }

    public void RequestMovieFromIndexedDB(string filename){
        #if UNITY_WEBGL && !UNITY_EDITOR
            webData = "";
            GetUrlFromIndexedDB(filename);
        #endif
    }

    void SetMovieIndexedDBURL(string filename){
        String[] webdata = filename.Split(' ');
        if(webdata[1] == stringLib.MOVIE_INTRO){
            GlobalState.URL_MOVIE = webdata[0];
            Debug.Log("Intro Movie Saved! : " + GlobalState.URL_MOVIE);
        }else if(webdata[1] == stringLib.MOVIE_INTRO_MENU){
            GlobalState.URL_MOVIE_MENU = webdata[0];
            Debug.Log("Intro Menu Movie Saved! : " + GlobalState.URL_MOVIE_MENU);
        }else if(webdata[1] == stringLib.MOVIE_ON){
            GlobalState.URL_MOVIE_ON = webdata[0];
            Debug.Log("ON Movie Saved! : " + GlobalState.URL_MOVIE_ON);
        }else if(webdata[1] == stringLib.MOVIE_BUG){
            GlobalState.URL_MOVIE_BUG = webdata[0];
            Debug.Log("BUG Movie Saved! : " + GlobalState.URL_MOVIE_BUG);
        }
    }

    public void DownloadVideoAssetBundle(string url){
        Caching.compressionEnabled = false;
        Caching.ClearCache();
        StartCoroutine(GetVideos(url));
    
    }

    IEnumerator GetVideos(string url){
        yield return GetAssetBundlesM(url);

        if(!assetBundle){
            Debug.Log("Asset Bundle with URL " + url + " failed");
            yield break;
        }
    }

}


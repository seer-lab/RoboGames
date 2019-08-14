using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// A class that sends request to the server
/// * This class uses javascript browswer functionality and UnityWebRequest
/// * This class must be attach to a gameobject in order for it to work
/// </summary>
public class WebHelper : MonoBehaviour
{
    public static WebHelper i;
    public string url {get; set;}
    public string someData {get; set;}

    public string webData {get; set;}
    public AssetBundle assetBundle {get; set;}

    //Make the gameobject that the class attach to a 
    // DontDestroyOnLoad
    void Awake(){
        if(!i){
            i = this;
            DontDestroyOnLoad(gameObject);
        }else{
            DestroyImmediate(gameObject);
        }
    }
    //Grabs the method from the serverFunction.jslib which is located in Assets/Plugins
    #if UNITY_WEBGL && !UNITY_EDITOR
        [DllImport("__Internal")]
        private static extern string GetData(string url);
        [DllImport("__Internal")]
        private static extern string GetUrlFromIndexedDB(string str);
        [DllImport("__Internal")]
        private static extern void SaveVideoInIndexedDB(string path, string filename);
    #endif

/// <summary>
/// A method which grabs web data from a url
/// * DOESNT HAVE TO BE XML
/// <param name="url"></param>
/// </summary>
    IEnumerator GetXMLFromServer(string url) {
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();      
        if (www.isNetworkError || www.isHttpError) {
            Debug.Log(www.error);
            someData = "File not found!";
        }else {
            //Debug.Log(www.downloadHandler.text);
            someData = www.downloadHandler.text;
        }
    }

    IEnumerator GetXMLFromServerEditor(string url) {
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
    
    [ObsoleteAttribute("Doesnt workin in Unity 2019.1/2019.2")]
    IEnumerator GetMovieFromServer(string filename,string url){
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        if(www.isNetworkError){
            Debug.Log(url + " ERROR: " + www.error);
        }else{
            File.WriteAllBytes(Application.persistentDataPath + "/" + filename, www.downloadHandler.data);
        }
    }

    [ObsoleteAttribute("Doesnt workin in Unity 2019.1/2019.2")]
    IEnumerator GetAssetBundlesM(string url){
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
    //Determines which platform you are on, and uses the appropriate function
    public string GetWebDataFromWeb(){
        #if UNITY_WEBGL && !UNITY_EDITOR
            this.webData = GetData(this.url); //Synchronous
            // Console.WriteLine(this.webData);
        #elif UNITY_WEBGL
            StartCoroutine(GetXMLFromServerEditor(this.url));
            this.webData = this.someData;
        #endif

        return this.webData;
    }
    //Determines which platform you are on, and uses the appropriate function
        public string GetWebDataFromWeb(bool useJavaScripsFunction){
            if(useJavaScripsFunction){
                #if UNITY_WEBGL && !UNITY_EDITOR
                this.webData = GetData(this.url);
                #endif
            }else{
                StartCoroutine(GetXMLFromServer(this.url));
                this.webData = this.someData;
            }
        return this.webData;
    }

    //Save the movie data into browsers storage cache, and when needed, it will grab the url which the Video
    // Player used to play it
    //Unfortunatly, grabing the movie url doesnt work
    [ObsoleteAttribute("Does not work in Unity, use the actual URL")]
    public void SaveMovieDataFromWeb(string filename){
        //StartCoroutine(GetMovieFromServer(filename,stringLib.SERVER_URL + "StreamingAssets/" + filename ));
        //Get the URL from INDEXEDDB
        #if UNITY_WEBGL && !UNITY_EDITOR
            SaveVideoInIndexedDB(stringLib.SERVER_URL + stringLib.STREAMING_ASSETS + filename, filename);
        #endif
    }

    //Grabs the movie data from browsers storage cache, and when needed
    //Unfortunatly, it doesnt work in Unity
    [ObsoleteAttribute("Does not work in Unity, use the actual URL")]
    public void RequestMovieFromIndexedDB(string filename){
        #if UNITY_WEBGL && !UNITY_EDITOR
            webData = "";
            GetUrlFromIndexedDB(filename);
        #endif
    }

    //Debuging function, Delete it if you wish
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

    //Downloading Video AssetsBundle doesnt work in Unity 2019.1/2019.2
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


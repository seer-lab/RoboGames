using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System;
using UnityEngine;
using UnityEngine.Networking;

public class WebHelper : MonoBehaviour
{
    public static WebHelper i;
    public string url {get; set;}
    public string someData {get; set;}

    public string webData {get; set;}

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
    #endif

    IEnumerator GetXMLFromServer(string url) {
        UnityWebRequest www = UnityWebRequest.Get(url);
        www.SendWebRequest();
        System.Threading.Thread.Sleep(stringLib.DOWNLOAD_TIME);        
        if (www.isNetworkError || www.isHttpError) {
            Debug.Log(www.error);
        }else {
            //Debug.Log(www.downloadHandler.text);
            someData = www.downloadHandler.text;
        }
        yield return new WaitForSeconds(0.5f);
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
}

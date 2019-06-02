using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System;
using UnityEngine;
using UnityEngine.Networking;

public class WebHelper : MonoBehaviour
{
    private string url;
    private string webData;
    public string someData;

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
    public WebHelper(string url){
        this.url = url;
    }

    public string getUrl(){
        return this.url;
    }

    public string GetWebDataFromWeb(){
        #if UNITY_WEBGL && !UNITY_EDITOR
            this.webData = GetData(this.url); //Synchronous
        #else
            StartCoroutine(GetXMLFromServer(this.url));
            this.webData = someData;
        #endif
        return this.webData;
    }

    public string getWebData(){
        return this.webData;
    }
}

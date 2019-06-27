using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
public class DatabaseHelperV2 : MonoBehaviour
{
    public static DatabaseHelperV2 i;
    public string url {get; set;}
    public string jsonData {get; set;}
    void Awake(){
        if(!i){
            i = this;
            DontDestroyOnLoad(gameObject);
        }else{
            DestroyImmediate(gameObject);
        }
    }

    IEnumerator PostToDB(string url, string bodyJsonString)
    {
        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(bodyJsonString);
        request.uploadHandler = (UploadHandler) new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler) new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Accept", "*");
        request.SetRequestHeader("Access-Control-Allow-Methods", "GET, POST, PUT");
        request.SetRequestHeader("Access-Control-Allow-Headers", "Accept, X-Access-Token, X-Requested-With,content-type");
        request.SetRequestHeader("Access-Control-Allow-Origin", "*");
        yield return request.SendWebRequest();

        Debug.Log("Status Code: " + request.responseCode);
    }

    IEnumerator Put(string url, string bodyJsonString)
    {
        byte[] myData = System.Text.Encoding.UTF8.GetBytes(bodyJsonString);
        UnityWebRequest request = UnityWebRequest.Put(url, myData);
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Accept", "*");
        request.SetRequestHeader("cache-control", "no-cache");
        request.SetRequestHeader("Access-Control-Allow-Methods", "GET, POST, PUT");
        request.SetRequestHeader("Access-Control-Allow-Headers", "Accept, X-Access-Token, X-Requested-With,content-type");
        request.SetRequestHeader("Access-Control-Allow-Origin", "*");
        yield return request.SendWebRequest();
        Debug.Log("Status Code: " + request.responseCode);
    }

    public void PostToDataBase(){
        StartCoroutine(PostToDB(url, jsonData));
    }

    public void PutToDataBase(){
        StartCoroutine(Put(url, jsonData));
    }
}

using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// A class that sends Data to DB
/// * This class must be attach to a gameobject in order for it to work
/// </summary>
public class DatabaseHelperV2 : MonoBehaviour
{
    public static DatabaseHelperV2 i;
    public string url {get; set;}
    public string jsonData {get; set;}
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

    //TODO Change Access Control Allow Origin , *  to an actual adress
    //Sends Data to DB thorugh POST
    IEnumerator PostToDB(string url, string bodyJsonString)
    {
        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(bodyJsonString);
        Debug.Log("POST Data: " + bodyJsonString );
        request.uploadHandler = (UploadHandler) new UploadHandlerRaw(bodyRaw);
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Accept", "*");
        request.SetRequestHeader("Access-Control-Allow-Methods", "GET, POST, PUT");
        request.SetRequestHeader("Access-Control-Allow-Headers", "Accept, X-Access-Token, X-Requested-With,content-type");
        request.SetRequestHeader("Access-Control-Allow-Origin", "*");
        yield return request.SendWebRequest();

        Debug.Log("Status Code for POST: " + request.responseCode);
    }

   //TODO Change Access Control Allow Origin , *  to an actual adress
   //Sends Data to DB thorugh PUT
    IEnumerator Put(string url, string bodyJsonString)
    {
        byte[] myData = System.Text.Encoding.UTF8.GetBytes(bodyJsonString);
        Debug.Log("Put Data: " + bodyJsonString );
        UnityWebRequest request = UnityWebRequest.Put(url, myData);
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Accept", "*");
        request.SetRequestHeader("cache-control", "no-cache");
        request.SetRequestHeader("Access-Control-Allow-Methods", "GET, POST, PUT");
        request.SetRequestHeader("Access-Control-Allow-Headers", "Accept, X-Access-Token, X-Requested-With,content-type");
        request.SetRequestHeader("Access-Control-Allow-Origin", "*");
        yield return request.SendWebRequest();
        Debug.Log("Status Code for PUT: " + request.responseCode);
    }

    public void PostToDataBase(){
        StartCoroutine(PostToDB(url, jsonData));
    }

    public void PutToDataBase(){
        StartCoroutine(Put(url, jsonData));
    }
}

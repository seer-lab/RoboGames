using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections;
using System.Collections.Generic;


public class databaseHelper: MonoBehaviour{
    IEnumerator Upload(string url, string data){
        
        using (UnityWebRequest www = UnityWebRequest.Post(url, data)){
            yield return www.SendWebRequest();

            if(www.isNetworkError || www.isHttpError){
                Debug.Log(www.error);
            }else{
                Debug.Log("Form upload");
            }
        }
    }

	public void uploadLogs(string url, string data){
		StartCoroutine(Upload(url, data));
	}
}

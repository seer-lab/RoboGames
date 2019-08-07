using System.Runtime.Versioning;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class LogoSwitcher : MonoBehaviour
{
    void Start(){
        string logoName = "Logo"; 
        if (GlobalState.GameMode == stringLib.GAME_MODE_BUG){
            logoName += "Bug";
        }
        if (GlobalState.IsDark){
            logoName+= "Dark";
        }
        else logoName += "Light";
        GetComponent<Image>().sprite = Resources.Load<Sprite>("MenuPrefabs/" + logoName); 
    }
}

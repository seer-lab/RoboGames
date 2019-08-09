using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputButtonScript : MonoBehaviour
{
    public void onclickInput(){
        LeaderboardControl ld = GameObject.Find("Canvas").GetComponent<LeaderboardControl>(); 
        ld.onclickInput();
    }
}

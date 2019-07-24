using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class ScoreText : MonoBehaviour
{
    Text text; 
    void Start(){
        text = GetComponent<Text>(); 
    }
    // Update is called once per frame
    void Update()
    {
        text.text = stringLib.SCORE_PREFIX +  (GlobalState.CurrentLevelPoints + GlobalState.RunningScore); 
    }
}

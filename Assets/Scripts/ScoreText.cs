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
<<<<<<< HEAD
        text.text = stringLib.SCORE_PREFIX +  (GlobalState.CurrentLevelPoints + GlobalState.Stats.Points); 
=======
        text.text = stringLib.SCORE_PREFIX +  (GlobalState.CurrentLevelPoints + GlobalState.totalPoints); 
>>>>>>> 9a07b32993b3c1b61feb82852987516bac4b442a
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using UnityEngine.SceneManagement; 

public class BackButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {   
        this.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(onClick); 
    }

    void onClick(){
         GlobalState.IsResume = true; 
        GlobalState.GameState = stateLib.GAMESTATE_MENU;
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Additive);
    }
    void Update(){
        if (GlobalState.GameState == stateLib.GAMESTATE_IN_GAME){
            this.GetComponent<Canvas>().enabled = true;
        }
        else if (GlobalState.GameState != stateLib.GAMESTATE_IN_GAME) GetComponent<Canvas>().enabled = false; 
    }
}

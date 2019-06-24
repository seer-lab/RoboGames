using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using UnityEngine.SceneManagement; 

public class BackButton : MonoBehaviour
{
    bool dark = true; 
    // Start is called before the first frame update
    public void ToggleColor(){
        if (GlobalState.IsDark != dark){
            dark = !dark; 
            if (dark){
                transform.GetChild(0).transform.GetChild(0).gameObject.GetComponent<Text>().color = Color.white;
                transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/panel-4");
            }else {
                transform.GetChild(0).transform.GetChild(0).gameObject.GetComponent<Text>().color = Color.black;
                transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/panel-8"); 
            }
        }
    }

    void Update(){
        if (GlobalState.GameState == stateLib.GAMESTATE_IN_GAME){
            this.GetComponent<Canvas>().enabled = true;
        }
        else if (GlobalState.GameState != stateLib.GAMESTATE_IN_GAME) GetComponent<Canvas>().enabled = false; 
    }
}
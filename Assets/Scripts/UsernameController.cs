using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UsernameController : MonoBehaviour
{
    Fade fade;

    Text errorText;
    InputField inputField;

    TouchScreenKeyboard keyboard;
    // Start is called before the first frame update
    void Start()
    {
        errorText = GameObject.Find("Error").GetComponent<Text>();
        errorText.text ="";
        inputField = GameObject.Find("InputField").GetComponent<InputField>();
        fade = GameObject.Find("Fade").GetComponent<Fade>();
        fade.onFadeIn();
        GameObject.Find("Fade").GetComponent<Canvas>().sortingOrder = 0;
        keyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default);
    }

    // Update is called once per frame
    void Update(){
        if(Input.GetKeyDown(KeyCode.Return)){
            onclickInput();
        }

        if(inputField.isFocused){
            keyboard.active = true;
        }else{
            keyboard.active = false;
        }
        
    }

    public void onclickInput(){
        Regex rgxCheck = new Regex("^[0-9]*$");
        if(inputField.text == ""){
            errorText.text = "PID cannot be empty!";
            inputField.text = "";
            return;
        }else if(inputField.text.Contains(" ")){
            errorText.text = "PID cannot contain spaces!";
            inputField.text = "";
        }else if(!rgxCheck.IsMatch(inputField.text)){
            errorText.text = "PID must only contain numbers!";
            inputField.text = "";
            return;
        }   

        WebHelper.i.url = stringLib.DB_URL + GlobalState.GameMode.ToUpper() + "/check/" + inputField.text;
        WebHelper.i.GetWebDataFromWeb();
        string reply = WebHelper.i.webData;

        if(reply == "true"){
            errorText.text = "Cannot Use this PID!";
            inputField.text = "";
            return;
        }

        GlobalState.username = inputField.text;
        GlobalState.sessionID = Convert.ToInt64( inputField.text);

        PlayerPrefs.SetString("sessionID", GlobalState.sessionID.ToString());
        GameObject.Find("InputPanel").SetActive(false);
        StartCoroutine(LoadIntroScene());
    }

    IEnumerator LoadIntroScene(){
        fade.onFadeOut(); 
        yield return new WaitForSecondsRealtime(0.5f); 
        SceneManager.LoadScene("IntroScene");
    }
}

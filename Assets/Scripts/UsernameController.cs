using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UsernameController : MonoBehaviour
{
    Fade fade;

    Text errorText;
    InputField inputField;
    // Start is called before the first frame update
    void Start()
    {
        errorText = GameObject.Find("Error").GetComponent<Text>();
        errorText.text ="";
        inputField = GameObject.Find("InputField").GetComponent<InputField>();
        fade = GameObject.Find("Fade").GetComponent<Fade>();
        fade.onFadeIn();
        GameObject.Find("Fade").GetComponent<Canvas>().sortingOrder = 0;


        
    }

    // Update is called once per frame
    void Update(){
        if(Input.GetKeyDown(KeyCode.Return)){
            onclickInput();
        }

        if(inputField.isFocused){
            // if(SystemInfo.operatingSystem.Contains("Android") || SystemInfo.operatingSystem.Contains("iOS")){
                TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default,false,false,false,false);
            //}
        }
        
    }

    public void onclickInput(){
        Debug.Log("ButtonClick Test");
        if(inputField.text == ""){
            errorText.text = "Username cannot be empty!";
            inputField.text = "";
            return;
        }else if(inputField.text.Contains(" ")){
            errorText.text = "Username cannot contain spaces!";
            inputField.text = "";
        }

        WebHelper.i.url = stringLib.DB_URL + GlobalState.GameMode.ToUpper() + "/check/" + inputField.text;
        WebHelper.i.GetWebDataFromWeb();
        string reply = WebHelper.i.webData;

        if(reply == "true"){
            errorText.text = "Cannot Use this name!";
            inputField.text = "";
            return;
        }

        GlobalState.username = inputField.text;
        GameObject.Find("InputPanel").SetActive(false);
        StartCoroutine(LoadIntroScene());
    }

    IEnumerator LoadIntroScene(){
        fade.onFadeOut(); 
        yield return new WaitForSecondsRealtime(0.5f); 
        SceneManager.LoadScene("IntroScene");
    }
}

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CourseCodeController : MonoBehaviour
{
    Fade fade;

    Text errorText;
    Dropdown dropDown;

    String dropDownValue;

    // Start is called before the first frame update
    void Start()
    {
        errorText = GameObject.Find("Error").GetComponent<Text>();
        errorText.text = "";

        dropDown = GameObject.Find("Dropdown").GetComponent<Dropdown>();

        fade = GameObject.Find("Fade").GetComponent<Fade>();
        fade.onFadeIn();
        GameObject.Find("Fade").GetComponent<Canvas>().sortingOrder = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void onclickInput()
    {
        if (dropDown.value == 0)
        {
            errorText.text = "Course Code cannot be empty";
            return;
        }
        else if (dropDown.value == 1)
        {
            dropDownValue = "CSCI 3060U";
        }
        else if (dropDown.value == 2)
        {
            dropDownValue = "CSCI 4020U";
        }
        else if (dropDown.value == 3)
        {
            dropDownValue = "CSCI 4080U";
        }

        GlobalState.courseCode = dropDownValue;

        PlayerPrefs.SetString("courseCode", GlobalState.courseCode);

        Debug.Log(GlobalState.courseCode);

        GameObject.Find("InputPanel").SetActive(false);

        if (GlobalState.RestrictGameMode && PlayerPrefs.HasKey("sessionID"))
        {
            StartCoroutine(LoadIntroScene());
        }
        else
        {
            StartCoroutine(LoadStartScene());
        }

    }

    IEnumerator LoadIntroScene()
    {
        fade.onFadeOut();
        yield return new WaitForSecondsRealtime(0.5f);
        SceneManager.LoadScene("IntroScene");
    }

    IEnumerator LoadStartScene()
    {
        fade.onFadeOut();
        yield return new WaitForSecondsRealtime(0.5f);
        SceneManager.LoadScene("StartScene");
    }
}

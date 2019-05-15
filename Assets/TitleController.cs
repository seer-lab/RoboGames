using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video; 
using UnityEngine.UI; 
using UnityEngine.SceneManagement; 

public class TitleController : MonoBehaviour
{
    VideoPlayer player; 
    Animator robot, boy, girl; 
    Text text; 
    bool isCharacterUp = false; 
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Video Player").GetComponent<VideoPlayer>();
        robot = this.transform.GetChild(0).GetComponent<Animator>(); 
        girl = this.transform.GetChild(1).GetComponent<Animator>(); 
        boy = this.transform.GetChild(2).GetComponent<Animator>(); 
        text = this.transform.GetChild(3).GetComponent<Text>(); 
    }   

    IEnumerator ShowCharacters(){
        isCharacterUp = true; 
        robot.SetTrigger("Jump"); 
        yield return new WaitForSecondsRealtime(0.3f);
        girl.SetTrigger("Jump"); 
        yield return new WaitForSecondsRealtime(0.3f);
        boy.SetTrigger("Jump");
        yield return new WaitForSecondsRealtime(0.4f); 
        while(text.color.a < 0.95f){
            text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a + 0.05f); 
            yield return null; 
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (!player.isPlaying && !isCharacterUp && Time.timeSinceLevelLoad > 2){
            StartCoroutine(ShowCharacters()); 
        }
        if (Input.anyKey || Input.GetMouseButton(0)){
            SceneManager.LoadScene("MainMenu");
        }
    }
}

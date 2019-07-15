using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class ProgressionUI : MonoBehaviour
{
    GameObject character; 
    GameObject Points; 
    bool glitching; 
    // Start is called before the first frame update
    void Start()
    {
        GameObject.Find("Fade").GetComponent<Fade>().onFadeIn(); 
        glitching = false; 
        Points = transform.Find("TotalPoints").gameObject; 
        Points.GetComponent<Text>().text = GlobalState.Stats.Points.ToString(); 
        character = transform.Find(GlobalState.Character).gameObject; 
        StartCoroutine(FadeIn(character));     
    }
    IEnumerator FadeIn(GameObject obj){
        Image image = obj.GetComponent<Image>(); 
        while(image.color.a < 1){
            Debug.Log(image.color.a); 
            image.color = new Color(image.color.r, image.color.g, image.color.b, image.color.a + 0.02f); 
            yield return null; 
        }
    }
    public void AnimateButtons(List<GameObject> buttons){
        StartCoroutine(AnimButtons(buttons)); 
    }
    IEnumerator AnimButtons(List<GameObject> buttons){
        yield return new WaitForSecondsRealtime(0.5f); 
        foreach(GameObject button in buttons){
            button.GetComponent<Animator>().SetTrigger("Start"); 
            yield return new WaitForSecondsRealtime(0.5f); 
            Text text = button.transform.GetChild(0).gameObject.GetComponent<Text>(); 
            text.color = new Color(text.color.r, text.color.g, text.color.b, 1); 
            StartCoroutine(GlitchText(button.transform.GetChild(0).gameObject)); 
        }
    }
    public void UpdateText(){
        //StopCoroutine("GlitchText"); 
        glitching = false; 
        Points.GetComponent<Text>().text = GlobalState.Stats.Points.ToString(); 
        StartCoroutine(GlitchText(Points)); 
    }
    IEnumerator GlitchText(GameObject obj){
        glitching = true; 
        Text text = obj.GetComponent<Text>(); 
        text.font = Resources.Load<Font>("Fonts/HACKED"); 
		yield return new WaitForSeconds(0.12f); 
		text.font = Resources.Load<Font>("Fonts/CFGlitchCity-Regular"); 
		yield return new WaitForSeconds(0.1f); 
		text.font = Resources.Load<Font>("Fonts/HACKED"); 
		yield return new WaitForSeconds(0.1f); 
		text.font = Resources.Load<Font>("Fonts/Inconsolata"); 
        yield return new WaitForSecondsRealtime(10); 
        glitching = false; 
    }

    // Update is called once per frame
    void Update()
    {
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class Fade : MonoBehaviour
{
   CanvasGroup screen; 
   void Awake(){
       screen = GetComponent<CanvasGroup>();
   }
   public void onFadeOut(){
       //screen.alpha = 0; 
       StartCoroutine(DoFade(false));
   }
   public void onFadeIn(){
       screen.alpha = 1;
       StartCoroutine(DoFade(true));
   }
    IEnumerator DoFade(bool fadeIn){
        float step = 0.05f; 
        if (fadeIn)
            step = -step; 
        while ((fadeIn && screen.alpha > 0) || (!fadeIn && screen.alpha < 1f)){
            screen.alpha += step;
            yield return null;
        }

        if (!fadeIn) StartCoroutine(Loading()); 
    }
    IEnumerator Loading(){
        yield return new WaitForSecondsRealtime(2f); 
        Text text = transform.GetChild(1).GetComponent<Text>(); 
        float step = 0.05f; 
        while(text.color.a < 1){
            text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a + step); 
            yield return null; 
        }
        yield return new WaitForSecondsRealtime(1f); 
        while(true){
            step*= -1; 
            yield return null; 
            for (int i = 0; i < 30; i++){
                text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a - step); 
                yield return null; 
            }
            yield return new WaitForSecondsRealtime(1f); 
        }
    }
}

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
    }
}

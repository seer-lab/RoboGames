using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class GlowControl : MonoBehaviour
{
    Button button; 
    bool show = false; 
    public bool IsReady {get;set;} = false; 
    // Start is called before the first frame update
    void Start()
    {
        button = transform.parent.GetComponent<Button>(); 
        GetComponent<Image>().color = new Color(1,1,1,0); 
    }

    void FadeIn(){
        show = true; 
        StopAllCoroutines(); 
        StartCoroutine(Fade(true)); 
    }
    void FadeOut(){
        show = false;  
        StopAllCoroutines(); 
        StartCoroutine(Fade(false)); 

    }
    IEnumerator Fade(bool fadeIn){
        float steps = 0.05f; 
        if (!fadeIn)steps*= -1 ;

        Image glow = GetComponent<Image>(); 
        while((fadeIn && glow.color.a < 1) || (!fadeIn && glow.color.a > 0)){
            glow.color = new Color(glow.color.r, glow.color.g, glow.color.b, glow.color.a + steps); 
            yield return null; 
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (button.interactable && !show && IsReady){
            FadeIn(); 
        }
        else if (!button.interactable && show && IsReady){
            FadeOut();
        }
    }
}

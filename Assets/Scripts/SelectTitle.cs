using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class SelectTitle : MonoBehaviour
{
    Image logo; 
    RectTransform pos; 
    Animator charAnim; 
    float maxScale = 0.6f; 
    float minScale = 0.5f; 
    bool isSelected; 
    // Start is called before the first frame update
    void Start()
    {  
        logo = GetComponent<Image>(); 
        pos = GetComponent<RectTransform>(); 
        isSelected = true; 
        charAnim = transform.GetChild(1).GetComponent<Animator>(); 
        Deselect(); 
    }

    public void Select(){
        if (!isSelected){
            isSelected = true; 
            StartCoroutine(AnimateSelect()); 
        }
    }
    public void Deselect(){
        if (isSelected){
            isSelected = false; 
            StartCoroutine(AnimateDeselect()); 
        }
    }
    IEnumerator AnimateSelect(){
        Color finalColor = new Color(1,1,1); 
        charAnim.SetBool("running", true); 
        Text subtext = transform.GetChild(0).GetComponent<Text>(); 
        float scaledif = Math.Abs(maxScale - pos.localScale.x); 
        float colorDif = Math.Abs(finalColor.r - logo.color.r); 
        float frames = 30; 
        while((pos.localScale.x < maxScale && isSelected)){
            pos.localScale = new Vector3(pos.localScale.x + (scaledif/frames), pos.localScale.y + (scaledif/frames) ,1); 
            logo.color = new Color(logo.color.r + (colorDif/frames), logo.color.g + (colorDif/frames), logo.color.b + (colorDif/frames)); 
            subtext.color = logo.color;
            charAnim.GetComponent<Image>().color = logo.color;  
            yield return null; 
        }
    }
    IEnumerator AnimateDeselect(){
        Color finalColor = new Color(0.3f, 0.3f, 0.3f); 
        charAnim.SetBool("running", false); 
        Text subtext = transform.GetChild(0).GetComponent<Text>(); 
        float scaledif = Math.Abs(minScale - pos.localScale.x); 
        float colorDif = Math.Abs(finalColor.r - logo.color.r); 
        float frames = 25; 
        while(pos.localScale.x > minScale && !isSelected){
            pos.localScale = new Vector3(pos.localScale.x - (scaledif/frames), pos.localScale.y - (scaledif/frames) ,1); 
            logo.color = new Color(logo.color.r - (colorDif/frames), logo.color.g - (colorDif/frames), logo.color.b - (colorDif/frames)); 
            subtext.color = logo.color; 
            charAnim.GetComponent<Image>().color = logo.color; 
            yield return null; 
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class Banner : MonoBehaviour
{
   public float fadeTime = 2f; 
   public int index; 
   Image glow; 
   CharacterSelect select; 
   void Awake(){
       glow = GameObject.Find("Glow" + this.gameObject.name).GetComponent<Image>();
       glow.color = new Color(1,1,1,0);  
       select = this.transform.parent.GetComponent<CharacterSelect>(); 
       this.GetComponent<Button>().onClick.AddListener(onClick); 
   }
   void onClick(){
       select.SelectCharacter(index); 
   }
   public void SelectCharacter(){
       StartCoroutine(doFade(true)); 
   }
   public void DeselectCharacter(){
       StartCoroutine(doFade(false)); 
   }
   IEnumerator doFade(bool fadeIn){
       float iterator = 1/((fadeTime)*20); 
       if (!fadeIn){
           iterator*=-1; 
       }
       while((fadeIn) ? glow.color.a < 1: glow.color.a > 0){
           glow.color = new Color(glow.color.r, glow.color.g, glow.color.b, glow.color.a + iterator); 
           yield return null; 
       }
   }
}

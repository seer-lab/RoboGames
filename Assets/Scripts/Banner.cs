using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
/// <summary>
/// Handles Characters when they are being displayed in menus.
/// </summary>
public class Banner : MonoBehaviour
{
   public float fadeTime = 2f; 
   public int index; 
   Image glow; 
   Text textName; 
   CharacterSelect select; 
   bool isRunning = false; 
   bool selected = false; 
   void Awake(){
       glow = GameObject.Find("Glow" + this.gameObject.name).GetComponent<Image>();
       textName = GameObject.Find("Text" + this.gameObject.name).GetComponent<Text>(); 
       textName.color = new Color(1,1,1,0); 
       if (!GlobalState.IsDark) textName.color = Color.black; 
       glow.color = new Color(1,1,1,0);  
       select = this.transform.parent.GetComponent<CharacterSelect>(); 
       this.GetComponent<Button>().onClick.AddListener(onClick); 
   }
   void onClick(){
       select.SelectCharacter(index); 
   }
   public void SelectCharacter(){
       selected = true; 
       StopAllCoroutines(); 
       StartCoroutine(doFade(true)); 
   }
   public void DeselectCharacter(){
       selected = false; 
       StopAllCoroutines();
       StartCoroutine(doFade(false)); 
   }

   /// <summary>
   /// Fades the glow and nametag in and out.
   /// </summary>
   /// <param name="fadeIn">True: Fade in, False: Fade out</param>
   /// <returns></returns>
   IEnumerator doFade(bool fadeIn = true){
       isRunning = true; 
       float iterator = 1/((fadeTime)*20); 
       if (!fadeIn){
           iterator*=-1; 
       }
       while((fadeIn) ? glow.color.a < 1: glow.color.a > 0){
           glow.color = new Color(glow.color.r, glow.color.g, glow.color.b, glow.color.a + iterator); 
           textName.color = new Color(textName.color.r, textName.color.g, textName.color.b, textName.color.a + iterator); 
           yield return null; 
       }
       isRunning = false; 
   }
   void Update(){
       //ensure the alphas of the text and glows 
       //are appropriately if the coroutine is abruptly ended. 
       if (!isRunning && selected){
            textName.color = new Color(textName.color.r, textName.color.g, textName.color.b, 1);  
            glow.color = new Color(1,1,1,1); 
       }
       else if (!isRunning && !selected){
            textName.color = new Color(textName.color.r, textName.color.g, textName.color.b, 0);  
            glow.color = new Color(1,1,1,0); 
       }
   }
}

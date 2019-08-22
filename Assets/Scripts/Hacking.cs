using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class Hacking : Obstacle
{
    float timeToHack = 3f; 
    Sprite[] hackingPhases; 
    Collider2D lastHero; 
    BoxCollider2D hitBox; 
    Animator animator; 
    bool hacking = false; 
    bool finishedHacking = false; 
    bool glitching = false; 
    bool isDark = true; 
    SpriteRenderer visibility; 
    public override string GetObstacleType(){
        return "Hacking"; 
    }
    protected override void Initialize(){
        string path = "Sprites/"; 

        //The panel will show different colours based on its progress.
        hackingPhases = new Sprite[]{
            Resources.Load<Sprite>(path + "hackingOff"),
            Resources.Load<Sprite>(path + "hacking"), 
            Resources.Load<Sprite>(path + "hackingCOmplete")
        };
        hitBox = GetComponent<BoxCollider2D>(); 
        //Animator controls the progress indicator. 
        animator = transform.GetChild(0).GetComponent<Animator>() ;
        animator.speed/= timeToHack; 
        visibility = transform.GetChild(1).GetComponent<SpriteRenderer>();
        if (!GlobalState.IsDark){
            visibility.sprite = Resources.Load<Sprite>("Sprites/transparentbox_white");
            isDark = false; 
        }
    }

    /// <summary>
    /// Deals with if the player is returning or coming for the first time or is 
    /// still in the box.
    /// </summary>
    /// <param name="hero">The players Collider</param>
    void HandleResets(Collider2D hero){
        lastHero = hero; 
        if (!hacking){
            StopAllCoroutines(); 
            StartCoroutine(LoadHack()); 
        }

    }
    /// <summary>
    /// Glitches the Text.
    /// </summary>
    /// <returns></returns>
    IEnumerator GlitchText(){
        glitching = true; 
        TextMesh text = GameObject.Find("Code").GetComponent<TextMesh>(); 
        text.font = Resources.Load<Font>("Fonts/HACKED"); 
        yield return new WaitForSeconds(0.12f); 
        text.font = Resources.Load<Font>("Fonts/CFGlitchCity-Regular"); 
        yield return new WaitForSeconds(0.12f); 
        text.font = Resources.Load<Font>("Fonts/HACKED"); 
        yield return new WaitForSeconds(0.12f); 
        text.font = Resources.Load<Font>("Fonts/Inconsolata"); 
        yield return new WaitForSeconds(0.12f); 
        glitching = false; 
    }
    /// <summary>
    /// Awaits when the player has finished hacking. 
    /// Simply acts as a timer.
    /// </summary>
    /// <returns></returns>
    IEnumerator LoadHack(){
        hacking = true; 
        GetComponent<SpriteRenderer>().sprite = hackingPhases[1]; 
        yield return new WaitForSecondsRealtime(timeToHack); 
        Debug.Log(hacking);
        if (hacking && hitBox.IsTouching(lastHero)){
            finishedHacking = true; 
            GetComponent<SpriteRenderer>().sprite = hackingPhases[2]; 
            visibility.enabled = false; 
            GlobalState.CurrentLevelPoints += stateLib.POINTS_HACKING;
            StartCoroutine(FadeConsole());
            //checkRefresh();
        }
        else {
            GetComponent<SpriteRenderer>().sprite = hackingPhases[0]; 
        }
        hacking = false; 
        
    }
    IEnumerator FadeConsole(){
        SpriteRenderer image = this.GetComponent<SpriteRenderer>(); 
        float step = 0.05f; 
        yield return new WaitForSecondsRealtime(2f); 
        while(image.color.a > 0){
            image.color = new Color(image.color.r, image.color.g, image.color.b ,image.color.a - step); 
            yield return null;
        }

    }

    /// <summary>
    /// Used for linking ohter consoles with eachother.
    /// </summary>
    void checkRefresh(){
       GameObject[] hacks =  GameObject.FindGameObjectsWithTag("hacking"); 
       bool hackComplete = true; 
       foreach(GameObject hack in hacks){
           if (!hack.GetComponent<Hacking>().finishedHacking){
               hackComplete = false; 
               break; 
           }
       }
       if (hackComplete) {
           TextMesh text = GameObject.Find("Code").GetComponent<TextMesh>(); 
           text.font = Resources.Load<Font>("Fonts/Inconsolata"); 
           GameObject.Find("CodeScreen").GetComponent<LevelGenerator>().DrawInnerXmlLinesToScreen();

       }
    }
    protected override void UpdateProtocol(){
        if (hacking && !finishedHacking && lastHero != null && !hitBox.IsTouching(lastHero)){
            StopAllCoroutines(); 
            hacking = false; 
            GetComponent<SpriteRenderer>().sprite = hackingPhases[0];
            //if (glitching) StartCoroutine(GlitchText()); 
        }
        if (!glitching && !finishedHacking){
            //StartCoroutine(GlitchText()); 
        }
        animator.SetBool("hacking", hacking); //Let's the progress indicator know if we're hacking
    }
    
    void OnTriggerEnter2D(Collider2D collidingObj){
        if (collidingObj.name == "Hero" && !finishedHacking){
            HandleResets(collidingObj); 
        }
    }
    public override void SetPosition(){
        base.SetPosition(); 
        transform.GetChild(1).GetComponent<Transform>().localScale = new Vector3(8,(properties.linespacing*10)*0.35f, 1); 
    }

}

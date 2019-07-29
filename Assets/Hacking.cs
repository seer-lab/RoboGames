using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hacking : Obstacle
{
    float timeToHack = 3f; 
    Sprite[] hackingPhases; 
    Collider2D lastHero; 
    BoxCollider2D hitBox; 
    bool hacking = false; 
    bool finishedHacking = false; 
    public override string GetObstacleType(){
        return "Hacking"; 
    }
    protected override void Initialize(){
        string path = "Sprites/"; 
        hackingPhases = new Sprite[]{
            Resources.Load<Sprite>(path + "hackingOff"),
            Resources.Load<Sprite>(path + "hacking"), 
            Resources.Load<Sprite>(path + "hackingCOmplete")
        };
        hitBox = GetComponent<BoxCollider2D>(); 
    }

    void HandleResets(Collider2D hero){
        if (lastHero != null && hitBox.IsTouching(lastHero)){
            return; 
        }
        if (!finishedHacking){
            StopAllCoroutines(); 
            lastHero = hero; 
            StartCoroutine(LoadHack()); 
        }

    }
    IEnumerator LoadHack(){
        hacking = true; 
        GetComponent<SpriteRenderer>().sprite = hackingPhases[1]; 
        yield return new WaitForSecondsRealtime(timeToHack); 
        if (hitBox.IsTouching(lastHero)){
            finishedHacking = true; 
            GetComponent<SpriteRenderer>().sprite = hackingPhases[2]; 
        }
        else {
            GetComponent<SpriteRenderer>().sprite = hackingPhases[0]; 
        }
        hacking = false; 
        
    }
    protected override void UpdateProtocol(){
        if (hacking && !finishedHacking && !hitBox.IsTouching(lastHero)){
            StopAllCoroutines(); 
            hacking = false; 
            GetComponent<SpriteRenderer>().sprite = hackingPhases[0];
        }
    }
    
    void OnTriggerEnter2D(Collider2D collidingObj){
        if (collidingObj.name == "Hero" && !finishedHacking){
            HandleResets(collidingObj); 
        }
    }

}

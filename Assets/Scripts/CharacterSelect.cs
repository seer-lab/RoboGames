﻿using System.Xml.Schema;
using System.Collections;
using System.Collections.Generic;
using System; 
using UnityEngine;
using UnityEngine.UI; 
using UnityEngine.SceneManagement; 
public class CharacterSelect : MonoBehaviour
{
    // Start is called before the first frame update
    public const int NUM_CHARACTERS = 3; 
    float[] xPositions; 
    public GameObject[] characterObjects = new GameObject[NUM_CHARACTERS]; 
    public Text[] characterNames = new Text[NUM_CHARACTERS]; 
    Banner[] characters; 
    GameObject disk; 
    int indexOfSelected; 
    string[] names; 
    void Start()
    {
        if (!GlobalState.IsDark){
            GameObject.Find("BackgroundCanvas").transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/circuit_board_light"); 
            transform.Find("Title").GetComponent<Text>().color = Color.black; 
        }
        disk = this.transform.GetChild(0).gameObject; 
        names = new String[]{"Robot", "Boy", "Girl"};
        xPositions = new float[NUM_CHARACTERS]; 
        for (int i = 1; i <= NUM_CHARACTERS; i++){
            xPositions[i-1] = this.transform.GetChild(i).GetComponent<RectTransform>().position.x; 
        }

        characters = new Banner[NUM_CHARACTERS]; 
        for (int i = 0; i < NUM_CHARACTERS; i++){
            characters[i] = characterObjects[i].GetComponent<Banner>(); 
        }
        indexOfSelected = 0; 
        characters[indexOfSelected].SelectCharacter(); 
    }
     IEnumerator LoadGame(){
        GameObject.Find("Fade").GetComponent<Fade>().onFadeOut(); 
        yield return new WaitForSecondsRealtime(1f); 
		Debug.Log("CharacterSelect.cs 42 - FilePath = " + GlobalState.FilePath);
		Debug.Log("CharacterSelect.cs 42 - CurrentBUGLevel = " + GlobalState.CurrentBUGLevel);
        SceneManager.LoadScene("Cinematic");

    }
    /// <summary>
    /// Tells the appropriate banners to glow/stop glowing, 
    /// and will move the player disk to the appropriate place.
    /// </summary>
    /// <param name="index"></param>
    public void SelectCharacter(int index){
        if (index == indexOfSelected){
            GlobalState.Character = names[indexOfSelected]; 
            StartCoroutine(LoadGame()); 
        }
        else {
            StopAllCoroutines(); 
        }
        characters[indexOfSelected].DeselectCharacter(); 
        indexOfSelected = index; 
        characters[indexOfSelected].SelectCharacter();
        StartCoroutine(MoveDisk()); 
    }
    /// <summary>
    /// Moves the player disk to the correct character.
    /// </summary>
    /// <returns></returns>
    IEnumerator MoveDisk(){
        float time = 30f; 
        float finalPos = xPositions[indexOfSelected]; 
        RectTransform pos = disk.GetComponent<RectTransform>();
        float dist = finalPos - pos.position.x;  
        while(Math.Abs(pos.position.x - finalPos) > 0.1f){
            pos.position = new Vector3(pos.position.x + dist/time, pos.position.y, pos.position.z);
            yield return null; 
        }
    }
    // Update is called once per frame
    void Update()
    {
        //Checks Keyboard input for adjusting the character. 
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)){
            SelectCharacter((indexOfSelected+1 > NUM_CHARACTERS-1) ? 0: indexOfSelected+1); 
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)){
            SelectCharacter((indexOfSelected-1 < 0) ? NUM_CHARACTERS-1: indexOfSelected-1); 
        }
        else if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return)){
            GlobalState.Character = names[indexOfSelected]; 
            StartCoroutine(LoadGame());
        }
    }
}

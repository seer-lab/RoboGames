using System.Collections;
using System.Collections.Generic;
using System; 
using UnityEngine;

public class CharacterSelect : MonoBehaviour
{
    // Start is called before the first frame update
    public const int NUM_CHARACTERS = 3; 
    float[] xPositions; 
    public GameObject[] characterObjects = new GameObject[NUM_CHARACTERS]; 
    Banner[] characters; 
    GameObject disk; 
    int indexOfSelected; 
    void Start()
    {
        disk = this.transform.GetChild(0).gameObject; 

        xPositions = new float[NUM_CHARACTERS]; 
        for (int i = 1; i <= NUM_CHARACTERS; i++){
            xPositions[i-1] = this.transform.GetChild(i).GetComponent<RectTransform>().position.x; 
        }

        characters = new Banner[NUM_CHARACTERS]; 
        for (int i = 0; i < NUM_CHARACTERS; i++){
            characters[i] = characterObjects[i].GetComponent<Banner>(); 
        }
        characters[0].SelectCharacter(); 
    }
    
    public void SelectCharacter(int index){
        StopAllCoroutines(); 
        characters[indexOfSelected].DeselectCharacter(); 
        indexOfSelected = index; 
        Debug.Log("Index of Selected: " + indexOfSelected); 
        characters[indexOfSelected].SelectCharacter();
        StartCoroutine(MoveDisk()); 
    }
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
        
    }
}

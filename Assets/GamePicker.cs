using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePicker : MonoBehaviour
{
    int indexSelcted; 
    public GameObject[] Items = new GameObject[2]; 
    SelectTitle[] titles = new SelectTitle[2]; 
    // Start is called before the first frame update
    void Start()
    {
        indexSelcted = 0; 
        for (int i = 0; i < Items.Length; i++){
            titles[i] = Items[i].GetComponent<SelectTitle>(); 
        }
        titles[indexSelcted].Select(); 
    }
    public void SelectItem(int index){
        if (index != indexSelcted){
            titles[indexSelcted].Deselect(); 
            indexSelcted = index; 
            titles[indexSelcted].Select(); 
        }

    }
    // Update is called once per frame
    void Update()
    {
        int currentIndex = indexSelcted; 
        if (Input.GetKeyDown(KeyCode.RightArrow)){
            indexSelcted ++; 
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow)){
            if (indexSelcted >0)
                indexSelcted--; 
            else indexSelcted++; 
        }
        if (currentIndex != indexSelcted%2){
            indexSelcted = indexSelcted %2; 
            titles[indexSelcted].Select(); 
            titles[currentIndex].Deselect(); 
        }
    }
}

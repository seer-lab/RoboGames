using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI; 
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    Image background;
    private Sprite light, dark; 
    public void ToggleLight()
    {
        background.sprite = light; 
    }
    public void ToggleDark()
    {
        background.sprite = dark; 
    }
    // Start is called before the first frame update
    void Start()
    {
        string path = "Sprites/";
        light = Resources.Load<Sprite>(path + "circuit_board_light");
        dark = Resources.Load<Sprite>(path + "circuit_board_dark");
        background = this.transform.GetChild(0).GetComponent<Image>(); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

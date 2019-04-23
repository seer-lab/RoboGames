using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class MenuButton : MonoBehaviour
{
    private Image image;
    private Sprite blueButton, greenButton;
    public Menu Callback { get; set; } 
    public int Index { get; set; }
    public void LoadText(string text)
    {
        if (image == null) Start(); 
        this.transform.GetChild(0).GetComponent<Text>().text = text; 
    }
    public void ToggleInactive()
    {
        image.color = Color.grey; 
    }
    public void ToggleBlue()
    {
        image.sprite = blueButton; 
    }
    public void ToggleGreen()
    {
        image.sprite = greenButton; 
    }

    private void OnClick()
    {
        Callback.HandleClick(Index); 
    }
    public void Hide()
    {
        image.enabled = false; 
    }
    public void Show()
    {
       image.enabled = true; 
    }
    
    // Start is called before the first frame update
    void Start()
    {
        image = this.GetComponent<Image>();
        string path = "Sprites/UIpack/PNG/";
        blueButton = Resources.Load<Sprite>(path + "blue_button02");
        greenButton = Resources.Load<Sprite>(path + "green_button02");
        this.GetComponent<Button>().onClick.AddListener(OnClick); 
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}

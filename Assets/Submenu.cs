using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI; 
using UnityEngine;

public class Submenu : MonoBehaviour
{
    public Vector3 ParentPosition { get; set; } 
    List<MenuButton> buttons;
    public GameObject background; 
    public void OpenSubmenu()
    {
        foreach(MenuButton button in buttons)
        {
            button.Show(); 
        }
        background.GetComponent<Image>().enabled = true; 
    }
    public void CloseSubmenu()
    {
        if (buttons != null)
        {
            foreach (MenuButton button in buttons)
            {
                button.Hide();
            }
        }
        background.GetComponent<Image>().enabled = false;
        Debug.Log("Clearing");
    }
    // Start is called before the first frame update
    void Start()
    {
        background = Instantiate(background);
        background.transform.SetParent(this.transform); 
        background.GetComponent<Image>().enabled = false;
        background.transform.localPosition = new Vector3(0,0, 0); 
    }
    private void ClearButtons()
    {
        if (buttons != null)
        {
            foreach (MenuButton button in buttons)
            {
                Destroy(button.gameObject);
            }
        }
        buttons = new List<MenuButton>();

    }
    public void LoadButtons(string[] names)
    {
        CloseSubmenu();
        ClearButtons(); 
        string path = "MenuPrefabs/MenuButton";
        float sizex = 400f;
        float sizey = 100f;
        for (int i = 0; i < names.Length; i++)
        {
            GameObject temp = Instantiate(Resources.Load<GameObject>(path));
            temp.transform.position = new Vector3(ParentPosition.x + sizex*1.4f, ParentPosition.y - (sizey * i + 20), ParentPosition.z);
            temp.transform.SetParent(this.transform);
            buttons.Add(temp.GetComponent<MenuButton>());
            buttons[i].LoadText(names[i]);
        }
        if (names.Length > 0)
        {
            Vector3 btnPos = buttons[0].transform.localPosition;
            background.transform.localPosition = new Vector3(btnPos.x, btnPos.y - sizey / 2, 0);  
        }
        OpenSubmenu(); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

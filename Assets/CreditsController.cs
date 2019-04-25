using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.SceneManagement; 
using UnityEngine;

public class CreditsController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(GlobalState.GameState);
        FileInfo fi = new FileInfo(@"onleveldata/credits.txt");
        StreamReader sr = fi.OpenText();
        string text;
        string credtext = ""; 
        do
        {
            text = sr.ReadLine();
            credtext += text + "\n";
        } while (text != null);
        this.GetComponent<TextMesh>().text = credtext;
        this.GetComponent<Animator>().SetBool("Ended", true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown && !Input.GetMouseButton(0))
        {
            GlobalState.GameState = 0;
           // this.GetComponent<TextMesh>().text = "";
           // this.GetComponent<Animator>().SetBool("Ended", false);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("MainMenu"); 
        }
    }
}

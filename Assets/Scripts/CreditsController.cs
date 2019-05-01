using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.SceneManagement; 
using UnityEngine;

/// <summary>
/// Controls the credits functionality. 
/// </summary>
public class CreditsController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
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
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("MainMenu"); 
        }
    }
}

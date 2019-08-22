using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.SceneManagement; 
using UnityEngine;
using UnityEngine.UI; 

/// <summary>
/// Controls the credits functionality. 
/// </summary>
public class CreditsController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        string credtext = "";
        //Reads the credits file. 
        #if UNITY_EDITOR || UNITY_STANDALONE && !UNITY_WEBGL
            FileInfo fi = new FileInfo(Path.Combine(Application.streamingAssetsPath, GlobalState.GameMode + "leveldata") + "/credits.txt");
            StreamReader sr = fi.OpenText();
            string text;
 
            do
            {
                text = sr.ReadLine();
                credtext += text + "\n";
            } while (text != null);
        #endif

        #if UNITY_WEBGL
            WebHelper.i.url = stringLib.SERVER_URL + stringLib.STREAMING_ASSETS + GlobalState.GameMode.ToLower() + "leveldata/credits.txt";
            WebHelper.i.GetWebDataFromWeb();
            credtext = WebHelper.i.webData;
        #endif

        this.GetComponent<TextMesh>().text = credtext;
        this.GetComponent<Animator>().SetBool("Ended", true);
        if (!GlobalState.IsDark){
            GameObject.Find("BackgroundCanvas").transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/circuit_board_light"); 
            GameObject.Find("Credits").GetComponent<TextMesh>().color = Color.black; 
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown && !Input.GetMouseButton(0))
        {
            GlobalState.GameState = 0;
        }
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1))
        {
            SceneManager.LoadScene("MainMenu"); 
        }
    }
}

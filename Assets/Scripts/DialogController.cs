using System.Runtime.Versioning;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using UnityEngine.Video; 
using UnityEngine.SceneManagement; 
using UnityEngine.Networking;
using System;
using System.IO; 
using System.Runtime.InteropServices;
/// <summary>
/// Controls the dialog during the Intro Scene.
/// </summary>
public class DialogController : MonoBehaviour
{
    public GameObject girlDialog, boyDialog, botDialog; 
    List<string> lines; 
    VideoPlayer player; 
    List<string> actorOrder; 
    bool started = false; 
    int index = 0; 
    // Start is called before the first frame update
    /// <summary>
    /// Flips the dialog box in its opposite direction.
    /// </summary>
    /// <param name="dialog">Dialog box that should be flipped</param>
    private void FlipDialog(GameObject dialog){
        dialog.GetComponent<RectTransform>().localScale = new Vector3(-dialog.GetComponent<RectTransform>().localScale.x,1,1); 
        dialog.transform.GetChild(0).GetComponent<RectTransform>().localScale = new Vector3(- dialog.transform.GetChild(0).GetComponent<RectTransform>().localScale.x ,1,1); 
    }
    void Start()
    {
        player = GameObject.Find("Video Player").GetComponent<VideoPlayer>(); 
        
        #if UNITY_WEBGL                        
            if (GlobalState.GameMode == "bug"){
                if(GlobalState.GameState == stateLib.GAMESTATE_GAME_END){
                    player.url = stringLib.SERVER_URL + stringLib.STREAMING_ASSETS + stringLib.MOVIE_BUG_END;
                    FlipDialog(boyDialog); 
                    botDialog.GetComponent<RectTransform>().localPosition = new Vector3(350,70,0); 
                    girlDialog.GetComponent<RectTransform>().localPosition = new Vector3(450, 70, 0); 
                    boyDialog.GetComponent<RectTransform>().localPosition = new Vector3(600,70,0); 
                }else{
                    player.url = stringLib.SERVER_URL + stringLib.STREAMING_ASSETS + stringLib.MOVIE_BUG;
                    girlDialog.GetComponent<RectTransform>().localPosition = new Vector3(250, 250, 0); 
                    boyDialog.GetComponent<RectTransform>().localPosition = new Vector3(-200, 250, 0); 
                    FlipDialog(girlDialog); 
                    botDialog.GetComponent<RectTransform>().localPosition = new Vector3(400,250,0); 
                
                }
            }else{
                if(GlobalState.GameState == stateLib.GAMESTATE_GAME_END){
                    player.url = stringLib.SERVER_URL + stringLib.STREAMING_ASSETS + stringLib.MOVIE_ON_END;
                    FlipDialog(girlDialog); 
                    FlipDialog(botDialog); 
                    botDialog.GetComponent<RectTransform>().localPosition = new Vector3(200,70,0); 
                    girlDialog.GetComponent<RectTransform>().localPosition = new Vector3(0,70,0); 
                    boyDialog.GetComponent<RectTransform>().localPosition = new Vector3(-400,70,0); 
                }else{
                    player.url = stringLib.SERVER_URL + stringLib.STREAMING_ASSETS + stringLib.MOVIE_ON;
                    FlipDialog(girlDialog); 
                    FlipDialog(botDialog); 
                }

                //player.clip = Resources.Load<VideoClip>(stringLib.SERVER_URL + filepathON);
            }
            
        #endif

        #if (UNITY_EDITOR || UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN) && !UNITY_WEBGL
        //select the correct video and placement of dialog for the correct game mode and 
        //condition. This scene is used for the beginning and the ending of the game.
            if (GlobalState.GameMode == "bug"){
                if (GlobalState.GameState == stateLib.GAMESTATE_GAME_END){
                    player.clip = Resources.Load<VideoClip>("Video/RoboBugEnding"); 
                    FlipDialog(boyDialog); 
                    botDialog.GetComponent<RectTransform>().localPosition = new Vector3(350,70,0); 
                    girlDialog.GetComponent<RectTransform>().localPosition = new Vector3(450, 70, 0); 
                    boyDialog.GetComponent<RectTransform>().localPosition = new Vector3(600,70,0); 
                }
                else {
                    player.clip = Resources.Load<VideoClip>("Video/RoboBugIntro_1"); 
                    girlDialog.GetComponent<RectTransform>().localPosition = new Vector3(250, 250, 0); 
                    boyDialog.GetComponent<RectTransform>().localPosition = new Vector3(-200, 250, 0); 
                    FlipDialog(girlDialog); 
                    botDialog.GetComponent<RectTransform>().localPosition = new Vector3(400,250,0); 
                }
            }
            else if (GlobalState.GameState == stateLib.GAMESTATE_GAME_END){
                player.clip = Resources.Load<VideoClip>("Video/RobotONEnding");
                FlipDialog(girlDialog); 
                FlipDialog(botDialog); 
                botDialog.GetComponent<RectTransform>().localPosition = new Vector3(200,70,0); 
                girlDialog.GetComponent<RectTransform>().localPosition = new Vector3(0,70,0); 
                boyDialog.GetComponent<RectTransform>().localPosition = new Vector3(-400,70,0); 
            }
            else{
                FlipDialog(girlDialog); 
                FlipDialog(botDialog); 
            }
        #endif
        ReadFile(); 
    }
    // Update is called once per frame
    void Update()
    {
        //time delay is added to prevent the dialog from showing 
        //while the player is loading. 
        if (!player.isPlaying && Time.timeSinceLevelLoad > 3 && !started){
            StartCoroutine(ShowDialog(GetDialog(actorOrder[index]))); 
            started = true; 
        }
        else if (!player.isPlaying && Time.timeSinceLevelLoad > 3 && (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Return))){
            NextDialog(); 
        }
        else if(player.isPlaying &&( Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0))){
            EndScene(); 
        }
    }
    /// <summary>
    /// Switches and fades out the scene.
    /// </summary>
    void EndScene(){
        GameObject.Find("Fade").GetComponent<Fade>().onFadeOut(); 
        StartCoroutine(WaitForSwitchScene()); 
    }
    IEnumerator WaitForSwitchScene(){
        yield return new WaitForSeconds(1f); 
        if (GlobalState.GameState == stateLib.GAMESTATE_GAME_END)
            SceneManager.LoadScene("Credits"); 
        else 
            SceneManager.LoadScene("MainMenu"); 
    }
    /// <summary>
    /// Uses the actor order to show and hide the correct dialog(s).
    /// </summary>
    void NextDialog(){
        if (index +1 == lines.Count){
            EndScene(); 
            return; 
        }
        if (actorOrder[index] == actorOrder[index+1]){
            index++; 
            StartCoroutine(ShowDialog(GetDialog(actorOrder[index]))); 
        }else{
            StopAllCoroutines(); 
            StartCoroutine(HideDialog(GetDialog(actorOrder[index]))); 
            index++; 
            StartCoroutine(ShowDialog(GetDialog(actorOrder[index]))); 
        }
    }
    /// <summary>
    ///  Simplifys getting the correct Dialog
    /// </summary>
    /// <param name="name">Name the dialog belongs to</param>
    /// <returns>The Dialog GameObject associated with that name</returns>
    GameObject GetDialog(string name){
        if (name == "Boy") return boyDialog; 
        else if (name == "Girl") return girlDialog; 
        else return botDialog; 
    }
    /// <summary>
    /// Reads and stores the information for the actors and lines.
    /// </summary>
    void ReadFile(){
        //Read the appropriate file.
        string bug = ""; 
        string prefix = "Intro"; 
        if( GlobalState.GameMode == "bug") bug = "Bug"; 
        if (GlobalState.GameState == stateLib.GAMESTATE_GAME_END) prefix = "Ending"; 
        string filepath = "StreamingAssets/onleveldata/" + prefix + bug + ".txt";
        
        actorOrder = new List<string>(); 
        lines = new List<string>(); 

        //Check each line for the name of the speaker and add it to the actor list. 
        //Remove the name indicator and add the speech to the lines list. 
        #if (UNITY_EDITOR || UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN) && !UNITY_WEBGL
            filepath = Path.Combine(Application.streamingAssetsPath, "onleveldata/"+ prefix + bug +".txt");
            
            using (StreamReader reader = new StreamReader(filepath)){
                while(!reader.EndOfStream){
                    string line = reader.ReadLine(); 
                    if (line.Contains("$Boy")){
                        actorOrder.Add("Boy"); 
                        line = "Guy: " + line.Remove(0,line.IndexOf(':') + 1); 
                    }
                    else if (line.Contains("$Girl")){
                        actorOrder.Add("Girl"); 
                        line = "Ivy: " + line.Remove(0,line.IndexOf(':')+1); 
                    }
                    else{
                        actorOrder.Add("Robot"); 
                        line = "V.I.: " + line.Remove(0,line.IndexOf(':')+1); 
                    }
                    lines.Add(line); 
                }
            }
        #endif

        #if UNITY_WEBGL
            WebHelper.i.url = stringLib.SERVER_URL + filepath;
            WebHelper.i.GetWebDataFromWeb();
            filepath = WebHelper.i.webData;

            String[] linesS = filepath.Split('\n');
            for(int i = 0; i < linesS.Length - 1; i++){
                String[] scriptLines = linesS[i].Split('\r');
                if(scriptLines[0].Contains("$Boy")){
                    actorOrder.Add("Boy");
                    scriptLines[0] = "Guy: " + scriptLines[0].Remove(0, scriptLines[0].IndexOf(':'));
                }else if (scriptLines[0].Contains("$Girl")){
                    actorOrder.Add("Girl"); 
                    scriptLines[0] = "Ivy: " + scriptLines[0].Remove(0,scriptLines[0].IndexOf(':')+1); 
                }else{
                    actorOrder.Add("Robot"); 
                    scriptLines[0] = "V.I.: " + scriptLines[0].Remove(0,scriptLines[0].IndexOf(':')+1); 
                }
                lines.Add(scriptLines[0]);
            }
            //Debug.Log("DialogController: ReadFile() WEBGL");
        #endif
    }

    /// <summary>
    /// Fades and scales the dialog box out.
    /// </summary>
    /// <param name="dialog">The Dialog box to animate out</param>
    /// <returns></returns>
    IEnumerator HideDialog(GameObject dialog){
        //Initialization
        RectTransform transformm = dialog.GetComponent<RectTransform>(); 
        Image image = dialog.GetComponent<Image>(); 
        CanvasGroup canvas = dialog.GetComponent<CanvasGroup>(); 
        
        //adjust the scale and the alpha to hide the dialog
        transform.localScale = new Vector3(1,1,1); 
        canvas.alpha = 1; 
        float frames = 10; 
        while(canvas.alpha > 0){
            canvas.alpha -= (1/frames); 
            transform.localScale = new Vector3(transform.localScale.x - (0.25f/frames), transform.localScale.y - (0.25f/frames), transform.localScale.z - (0.25f/frames)); 
            yield return null; 
        }
        dialog.transform.GetChild(0).GetComponent<Text>().text = ""; 
    }
    /// <summary>
    /// Fades and Scales the dialog box in.
    /// </summary>
    /// <param name="dialog">The dialog box to animate in</param>
    /// <returns></returns>
    IEnumerator ShowDialog(GameObject dialog){
        //Initialization
        RectTransform transformm = dialog.GetComponent<RectTransform>(); 
        Image image = dialog.GetComponent<Image>(); 
        CanvasGroup canvas = dialog.GetComponent<CanvasGroup>(); 
        dialog.transform.GetChild(0).GetComponent<Text>().text = lines[index];  
        //Initialize values 
        transform.localScale = new Vector3(0,0,0); 
        canvas.alpha = 0;    //over push the animation
        float frames = 15; 

        while(transform.localScale.x < 1.2){
            transform.localScale = new Vector3(transform.localScale.x + (1/frames), transform.localScale.y + (1/frames), transform.localScale.z + (1/frames)); 
            if (canvas.alpha < 1)
                canvas.alpha+= (2/frames); 
            yield return null; 
        }

        //bring back 
        while(transform.localScale.x > 1){
            transform.localScale = new Vector3(transform.localScale.x - (1/frames), transform.localScale.y - (1/frames), transform.localScale.z - (1/frames)); 
            yield return null; 
        }
    }

    void SetVideo(string filename){
        #if UNITY_WEBGL && !UNITY_EDITOR
            String url = ""; 
            if(url == "" || url == null){
                Debug.Log("Playing Movie from Server");
                player.url = stringLib.SERVER_URL + stringLib.STREAMING_ASSETS + filename;
            }else{
                Debug.Log("Playing Movie from cache, url: " + url + ", length: " + url.Length);
                player.url = url;
            }
        #endif
    }
}

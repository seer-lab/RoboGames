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

public class DialogController : MonoBehaviour
{
    public GameObject girlDialog, boyDialog, botDialog; 
    List<string> lines; 
    VideoPlayer player; 
    List<string> actorOrder; 
    bool started = false; 
    int index = 0; 
    // Start is called before the first frame update

    private void FlipDialog(GameObject dialog){
        dialog.GetComponent<RectTransform>().localScale = new Vector3(-1,1,1); 
        dialog.transform.GetChild(0).GetComponent<RectTransform>().localScale = new Vector3(-1,1,1); 
    }
    void Start()
    {
        // string filepathON ="";
        // string filepathBug = "";
        player = GameObject.Find("Video Player").GetComponent<VideoPlayer>(); 
        
        #if UNITY_WEBGL && !UNITY_EDITOR                    
            // filepathON = "StreamingAssets/IntroScene.mp4";
            // filepathBug = "StreamingAssets/RoboBugIntro_1.mp4";
            //Debug.Log("OldMenu: Update() WEBGL AND WINDOW");
            
            if (GlobalState.GameMode == "bug"){
                SetVideo(stringLib.MOVIE_BUG);
                // String url = WebHelper.i.GetMovieFromIndexDB(stringLib.MOVIE_BUG);
                // Debug.Log("URL : " + url);
                // if(!url.Contains("ERROR") || url.Contains("") && url.Length > 0){
                //     Debug.Log("Playing Movie from cache, url: " + url + ", length: " + url.Length);
                //     player.url = url;
                // }else{
                //     Debug.Log("Playing Movie from Server");
                //     player.url = stringLib.SERVER_URL + stringLib.STREAMING_ASSETS + stringLib.MOVIE_BUG;
                // } 
                //player.clip = Resources.Load<VideoClip>(stringLib.SERVER_URL + filepathBug); 
                girlDialog.GetComponent<RectTransform>().localPosition = new Vector3(150, 250, 0);
                boyDialog.GetComponent<RectTransform>().localPosition = new Vector3(-300, 250, 0);  
                FlipDialog(girlDialog); 
                botDialog.GetComponent<RectTransform>().localPosition = new Vector3(500,250,0); 
                
            }else{
                SetVideo(stringLib.MOVIE_ON);
                FlipDialog(botDialog); 
                FlipDialog(boyDialog); 
                //player.clip = Resources.Load<VideoClip>(stringLib.SERVER_URL + filepathON);
            }
            
        #endif

        #if (UNITY_EDITOR || UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN) && !UNITY_WEBGL
            if (GlobalState.GameMode == "bug"){
                player.clip = Resources.Load<VideoClip>("Video/RoboBugIntro_1"); 
                girlDialog.GetComponent<RectTransform>().localPosition = new Vector3(250, 250, 0); 
                boyDialog.GetComponent<RectTransform>().localPosition = new Vector3(-200, 250, 0); 
                FlipDialog(girlDialog); 
                botDialog.GetComponent<RectTransform>().localPosition = new Vector3(400,250,0); 
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
    void EndScene(){
        GameObject.Find("Fade").GetComponent<Fade>().onFadeOut(); 
        StartCoroutine(WaitForSwitchScene()); 
    }
    IEnumerator WaitForSwitchScene(){
        yield return new WaitForSeconds(1f); 
        SceneManager.LoadScene("MainMenu"); 
    }
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
    GameObject GetDialog(string name){
        if (name == "Boy") return boyDialog; 
        else if (name == "Girl") return girlDialog; 
        else return botDialog; 
    }

    void ReadFile(){
        string bug = ""; 
        if( GlobalState.GameMode == "bug") bug = "bug"; 
        string filepath = "StreamingAssets/onleveldata/Intro" + bug + ".txt";
        actorOrder = new List<string>(); 
        lines = new List<string>(); 
        #if (UNITY_EDITOR || UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN) && !UNITY_WEBGL
            filepath = Path.Combine(Application.streamingAssetsPath, "onleveldata/Intro" + bug +".txt");
            
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

    IEnumerator HideDialog(GameObject dialog){
        //Initialization
        RectTransform transformm = dialog.GetComponent<RectTransform>(); 
        Image image = dialog.GetComponent<Image>(); 
        CanvasGroup canvas = dialog.GetComponent<CanvasGroup>(); 
        
        transform.localScale = new Vector3(1,1,1); 
        //image.color = new Color(image.color.r, image.color.g, image.color.b, 1); 
        canvas.alpha = 1; 
        float frames = 10; 
        while(canvas.alpha > 0){
            canvas.alpha -= (1/frames); 
            transform.localScale = new Vector3(transform.localScale.x - (0.25f/frames), transform.localScale.y - (0.25f/frames), transform.localScale.z - (0.25f/frames)); 
            yield return null; 
        }
        dialog.transform.GetChild(0).GetComponent<Text>().text = ""; 
    }
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
            if(GlobalState.GameMode == "bug"){
                url = GlobalState.URL_MOVIE_BUG;
            }else{
                url = GlobalState.URL_MOVIE_ON;
            }            
            Debug.Log("URL : " + url + " Movie: " + filename);

        
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

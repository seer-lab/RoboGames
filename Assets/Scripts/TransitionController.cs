using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO; 
using UnityEngine.SceneManagement; 
using UnityEngine.UI;
using System.Runtime.InteropServices;
using UnityEngine.Networking;
using System.Text;
using System;

public class TransitionController : MonoBehaviour
{
    
    public GameObject girlDialog, boyDialog, botDialog; 
    List<string> lines; 
    List<string> actorOrder; 
    bool started = false; 
    string image; 
    int index = 0; 
    #if UNITY_WEBGL && !UNITY_EDITOR
        [DllImport("__Internal")]
        private static extern string GetData(string url);
    #endif
    string webdata;
    IEnumerator GetXMLFromServer(string url){
        UnityWebRequest www = UnityWebRequest.Get(url);
        www.SendWebRequest();
        System.Threading.Thread.Sleep(stringLib.DOWNLOAD_TIME);        
        if(www.isNetworkError || www.isHttpError){
            Debug.Log(www.error);
        }else{
            Debug.Log(www.downloadHandler.text);
            webdata = www.downloadHandler.text;
        }
        yield return new WaitForSeconds(0.5f);
    }
    // Start is called before the first frame update
    void Start()   
    {
        if (GlobalState.GameMode == "bug"){

            girlDialog.GetComponent<RectTransform>().localPosition = new Vector3(150, 250, 0); 
            boyDialog.GetComponent<RectTransform>().localPosition = new Vector3(-300, 250, 0); 
            botDialog.GetComponent<RectTransform>().localPosition = new Vector3(500,250,0); 
        }
        ReadFile(); 
        transform.Find("RawImage").GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/TransitionImages/" + image); 
        StartCoroutine(ShowDialog(GetDialog(actorOrder[index]))); 
        started = true; 
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return)){
            NextDialog(); 
        }
    }
    void EndScene(){
        GameObject.Find("Fade").GetComponent<Fade>().onFadeOut(); 
        StartCoroutine(WaitForSwitchScene()); 
    }
    IEnumerator WaitForSwitchScene(){
        yield return new WaitForSeconds(1f); 
        SceneManager.LoadScene("newgame"); 
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
    float[] GetLinePosition(string line){
        string[] values = line.Split(' ');
        float[] pos = new float[3];  
        for (int i = 1; i <= 3; i++){
            float.TryParse(values[i], out pos[i-1]); 
        }
        return pos; 
    }
    void ReadFile(){
        string bug = "on"; 
        if( GlobalState.GameMode == "bug") bug = "bug";
        string filepath = Path.Combine(Application.streamingAssetsPath, GlobalState.level.FileName.Remove(GlobalState.level.FileName.IndexOf('.')) + ".txt");
        actorOrder = new List<string>(); 
        lines = new List<string>(); 
        bool positionLine = false;

        #if UNITY_WEBGL

            filepath ="StreamingAssets/" + bug + "leveldata/" +  GlobalState.level.FileName.Remove(GlobalState.level.FileName.IndexOf('.')) + ".txt";
            WebHelper.i.url = stringLib.SERVER_URL + filepath;
            WebHelper.i.GetWebDataFromWeb();
            filepath = WebHelper.i.webData;

            byte[] byteArr = Encoding.ASCII.GetBytes(filepath);
            MemoryStream stream = new MemoryStream(byteArr);

            using (StreamReader reader = new StreamReader(stream)){
                image = reader.ReadLine(); 
                while(!reader.EndOfStream){
                    string line = reader.ReadLine(); 
                    if (line.Contains("$Boy")){
                        actorOrder.Add("Boy"); 
                        line = line.Remove(0,line.IndexOf(':')); 
                    }
                    else if (line.Contains("$Girl")){
                        actorOrder.Add("Girl"); 
                    }
                    else if (line.Contains("$Robot")){
                        actorOrder.Add("Robot"); 
                    }
                    else if (line.Contains("#Boy:")){
                        positionLine = true; 
                        float[] pos = GetLinePosition(line); 
                        boyDialog.GetComponent<RectTransform>().localPosition = new Vector3(pos[0], pos[1], pos[2]); 
                    }
                    else if (line.Contains("#Girl:")){
                        positionLine = true; 
                        float[] pos = GetLinePosition(line); 
                        girlDialog.GetComponent<RectTransform>().localPosition = new Vector3(pos[0], pos[1], pos[2]); 
                    }
                    else if (line.Contains("#Robot:")){
                        positionLine = true; 
                        float[] pos = GetLinePosition(line); 
                        botDialog.GetComponent<RectTransform>().localPosition = new Vector3(pos[0], pos[1], pos[2]); 
                    }
                    if (!positionLine){
                        line = line.Remove(0,line.IndexOf(':')+1); 
                        lines.Add(line);
                    }
                    else positionLine = false; 
                }
            }



        #else
            Debug.Log(filepath); 
            using (StreamReader reader = new StreamReader(filepath)){
                
                image = reader.ReadLine(); 
                while(!reader.EndOfStream){
                    string line = reader.ReadLine(); 
                    if (line.Contains("$Boy")){
                        actorOrder.Add("Boy"); 
                        line = line.Remove(0,line.IndexOf(':')); 
                    }
                    else if (line.Contains("$Girl")){
                        actorOrder.Add("Girl"); 
                    }
                    else if (line.Contains("$Robot")){
                        actorOrder.Add("Robot"); 
                    }
                    else if (line.Contains("#Boy:")){
                        positionLine = true; 
                        float[] pos = GetLinePosition(line); 
                        boyDialog.GetComponent<RectTransform>().localPosition = new Vector3(pos[0], pos[1], pos[2]); 
                    }
                    else if (line.Contains("#Girl:")){
                        positionLine = true; 
                        float[] pos = GetLinePosition(line); 
                        girlDialog.GetComponent<RectTransform>().localPosition = new Vector3(pos[0], pos[1], pos[2]); 
                    }
                    else if (line.Contains("#Robot:")){
                        positionLine = true; 
                        float[] pos = GetLinePosition(line); 
                        botDialog.GetComponent<RectTransform>().localPosition = new Vector3(pos[0], pos[1], pos[2]); 
                    }
                    if (!positionLine){
                        line = line.Remove(0,line.IndexOf(':')+1); 
                        lines.Add(line);
                    }
                    else positionLine = false; 
                }
            }
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
        string prefix; 
        if (actorOrder[index] == "Girl") prefix = "Ivy:"; 
        else if (actorOrder[index] == "Boy") prefix = "Guy:"; 
        else prefix = "V.I:";
        dialog.transform.GetChild(0).GetComponent<Text>().text = prefix + lines[index];  
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
}

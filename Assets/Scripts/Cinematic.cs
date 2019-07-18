
using System;
using System.Transactions;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using UnityEngine.Networking;

public class Cinematic : MonoBehaviour
{
    LevelFactory factory;
    // This is the text that is displayed at the start of the level (during the "loading screen") prior to playing the level.
    public string introtext = "Level Start Placeholder!";
    // This text basically says "Press Enter to Continue" and is displayed at the bottom of the "Loading Screen" prior to playing the level.
    public string continuetext = "Continue Text Placeholder";
    // This is the text that is displayed at the end of the level (in the "Victory Screen") after playing the level.
    public string endtext = "Winner!\nLevel End Placeholder!";
    public GameObject prompt1, prompt2;
    public GameObject[] stars = new GameObject[5];
    float originalEnergy; 
    private bool cinerun = false;
    private float delaytime = 0f;
    private float delay = 0.1f;

    int score; 
    bool updatedLevel = false; 
    bool shownCharacter = false; 
    bool hasTimeBonus = true; 
    string webdata;
    int maxScore; 

    //.................................>8.......................................
    // Use this for initialization
    void Start()
    {
        continuetext = stringLib.CONTINUE_TEXT;
        
        
        score = -1; 
        if (GlobalState.timeBonus < 0) GlobalState.timeBonus = 0; 
        if (GlobalState.level != null && !GlobalState.level.IsDemo){
            score = GlobalState.CurrentLevelPoints; 
            originalEnergy = GlobalState.Stats.Points; 
            GlobalState.Stats.Points += score + GlobalState.timeBonus; 
            maxScore = 0; 
        int[] pointArr; 
        if (GlobalState.GameMode == stringLib.GAME_MODE_ON){
            pointArr = new int[]{stateLib.POINTS_BEACON, stateLib.POINTS_QUESTION, stateLib.POINTS_RENAMER ,stateLib.POINTS_COMMENT, stateLib.POINTS_UNCOMMENT}; 
        }
        else {
            pointArr = new int[]{stateLib.POINTS_CATCHER, stateLib.POINTS_CHECKER, stateLib.POINTS_WARPER, stateLib.POINTS_COMMENT, stateLib.POINTS_BREAKPOINT}; 
        }
        for (int i= 0; i < pointArr.Length; i++){
            maxScore += GlobalState.level.Tasks[i] * pointArr[i]; 
        }
        }
        
        UpdateText();
        GameObject.Find("Fade").GetComponent<Fade>().onFadeIn();
        if (!GlobalState.IsDark)
        {
            GameObject.Find("BackgroundCanvas").transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/circuit_board_light");
            transform.Find("PressEnter").GetComponent<Text>().color = Color.black; 
            transform.Find("Title").GetComponent<Text>().color = Color.black; 
        }
        foreach (GameObject star in stars){
            star.GetComponent<Image>().enabled = false; 
            star.GetComponent<Animator>().enabled = false; 
        }

        //Debug.Log(SceneManager.sceneCount);
    }
    IEnumerator ShowCharacter(){
        GameObject player = transform.Find(GlobalState.Character).gameObject; 
        player.GetComponent<Animator>().SetTrigger("isRunning"); 
        player.GetComponent<Animator>().SetBool("running", true); 
        Image image = player.GetComponent<Image>(); 
        while(image.color.a < 1){
            image.color = new Color(image.color.r, image.color.g, image.color.b, image.color.a + 0.05f); 
            yield return null; 
        }
    }
    IEnumerator AnimateStars(){
        
        int value = (int)(((float)score/(float)maxScore)*5f); 
        if (score == 0) value =0; 
        else if (value <= 0) value = 1; 
        foreach (GameObject star in stars){
            star.GetComponent<Image>().enabled = true; 
            star.GetComponent<Animator>().enabled = true; 
            yield return new WaitForSecondsRealtime(0.2f); 
        }
        for (int i = 0; i < value; i++){
            stars[i].GetComponent<Animator>().SetBool("isComplete", true); 
            yield return new WaitForSecondsRealtime(0.1f); 
        }
        if (score == 0){
            float[] speeds = new float[]{0.3f, 0.5f, 1f, 0.5f, 0.3f}; 
            while(stars[0].transform.position.y > -13){
                for (int i = 0; i < stars.Length; i++){
                    stars[i].transform.position = new Vector3(stars[i].transform.position.x, stars[i].transform.position.y - speeds[i], stars[i].transform.position.z); 
                }
                yield return new WaitForSeconds(0.15f); 
            }
        }
    }
    IEnumerator FadeFailStars(){
        while(stars[score].GetComponent<Image>().color.a > 0){
            for (int i = score; i < stars.Length; i++){
                Image image = stars[i].GetComponent<Image>(); 
                image.color = new Color(image.color.r, image.color.b, image.color.g, image.color.a - 0.05f); 
            }
            yield return null; 
        }
    }
    IEnumerator FadeInResults(){
        
        CanvasGroup canvas = transform.Find("Energy").gameObject.GetComponent<CanvasGroup>();
        CanvasGroup energyCanvas = transform.Find("void main").gameObject.GetComponent<CanvasGroup>();
        energyCanvas.GetComponent<Text>().text = stringLib.POINTS_PREFIX + originalEnergy.ToString();
        canvas.GetComponent<Text>().text = "Level Points: " + score.ToString(); 
        while(canvas.alpha < 1){
            canvas.alpha += 0.02f; 
            yield return null; 
        }
        StartCoroutine(ShowBonusEnergy()); 
        yield return new WaitForSecondsRealtime(0.5f); 
        while(energyCanvas.alpha < 1){
            energyCanvas.alpha += 0.02f; 
            yield return null; 
        }
    }
    IEnumerator FadeOutResults(){
        CanvasGroup timer = transform.Find("Time").GetComponent<CanvasGroup>(); 
        CanvasGroup level = transform.Find("Energy").GetComponent<CanvasGroup>(); 
        while(timer.alpha > 0){
            timer.alpha -= 0.05f; 
            level.alpha -= 0.05f; 
            yield return null; 
        }
    }
    IEnumerator ShowBonusEnergy(){
        if (hasTimeBonus){
            StartCoroutine(ShowTimeBonus()); 
            hasTimeBonus = false; 
        }
        Text field = transform.Find("void main").gameObject.GetComponent<Text>(); 
        float dif = GlobalState.Stats.Points - originalEnergy - GlobalState.timeBonus; 
        int frames = 30; 
        float count = originalEnergy; 
        yield return new WaitForSecondsRealtime(0.5f); 
        for (int i = 1; i <= 30; i++)
        {
            count += dif/(float)frames; 
            field.text = stringLib.POINTS_PREFIX +((int)count).ToString(); 
            field.color = new Color(field.color.r, field.color.g + 0.05f, field.color.b); 
            yield return null; 
        }
        field.text = stringLib.POINTS_PREFIX + (GlobalState.Stats.Points - GlobalState.timeBonus).ToString(); 
    }
    IEnumerator PushResults(){
        StartCoroutine(FadeInResults()); 
        if (score < stars.Length) StartCoroutine(FadeFailStars()); 
        float[] speeds = new float[]{1.3f,1.1f, 0.9f, 0.7f, 0.5f}; 
        float[] xPositions = new float[]{-300,-200,-100,0,100};
        float[] xdifs = new float[score]; 
        for (int i = 0; i < xdifs.Length; i++){
            xdifs[i] = xPositions[i] - stars[i].GetComponent<RectTransform>().localPosition.x; 
        }
        float frames = 20f; 
        float scaleDif = -0.5f; 
        int framecount = 0; 
        while(stars[score-1].GetComponent<RectTransform>().localPosition.y > -130f){
            for (int i = 0; i < score; i++){
                RectTransform transform = stars[i].GetComponent<RectTransform>(); 
                if (transform.localScale.x > 0.5f){
                    transform.localScale = new Vector3(transform.localScale.x + scaleDif/frames, transform.localScale.y + scaleDif/frames, transform.localScale.z); 
                }
                if (Math.Abs(transform.localPosition.x - xPositions[i]) > 1){
                    transform.localPosition = new Vector3(transform.localPosition.x + xdifs[i]/frames, transform.localPosition.y, transform.localPosition.z); 
                }
                if (transform.localPosition.y > -130f && framecount > 10){
                    float ydif = -130f - transform.localPosition.y; 
                    transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + (ydif*speeds[i])/frames, transform.localPosition.z); 
                }
            }
            framecount++; 
            yield return null; 
        } 
    }
    IEnumerator ShowTimeBonus(){
        transform.Find("Time").GetComponent<Animator>().SetTrigger("ShowTime"); 
        Text bonus = transform.Find("Time").transform.GetChild(0).GetComponent<Text>(); 
        string starterText = "Time Bonus: "; 
        Text field = transform.Find("void main").gameObject.GetComponent<Text>();
        bonus.text = starterText + GlobalState.StringLib.comment_block_color_tag + "0" + stringLib.CLOSE_COLOR_TAG;
        yield return new WaitForSecondsRealtime(1.3f + 1f/GlobalState.timeBonus); 
        int amount = GlobalState.timeBonus; 
        while(GlobalState.timeBonus > 0){
            GlobalState.timeBonus--; 
            bonus.text = starterText + GlobalState.StringLib.comment_block_color_tag + (amount -GlobalState.timeBonus) + stringLib.CLOSE_COLOR_TAG;
            field.text = stringLib.POINTS_PREFIX +  GlobalState.StringLib.comment_block_color_tag + (GlobalState.Stats.Points - GlobalState.timeBonus) + stringLib.CLOSE_COLOR_TAG; 
            yield return new WaitForSecondsRealtime(0.12f); 
        } 

    }
    IEnumerator LoadGame()
    {
        GameObject.Find("Fade").GetComponent<Fade>().onFadeOut();
        yield return new WaitForSecondsRealtime(1f);
        string filepath; 
        bool web = false; 
        #if UNITY_EDITOR && !UNITY_WEBGL
        string txtFile = GlobalState.level.FileName.Remove(GlobalState.level.FileName.IndexOf('.')) + ".txt";
        filepath = Path.Combine(Application.streamingAssetsPath, txtFile); 
        #else
        string txtFile = GlobalState.level.FileName.Remove(GlobalState.level.FileName.IndexOf('.')) + ".txt";
        filepath = stringLib.SERVER_URL +"StreamingAssets/" + GlobalState.GameMode + "leveldata/" + txtFile;
        WebHelper.i.url = filepath; 
        WebHelper.i.GetWebDataFromWeb(); 
        filepath = WebHelper.i.webData; 
        web = true; 
        #endif       
        if (File.Exists(filepath) || (!filepath.Contains("File not found!") && web)){
            if (filepath.Contains("tutorial")) GlobalState.level.IsDemo = true; 
            Debug.Log("Transition");
            SceneManager.LoadScene("Transition"); 
        }
        else if (GlobalState.CurrentONLevel.Contains("tutorial")){
            GlobalState.level.IsDemo = true;
            Debug.Log("Tutorial");
            SceneManager.LoadScene("newgame"); 
        }
        else{ 
            GlobalState.level.IsDemo = false; 
            Debug.Log("NewGame");
            SceneManager.LoadScene("newgame");
        }
}
    public void ToggleLight()
    {
        prompt1.GetComponent<Text>().color = Color.black;
        prompt2.GetComponent<Text>().color = Color.black;
    }
    public void ToggleDark()
    {
        prompt1.GetComponent<Text>().color = Color.white;
        prompt2.GetComponent<Text>().color = Color.white;
    }

    private void UpdateText()
    {
        if (GlobalState.level == null || GlobalState.level.FileName == GlobalState.CurrentONLevel||GlobalState.GameState == stateLib.GAMESTATE_LEVEL_START)
        {
            UpdateLevel();
        }
        introtext = GlobalState.level.IntroText;
        endtext = GlobalState.level.ExitText;
    }
    private void UpdateLevel()
    {
        //string filepath = Application.streamingAssetsPath +"/"+ GlobalState.GameMode + "leveldata/" + GlobalState.CurrentONLevel;
        //filepath = Path.Combine(filepath,  GlobalState.CurrentONLevel);

        string filepath ="";
        #if (UNITY_EDITOR || UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN) && !UNITY_WEBGL
            filepath = Path.Combine(Application.streamingAssetsPath, GlobalState.GameMode + "leveldata");
            if (GlobalState.Language == "python") filepath = Path.Combine(filepath, "python");
            filepath = Path.Combine(filepath, GlobalState.CurrentONLevel);
        #endif

        //Want to check if the player is WebGL, and if it is, grab the xml as a string and put it in levelfactory

        #if UNITY_WEBGL
            filepath = "StreamingAssets" + "/" + GlobalState.GameMode + "leveldata/";
            if (GlobalState.Language == "python") filepath += "python/";
            filepath+=GlobalState.CurrentONLevel;

            WebHelper.i.url = stringLib.SERVER_URL + filepath;
            WebHelper.i.GetWebDataFromWeb();
            filepath = WebHelper.i.webData;
        #endif
        
        updatedLevel = true; 
        factory = new LevelFactory(filepath);
        GlobalState.level = factory.GetLevel();
    }
    private void UpdateLevel(string file)
    {
        string[] temp = file.Split('\\');
        GlobalState.CurrentONLevel = temp[temp.Length - 1];

        string filepath ="";
        #if (UNITY_EDITOR || UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN) && !UNITY_WEBGL
            filepath = Path.Combine(Application.streamingAssetsPath, GlobalState.GameMode + "leveldata");
            if (GlobalState.Language == "python") filepath = Path.Combine(filepath, "python");
            filepath = Path.Combine(filepath, file);
            Debug.Log("Cinematics: UpdateLevel(string file) WINDOWS");
        #endif
        

        #if UNITY_WEBGL
            filepath = "StreamingAssets" + "/" + GlobalState.GameMode + "leveldata/";
            if (GlobalState.Language == "python") filepath += "python/";
            filepath+=GlobalState.CurrentONLevel;

            WebHelper.i.url = stringLib.SERVER_URL + filepath;
            WebHelper.i.GetWebDataFromWeb();
            filepath = WebHelper.i.webData;
        #endif

        updatedLevel = true; 
        factory = new LevelFactory(filepath);
        GlobalState.level = factory.GetLevel();
    }
    //.................................>8.......................................
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GlobalState.GameState = stateLib.GAMESTATE_MENU;
            GlobalState.IsResume = false; 
            if (!updatedLevel)UpdateLevel(GlobalState.level.NextLevel); 
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
        }
        if (GlobalState.GameState == stateLib.GAMESTATE_LEVEL_START)
        {
            if (!cinerun)
            {
                cinerun = true;
 
            }

            if(!shownCharacter){
                shownCharacter = true; 
                StartCoroutine(ShowCharacter()); 
            }
            
            prompt1.GetComponent<Text>().text = introtext;
            if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetMouseButton(0)) && delaytime < Time.time)
            {
                if (GlobalState.level == null){
                    UpdateLevel(GlobalState.CurrentONLevel); 
                }
                GlobalState.GameState = stateLib.GAMESTATE_IN_GAME;
                cinerun = false;
                StartCoroutine(LoadGame());
            }
        }
        else if (GlobalState.GameState == stateLib.GAMESTATE_LEVEL_WIN)
        {
            if (!cinerun)
            {
                cinerun = true;
            }
            if (score > 0)
                StartCoroutine(FadeInResults()); 
            prompt1.GetComponent<Text>().text = endtext;

            if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetMouseButtonDown(0)) && delaytime < Time.time)
            {
                GlobalState.GameState = stateLib.GAMESTATE_LEVEL_START;
                UpdateLevel(GlobalState.level.NextLevel);
                UpdateText();
                if (score > 0){
                    StartCoroutine(FadeOutResults()); 
                    StartCoroutine(AnimateStars()); 
                }
                //GameObject.Find("Main Camera").GetComponent<GameController>().SetLevel(GlobalState.level.NextLevel);
                cinerun = false;

            }
        }
        else if (GlobalState.GameState == stateLib.GAMESTATE_LEVEL_LOSE)
        {
            score = 0; 
            StartCoroutine(AnimateStars()); 
            if (!cinerun)
            {
                cinerun = true;
            }
            prompt1.GetComponent<Text>().text = stringLib.LOSE_TEXT;
            prompt2.GetComponent<Text>().text = stringLib.RETRY_TEXT;
            if (Input.GetKeyDown(KeyCode.Escape) && delaytime < Time.time)
            {
                prompt2.GetComponent<Text>().text = stringLib.CONTINUE_TEXT;

                cinerun = false;
                GlobalState.GameState = stateLib.GAMESTATE_MENU;
            }
            if ((Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetMouseButtonDown(0)) && delaytime < Time.time)
            {
                prompt2.GetComponent<Text>().text = stringLib.CONTINUE_TEXT;

                cinerun = false;
                // One is called Bugleveldata and another OnLevel data.
                // Levels.txt, coding in menu.cs

                string filepath = GlobalState.level.Failure_Level;
                Debug.Log("FailureLevel: " + filepath);
                if (filepath == "Null")
                    UpdateLevel(); 
                else
                    UpdateLevel(filepath);
                GlobalState.GameState = stateLib.GAMESTATE_LEVEL_START;
                //Debug.Log("LoadingScreen");
            }
        }
        else
        {
            delaytime = Time.time + delay;
        }
    }

    //.................................>8.......................................
}

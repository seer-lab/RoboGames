
using System;
using System.Transactions;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public partial class Cinematic : MonoBehaviour{
     /// <summary>
    /// Fade Character in and commence the running animation
    /// </summary>
    /// <returns></returns>
    IEnumerator ShowCharacter()
    {
        GameObject player = transform.Find(GlobalState.Character).gameObject;
        player.GetComponent<Animator>().SetTrigger("isRunning");
        player.GetComponent<Animator>().SetBool("running", true);
        Image image = player.GetComponent<Image>();

        //fade alpha
        while (image.color.a < 1)
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, image.color.a + 0.05f);
            yield return null;
        }
    }
    /// <summary>
    /// Animate the 5 isstars on screen, along with their place holders.
    /// Will handle when the player fails the level as well (score = 0)
    /// </summary>
    /// <returns></returns>
    IEnumerator AnimateStars()
    {

        int value = 3; 
        if(originalTimeBonus > 0) value++; 
        if (GlobalState.CurrentLevelEnergy == GlobalState.Stats.Energy) value++;
        if (score == 0) value = 0; 
        //show place holder
        foreach (GameObject star in stars)
        {
            star.GetComponent<Image>().enabled = true;
            star.GetComponent<Animator>().enabled = true;
            yield return new WaitForSecondsRealtime(0.2f);
        }
        //display the stars if the player recieves any
         
        for (int i = 0; i < value; i++)
        {
            stars[i].GetComponent<Animator>().SetBool("isComplete", true);
            yield return new WaitForSecondsRealtime(0.1f);
        }
        //if the player failed animate the stars falling using the speeds array. 
        if (score == 0)
        {
            float[] speeds = new float[] { 0.3f, 0.5f, 1f, 0.5f, 0.3f };
            while (stars[0].transform.position.y > -13)
            {
                for (int i = 0; i < stars.Length; i++)
                {
                    stars[i].transform.position = new Vector3(stars[i].transform.position.x, stars[i].transform.position.y - speeds[i], stars[i].transform.position.z);
                }
                yield return new WaitForSeconds(0.15f);
            }
        }
    }

    /// <summary>
    /// Fade out the stars. The Stars should already be displayed.
    /// </summary>
    /// <returns></returns>
    IEnumerator FadeFailStars()
    {
        while (stars[score].GetComponent<Image>().color.a > 0)
        {
            for (int i = score; i < stars.Length; i++)
            {
                Image image = stars[i].GetComponent<Image>();
                image.color = new Color(image.color.r, image.color.b, image.color.g, image.color.a - 0.05f);
            }
            yield return null;
        }
    }
    /// <summary>
    /// Introduces the various points the player recieved upon completion 
    /// of the level.
    /// 
    /// Current Order: Level Points => BonusPoints => Total Points
    /// </summary>
    /// <returns></returns>
    IEnumerator FadeInResults()
    {

        CanvasGroup canvas = transform.Find("Energy").gameObject.GetComponent<CanvasGroup>();
        CanvasGroup energyCanvas = transform.Find("void main").gameObject.GetComponent<CanvasGroup>();
        energyCanvas.GetComponent<Text>().text = stringLib.POINTS_PREFIX + originalEnergy.ToString();
        canvas.GetComponent<Text>().text = "Completion Score: " + score.ToString();

        //Fades In Level Points
        while (canvas.alpha < 1)
        {
            canvas.alpha += 0.02f;
            yield return null;
        }

        //Fades in Bonus Points
        StartCoroutine(ShowBonusEnergy());
        yield return new WaitForSecondsRealtime(0.5f);

        //Fades in Total Points  
        while (energyCanvas.alpha < 1)
        {
            energyCanvas.alpha += 0.02f;
            yield return null;
        }
    }
    /// <summary>
    /// Fades out the additional points. The total points are left
    /// for the following step in the scene.
    /// </summary>
    /// <returns></returns>
    IEnumerator FadeOutResults()
    {
        CanvasGroup timer = transform.Find("Time").GetComponent<CanvasGroup>();
        CanvasGroup level = transform.Find("Energy").GetComponent<CanvasGroup>();
        while (timer.alpha > 0)
        {
            timer.alpha -= 0.05f;
            level.alpha -= 0.05f;
            yield return null;
        }
    }
    /// <summary>
    /// Increase the total amount of points incrementally. 
    /// Also start any additional points that player could be getting.
    /// </summary>
    /// <returns></returns>
    IEnumerator ShowBonusEnergy()
    {
        //if there is a time bonus animate in now. 
        if (hasTimeBonus)
        {
            StartCoroutine(ShowTimeBonus());
            hasTimeBonus = false;
        }
        Text field = transform.Find("void main").gameObject.GetComponent<Text>();
        float dif = totalEnergy - originalEnergy - GlobalState.timeBonus;
        int frames = 30;
        float count = originalEnergy;
        yield return new WaitForSecondsRealtime(0.5f);
        for (int i = 1; i <= 30; i++)
        {
            count += dif / (float)frames;
            field.text = stringLib.POINTS_PREFIX + ((int)count).ToString();
            field.color = new Color(field.color.r, field.color.g + 0.05f, field.color.b);
            yield return null;
        }
        while (showingTime) yield return null;
        field.text = stringLib.POINTS_PREFIX + GlobalState.StringLib.node_color_print_light + totalEnergy.ToString() + stringLib.CLOSE_COLOR_TAG;
        StartCoroutine(ShowTotal());
    }
    /// <summary>
    /// Displays the total points.This will pause in between where the points were gathered.
    /// </summary>
    /// <returns></returns>
    IEnumerator ShowTotal()
    {
        GameObject totalPoints = transform.Find("final points").gameObject;
        Text pointText = totalPoints.GetComponent<Text>();
        string prefix = "<color=#00ffffff>Total Points:</color> ";
        pointText.text = prefix + "0";

        CanvasGroup canvas = totalPoints.GetComponent<CanvasGroup>();

        float speed = 40f;
        int increment = (int)((GlobalPoints - totalEnergy) / speed);
        int total = 0;
        while (canvas.alpha < 1 || total < (GlobalPoints - totalEnergy))
        {
            canvas.alpha += (1.0f / speed);
            total += increment;
            pointText.text = prefix + (total).ToString();
            yield return null;
        }
        pointText.text = prefix + (GlobalPoints - totalEnergy).ToString();

        yield return new WaitForSecondsRealtime(0.7f);
        speed = 40f;
        increment = (int)((totalEnergy) / speed);
        int temp = (int)totalEnergy;
        while (temp > 0)
        {
            temp -= increment;
            pointText.text = prefix + GlobalState.StringLib.node_color_print_light + (GlobalPoints - temp).ToString() + stringLib.CLOSE_COLOR_TAG;
            yield return null;
        }
        pointText.text = prefix + GlobalState.StringLib.node_color_print_light + (GlobalPoints).ToString() + stringLib.CLOSE_COLOR_TAG;

        StartCoroutine(ShowEnergyBonus());


    }
    /// <summary>
    /// If the player is capable of recieving an energy bonus 
    /// highlight the level points and show a multiplyer indicating the additional
    /// points. Then increase the total points. Any additional points from XPBoosts are added.
    /// </summary>
    /// <returns></returns>
    IEnumerator ShowEnergyBonus()
    {
        Text levelText = transform.Find("void main").GetComponent<Text>();
        Text totalText = transform.Find("final points").GetComponent<Text>();
        yield return new WaitForSecondsRealtime(1f);
        GameObject energy = transform.Find("EnergyDisplay").gameObject;
        energy.GetComponent<Animator>().SetTrigger("Show");
        float perecent = 1 + ((float)GlobalState.CurrentLevelEnergy / (float)GlobalState.Stats.Energy);
        perecent *= GlobalState.Stats.XPBoost;
        //Debug.Log(perecent);
        if (perecent > 1)
        {
            energy.GetComponent<Animator>().SetTrigger("Show");
            energy.transform.GetChild(0).GetComponent<Text>().text = ('X' + perecent.ToString());
            int difference = (int)(totalEnergy * perecent - totalEnergy);
            GlobalPoints += (int)difference;
            string prefix = "<color=#00ffffff>Total Points:</color> ";
            float speed = 50f;
            int increment = (int)(difference / speed);
            while (difference > 0)
            {
                difference -= increment;
                totalText.text = prefix + stringLib.BLUE_COLOR_TAG + (GlobalPoints - difference).ToString() + stringLib.CLOSE_COLOR_TAG;
                yield return null;
            }
            totalText.text = prefix + stringLib.BLUE_COLOR_TAG + (GlobalPoints).ToString() + stringLib.CLOSE_COLOR_TAG;
            yield return null;
        }
    }

    /// <summary>
    /// Moves stars from the center to the bottom line. This is no longer used
    /// however the function is still available if the stars need to be pushed out 
    /// of the way in the future.!-- 
    /// /// </summary>
    /// <returns></returns>
    IEnumerator PushResults()
    {
        StartCoroutine(FadeInResults());
        if (score < stars.Length) StartCoroutine(FadeFailStars());
        float[] speeds = new float[] { 1.3f, 1.1f, 0.9f, 0.7f, 0.5f };
        float[] xPositions = new float[] { -300, -200, -100, 0, 100 };
        float[] xdifs = new float[score];
        for (int i = 0; i < xdifs.Length; i++)
        {
            xdifs[i] = xPositions[i] - stars[i].GetComponent<RectTransform>().localPosition.x;
        }
        float frames = 20f;
        float scaleDif = -0.5f;
        int framecount = 0;
        while (stars[score - 1].GetComponent<RectTransform>().localPosition.y > -130f)
        {
            for (int i = 0; i < score; i++)
            {
                RectTransform transform = stars[i].GetComponent<RectTransform>();
                if (transform.localScale.x > 0.5f)
                {
                    transform.localScale = new Vector3(transform.localScale.x + scaleDif / frames, transform.localScale.y + scaleDif / frames, transform.localScale.z);
                }
                if (Math.Abs(transform.localPosition.x - xPositions[i]) > 1)
                {
                    transform.localPosition = new Vector3(transform.localPosition.x + xdifs[i] / frames, transform.localPosition.y, transform.localPosition.z);
                }
                if (transform.localPosition.y > -130f && framecount > 10)
                {
                    float ydif = -130f - transform.localPosition.y;
                    transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + (ydif * speeds[i]) / frames, transform.localPosition.z);
                }
            }
            framecount++;
            yield return null;
        }
    }
    /// <summary>
    /// Animates the clock and text into view and will increment the 
    /// time if they have a good time. 
    /// </summary>
    /// <returns></returns>
    IEnumerator ShowTimeBonus()
    {
        showingTime = true;
        //initialization
        transform.Find("Time").GetComponent<Animator>().SetTrigger("ShowTime");
        Text bonus = transform.Find("Time").transform.GetChild(0).GetComponent<Text>();
        string starterText = "Time Bonus: ";
        Text field = transform.Find("void main").gameObject.GetComponent<Text>();
        bonus.text = starterText + GlobalState.StringLib.comment_block_color_tag + "0" + stringLib.CLOSE_COLOR_TAG;
        //delay longer if the time bonus is lower. This keeps the time spent viewing
        //the time bonus consistent. 
        yield return new WaitForSecondsRealtime(1.3f + 1f / GlobalState.timeBonus);
        int amount = GlobalState.timeBonus;
        int subtraction;
        if(GlobalState.timeBonus < 100 ){
            subtraction = GlobalState.timeBonus/ 20;
        }else{
           subtraction = GlobalState.timeBonus / 100;
        }

        //Debug.Log("TimeBonus: " + GlobalState.timeBonus);
        //Debug.Log("Subtraction: " + subtraction);

        //increment the amount on both the total and the time section. 
        while (GlobalState.timeBonus > 0)
        {
            GlobalState.timeBonus -= subtraction;
            bonus.text = starterText + GlobalState.StringLib.comment_block_color_tag + (amount - GlobalState.timeBonus) + stringLib.CLOSE_COLOR_TAG;
            field.text = stringLib.POINTS_PREFIX + GlobalState.StringLib.comment_block_color_tag + (totalEnergy - GlobalState.timeBonus) + stringLib.CLOSE_COLOR_TAG;
            yield return null;
        }
        bonus.text = starterText + GlobalState.StringLib.comment_block_color_tag + (amount) + stringLib.CLOSE_COLOR_TAG;
        field.text = stringLib.POINTS_PREFIX + GlobalState.StringLib.comment_block_color_tag + (totalEnergy) + stringLib.CLOSE_COLOR_TAG;
        showingTime = false;
    }

    /// <summary>
    /// Load the next scene.
    /// IEnumerator is required for web and to fade the scene out. 
    /// </summary>
    /// <returns></returns>
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

        //Determine properties of the next level. 
        //All tutorial/demo levels will have "Tutorial" in the name. 
        //If a txt file with the same name of the following level exists the 
        //game will load the transition scene which will continue the story.     
        if (File.Exists(filepath) || (!filepath.Contains("File not found!") && web))
        {
            Debug.Log("FileP: " + filepath);
            if (filepath.Contains("tutorial")) GlobalState.level.IsDemo = true;
            SceneManager.LoadScene("Transition");
        }
        else if (GlobalState.CurrentONLevel.Contains("tutorial"))
        {
            GlobalState.level.IsDemo = true;
            SceneManager.LoadScene("newgame");
        }
        else
        {
            GlobalState.level.IsDemo = false;
            // Debug.Log(GlobalState.previousFilename);
            // if(GlobalState.LeaderBoardMode && !GlobalState.previousFilename.Contains("tutorial") && GlobalState.GameState != stateLib.GAMESTATE_LEVEL_LOSE){
            //     SceneManager.LoadScene("Leaderboard");
            // }else{
                SceneManager.LoadScene("newgame");
            //}
        }
    }
}
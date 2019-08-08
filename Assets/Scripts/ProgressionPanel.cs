using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Handles logic in the Progression scene.
/// </summary>
public class ProgressionPanel : MonoBehaviour
{
    Logger logger;
    List<GameObject> buttons;
    GameObject done, reset; 
    int[] costs;
    string[] starterText;
    float[] originalValues;
    ProgressionUI ui;
    GameObject selectedObject; 
    int points;
    Color selectColor, deselectColor, invertSelect, invertDeselect; 
    void Start()
    {
         if (GlobalState.IsDark){
            selectColor = new Color(1,1,1,1); 
            deselectColor = new Color(1,1,1,0.6f); 
            invertSelect = new Color(0,0,0,1); 
            invertDeselect = new Color(0,0,0,0.6f); 
        }
        else{
            selectColor = new Color(0,0,0,1); 
            deselectColor = new Color(0,0,0,0.6f); 
            invertSelect = new Color(1,1,1,1); 
            invertDeselect = new Color(1,1,1,0.6f); 
        }
        logger = new Logger(true);
        points = GlobalState.Stats.Points;
        ui = GetComponent<ProgressionUI>();
        if (GlobalState.Stats == null) GlobalState.Stats = new CharacterStats(true);
        buttons = new List<GameObject>();
        starterText = new string[4];
        costs = new int[] { stateLib.COST_SPEED, stateLib.COST_DAMAGE_REDUCE, stateLib.COST_HEALTH, stateLib.COST_XPBOOST };
        originalValues = new float[] { GlobalState.Stats.Speed, GlobalState.Stats.DamageLevel, GlobalState.Stats.Energy, GlobalState.Stats.XPBoost };
        for (int i = 0; i < 4; i++)
        {
            buttons.Add(transform.GetChild(i).gameObject);
            starterText[i] = buttons[i].transform.GetChild(0).GetComponent<Text>().text;
            if (i < costs.Length && points < costs[i])
            {
                buttons[i].GetComponent<Button>().interactable = false;
            }
            else buttons[i].GetComponent<Button>().interactable = true;
        }
        done = transform.Find("Done").gameObject; 
        reset = transform.Find("Reset").gameObject;
        ui.AnimateButtons(buttons);

        if (!GlobalState.IsDark){
            reset.GetComponent<Image>().color = Color.black; 
            reset.transform.GetChild(0).GetComponent<Text>().color = Color.white; 

            done.GetComponent<Image>().color = Color.black; 
            done.transform.GetChild(0).GetComponent<Text>().color = Color.white; 
        }
        savePrefs();
        UpdateValues();
       
    }
    /// <summary>
    /// Invokes the button of the selected object
    /// </summary>
    void SelectSelectedObject(){
        if (selectedObject != null && selectedObject.GetComponent<Button>().interactable)
            selectedObject.GetComponent<Button>().onClick.Invoke(); 
    }
    /// <summary>
    /// Color the object correctly when it is no longer selected
    /// </summary>
    void DeSelect(){
        selectedObject.GetComponent<Image>().color = deselectColor; 
        if ((selectedObject.name == "Reset" || selectedObject.name == "Done"))
            selectedObject.transform.GetChild(0).GetComponent<Text>().color = invertDeselect; 
        else selectedObject.transform.GetChild(0).GetComponent<Text>().color = deselectColor; 
    }
    /// <summary>
    /// color the object correctly when it is selected and point selected
    /// object to it. 
    /// </summary>
    void Select(){
        selectedObject.GetComponent<Image>().color = selectColor; 
        if ((selectedObject.name == "Reset" || selectedObject.name == "Done"))
            selectedObject.transform.GetChild(0).GetComponent<Text>().color = invertSelect; 
        else selectedObject.transform.GetChild(0).GetComponent<Text>().color = selectColor; 
    }
    /// <summary>
    /// decolour all objects and prepare for keyboard selection. 
    /// </summary>
    void FirstSelect(){
        foreach(GameObject button in buttons){
            button.GetComponent<Image>().color = deselectColor; 
            button.transform.GetChild(0).GetComponent<Text>().color = deselectColor; 
        }
        done.GetComponent<Image>().color = deselectColor; 
        done.transform.GetChild(0).GetComponent<Text>().color = invertDeselect; 
        reset.GetComponent<Image>().color = deselectColor; 
        reset.transform.GetChild(0).GetComponent<Text>().color = invertDeselect; 
        Select(); 
    }
    void OnLeftArrow(){
        //check if the keyboard has been in use previously
        if(selectedObject == null){
            selectedObject = reset; 
            FirstSelect(); 
            return; 
        } else if(selectedObject.name == "Reset") return; 
        DeSelect();
        if (selectedObject.name == "Done"){
            bool found = false; 
            //if landing on stats, pick only  a stat that is available, otherwise skip it.
            for (int i = buttons.Count-1; i >= 0; i--){
                if (buttons[i].GetComponent<Button>().interactable){
                    selectedObject = buttons[i]; 
                    found = false; 
                    break; 
                }
                if (!found){
                    selectedObject = reset; 
                }
            }
        }
        else selectedObject = reset;  
        Select(); 
    }
    void OnRightArrow(){
        if(selectedObject == null){
            selectedObject = done; 
            FirstSelect(); 
            return; 
        }
        else if (selectedObject.name == "Done") return; 
        DeSelect(); 
        if (selectedObject.name == "Reset"){
            bool found = false; 
            for (int i = 0; i < buttons.Count; i++){
                if (buttons[i].GetComponent<Button>().interactable){
                    selectedObject = buttons[i]; 
                    found = true; 
                    break; 
                }
            }
            if (!found){
                selectedObject = done; 
            }
        } 
        else selectedObject = done; 
        Select(); 
    }
    void OnUpArrow(){
        if(selectedObject == null){
            selectedObject = buttons[0]; 
            FirstSelect(); 
        } 
        if (selectedObject.name == "Reset" || selectedObject.name == "Done") return; 
        DeSelect(); 
        for (int i = buttons.IndexOf(selectedObject)-1; i >= 0; i--){
            if (buttons[i].GetComponent<Button>().interactable){
                selectedObject = buttons[i]; 
                break; 
            }
        }
        Select(); 
    }
    void OnDownArrow(){
        if(selectedObject == null){
            selectedObject = buttons.Last(); 
            FirstSelect(); 
        } 
        if (selectedObject.name == "Reset" || selectedObject.name == "Done") return; 
        DeSelect(); 
         for (int i = buttons.IndexOf(selectedObject)+1; i <buttons.Count; i++){
            if (buttons[i].GetComponent<Button>().interactable){
                selectedObject = buttons[i]; 
                break; 
            }
        }
        Select(); 
    }
    /// <summary>
    /// Checks and disables buttons that can not be activated because
    /// the user doesn't have enough points. 
    /// </summary>
    /// <param name="costValue">the cost of each upgrade</param>
    void CheckInteractable(int[] costValue)
    {
        for (int i = 0; i < 4; i++)
        {
            if (points < costValue[i])
            {
                buttons[i].GetComponent<Button>().interactable = false;
            }
            else buttons[i].GetComponent<Button>().interactable = true;
        }
    }
    /// <summary>
    /// Reset the values to their original before entering this scene.
    /// </summary>
    public void OnReset()
    {
        GlobalState.Stats.Speed = originalValues[0];
        GlobalState.Stats.DamageLevel = originalValues[1];
        GlobalState.Stats.Energy = (int)originalValues[2];
        GlobalState.Stats.XPBoost = (int)originalValues[3];
        logger.sendUpgrades("RESET", points, GlobalState.Stats.Points);

        points = GlobalState.Stats.Points;

        UpdateValues();
        savePrefs();

    }
    public void EndScene()
    {
        GameObject.Find("Fade").GetComponent<Fade>().onFadeOut();
        StartCoroutine(WaitForSwitchScene());
    }
    IEnumerator WaitForSwitchScene()
    {
        yield return new WaitForSeconds(1f);
        GlobalState.Stats.Points = points;
        if (GlobalState.level.FileName.Contains("tutorial")) GlobalState.level.IsDemo = true;
        SceneManager.LoadScene("newgame");
    }
    /// <summary>
    /// Upgrades the Speed to the next tier unless maxed out.
    /// </summary>
    public void OnUpgradeSpeed()
    {
        int prePoints = points;
        int index = StatLib.speeds.ToList().IndexOf(GlobalState.Stats.Speed) + 1;
        points -= stateLib.COST_SPEED*index;
        logger.sendUpgrades("SPEED", prePoints, points);
        if (index < StatLib.speeds.Length)
            GlobalState.Stats.Speed = StatLib.speeds[index];
        UpdateValues();
        savePrefs();
    }
    /// <summary>
    /// Upgrades the Damage taken to the next tier unless maxed out.
    /// </summary>
    public void OnUpgradeDamageReduce()
    {
        int prePoints = points;
        if (GlobalState.GameMode == stringLib.GAME_MODE_ON)
        {
            int index = StatLib.on_damageLevels.ToList().IndexOf(GlobalState.Stats.DamageLevel) + 1;
            points -= stateLib.COST_DAMAGE_REDUCE*index;
            if (index < StatLib.on_damageLevels.Length)
                GlobalState.Stats.DamageLevel = StatLib.on_damageLevels[index];
        }
        else
        {
            int index = StatLib.bug_damageLevels.ToList().IndexOf(GlobalState.Stats.DamageLevel) + 1;
            points -= stateLib.COST_DAMAGE_REDUCE*index;
            if (index < StatLib.bug_damageLevels.Length)
                GlobalState.Stats.DamageLevel = StatLib.bug_damageLevels[index];
        }
        logger.sendUpgrades("DAMAGE_REDUCE", prePoints, points);
        UpdateValues();
        savePrefs();
    }
    /// <summary>
    /// Upgrades the Energy to the next tier unless maxed out.
    /// </summary>
    public void OnUpgradeEnergy()
    {
        int prePoints = points;
        int index = StatLib.energyLevels.ToList().IndexOf(GlobalState.Stats.Energy) + 1;
        points -= stateLib.COST_HEALTH*index;
        if (index < StatLib.energyLevels.Length)
            GlobalState.Stats.Energy = StatLib.energyLevels[index];
        logger.sendUpgrades("ENERGY_UPGRADE", prePoints, points);
        UpdateValues();
        savePrefs();
    }
    public void OnUpgradeFreefall()
    {
       int prePoints = points;
        int index = StatLib.xpboost.ToList().IndexOf(GlobalState.Stats.XPBoost) + 1;
        points -= stateLib.COST_XPBOOST*index;
        if (index < StatLib.xpboost.Length)
            GlobalState.Stats.XPBoost = StatLib.xpboost[index];
        logger.sendUpgrades("XP_BOOST", prePoints, points);
        UpdateValues();
        savePrefs();
    }
    void Update(){
        if (Input.GetKeyDown(KeyCode.LeftArrow)){
            OnLeftArrow(); 
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow)){
            OnRightArrow(); 
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow)){
            OnUpArrow(); 
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow)){
            OnDownArrow();
        }
        else if (Input.GetKeyDown(KeyCode.Return)){
            SelectSelectedObject(); 
        }
        if (Input.GetMouseButtonDown(0)){
            foreach (GameObject button in buttons){
                button.GetComponent<Image>().color = selectColor; 
            }
            done.GetComponent<Image>().color = selectColor; 
            done.transform.GetChild(0).GetComponent<Text>().color = invertSelect; 

            reset.GetComponent<Image>().color = selectColor; 
            reset.transform.GetChild(0).GetComponent<Text>().color = invertSelect; 
        }
    }
    void UpdateValues()
    {
        int counter = 0;
        string[] values = new string[4]{GlobalState.Stats.Speed.ToString(), GlobalState.Stats.DamageLevel.ToString(),
                            GlobalState.Stats.Energy.ToString(), GlobalState.Stats.XPBoost.ToString()};
        costs = new int[4] { stateLib.COST_SPEED, stateLib.COST_DAMAGE_REDUCE, stateLib.COST_HEALTH, stateLib.COST_XPBOOST };
        string[] updatedValues = new string[values.Length];
        string maxed = "Maxed Out!";
        //Find the next tier 
        int index = StatLib.speeds.ToList().IndexOf(GlobalState.Stats.Speed) + 1;
        if (index < 5){ updatedValues[0] = StatLib.speeds[index].ToString(); costs[0]*= (int)Math.Pow(2,index-1);} 
        else{ updatedValues[0] = maxed; costs[0] = 9999999;}

        if (GlobalState.GameMode == stringLib.GAME_MODE_ON){
            index = StatLib.on_damageLevels.ToList().IndexOf(GlobalState.Stats.DamageLevel) + 1;
            if (index < 5) {updatedValues[1] = StatLib.on_damageLevels[index].ToString();costs[1]*= (int)Math.Pow(2,index-1);} 
            else {updatedValues[1] = maxed; costs[1] = 9999999;}
        }
        else{
            index = StatLib.bug_damageLevels.ToList().IndexOf(GlobalState.Stats.DamageLevel) + 1;
            if (index < 5) {updatedValues[1] = StatLib.bug_damageLevels[index].ToString();costs[1]*= (int)Math.Pow(2,index-1);} 
            else {updatedValues[1] = maxed; costs[2] = 9999999;}
        }

        index = StatLib.energyLevels.ToList().IndexOf(GlobalState.Stats.Energy) + 1;
        if (index < 5) {updatedValues[2] = StatLib.energyLevels[index].ToString();costs[2]*= (int)Math.Pow(2,index-1);} 
        else {updatedValues[2] = maxed; costs[3] = 9999999;}

        index = StatLib.xpboost.ToList().IndexOf(GlobalState.Stats.XPBoost) + 1;
        if (index < 5) {updatedValues[3] = StatLib.xpboost[index].ToString();costs[3]*= (int)Math.Pow(4,index-1);} 
        else{ updatedValues[3] = maxed; costs[4] = 9999999;}


        //update the text, and indicate the next tier they can get.
        foreach (GameObject button in buttons)
        {
            Text text = button.transform.GetChild(0).GetComponent<Text>();
            text.text = starterText[counter] + values[counter] + " >> " + updatedValues[counter] + ((updatedValues[counter] == maxed) ? "" : " COST: " + costs[counter]);
            counter++;
        }
        CheckInteractable(costs);

        ui.UpdateText(points.ToString());
    }


    public void savePrefs(){

        if(GlobalState.Stats == null){
            GlobalState.Stats = new CharacterStats();
        }

        string url = stringLib.DB_URL + GlobalState.GameMode.ToUpper() + "/points/" + GlobalState.sessionID; 

        SendPointsToDB(url + "/totalPoints", 
                        "{ \"totalPoints\":\"" + GlobalState.totalPoints.ToString() + "\"}");

        SendPointsToDB(url + "/currentPoints", 
                        "{ \"currentPoints\":\"" + GlobalState.Stats.Points.ToString() + "\"}");

        SendPointsToDB(url + "/resistanceUpgrade", 
                        "{ \"resistanceUpgrade\":\"" + GlobalState.Stats.DamageLevel.ToString() + "\"}");

        SendPointsToDB(url + "/energyUpgrades", 
                        "{ \"energyUpgrades\":\"" + GlobalState.Stats.Energy.ToString() + "\"}");

        SendPointsToDB(url + "/xpUpgrades", 
                        "{ \"xpUpgrades\":\"" + GlobalState.Stats.XPBoost.ToString() + "\"}");

        SendPointsToDB(url + "/speedUpgrades", 
                        "{ \"speedUpgrades\":\"" + GlobalState.Stats.Speed.ToString() + "\"}");
    }

    public void SendPointsToDB(string url, string json){
        DatabaseHelperV2.i.url = url;
        DatabaseHelperV2.i.jsonData = json;
        DatabaseHelperV2.i.PutToDataBase();
    }
}

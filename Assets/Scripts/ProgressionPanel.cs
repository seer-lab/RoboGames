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
    void Start()
    {
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
        UpdateValues();

    }
    void SelectSelectedObject(){
        if (selectedObject != null)
            selectedObject.GetComponent<Button>().onClick.Invoke(); 
    }
    void DeSelect(){
        selectedObject.GetComponent<Image>().color = new Color(1,1,1,0.6f); 
        if ((selectedObject.name == "Reset" || selectedObject.name == "Done"))
            selectedObject.transform.GetChild(0).GetComponent<Text>().color = new Color(0,0,0,0.6f); 
        else selectedObject.transform.GetChild(0).GetComponent<Text>().color = new Color(1,1,1,0.6f); 
    }
    void Select(){
        selectedObject.GetComponent<Image>().color = new Color(1,1,1,1); 
        if ((selectedObject.name == "Reset" || selectedObject.name == "Done"))
            selectedObject.transform.GetChild(0).GetComponent<Text>().color = new Color(0,0,0,1f); 
        else selectedObject.transform.GetChild(0).GetComponent<Text>().color = new Color(1,1,1,1); 
    }
    void FirstSelect(){
        foreach(GameObject button in buttons){
            button.GetComponent<Image>().color = new Color(1,1,1,0.6f); 
            button.transform.GetChild(0).GetComponent<Text>().color = new Color(1,1,1,0.6f); 
        }
        done.GetComponent<Image>().color = new Color(1,1,1,0.6f); 
        done.transform.GetChild(0).GetComponent<Text>().color = new Color(0,0,0,0.6f); 
        reset.GetComponent<Image>().color = new Color(1,1,1,0.6f); 
        reset.transform.GetChild(0).GetComponent<Text>().color = new Color(0,0,0,0.6f); 
        Select(); 
    }
    void OnLeftArrow(){
        if(selectedObject == null){
            selectedObject = reset; 
            FirstSelect(); 
            return; 
        }
        else DeSelect(); 

        if (selectedObject.name == "Reset") return; 
        else if (selectedObject.name == "Done"){
            bool found = false; 
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
        else DeSelect(); 

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
        else if (selectedObject.name == "Done") return; 
        else selectedObject = done; 
        Select(); 
    }
    void OnUpArrow(){
        if(selectedObject == null){
            selectedObject = buttons[0]; 
            FirstSelect(); 
        }
        else DeSelect(); 
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
        else DeSelect(); 
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
    void CheckInteractable()
    {
        for (int i = 0; i < 4; i++)
        {
            if (i < costs.Length && points < costs[i])
            {
                buttons[i].GetComponent<Button>().interactable = false;
            }
            else buttons[i].GetComponent<Button>().interactable = true;
        }
    }
    public void OnReset()
    {
        GlobalState.Stats.Speed = originalValues[0];
        GlobalState.Stats.DamageLevel = originalValues[1];
        GlobalState.Stats.Energy = (int)originalValues[2];
        GlobalState.Stats.XPBoost = (int)originalValues[3];
        logger.sendUpgrades("RESET", points, GlobalState.Stats.Points);

        points = GlobalState.Stats.Points;

        UpdateValues();

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
    }
    void UpdateValues()
    {
        int counter = 0;
        string[] values = new string[4]{GlobalState.Stats.Speed.ToString(), GlobalState.Stats.DamageLevel.ToString(),
                            GlobalState.Stats.Energy.ToString(), GlobalState.Stats.XPBoost.ToString()};
        int[] costs = new int[4] { stateLib.COST_SPEED, stateLib.COST_DAMAGE_REDUCE, stateLib.COST_HEALTH, stateLib.COST_XPBOOST };
        string[] updatedValues = new string[values.Length];
        string maxed = "Maxed Out!";
        //Find the next tier 
        int index = StatLib.speeds.ToList().IndexOf(GlobalState.Stats.Speed) + 1;
        if (index < 5){ updatedValues[0] = StatLib.speeds[index].ToString(); costs[0]*= (int)Math.Pow(2,index-1);} 
        else updatedValues[0] = maxed;

        if (GlobalState.GameMode == stringLib.GAME_MODE_ON){
            index = StatLib.on_damageLevels.ToList().IndexOf(GlobalState.Stats.DamageLevel) + 1;
            if (index < 5) {updatedValues[1] = StatLib.on_damageLevels[index].ToString();costs[1]*= (int)Math.Pow(2,index-1);} 
            else updatedValues[1] = maxed;
        }
        else{
            index = StatLib.bug_damageLevels.ToList().IndexOf(GlobalState.Stats.DamageLevel) + 1;
            if (index < 5) {updatedValues[1] = StatLib.bug_damageLevels[index].ToString();costs[1]*= (int)Math.Pow(2,index-1);} 
            else updatedValues[1] = maxed;
        }

        index = StatLib.energyLevels.ToList().IndexOf(GlobalState.Stats.Energy) + 1;
        if (index < 5) {updatedValues[2] = StatLib.energyLevels[index].ToString();costs[2]*= (int)Math.Pow(2,index-1);} 
        else updatedValues[2] = maxed;

        index = StatLib.xpboost.ToList().IndexOf(GlobalState.Stats.XPBoost) + 1;
        if (index < 5) {updatedValues[3] = StatLib.xpboost[index].ToString();costs[3]*= (int)Math.Pow(4,index-1);} 
        else updatedValues[3] = maxed;


        //update the text, and indicate the next tier they can get.
        foreach (GameObject button in buttons)
        {
            Text text = button.transform.GetChild(0).GetComponent<Text>();
            text.text = starterText[counter] + values[counter] + " >> " + updatedValues[counter] + " COST: " + costs[counter];
            counter++;
        }
        CheckInteractable();

        ui.UpdateText(points.ToString());
    }
}

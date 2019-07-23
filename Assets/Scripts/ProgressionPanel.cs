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
    List<GameObject> buttons; 
    int[] costs; 
    string[] starterText; 
    ProgressionUI ui; 
    void Start(){
        ui = GetComponent<ProgressionUI>(); 
        if (GlobalState.Stats == null) GlobalState.Stats = new CharacterStats(true); 
        buttons = new List<GameObject>(); 
        starterText = new string[4];
        costs = new int[]{stateLib.COST_SPEED, stateLib.COST_DAMAGE_REDUCE, stateLib.COST_HEALTH, stateLib.COST_XPBOOST}; 
        for (int i = 0; i < 4; i++){
            buttons.Add(transform.GetChild(i).gameObject); 
            starterText[i] = buttons[i].transform.GetChild(0).GetComponent<Text>().text; 
            if (i < costs.Length && GlobalState.Stats.Points < costs[i]){
                buttons[i].GetComponent<Button>().interactable = false; 
            }
        }
        ui.AnimateButtons(buttons); 
        UpdateValues(); 
        
    }
    public void EndScene(){
        GameObject.Find("Fade").GetComponent<Fade>().onFadeOut(); 
        StartCoroutine(WaitForSwitchScene()); 
    }
    IEnumerator WaitForSwitchScene(){
        yield return new WaitForSeconds(1f); 
        if (GlobalState.level.FileName.Contains("tutorial")) GlobalState.level.IsDemo = true; 
        SceneManager.LoadScene("newgame"); 
    }
    /// <summary>
    /// Upgrades the Speed to the next tier unless maxed out.
    /// </summary>
    public void OnUpgradeSpeed(){
        GlobalState.Stats.Points -= stateLib.COST_SPEED; 
        int index = StatLib.speeds.ToList().IndexOf(GlobalState.Stats.Speed) + 1; 
        if (index < StatLib.speeds.Length)
            GlobalState.Stats.Speed = StatLib.speeds[index]; 
        UpdateValues(); 
    }
    /// <summary>
    /// Upgrades the Damage taken to the next tier unless maxed out.
    /// </summary>
    public void OnUpgradeDamageReduce(){
        GlobalState.Stats.Points -= stateLib.COST_DAMAGE_REDUCE; 
        int index = StatLib.damageLevels.ToList().IndexOf(GlobalState.Stats.DamageLevel) + 1; 
        if (index < StatLib.damageLevels.Length)
            GlobalState.Stats.DamageLevel = StatLib.damageLevels[index];
        UpdateValues(); 
    }
    /// <summary>
    /// Upgrades the Energy to the next tier unless maxed out.
    /// </summary>
    public void OnUpgradeEnergy(){
        GlobalState.Stats.Points -= stateLib.COST_HEALTH; 
        int index = StatLib.energyLevels.ToList().IndexOf(GlobalState.Stats.Energy) + 1; 
        if (index < StatLib.energyLevels.Length)
            GlobalState.Stats.Energy = StatLib.energyLevels[index];
        UpdateValues(); 
    }
    public void OnUpgradeFreefall(){
         GlobalState.Stats.Points -= stateLib.COST_XPBOOST; 
        int index = StatLib.xpboost.ToList().IndexOf(GlobalState.Stats.XPBoost) + 1; 
        if (index < StatLib.xpboost.Length)
            GlobalState.Stats.XPBoost = StatLib.xpboost[index];
        UpdateValues(); 
    }
    void UpdateValues(){
        int counter = 0; 
        string[] values = new string[4]{GlobalState.Stats.Speed.ToString(), GlobalState.Stats.DamageLevel.ToString(),
                            GlobalState.Stats.Energy.ToString(), GlobalState.Stats.XPBoost.ToString()};
        int[] costs = new int[4]{stateLib.COST_SPEED, stateLib.COST_DAMAGE_REDUCE, stateLib.COST_HEALTH, stateLib.COST_XPBOOST};
        string[] updatedValues = new string[values.Length]; 
        string maxed = "Maxed Out!"; 
        //Find the next tier 
        int index = StatLib.speeds.ToList().IndexOf(GlobalState.Stats.Speed) + 1;
        if (index < 5) updatedValues[0] = StatLib.speeds[index].ToString();
        else updatedValues[0] = maxed; 

        index = StatLib.damageLevels.ToList().IndexOf(GlobalState.Stats.DamageLevel) + 1;
        if (index < 5) updatedValues[1] = StatLib.damageLevels[index].ToString();
        else updatedValues[1] = maxed; 
        
        index = StatLib.energyLevels.ToList().IndexOf(GlobalState.Stats.Energy) + 1;
        if (index < 5) updatedValues[2] = StatLib.energyLevels[index].ToString();
        else updatedValues[2] = maxed; 

        index = StatLib.xpboost.ToList().IndexOf(GlobalState.Stats.XPBoost) + 1;
        if (index < 5) updatedValues[2] = StatLib.xpboost[index].ToString();
        else updatedValues[2] = maxed; 
        
        updatedValues[3] = maxed; 

        //update the text, and indicate the next tier they can get.
        foreach (GameObject button in buttons){
            Text text = button.transform.GetChild(0).GetComponent<Text>(); 
            text.text = starterText[counter] + values[counter] + " >> " + updatedValues[counter] + " COST: " + costs[counter];
            if (counter < costs.Length && GlobalState.Stats.Points < costs[counter]){
                button.GetComponent<Button>().interactable = false; 
            }
            counter++;  
        }
        ui.UpdateText(); 
    }
}

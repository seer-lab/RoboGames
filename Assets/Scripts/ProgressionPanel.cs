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
    float[] originalValues;
    ProgressionUI ui;
    int points;
    void Start()
    {
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
        ui.AnimateButtons(buttons);
        UpdateValues();

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
        int index = StatLib.speeds.ToList().IndexOf(GlobalState.Stats.Speed) + 1;
        points -= stateLib.COST_SPEED*index;
        if (index < StatLib.speeds.Length)
            GlobalState.Stats.Speed = StatLib.speeds[index];
        UpdateValues();
    }
    /// <summary>
    /// Upgrades the Damage taken to the next tier unless maxed out.
    /// </summary>
    public void OnUpgradeDamageReduce()
    {
        
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
        UpdateValues();
    }
    /// <summary>
    /// Upgrades the Energy to the next tier unless maxed out.
    /// </summary>
    public void OnUpgradeEnergy()
    {
        int index = StatLib.energyLevels.ToList().IndexOf(GlobalState.Stats.Energy) + 1;
        points -= stateLib.COST_HEALTH*index;
        if (index < StatLib.energyLevels.Length)
            GlobalState.Stats.Energy = StatLib.energyLevels[index];
        UpdateValues();
    }
    public void OnUpgradeFreefall()
    {
       
        int index = StatLib.xpboost.ToList().IndexOf(GlobalState.Stats.XPBoost) + 1;
        points -= stateLib.COST_XPBOOST*index;
        if (index < StatLib.xpboost.Length)
            GlobalState.Stats.XPBoost = StatLib.xpboost[index];
        UpdateValues();
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
        if (index < 5){ updatedValues[0] = StatLib.speeds[index].ToString(); costs[0]*= index;} 
        else updatedValues[0] = maxed;

        if (GlobalState.GameMode == stringLib.GAME_MODE_ON){
            index = StatLib.on_damageLevels.ToList().IndexOf(GlobalState.Stats.DamageLevel) + 1;
            if (index < 5) {updatedValues[1] = StatLib.on_damageLevels[index].ToString();costs[1]*= index;} 
            else updatedValues[1] = maxed;
        }
        else{
            index = StatLib.bug_damageLevels.ToList().IndexOf(GlobalState.Stats.DamageLevel) + 1;
            if (index < 5) {updatedValues[1] = StatLib.bug_damageLevels[index].ToString();costs[1]*= index;} 
            else updatedValues[1] = maxed;
        }

        index = StatLib.energyLevels.ToList().IndexOf(GlobalState.Stats.Energy) + 1;
        if (index < 5) {updatedValues[2] = StatLib.energyLevels[index].ToString();costs[2]*= index;} 
        else updatedValues[2] = maxed;

        index = StatLib.xpboost.ToList().IndexOf(GlobalState.Stats.XPBoost) + 1;
        if (index < 5) {updatedValues[3] = StatLib.xpboost[index].ToString();costs[3]*= index;} 
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

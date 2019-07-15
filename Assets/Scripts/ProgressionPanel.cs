using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 
using UnityEngine.UI; 


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
        costs = new int[]{stateLib.COST_SPEED, stateLib.COST_PROJECTILE, stateLib.COST_HEALTH}; 
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
    public void OnUpgradeSpeed(){
        GlobalState.Stats.Points -= stateLib.COST_SPEED; 
        int index = StatLib.speeds.ToList().IndexOf(GlobalState.Stats.Speed) + 1; 
        if (index < StatLib.speeds.Length)
            GlobalState.Stats.Speed = StatLib.speeds[index]; 
        UpdateValues(); 
    }
    public void OnUpgradeProjectile(){
        GlobalState.Stats.Points = stateLib.COST_PROJECTILE; 
        int index = StatLib.projectileDistance.ToList().IndexOf(GlobalState.Stats.ProjectileTime) + 1; 
        if (index < StatLib.projectileDistance.Length)
            GlobalState.Stats.ProjectileTime = StatLib.projectileDistance[index];
        UpdateValues(); 
    }
    public void OnUpgradeEnergy(){
        GlobalState.Stats.Points = stateLib.COST_HEALTH; 
        int index = StatLib.energyLevels.ToList().IndexOf(GlobalState.Stats.Energy) + 1; 
        if (index < StatLib.energyLevels.Length)
            GlobalState.Stats.Energy = StatLib.energyLevels[index];
        UpdateValues(); 
    }
    public void OnUpgradeFreefall(){
        GlobalState.Stats.FreeFall = true; 
        UpdateValues(); 
    }
    void UpdateValues(){
        int counter = 0; 
        string[] values = new string[4]{GlobalState.Stats.Speed.ToString(), GlobalState.Stats.ProjectileTime.ToString(),
                            GlobalState.Stats.Energy.ToString(), GlobalState.Stats.FreeFall.ToString()};
        string[] updatedValues = new string[values.Length]; 
        string maxed = "Maxed Out!"; 

        int index = StatLib.speeds.ToList().IndexOf(GlobalState.Stats.Speed) + 1;
        if (index < 5) updatedValues[0] = StatLib.speeds[index].ToString();
        else updatedValues[0] = maxed; 

        index = StatLib.projectileDistance.ToList().IndexOf(GlobalState.Stats.ProjectileTime) + 1;
        if (index < 5) updatedValues[1] = StatLib.projectileDistance[index].ToString();
        else updatedValues[1] = maxed; 
        
        index = StatLib.energyLevels.ToList().IndexOf(GlobalState.Stats.Energy) + 1;
        if (index < 5) updatedValues[2] = StatLib.energyLevels[index].ToString();
        else updatedValues[2] = maxed; 
        
        updatedValues[3] = maxed; 

        foreach (GameObject button in buttons){
            Text text = button.transform.GetChild(0).GetComponent<Text>(); 
            text.text = starterText[counter] + values[counter] + " >> " + updatedValues[counter];
            if (counter < costs.Length && GlobalState.Stats.Points < costs[counter]){
                button.GetComponent<Button>().interactable = false; 
            }
            counter++;  
        }
        ui.UpdateText(); 
    }
}

using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 
using UnityEngine.UI; 


public class ProgressionPanel : MonoBehaviour
{
    List<GameObject> buttons; 
    string[] starterText; 
    void Start(){
        if (GlobalState.Stats == null) GlobalState.Stats = new CharacterStats(true); 
        buttons = new List<GameObject>(); 
        starterText = new string[4];
        for (int i = 0; i < 4; i++){
            buttons.Add(transform.GetChild(i).gameObject); 
            starterText[i] = buttons[i].transform.GetChild(0).GetComponent<Text>().text; 
        }
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
        int index = StatLib.speeds.ToList().IndexOf(GlobalState.Stats.Speed) + 1; 
        if (index < StatLib.speeds.Length)
            GlobalState.Stats.Speed = StatLib.speeds[index]; 
        UpdateValues(); 
    }
    public void OnUpgradeProjectile(){
        int index = StatLib.projectileDistance.ToList().IndexOf(GlobalState.Stats.ProjectileTime) + 1; 
        if (index < StatLib.projectileDistance.Length)
            GlobalState.Stats.ProjectileTime = StatLib.projectileDistance[index];
        UpdateValues(); 
    }
    public void OnUpgradeEnergy(){
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
        foreach (GameObject button in buttons){
            Text text = button.transform.GetChild(0).GetComponent<Text>(); 
            text.text = starterText[counter] + values[counter];
            counter++;  
        }
    }
}

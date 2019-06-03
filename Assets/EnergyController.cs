﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class EnergyController : MonoBehaviour
{
    GameObject energyBar; 
    float initialEnergy;
    public float currentEnergy;
    Text indicator;
    float[] throwEnergy = new float[stateLib.NUMBER_OF_TOOLS];
    SelectedTool tools;
    bool initial = true;
    float initialScale; 
    // Start is called before the first frame update
    void Start()
    {
        initialEnergy = GlobalState.TotalEnergy;
        currentEnergy = initialEnergy;
        indicator = transform.GetChild(0).GetComponent<Text>();
        tools = GameObject.Find("Sidebar").transform.Find("Sidebar Tool").GetComponent<SelectedTool>();
        energyBar = transform.GetChild(1).gameObject; 
        initialScale = energyBar.GetComponent<RectTransform>().localScale.x; 
    }
    public void onThrow(int projectileCode)
    {
        
        currentEnergy -= throwEnergy[projectileCode];
        if (GlobalState.GameMode == "bug" && projectileCode == stateLib.TOOL_CATCHER_OR_CONTROL_FLOW)
        {
            currentEnergy = 0;
        }
        if (currentEnergy > 0 )
            indicator.text = (currentEnergy / initialEnergy).ToString() + '%';
        else indicator.text = "0%"; 
        energyBar.GetComponent<RectTransform>().localScale = new Vector3(initialScale*((currentEnergy / initialEnergy)), 1, 1); 
    }
    public void onFail(int projectileCode){
        currentEnergy-= throwEnergy[projectileCode];
        if (currentEnergy> 0)
            indicator.text = (currentEnergy / initialEnergy).ToString() + '%'; 
        else indicator.text = "0%"; 
        energyBar.GetComponent<RectTransform>().localScale = new Vector3(initialScale*(currentEnergy / initialEnergy), 1, 1); 
    }
    // Update is called once per frame
    void Update()
    {
        if (initial)
        {
            int totalCounts = 0; 
            for (int i = 0; i < stateLib.NUMBER_OF_TOOLS; i++){
                if (tools.toolCounts[i] > 0){
                    totalCounts++; 
                }
            } 
            Debug.Log(totalCounts); 
            for (int i = 0; i < stateLib.NUMBER_OF_TOOLS; i++)
            {
                throwEnergy[i] = (100f/((float)totalCounts));
                if (tools.toolCounts[i] > 0){
                    if (tools.toolCounts[i] < 999)
                        throwEnergy[i] /= (float)tools.toolCounts[i]; 
                    else throwEnergy[i] /= (GlobalState.level.Tasks[i] + 5);
                }
                else if (i < 5){
                    throwEnergy[i] /= (GlobalState.level.Tasks[i] + 10);
                }
                
            }
            initial = false;
        }
    }
}
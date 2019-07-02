using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class EnergyController : MonoBehaviour
{
    GameObject energyBar;
    RectTransform energyBarTrans;
    float initialEnergy;
    public float currentEnergy;
    Text indicator;
    float[] throwEnergy = new float[stateLib.NUMBER_OF_TOOLS];
    SelectedTool tools;
    bool initial = true;
    bool hidden = false;
    float initialScale;
    float initialX;
    float positionCompensation = 160f;
    bool toggle = false; 
    public float[] percentPerUse()
    {
        float[] percent = new float[throwEnergy.Length];
        for (int i = 0; i < percent.Length; i++)
        {
            if (GlobalState.GameMode == "bug" && i == stateLib.TOOL_CATCHER_OR_CONTROL_FLOW)
            {
                percent[i] = 99f;
            }
            else percent[i] = (throwEnergy[i] / initialEnergy) * 100f;
        }
        return percent;
    }
    public void ToggleEnergy()
    {
        toggle = !toggle; 
        if (!toggle){
            energyBar.GetComponent<Image>().enabled = true; 
            energyBar.transform.GetChild(0).GetComponent<Image>().enabled = true; 
            indicator.enabled = true; 
        }
        else {
            energyBar.GetComponent<Image>().enabled = false; 
            energyBar.transform.GetChild(0).GetComponent<Image>().enabled = false; 
            indicator.enabled = false; 
        }
    }
    public void ToggleLight(){
        indicator.color = Color.black; 
    }
    public void ToggleDark(){
        indicator.color = Color.white; 
    }
    // Start is called before the first frame update
    void Start()
    {
        initialEnergy = GlobalState.TotalEnergy;
        currentEnergy = initialEnergy;
        indicator = transform.GetChild(0).GetComponent<Text>();
        tools = GameObject.Find("Sidebar").transform.GetChild(2).transform.Find("Sidebar Tool").GetComponent<SelectedTool>();
        energyBar = transform.GetChild(1).gameObject;
        energyBarTrans = energyBar.GetComponent<RectTransform>();
        initialX = energyBarTrans.position.x;
        initialScale = energyBar.GetComponent<RectTransform>().localScale.x;
    }
    public void onThrow(int projectileCode)
    {

        currentEnergy -= throwEnergy[projectileCode];
        if (GlobalState.GameMode == "bug" && projectileCode == stateLib.TOOL_CATCHER_OR_CONTROL_FLOW)
        {
            currentEnergy = 0;
        }
        updateBar(); 
    }
    public void onFail(int projectileCode)
    {
        currentEnergy -= throwEnergy[projectileCode];
        updateBar();
    }
    public void onDamange(float damage)
    {
        currentEnergy -= damage;
        updateBar();
    }
    void updateBar()
    {
        if (currentEnergy > 0)
        {
            indicator.text = ((int)(currentEnergy * 100f / initialEnergy)).ToString() + '%';
            energyBar.GetComponent<RectTransform>().localScale = new Vector3(initialScale * ((currentEnergy / initialEnergy)), 1, 1);
            energyBarTrans.position = new Vector3(initialX + positionCompensation * ((initialEnergy - currentEnergy) / initialEnergy), energyBarTrans.position.y, 0);
        }
        else
        {
            indicator.text = "0%";
            energyBar.GetComponent<RectTransform>().localScale = new Vector3(0, 1, 1);
        }
    }
    void LateUpdate(){
        if (currentEnergy> 0)
         energyBarTrans.position = new Vector3(initialX + positionCompensation * ((initialEnergy - currentEnergy) / initialEnergy), energyBarTrans.position.y, 0);
    }
    // Update is called once per frame
    void Update()
    {
        if (initial)
        {
            int totalCounts = 0;
            for (int i = 0; i < stateLib.NUMBER_OF_TOOLS; i++)
            {
                if (tools.toolCounts[i] > 0)
                {
                    totalCounts++;
                }
            }
            //Debug.Log(totalCounts); 
            for (int i = 0; i < stateLib.NUMBER_OF_TOOLS; i++)
            {
                throwEnergy[i] = (100f / ((float)totalCounts));
                if (tools.toolCounts[i] > 0)
                {
                    if (tools.toolCounts[i] < 999)
                        throwEnergy[i] /= (float)tools.toolCounts[i];
                    else throwEnergy[i] /= (GlobalState.level.Tasks[i] + 5);
                }
                else if (i < 5)
                {
                    throwEnergy[i] /= (GlobalState.level.Tasks[i] + 10);
                }

            }
            initial = false;
        }
        if (GlobalState.GameState != stateLib.GAMESTATE_IN_GAME && !hidden)
        {
            energyBar.GetComponent<Image>().enabled = false;
            energyBar.transform.GetChild(0).GetComponent<Image>().enabled = false;
            indicator.text = "";
            hidden = true;
        }
        else if (GlobalState.GameState == stateLib.GAMESTATE_IN_GAME && hidden && !toggle)
        {
            energyBar.GetComponent<Image>().enabled = true;
            energyBar.transform.GetChild(0).GetComponent<Image>().enabled = false;
            if (currentEnergy > 0)
                indicator.text = ((int)(currentEnergy * 100f / initialEnergy)).ToString() + '%';
            else indicator.text = "0%";
            hidden = false;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class EnergyController : MonoBehaviour
{
    GameObject energyBar;
    RectTransform energyBarTrans;
    float initialEnergy;
    public float currentEnergy, displayEnergy, originalEnergy;
    Text indicator;
    float[] throwEnergy = new float[stateLib.NUMBER_OF_TOOLS];
    SelectedTool tools;
    bool initial = true;
    bool hidden = false;
    float initialScale;
    float initialX, topBar, bottomBar;
    float positionCompensation = 150f;
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
            else percent[i] = (throwEnergy[i] / initialEnergy) * (float)GlobalState.Stats.Energy;
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
        originalEnergy = GlobalState.Stats.Energy;
        currentEnergy = originalEnergy;
        indicator = transform.GetChild(0).GetComponent<Text>();
        indicator.text = originalEnergy.ToString() + "%"; 
        tools = GameObject.Find("Sidebar").transform.GetChild(2).transform.Find("Sidebar Tool").GetComponent<SelectedTool>();
        topBar = transform.GetChild(2).gameObject.GetComponent<RectTransform>().position.x;
        bottomBar = transform.GetChild(1).gameObject.GetComponent<RectTransform>().position.x;  
        SelectBar(); 
        initialScale = energyBar.GetComponent<RectTransform>().localScale.x;
        updateBar(); 
        if (energyBar.name == transform.GetChild(1).gameObject.name){
            transform.GetChild(2).gameObject.GetComponent<Image>().enabled = false; 
            transform.GetChild(2).transform.GetChild(0).gameObject.GetComponent<Image>().enabled = false; 
        }
        else {
            energyBar.GetComponent<RectTransform>().localScale = new Vector3(initialScale * ((displayEnergy / initialEnergy)), 1, 1);
            energyBarTrans.position = new Vector3(initialX + positionCompensation * ((initialEnergy - displayEnergy) / initialEnergy), energyBarTrans.position.y, 0);
        }
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
    void SelectBar(){
        if (currentEnergy - 100 >= 0){
            energyBar = transform.GetChild(2).gameObject; 
            energyBarTrans = energyBar.GetComponent<RectTransform>(); 
            displayEnergy = currentEnergy -100; 
            initialEnergy = 100; 
            initialX = topBar;
            Debug.Log(initialX); 
        }
        else {
            energyBar = transform.GetChild(1).gameObject; 
            energyBarTrans = energyBar.GetComponent<RectTransform>(); 
            displayEnergy = currentEnergy; 
            initialEnergy = 100; 
            initialX = bottomBar;
        }
    }
    void updateBar()
    {
        SelectBar(); 
        if (currentEnergy > 0)
        {
            indicator.text = ((int)(currentEnergy * GlobalState.Stats.Energy / originalEnergy)).ToString() + '%';
            energyBar.GetComponent<RectTransform>().localScale = new Vector3(initialScale * ((displayEnergy / initialEnergy)), 1, 1);
            energyBarTrans.position = new Vector3(initialX + positionCompensation * ((initialEnergy - displayEnergy) / initialEnergy), energyBarTrans.position.y, 0);
        }
        else
        {
            indicator.text = "0%";
            energyBar.GetComponent<RectTransform>().localScale = new Vector3(0, 1, 1);
        }
    }
    void LateUpdate(){
        if (currentEnergy> 0)
            energyBarTrans.position = new Vector3(initialX + positionCompensation * ((initialEnergy - displayEnergy) / initialEnergy), energyBarTrans.position.y, 0);
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
                throwEnergy[i] = ( (float)GlobalState.Stats.Energy / ((float)totalCounts));
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
            transform.GetChild(1).gameObject.GetComponent<Image>().enabled = false;
            transform.GetChild(1).gameObject.transform.GetChild(0).GetComponent<Image>().enabled = false;
            transform.GetChild(2).gameObject.GetComponent<Image>().enabled = false;
            transform.GetChild(2).gameObject.transform.GetChild(0).GetComponent<Image>().enabled = false;
            
            indicator.text = "";
            hidden = true;
        }
        else if (GlobalState.GameState == stateLib.GAMESTATE_IN_GAME && hidden && !toggle)
        {
            transform.GetChild(1).gameObject.GetComponent<Image>().enabled = true;
            transform.GetChild(1).gameObject.transform.GetChild(0).GetComponent<Image>().enabled = true;
            transform.GetChild(2).gameObject.GetComponent<Image>().enabled = true;
            transform.GetChild(2).gameObject.transform.GetChild(0).GetComponent<Image>().enabled = true;
            
            if (currentEnergy > 0)
                indicator.text = ((int)(currentEnergy * GlobalState.Stats.Energy / originalEnergy)).ToString() + '%';
            else indicator.text = "0%";
            hidden = false;
        }
    }
}

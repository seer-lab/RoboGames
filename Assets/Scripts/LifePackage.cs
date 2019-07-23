using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifePackage : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D colldingObj){
        if (colldingObj.gameObject.name == "Hero"){
            Logger log = new Logger();
            float preEnergy = GameObject.Find("Energy").GetComponent<EnergyController>().currentEnergy;
            GameObject.Find("Energy").GetComponent<EnergyController>().onEnergyReset(); 
            
            log.onStateChangeEnergy("Life Package",4 ,this.gameObject.transform.position, preEnergy
                                    ,GameObject.Find("Energy").GetComponent<EnergyController>().currentEnergy,
                                    true,0);

            GameObject.Find("CodeScreen").GetComponent<LevelGenerator>().floatingTextOnPlayer(Color.white);
            GameObject.Find("OutputCanvas").transform.GetChild(0).GetComponent<Output>().Text.text= "Energy up!";
            Destroy(this.gameObject); 
        }
    }
}

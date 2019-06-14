using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoBoyControl : MonoBehaviour
{
    hero2Controller movement; 
    // Start is called before the first frame update
    void Start()
    {
        movement = this.GetComponent<hero2Controller>(); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

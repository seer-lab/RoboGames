using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SparkController : MonoBehaviour
{
    Animator anim; 
    RectTransform pos; 
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>(); 
        pos = GetComponent<RectTransform>(); 
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)){
            pos.position = Input.mousePosition; 
            pos.position = new Vector3(pos.position.x, pos.position.y, 5);
            anim.SetTrigger("Spark"); 
        }
    }
}

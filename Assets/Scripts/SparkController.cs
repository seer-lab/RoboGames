using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SparkController : MonoBehaviour
{
    Animator anim; 
    RectTransform pos; 
    bool delaying = false; 
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>(); 
        pos = GetComponent<RectTransform>(); 
    }
    IEnumerator DelaySpark(){
        delaying = true; 
        yield return new WaitForSecondsRealtime(0.5f); 
        anim.ResetTrigger("Spark"); 
        delaying = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !delaying){
            pos.position = Input.mousePosition; 
            pos.position = new Vector3(pos.position.x, pos.position.y, 5);
            anim.ResetTrigger("Spark");
            anim.SetTrigger("Spark");  
            StartCoroutine(DelaySpark()); 
        }
    }
}

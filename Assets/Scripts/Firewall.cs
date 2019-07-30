using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firewall : Obstacle
{
    public int Damage { get; set; }

    Collider2D lastHero;
    public override void SetPosition()
    {
        base.SetPosition(); 
        if (GlobalState.TextSize == 3)
        {
            this.transform.position += new Vector3(2.4f, 0, 0);
            this.transform.localScale += new Vector3(0.8f, 0, 0);
        }
    }
    public override string GetObstacleType()
    {
        return "Firewall";
    }
    void OnTriggerEnter2D(Collider2D collidingObj)
    {
        if (collidingObj.name == "Hero")
        {
            if (collidingObj.GetComponent<hero2Controller>().onTakeDamage(Damage, stateLib.OBSTACLE_FIREWALL))
                GetComponent<AudioSource>().Play();
            lastHero = collidingObj;
        }
    }
}

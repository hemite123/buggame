using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDemage : MonoBehaviour
{
    public int demagePerSecond;
    Gamemanager gm;

    private void Start()
    {
        gm = Gamemanager.instace;
    }


    private void OnTriggerStay(Collider collision)
    {                   
        if (collision.transform.GetComponent<Objective>() && gm.leftclick)
        {
            Objective obj = collision.transform.GetComponent<Objective>();
            if (!obj.isSkillTrigger && obj.isOn)
            {
                gm.leftclick = false;
                if (!obj.isSkillTrigger)
                {
                    obj.HitEnemy(Random.Range(1, demagePerSecond));
                }
               
            }
            
        }
        else if(collision.tag == "offbtn" && gm.leftclick)
        {
            if (!gm.pauseTime)
            {
                gm.pauseTime = true;
                StartCoroutine(gm.resetTimeActive());
            }
        }
    }

   

  
}

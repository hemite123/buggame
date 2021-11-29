using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonkManagement : MonoBehaviour
{
    
    private void OnTriggerEnter(Collider collision)
    {
        Gamemanager gm = Gamemanager.instace;
        if (collision.transform.tag != "envi")
        {
            if (collision.transform.tag == "player" && gm.isBonk && !gm.isDone)
            {
                gm.PLose();
            }
           
            Destroy(this.gameObject);
           
            gm.CooldownBonk();
        }
        
       
       
    }
}

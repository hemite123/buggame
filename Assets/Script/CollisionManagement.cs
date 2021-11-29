using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionManagement : MonoBehaviour
{
    public string objectname;
    ArcadeScript ascript;

    private void Start()
    {
        ascript = ArcadeScript.instance;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(objectname == "enemyarcade")
        {
            if (collision.gameObject.tag == "parcade")
            {
                if (ascript.playerhealth >= 1)
                {
                    ascript.playerhealth -= 1;
                }
                ascript.DestroyGo(this.gameObject);
            }
            else if(collision.gameObject.tag == "destroyobject")
            {
                if (ascript.arcadeDodge)
                {
                    ascript.curentcond += 1;
                }
                else if(ascript.arcadeShoot)
                {
                    ascript.playerhealth -= 1;
                }
                
                ascript.DestroyGo(this.gameObject);
            }
        }else if(objectname == "playerarcade")
        {
            if(collision.transform.tag == "obstacel")
            {
                if(ascript.playerhealth >= 1 && ascript.arcadeDodge)
                {
                    ascript.playerhealth -= 1;
                }
            }
        }else if(objectname == "projectile")
        {
            if (collision.transform.tag == "enemy")
            {
                if (ascript.arcadeShoot)
                {
                    ascript.curentcond += 1;
                    Destroy(collision.gameObject);
                }
                Destroy(this.gameObject);
            }
            
        }
       
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (objectname == "projectile")
        {
            if (collision.transform.tag == "spawnarea")
            {
                Destroy(this.gameObject);
            }

        }
    }

   
}

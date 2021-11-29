using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objective : MonoBehaviour
{
    public int health;
    public int curHeal;
    public GameObject objectChange;
    public string toolname;
    public bool isOn;
    public bool isSkillTrigger;
    public bool isArcade;
    Gamemanager gmane;

    private void Start()
    {
        gmane = Gamemanager.instace;
        curHeal = health;
    }

    public void HitEnemy(int demage)
    {
        if (!isArcade)
        {
            curHeal -= demage;
            gmane.health.gameObject.SetActive(true);
            gmane.health.maxValue = health;
            gmane.health.value = curHeal;
            if (curHeal <= 0)
            {
                gmane.ObjectiveSetter(this.gameObject, true);
                if (toolname == "cpu" || toolname == "pin" || toolname == "cable")
                {
                    this.gameObject.GetComponent<ParticleSystem>().Play();
                }
                else if (toolname == "ram")
                {
                    gameObject.SetActive(false);
                    gmane.brokenRam.SetActive(true);
                }
                else if (toolname == "harddisk")
                {
                    this.gameObject.SetActive(false);
                    gmane.brokenHarddrive.SetActive(true);
                }
                gmane.health.gameObject.SetActive(false);
            }
        }
       
    }
}

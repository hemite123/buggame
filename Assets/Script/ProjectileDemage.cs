using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileDemage : MonoBehaviour
{
    public int demageOuput;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Objective>())
        {
            other.GetComponent<Objective>().HitEnemy(demageOuput);
            this.gameObject.SetActive(false);
        }
    }

    

    private void OnBecameInvisible()
    {
        this.gameObject.SetActive(false);
    }
}

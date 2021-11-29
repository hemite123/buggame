using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CamManagement : MonoBehaviour
{
    public static CamManagement instance;
    [SerializeField]
    public List<CinemachineVirtualCamera> camList = new List<CinemachineVirtualCamera>();
    [SerializeField]
    public List<CinemachineFreeLook> freeLook = new List<CinemachineFreeLook>();


    private void Awake()
    {
        if(instance != null)
        {
            return;
        }
        instance = this;
    }

  

    public void ChangeCam(string camname)
    {
        foreach(CinemachineVirtualCamera list in camList)
        {
            if(list.isActiveAndEnabled && list.name == camname)
            {
                return;
            }
            else if (!list.isActiveAndEnabled && list.name == camname)
            {
                list.gameObject.SetActive(true);
            }
            else
            {
                list.gameObject.SetActive(false);
            }
        }
        foreach (CinemachineFreeLook list in freeLook)
        {
            if (list.isActiveAndEnabled && list.name == camname)
            {
                return;
            }
            else if (!list.isActiveAndEnabled && list.name == camname)
            {
                list.gameObject.SetActive(true);
            }
            else
            {
                list.gameObject.SetActive(false);
            }
        }
    }
}

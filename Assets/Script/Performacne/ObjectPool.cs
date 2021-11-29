using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool instance;
    public List<GameObject> objectpooled = new List<GameObject>();
    public GameObject objectToPool;
    public int amountPool;

    private void Awake()
    {
        if(instance != null)
        {
            return;
        }
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        for(int i =0;i < amountPool; i++)
        {
            GameObject obj = (GameObject)Instantiate(objectToPool);
            obj.SetActive(false);
            objectpooled.Add(obj);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject GetPooledObject()
    {
        for(int i = 0; i < objectpooled.Count; i++)
        {
            if (!objectpooled[i].activeInHierarchy)
            {
                return objectpooled[i];
            }
        }
        return null;
    }
}

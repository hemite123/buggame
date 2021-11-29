using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopMode
{
    public List<shoplist> shoplist = new List<shoplist>();
}

[System.Serializable]
public class shoplist
{
    public string skillName;
    public int[] money;
}

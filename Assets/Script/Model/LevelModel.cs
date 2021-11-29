using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelModel
{
    public List<lmodel> levelList = new List<lmodel>();
}

[System.Serializable]
public class lmodel
{
    public string lname;
    public string scenename;
    public float starttime;
    public float[] timeStar;

}

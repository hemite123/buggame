using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillPhase
{
    public List<skillphase> skillphase = new List<skillphase>();
}

[System.Serializable]
public class skillphase
{
    public string skillName;
    public float[] coolDown;
}

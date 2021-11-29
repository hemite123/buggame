using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel
{
    public string bugcoin;
    public List<SkillName> sname = new List<SkillName>();
    public List<LevelManager> lmanager = new List<LevelManager>();

   
    
}

[System.Serializable]
public class SkillName
{
    public string skillName;
    public int phase;

    public SkillName(string x,int y)
    {
        skillName = x;
        phase = y;
    }

}

[System.Serializable]
public class LevelManager
{
    public string levelname;
    public int star;

    public LevelManager(string x , int y)
    {
        levelname = x;
        star = y;
    }
}


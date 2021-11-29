using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class DataPersinstance : MonoBehaviour
{
   

    public void SaveToJson(PlayerModel pmodel)
    {
        string json = JsonUtility.ToJson(pmodel, true);
        File.WriteAllText(Application.dataPath + "/pData.json", json);
        
    }

    public PlayerModel LoadFromJson(string jsonload)
    {
        string json = File.ReadAllText(Application.dataPath + "/" + jsonload);
        PlayerModel pmodel = JsonUtility.FromJson<PlayerModel>(json);
        return pmodel;
    }

    public ShopMode LoadShop(string jsonload)
    {
        string json = File.ReadAllText(Application.dataPath + "/" + jsonload);
        ShopMode pmodel = JsonUtility.FromJson<ShopMode>(json);
        return pmodel;
    }

    public SkillPhase LoadSkillPhase(string jsonload)
    {
        string json = File.ReadAllText(Application.dataPath + "/" + jsonload);
        SkillPhase pmodel = JsonUtility.FromJson<SkillPhase>(json);
        return pmodel;
    }

    public LevelModel LoadLevelData(string jsonload)
    {
        string json = File.ReadAllText(Application.dataPath + "/" + jsonload);
        LevelModel pmodel = JsonUtility.FromJson<LevelModel>(json);
        return pmodel;
    }
}

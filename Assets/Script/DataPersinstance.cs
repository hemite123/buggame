using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class DataPersinstance : MonoBehaviour
{
    SaveDataManagement sdm;

    private void Start()
    {
        sdm = SaveDataManagement.instance;
    }

    public void SaveToJson(PlayerModel pmodel)
    {
        string json = JsonUtility.ToJson(pmodel, true);
        File.WriteAllText(Application.persistentDataPath + "/pData.json", json);

        
        
    }

    public PlayerModel LoadFromJson(string jsonload)
    {
        string json = File.ReadAllText(Application.persistentDataPath + "/" + jsonload);
        PlayerModel pmodel = JsonUtility.FromJson<PlayerModel>(json);
        return pmodel;
    }

    public ShopMode LoadShop(string jsonload)
    {
        string json;
        StartCoroutine(loadJson("https://hemite123.github.io/buggame/shopData.json", "shop"));
        return null;
    }

    public SkillPhase LoadSkillPhase(string jsonload)
    {
        string json;
        StartCoroutine(loadJson("https://hemite123.github.io/buggame/skillPhase.json", "skill"));
        return null;
    }

    public LevelModel LoadLevelData(string jsonload)
    {
        string json;
        StartCoroutine(loadJson("https://hemite123.github.io/buggame/levelData.json", "level"));
        return null;
    }

    IEnumerator loadJson(string url,string mdl)
    {
        WWW www = new WWW(url);
        yield return www;
        if(www.error == null)
        {
            LoadJsonToVar(www.text,mdl);
        }
    }

    public void LoadJsonToVar(string result,string model)
    {
        if(model == "level")
        {
            LevelModel pmodel = JsonUtility.FromJson<LevelModel>(result);
            sdm.lmodel = pmodel;
        }
        else if(model == "skill")
        {
            SkillPhase pmodel = JsonUtility.FromJson<SkillPhase>(result);
            sdm.sphase = pmodel;
        }
        else if(model == "shop")
        {
            ShopMode pmodel = JsonUtility.FromJson<ShopMode>(result);
            sdm.smodel = pmodel;
        }
    }



}

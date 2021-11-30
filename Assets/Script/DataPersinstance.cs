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
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            StartCoroutine(loadJson("https://localhost/gameJam/shopData.json", "shop"));
            return null;  
        }
        else
        {
             json = File.ReadAllText(Application.dataPath + "/" + jsonload);
        }
        ShopMode pmodel = JsonUtility.FromJson<ShopMode>(json);
        return pmodel;
    }

    public SkillPhase LoadSkillPhase(string jsonload)
    {
        string json;
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            StartCoroutine(loadJson("https://localhost/gameJam/skillPhase.json", "skill"));
            return null;
        }
        else
        {
            json = File.ReadAllText(Application.dataPath + "/" + jsonload);
        }
        SkillPhase pmodel = JsonUtility.FromJson<SkillPhase>(json);
        return pmodel;
    }

    public LevelModel LoadLevelData(string jsonload)
    {
        string json;
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            StartCoroutine(loadJson("https://github.com/hemite123/buggame/tree/main/Assets/levelData.json", "level"));
            return null;
        }
        else
        {
            json = File.ReadAllText(Application.dataPath + "/" + jsonload);
        }
        LevelModel pmodel = JsonUtility.FromJson<LevelModel>(json);
        return pmodel;
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

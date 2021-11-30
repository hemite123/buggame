using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

public class SaveDataManagement : MonoBehaviour
{
    string pathplayer;
    string file = "pData.json";
    string shopfile = "shopData.json";
    string levelfile = "levelData.json";
    public PlayerModel mdata;
    public ShopMode smodel;
    public SkillPhase sphase;
    [SerializeField]
    public LevelModel lmodel;
    public static SaveDataManagement instance;
    DataPersinstance dpres;
    MainMenu main;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance);
            return;
        }
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        pathplayer = Application.persistentDataPath + "/" + file;
        dpres = GetComponent<DataPersinstance>();
        if(Application.platform == RuntimePlatform.WebGLPlayer)
        {
            Debug.Log("x");
            dpres.LoadShop(shopfile);
            dpres.LoadSkillPhase("skillPhase.json");
            dpres.LoadLevelData(levelfile);
        }
        else
        {
            smodel = dpres.LoadShop(shopfile);
            sphase = dpres.LoadSkillPhase("skillPhase.json");
            lmodel = dpres.LoadLevelData(levelfile);
        }
       
        main = MainMenu.instance;
        DontDestroyOnLoad(this);
    }

    private void Update()
    {
        if (!File.Exists(pathplayer))
        {
            PlayerModel pmodel = new PlayerModel();
            pmodel.bugcoin = "0";
            pmodel.sname.Add(new SkillName("lclick", 0));
            pmodel.sname.Add(new SkillName("eclick", 0));
            pmodel.sname.Add(new SkillName("clap", 0));
            pmodel.sname.Add(new SkillName("caskill", 0));
            dpres.SaveToJson(pmodel);
            mdata = pmodel;
            foreach (SkillUpgrade sup in main.supgrd)
            {
                sup.phase = 0;
            }
            main.bugCoin = "0";
        }
        else if (mdata == null)
        {
            mdata = dpres.LoadFromJson(file);
            foreach (SkillName sup in mdata.sname)
            {
                var skillup = main.supgrd.Single(x => x.skillname == sup.skillName);
                skillup.phase = sup.phase;
            }
            main.bugCoin = mdata.bugcoin;
        }
        else
        {
            foreach (SkillUpgrade sup in main.supgrd)
            {
                var skillup = mdata.sname.Single(x => x.skillName == sup.skillname);
                skillup.phase = sup.phase;
            }
            dpres.SaveToJson(mdata);
        }
    }
}

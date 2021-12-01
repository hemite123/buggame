using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{
    [SerializeField]
    public string bugCoin;
    [SerializeField]
    public static MainMenu instance;
    public List<SkillUpgrade> supgrd = new List<SkillUpgrade>();
    public List<LevelUpdate> lupdate = new List<LevelUpdate>();
    public TMPro.TextMeshProUGUI coinUI;
    SaveDataManagement sdm;
    public GameObject uiSUpdate;
    public GameObject uiLevel;
    public Sprite starActive;
    public GameObject uiCredit;
    public GameObject optionUi;
    public Slider musicSlide;
    public AudioSource source;
    public Text emessage;
    public GameObject tutorialUi;



    private void Awake()
    {
        if(instance != null)
        {
            return;
        }
        instance = this;
    }
    // Start is called before the first frame update
    private void Start()
    {
        sdm = SaveDataManagement.instance;
        if (PlayerPrefs.HasKey("audio"))
        {
            source.volume = PlayerPrefs.GetFloat("audio");
        }
        else
        {
            PlayerPrefs.SetFloat("audio",0.5f);
        }
        musicSlide.value = source.volume;
    }

    // Update is called once per frame
    void Update()
    {
        coinUI.text = bugCoin;
        foreach(SkillUpgrade sup in supgrd)
        {
            sup.skillSlider.value = (float)sup.phase / 5;
        }
        foreach(LevelUpdate lupdate in lupdate)
        {
            var x = sdm.mdata.lmanager.SingleOrDefault(c => c.levelname == lupdate.levelname);
            if(x != null)
            {
                for(int i = 0; i < x.star; i++)
                {
                    lupdate.stars[i].GetComponent<Image>().sprite = starActive;
                }
            }
        }
        source.volume = musicSlide.value;
        PlayerPrefs.SetFloat("audio", musicSlide.value);

       
    }


    IEnumerator closeText()
    {
        yield return new WaitForSeconds(2);
        emessage.gameObject.SetActive(false);
    }

    public void OptionMan(bool cond)
    {
        optionUi.SetActive(cond);
    }

    public void ShopCheck(Button btn)
    {
        var sup = supgrd.Single(x => x.buyBtn == btn);
        var slist = sdm.smodel.shoplist.Single(x => x.skillName == sup.skillname);
        decimal currentCurrency = decimal.Parse(bugCoin);
        if (sup.phase < slist.money.Length)
        {
            if (currentCurrency >= slist.money[sup.phase])
            {
                sup.phase += 1;
                currentCurrency -= slist.money[sup.phase];
                bugCoin = currentCurrency.ToString();
                emessage.color = new Color(0, 255, 0);
                emessage.text = "Level Upgraded";
                emessage.gameObject.SetActive(true);
                StartCoroutine(closeText());
            }
            else
            {
                emessage.color = new Color(255, 0, 0);
                emessage.text = "Not Enough BugCoin";
                emessage.gameObject.SetActive(true);
                StartCoroutine(closeText());
            }
        }
        else
        {
            emessage.color = new Color(0, 255, 0);
            emessage.text = "Max Level Upgraded";
            emessage.gameObject.SetActive(true);
            StartCoroutine(closeText());
        }
        

    }

    public void tutorialUiManagement(bool cond)
    {
        tutorialUi.SetActive(cond);
    }

    public void StartGame(GameObject btn)
    {
        var checklevel = lupdate.Single(x => x.button == btn);
        var checkleveldata = sdm.lmodel.levelList.Single(x => x.lname == checklevel.levelname);
        SceneManager.LoadSceneAsync(checkleveldata.scenename, LoadSceneMode.Additive).completed += (res) =>
        {
            if (res.isDone)
            {
                Gamemanager gm = Gamemanager.instace;
                gm.sdm = sdm;
                gm.levelModel = checkleveldata;
                gm.blackTransition.SetActive(true);
                gm.isStart = true;
                SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
            }
        };
    }

    public void UICred(bool cond)
    {
        uiCredit.SetActive(cond);
    }

    public void SUpdateUI(bool cond)
    {
        uiSUpdate.SetActive(cond);
    }

    public void LevelUi(bool cond)
    {
        uiLevel.SetActive(cond);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    
}

[System.Serializable]
public class SkillUpgrade
{
    public string skillname;
    public Slider skillSlider;
    public Button buyBtn;
    public int phase;
}

[System.Serializable]
public class LevelUpdate
{
    public string levelname;
    public GameObject[] stars;
    public GameObject button;
}

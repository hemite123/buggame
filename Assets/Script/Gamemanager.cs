using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System;
using UnityEngine.Playables;
using UnityEngine.UI;
using System.Globalization;
using UnityEngine.SceneManagement;

[System.Serializable]
public class Gamemanager : MonoBehaviour
{

    PlayerMovement pmove;
    public GameObject Transition;
    public static Gamemanager instace;
    [SerializeField]
    List<WarpTo> WarpManagement = new List<WarpTo>();
    public float timelimit;
    public bool timeStart;
    public GameObject target;
    public GameObject prefab;
    public TMPro.TMP_Text timelimitui;
    public bool isSkillExe;
    public GameObject timeUi;
    public bool pauseTime;
    public bool cooldownLeftClick;
    public bool leftclick;
    public float longTimeActive;
    public float longTimeSpawn;
    public GameObject uiGroup;
    public bool DeveloperMode;
    public float longLeftClickCD;
    [SerializeField]
    List<Timelineplay> SkillTimeline = new List<Timelineplay>();
    [SerializeField]
    List<ObjectiveNeed> objectiveManagement = new List<ObjectiveNeed>();
    [SerializeField]
    List<Skill> skillManagement = new List<Skill>();
    [SerializeField]
    List<ObjectiveArcade> oActive = new List<ObjectiveArcade>();
    [SerializeField]
    public Slider[] starSlide;
    public Image imageSkill;
    public Sprite skillChangeOn;
    public Sprite skillChangeOff;
    public SaveDataManagement sdm;
    [SerializeField]
    public bool isDone;
    [HideInInspector]
    public lmodel levelModel;
    public GameObject uiWin;
    public bool isUiPlay;
    public GameObject spiralTran;
    Animator anim;
    public GameObject uiGroupArcade;
    public GameObject uiGroupArcade2;
    public GameObject uiLookat;
    public GameObject collisionArcade;
    public bool isBonk;
    public GameObject powerobject;
    public Material offMat;
    public Material onMat;
    public ParticleSystem pcParticle;
    public GameObject brokenRam;
    public GameObject brokenHarddrive;
    public AudioClip ingame;
    public AudioClip transition;
    public AudioClip arcade;
    public AudioSource source;
    public TMPro.TextMeshProUGUI textCoinadd;
    public GameObject loseUi;
    public Slider health;
    public GameObject blackTransition;
    public bool isStart;
    Vector3 firstposition;
    public GameObject pauseUi;
    public bool isPause;
    public Slider volumeSlider;
    public bool isLose;
    public float volume = 1;

    public void Awake()
    {
        if (instace != null)
        {
            return;
        }
        instace = this;
    }


    public IEnumerator waitForTranAnimDone(string animtype, WarpTo wp,string actions,int lenghtstar)
    {
        source.Stop();
        source.loop = false;
        Transition.SetActive(true);
        spiralTran.SetActive(true);
        if (animtype == "slide")
        {
            anim = Transition.GetComponent<Animator>(); 
            anim.SetBool("sliding", true);
        }else if(animtype == "spiral")
        {
            anim = spiralTran.GetComponent<Animator>();
            anim.SetBool("isTran", true);
        }
        source.clip = transition;
        source.Play();
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        if (wp != null)
        {
            pmove.transform.position = wp.Point.transform.position;
        }
        if (actions == "finish")
        {
            yield return new WaitForSeconds(2);
            pmove.camMan.ChangeCam("finish");
            
            
        }else if(actions == "arcade")
        {
            yield return new WaitForSeconds(2);
            ArcadeScript ascript = ArcadeScript.instance;
            ascript.SetRB();
            if (ascript.arcadeDodge)
            {
                pmove.camMan.ChangeCam("arcadecam");
                uiGroupArcade.SetActive(true);
            }else if (ascript.arcadeShoot)
            {
                pmove.camMan.ChangeCam("arcadecam2");
                uiGroupArcade2.SetActive(true);
            }
            
            uiGroup.SetActive(false);       
            
        }else if(actions == "arcadedone")
        {
            yield return new WaitForSeconds(2);
            pmove.camMan.ChangeCam("cam1");
            
            
        }
        if (animtype == "slide")
        {
            yield return new WaitForSeconds(2);
            anim.SetBool("sliding", false);
        }
        else if (animtype == "spiral")
        {
            yield return new WaitForSeconds(2);
            anim.SetBool("isTran", false);
        }
        source.Play();

        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        if(actions == "finish")
        {
            uiGroup.SetActive(false);
            uiGroupArcade.SetActive(false);
            uiGroupArcade2.SetActive(false);
            pcParticle.Play();
            Transition.SetActive(false);
            pmove.getCC().enabled = false;
            StartCoroutine(fillStart(lenghtstar));
            source.clip = ingame;
            source.loop = true;
            source.Play();
        }
        else if(actions == "arcade")
        {
            Transition.SetActive(false);
            spiralTran.SetActive(false);
            ArcadeScript ascript = ArcadeScript.instance;
            pmove.getCC().enabled = false;
            ascript.isStart = true;
            StartCoroutine(ascript.readyGo());
        }
        else
        {
            timeStart = true;
            pmove._cantFly = true;
            Transition.SetActive(false);
            uiGroup.SetActive(true);
            uiGroupArcade.SetActive(false);
            uiGroupArcade2.SetActive(false);
            pmove.playerLight.enabled = true;
            isUiPlay = false;
            spiralTran.SetActive(false);
            pmove.getCC().enabled = true;
            source.clip = ingame;
            source.loop = true;
            source.Play();
        }
       
    }

    IEnumerator resetLeftClick()
    {
        cooldownLeftClick = true;
        yield return new WaitForSeconds(longLeftClickCD);
        cooldownLeftClick = false;
        leftclick = false;
    }

    IEnumerator fillStart(int length)
    {
        yield return new WaitForSeconds(4f);
        uiWin.SetActive(true);
        int bugcoin = UnityEngine.Random.Range(10000, 100000);
        for (int i = 0; i < bugcoin; i++)
        {
            textCoinadd.text = i.ToString();
        }
        decimal currentCurrency = decimal.Parse(sdm.mdata.bugcoin);
        currentCurrency += bugcoin;
        sdm.mdata.bugcoin = currentCurrency.ToString();
        for (int i = 0;i < length; i++)
        {
            while(starSlide[i].value < 1)
            {
                starSlide[i].value += 0.01f;
                yield return null;
            }
        }
        
    }

    public IEnumerator resetTimeActive()
    {
        yield return new WaitForSeconds(longTimeActive);
        pauseTime = false;
    }

    public IEnumerator waitfor(float second)
    {
        yield return new WaitForSeconds(second);
    }

    private void Start()
    {
        pmove = PlayerMovement.instance;
        timelimit = levelModel.starttime;
        firstposition = pmove.transform.position;
        Objective[] obj = FindObjectsOfType(typeof(Objective)) as Objective[];
        volume = PlayerPrefs.GetFloat("audio");
        volumeSlider.value = volume;
        source.volume = volume;
        foreach (Objective obje in obj)
        {
            objectiveManagement.Add(new ObjectiveNeed(obje.gameObject,false));
        }
        foreach(ObjectiveNeed obned in objectiveManagement)
        {
            Objective ob = obned.objective.GetComponent<Objective>();
            if (!obned.isDone && ob.isOn)
            {
                obned.objective.transform.Find("Outline").gameObject.SetActive(true);
            }
            else if(!ob.isOn)
            {
                obned.isDone = true;
            }
        }
        if (!DeveloperMode)
        {
            foreach(SkillName pm in sdm.mdata.sname)
            {
                if(pm.skillName == "lclick")
                {
                    var cdcheck = sdm.sphase.skillphase.Single(x => x.skillName == pm.skillName);
                    if(pm.phase == 0)
                    {
                        longLeftClickCD = cdcheck.coolDown[pm.phase];
                    }
                    else
                    {
                        longLeftClickCD = cdcheck.coolDown[pm.phase - 1];
                    }
                   
                }else if(pm.skillName == "eclick")
                {
                    var cdcheck = sdm.sphase.skillphase.Single(x => x.skillName == pm.skillName);
                    var skillcheck = skillManagement.Single(x => x.skillname == cdcheck.skillName);
                    if (pm.phase == 0)
                    {
                        skillcheck.skillCooldown = cdcheck.coolDown[pm.phase];
                    }
                    else
                    {
                        skillcheck.skillCooldown = cdcheck.coolDown[pm.phase - 1];
                    }
                }else if(pm.skillName == "caskill")
                {
                    var cdcheck = sdm.sphase.skillphase.Single(x => x.skillName == pm.skillName);
                    if (pm.phase == 0)
                    {
                        longTimeActive = cdcheck.coolDown[pm.phase];
                    }
                    else
                    {
                        longTimeActive = cdcheck.coolDown[pm.phase - 1];
                    }
                }
                else if (pm.skillName == "clap")
                {
                    var cdcheck = sdm.sphase.skillphase.Single(x => x.skillName == pm.skillName);
                    if (pm.phase == 0)
                    {
                        longTimeSpawn = cdcheck.coolDown[pm.phase];
                    }
                    else
                    {
                        longTimeSpawn = cdcheck.coolDown[pm.phase - 1];
                    }
                }
            }
        }
        
    }

    private void LateUpdate()
    {

        if (Input.GetKeyDown(KeyCode.Escape) && !isPause)
        {
            isPause = true;
        }else if(Input.GetKeyDown(KeyCode.Escape) && isPause)
        {
            isPause = false;
        }

        if (isPause)
        {
            Time.timeScale = 0;
            pauseUi.SetActive(true);
            pmove.isStun = true;
            volume = volumeSlider.value;
            source.volume = volume;
        }
        else if(!isPause && !isLose)
        {
            Time.timeScale = 1;
            pauseUi.SetActive(false);
            pmove.isStun = false;
        }
    }

    private void Update()
    {
        if (isStart && !isPause)
        {

            if (timeStart && !isDone && !isUiPlay)
            {
                timeUi.SetActive(true);
                if (!pauseTime)
                {
                    timelimit -= Time.deltaTime;
                    string minute = Mathf.Floor(timelimit / 60).ToString("00");
                    string second = (timelimit % 60).ToString("00");
                    timelimitui.text = minute + ":" + second;
                    powerobject.GetComponent<MeshRenderer>().material = onMat;
                }
                else
                {
                    powerobject.GetComponent<MeshRenderer>().material = offMat;
                }


            }
            else
            {
                timeUi.SetActive(false);
            }
            if (Input.GetKeyDown(KeyCode.E) && !pmove.isStun && !isUiPlay)
            {
                if (isSkillExe)
                {
                    TargetCheck("skill");
                }
                else if (collisionArcade != null)
                {
                    TargetCheck("arcade");
                }

                //ExecuteSkill
            }
            if (Input.GetMouseButtonDown(0) && !pmove.isStun && !cooldownLeftClick && !leftclick && !isUiPlay && pmove._cantFly)
            {
                pmove.getAnim().SetBool("isAttack", true);
                leftclick = true;
                StartCoroutine(resetLeftClick());
            }
            else
            {
                pmove.getAnim().SetBool("isAttack", false);
            }


            if (objectiveManagement.Count == 0 && !DeveloperMode)
            {
                if (!isDone)
                {
                    isDone = true;
                    PWin();
                }
            }
            else if (!DeveloperMode)
            {
                if (retIsDone())
                {
                    Debug.Log("Objective Complate");
                    if (!isDone)
                    {
                        isDone = true;
                        PWin();
                    }
                }
                else if (timelimitui.text == "00:00")
                {
                    pmove.isStun = true;
                    PLose();
                }
            }

            //UI Management
            if (isSkillExe)
            {
                imageSkill.sprite = skillChangeOn;
            }
            else
            {
                imageSkill.sprite = skillChangeOff;
            }

            //skill cd
            foreach (Skill skillm in skillManagement)
            {
                if (skillm.skillname == "eclick")
                {
                    if (skillm.curCooldown > 0)
                    {
                        skillm.curCooldown -= 0.001f;
                        skillm.skillUI.GetComponent<Image>().fillAmount += 0.001f / skillm.skillCooldown;
                    }
                }
            }

            foreach (ObjectiveNeed obned in objectiveManagement)
            {
                if (obned.isDone)
                {
                    obned.objective.transform.Find("Outline").gameObject.SetActive(false);
                }
                else
                {
                    obned.objective.transform.Find("Outline").gameObject.SetActive(true);
                }
            }
        }
    }

    public void PLose()
    {
        isLose = true;
        timeStart = false;
        pmove.isStun = true;
        uiGroup.SetActive(false);
        uiGroupArcade.SetActive(false);
        uiGroupArcade2.SetActive(false);
        isBonk = false;
        pmove.camMan.ChangeCam("CamAnimation");
        loseUi.SetActive(true);
    }

    public void CloseMenu(bool cond)
    {
        pauseUi.SetActive(cond);
    }

    public void PWin()
    {
        timeStart = false;
        isBonk = false;
        for(int i = levelModel.timeStar.Length; i > 0; i--)
        {
            if (timelimit >= levelModel.timeStar[i - 1])
            {
                var lmsame = sdm.mdata.lmanager.SingleOrDefault(x => x.levelname == levelModel.lname);
                if(lmsame == null)
                {
                    LevelManager lmd = new LevelManager(levelModel.lname, i);
                    sdm.mdata.lmanager.Add(lmd);
                    StartCoroutine(waitForTranAnimDone("slide", null, "finish", i));
                    return;
                }
                else
                {
                    lmsame.star = i;
                    StartCoroutine(waitForTranAnimDone("slide", null, "finish", i));
                    return;
                }
                
            }
        }
        var lmsamec = sdm.mdata.lmanager.SingleOrDefault(x => x.levelname == levelModel.lname);
        if (lmsamec == null)
        {
            LevelManager lmd = new LevelManager(levelModel.lname, 0);
            sdm.mdata.lmanager.Add(lmd);
            return;
        }
        else
        {
            lmsamec.star = 0;
            return;
        }
    }

    public void TargetCheck(string action)
    {
        if(action == "skill")
        {
            pmove.isSkill = true;
            foreach (Skill skilm in skillManagement)
            {
                if (skilm.skillname == "eclick")
                {
                    if (skilm.curCooldown > 0)
                    {
                        return;
                    }
                    skilm.curCooldown = skilm.skillCooldown;
                    skilm.skillUI.GetComponent<Image>().fillAmount = 0;
                }
            }
            foreach (Timelineplay tm in SkillTimeline)
            {
                if (tm.skillOrigin == pmove.targetskill)
                {
                    pmove.director.playableAsset = tm.asset;
                    target = tm.target;
                    prefab = tm.shootProject;
                    pmove.director.Play();
                }
            }
        }else if(action == "arcade")
        {
            isUiPlay = true;
            SkillArcadeCheck("enter");
        }
        
    }

    public void Warping(GameObject origin)
    {
        var warppos = WarpManagement.Single(x => x.Origin == origin);
        isBonk = false;
        pmove.getCC().enabled = false;
        StartCoroutine(waitForTranAnimDone("slide",warppos,"",0));
    }

    public void ResetGame()
    {
        isLose = false;
        blackTransition.SetActive(true);
        pmove.isStun = false;
        pmove.transform.position = firstposition;
        uiGroup.SetActive(true);
        loseUi.SetActive(false);
        isBonk = true;
        timelimit = levelModel.starttime;
        StartCoroutine(waitfor(2));
        foreach(ObjectiveNeed obn in objectiveManagement)
        {
            obn.isDone = false;
        }
        blackTransition.SetActive(false);
    }
    
    public void SetCantFly(bool cond)
    {
        pmove._cantFly = cond;
    }

    public void ContinueNextLevel()
    {
        int checkleveldata = sdm.lmodel.levelList.FindIndex(x => x.lname == levelModel.lname);
        if (sdm.lmodel.levelList[checkleveldata + 1] != null)
        {
            levelModel = sdm.lmodel.levelList[checkleveldata + 1];
            SceneManager.LoadSceneAsync(levelModel.scenename, LoadSceneMode.Additive).completed += (res) =>
            {
                if (res.isDone)
                {
                    SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
                     
                }
            };
        }
        else
        {
            //TO Main Menu And Coming Soon
        }

        
    }

    public void Continue()
    {
        SceneManager.LoadSceneAsync("MainMenu", LoadSceneMode.Additive).completed += (res) =>
        {
            if (res.isDone)
            {
                SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
            }
        };
    }

    public void Shoot()
    {
        GameObject bullet = ObjectPool.instance.GetPooledObject();
        if(bullet != null)
        {
            bullet.transform.position = pmove.shootOrigin.transform.position;
            bullet.transform.rotation = pmove.shootOrigin.transform.rotation;
            bullet.SetActive(true);
        }
        Vector3 dir = target.transform.position - bullet.transform.position;
        bullet.GetComponent<Rigidbody>().velocity = dir * 10f;
    }

    public void SetTimeStart(bool cond)
    {
        timeStart = cond;
    }

    public void SetIsSkill(bool cond)
    {
        pmove.isSkill = cond;
    }

    public void ActiveUiGroup(bool cond)
    {
        uiGroup.SetActive(cond);
    }

    public bool retIsDone()
    {
        foreach(ObjectiveNeed increment in objectiveManagement)
        {
            if (!increment.isDone)
            {
                return false;
            }
        }
        return true;
    }

    public void SkillArcadeCheck(string action)
    {
        var oas = oActive.SingleOrDefault(x => x.colider == collisionArcade);
        if(oas != null)
        {
            if (!oas.isDone)
            {
                if(action == "" && oas.objective.GetComponent<Objective>().isOn)
                {
                    uiLookat = oas.uiText;
                    uiLookat.SetActive(true);
                }else if(action == "enter")
                {
                    uiLookat.SetActive(false);
                    uiLookat = null;
                    ArcadeScript ascript = ArcadeScript.instance;
                    ascript.conditionwin = oas.conditionwin;
                    ascript.playerhealth = oas.health;
                    ascript.objective = oas.objective;
                    ascript.arcadeDodge = oas.GameModeDodge;
                    ascript.arcadeShoot = oas.GameModeShoot;
                    ascript.timerPenalty = oas.timePenality;
                    StartCoroutine(waitForTranAnimDone("spiral", null, "arcade", 0));
                }
                
            }
        }
    }

    public void CooldownBonk()
    {
        BonkPlayer bm = BonkPlayer.instance;
        StartCoroutine(bm.startfornextgambling());
    }

    public void ObjectiveSetter(GameObject objecttoset,bool cond)
    {
        foreach(ObjectiveNeed obneed in objectiveManagement)
        {
            if(obneed.objective == objecttoset)
            {
                obneed.isDone = cond;
                if(obneed.objective.GetComponent<Objective>().toolname == "cpu" || obneed.objective.GetComponent<Objective>().toolname == "pin" || obneed.objective.GetComponent<Objective>().toolname == "cable")
                {
                    obneed.objective.GetComponent<ParticleSystem>().Play();
                }else if(obneed.objective.GetComponent<Objective>().toolname == "ram")
                {
                    obneed.objective.SetActive(false);
                    brokenRam.SetActive(true);
                }else if(obneed.objective.GetComponent<Objective>().toolname == "harddisk")
                {
                    obneed.objective.SetActive(false);
                    brokenHarddrive.SetActive(true);
                }
            }
        }
        foreach (ObjectiveArcade oarc in oActive)
        {
            if (oarc.objective == objecttoset)
            {
                oarc.isDone = cond;
            }
        }
    }
}
[Serializable]
public class WarpTo
{
    public GameObject Origin;
    public GameObject Point;
}

[Serializable]
public class Timelineplay
{
    public GameObject skillOrigin;
    public PlayableAsset asset;
    public GameObject target;
    public GameObject shootProject;
}

[Serializable]
public class ObjectiveNeed
{
    public GameObject objective;
    public bool isDone;

    public ObjectiveNeed (GameObject x , bool y)
    {
        this.objective = x;
        this.isDone = y;
    }
}

[Serializable]
public class Skill
{
    public String skillname;
    public float curCooldown;
    public float skillCooldown;
    public GameObject skillUI;
}

[Serializable]
public class ObjectiveArcade
{
    public GameObject objective;
    public GameObject colider;
    public float timePenality;
    public int conditionwin;
    public int health;
    public bool isDone;
    public GameObject uiText;
    public bool GameModeDodge;
    public bool GameModeShoot;
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class BonkPlayer : MonoBehaviour
{
    public GameObject handObject;
    public static BonkPlayer instance;
    public GameObject player;
    public float Chance;
    public float ChanceClap;
    public GameObject canvasWorld;
    Gamemanager gameman;
    public bool gambling;
    public GameObject targetUI;
    public GameObject[] playerTarget;
    bool waiting;
    [SerializeField]
    public Dictionary<GameObject,GameObject> tempTarget = new Dictionary<GameObject,GameObject>();

    private void Awake()
    {
        if(instance != null)
        {
            return;
        }
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        gameman = Gamemanager.instace;
        waiting = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!gambling && waiting && gameman.isBonk && gameman.isStart && !gameman.isPause)
        {
            if(Chance > 0)
            {
                if(tempTarget.Count == 0)
                {
                    if(Random.value < Chance)
                    {
                        gambling = true;
                        targetUI.GetComponent<RectTransform>().rotation = Quaternion.Euler(new Vector3(-90, 0, 0));
                        GameObject instante = Instantiate(targetUI, canvasWorld.transform);
                        instante.transform.localScale = new Vector3(0, 0, 0);
                        tempTarget.Add(playerTarget.First(pred => pred.name == "bottom"), instante);
                        StartCoroutine(Spawn());
                    }
                }
            }
            if (ChanceClap > 0)
            {
                if (tempTarget.Count == 0)
                {
                    if (Random.value < ChanceClap)
                    {
                        targetUI.GetComponent<RectTransform>().rotation = Quaternion.Euler(new Vector3(-180, 0, 0));
                        GameObject instante = Instantiate(targetUI, canvasWorld.transform);
                        GameObject instante1 = Instantiate(targetUI, canvasWorld.transform);
                        instante.transform.localScale = new Vector3(0, 0, 0);
                        instante1.transform.localScale = new Vector3(0, 0, 0);
                        tempTarget.Add(playerTarget.First(pred => pred.name == "left"), instante);
                        tempTarget.Add(playerTarget.First(pred => pred.name == "right"), instante1);
                        StartCoroutine(Spawn());
                    }
                }
            }
        }
        
        if (tempTarget.Count > 0)
        {
            foreach(KeyValuePair<GameObject,GameObject> dis in tempTarget)
            {
                dis.Value.transform.position = dis.Key.transform.position;
            }
        }
    }

    IEnumerator Spawn()
    {
        waiting = false;
        foreach (KeyValuePair<GameObject, GameObject> dis in tempTarget)
        {
            StartCoroutine(scalingAnimator(targetUI.transform.localScale, dis.Value));
            yield return null;
        }
        yield return new WaitForSeconds(4f);
        foreach (KeyValuePair<GameObject, GameObject> dis in tempTarget)
        {
            GameObject instatiate = Instantiate(handObject, dis.Key.transform.position, dis.Key.transform.rotation);
            if(dis.Key.name == "right")
            {
                instatiate.transform.position += new Vector3(-10f,0f, 0f);
                instatiate.transform.rotation = Quaternion.LookRotation(instatiate.transform.position - player.transform.position);
                instatiate.transform.LookAt(player.transform);
                instatiate.GetComponent<Rigidbody>().useGravity = false;
                StartCoroutine(addToPos(instatiate));
            }
            else if(dis.Key.name == "left")
            {
                instatiate.transform.position += new Vector3(10f,0f, 0f);
                instatiate.GetComponent<Rigidbody>().useGravity = false;
                instatiate.transform.rotation = Quaternion.LookRotation(instatiate.transform.position - player.transform.position);
                instatiate.transform.LookAt(player.transform);
                StartCoroutine(addToPos(instatiate));
            }
            else if(dis.Key.name == "bottom")
            {
                instatiate.transform.position += new Vector3(0f, 8f,0f);
                instatiate.transform.rotation = Quaternion.LookRotation(instatiate.transform.position -  player.transform.position);
                instatiate.transform.LookAt(player.transform);
            }
            //instatiate.GetComponent<Rigidbody>().velocity = new Vector3(0, 10, 0);
            Destroy(dis.Value);
            yield return null;
        }
        tempTarget.Clear();
        

    }

    public IEnumerator startfornextgambling()
    {
        yield return new WaitForSeconds(gameman.longTimeSpawn);
        gambling = false;
        waiting = true;
    }

    IEnumerator scalingAnimator(Vector3 endPoint, GameObject objects)
    {
        while (objects.transform.localScale != endPoint && objects != null)
        {
            objects.transform.localScale = Vector3.Lerp(objects.transform.localScale, endPoint, Time.deltaTime);
            yield return null;
        }
       
    }

    IEnumerator addToPos(GameObject objects)
    {
        while(objects.transform.position != player.transform.position)
        {
            if(objects.transform.position.x < player.transform.position.x)
            {
                objects.transform.position += new Vector3(0.1f,0f,0f);
            }else if(objects.transform.position.x > player.transform.position.x)
            {
                objects.transform.position -= new Vector3(0.1f, 0f, 0f);
            }
            yield return null;
        }
    }
}

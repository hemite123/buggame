using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcadeScript : MonoBehaviour
{
    public GameObject player;
    public GameObject playerArcadeShoot;
    public GameObject shotPrefab;
    public BoxCollider2D spawningenemy;
    public BoxCollider2D spawningenemyArcadeShoot;
    public GameObject shootPoint;
    public GameObject enemy;
    public List<GameObject> alreadyspawn = new List<GameObject>();
    public bool isStart;
    public int playerhealth;
    public TMPro.TextMeshProUGUI textui;
    public TMPro.TextMeshProUGUI textuishoot;
    public static ArcadeScript instance;
    public float speedToPlayer;
    public float timerPenalty;
    public GameObject objective;
    Gamemanager gm;
    public int curentcond;
    public int conditionwin;
    bool startSpawn;
    public bool execute;
    public TMPro.TextMeshProUGUI readygo;
    public TMPro.TextMeshProUGUI readygoShoot;
    public TMPro.TextMeshProUGUI needed;
    public TMPro.TextMeshProUGUI neededShoot;
    public Vector3 positionreset;
    public Vector3 positionResetShoot;
    public GameObject destroyObject;
    public bool arcadeDodge;
    public bool arcadeShoot;
    public bool infinity;
    Rigidbody2D rb;

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
        
        gm = Gamemanager.instace;
        positionResetShoot = playerArcadeShoot.transform.localPosition;
        positionreset = player.transform.localPosition;
    }

    

    // Update is called once per frame
    void Update()
    {
        if (execute)
        {
            if (arcadeDodge)
            {
                rb.gravityScale = 1;
            }
            else
            {
                rb.gravityScale = 0;
            }
            if (arcadeDodge)
            {
                textui.text = playerhealth.ToString();
                needed.text = curentcond.ToString() + "/" + conditionwin.ToString();
            }
            else if (arcadeShoot)
            {
                textuishoot.text = playerhealth.ToString();
                neededShoot.text = curentcond.ToString() + "/" + conditionwin.ToString();
            }
            
           
            if (isStart)
            {
                var movement = Input.GetAxis("Vertical");
                var movementx = Input.GetAxis("Horizontal");
                if (arcadeDodge)
                {
                    rb.velocity += new Vector2(0, movement) * Time.deltaTime * 20f;
                    if (!startSpawn)
                    {

                        Vector3 randompos = new Vector3(Random.Range(-spawningenemy.bounds.extents.x / 2, spawningenemy.bounds.extents.x / 2), Random.Range(-spawningenemy.bounds.extents.y / 2, spawningenemy.bounds.extents.y / 2), 0f);
                        Vector3 pos = spawningenemy.transform.position + randompos;
                        GameObject objectspawn = Instantiate(enemy, pos, Quaternion.identity);
                        objectspawn.transform.SetParent(spawningenemy.transform);
                        objectspawn.GetComponent<Rigidbody2D>().AddRelativeForce((destroyObject.transform.position - objectspawn.transform.position) * speedToPlayer, ForceMode2D.Impulse);
                        startSpawn = true;
                        StartCoroutine(resetSpawn());

                    }
                }
                else if (arcadeShoot)
                {
                    rb.velocity += new Vector2(movementx, 0) * Time.deltaTime * 20f;
                    if (!startSpawn)
                    {
                        Vector3 randompos = new Vector3(Random.Range(-spawningenemyArcadeShoot.bounds.extents.x / 2, spawningenemyArcadeShoot.bounds.extents.x / 2), Random.Range(-spawningenemyArcadeShoot.bounds.extents.y / 2, spawningenemyArcadeShoot.bounds.extents.y / 2), 0f);
                        Vector3 pos = spawningenemyArcadeShoot.transform.position + randompos;
                        GameObject objectspawn = Instantiate(enemy,pos, Quaternion.identity);
                        objectspawn.transform.SetParent(spawningenemyArcadeShoot.transform);
                        objectspawn.GetComponent<Rigidbody2D>().AddRelativeForce((playerArcadeShoot.transform.position - objectspawn.transform.position) * speedToPlayer, ForceMode2D.Impulse);
                        alreadyspawn.Add(objectspawn);
                        startSpawn = true;
                        StartCoroutine(resetSpawn());

                    }
                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        GameObject spawnProjectile = Instantiate(shotPrefab, shootPoint.transform.position, Quaternion.identity);
                        spawnProjectile.GetComponent<Rigidbody2D>().gravityScale = 0;
                        spawnProjectile.GetComponent<Rigidbody2D>().velocity = new Vector2(0,10f);
                    }
                }
                
               
            }
           

            if (curentcond >= conditionwin && !infinity)
            {
                rb.gravityScale = 0;
                execute = false;
                isStart = false;
                gm.ObjectiveSetter(objective, true);
                curentcond = 0;
                if (arcadeShoot)
                {
                    readygoShoot.transform.gameObject.SetActive(true);
                    readygoShoot.text = "Ready";
                    playerArcadeShoot.transform.localPosition = positionResetShoot;
                }
                else if (arcadeDodge)
                {
                    readygo.transform.gameObject.SetActive(true);
                    readygo.text = "Ready";
                    player.transform.localPosition = positionreset;
                }
                StartCoroutine(gm.waitForTranAnimDone("spiral", null, "arcadedone", 0));
                foreach (GameObject go in alreadyspawn)
                {
                    DestroyGo(go);
                }
                
                
                //objectivedone
            }
            else if (playerhealth < 1 && !infinity)
            {
                //minus the timer
                execute = false;
                isStart = false;
                gm.timelimit -= timerPenalty;
                rb.gravityScale = 0;
                curentcond = 0;
                if (arcadeShoot)
                {
                    readygoShoot.transform.gameObject.SetActive(true);
                    readygoShoot.text = "Ready";
                    playerArcadeShoot.transform.localPosition = positionreset;
                }
                else if (arcadeDodge)
                {
                    readygo.transform.gameObject.SetActive(true);
                    readygo.text = "Ready";
                    player.transform.localPosition = positionreset;
                }
                
                StartCoroutine(gm.waitForTranAnimDone("spiral", null, "arcadedone", 0));
            }
        }
        else
        {
            player.transform.localPosition = positionreset;
            playerArcadeShoot.transform.localPosition = positionResetShoot;
        }
        
        
    }

    public IEnumerator readyGo()
    {
        yield return new WaitForSeconds(2);
        if (arcadeDodge)
        {
            readygo.text = "GO";
        }else if (arcadeShoot)
        {
            readygoShoot.text = "GO";
        }
        
        yield return new WaitForSeconds(2);
        gm.source.clip = gm.arcade;
        gm.source.loop = true;
        gm.source.Play();
        if (arcadeDodge)
        {
            readygo.transform.gameObject.SetActive(false);
        }
        else if (arcadeShoot)
        {
            readygoShoot.transform.gameObject.SetActive(false);
        }
        rb.isKinematic = false;
        execute = true;
    }

    public void DestroyGo(GameObject objects)
    {
        alreadyspawn.Remove(objects);
        Destroy(objects);
    }

    IEnumerator resetSpawn()
    {
        yield return new WaitForSeconds(2);
        startSpawn = false;
    }

    public void SetRB()
    {
        if (arcadeDodge)
        {
            rb = player.GetComponent<Rigidbody2D>();
            if (!infinity)
            {
                rb.isKinematic = true;
            }
            else
            {
                rb.isKinematic = false;
            }
            
        }
        else if (arcadeShoot)
        {
            rb = playerArcadeShoot.GetComponent<Rigidbody2D>();
            if (!infinity)
            {
                rb.isKinematic = true;
            }
            else
            {
                rb.isKinematic = false;
            }
         
        }
    }
}

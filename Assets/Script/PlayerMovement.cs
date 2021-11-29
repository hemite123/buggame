using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using Cinemachine;

public class PlayerMovement : MonoBehaviour
{
    CharacterController _charcon;
    Rigidbody rigi;
    Vector3 move;
    public LayerMask groundMask;
    public bool _isGround;
    public float _camRotationX = 0.0f;
    public float _camRotationY = 0.0f;
    public float senPc = 100f;
    Gamemanager gamemanager;
    public CamManagement camMan;
    public PlayableDirector director;
    public GameObject playerStep;
    public static PlayerMovement instance;
    public GameObject shootOrigin;
    public bool isSkill;
    public bool isStun;
    public GameObject targetskill;
    public Vector3 groundPosition;
    bool seeTarget;
    public bool _cantFly;
    public bool _isFly;
    public float playerSpeed;
    Animator anim;
    public float TurnSmoothVelo;
    public Transform cam;
    Vector3 movedir;
    public Light playerLight;
    // Start is called before the first frame update

    private void Awake()
    {
        if(instance != null)
        {
            return;
        }
        instance = this;
    }

    void Start()
    {
        _charcon = GetComponent<CharacterController>();
        gamemanager = Gamemanager.instace;
        camMan = CamManagement.instance;
        rigi = GetComponent<Rigidbody>();
        anim = transform.parent.GetComponent<Animator>();
        director = GetComponent<PlayableDirector>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isStun && !gamemanager.isDone && !gamemanager.isUiPlay && gamemanager.isStart && !gamemanager.isPause)
        {
            
            if (seeTarget)
            {
                Vector3 dir = gamemanager.target.transform.position - transform.position;
                Quaternion qt = Quaternion.LookRotation(dir);
                transform.localRotation = Quaternion.Slerp(transform.rotation, qt, 1f);
            }
            if (Input.GetKey(KeyCode.Space) && !_cantFly)
            {
                _isFly = true;
            }
            else
            {
                _isFly = false;
            }

            if (_isFly)
            {
                if (transform.position.y < groundPosition.y + 10f)
                {
                    movedir.y = Mathf.Sqrt(0.1f * -2f * -9.15f);
                    _charcon.Move(movedir.normalized * playerSpeed * Time.deltaTime);
                    camMan.ChangeCam("camFly");
                }
                else
                {
                    camMan.ChangeCam("SteadyCam");
                }
            }
            else
            {
                camMan.ChangeCam("cam1");
            }

            
        }
        
        
    }

    private void LateUpdate()
    {
        if (!isStun && !gamemanager.isDone && !gamemanager.isUiPlay && gamemanager.isStart && !gamemanager.isPause)
        {
            _isGround = Physics.CheckSphere(playerStep.transform.position, 0.01f, groundMask);
            if (_isGround)
            {
                groundPosition = this.transform.position;
            }
            float moveHorizontal = Input.GetAxisRaw("Horizontal");
            float moveVertical = Input.GetAxisRaw("Vertical");
            move = new Vector3(moveHorizontal, 0f, moveVertical).normalized;
            if (move.magnitude >= 0.1f)
            {
                anim.SetBool("isWalking", true);
                float tAngle = Mathf.Atan2(move.x, move.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, tAngle, ref TurnSmoothVelo, 0.1f);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
                movedir = Quaternion.Euler(0f, tAngle,0f) * Vector3.forward;
                _charcon.Move(movedir.normalized * playerSpeed * Time.deltaTime);
            }
            else
            {
                anim.SetBool("isWalking", false);
            }
            if (!_isGround && !_isFly && !_cantFly)
            {
                movedir.y += -9.15f * 3f * Time.deltaTime;
                _charcon.Move(movedir.normalized * playerSpeed * Time.deltaTime);
            }
            else if (!_isGround && _cantFly)
            {
                movedir.y += -9.15f * 5f * Time.deltaTime;
                _charcon.Move(movedir.normalized * playerSpeed * Time.deltaTime);
            }
        }   
    }

    public CharacterController getCC()
    {
        return _charcon;
    }

   

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "skill" && other.GetComponent<Objective>().isOn)
        {
            gamemanager.isSkillExe = true;
            targetskill = other.gameObject;
        }
        else if (other.transform.tag == "arcade")
        {
            gamemanager.collisionArcade = other.gameObject;
            gamemanager.SkillArcadeCheck("");
        }else if (other.transform.tag == "warp")
        {
            gamemanager.Warping(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "skill")
        {
            gamemanager.isSkillExe = false;
            targetskill = null;
        }else if(other.transform.tag == "arcade")
        {
            gamemanager.collisionArcade = null;
            gamemanager.uiLookat.SetActive(false);
            gamemanager.uiLookat = null;
        }
    }

    public void SetLook(bool cond)
    {
        seeTarget = cond;
    }

   


    public Animator getAnim()
    {
        return anim;
    }

}

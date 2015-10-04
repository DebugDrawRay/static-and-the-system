using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class player : MonoBehaviour
{
    //state control
    private delegate void stateContainer();
    private stateContainer activeState;

    private enum state
    {
        grounded,
        jumping,
        inAir,
        onWall,
        offWall
    }
    private state currentState;

    [Header("Weapons Control")]
    private GameObject proj;
    public GameObject bitGun;
    public GameObject recordGun;
    public float projOffsetX;
    public float projOffsetY;

    public bool hasBitGun;
    public bool hasRecordGun;

    public enum guns
    {
        bitGun,
        recordGun
    }

    public guns currentGun;

    [Header("Collision Control")]
    public LayerMask jumpableLayers;
    public LayerMask wallJumpableLayers;
    public float jumpCheckBuffer;
    public float wallJumpCheckMod;
    public string[] damageSourceTags;

    private Vector3 knockbackDir;

    [Header("Scanner Control")]
    public GameObject scanObject;
    public GameObject recObject;

    public Vector2 scanRelPos;
    public Vector2 recordRelPos;

    private GameObject scanner;
    private GameObject recorder;

    public GameObject recordedObject;
    public GameObject recordableObject;
    private bool recordHold;

    public Sprite canRecord;
    public Sprite canNotRecord;

    [Header("Damage Source Reaction")]
    public float knockbackForce;
    public float knockbackLength;
    private float currentKnockbackLength;
    public Color damageColor;

    [Header("External Animations")]
    public GameObject recordAnim;
    public GameObject dashCloud;

    //components
    private Rigidbody2D rigid;
    private SpriteRenderer currentSprite;
    private Animator anim;
    private status currentStatus;
    private AudioSource audio;

    private motor motor;
    private jumpController jumpController;
    private wallJumpController wallJumpController;
    private gunController gunController;

    //inputs - get definitions within inputs.cs
    private IinputListener controllerInput;

    private float hor;
    private float lastDir;
    private bool jump;
    private bool jumpOnce;
    private bool jumpInput;
    private bool wallJumpInput;
    private bool fireWeapon;
    private bool scan;
    private bool recordDown;
    private bool recordUp;
    private bool dash;
    private bool switchGuns;
    //virtual inputs
    public bool knockback;

    void initializeComponents()
    {
        rigid = GetComponent<Rigidbody2D>();
        currentSprite = GetComponent<SpriteRenderer>();
        currentStatus = GetComponent<status>();
        anim = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();

        motor = GetComponent<motor>();
        jumpController = GetComponent<jumpController>();
        wallJumpController = GetComponent<wallJumpController>();
        gunController = GetComponent<gunController>();
    }

    void initializeClasses()
    {
        controllerInput = new inputListener();
    }

    void initializeVars()
    {
        currentKnockbackLength = knockbackLength;
        lastDir = 1;
        scanner = Instantiate(scanObject, transform.position, Quaternion.identity) as GameObject;
        recorder = Instantiate(recObject, transform.position + transform.right, Quaternion.identity) as GameObject;
    }

    public static player Instance
    {
        get;
        private set;
    }

    void initializeInstance()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
        GameObject.DontDestroyOnLoad(this.gameObject);
    }

    void Awake()
    {
        initializeVars();
        initializeInstance();
        initializeComponents();
        initializeClasses();
    }

    void Update()
    {
        stateMachine();
        animStateMachine();
    }

    //equipment controllers
    /*void switchCurrentGun()
    {
        if(switchGuns)
        {
            int length = System.Enum.GetNames(typeof(guns)).Length - 1;
            currentGun = (guns)(int)currentGun + 1;

            if ((int)currentGun > length)
            {
                currentGun = (guns)0;
            }
        }
    }

    void checkEquipedGun()
    {
        if(currentGun == guns.bitGun)
        {
            proj = bitGun;
        }
        else if(currentGun == guns.recordGun)
        {
            proj = recordGun;
        }
    }

    void checkEquipment()
    {
        checkEquipedGun();
    }

    //movement 

    void dashController()
    {
        if (dash)
        {
            if (!inDash)
            {
                //AudioSource.PlayClipAtPoint(dashSound, transform.position);
                Vector2 newDir = Vector2.right * lastDir;
                Vector2 addDash = newDir * dashForce;
                rigid.AddForce(addDash);
                Instantiate(dashCloud, transform.position, Quaternion.identity);
                inDash = true;
            }
        }
        if (inDash)
        {
            currentDashLength -= Time.deltaTime;
            if (currentDashLength <= 0)
            {
                inDash = false;
                currentDashLength = dashLength;
            }
        }
    }*/

    //Scanning and recording interactions
    void scanController()
    {
        scanObject obj = scanner.GetComponent<scanObject>();
        if (scan)
        {
            obj.toggleActive();
        }
        Vector2 anchor = scanRelPos + new Vector2(transform.position.x, transform.position.y);
        scanner.transform.position = anchor;
    }

    void recordController()
    {
        recorderObject obj = recorder.GetComponent<recorderObject>();
        if (recordDown)
        {
            recordHold = true;
            if (recordedObject != null)
            {
                previewObject(recordedObject);
            }
            if(recordedObject == null && recordableObject != null)
            {
                recorder.GetComponent<SpriteRenderer>().sprite = canRecord;
            }
            if (recordedObject == null && recordableObject == null)
            {
                recorder.GetComponent<SpriteRenderer>().sprite = canNotRecord;
            }
        }
        if (recordHold && recordUp)
        {
            if (recordedObject == null && recordableObject != null)
            {
                recordObject(recordableObject);
            }
            else if(recordedObject != null)
            {
                replayObject(recordedObject);
            }
            recorder.GetComponent<SpriteRenderer>().sprite = null;
            recordHold = false;

        }
        Vector2 origin = new Vector2(transform.position.x, transform.position.y);
        Vector2 anchor = origin + (recordRelPos * lastDir);
        recorder.transform.position = anchor;
    }

    void recordObject(GameObject target)
    {
        recordAnimation(target);
        target.GetComponent<objectData>().labelActive(false);
        recordedObject = target;
        recordedObject.SetActive(false);
    }

    void previewObject(GameObject target)
    {
        target.SetActive(true);
        foreach (Collider2D col in target.GetComponents<Collider2D>())
        {
            col.enabled = false;
        }
        target.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 0.5f);
        target.transform.position = transform.position + (Vector3.right * lastDir);
    }

    void replayObject(GameObject target)
    {
        foreach (Collider2D col in target.GetComponents<Collider2D>())
        {
            col.enabled = true;
        }
        target.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 1);
        target.SetActive(true);
        target.transform.position = transform.position + (Vector3.right * lastDir);
        recordAnimation(target);
        recordedObject = null;
    }

    void recordAnimation(GameObject target)
    {
        Vector2 origin = target.transform.position;
        Instantiate(recordAnim, origin, Quaternion.identity);
    }

    //bang bangs
    /*void weaponController()
    {
        if (fireWeapon)
        {
            Vector3 posX = ((transform.right * lastDir) / projOffsetX);
            Vector3 posY = -(transform.up / projOffsetY);
            Vector3 pos = posX + posY;
            GameObject newProj = Instantiate(proj, transform.position + pos, Quaternion.identity) as GameObject;
            newProj.GetComponent<playerProjectile>().facing = lastDir;
        }
        switchCurrentGun();
        checkEquipedGun();
    }*/

    //collisions
    void OnTriggerEnter2D(Collider2D hit)
    {
        if (tagMatch(damageSourceTags, hit.gameObject.tag))
        {
            knockback = true;
            knockbackDir = hit.gameObject.transform.position - transform.position;  
        }
    }
    bool checkGrounded()
    {
        Vector2 pos = GetComponent<Collider2D>().bounds.center;
        Vector2 dir = -Vector2.up;
        Vector2 size = GetComponent<Collider2D>().bounds.size;
        RaycastHit2D checkHit = Physics2D.BoxCast(pos, size, 0, dir, jumpCheckBuffer, jumpableLayers);
        if (checkHit && checkHit.collider != null)
        { 
            return true;
        }
        else
        {
            return false;
        }
    }

    bool checkWallCling()
    {
        Vector2 pos = GetComponent<Collider2D>().bounds.center;
        Vector2 lDir = -Vector2.right;
        Vector2 rDir = Vector2.right;
        Vector2 size = GetComponent<Collider2D>().bounds.size;
        size.y = size.y / wallJumpCheckMod;
        RaycastHit2D checkHitL = Physics2D.BoxCast(pos, size, 0, lDir, jumpCheckBuffer, wallJumpableLayers);
        RaycastHit2D checkHitR = Physics2D.BoxCast(pos, size, 0, rDir, jumpCheckBuffer, wallJumpableLayers);
        if (checkHitL && checkHitL.collider != null && controllerInput.horAxis() < 0)
        {
            return true;
        }
        else if (checkHitR && checkHitR.collider != null && controllerInput.horAxis() > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void knockbackController()
    {
        if (currentKnockbackLength > 0)
        {
            currentSprite.color = damageColor;
            Vector2 newDir = new Vector2(knockbackDir.x, 0);
            newDir.x = Mathf.Round(newDir.normalized.x);
            Vector2 knock = (newDir - Vector2.up) * knockbackForce;
            rigid.AddForce(-knock);
            currentKnockbackLength -= Time.deltaTime;
        }
        else if(currentKnockbackLength <= 0)
        {
            currentSprite.color = Color.white;
            if(checkGrounded())
            {
                currentKnockbackLength = knockbackLength;
                knockback = false;
            }
        }
    }

    //utlities
    bool tagMatch(string[] list, string tag)
    {
        foreach (string item in list)
        {
            if (item == tag)
            {
                return true;
            }
        }
        return false;
    }

    //state control
    void runState(actionsController target)
    {
        if (target != null)
        {
            target.update(controllerInput);
        }
    }
    void stateMachine()
    {
        Debug.Log(currentState);
        switch (currentState)
        {
            case state.grounded:
                runState(motor);
                if (controllerInput.jumpOnce())
                {
                    currentState = state.jumping;
                }
                break;
            case state.jumping:
                runState(jumpController);
                runState(motor);
                if (!controllerInput.jump())
                {
                    currentState = state.inAir;
                }
                break;
            case state.onWall:
                runState(wallJumpController);
                runState(motor);
                if (controllerInput.jumpOnce())
                {
                    currentState = state.offWall;
                }
                if(checkGrounded())
                {
                    currentState = state.grounded;
                }
                break;
            case state.offWall:
                if(checkGrounded())
                {
                    currentState = state.grounded;
                }
                if (checkWallCling())
                {
                    currentState = state.onWall;
                }
                break;
            case state.inAir:
                runState(motor);
                if (checkGrounded())
                {
                    currentState = state.grounded;
                }
                if (checkWallCling())
                {
                    currentState = state.onWall;
                }
                break;
        }
        constantStates();
    }

    void constantStates()
    {
        runState(gunController);
    }

    void animStateMachine()
    {
        anim.SetFloat("Direction", controllerInput.horAxis());
        anim.SetBool("isShooting", fireWeapon);
        anim.SetBool("isGrounded", checkGrounded());
        if (controllerInput.horAxis() != 0)
        {
            anim.SetBool("isMoving", true);
        }
        else
        {
            anim.SetBool("isMoving", false);
        }
    }
}
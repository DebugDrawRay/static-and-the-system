using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class player : MonoBehaviour
{

    //state control
    private delegate void stateContainer();
    private stateContainer activeState;

    [Header("Movement Control")]
    public float moveSpeed;
    public float moveAccel;
    public float dashForce;
    public float dashLength;
    private float currentDashLength;
    private bool inDash;
    public AudioClip dashSound;
    [Header("Jump Control")]
    public float jumpVel;
    public AnimationCurve jumpCurve;
    public float wallJumpVelMod;

    public float jumpHold;
    public float jumpHoldMax;
    

    private float wallJumpDir;
    public AudioClip jumpSound;

    [Header("Weapons Control")]
    public GameObject proj;
    public float projOffsetX;
    public float projOffsetY;

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

    //inputs - get definitions within inputs.cs
    private float hor;
    private float lastDir;
    private bool jump;
    private bool jumpInput;
    private bool wallJumpInput;
    private bool fireWeapon;
    private bool scan;
    private bool record;
    private bool dash;
    //virtual inputs
    public bool knockback;

    void initializeComponents()
    {
        rigid = GetComponent<Rigidbody2D>();
        currentSprite = GetComponent<SpriteRenderer>();
        currentStatus = GetComponent<status>();
        anim = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
    }

    void initializeClasses()
    {

    }

    void initializeVars()
    {
        currentDashLength = dashLength;
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
        runListeners();
        stateMachine();
        animStateMachine();
    }

    void runListeners()
    {
        inputListener();
    }

    //inputs
    void inputListener()
    {
        hor = Input.GetAxisRaw(Inputs.horAxis);
        jump = Input.GetButton(Inputs.jump);
        fireWeapon = Input.GetButtonDown(Inputs.fire);
        scan = Input.GetButtonDown(Inputs.scan);
        record = Input.GetButtonDown(Inputs.record);
        dash = Input.GetButtonDown(Inputs.dash);
        
        //get the last pressed direction
        getLastDir(hor);

        //get length of press
        jumpHold += getHoldLength(jump);
    }

    float getHoldLength(bool input)
    {
        if(input)
        {
            return Time.deltaTime;
        }
        else
        {
            return 0;
        }
    }

    void getLastDir(float input)
    {
        if (hor != 0)
        {
            lastDir = input;
        }
    }

    //movement 
    void movementController()
    {
            Vector3 startVel = rigid.velocity;
            Vector3 newVel = (transform.right * hor) * moveSpeed;
            Vector3 vel = Vector3.Lerp(startVel, newVel, moveAccel);
            vel.y = rigid.velocity.y;
            rigid.velocity = vel;
    }

    void dashController(Vector3 dir)
    {
        if (dash)
        {
            if (!inDash)
            {
                //AudioSource.PlayClipAtPoint(dashSound, transform.position);
                Vector2 newDir = new Vector2(dir.x, dir.y);
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
    }

    //jumping
    void jumpController()
    {
        if (jump)
        {
            jumpInput = true;
            if (jumpHold < jumpHoldMax)
            {
                if (Input.GetButtonDown(Inputs.jump) && (checkGrounded() || checkWallCling()))
                {
                    AudioSource.PlayClipAtPoint(jumpSound, transform.position);
                }
                Vector2 force = Vector2.up * jumpVel;
                force.x = rigid.velocity.x;
                jumpAction(force);
            }
        }
        if(jumpInput)
        {
            if(!jump)
            {
                jumpHold = jumpHoldMax;
                jumpInput = false;
            }
        }
    }
    void wallJumpController()
    {
        if (jump)
        {
            if (wallJumpInput && hor != 0)
            {
                if (Input.GetButtonDown(Inputs.jump) && (checkGrounded() || checkWallCling()))
                {
                    AudioSource.PlayClipAtPoint(jumpSound, transform.position);
                }
                Vector2 jumpVect = (Vector2.up + (-Vector2.right * wallJumpDir)) * jumpVel;
                jumpAction(jumpVect / wallJumpVelMod);
                wallJumpInput = false;
            }
        }
    }

    void jumpAction(Vector2 jumpVector)
    {
        float height = jumpCurve.Evaluate(jumpHold / jumpHoldMax);
        Vector2 calcVel = new Vector2(jumpVector.x, jumpVector.y * height);
        rigid.velocity = calcVel;
    }

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
        if (record)
        {
            if (recordedObject == null && recordableObject != null)
            {
                recordObject(recordableObject);
            }
            else if(recordedObject != null)
            {
                replayObject(recordedObject);
            }
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

    void replayObject(GameObject target)
    {
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
    void weaponController()
    {
        if (fireWeapon)
        {
            Vector3 posX = ((transform.right * lastDir) / projOffsetX);
            Vector3 posY = -(transform.up / projOffsetY);
            Vector3 pos = posX + posY;
            GameObject newProj = Instantiate(proj, transform.position + pos, Quaternion.identity) as GameObject;
            newProj.GetComponent<playerProjectile>().facing = lastDir;
        }
    }

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
            if(!jump)
            {
                jumpHold = 0;
            }

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
        if (hor != 0)
        {
            if (checkHitL && checkHitL.collider != null)
            {
                if (!jump)
                {
                    wallJumpInput = true;
                }
                wallJumpDir = -1;
                return true;
            }
            else if (checkHitR && checkHitR.collider != null)
            {
                if (!jump)
                {
                    wallJumpInput = true;
                }
                wallJumpDir = 1;
                return true;
            }
            else
            {
                if (!jump)
                {
                    wallJumpInput = false;
                }
                return false;
            }
        }
        else
        {
            if (!jump)
            {
                wallJumpInput = false;
            }
            return false;
        }
    }

    void knockbackController()
    {
        currentSprite.color = damageColor;
        Vector2 newDir = new Vector2(knockbackDir.x, knockbackDir.y);
        Vector2 knock = newDir * knockbackForce;
        rigid.AddForce(-knock);
        //Vector2 randomPosition = Random.insideUnitCircle + new Vector2(transform.position.x, transform.position.y);
        //transform.position = new Vector3(randomPosition.x, randomPosition.y, transform.position.z);
        currentKnockbackLength -= Time.deltaTime;
        if (currentKnockbackLength <= 0)
        {
            currentKnockbackLength = knockbackLength;
            knockback = false;
            currentSprite.color = Color.white;
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

    //states
    void groundedState()
    {
        weaponController();
        movementController();
        jumpController();
        dashController(Vector2.right * lastDir);
        scanController();
        recordController();
    }
    void jumpedState()
    {
        weaponController();
        movementController();
        dashController(Vector2.right * lastDir);//THIS IS FUCKING DUMB WRITE A FUNCTION/NEW VAR ASSHOLE
        scanController();
    }
    void wallClingState()
    {
        wallJumpController();
        movementController();
        scanController();
    }
    void wallJumpState()
    {
        wallJumpController();
    }
    void dashState()
    {
        dashController(Vector2.right * lastDir);
        scanController();
    }
    void knockbackState()
    {
        knockbackController();
    }

    //state control
    void stateMachine()
    {
        if (knockback)
        {
            activeState = knockbackState;
        }
        else if (checkGrounded())
        {
            if (inDash)
            {
                activeState = dashState;
            }
            else
            {
                activeState = groundedState;
            }
        }
        else if (checkWallCling())
        {
            Debug.Log("Is clinged");
            activeState = wallClingState;
            if(jump)
            {
                activeState = wallJumpState;
            }
        }
        else if (inDash)
        {
            activeState = dashState;
        }
        /* else
        {
            if (inDash)
            {
                activeState = dashState;
            }
            else
            {
                activeState = jumpedState;
            }
        }*/

        if (activeState != null)
        {
            activeState();
        }
    }

    void animStateMachine()
    {
        anim.SetFloat("Direction", lastDir);
        anim.SetBool("isShooting", fireWeapon);
        if (checkGrounded())
        {
            anim.SetBool("isGrounded", true);
            if (hor != 0)
            {
                anim.SetBool("isMoving", true);
            }
            else
            {
                anim.SetBool("isMoving", false);
            }
        }
        else
        {
            anim.SetBool("isGrounded", false);
        }
    }
}

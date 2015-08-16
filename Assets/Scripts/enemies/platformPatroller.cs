using UnityEngine;
using System.Collections;

public class platformPatroller : MonoBehaviour
{
    //state control
    private delegate void stateContainer();
    private stateContainer activeState;

    //inputs
    private float hor;

    [Header("Collisions")]
    private bool hostileCollision;
    public float collisionReactTime;
    private float maxCollisionReactTime;

    //components
    private enemy properties;
    private Rigidbody2D rigid;
    private SpriteRenderer sprite;

    private const float EDGE_CHECK_BUFFER = 1;

    void initializeComponents()
    {
        properties = GetComponent<enemy>();
        rigid = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    void Awake()
    {
        maxCollisionReactTime = collisionReactTime;
        initializeComponents();
    }
    void Update()
    {
        inputListener();
        stateMachine();
        currentState();
    }

    void inputListener()
    {
        hor = getDirection();
    }

    //virtual input & edge detection 
    float getDirection()
    {
        if(hor == 0)
        {
            hor = 1;
        }

        Vector2 forward = Vector2.right;//new Vector2(transform.right.x, transform.right.y);
        Vector2 origin = new Vector2(transform.position.x, transform.position.y) + (forward * hor);
        Vector2 direction = new Vector2(-Vector3.up.x, -Vector3.up.y);
        Ray2D check = new Ray2D(origin, direction);
        RaycastHit2D checkHit = Physics2D.Raycast(check.origin, check.direction, EDGE_CHECK_BUFFER, properties.ground);
        if (checkHit.collider == null)
        {
            hor = hor * -1;
        }

        return hor;
    }

    //movement
    void movementController()
    {
        Vector2 vel = (Vector2.right * hor) * properties.moveSpeed;
        rigid.velocity = vel;
    }

    void pauseMovementController()
    {
        rigid.velocity = Vector2.zero;
        collisionReactTime -= Time.deltaTime;
        if(collisionReactTime <= 0)
        {
            collisionReactTime = maxCollisionReactTime;
            hostileCollision = false;
        }
    }

    //collisions
    void OnTriggerEnter2D(Collider2D hit)
    {
        foreach (string tag in properties.hostileTags)
        {
            if (hit.gameObject.tag == tag && !hostileCollision)
            {
                hostileCollision = true;
            }
        }
    }
    //states
    void groundedState()
    {
        movementController();
    }
    void hitReactionState()
    {
        pauseMovementController();
    }

    //state control
    void stateMachine()
    {
        if(hostileCollision)
        {
            activeState = pauseMovementController;
        }
        else
        {
            activeState = movementController;
        }
    }
    void currentState()
    {
        if(activeState != null)
        {
            activeState();
        }
    }
}
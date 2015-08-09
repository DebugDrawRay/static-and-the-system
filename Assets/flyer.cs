using UnityEngine;
using System.Collections;

public class flyer : MonoBehaviour
{
    private delegate void stateContainer();
    private stateContainer activeState;
    private Rigidbody2D rigid;
    private enemy properties;
    private Vector2 currentTargetDir;
    public LayerMask sightBlockers;
    void initializeComponents()
    {
        rigid = GetComponent<Rigidbody2D>();
        properties = GetComponent<enemy>();
    }

    void Awake()
    {
        initializeComponents();
    }

    void Start()
    {
    }

    void Update()
    {
        stateMachine();
        currentTargetDir = trackTarget(player.Instance.gameObject);
    }
    
    Vector2 trackTarget(GameObject target)
    {
        Vector2 targetPos = new Vector2(target.transform.position.x, target.transform.position.y);
        Vector2 currentPos = new Vector2(transform.position.x, transform.position.y);
        return targetPos - currentPos;
    }
    void followController()
    {
        rigid.AddForce(currentTargetDir * properties.moveSpeed);//this gonna break yo, fix yo shit
    } 
    
    bool targetInSight()
    {
        Vector2 currentPos = new Vector2(transform.position.x, transform.position.y);
        RaycastHit2D inSight = Physics2D.Raycast(currentPos, currentTargetDir, Mathf.Infinity, sightBlockers);
        Debug.DrawRay(currentPos, currentTargetDir);
        if(inSight.collider.gameObject.tag == player.Instance.gameObject.tag)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void followState()
    {
        followController();
    }
    void idleState()
    {

    }
    void stateMachine()
    {
        if (targetInSight())
        {
            activeState = followState;
        }
        else
        {
            activeState = idleState;
        }
        activeState();
    }
}

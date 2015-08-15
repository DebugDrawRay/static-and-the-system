using UnityEngine;
using System.Collections;

public class flyer : MonoBehaviour
{
    private delegate void stateContainer();
    private stateContainer activeState;
    private Rigidbody2D rigid;
    private enemy properties;
    private GameObject _player;
    public LayerMask sightBlockers;
    void initializeComponents()
    {
        rigid = GetComponent<Rigidbody2D>();
        properties = GetComponent<enemy>();
        _player = player.Instance.gameObject;
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
    }
    
    Vector2 direction(GameObject target, GameObject origin)
    {
        Vector2 targetPos = new Vector2(target.transform.position.x, target.transform.position.y);
        Vector2 currentPos = new Vector2(origin.transform.position.x, origin.transform.position.y);
        return targetPos - currentPos;
    }
    void followController()
    {
        Vector2 currentTargetDir = direction(_player, this.gameObject);
        rigid.AddForce(currentTargetDir * properties.moveSpeed);//this gonna break yo, fix yo shit(fixed with angular drag for the moment, revisit?)
    } 
    
    bool targetInSight()
    {
        Vector2 currentPos = new Vector2(transform.position.x, transform.position.y);
        Vector2 currentTargetDir = direction(_player, this.gameObject);
        RaycastHit2D inSight = Physics2D.Raycast(currentPos, currentTargetDir, Mathf.Infinity, sightBlockers);
        Debug.DrawRay(currentPos, currentTargetDir);
        Debug.Log(inSight.collider.name);
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

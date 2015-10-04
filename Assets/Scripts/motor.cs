using UnityEngine;
using System;
using System.Collections;

//[RequireComponent(typeof(Rigidbody))]
public class motor : MonoBehaviour, actionsController
{
    [Header("Movement Properties")]
    public MoveType moveType;
    public float moveSpeed;
    public float moveAccel;
    public bool hasGravity;

    public enum MoveType
    {
        xDirectional,
        yDirectional,
        xyDirectional
    }

    //components
    private Rigidbody2D rigid;

    void Awake()
    {
        onEnter();
    }
    void movementController(float horAxis, float verAxis)
    {
        Vector2 startVel = rigid.velocity;
        Vector2 newVelX = (transform.right * horAxis) * moveSpeed;
        Vector2 newVelY = (transform.up * verAxis) * moveSpeed;
        Vector2 vel = Vector2.zero;

        if (moveType == MoveType.xDirectional)
        {
            Vector2 newVel = newVelX;
            vel = Vector2.Lerp(startVel, newVel, moveAccel);
            vel.y = rigid.velocity.y;
        }
        if (moveType == MoveType.yDirectional)
        {
            Vector2 newVel = newVelY;
            vel = Vector2.Lerp(startVel, newVel, moveAccel);
            vel.x = rigid.velocity.x;
        }
        if (moveType == MoveType.xyDirectional)
        {
            Vector2 newVel = newVelX + newVelY;
            vel = Vector2.Lerp(startVel, newVel, moveAccel);
        }

        rigid.velocity = vel;
    }

    public void onEnter()
    {
        rigid = GetComponent<Rigidbody2D>();
    }
    public void update(IinputListener input)
    {
        movementController(input.horAxis(), input.verAxis());
    }
    public void onExit()
    {

    }
}

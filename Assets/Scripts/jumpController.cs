using UnityEngine;
using System.Collections;

public class jumpController : MonoBehaviour, actionsController
{
    [Header("Jump Control")]
    public float jumpVel;
    public AnimationCurve jumpCurve;

    public float jumpHoldMax;
    public AudioClip jumpSound;

    private bool jumpInput;

    public LayerMask jumpableLayers;
    public float jumpCheckBuffer;

    //components
    private Rigidbody2D rigid;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    void jumpHandler(bool jump, float jumpHold)
    {
        if (jump)
        {
            if (jumpHold < jumpHoldMax)
            {
                //fix this
                if (Input.GetButtonDown(Inputs.jump))
                {
                    AudioSource.PlayClipAtPoint(jumpSound, transform.position);
                }
                Vector2 jumpVector = Vector2.up * jumpVel;
                jumpVector.x = rigid.velocity.x;
                float height = jumpCurve.Evaluate(jumpHold / jumpHoldMax);
                Vector2 calcVel = jumpVector * height;
                rigid.velocity = calcVel;
            }
        }
    }

    public void onEnter()
    {

    }
    public void update(IinputListener input)
    {
        jumpHandler(input.jump(), input.inputHold(input.jump()));
    }
    public void onExit()
    {
    }
}

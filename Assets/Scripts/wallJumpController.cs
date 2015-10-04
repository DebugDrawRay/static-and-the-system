using UnityEngine;
using System.Collections;

public class wallJumpController : MonoBehaviour, actionsController
{
    [Header("Jump Control")]
    public float jumpVel;
    public AnimationCurve jumpCurve;
    public float wallJumpVelMod;

    public AudioClip jumpSound;

    //components
    private Rigidbody2D rigid;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }
    void wallJumpHandler(bool jump, float wallJumpDir)
    {
        if (jump)
        {
                if (Input.GetButtonDown(Inputs.jump))
                {
                    AudioSource.PlayClipAtPoint(jumpSound, transform.position);
                }
                Vector2 jumpVect = (Vector2.up + (-Vector2.right * wallJumpDir)) * jumpVel;
                Vector2 calcVel = jumpVect;
                rigid.velocity = calcVel;
        }
    }
    public void onEnter()
    {
    }
    public void update(IinputListener input)
    {
        wallJumpHandler(input.jumpOnce(), input.horAxis());
    }
    public void onExit()
    {
    }
}

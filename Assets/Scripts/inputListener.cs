using UnityEngine;
using System.Collections;

public class inputListener : IinputListener
{
    public float hold;

    public float horAxis()
    {
        return Input.GetAxisRaw(Inputs.horAxis);
    }
    public float verAxis()
    {
        return Input.GetAxisRaw(Inputs.verAxis);
    }
    public bool jump()
    {
        return Input.GetButton(Inputs.jump);
    }
    public bool jumpOnce()
    {
        return Input.GetButtonDown(Inputs.jump);
    }
    public bool fireGun()
    {
        return Input.GetButtonDown(Inputs.fireGun);
    }
    public bool scan()
    {
        return Input.GetButtonDown(Inputs.scan);
    }
    public bool dash()
    {
        return Input.GetButtonDown(Inputs.dash);
    }
    public bool switchGuns()
    {
        return Input.GetButtonDown(Inputs.switchGuns);
    }
    public float inputHold(bool input)
    {
        if(input)
        {
            hold += Time.deltaTime;
            return hold;
        }
        else
        {
            hold = 0;
            return hold;
        }
    }
}

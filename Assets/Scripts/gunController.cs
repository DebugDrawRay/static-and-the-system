using UnityEngine;
using System.Collections;

public class gunController : MonoBehaviour, actionsController
{


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

using UnityEngine;
using System.Collections;

public interface IinputListener
{
    float horAxis();
    float verAxis();
    bool jump();
    bool jumpOnce();
    bool fireGun();
    bool scan();
    bool dash();
    bool switchGuns();
    float inputHold(bool input);
}

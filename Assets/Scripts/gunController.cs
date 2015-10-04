using UnityEngine;
using System.Collections;

public class gunController : MonoBehaviour, actionsController
{
    [Header("Weapons Control")]
    public GameObject[] availableGuns;

    private int currentGunIndex;
    private GameObject currentGun;

    public enum guns
    {
        bitGun,
        recordGun
    }

    void Awake()
    {
        currentGun = availableGuns[currentGunIndex];
    }

    void switchGun(bool switchGun)
    {
        if (switchGun)
        {
            if (availableGuns[currentGunIndex + 1] != null)
            {
                currentGunIndex = 0;
            }
            currentGun = availableGuns[currentGunIndex];
        }
    }
    void useGun(bool fireGun, float horAxis)
    {
        if (fireGun)
        {
            currentGun.GetComponent<Igun>().fire(transform.position, horAxis);
        }
    }

    public void onEnter()
    {

    }
    public void update(IinputListener input)
    {
        useGun(input.fireGun(), input.lastDir(input.horAxis()));
    }
    public void onExit()
    {
    }
}

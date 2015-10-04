using UnityEngine;
using System.Collections;

public class bitGun : MonoBehaviour, Igun
{
    public GameObject proj;

    public float projOffsetX;
    public float projOffsetY;

    public void fire(Vector2 origin, float horAxis)
    {
        Vector2 posX = ((transform.right * horAxis) / projOffsetX);
        Vector2 posY = -(transform.up / projOffsetY);
        Vector2 pos = posX + posY;
        GameObject newProj = Instantiate(proj, origin + pos, Quaternion.identity) as GameObject;
        newProj.GetComponent<projectile>().facing = horAxis;
    }
}

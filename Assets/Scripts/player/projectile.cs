using UnityEngine;
using System.Collections;

public class projectile : MonoBehaviour
{
    public float facing;
    public float speed;

    void Start()
    {
        if(facing < 0)
        {
            transform.eulerAngles = new Vector3(0, 0, 180);
        }
        else if (facing > 0)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        GetComponent<Rigidbody2D>().velocity = speed * (Vector2.right * facing);
    }
}

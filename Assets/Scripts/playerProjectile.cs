using UnityEngine;
using System.Collections;

public class playerProjectile : MonoBehaviour
{
    public float facing;
    public float speed;
    public float damage;

    void Start()
    {
        GetComponent<Rigidbody2D>().velocity = speed * (Vector2.right * facing);
    }
    void OnTriggerEnter2D (Collider2D hit)
    {
        if(hit.gameObject.tag == "Enemy")
        {
            if(hit.gameObject.GetComponent<status>())
            {
                hit.gameObject.GetComponent<status>().changeStatus(damage);
            }
        }
    }

}

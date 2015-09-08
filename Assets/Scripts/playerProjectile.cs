using UnityEngine;
using System.Collections;

public class playerProjectile : MonoBehaviour
{
    public float facing;
    public float speed;
    public float damage;

    void Start()
    {
        if(facing < 0)
        {
            transform.eulerAngles = new Vector3(0, 0, 180);
        }
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

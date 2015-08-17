using UnityEngine;
using System.Collections;

public class enemy : MonoBehaviour 
{
    public string[] hostileTags;
    public LayerMask ground;
    public float moveSpeed;
    public float baseDamage;
    public Sprite[] sprites;
    public bool contactDamage;

    void OnTriggerEnter2D(Collider2D hit)
    {
        if(contactDamage)
        {
            foreach(string tag in hostileTags)
            {
                if(hit.gameObject.tag == tag)
                {
                    if (hit.GetComponent<status>())
                    {
                        hit.GetComponent<status>().changeStatus(baseDamage);
                    }
                }
            }
        }
    }
}

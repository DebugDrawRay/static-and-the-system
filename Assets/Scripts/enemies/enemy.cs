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

    public int dropRate;
    public int dropMaxAmount;
    public GameObject[] pickupDrops;

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

    public void dropPickups()
    {
        int willDrop = Random.Range(0, 1 / dropRate);

        if (willDrop == 0)
        {
            foreach (GameObject drop in pickupDrops)
            {
                int dropAmount = Random.Range(1, dropMaxAmount);
                for(int i = 1; i <= dropAmount; i++)
                {
                    Instantiate(drop, transform.position, Quaternion.identity);
                }
            }
        }
    }
}

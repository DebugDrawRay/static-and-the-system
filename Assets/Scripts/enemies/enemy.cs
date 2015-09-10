using UnityEngine;
using System.Collections;

public class enemy : MonoBehaviour 
{
    [Header("Collisions")]
    public string[] hostileTags;
    public LayerMask ground;

    [Header("Properties")]
    public float moveSpeed;
    public float baseDamage;
    public Sprite[] sprites;
    public bool contactDamage;

    [Header("Item Drops")]
    
    public int dropRate;
    public int dropMaxAmount;
    public GameObject[] pickupDrops;

    [Header("Reactions")]
    public Color damageColor;
    public float reactLength;
    private float maxReactLength;
    public AudioClip damageSound;
    private bool damageHit;
    //Components
    private SpriteRenderer sprite;

    void Start()
    {
        maxReactLength = reactLength;
        sprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if(damageHit)
        {
            hitReact();
        }
    }
    void OnTriggerEnter2D(Collider2D hit)
    {
        if(contactDamage)
        {
            foreach(string tag in hostileTags)
            {
                if(hit.gameObject.tag == tag)
                {
                    damageHit = true;
                }
            }
        }
    }

    public void dropPickups()
    {
        int willDrop = Random.Range(0, dropRate);

        Debug.Log(willDrop);
        if (willDrop == 0)
        {
            foreach (GameObject drop in pickupDrops)
            {
                int dropAmount = Random.Range(1, dropMaxAmount);
                for(int i = 1; i <= dropAmount; i++)
                {
                    Vector2 cir = Random.insideUnitCircle + Vector2.up;
                    Vector2 pos = new Vector2(transform.position.x, transform.position.y) + cir;
                    Instantiate(drop, pos, Quaternion.identity);
                }
            }
        }
    }

    void hitReact()
    {
        if(reactLength == maxReactLength)
        {
            AudioSource.PlayClipAtPoint(damageSound, transform.position);
        }
        sprite.color = damageColor;
        reactLength -= Time.deltaTime;
        if(reactLength <= 0)
        {
            sprite.color = Color.white;
            reactLength = maxReactLength;
            damageHit = false;
        }

    }
}

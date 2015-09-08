using UnityEngine;
using System.Collections;

public class interactionSource : MonoBehaviour 
{
    public string[] tagsListened;
    public float damage;
    public bool isInvul;
    public float lifeTime;
    public GameObject deathReact;

    void Update()
    { 
        if(!isInvul)
        {
            runTimer();
        }
    }

    void runTimer()
    {
        lifeTime -= Time.deltaTime;
        if(lifeTime <= 0)
        {
            Destroy(this.gameObject);
        }
    }
    void OnTriggerEnter2D(Collider2D hit)
    {
        if(checkTagArray(tagsListened, hit.gameObject.tag))
        {
            applyEffects(hit.gameObject);
        }
    }

    bool checkTagArray(string[] array, string target)
    { 
        foreach (string item in array)
        {
            if(item == target)
            {
                return true;
            }
        }
        return false;
    }

    void applyEffects(GameObject target)
    {
        status stats = target.GetComponent<status>();
        if (stats != null)
        {
            stats.changeStatus(damage);
        }
        if (!isInvul)
        {
            Instantiate(deathReact, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }
}

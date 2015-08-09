using UnityEngine;
using System.Collections;

public class status : MonoBehaviour 
{
    public float maxHealth;
    public float currentHealth;

    void Awake()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        if(currentHealth <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    public void changeStatus(float damage)
    {
        currentHealth -= damage;
    }
}

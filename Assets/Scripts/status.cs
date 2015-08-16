using UnityEngine;
using System.Collections;

public class status : MonoBehaviour 
{
    public float maxHealth;
    public float currentHealth;
    public int maxHealthContainers;
    public int currentHealthContainers;
    void Awake()
    {
        currentHealth = maxHealth;
        currentHealthContainers = maxHealthContainers;
    }

    void Update()
    {
        if(currentHealth <= 0)
        {
            currentHealthContainers--;
            if (currentHealthContainers <= 0)
            {
                deathEvent();
            }
        }
    }

    public void deathEvent()
    {
        Destroy(this.gameObject);
    }

    public void changeStatus(float damage)
    {
        currentHealth -= damage;
    }
}

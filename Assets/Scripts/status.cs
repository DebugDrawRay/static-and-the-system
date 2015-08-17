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
  
    }

    public void deathEvent()
    {
        Destroy(this.gameObject);
    }

    public void changeStatus(float health)
    {
        float healthDif = Mathf.Abs(currentHealth - (currentHealth + health));
        currentHealth += health;

        if (currentHealth <= 0)
        {
            if (currentHealthContainers <= 0)
            {
                deathEvent();
            }
            else
            {
                currentHealthContainers--;
                currentHealth = maxHealth - healthDif;
            }
        }

        if (currentHealth >= maxHealth)
        {
            if(currentHealthContainers < maxHealthContainers)
            {
                currentHealthContainers++;
            }
            currentHealth = currentHealth - maxHealth;
        } 
    }
}

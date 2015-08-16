using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class healthMeterUI : MonoBehaviour
{
    public Image healthMeter;
    public Text healthMeterText;
    public Image healthMeterBg;
    public GameObject healthContainer;

    private List<GameObject> containers = new List<GameObject>();

    public float containerRadius;
    public float containerSpreadOffset;
    public float containerSpacing;

    private float maxHealth;
    private int maxHealthContainers;
    private int currentHealthContainers;
    private status playerStatus;

    void Start()
    {
        playerStatus = player.Instance.GetComponent<status>();
        maxHealth = playerStatus.maxHealth;
        maxHealthContainers = playerStatus.maxHealthContainers;
        currentHealthContainers = playerStatus.currentHealthContainers;
        createHealthContainers(currentHealthContainers);
    }

    void createHealthContainers(int count)
    {
        for(int i = 0; i <= count - 1; i++)
        {
            float space = 90 / maxHealthContainers;

            float xSpa = i * space * Mathf.Deg2Rad;
            float xOff = containerSpreadOffset * Mathf.Deg2Rad;
            float xCos = Mathf.Cos(xSpa + xOff);
            float x = transform.position.x + containerRadius * xCos;

            float ySpa = i * space * Mathf.Deg2Rad;
            float yOff = containerSpreadOffset * Mathf.Deg2Rad;
            float ySin = Mathf.Sin(ySpa + yOff);
            float y = transform.position.y + containerRadius * ySin;

            Vector2 pos = new Vector2(x, y);
            GameObject newContainer = Instantiate(healthContainer, pos, Quaternion.identity) as GameObject;
            newContainer.transform.SetParent(this.gameObject.transform);

            containers.Add(newContainer);
        }
    }

    void Update()
    {
        float currentHealth = playerStatus.currentHealth / maxHealth;

        checkHealthContainers(playerStatus.currentHealthContainers);

        healthMeter.fillAmount = currentHealth;
        healthMeterText.text = currentHealth.ToString("0.0");
        healthMeterBg.fillAmount = currentHealth;
    }

    void checkHealthContainers(int count)
    {
        if(count < currentHealthContainers)
        {
            int dif = currentHealthContainers - count;
            currentHealthContainers = count;
            for(int i = 0; i < dif; i++)
            {
                if (containers.Count > 0)
                {
                    Destroy(containers[containers.Count - 1].gameObject);
                    containers.RemoveAt(containers.Count - 1);
                }
            }
        }
        if(count > currentHealthContainers)
        {
            if (containers.Count < maxHealthContainers)
            {
                int dif = count - currentHealthContainers;
                currentHealthContainers = count;
                for (int i = 0; i < dif; i++)
                {
                    float space = 90 / maxHealthContainers;
                    float j = containers.Count;
                    float xSpa = j * space * Mathf.Deg2Rad;
                    float xOff = containerSpreadOffset * Mathf.Deg2Rad;
                    float xCos = Mathf.Cos(xSpa + xOff);
                    float x = transform.position.x + containerRadius * xCos;

                    float ySpa = j * space * Mathf.Deg2Rad;
                    float yOff = containerSpreadOffset * Mathf.Deg2Rad;
                    float ySin = Mathf.Sin(ySpa + yOff);
                    float y = transform.position.y + containerRadius * ySin;

                    Vector2 pos = new Vector2(x, y);
                    GameObject newContainer = Instantiate(healthContainer, pos, Quaternion.identity) as GameObject;
                    newContainer.transform.SetParent(this.gameObject.transform);

                    containers.Add(newContainer);
                }
            }
        }
    }
}

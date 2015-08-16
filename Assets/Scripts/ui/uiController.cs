using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class uiController : MonoBehaviour
{
    public Image healthMeter;
    public Image healthSecondary;
    public Text healthText;
    private status playerStatus;

    void Start()
    {
        playerStatus = player.Instance.gameObject.GetComponent<status>();
    }
    void Update()
    {
        healthController();
    }

    void healthController()
    {
        healthMeter.fillAmount = playerStatus.currentHealth / playerStatus.maxHealth;
        healthSecondary.fillAmount = playerStatus.currentHealth / playerStatus.maxHealth;
        healthText.text = (playerStatus.currentHealth / playerStatus.maxHealth).ToString();
    }
}

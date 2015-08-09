using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class uiController : MonoBehaviour
{
    public Image healthMeter;
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
    }
}

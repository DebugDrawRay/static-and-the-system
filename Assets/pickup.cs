using UnityEngine;
using System.Collections;

public class pickup : MonoBehaviour
{
    public float healthIncrease;

    void OnTriggerEnter2D(Collider2D hit)
    {
        if(hit.gameObject.tag == "Player")
        {
            player.Instance.GetComponent<status>().changeStatus(healthIncrease);
            Destroy(this.gameObject);
        }
    }
}

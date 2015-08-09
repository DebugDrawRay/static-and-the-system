 using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class scanObject : MonoBehaviour
{
    public GameObject label;
    public bool activeState;
    
    void Awake()
    {
        GetComponent<SpriteRenderer>().enabled = activeState;
    }

    public void toggleActive()
    {
        activeState = !activeState;
        GetComponent<SpriteRenderer>().enabled = activeState;
    }
    void OnTriggerStay2D(Collider2D hit)
    {
        if (hit.gameObject.tag == "Finish")
        {
            hit.gameObject.SendMessage("labelActive", activeState, SendMessageOptions.DontRequireReceiver);
            player.Instance.selectedObject = hit.gameObject;
        }
     
    }
    void OnTriggerExit2D(Collider2D hit)
    {
        hit.gameObject.SendMessage("labelActive", false, SendMessageOptions.DontRequireReceiver);
        player.Instance.selectedObject = null;
    }
}

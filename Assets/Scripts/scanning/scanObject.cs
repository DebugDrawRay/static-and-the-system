using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class scanObject : MonoBehaviour
{
    public GameObject label;
    public bool activeState;
    public string[] scanableObjects;

    private SpriteRenderer sprite;
    private player _player;
    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        sprite.enabled = activeState;
    }
    void Start()
    {
        _player = player.Instance;
    }

    public void toggleActive()
    {
        activeState = !activeState;
        sprite.enabled = activeState;
    }

    void OnTriggerStay2D(Collider2D hit)
    {
        objectData data = hit.gameObject.GetComponent<objectData>();
        if (data)
        {
            data.labelActive(activeState);
        }
    }
    void OnTriggerExit2D(Collider2D hit)
    {
        objectData data = hit.gameObject.GetComponent<objectData>();
        if (data)
        {
            data.labelActive(false);
        }
    }
}

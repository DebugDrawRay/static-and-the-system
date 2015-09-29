using UnityEngine;
using System.Collections;

public class recordGun : MonoBehaviour
{
    private player _player;
    public GameObject recordAnim;
    void Start()
    {
        _player = player.Instance;
    }
    void OnTriggerEnter2D(Collider2D hit)
    {
        if (hit.gameObject.GetComponent<objectData>())
        {
            if (_player.recordedObject == null && _player.recordableObject != null)
            {
                recordObject(hit.gameObject);
            }
            else if (_player.recordedObject != null)
            {
                replayObject(_player.recordedObject);
            }
        }
    }

    void recordObject(GameObject target)
    {
        recordAnimation(target);
        target.GetComponent<objectData>().labelActive(false);
        _player.recordedObject = target;
        _player.recordedObject.SetActive(false);
    }

    void replayObject(GameObject target)
    {
        foreach (Collider2D col in target.GetComponents<Collider2D>())
        {
            col.enabled = true;
        }
        target.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 1);
        target.SetActive(true);
        recordAnimation(target);
        _player.recordedObject = null;
    }

    void recordAnimation(GameObject target)
    {
        Vector2 origin = target.transform.position;
        Instantiate(recordAnim, origin, Quaternion.identity);
    }
}

using UnityEngine;
using System.Collections;

public class recorderObject : MonoBehaviour
{
    private player _player;
    private BoxCollider2D bCollider;
    
    void Awake()
    {
        bCollider = GetComponent<BoxCollider2D>();
    }

	void Start ()
    {
        _player = player.Instance;	
	}
	
    void OnTriggerStay2D(Collider2D hit)
    {
        objectData data = hit.gameObject.GetComponent<objectData>();
        if(data)
        {
            _player.recordableObject = hit.gameObject;
        }
    }

    void OnTriggerExit2D(Collider2D hit)
    {
        _player.recordableObject = null;
    }
}

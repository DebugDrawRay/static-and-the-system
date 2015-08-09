using UnityEngine;
using System.Collections;

public class cameraController : MonoBehaviour 
{
    private GameObject leader;

	void Start () 
    {
        leader = player.Instance.gameObject;
	}
	
	void Update () 
    {
        if (leader)
        {
            Vector3 targetPos = new Vector3(leader.transform.position.x, leader.transform.position.y, transform.position.z);
            transform.position = targetPos;
        }
	}
}

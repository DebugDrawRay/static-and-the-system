using UnityEngine;
using System.Collections;

public class enemy : MonoBehaviour 
{
    public string[] hostileTags;
    public LayerMask ground;
    public float moveSpeed;
    public float baseDamage;
    public Sprite[] sprites;
}

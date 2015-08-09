using UnityEngine;
using System.Collections;

public class objectData : MonoBehaviour 
{  
    public enum type
    {
        earth,
        plant,
        enemy
    }

    public type objectType;
    public string name;
    public string description;

    public GameObject label;
    private GameObject currentLabel;

   void Start()
    {
        currentLabel = Instantiate(label, transform.position, Quaternion.identity) as GameObject;
        currentLabel.GetComponent<dataLabel>().anchorObj = this.gameObject;
        currentLabel.transform.SetParent(labelCanvas.Instance.transform);
    }

    public void labelActive(bool active)
   {
       currentLabel.SetActive(active);
   }
}

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class dataLabel : MonoBehaviour 
{
    public Text name;
    public Text type;
    public Text desc;

    public GameObject anchorObj;

    private const float _positionOffset = 2.0f;

    void Start()
    {
        populateData(anchorObj);
        this.gameObject.SetActive(false);
    }

    void populateData(GameObject obj)
    {
        objectData data = anchorObj.GetComponent<objectData>();
        if (data)
        {
            name.text = data.name;
            type.text = data.objectType.ToString();
            desc.text = data.description;

        }
    }
    void Update()
    {
        anchorTo(anchorObj);
    }

    void anchorTo(GameObject obj)
    {
        Vector3 offset = new Vector3(0, _positionOffset, 0);
        Vector3 anchorPos = obj.transform.position + offset;
        transform.position = anchorPos;
    }
}

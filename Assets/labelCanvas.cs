using UnityEngine;
using System.Collections;

public class labelCanvas : MonoBehaviour 
{

    void Awake()
    {
        initializeInstance();
    }
    
    public static labelCanvas Instance
    {
        get;
        private set;
    }

    void initializeInstance()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
        GameObject.DontDestroyOnLoad(this.gameObject);
    }

}
